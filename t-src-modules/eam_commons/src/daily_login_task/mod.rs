#[cfg(target_os = "windows")]
mod windows;
#[cfg(target_os = "windows")]
pub use windows::{check_for_installed_eam_daily_login_task, install_eam_daily_login_task, uninstall_eam_daily_login_task};

#[cfg(target_os = "macos")]
mod macos;
#[cfg(target_os = "macos")]
pub use macos::{check_for_installed_eam_daily_login_task, install_eam_daily_login_task, uninstall_eam_daily_login_task};

#[cfg(not(any(target_os = "windows", target_os = "macos")))]
mod unsupported;
#[cfg(not(any(target_os = "windows", target_os = "macos")))]
pub use unsupported::{check_for_installed_eam_daily_login_task, install_eam_daily_login_task, uninstall_eam_daily_login_task};
