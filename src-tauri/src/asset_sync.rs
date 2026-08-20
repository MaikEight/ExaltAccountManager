use base64::{engine::general_purpose, Engine as _};
use reqwest::{Client, StatusCode, Url};
use serde::Deserialize;
use serde_json::{json, Map, Value};
use sha2::{Digest, Sha256};
use std::fs;
use std::path::{Path, PathBuf};
use std::sync::OnceLock;
use std::time::Duration;
use tauri::{AppHandle, Manager};
use tokio::sync::{Mutex, Semaphore};
use uuid::Uuid;

const MANIFEST_LIMIT: usize = 64 * 1024 * 1024;
const SPRITE_LIMIT: usize = 1024 * 1024;
const PNG_SIGNATURE: &[u8; 8] = b"\x89PNG\r\n\x1a\n";

static REFRESH_LOCK: Mutex<()> = Mutex::const_new(());
static SPRITE_DOWNLOADS: Semaphore = Semaphore::const_new(16);
static HTTP_CLIENT: OnceLock<Client> = OnceLock::new();

#[derive(Debug, Deserialize)]
#[serde(rename_all = "camelCase")]
struct LatestBuild {
    schema_version: u64,
    build_id: String,
    realm_build_hash: String,
    source_checksum: String,
    generated_at: String,
    manifest_sha256: String,
}

/// Refreshes EAM's last-good game-data manifest from the shared service.
/// A retained local manifest is returned whenever the service is unavailable.
pub async fn refresh_asset_cache_from_api(app: AppHandle, force: bool) -> Result<Value, String> {
    let _refresh_guard = REFRESH_LOCK.lock().await;
    let cache_directory = cache_directory(&app)?;
    fs::create_dir_all(&cache_directory)
        .map_err(|error| format!("Could not create the EAM game-data cache: {error}"))?;

    let manifest_path = cache_directory.join("manifest.json");
    let cached_manifest = load_cached_manifest(&manifest_path).ok();

    match refresh_from_service(&manifest_path, cached_manifest.as_ref(), force).await {
        Ok(manifest) => Ok(with_cache_status(manifest, "service", None)),
        Err(error) => match cached_manifest {
            Some(manifest) => Ok(with_cache_status(manifest, "cache", Some(error))),
            None => Err(format!(
                "Unable to load game data from the service and no local cache is available. {error}"
            )),
        },
    }
}

/// Returns a final 40x40 PNG as a data URL. Sprites are fetched once, verified
/// against their content-addressed hash, and then served from the disk cache.
pub async fn get_asset_sprite_data_url(
    app: AppHandle,
    sprite_hash: String,
) -> Result<String, String> {
    let normalized_hash = sprite_hash.to_ascii_lowercase();
    if !is_hex(&normalized_hash, 64) {
        return Err("The sprite hash has an invalid format.".to_string());
    }

    let sprite_directory = cache_directory(&app)?.join("sprites");
    fs::create_dir_all(&sprite_directory)
        .map_err(|error| format!("Could not create the EAM sprite cache: {error}"))?;
    let sprite_path = sprite_directory.join(format!("{normalized_hash}.png"));

    let bytes = match read_verified_sprite(&sprite_path, &normalized_hash) {
        Ok(bytes) => bytes,
        Err(_) => {
            let _download_permit = SPRITE_DOWNLOADS
                .acquire()
                .await
                .map_err(|error| format!("Could not queue the sprite download: {error}"))?;

            if let Ok(bytes) = read_verified_sprite(&sprite_path, &normalized_hash) {
                bytes
            } else {
                if sprite_path.exists() {
                    fs::remove_file(&sprite_path).map_err(|error| {
                        format!("Could not replace an invalid cached sprite: {error}")
                    })?;
                }
                let client = http_client()?;
                let base_url = game_data_api_base_url()?;
                let url = endpoint(&base_url, &format!("api/v1/sprites/{normalized_hash}.png"));
                let downloaded = fetch_bytes(&client, url, SPRITE_LIMIT).await?;
                verify_sprite(&downloaded, &normalized_hash)?;
                write_new_file(&sprite_path, &downloaded)?;
                downloaded
            }
        }
    };

    Ok(format!(
        "data:image/png;base64,{}",
        general_purpose::STANDARD.encode(bytes)
    ))
}

async fn refresh_from_service(
    manifest_path: &Path,
    cached_manifest: Option<&Value>,
    force: bool,
) -> Result<Value, String> {
    let client = http_client()?;
    let base_url = game_data_api_base_url()?;
    let latest_url = endpoint(&base_url, "api/v1/builds/latest");
    let latest_bytes = fetch_bytes(&client, latest_url, 1024 * 1024).await?;
    let latest: LatestBuild = serde_json::from_slice(&latest_bytes).map_err(|error| {
        format!("The game-data service returned invalid latest-build data: {error}")
    })?;
    validate_latest(&latest)?;

    if !force {
        if let Some(cached) = cached_manifest {
            if cached.get("buildId").and_then(Value::as_str) == Some(&latest.build_id) {
                validate_manifest(cached, &latest)?;
                return Ok(cached.clone());
            }
        }
    }

    if let Some(cached) = cached_manifest {
        if let Some(from_build_id) = cached.get("buildId").and_then(Value::as_str) {
            if is_hex(from_build_id, 64) && from_build_id != latest.build_id {
                let diff_url = endpoint(
                    &base_url,
                    &format!(
                        "api/v1/builds/{}/diff?from={}",
                        latest.build_id, from_build_id
                    ),
                );
                if let Ok(Some(diff_bytes)) =
                    fetch_optional_bytes(&client, diff_url, MANIFEST_LIMIT).await
                {
                    if let Ok(updated) = apply_diff(cached, &diff_bytes, &latest) {
                        write_manifest_atomically(manifest_path, &updated)?;
                        return Ok(updated);
                    }
                }
            }
        }
    }

    let manifest_url = endpoint(
        &base_url,
        &format!("api/v1/builds/{}/manifest", latest.build_id),
    );
    let manifest_bytes = fetch_bytes(&client, manifest_url, MANIFEST_LIMIT).await?;
    let actual_hash = sha256_hex(&manifest_bytes);
    if actual_hash != latest.manifest_sha256 {
        return Err("The downloaded game-data manifest failed SHA-256 verification.".to_string());
    }

    let manifest: Value = serde_json::from_slice(&manifest_bytes)
        .map_err(|error| format!("The downloaded game-data manifest is invalid: {error}"))?;
    validate_manifest(&manifest, &latest)?;
    write_manifest_atomically(manifest_path, &manifest)?;
    Ok(manifest)
}

fn apply_diff(cached: &Value, diff_bytes: &[u8], latest: &LatestBuild) -> Result<Value, String> {
    let diff: Value = serde_json::from_slice(diff_bytes)
        .map_err(|error| format!("The game-data diff is invalid: {error}"))?;
    let from_build_id = cached
        .get("buildId")
        .and_then(Value::as_str)
        .ok_or_else(|| "The cached manifest has no build ID.".to_string())?;
    if diff.get("fromBuildId").and_then(Value::as_str) != Some(from_build_id)
        || diff.get("toBuildId").and_then(Value::as_str) != Some(&latest.build_id)
    {
        return Err("The game-data diff does not match the requested builds.".to_string());
    }

    let mut updated = cached.clone();
    let objects = updated
        .get_mut("objects")
        .and_then(Value::as_object_mut)
        .ok_or_else(|| "The cached object map is invalid.".to_string())?;

    merge_object_section(objects, diff.get("addedObjects"), "addedObjects")?;
    merge_object_section(objects, diff.get("modifiedObjects"), "modifiedObjects")?;
    for removed_id in diff
        .get("removedObjectIds")
        .and_then(Value::as_array)
        .ok_or_else(|| "The diff's removed-object list is invalid.".to_string())?
    {
        let id = removed_id
            .as_i64()
            .ok_or_else(|| "A removed object ID is invalid.".to_string())?;
        objects.remove(&id.to_string());
    }

    replace_section_when_present(&mut updated, &diff, "playerStats")?;
    replace_section_when_present(&mut updated, &diff, "fameBonuses")?;
    copy_required_diff_field(&mut updated, &diff, "schemaVersion")?;
    updated["buildId"] = diff
        .get("toBuildId")
        .cloned()
        .ok_or_else(|| "The diff has no target build ID.".to_string())?;
    for field in [
        "realmBuildHash",
        "sourceChecksum",
        "generatedAt",
        "playerStatsHash",
        "fameBonusesHash",
    ] {
        copy_required_diff_field(&mut updated, &diff, field)?;
    }

    validate_manifest(&updated, latest)?;
    Ok(updated)
}

fn merge_object_section(
    target: &mut Map<String, Value>,
    source: Option<&Value>,
    name: &str,
) -> Result<(), String> {
    let source = source
        .and_then(Value::as_object)
        .ok_or_else(|| format!("The diff's {name} section is invalid."))?;
    for (id, value) in source {
        target.insert(id.clone(), value.clone());
    }
    Ok(())
}

fn replace_section_when_present(
    target: &mut Value,
    diff: &Value,
    field: &str,
) -> Result<(), String> {
    match diff.get(field) {
        Some(Value::Null) => Ok(()),
        Some(value) => {
            target[field] = value.clone();
            Ok(())
        }
        None => Err(format!("The diff has no {field} field.")),
    }
}

fn copy_required_diff_field(target: &mut Value, diff: &Value, field: &str) -> Result<(), String> {
    let value = diff
        .get(field)
        .cloned()
        .ok_or_else(|| format!("The diff has no {field} field."))?;
    target[field] = value;
    Ok(())
}

fn validate_latest(latest: &LatestBuild) -> Result<(), String> {
    if latest.schema_version == 0
        || !is_hex(&latest.build_id, 64)
        || !is_hex(&latest.realm_build_hash, 32)
        || !is_hex(&latest.source_checksum, 32)
        || !is_hex(&latest.manifest_sha256, 64)
        || latest.generated_at.trim().is_empty()
    {
        return Err("The latest game-data build metadata is incomplete.".to_string());
    }
    Ok(())
}

fn validate_manifest(manifest: &Value, latest: &LatestBuild) -> Result<(), String> {
    if manifest.get("schemaVersion").and_then(Value::as_u64) != Some(latest.schema_version)
        || manifest.get("buildId").and_then(Value::as_str) != Some(&latest.build_id)
        || manifest.get("realmBuildHash").and_then(Value::as_str) != Some(&latest.realm_build_hash)
        || manifest.get("sourceChecksum").and_then(Value::as_str) != Some(&latest.source_checksum)
    {
        return Err("The game-data manifest does not match the latest build metadata.".to_string());
    }

    let objects = manifest
        .get("objects")
        .and_then(Value::as_object)
        .ok_or_else(|| "The game-data object map is missing.".to_string())?;
    if objects.is_empty() {
        return Err("The game-data object map is empty.".to_string());
    }
    for object in objects.values() {
        let sprite_hash = object.get("spriteHash").and_then(Value::as_str);
        if !sprite_hash.is_some_and(|hash| is_hex(hash, 64)) {
            return Err("A game-data object contains an invalid sprite hash.".to_string());
        }
    }
    if !manifest.get("playerStats").is_some_and(Value::is_object)
        || !manifest.get("fameBonuses").is_some_and(Value::is_array)
        || !manifest
            .get("playerStatsHash")
            .and_then(Value::as_str)
            .is_some_and(|hash| is_hex(hash, 64))
        || !manifest
            .get("fameBonusesHash")
            .and_then(Value::as_str)
            .is_some_and(|hash| is_hex(hash, 64))
    {
        return Err("The game-data stats or fame-bonus sections are invalid.".to_string());
    }
    Ok(())
}

fn cache_directory(app: &AppHandle) -> Result<PathBuf, String> {
    app.path()
        .app_data_dir()
        .map(|path| path.join("game-data"))
        .map_err(|error| format!("Could not resolve the EAM app-data directory: {error}"))
}

fn load_cached_manifest(path: &Path) -> Result<Value, String> {
    let text = fs::read_to_string(path)
        .map_err(|error| format!("Could not read the cached game-data manifest: {error}"))?;
    serde_json::from_str(&text)
        .map_err(|error| format!("The cached game-data manifest is invalid: {error}"))
}

fn write_manifest_atomically(path: &Path, manifest: &Value) -> Result<(), String> {
    let bytes = serde_json::to_vec(manifest)
        .map_err(|error| format!("Could not serialize the game-data manifest: {error}"))?;
    let temporary = path.with_extension(format!("tmp-{}", Uuid::new_v4()));
    fs::write(&temporary, bytes)
        .map_err(|error| format!("Could not stage the game-data manifest: {error}"))?;

    let backup = path.with_extension("previous");
    let _ = fs::remove_file(&backup);
    if path.exists() {
        fs::rename(path, &backup).map_err(|error| {
            format!("Could not rotate the previous game-data manifest: {error}")
        })?;
    }
    if let Err(error) = fs::rename(&temporary, path) {
        if backup.exists() {
            let _ = fs::rename(&backup, path);
        }
        let _ = fs::remove_file(&temporary);
        return Err(format!(
            "Could not activate the game-data manifest: {error}"
        ));
    }
    let _ = fs::remove_file(backup);
    Ok(())
}

fn read_verified_sprite(path: &Path, expected_hash: &str) -> Result<Vec<u8>, String> {
    let bytes =
        fs::read(path).map_err(|error| format!("Could not read the cached sprite: {error}"))?;
    verify_sprite(&bytes, expected_hash)?;
    Ok(bytes)
}

fn verify_sprite(bytes: &[u8], expected_hash: &str) -> Result<(), String> {
    if bytes.len() < PNG_SIGNATURE.len()
        || &bytes[..PNG_SIGNATURE.len()] != PNG_SIGNATURE
        || sha256_hex(bytes) != expected_hash
    {
        return Err("The sprite failed PNG or SHA-256 verification.".to_string());
    }
    Ok(())
}

fn write_new_file(path: &Path, bytes: &[u8]) -> Result<(), String> {
    let temporary = path.with_extension(format!("tmp-{}", Uuid::new_v4()));
    fs::write(&temporary, bytes)
        .map_err(|error| format!("Could not stage a downloaded sprite: {error}"))?;
    match fs::rename(&temporary, path) {
        Ok(()) => Ok(()),
        Err(_) if path.exists() => {
            let _ = fs::remove_file(&temporary);
            Ok(())
        }
        Err(error) => {
            let _ = fs::remove_file(&temporary);
            Err(format!("Could not cache a downloaded sprite: {error}"))
        }
    }
}

fn with_cache_status(mut manifest: Value, source: &str, warning: Option<String>) -> Value {
    manifest["clientCache"] = json!({
        "source": source,
        "warning": warning,
    });
    manifest
}

fn http_client() -> Result<Client, String> {
    if let Some(client) = HTTP_CLIENT.get() {
        return Ok(client.clone());
    }

    let client = Client::builder()
        .connect_timeout(Duration::from_secs(5))
        .timeout(Duration::from_secs(30))
        .user_agent(concat!("ExaltAccountManager/", env!("CARGO_PKG_VERSION")))
        .build()
        .map_err(|error| format!("Could not initialize the game-data HTTP client: {error}"))?;
    let _ = HTTP_CLIENT.set(client);
    HTTP_CLIENT
        .get()
        .cloned()
        .ok_or_else(|| "Could not retain the game-data HTTP client.".to_string())
}

async fn fetch_optional_bytes(
    client: &Client,
    url: String,
    limit: usize,
) -> Result<Option<Vec<u8>>, String> {
    let response = client
        .get(url)
        .send()
        .await
        .map_err(|error| format!("The game-data service request failed: {error}"))?;
    if response.status() == StatusCode::NOT_FOUND {
        return Ok(None);
    }
    let response = response
        .error_for_status()
        .map_err(|error| format!("The game-data service returned an error: {error}"))?;
    if response
        .content_length()
        .is_some_and(|length| length > limit as u64)
    {
        return Err("The game-data service response exceeded the size limit.".to_string());
    }
    let bytes = response
        .bytes()
        .await
        .map_err(|error| format!("Could not read the game-data service response: {error}"))?;
    if bytes.len() > limit {
        return Err("The game-data service response exceeded the size limit.".to_string());
    }
    Ok(Some(bytes.to_vec()))
}

async fn fetch_bytes(client: &Client, url: String, limit: usize) -> Result<Vec<u8>, String> {
    fetch_optional_bytes(client, url, limit)
        .await?
        .ok_or_else(|| "The requested game-data resource was not found.".to_string())
}

fn game_data_api_base_url() -> Result<Url, String> {
    #[cfg(debug_assertions)]
    let development_default = "http://192.168.1.2:8090";
    #[cfg(not(debug_assertions))]
    let development_default = "";

    let configured = option_env!("EAM_GAME_DATA_API_URL")
        .unwrap_or(development_default)
        .trim();
    if configured.is_empty() {
        return Err(
            "EAM_GAME_DATA_API_URL was not configured when this EAM build was created.".to_string(),
        );
    }

    let url = Url::parse(configured)
        .map_err(|error| format!("The configured game-data service URL is invalid: {error}"))?;
    if !url.username().is_empty()
        || url.password().is_some()
        || url.query().is_some()
        || url.fragment().is_some()
        || url.host_str().is_none()
    {
        return Err("The configured game-data service URL is not a valid base URL.".to_string());
    }
    if url.scheme() != "https" && !(cfg!(debug_assertions) && url.scheme() == "http") {
        return Err("The game-data service must use HTTPS outside development builds.".to_string());
    }
    Ok(url)
}

fn endpoint(base_url: &Url, path: &str) -> String {
    format!(
        "{}/{}",
        base_url.as_str().trim_end_matches('/'),
        path.trim_start_matches('/')
    )
}

fn sha256_hex(bytes: &[u8]) -> String {
    hex::encode(Sha256::digest(bytes))
}

fn is_hex(value: &str, length: usize) -> bool {
    value.len() == length && value.bytes().all(|byte| byte.is_ascii_hexdigit())
}

#[cfg(test)]
mod tests {
    use super::*;

    #[test]
    fn rejects_invalid_sprite_hashes() {
        assert!(is_hex(&"a".repeat(64), 64));
        assert!(!is_hex(&"g".repeat(64), 64));
        assert!(!is_hex(&"a".repeat(63), 64));
    }

    #[test]
    fn applies_a_complete_diff() {
        let latest = LatestBuild {
            schema_version: 1,
            build_id: "b".repeat(64),
            realm_build_hash: "c".repeat(32),
            source_checksum: "d".repeat(32),
            generated_at: "2026-08-20T00:00:00Z".to_string(),
            manifest_sha256: "e".repeat(64),
        };
        let cached = json!({
            "schemaVersion": 1,
            "buildId": "a".repeat(64),
            "realmBuildHash": "1".repeat(32),
            "sourceChecksum": "2".repeat(32),
            "generatedAt": "2026-08-19T00:00:00Z",
            "objects": {
                "1": { "id": 1, "spriteHash": "1".repeat(64) }
            },
            "playerStats": {},
            "fameBonuses": [],
            "playerStatsHash": "3".repeat(64),
            "fameBonusesHash": "4".repeat(64)
        });
        let diff = json!({
            "schemaVersion": 1,
            "fromBuildId": "a".repeat(64),
            "toBuildId": "b".repeat(64),
            "realmBuildHash": "c".repeat(32),
            "sourceChecksum": "d".repeat(32),
            "generatedAt": "2026-08-20T00:00:00Z",
            "playerStatsHash": "5".repeat(64),
            "fameBonusesHash": "6".repeat(64),
            "addedObjects": {
                "2": { "id": 2, "spriteHash": "2".repeat(64) }
            },
            "modifiedObjects": {},
            "removedObjectIds": [1],
            "playerStats": {},
            "fameBonuses": []
        });

        let updated = apply_diff(&cached, &serde_json::to_vec(&diff).unwrap(), &latest).unwrap();
        assert!(updated["objects"].get("1").is_none());
        assert_eq!(updated["objects"]["2"]["id"], 2);
        assert_eq!(updated["buildId"], latest.build_id);
        assert_eq!(updated["playerStatsHash"], "5".repeat(64));
    }
}
