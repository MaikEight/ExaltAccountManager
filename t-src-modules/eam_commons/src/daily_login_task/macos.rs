extern crate dirs;

use log::info;
use std::fs;
use std::fs::File;
use std::io::Error;
use std::io::Write;
use std::path::PathBuf;
use std::process::Command;

// LaunchAgent configuration for daily login task:
// 
// TRIGGERS:
// 1. Daily at 01:00 local time (automatically adjusts for DST)
//    - Uses StartCalendarInterval for scheduled execution
// 
// 2. On system wake/unlock (if daily trigger was missed)
//    - Uses WatchPaths monitoring /var/run/com.apple.SystemManagement.wakeup
//    - Combined with ThrottleInterval (1 hour) to prevent spam
//    - Ensures task runs if Mac was asleep during scheduled time
// 
// 3. 5 minutes after login (via StartInterval)
//    - Only triggers once per login session
//    - Provides fallback if other triggers fail
//
// BEHAVIOR:
// - Uses deep link (eam:start-daily-login-task) for silent startup
// - ThrottleInterval (3600s = 1 hour) prevents multiple rapid executions
// - macOS automatically handles DST changes for calendar-based triggers
const LAUNCH_AGENT_LABEL: &str = "com.exaltaccountmanager.dailylogin";
const LAUNCH_AGENT_PLIST_NAME: &str = "com.exaltaccountmanager.dailylogin.plist";
const LAUNCH_AGENT_PLIST_TEMPLATE: &str = include_str!("com.exaltaccountmanager.dailylogin.plist");

/// Check if the EAM daily login LaunchAgent is installed
#[cfg(target_os = "macos")]
pub fn check_for_installed_eam_daily_login_task(check_for_old_versions: bool) -> Result<bool, Error> {
    if check_for_old_versions {
        // Currently no old versions to check for
        return Ok(false);
    }

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

    // Convert executable path to .app bundle path
    // e.g., /Applications/Exalt Account Manager.app/Contents/MacOS/Exalt Account Manager
    // becomes /Applications/Exalt Account Manager.app
    let app_bundle_path = extract_app_bundle_path(exe_path)?;
    
    info!("Executable path: {}", exe_path);
    info!("Extracted app bundle path: {}", app_bundle_path);

    // Ensure the LaunchAgents directory exists
    let plist_path = get_launch_agent_path()?;
    if let Some(parent) = plist_path.parent() {
        fs::create_dir_all(parent)?;
    }

    // Build the program arguments array using 'open' command to launch the .app bundle
    // This ensures macOS properly identifies the app by its Info.plist instead of the code signing identity
    let mut program_arguments = vec![
        "/usr/bin/open".to_string(),
        "-a".to_string(),
        app_bundle_path.clone(),
    ];
    
    // Add the deep link argument if provided (filter out ignoreFileCheck)
    if let Some(args_str) = args {
        if !args_str.is_empty() {
            let filtered_args: Vec<String> = args_str.split_whitespace()
                .filter(|&s| s != "ignoreFileCheck")
                .map(|s| s.to_string())
                .collect();
            
            if !filtered_args.is_empty() && filtered_args[0].starts_with("eam:") {
                // For deep links (eam:*), use 'open' with the URL directly
                // The URL scheme handler will launch the app automatically
                program_arguments = vec![
                    "/usr/bin/open".to_string(),
                    filtered_args[0].clone(),
                ];
                info!("Using deep link URL: {}", filtered_args[0]);
            }
        }
    }
    
    info!("Final program arguments: {:?}", program_arguments);

    // Calculate trigger time - Use 01:00 local time to account for DST
    // macOS automatically adjusts calendar intervals for DST, so we use local time directly
    let trigger_hour = 1;  // 01:00 local time (after midnight UTC, accounting for most timezones)
    let trigger_minute = 0;

    info!("LaunchAgent will trigger daily at {:02}:{:02} local time", trigger_hour, trigger_minute);

    // Generate the plist content
    let plist_content = generate_launch_agent_plist(
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
pub fn uninstall_eam_daily_login_task(_uninstall_old_versions: bool) -> Result<bool, Error> {
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

/// Extract the .app bundle path from an executable path
/// e.g., /Applications/Exalt Account Manager.app/Contents/MacOS/Exalt Account Manager
/// returns /Applications/Exalt Account Manager.app
fn extract_app_bundle_path(exe_path: &str) -> Result<String, Error> {
    let path = std::path::Path::new(exe_path);
    
    // Walk up the directory tree looking for .app
    for ancestor in path.ancestors() {
        if let Some(extension) = ancestor.extension() {
            if extension == "app" {
                return ancestor.to_str()
                    .map(|s| s.to_string())
                    .ok_or_else(|| Error::new(
                        std::io::ErrorKind::InvalidData,
                        "Failed to convert .app bundle path to string"
                    ));
            }
        }
    }
    
    // If no .app bundle found, return the original path
    // (this shouldn't happen in normal Tauri apps, but provides a fallback)
    info!("Warning: Could not find .app bundle in path: {}. Using executable path directly.", exe_path);
    Ok(exe_path.to_string())
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
    program_arguments: Vec<String>,
    trigger_hour: i32,
    trigger_minute: i32
) -> Result<String, Error> {
    // Build the ProgramArguments array
    let mut args_xml = String::new();
    for arg in program_arguments {
        args_xml.push_str(&format!("\t\t<string>{}</string>\n", escape_xml(&arg)));
    }

    // Build the login delay section (5 minutes delay after login via StartInterval)
    let minutes_after_login_delay = 5;
    let startup_delay_xml = format!(
        "\t<key>StartInterval</key>\n\t<integer>{}</integer>",
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
