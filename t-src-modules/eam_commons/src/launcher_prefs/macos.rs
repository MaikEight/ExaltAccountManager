use base64::{engine::general_purpose::STANDARD, Engine as _};
use core_foundation::base::TCFType;
use core_foundation::number::CFNumber;
use core_foundation::string::CFString;
use core_foundation_sys::preferences::{CFPreferencesAppSynchronize, CFPreferencesSetAppValue};
use core_foundation_sys::propertylist::CFPropertyListRef;
use std::error::Error;

use super::LauncherLogin;

/// The launcher obfuscates the PlayerPrefs key names of its credential fields by
/// base64-encoding `"{env_prefix}{suffix}"`. For example, `"Productionguid"`
/// becomes `"UHJvZHVjdGlvbmd1aWQ="` and `"Productionps"` becomes
/// `"UHJvZHVjdGlvbnBz"`.
fn obfuscated_key(env_prefix: &str, suffix: &str) -> String {
    STANDARD.encode(format!("{env_prefix}{suffix}").as_bytes())
}

/// Writes the selected account's login into the official launcher's CFPreferences
/// domain so the launcher comes up already signed in.
///
/// `domain` is the launcher's preference/bundle identifier
/// (`com.decagames.RealmOfTheMadGodExaltLauncher`); `env_prefix` is the
/// environment the credential keys are scoped to (`Production`).
///
/// This goes through CFPreferences (talking to the `cfprefsd` daemon) rather than
/// writing the `.plist` file directly. A direct file write would race the daemon's
/// in-memory cache and be silently reverted. CFPreferences also sets keys
/// individually, so the launcher's own Unity/analytics keys (`Screenmanager *`,
/// `unity.*`, `UnitySelectMonitor`) are left untouched.
pub fn write_launcher_login(
    domain: &str,
    env_prefix: &str,
    login: &LauncherLogin,
) -> Result<(), Box<dyn Error>> {
    let app_id = CFString::new(domain);

    // Credential keys: obfuscated (base64) key names. Email is stored plaintext,
    // password is stored base64-encoded.
    let guid_key = CFString::new(&obfuscated_key(env_prefix, "guid"));
    let ps_key = CFString::new(&obfuscated_key(env_prefix, "ps"));
    let email_val = CFString::new(&login.email);
    let password_val = CFString::new(&STANDARD.encode(login.password.as_bytes()));

    // String-typed keys (the launcher reads these via PlayerPrefs.GetString).
    let string_keys: [(CFString, CFString); 4] = [
        (CFString::new("token"), CFString::new(&login.token)),
        (
            CFString::new("tokenTimestamp"),
            CFString::new(&login.token_timestamp),
        ),
        (
            CFString::new("tokenExpiration"),
            CFString::new(&login.token_expiration),
        ),
        (CFString::new("name"), CFString::new(&login.name)),
    ];

    // Integer-typed keys (the launcher reads these via PlayerPrefs.GetInt).
    let name_chosen: i32 = if login.name.is_empty() { 0 } else { 1 };
    let verified_email: i32 = if login.verified_email { 1 } else { 0 };
    let int_keys: [(CFString, CFNumber); 6] = [
        (CFString::new("nameChosen"), CFNumber::from(name_chosen)),
        (CFString::new("verifiedEmail"), CFNumber::from(verified_email)),
        (CFString::new("AccountVersion"), CFNumber::from(2i32)),
        (CFString::new("characterId"), CFNumber::from(-1i32)),
        (CFString::new("isAdmin"), CFNumber::from(0i32)),
        (CFString::new("showTosPopup"), CFNumber::from(0i32)),
    ];

    // SAFETY: every CFString/CFNumber above outlives this block (they are owned by
    // local bindings), and CFPreferencesSetAppValue copies the values into the
    // daemon's representation. We never take ownership under the create rule, so no
    // manual CFRelease is needed.
    unsafe {
        let app_id_ref = app_id.as_concrete_TypeRef();

        CFPreferencesSetAppValue(
            guid_key.as_concrete_TypeRef(),
            email_val.as_concrete_TypeRef() as CFPropertyListRef,
            app_id_ref,
        );
        CFPreferencesSetAppValue(
            ps_key.as_concrete_TypeRef(),
            password_val.as_concrete_TypeRef() as CFPropertyListRef,
            app_id_ref,
        );

        for (key, val) in &string_keys {
            CFPreferencesSetAppValue(
                key.as_concrete_TypeRef(),
                val.as_concrete_TypeRef() as CFPropertyListRef,
                app_id_ref,
            );
        }
        for (key, val) in &int_keys {
            CFPreferencesSetAppValue(
                key.as_concrete_TypeRef(),
                val.as_concrete_TypeRef() as CFPropertyListRef,
                app_id_ref,
            );
        }

        // Flush once. Must succeed before the launcher is started, otherwise the
        // launcher would read stale/absent credentials.
        if CFPreferencesAppSynchronize(app_id_ref) == 0 {
            return Err(format!("CFPreferencesAppSynchronize failed for domain {domain}").into());
        }
    }

    Ok(())
}

#[cfg(test)]
mod tests {
    use super::obfuscated_key;

    #[test]
    fn obfuscated_keys_match_launcher_format() {
        // Locks the contract against the real launcher plist.
        assert_eq!(obfuscated_key("Production", "guid"), "UHJvZHVjdGlvbmd1aWQ=");
        assert_eq!(obfuscated_key("Production", "ps"), "UHJvZHVjdGlvbnBz");
    }
}
