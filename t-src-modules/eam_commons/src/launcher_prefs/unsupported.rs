use std::error::Error;

use super::LauncherLogin;

pub fn write_launcher_login(
    _domain: &str,
    _env_prefix: &str,
    _login: &LauncherLogin,
) -> Result<(), Box<dyn Error>> {
    Err("Launcher login writing is not supported on this platform".into())
}

pub fn write_game_character_id(_character_id: i32) -> Result<(), Box<dyn Error>> {
    Err("Setting the game character is not supported on this platform".into())
}
