use std::error::Error;

use super::LauncherLogin;

/// TODO: Write the launcher login into the Windows registry.
///
/// The official launcher stores its signed-in account in the registry with the
/// values encoded as base64 strings. Mirror the macOS layout (see `macos.rs`):
/// the credential keys are `base64("{env_prefix}guid")` = email (plaintext) and
/// `base64("{env_prefix}ps")` = base64(password), plus `token`,
/// `tokenTimestamp`, `tokenExpiration`, `name`, and the integer flag keys
/// (`nameChosen`, `verifiedEmail`, `AccountVersion`, `characterId`, `isAdmin`,
/// `showTosPopup`).
///
/// Use the `winreg` crate (already a dependency of the `src-tauri` crate) once
/// the exact registry path and value layout are confirmed.
pub fn write_launcher_login(
    _domain: &str,
    _env_prefix: &str,
    _login: &LauncherLogin,
) -> Result<(), Box<dyn Error>> {
    Err("Launcher login writing is not yet implemented on Windows".into())
}
