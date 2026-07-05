use std::error::Error;

use super::LauncherLogin;

pub fn write_launcher_login(
    _domain: &str,
    _env_prefix: &str,
    _login: &LauncherLogin,
) -> Result<(), Box<dyn Error>> {
    Err("Launcher login writing is not supported on this platform".into())
}
