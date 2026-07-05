//! Detects which EAM accounts currently have the RotMG game running.
//!
//! Every game process (`RotMG Exalt.exe`) that EAM launches directly — the Steam
//! flow and the daily-login CLI — is started with a `data:{...}` parameter string
//! that embeds the account identity as `guid:<base64(email)>` (see
//! `src/hooks/useStartGame.jsx`). For the default non-Steam launcher flow EAM does
//! NOT pass this argument itself; it writes the login into the launcher's prefs
//! store and the (closed-source) DECA launcher then spawns the game. Whether the
//! launcher-spawned process carries a `guid:` parameter — and whether that value
//! is base64 or plain text — is external to this repo and MUST be verified
//! empirically (e.g. `Get-CimInstance Win32_Process -Filter "Name='RotMG Exalt.exe'" | Select CommandLine`).
//! To be tolerant, [`extract_email_from_command_line`] accepts both a
//! base64-encoded email and a plain-text email, and [`log_detection_diagnostics`]
//! logs when game processes are found but cannot be mapped to an account.
//!
//! A background thread polls the running game processes, decodes the account
//! email from each, and emits the full set of running emails to the frontend
//! whenever it changes. The current set is also exposed synchronously through
//! [`get_running_game_accounts`] so the frontend can fetch the initial state on
//! mount.

use std::collections::HashSet;
use std::sync::atomic::{AtomicBool, Ordering};
use std::sync::Mutex;
use std::time::Duration;

use base64::{engine::general_purpose::STANDARD, Engine as _};
use lazy_static::lazy_static;
use log::{error, info};
use tauri::{AppHandle, Emitter};

lazy_static! {
    /// The set of account emails that currently have the game running.
    static ref RUNNING_GAME_EMAILS: Mutex<HashSet<String>> = Mutex::new(HashSet::new());
}

/// Ensures the watcher thread is only spawned once.
static HAS_STARTED: AtomicBool = AtomicBool::new(false);

/// Windows process image name used to filter the WMI query.
#[cfg(windows)]
const GAME_PROCESS_NAME: &str = "RotMG Exalt.exe";

/// Tauri event emitted (with a [`RunningGamesPayload`]) whenever the running set changes.
const EVENT_NAME: &str = "running-game-accounts-changed";

/// How often the running processes are polled.
const POLL_INTERVAL_SECS: u64 = 3;

#[derive(serde::Serialize, Clone)]
struct RunningGamesPayload {
    emails: Vec<String>,
}

/// Returns the account emails that currently have the game running.
pub fn get_running_game_accounts() -> Vec<String> {
    let guard = RUNNING_GAME_EMAILS
        .lock()
        .unwrap_or_else(|poisoned| poisoned.into_inner());
    guard.iter().cloned().collect()
}

/// Starts the background watcher. Idempotent: subsequent calls are no-ops.
pub fn start(app: AppHandle) {
    if HAS_STARTED.swap(true, Ordering::SeqCst) {
        info!("Game instance watcher already started, skipping...");
        return;
    }

    #[cfg(windows)]
    {
        info!("Starting game instance watcher (Windows/WMI)...");
        std::thread::spawn(move || run_watcher_loop_windows(app));
    }

    #[cfg(target_os = "macos")]
    {
        info!("Starting game instance watcher (macOS/ps)...");
        std::thread::spawn(move || run_watcher_loop_macos(app));
    }

    #[cfg(not(any(windows, target_os = "macos")))]
    {
        let _ = app;
        info!("Game instance watcher is not implemented on this platform; skipping.");
    }
}

/// Extracts the account email from a game process command line.
///
/// Looks for `guid:` and reads the value up to the next `,` or `}`. The game's own
/// launch format base64-encodes the email (`guid:<base64(email)>`), so we try that
/// first; as a fallback we accept a plain-text email in case the launcher hands the
/// game the raw email. Returns `None` if neither yields a plausible email (i.e.
/// something containing `@`).
fn extract_email_from_command_line(cmd: &str) -> Option<String> {
    const MARKER: &str = "guid:";
    let start = cmd.find(MARKER)? + MARKER.len();
    let rest = &cmd[start..];
    let end = rest.find(|c: char| c == ',' || c == '}').unwrap_or(rest.len());
    let raw = rest[..end].trim();
    if raw.is_empty() {
        return None;
    }

    // Preferred: base64-encoded email (matches useStartGame.jsx's `btoa(email)`).
    if let Ok(bytes) = STANDARD.decode(raw) {
        if let Ok(email) = String::from_utf8(bytes) {
            if email.contains('@') {
                return Some(email);
            }
        }
    }

    // Fallback: the guid is already a plain-text email.
    if raw.contains('@') {
        return Some(raw.to_string());
    }

    None
}

/// Updates the shared set and emits the change event, but only if the set of
/// running emails actually changed since the last poll.
fn maybe_emit(app: &AppHandle, current: HashSet<String>) {
    {
        let mut guard = RUNNING_GAME_EMAILS
            .lock()
            .unwrap_or_else(|poisoned| poisoned.into_inner());
        if *guard == current {
            return;
        }
        *guard = current.clone();
    }

    let mut emails: Vec<String> = current.into_iter().collect();
    emails.sort();
    if let Err(e) = app.emit(EVENT_NAME, RunningGamesPayload { emails }) {
        error!("Failed to emit {} event: {}", EVENT_NAME, e);
    }
}

/// Logs — only when the counts change — how many game processes were found versus
/// how many could be mapped to an account. This makes the "a game is running but no
/// row lights up" situation diagnosable: chiefly the launcher flow (game may carry
/// no `guid:` argument) and the elevated case (WMI returns a NULL command line when
/// the game runs at a higher integrity level than EAM).
#[cfg(any(windows, target_os = "macos"))]
fn log_detection_diagnostics(last: &mut Option<(usize, usize)>, found: usize, parsed: usize) {
    let snapshot = (found, parsed);
    if *last == Some(snapshot) {
        return;
    }
    *last = Some(snapshot);

    if found == 0 {
        info!("Game instance watcher: no game processes running.");
    } else if parsed == 0 {
        log::warn!(
            "Game instance watcher: found {} running game process(es) but could not identify any \
             account from their command lines. The command line may lack a 'guid:' parameter (e.g. \
             launcher-started games) or be unreadable (game running elevated while EAM is not).",
            found
        );
    } else if parsed < found {
        log::warn!(
            "Game instance watcher: found {} running game process(es) but only identified {}; the \
             rest lack a readable/parseable 'guid:' parameter.",
            found, parsed
        );
    } else {
        info!(
            "Game instance watcher: {} running game process(es), all identified.",
            found
        );
    }
}

#[cfg(windows)]
fn run_watcher_loop_windows(app: AppHandle) {
    use wmi::WMIConnection;

    #[derive(serde::Deserialize)]
    #[serde(rename_all = "PascalCase")]
    struct ProcessRow {
        command_line: Option<String>,
    }

    let query = format!(
        "SELECT CommandLine FROM Win32_Process WHERE Name = '{}'",
        GAME_PROCESS_NAME
    );

    // The connection is (re)built inside the loop: a transient failure (e.g. the WMI
    // service still coming up at boot/autostart) or a later stale proxy self-heals on
    // the next tick instead of permanently killing detection. `WMIConnection::new()`
    // initializes COM (MTA) for this dedicated thread; the connection is `!Send` and
    // never leaves this thread.
    let mut wmi_con: Option<WMIConnection> = None;
    // (processes_found, accounts_parsed) — used to log diagnostics only on change.
    let mut last_diag: Option<(usize, usize)> = None;

    loop {
        if wmi_con.is_none() {
            match WMIConnection::new() {
                Ok(c) => wmi_con = Some(c),
                Err(e) => {
                    log::debug!("Game instance watcher: WMI connect failed, retrying: {:?}", e);
                    std::thread::sleep(Duration::from_secs(POLL_INTERVAL_SECS));
                    continue;
                }
            }
        }

        // Borrow the connection just for the query; the owned Result releases the
        // borrow so a failing query can drop the (possibly stale) connection.
        match wmi_con.as_ref().map(|con| con.raw_query::<ProcessRow>(&query)) {
            Some(Ok(rows)) => {
                let found = rows.len();
                let mut parsed = 0usize;
                let mut current: HashSet<String> = HashSet::new();
                for row in rows {
                    if let Some(cmd) = row.command_line {
                        if let Some(email) = extract_email_from_command_line(&cmd) {
                            parsed += 1;
                            current.insert(email);
                        }
                    }
                }
                log_detection_diagnostics(&mut last_diag, found, parsed);
                maybe_emit(&app, current);
            }
            Some(Err(e)) => {
                log::debug!("Game instance watcher: WMI query failed, reconnecting: {:?}", e);
                wmi_con = None; // rebuild on the next tick
            }
            None => {
                wmi_con = None; // unreachable (just ensured Some) — reconnect defensively
            }
        }

        std::thread::sleep(Duration::from_secs(POLL_INTERVAL_SECS));
    }
}

#[cfg(target_os = "macos")]
fn run_watcher_loop_macos(app: AppHandle) {
    let mut last_diag: Option<(usize, usize)> = None;

    loop {
        // `-ww` prevents argument truncation; `command=` prints the full argv with no
        // header. We scan every process line for the game and parse the guid.
        match std::process::Command::new("/usr/bin/ps")
            .args(["-axww", "-o", "command="])
            .output()
        {
            Ok(output) => {
                let stdout = String::from_utf8_lossy(&output.stdout);
                let game_lines: Vec<&str> = stdout
                    .lines()
                    .filter(|line| is_macos_game_process_line(line))
                    .collect();
                let found = game_lines.len();
                let mut parsed = 0usize;
                let mut current: HashSet<String> = HashSet::new();
                for line in game_lines {
                    if let Some(email) = extract_email_from_command_line(line) {
                        parsed += 1;
                        current.insert(email);
                    }
                }
                log_detection_diagnostics(&mut last_diag, found, parsed);
                maybe_emit(&app, current);
            }
            Err(e) => log::debug!("Game instance watcher: 'ps' invocation failed: {:?}", e),
        }

        std::thread::sleep(Duration::from_secs(POLL_INTERVAL_SECS));
    }
}

/// Heuristic for a macOS RotMG game process line: the game executable is
/// `RotMG Exalt.app/Contents/MacOS/RotMG Exalt`; exclude the separate launcher.
#[cfg(target_os = "macos")]
fn is_macos_game_process_line(line: &str) -> bool {
    let lower = line.to_ascii_lowercase();
    lower.contains("rotmg exalt") && !lower.contains("launcher")
}
