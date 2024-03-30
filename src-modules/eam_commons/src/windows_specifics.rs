extern crate winapi;
extern crate dirs;

use std::fs::File;
use std::io::Error;
use std::io::Write;
use std::os::windows::process::CommandExt;
use std::process::Stdio;

use std::os::raw::c_void;
use std::ptr::null_mut;
use winapi::shared::sddl::ConvertSidToStringSidA;
use winapi::um::handleapi::CloseHandle;
use winapi::um::processthreadsapi::OpenProcessToken;
use winapi::um::securitybaseapi::GetTokenInformation;
use winapi::um::winnt::{TokenUser, TOKEN_QUERY};

const CREATE_NO_WINDOW: u32 = 0x08000000;

// Exit codes of EAM_Task_Tools:
// 0 - Success
// 1 - No installation mode or path to daily login application provided
// 2 - Invalid mode
// 3 - Error un-/installing task
// 4 - Invalid path to daily login application
// 5 - Task already installed
// 6 - Task not installed
#[cfg(target_os = "windows")]
const EAM_Task_Tools: &'static [u8] =
    include_bytes!("../../EAM_Task_Installer/EAM_Task_Installer/bin/Release/EAM_Task_Tools.exe");

pub fn check_for_installed_eam_daily_login_task() -> Result<bool, Error> {    
    let path = ensure_eam_task_tools()?;

    let output = std::process::Command::new(path)
        .arg("check")
        .stdout(Stdio::null())
        .stderr(Stdio::null())
        .creation_flags(CREATE_NO_WINDOW)
        .output()?;

    match output.status.success() {
        true => Ok(true),
        false => Ok(false),
    }
}

pub fn install_eam_daily_login_task(exe_path: &str) -> Result<bool, Error> {
    let path = ensure_eam_task_tools()?;

    let output = std::process::Command::new(path)
        .arg("install")
        .arg(exe_path)
        .stdout(Stdio::null())
        .stderr(Stdio::null())
        .creation_flags(CREATE_NO_WINDOW)
        .output()?;

    if output.status.success() {
        let res = check_for_installed_eam_daily_login_task();
        if res.is_ok() && res.unwrap() {
            Ok(true)
        } else {
            Err(Error::new(
                std::io::ErrorKind::Other,
                "Failed to install task ".to_owned() + &output.status.to_string(),
            ))
        }
    } else {
        Err(Error::new(
            std::io::ErrorKind::Other,
            "Failed to install task ".to_owned() + &output.status.to_string(),
        ))
    }
}

pub fn uninstall_eam_daily_login_task() -> Result<bool, Error> {
    let path = ensure_eam_task_tools()?;

    let output = std::process::Command::new(path)
        .arg("uninstall")
        .stdout(Stdio::null())
        .stderr(Stdio::null())
        .creation_flags(CREATE_NO_WINDOW)
        .output()?;

    if output.status.success() {
        let res = check_for_installed_eam_daily_login_task();
        if res.is_ok() && res.unwrap() {
            Err(Error::new(
                std::io::ErrorKind::Other,
                "Failed to uninstall task ".to_owned() + &output.status.to_string(),
            ))
        } else {
            Ok(true)
        }
    } else {
        Err(Error::new(
            std::io::ErrorKind::Other,
            "Failed to uninstall task ".to_owned() + &output.status.to_string(),
        ))
    }
}

#[cfg(target_os = "windows")]
fn get_current_user_sid() -> Option<String> {
    unsafe {
        let mut token: winapi::um::winnt::HANDLE = null_mut();
        if OpenProcessToken(
            winapi::um::processthreadsapi::GetCurrentProcess(),
            TOKEN_QUERY,
            &mut token,
        ) == 0
        {
            return None;
        }

        let mut return_length = 0;
        GetTokenInformation(token, TokenUser, null_mut(), 0, &mut return_length);

        let mut token_user = vec![0u8; return_length as usize];
        if GetTokenInformation(
            token,
            TokenUser,
            token_user.as_mut_ptr() as *mut _,
            return_length,
            &mut return_length,
        ) == 0
        {
            CloseHandle(token);
            return None;
        }

        CloseHandle(token);
        let token_user = token_user.as_ptr() as *mut winapi::um::winnt::TOKEN_USER;
        let sid = (*token_user).User.Sid;

        let mut sid_str_ptr: *mut i8 = null_mut();
        if ConvertSidToStringSidA(sid, &mut sid_str_ptr) == 0 {
            return None;
        }

        let sid_str = std::ffi::CStr::from_ptr(sid_str_ptr)
            .to_string_lossy()
            .into_owned();
        // winapi::um::winbase::LocalFree(sid_str_ptr as *mut c_void);
        Some(sid_str)
    }
}

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
        file.write_all(EAM_Task_Tools)?;
        file.sync_all()?;
        drop(file);
    }

    Ok(path.to_str().unwrap().to_string())    
}
