use base64::{engine::general_purpose::STANDARD, Engine as _};
use std::error::Error;
use winreg::enums::*;
use winreg::{RegKey, RegValue};

use super::LauncherLogin;

/// The registry key (under `HKEY_CURRENT_USER`) where the official launcher
/// persists its Unity PlayerPrefs, including the signed-in account.
const LAUNCHER_REG_SUBKEY: &str = "Software\\DECA Live Operations GmbH\\RotMG Exalt Launcher";

/// The launcher obfuscates the PlayerPrefs key names of its credential fields by
/// base64-encoding `"{env_prefix}{suffix}"`. For example, `"Productionguid"`
/// becomes `"UHJvZHVjdGlvbmd1aWQ="` and `"Productionps"` becomes
/// `"UHJvZHVjdGlvbnBz"`. Mirrors the macOS implementation (see `macos.rs`).
fn obfuscated_key(env_prefix: &str, suffix: &str) -> String {
    STANDARD.encode(format!("{env_prefix}{suffix}").as_bytes())
}

/// Unity's PlayerPrefs hash for a key name (the `_h<hash>` suffix Unity appends
/// to every registry value name). It is a DJB2 variant over the key's UTF-8
/// bytes: `hash = 5381; hash = hash * 33 ^ byte`, kept as a wrapping `u32`.
/// The hash depends only on the key name, so it is identical for every user.
fn playerprefs_hash(name: &str) -> u32 {
    let mut hash: u32 = 5381;
    for byte in name.as_bytes() {
        hash = hash.wrapping_mul(33) ^ (*byte as u32);
    }
    hash
}

/// Builds the full registry value name Unity uses for a PlayerPrefs key:
/// `"{name}_h{hash}"`.
fn playerprefs_key(name: &str) -> String {
    format!("{name}_h{}", playerprefs_hash(name))
}

/// Unity stores PlayerPrefs strings in the registry as `REG_BINARY`: the UTF-8
/// bytes of the value followed by a single trailing NUL byte.
fn string_value(value: impl AsRef<str>) -> RegValue<'static> {
    let mut bytes = value.as_ref().as_bytes().to_vec();
    bytes.push(0);
    RegValue {
        bytes: bytes.into(),
        vtype: REG_BINARY,
    }
}

/// Writes the selected account's login into the official launcher's registry
/// key so the launcher comes up already signed in.
///
/// `env_prefix` is the environment the credential keys are scoped to
/// (`Production`); it obfuscates the `guid`/`ps` key names exactly like macOS.
/// `domain` is the macOS CFPreferences/bundle identifier and is meaningless on
/// Windows (the storage location is the registry key above), so it is unused
/// here.
///
/// Value layout mirrors `macos.rs`, but written to the registry with Unity's
/// PlayerPrefs conventions: `PlayerPrefs.GetString` values are `REG_BINARY`
/// (NUL-terminated), `PlayerPrefs.GetInt` values are `REG_DWORD`, and every
/// value name carries Unity's `_h<hash>` suffix. Only the keys we own are
/// written, so the launcher's own Unity/analytics keys are left untouched.
pub fn write_launcher_login(
    _domain: &str,
    env_prefix: &str,
    login: &LauncherLogin,
) -> Result<(), Box<dyn Error>> {
    let hkcu = RegKey::predef(HKEY_CURRENT_USER);
    // `create_subkey` opens the key if it exists and creates it otherwise, so
    // this works even if the launcher has never been run on this machine.
    let (key, _disposition) = hkcu.create_subkey(LAUNCHER_REG_SUBKEY)?;

    // Credential keys: obfuscated (base64) key names. Email is stored plaintext,
    // password is stored base64-encoded.
    let guid_key = playerprefs_key(&obfuscated_key(env_prefix, "guid"));
    let ps_key = playerprefs_key(&obfuscated_key(env_prefix, "ps"));
    let password_b64 = STANDARD.encode(login.password.as_bytes());

    // String-typed keys (the launcher reads these via PlayerPrefs.GetString).
    let string_keys: [(String, &str); 6] = [
        (guid_key, login.email.as_str()),
        (ps_key, password_b64.as_str()),
        (playerprefs_key("token"), login.token.as_str()),
        (playerprefs_key("tokenTimestamp"), login.token_timestamp.as_str()),
        (playerprefs_key("tokenExpiration"), login.token_expiration.as_str()),
        (playerprefs_key("name"), login.name.as_str()),
    ];
    for (name, value) in &string_keys {
        key.set_raw_value(name, &string_value(value))?;
    }

    // Integer-typed keys (the launcher reads these via PlayerPrefs.GetInt).
    let name_chosen: u32 = if login.name.is_empty() { 0 } else { 1 };
    let verified_email: u32 = if login.verified_email { 1 } else { 0 };
    let int_keys: [(String, u32); 6] = [
        (playerprefs_key("nameChosen"), name_chosen),
        (playerprefs_key("verifiedEmail"), verified_email),
        (playerprefs_key("AccountVersion"), 2),
        (playerprefs_key("characterId"), (-1i32) as u32),
        (playerprefs_key("isAdmin"), 0),
        (playerprefs_key("showTosPopup"), 0),
    ];
    for (name, value) in &int_keys {
        key.set_value(name, value)?;
    }

    Ok(())
}

#[cfg(test)]
mod tests {
    use super::{obfuscated_key, playerprefs_key};

    /// Locks the hashing/obfuscation against the real launcher registry: these
    /// are the exact value names observed on Windows for a signed-in launcher.
    #[test]
    fn playerprefs_keys_match_launcher_format() {
        assert_eq!(
            playerprefs_key(&obfuscated_key("Production", "guid")),
            "UHJvZHVjdGlvbmd1aWQ=_h808129427"
        );
        assert_eq!(
            playerprefs_key(&obfuscated_key("Production", "ps")),
            "UHJvZHVjdGlvbnBz_h3317303335"
        );
        assert_eq!(playerprefs_key("token"), "token_h183304158");
        assert_eq!(playerprefs_key("tokenTimestamp"), "tokenTimestamp_h121963056");
        assert_eq!(
            playerprefs_key("tokenExpiration"),
            "tokenExpiration_h4044509493"
        );
        assert_eq!(playerprefs_key("name"), "name_h2087876002");
        assert_eq!(playerprefs_key("isAdmin"), "isAdmin_h2754791920");
        assert_eq!(playerprefs_key("verifiedEmail"), "verifiedEmail_h2486142991");
        assert_eq!(playerprefs_key("nameChosen"), "nameChosen_h78283710");
        assert_eq!(playerprefs_key("showTosPopup"), "showTosPopup_h3263037028");
    }
}
