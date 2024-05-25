extern crate dirs;

use md5;
use std::fs;
use std::fs::File;
use std::io::Error;
use std::io::Read;
use std::io::Write;
use std::os::windows::process::CommandExt;
use std::process::Stdio;

#[cfg(target_os = "windows")]
const CREATE_NO_WINDOW: u32 = 0x08000000;

// Exit codes of EAM_Task_Tools:
// 0 - Success
// 1 - No mode provided
// 2 - Invalid mode
// 3 - Error un-/installing task
// 4 - Invalid path to daily login application
// 5 - Task already installed
// 6 - Task not installed
// 7 - Permission denied
#[cfg(target_os = "windows")]
const EAM_TASK_TOOLS: &'static [u8] =
    include_bytes!("../../EAM_Task_Installer/EAM_Task_Installer/bin/Release/EAM_Task_Tools.exe");
const EAM_TASK_TOOLS_HASH: &'static str = "f837ed769e432f7f1d0f49bdf4d30f05";

#[cfg(target_os = "windows")]
pub fn check_for_installed_eam_daily_login_task(check_for_v1: bool) -> Result<bool, Error> {
    let path = ensure_eam_task_tools()?;

    let check_arg = match check_for_v1 {
        true => "checkV1",
        false => "check",
    };

    let output = std::process::Command::new(path)
        .arg(check_arg)
        .stderr(Stdio::null())
        .creation_flags(CREATE_NO_WINDOW)
        .output()?;

    match output.status.code() {
        Some(0) => Ok(true),
        Some(6) => Ok(false),
        _ => {
            let stdout = String::from_utf8_lossy(&output.stdout);
            Err(Error::new(
                std::io::ErrorKind::Other,
                format!(
                    "Failed to check for installed task. Exit code: {}. Output: {}",
                    output.status, stdout
                ),
            ))
        }
    }
}

#[cfg(target_os = "windows")]
pub fn install_eam_daily_login_task(exe_path: &str) -> Result<bool, Error> {
    let path = ensure_eam_task_tools()?;

    let output = std::process::Command::new(path)
        .arg("install")
        .arg(exe_path)
        .creation_flags(CREATE_NO_WINDOW)
        .output()?;

    if output.status.success() {
        let res = check_for_installed_eam_daily_login_task(false);

        match res {
            Ok(true) =>  Ok(true),            
            Ok(false) => Ok(false),
            _ => {
                let stdout = String::from_utf8_lossy(&output.stdout);
                Err(Error::new(
                    std::io::ErrorKind::Other,
                    format!(
                        "Failed to install task. Exit code: {}. Output: {}. Result: {}",
                        output.status,
                        stdout,
                        res.unwrap().to_string()
                    ),
                ))
            }            
        }
    } else {
        let stdout = String::from_utf8_lossy(&output.stdout);
        Err(Error::new(
            std::io::ErrorKind::Other,
            format!(
                "Failed to install task. Exit code: {}. Output: {}",
                output.status, stdout
            ),
        ))
    }
}

#[cfg(target_os = "windows")]
pub fn uninstall_eam_daily_login_task(uninstall_v1: bool) -> Result<bool, Error> {
    let path = ensure_eam_task_tools()?;

    let uninstall_arg = match uninstall_v1 {
        true => "uninstallV1",
        false => "uninstall",
    };

    let output = std::process::Command::new(path)
        .arg(uninstall_arg)
        .creation_flags(CREATE_NO_WINDOW)
        .output()?;

    if output.status.success() {
        let res = check_for_installed_eam_daily_login_task(uninstall_v1);
        if res.is_ok() && res.unwrap() {
            let stdout = String::from_utf8_lossy(&output.stdout);
            Err(Error::new(
                std::io::ErrorKind::Other,
                format!(
                    "Failed to uninstall task. Exit code: {}. Output: {}",
                    output.status, stdout
                ),
            ))
        } else {
            Ok(true)
        }
    } else {
        let stdout = String::from_utf8_lossy(&output.stdout);
        Err(Error::new(
            std::io::ErrorKind::Other,
            format!(
                "Failed to uninstall task. Exit code: {}. Output: {}",
                output.status, stdout
            ),
        ))
    }
}

#[cfg(target_os = "windows")]
fn ensure_eam_task_tools() -> Result<String, Error> {
    let mut path = dirs::data_local_dir().unwrap();
    path.push("ExaltAccountManager");
    path.push("v4");
    std::fs::create_dir_all(&path)?;

    let mut path = dirs::data_local_dir().unwrap();
    path.push("ExaltAccountManager");
    path.push("v4");
    path.push("EAM_Task_Tools.exe");

    if !path.exists() {
        let mut file = File::create(&path)?;
        file.write_all(EAM_TASK_TOOLS)?;
        file.sync_all()?;
        drop(file);
    } else {
        //If hash is different, overwrite file
        let mut file = fs::File::open(path.clone())?;

        let mut buffer = Vec::new();
        file.read_to_end(&mut buffer)?;
        drop(file);
        let hash = md5::compute(&buffer);
        let hash_string = format!("{:x}", hash);

        if hash_string != EAM_TASK_TOOLS_HASH {
            println!("Hashes are different, overwriting EAM_Task_Tools.exe");
            fs::write(path.clone(), EAM_TASK_TOOLS)?;
        }
    }

    Ok(path.to_str().unwrap().to_string())
}
