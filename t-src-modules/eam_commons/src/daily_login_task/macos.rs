extern crate dirs;

use log::info;
use std::fs;
use std::fs::File;
use std::io::Error;
use std::io::Write;
use std::path::PathBuf;
use std::process::Command;
use chrono::{Local, Timelike};

// LaunchAgent configuration matching Windows Task Scheduler behavior:
// - Runs 5 minutes after login (matching LogonTrigger with 5min delay)
// - Runs daily at calculated time based on UTC offset (matching DailyTrigger)
// - ThrottleInterval prevents multiple runs within 24 hours
// - StartInterval provides the login delay mechanism
const LAUNCH_AGENT_LABEL: &str = "com.exaltaccountmanager.dailylogin";
const LAUNCH_AGENT_PLIST_NAME: &str = "com.exaltaccountmanager.dailylogin.plist";
const LAUNCH_AGENT_PLIST_TEMPLATE: &str = include_str!("com.exaltaccountmanager.dailylogin.plist");

/// Check if the EAM daily login LaunchAgent is installed
#[cfg(target_os = "macos")]
pub fn check_for_installed_eam_daily_login_task(check_for_old_versions: bool) -> Result<bool, Error> {
    let plist_path = get_launch_agent_path()?;
    
    if !plist_path.exists() {
        return Ok(false);
    }

    // Check if it's actually loaded with launchctl
    let output = Command::new("launchctl")
        .arg("list")
        .arg(LAUNCH_AGENT_LABEL)
        .output();

    match output {
        Ok(output) => {
            if output.status.success() {
                Ok(true)
            } else {
                // Plist exists but not loaded - consider as not properly installed
                Ok(false)
            }
        }
        Err(e) => {
            if e.kind() == std::io::ErrorKind::NotFound {
                Err(Error::new(
                    std::io::ErrorKind::NotFound,
                    "launchctl command not found. This should not happen on macOS.",
                ))
            } else {
                // If we can't check with launchctl, fall back to file existence
                Ok(plist_path.exists())
            }
        }
    }
}

/// Install the EAM daily login LaunchAgent
#[cfg(target_os = "macos")]
pub fn install_eam_daily_login_task(exe_path: &str, args: Option<&str>) -> Result<bool, Error> {
    // First check if already installed
    if check_for_installed_eam_daily_login_task(false)? {
        return Err(Error::new(
            std::io::ErrorKind::AlreadyExists,
            "Daily login task is already installed",
        ));
    }

    // Check if the executable path exists (unless args contains "ignoreFileCheck")
    let ignore_file_check = args.map_or(false, |a| a.contains("ignoreFileCheck"));
    let path_obj = std::path::Path::new(exe_path);
    
    if !ignore_file_check && !path_obj.exists() {
        return Err(Error::new(
            std::io::ErrorKind::NotFound,
            format!("The provided path to the daily login application is invalid: {}", exe_path),
        ));
    }

    // Ensure the LaunchAgents directory exists
    let plist_path = get_launch_agent_path()?;
    if let Some(parent) = plist_path.parent() {
        fs::create_dir_all(parent)?;
    }

    // Build the program arguments array (filter out ignoreFileCheck if present)
    let mut program_arguments = vec![exe_path.to_string()];
    if let Some(args_str) = args {
        if !args_str.is_empty() {
            // Split args by space and add them (excluding ignoreFileCheck)
            program_arguments.extend(
                args_str.split_whitespace()
                    .filter(|&s| s != "ignoreFileCheck")
                    .map(|s| s.to_string())
            );
        }
    }

    // Calculate trigger time with UTC offset (matching Windows behavior)
    let now = Local::now();
    let utc_offset_hours = now.offset().local_minus_utc() / 3600;
    let trigger_hour = (24 + utc_offset_hours) % 24;
    let trigger_minute = 1;

    // Generate the plist content
    let plist_content = generate_launch_agent_plist(
        exe_path, 
        program_arguments, 
        trigger_hour, 
        trigger_minute
    )?;

    // Write the plist file
    let mut file = File::create(&plist_path)?;
    file.write_all(plist_content.as_bytes())?;
    file.sync_all()?;
    drop(file);

    info!("Created LaunchAgent plist at: {}", plist_path.display());

    // Load the LaunchAgent
    let output = Command::new("launchctl")
        .arg("load")
        .arg("-w")
        .arg(&plist_path)
        .output()?;

    if !output.status.success() {
        let stderr = String::from_utf8_lossy(&output.stderr);
        let stdout = String::from_utf8_lossy(&output.stdout);
        
        // Clean up the plist file since loading failed
        let _ = fs::remove_file(&plist_path);
        
        return Err(Error::new(
            std::io::ErrorKind::Other,
            format!(
                "Failed to load LaunchAgent. Exit code: {}. Stdout: {}. Stderr: {}",
                output.status, stdout, stderr
            ),
        ));
    }

    info!("Successfully loaded LaunchAgent");

    // Verify installation
    match check_for_installed_eam_daily_login_task(false) {
        Ok(true) => Ok(true),
        Ok(false) => Err(Error::new(
            std::io::ErrorKind::Other,
            "LaunchAgent was loaded but verification failed",
        )),
        Err(e) => Err(e),
    }
}

/// Uninstall the EAM daily login LaunchAgent
#[cfg(target_os = "macos")]
pub fn uninstall_eam_daily_login_task(uninstall_old_versions: bool) -> Result<bool, Error> {
    let plist_path = get_launch_agent_path()?;

    if !plist_path.exists() {
        info!("LaunchAgent plist does not exist, nothing to uninstall");
        return Ok(true);
    }

    // Unload the LaunchAgent
    let output = Command::new("launchctl")
        .arg("unload")
        .arg("-w")
        .arg(&plist_path)
        .output();

    match output {
        Ok(output) => {
            if !output.status.success() {
                let stderr = String::from_utf8_lossy(&output.stderr);
                let stdout = String::from_utf8_lossy(&output.stdout);
                info!(
                    "launchctl unload returned non-zero status. Stdout: {}. Stderr: {}",
                    stdout, stderr
                );
                // Continue anyway - the agent might not be loaded
            }
        }
        Err(e) => {
            info!("Failed to execute launchctl unload: {}. Continuing...", e);
            // Continue anyway
        }
    }

    // Remove the plist file
    if plist_path.exists() {
        fs::remove_file(&plist_path)?;
        info!("Removed LaunchAgent plist from: {}", plist_path.display());
    }

    // Verify uninstallation
    match check_for_installed_eam_daily_login_task(false) {
        Ok(false) => Ok(true),
        Ok(true) => Err(Error::new(
            std::io::ErrorKind::Other,
            "Failed to uninstall LaunchAgent - still exists after removal",
        )),
        Err(_) => Ok(true), // If we can't check, assume success since we removed the file
    }
}

/// Get the path to the LaunchAgent plist file
fn get_launch_agent_path() -> Result<PathBuf, Error> {
    let mut path = dirs::home_dir().ok_or_else(|| {
        Error::new(
            std::io::ErrorKind::NotFound,
            "Could not determine home directory",
        )
    })?;
    
    path.push("Library");
    path.push("LaunchAgents");
    path.push(LAUNCH_AGENT_PLIST_NAME);
    
    Ok(path)
}

/// Generate the LaunchAgent plist XML content
fn generate_launch_agent_plist(
    exe_path: &str, 
    program_arguments: Vec<String>,
    trigger_hour: i32,
    trigger_minute: i32
) -> Result<String, Error> {
    // Build the ProgramArguments array
    let mut args_xml = String::new();
    for arg in program_arguments {
        args_xml.push_str(&format!("\t\t<string>{}</string>\n", escape_xml(&arg)));
    }

    // Build the StartupDelay section (5 minutes delay after login, matching Windows)
    let minutes_after_login_delay = 5;
    let startup_delay_xml = format!(
        "\t<key>StartOnMount</key>\n\t<true/>\n\t<key>LaunchOnlyOnce</key>\n\t<true/>\n\t<key>StartInterval</key>\n\t<integer>{}</integer>",
        minutes_after_login_delay * 60
    );

    // Replace all placeholders in the template
    let plist = LAUNCH_AGENT_PLIST_TEMPLATE
        .replace("<!-- PROGRAM_ARGUMENTS_PLACEHOLDER -->", &args_xml)
        .replace("<!-- STARTUP_DELAY_PLACEHOLDER -->", &startup_delay_xml)
        .replace("<!-- TRIGGER_HOUR_PLACEHOLDER -->", &trigger_hour.to_string())
        .replace("<!-- TRIGGER_MINUTE_PLACEHOLDER -->", &trigger_minute.to_string());

    Ok(plist)
}

/// Basic XML escaping for plist values
fn escape_xml(s: &str) -> String {
    s.replace("&", "&amp;")
        .replace("<", "&lt;")
        .replace(">", "&gt;")
        .replace("\"", "&quot;")
        .replace("'", "&apos;")
}
