use anyhow::{bail, Context, Result};
use security_framework::base::Error as SecError;
use security_framework::passwords::{delete_generic_password, get_generic_password, set_generic_password};
use security_framework_sys::base::errSecItemNotFound;

const SERVICE: &str = "com.exaltaccountmanager.credentials";

fn osstatus_of(e: &SecError) -> Option<i32> {
    #[allow(deprecated)]
    Some(e.code())
}

/// Writes/updates Keychain item for (SERVICE, identifier). 
/// Returns `identifier` for DB.
pub fn encrypt_data(identifier: &str, secret: &str) -> Result<String> {
    set_generic_password(SERVICE, identifier, secret.as_bytes())
        .with_context(|| format!("Keychain set failed for {SERVICE}/{identifier}"))?;
    Ok(identifier.to_string())
}

/// Reads Keychain item using the identifier stored in DB.
pub fn decrypt_data(db_value: &str) -> Result<String> {
    match get_generic_password(SERVICE, db_value) {
        Ok(bytes) => Ok(String::from_utf8(bytes).context("Keychain value not valid UTF-8")?),
        Err(e) => {
            if let Some(code) = osstatus_of(&e) {
                if code == errSecItemNotFound as i32 {
                    bail!("secret not found for identifier: {db_value}");
                }
            }
            bail!("Keychain get failed for {SERVICE}/{db_value}: {e}");
        }
    }
}

/// Deletes Keychain item using the identifier stored in DB.
pub fn delete_data(db_value: &str) -> Result<bool> {
    match delete_generic_password(SERVICE, db_value) {
        Ok(()) => Ok(true),
        Err(e) => {
            if let Some(code) = osstatus_of(&e) {
                if code == errSecItemNotFound as i32 {
                    return Ok(false);
                }
            }
            bail!("Keychain delete failed for {SERVICE}/{db_value}: {e}");
        }
    }
}
