use flate2::read::GzDecoder;
use reqwest::{Client, StatusCode, Url};
use serde::Deserialize;
use serde_json::{json, Map, Value};
use sha2::{Digest, Sha256};
use std::fs;
use std::io::Read;
use std::path::{Path, PathBuf};
use std::sync::OnceLock;
use std::time::Duration;
use tauri::{AppHandle, Manager};
use tokio::sync::{Mutex, Semaphore};
use uuid::Uuid;

const DEFAULT_GAME_DATA_API_URL: &str = "https://game-assets.api.exaltaccountmanager.com";
const MANIFEST_LIMIT: usize = 64 * 1024 * 1024;
const SPRITE_LIMIT: usize = 1024 * 1024;
const BUNDLE_LIMIT: usize = 128 * 1024 * 1024;
const PNG_SIGNATURE: &[u8; 8] = b"\x89PNG\r\n\x1a\n";
/// Records which build's sprite bundle has already been unpacked.
const BUNDLE_MARKER_FILE: &str = "sprite-bundle.build";
/// Snapshot shipped with the installer, used to seed an empty cache.
const SNAPSHOT_MANIFEST_FILE: &str = "manifest.json.gz";
const SNAPSHOT_SPRITES_FILE: &str = "sprites.tar.gz";
const MAX_FETCH_ATTEMPTS: u32 = 3;
const DEFAULT_RETRY_DELAY: Duration = Duration::from_secs(1);
const MAX_RETRY_DELAY: Duration = Duration::from_secs(5);

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
    let mut cached_manifest = load_cached_manifest(&manifest_path).ok();
    if cached_manifest.is_none() {
        // A fresh install has nothing retained yet. Seeding before the service
        // is contacted means the refresh below can diff against the snapshot,
        // and an unreachable service still leaves the application usable.
        match seed_cache_from_snapshot(&app, &cache_directory) {
            Ok(manifest) => cached_manifest = Some(manifest),
            Err(error) => eprintln!("[assets] no bundled snapshot to seed the cache: {error}"),
        }
    }

    match refresh_from_service(&manifest_path, cached_manifest.as_ref(), force).await {
        Ok(manifest) => {
            // Populating the sprite cache in one request keeps a cold start from
            // issuing thousands of individual requests, which the service rate
            // limits. A failure here is not fatal: sprites are still fetched
            // individually on demand.
            if let Some(build_id) = manifest.get("buildId").and_then(Value::as_str) {
                match prefetch_sprite_bundle(&cache_directory, build_id).await {
                    Ok(0) => {}
                    Ok(count) => println!("[assets] unpacked {count} sprites from the bundle"),
                    Err(error) => {
                        eprintln!("[assets] sprite bundle unavailable, falling back to individual sprite requests: {error}");
                    }
                }
            }
            Ok(with_cache_status(manifest, "service", None))
        }
        Err(error) => match cached_manifest {
            Some(manifest) => Ok(with_cache_status(manifest, "cache", Some(error))),
            None => Err(format!(
                "Unable to load game data from the service and no local cache is available. {error}"
            )),
        },
    }
}

/// Ensures a final 40x40 PNG is present in the disk cache and returns its path.
///
/// Sprites are fetched once and verified against their content-addressed hash
/// before being written. The webview then loads the file directly through
/// Tauri's asset protocol, so the bytes never cross the IPC boundary and no
/// base64 copy is made. `tauri.conf.json` restricts that protocol to exactly
/// this directory.
pub async fn get_asset_sprite_path(
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

    if !is_cached_sprite_valid(&sprite_path, &normalized_hash) {
        let _download_permit = SPRITE_DOWNLOADS
            .acquire()
            .await
            .map_err(|error| format!("Could not queue the sprite download: {error}"))?;

        // Another task may have completed the download while this one waited.
        if !is_cached_sprite_valid(&sprite_path, &normalized_hash) {
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
        }
    }

    sprite_path
        .to_str()
        .map(str::to_owned)
        .ok_or_else(|| "The sprite cache path is not valid UTF-8.".to_string())
}

/// Seeds an empty cache from the snapshot shipped with the application.
///
/// Without this, a fresh install whose first contact with the service fails has
/// nothing to fall back on and renders every item as a placeholder. An existing
/// install is unaffected, because it already retains its last-good manifest.
///
/// The snapshot is treated as a cache that arrives with the installer: it is
/// written into the cache directory and then superseded by the service in the
/// ordinary way, including by a diff against it. Its sprites are verified
/// against their own content hashes exactly as downloaded ones are. The manifest
/// has no published hash to check against here, so only its shape is validated;
/// it ships inside the signed installer, so it is trusted to the same degree as
/// the executable reading it.
fn seed_cache_from_snapshot(app: &AppHandle, cache_directory: &Path) -> Result<Value, String> {
    let resources = app
        .path()
        .resource_dir()
        .map_err(|error| format!("Could not resolve the resource directory: {error}"))?
        .join("game-data");

    // The manifest is stored compressed: as JSON it is mostly repeated keys, so
    // it deflates to roughly a sixth of its size and that saving goes straight
    // into the installer.
    let compressed = fs::read(resources.join(SNAPSHOT_MANIFEST_FILE))
        .map_err(|error| format!("No bundled game-data snapshot is available: {error}"))?;
    let mut manifest_bytes = Vec::new();
    GzDecoder::new(compressed.as_slice())
        .read_to_end(&mut manifest_bytes)
        .map_err(|error| {
            format!("The bundled game-data manifest could not be decompressed: {error}")
        })?;
    let manifest: Value = serde_json::from_slice(&manifest_bytes)
        .map_err(|error| format!("The bundled game-data manifest is invalid: {error}"))?;
    validate_manifest_shape(&manifest)?;

    write_manifest_atomically(&cache_directory.join("manifest.json"), &manifest)?;

    // Sprites are best-effort. The manifest alone already restores names, tiers
    // and stats, and anything missing falls back to the placeholder sprite.
    match seed_sprites_from_snapshot(&resources, cache_directory) {
        Ok(count) => println!("[assets] seeded {count} sprites from the bundled snapshot"),
        Err(error) => eprintln!("[assets] bundled sprite snapshot unusable: {error}"),
    }

    Ok(manifest)
}

fn seed_sprites_from_snapshot(resources: &Path, cache_directory: &Path) -> Result<usize, String> {
    let archive = fs::read(resources.join(SNAPSHOT_SPRITES_FILE))
        .map_err(|error| format!("No bundled sprite archive is available: {error}"))?;

    let mut tar_bytes = Vec::new();
    GzDecoder::new(archive.as_slice())
        .read_to_end(&mut tar_bytes)
        .map_err(|error| {
            format!("The bundled sprite archive could not be decompressed: {error}")
        })?;

    let sprite_directory = cache_directory.join("sprites");
    fs::create_dir_all(&sprite_directory)
        .map_err(|error| format!("Could not create the EAM sprite cache: {error}"))?;
    unpack_sprite_bundle(&sprite_directory, &tar_bytes)
}

/// Populates the sprite cache from the service's bundle route.
///
/// A marker file records the build whose sprites are already unpacked, so an
/// unchanged build costs nothing and a changed one asks only for the sprites it
/// added. Returns the number of sprites written.
async fn prefetch_sprite_bundle(
    cache_directory: &Path,
    build_id: &str,
) -> Result<usize, String> {
    if !is_hex(build_id, 64) {
        return Err("The manifest build ID has an invalid format.".to_string());
    }

    let marker_path = cache_directory.join(BUNDLE_MARKER_FILE);
    let unpacked_build = fs::read_to_string(&marker_path)
        .ok()
        .map(|value| value.trim().to_ascii_lowercase())
        .filter(|value| is_hex(value, 64));
    if unpacked_build.as_deref() == Some(build_id) {
        return Ok(0);
    }

    let sprite_directory = cache_directory.join("sprites");
    fs::create_dir_all(&sprite_directory)
        .map_err(|error| format!("Could not create the EAM sprite cache: {error}"))?;

    let client = http_client()?;
    let base_url = game_data_api_base_url()?;

    let mut archive = None;
    if let Some(from_build) = unpacked_build.as_deref() {
        let incremental = endpoint(
            &base_url,
            &format!("api/v1/builds/{build_id}/sprites?from={from_build}"),
        );
        // A 404 means the service no longer retains that build, so the complete
        // bundle is requested instead.
        archive = fetch_optional_bytes(&client, incremental, BUNDLE_LIMIT).await?;
    }
    let archive = match archive {
        Some(bytes) => bytes,
        None => {
            let complete = endpoint(&base_url, &format!("api/v1/builds/{build_id}/sprites"));
            fetch_bytes(&client, complete, BUNDLE_LIMIT).await?
        }
    };

    let target_directory = sprite_directory.clone();
    let written = tokio::task::spawn_blocking(move || {
        unpack_sprite_bundle(&target_directory, &archive)
    })
    .await
    .map_err(|error| format!("The sprite bundle could not be unpacked: {error}"))??;

    fs::write(&marker_path, build_id)
        .map_err(|error| format!("Could not record the unpacked sprite bundle: {error}"))?;
    Ok(written)
}

/// Writes every valid entry of a sprite bundle into the sprite cache.
///
/// Entry names are never used to build a path. The hash is taken from the file
/// name, validated, and the destination is composed from it, so a crafted entry
/// name cannot write outside the sprite directory. Each entry is verified
/// against its content hash exactly as an individually fetched sprite is, and
/// anything that fails is skipped rather than aborting the bundle.
fn unpack_sprite_bundle(sprite_directory: &Path, archive: &[u8]) -> Result<usize, String> {
    let mut reader = tar::Archive::new(archive);
    let entries = reader
        .entries()
        .map_err(|error| format!("The sprite bundle is not a readable archive: {error}"))?;

    let mut written = 0usize;
    let mut skipped = 0usize;
    for entry in entries {
        let mut entry = entry
            .map_err(|error| format!("The sprite bundle contains an unreadable entry: {error}"))?;

        let file_name = entry
            .path()
            .ok()
            .and_then(|path| path.file_name().map(|name| name.to_string_lossy().into_owned()))
            .unwrap_or_default();
        let Some(hash) = file_name
            .strip_suffix(".png")
            .map(str::to_ascii_lowercase)
            .filter(|hash| is_hex(hash, 64))
        else {
            skipped += 1;
            continue;
        };

        let size = entry.header().size().unwrap_or(u64::MAX);
        if size > SPRITE_LIMIT as u64 {
            skipped += 1;
            continue;
        }

        let mut bytes = Vec::with_capacity(size as usize);
        entry
            .read_to_end(&mut bytes)
            .map_err(|error| format!("Could not read a sprite from the bundle: {error}"))?;
        if verify_sprite(&bytes, &hash).is_err() {
            skipped += 1;
            continue;
        }

        let sprite_path = sprite_directory.join(format!("{hash}.png"));
        if sprite_path.exists() {
            continue;
        }
        write_new_file(&sprite_path, &bytes)?;
        written += 1;
    }

    if skipped > 0 {
        eprintln!("[assets] skipped {skipped} invalid entries in the sprite bundle");
    }
    Ok(written)
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
    verify_object_catalog(&updated, &diff)?;
    Ok(updated)
}

/// Checks a diff-assembled object map against the target catalog the diff
/// declares.
///
/// A downloaded manifest is verified against `manifestSha256`, but one assembled
/// locally cannot be: reproducing the service's exact serialized bytes is not
/// something a different language can be relied on to do. The catalog hash is
/// defined over the object ids and their metadata hashes instead, which is
/// reproducible, so an incomplete or inconsistent diff is caught here. Failing
/// makes the caller fall back to the full manifest, which is byte-verified.
fn verify_object_catalog(manifest: &Value, diff: &Value) -> Result<(), String> {
    let expected_count = diff
        .get("objectCount")
        .and_then(Value::as_u64)
        .ok_or_else(|| "The game-data diff does not declare an object count.".to_string())?;
    let expected_catalog = diff
        .get("objectsCatalogHash")
        .and_then(Value::as_str)
        .ok_or_else(|| "The game-data diff does not declare an object catalog hash.".to_string())?;

    let objects = manifest
        .get("objects")
        .and_then(Value::as_object)
        .ok_or_else(|| "The assembled object map is invalid.".to_string())?;
    if objects.len() as u64 != expected_count {
        return Err(format!(
            "The diff produced {} objects but declared {expected_count}.",
            objects.len()
        ));
    }

    let actual_catalog = hash_object_catalog(objects);
    if actual_catalog != expected_catalog {
        return Err("The diff-assembled object catalog does not match the diff.".to_string());
    }
    Ok(())
}

/// Hashes an object map as `id:metadataHash` lines joined by newlines.
///
/// Keys are sorted byte-wise, which matches the ordinal ordering the service
/// uses. The sort is explicit rather than relying on the map's iteration order,
/// so enabling serde_json's `preserve_order` feature could not silently change
/// the result.
fn hash_object_catalog(objects: &Map<String, Value>) -> String {
    let mut keys: Vec<&String> = objects.keys().collect();
    keys.sort();

    let mut catalog = String::new();
    for (index, key) in keys.iter().enumerate() {
        if index > 0 {
            catalog.push('\n');
        }
        let metadata_hash = objects
            .get(*key)
            .and_then(|object| object.get("metadataHash"))
            .and_then(Value::as_str)
            .unwrap_or_default();
        catalog.push_str(key);
        catalog.push(':');
        catalog.push_str(metadata_hash);
    }
    sha256_hex(catalog.as_bytes())
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

    validate_manifest_shape(manifest)
}

/// Checks a manifest is internally usable, without comparing it to a build the
/// service advertised. The bundled snapshot has no such build to compare
/// against, since it is read before the service is contacted.
fn validate_manifest_shape(manifest: &Value) -> Result<(), String> {
    if !manifest
        .get("buildId")
        .and_then(Value::as_str)
        .is_some_and(|id| is_hex(id, 64))
    {
        return Err("The game-data manifest has no valid build ID.".to_string());
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

/// Verifies a cached sprite on disk. The bytes are read only to check them; the
/// webview loads the file itself, so they are not returned.
fn is_cached_sprite_valid(path: &Path, expected_hash: &str) -> bool {
    match fs::read(path) {
        Ok(bytes) => verify_sprite(&bytes, expected_hash).is_ok(),
        Err(_) => false,
    }
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

/// How long to wait before retrying a throttled or temporarily failed request.
///
/// Only the delta-seconds form of `Retry-After` is parsed. The HTTP-date form is
/// legal but rare, and falling back to the default simply costs one more
/// rejected request before giving up.
fn retry_delay(response: &reqwest::Response) -> Duration {
    response
        .headers()
        .get(reqwest::header::RETRY_AFTER)
        .and_then(|value| value.to_str().ok())
        .and_then(|value| value.trim().parse::<u64>().ok())
        .map(Duration::from_secs)
        .unwrap_or(DEFAULT_RETRY_DELAY)
        .min(MAX_RETRY_DELAY)
}

async fn fetch_optional_bytes(
    client: &Client,
    url: String,
    limit: usize,
) -> Result<Option<Vec<u8>>, String> {
    let mut attempt = 1;
    let response = loop {
        let response = client
            .get(&url)
            .send()
            .await
            .map_err(|error| format!("The game-data service request failed: {error}"))?;
        if response.status() == StatusCode::NOT_FOUND {
            return Ok(None);
        }

        // The service throttles per client, so a rejection is worth waiting out
        // rather than surfacing immediately. The wait is capped so a busy
        // service cannot stall startup for the full period it asked for.
        let is_transient = response.status() == StatusCode::TOO_MANY_REQUESTS
            || response.status() == StatusCode::SERVICE_UNAVAILABLE;
        if is_transient && attempt < MAX_FETCH_ATTEMPTS {
            let delay = retry_delay(&response);
            tokio::time::sleep(delay).await;
            attempt += 1;
            continue;
        }

        break response;
    };

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
    // Debug builds may retarget the service at runtime, so a locally running
    // instance can be used without recompiling. Release builds only honour the
    // value baked in at compile time.
    #[cfg(debug_assertions)]
    let runtime_override = std::env::var("EAM_GAME_DATA_API_URL").ok();
    #[cfg(not(debug_assertions))]
    let runtime_override: Option<String> = None;

    let compiled = option_env!("EAM_GAME_DATA_API_URL").unwrap_or(DEFAULT_GAME_DATA_API_URL);
    let configured = runtime_override.as_deref().unwrap_or(compiled).trim();
    if configured.is_empty() {
        return Err("The configured game-data service URL is empty.".to_string());
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

    fn tiny_png() -> Vec<u8> {
        // Not a decodable image, but it carries the PNG signature that
        // verify_sprite checks for.
        let mut bytes = PNG_SIGNATURE.to_vec();
        bytes.extend_from_slice(b"sprite-bundle-test");
        bytes
    }

    /// Writes the entry name straight into the ustar header rather than going
    /// through `append_data`, which refuses to build archives containing `..`.
    /// Forging those names is the point of the traversal test.
    fn tar_with(entries: &[(&str, &[u8])]) -> Vec<u8> {
        let mut builder = tar::Builder::new(Vec::new());
        for (name, bytes) in entries {
            let mut header = tar::Header::new_ustar();
            {
                let raw = name.as_bytes();
                let old = header.as_old_mut();
                old.name[..raw.len()].copy_from_slice(raw);
            }
            header.set_size(bytes.len() as u64);
            header.set_mode(0o644);
            header.set_cksum();
            builder.append(&header, *bytes).unwrap();
        }
        builder.into_inner().unwrap()
    }

    fn temp_directory() -> PathBuf {
        let path = std::env::temp_dir().join(format!("eam-bundle-test-{}", Uuid::new_v4()));
        fs::create_dir_all(&path).unwrap();
        path
    }

    #[test]
    fn unpacks_only_entries_matching_their_content_hash() {
        let directory = temp_directory();
        let png = tiny_png();
        let hash = sha256_hex(&png);
        let archive = tar_with(&[
            (format!("{hash}.png").as_str(), png.as_slice()),
            // Correctly named, but the bytes hash to something else.
            (format!("{}.png", "c".repeat(64)).as_str(), png.as_slice()),
            ("not-a-hash.png", png.as_slice()),
            ("readme.txt", b"ignored".as_slice()),
        ]);

        let written = unpack_sprite_bundle(&directory, &archive).unwrap();

        assert_eq!(written, 1);
        assert!(directory.join(format!("{hash}.png")).exists());
        assert!(!directory.join(format!("{}.png", "c".repeat(64))).exists());
        assert_eq!(fs::read(directory.join(format!("{hash}.png"))).unwrap(), png);
        fs::remove_dir_all(&directory).ok();
    }

    #[test]
    fn traversal_entry_names_cannot_escape_the_sprite_directory() {
        let directory = temp_directory();
        let escape_target = directory.join("escaped.png");
        let png = tiny_png();
        let hash = sha256_hex(&png);
        let archive = tar_with(&[
            (format!("../../{hash}.png").as_str(), png.as_slice()),
            ("../../escaped.png", png.as_slice()),
        ]);

        let nested = directory.join("sprites");
        fs::create_dir_all(&nested).unwrap();
        unpack_sprite_bundle(&nested, &archive).unwrap();

        // Nothing may be written outside the sprite directory, whichever way the
        // traversal entries are handled.
        assert!(!escape_target.exists());
        assert_eq!(
            fs::read_dir(&directory)
                .unwrap()
                .filter_map(Result::ok)
                .map(|entry| entry.file_name().to_string_lossy().into_owned())
                .collect::<Vec<_>>(),
            vec!["sprites".to_string()],
        );
        // The hashed entry, if kept at all, lands under its own hash.
        for entry in fs::read_dir(&nested).unwrap().filter_map(Result::ok) {
            assert_eq!(entry.file_name().to_string_lossy(), format!("{hash}.png"));
        }
        fs::remove_dir_all(&directory).ok();
    }

    #[test]
    fn an_empty_bundle_unpacks_to_nothing() {
        let directory = temp_directory();
        let written = unpack_sprite_bundle(&directory, &vec![0u8; 1024]).unwrap();
        assert_eq!(written, 0);
        fs::remove_dir_all(&directory).ok();
    }

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
            "fameBonuses": [],
            // Only object 2 survives, and it carries no metadataHash, so the
            // catalog is the single line "2:".
            "objectCount": 1,
            "objectsCatalogHash": sha256_hex(b"2:")
        });

        let updated = apply_diff(&cached, &serde_json::to_vec(&diff).unwrap(), &latest).unwrap();
        assert!(updated["objects"].get("1").is_none());
        assert_eq!(updated["objects"]["2"]["id"], 2);
        assert_eq!(updated["buildId"], latest.build_id);
        assert_eq!(updated["playerStatsHash"], "5".repeat(64));
    }

    #[test]
    fn seeds_sprites_from_a_compressed_snapshot() {
        use std::io::Write;

        let directory = temp_directory();
        let resources = directory.join("resources");
        fs::create_dir_all(&resources).unwrap();

        let png = tiny_png();
        let hash = sha256_hex(&png);
        let archive = tar_with(&[(format!("{hash}.png").as_str(), png.as_slice())]);
        let mut encoder =
            flate2::write::GzEncoder::new(Vec::new(), flate2::Compression::default());
        encoder.write_all(&archive).unwrap();
        fs::write(
            resources.join(SNAPSHOT_SPRITES_FILE),
            encoder.finish().unwrap(),
        )
        .unwrap();

        let written = seed_sprites_from_snapshot(&resources, &directory).unwrap();

        assert_eq!(written, 1);
        assert!(directory
            .join("sprites")
            .join(format!("{hash}.png"))
            .exists());
        fs::remove_dir_all(&directory).ok();
    }

    #[test]
    fn a_missing_snapshot_reports_an_error() {
        let directory = temp_directory();
        assert!(seed_sprites_from_snapshot(&directory.join("absent"), &directory).is_err());
        fs::remove_dir_all(&directory).ok();
    }

    #[test]
    fn manifest_shape_validation_rejects_unusable_manifests() {
        let valid = json!({
            "buildId": "a".repeat(64),
            "objects": { "1": { "spriteHash": "b".repeat(64) } },
            "playerStats": {},
            "fameBonuses": [],
            "playerStatsHash": "c".repeat(64),
            "fameBonusesHash": "d".repeat(64),
        });
        assert!(validate_manifest_shape(&valid).is_ok());

        // Without a build ID the snapshot cannot be diffed against later.
        let mut no_build = valid.clone();
        no_build["buildId"] = json!("not-hex");
        assert!(validate_manifest_shape(&no_build).is_err());

        let mut empty_objects = valid.clone();
        empty_objects["objects"] = json!({});
        assert!(validate_manifest_shape(&empty_objects).is_err());

        let mut bad_sprite = valid.clone();
        bad_sprite["objects"]["1"]["spriteHash"] = json!("short");
        assert!(validate_manifest_shape(&bad_sprite).is_err());
    }

    #[test]
    fn hash_object_catalog_sorts_keys_byte_wise() {
        // "10" precedes "9" byte-wise, matching the service's ordinal ordering,
        // and the result must not depend on insertion order.
        let ascending = json!({
            "10": { "metadataHash": "hash-10" },
            "9": { "metadataHash": "hash-9" },
        });
        let descending = json!({
            "9": { "metadataHash": "hash-9" },
            "10": { "metadataHash": "hash-10" },
        });

        let expected = sha256_hex(b"10:hash-10\n9:hash-9");
        assert_eq!(hash_object_catalog(ascending.as_object().unwrap()), expected);
        assert_eq!(hash_object_catalog(descending.as_object().unwrap()), expected);
    }

    #[test]
    fn rejects_a_diff_whose_catalog_does_not_match() {
        let manifest = json!({ "objects": { "2": { "metadataHash": "abc" } } });

        // Correct count, wrong catalog hash.
        let wrong_hash = json!({ "objectCount": 1, "objectsCatalogHash": "f".repeat(64) });
        assert!(verify_object_catalog(&manifest, &wrong_hash).is_err());

        // Correct catalog hash, wrong count.
        let wrong_count = json!({
            "objectCount": 7,
            "objectsCatalogHash": sha256_hex(b"2:abc"),
        });
        assert!(verify_object_catalog(&manifest, &wrong_count).is_err());

        // A diff that predates the field cannot be verified, so it is refused
        // and the caller falls back to the byte-verified manifest.
        let missing = json!({ "objectCount": 1 });
        assert!(verify_object_catalog(&manifest, &missing).is_err());

        let correct = json!({
            "objectCount": 1,
            "objectsCatalogHash": sha256_hex(b"2:abc"),
        });
        assert!(verify_object_catalog(&manifest, &correct).is_ok());
    }
}
