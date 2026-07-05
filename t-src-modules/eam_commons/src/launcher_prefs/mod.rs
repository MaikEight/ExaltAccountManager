//! Writes the official ROTMG launcher's "signed-in account" data into the
//! OS-specific preference store, so EAM can pre-seed the launcher with the
//! account selected in EAM and start the launcher already signed in.
//!
//! This exists to comply with DECA's ToS: only the official launcher may start
//! the game. Instead of launching the game executable directly, EAM writes the
//! login the launcher expects and then starts the launcher.
//!
//! - macOS: CFPreferences domain `com.decagames.RealmOfTheMadGodExaltLauncher`.
//! - Windows: the registry (not yet implemented — see `windows.rs`).

/// The login payload written into the official launcher's preference store.
///
/// `password` is the PLAINTEXT password; it is base64-encoded at write time
/// (the launcher stores the password base64-encoded). `token_timestamp` and
/// `token_expiration` are kept as strings because the launcher reads them as
/// strings.
#[derive(Debug, Clone)]
pub struct LauncherLogin {
    /// Account email, stored in plaintext.
    pub email: String,
    /// Plaintext password; base64-encoded before it is written.
    pub password: String,
    /// Access token from the `account/verify` API.
    pub token: String,
    /// Token issued-at unix timestamp, as a string (e.g. "1782591327").
    pub token_timestamp: String,
    /// Token lifetime in seconds, as a string (e.g. "86400").
    pub token_expiration: String,
    /// In-game name (may be empty).
    pub name: String,
    /// Whether the account's email is verified, from the `account/verify`
    /// response (the launcher stores this as its `verifiedEmail` flag).
    pub verified_email: bool,
}

#[cfg(target_os = "windows")]
mod windows;
#[cfg(target_os = "windows")]
pub use windows::write_launcher_login;

#[cfg(target_os = "macos")]
mod macos;
#[cfg(target_os = "macos")]
pub use macos::write_launcher_login;

#[cfg(not(any(target_os = "windows", target_os = "macos")))]
mod unsupported;
#[cfg(not(any(target_os = "windows", target_os = "macos")))]
pub use unsupported::write_launcher_login;
