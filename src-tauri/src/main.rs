//Prevents additional console window on Windows in release, DO NOT REMOVE!!
#![cfg_attr(not(debug_assertions), windows_subsystem = "windows")]

extern crate dirs;

use diesel::r2d2::ConnectionManager;
use eam_background_sync::types::SyncMode;
use eam_background_sync::BackgroundSyncManager;
use eam_commons::diesel_functions;
use eam_commons::encryption_utils;
use eam_commons::limiter::manager::RateLimiterManager;
use eam_commons::limiter::setup_global_api_limiter;
use eam_commons::models;
use eam_commons::models::CallResult;
use eam_commons::models::DailyLoginReportEntries;
use eam_commons::models::DailyLoginReports;
use eam_commons::models::{AuditLog, ErrorLog};
use eam_commons::rotmg_updater::FileData;
use eam_commons::rotmg_updater::UpdaterError;
use eam_commons::setup_database;
use eam_commons::DbPool;

use chrono::{DateTime, Utc};
use diesel::r2d2::Pool;
use diesel::SqliteConnection;
use lazy_static::lazy_static;
use log::{error, info, warn};
use reqwest::header::{HeaderMap, HeaderValue, ACCEPT, CONTENT_TYPE, USER_AGENT};
use reqwest::Method;
use serde_json::Value;
use simplelog::*;
use std::collections::HashMap;
use std::env;
use std::error::Error as StdError;
use std::fs;
use std::fs::File;
use std::io::{ErrorKind, Read, Write};
use std::path::Path;
use std::path::PathBuf;
use std::str::FromStr;
use std::sync::atomic::{AtomicBool, AtomicU64, Ordering};
use std::sync::mpsc::channel;
use std::sync::{Arc, Mutex, MutexGuard};
use std::thread;
use std::thread::sleep;
use std::time::{Duration, Instant};
use tauri::http::HeaderName;
use tauri::Error;
use tauri::{AppHandle, Emitter, Manager};
use tokio::time::interval;
use zip::read::ZipArchive;

lazy_static! {
    static ref POOL: Arc<Mutex<Option<DbPool>>> = Arc::new(Mutex::new(None));
    static ref GLOBAL_API_LIMITER: Arc<Mutex<eam_commons::limiter::RateLimiterManager>> =
        Arc::new(Mutex::new(RateLimiterManager {
            cooldown_seconds: 315,
            sub_limiters: HashMap::new(),
            cooldown_until: None,
            last_cooldown_end: None,
            limited_endpoints: vec![
                ("account/verify".to_string(), "account/verify".to_string()),
                ("char/list".to_string(), "char/list".to_string()),
                (
                    "account/register".to_string(),
                    "account/register".to_string(),
                )
            ],

            cooldown_id: Arc::new(AtomicU64::new(0)),
            cooldown_listeners: Vec::new(),
            cooldown_reset_listeners: Arc::new(Mutex::new(Vec::new())),

            api_remaining_changed_listeners: Arc::new(Mutex::new(vec![])),
            last_known_remaining: HashMap::new(),
        }));
    static ref HAS_REGISTERED_API_LIMIT_EVENT_LISTENER: Arc<Mutex<bool>> =
        Arc::new(Mutex::new(false));
    static ref HAS_REGISTERED_BACKGROUND_SYNC_EVENT_LISTENER: AtomicBool = AtomicBool::new(false);
    static ref BACKGROUND_SYNC_MANAGER: Arc<Mutex<Option<BackgroundSyncManager>>> =
        Arc::new(Mutex::new(None));
    static ref HAS_STARTED_PERIODIC_DAILY_LOGIN_CHECK: AtomicBool = AtomicBool::new(false);
}

//IMPORTANT: The file is not checked in to the repository
#[cfg(target_os = "windows")]
const EAM_SAVE_FILE_CONVERTER: &'static [u8] =
    include_bytes!("../IncludedBinaries/EAM_Save_File_Converter.exe");

fn main() {
    println!("Starting Exalt Account Manager...");
    //Create the save file directory if it does not exist
    let save_file_path = get_save_file_path();
    if !Path::new(&save_file_path).exists() {
        fs::create_dir_all(&save_file_path).unwrap();
    }
    println!("Save file path: {}", save_file_path.clone());

    // Initialize the logger
    let log_file = File::create(save_file_path + "\\log.txt").unwrap();
    CombinedLogger::init(vec![WriteLogger::new(
        LevelFilter::Info,
        Config::default(),
        log_file,
    )])
    .unwrap();

    //Print the Startup ASCII art
    info!("  _______     ___    .___  ___.");
    info!(" |   ____|   /   \\   |   \\/   |");
    info!(" |  |__     /  ^  \\  |  \\  /  |");
    info!(" |   __|   /  /_\\  \\ |  |\\/|  |");
    info!(" |  |____ /  _____  \\|  |  |  |");
    info!(" |_______/__/     \\__\\__|  |__|");
    info!("                           by MaikEight");

    //Initialize the database pool
    info!("Initialize the database pool...");
    let database_url = get_database_path().to_str().unwrap().to_string();
    info!("Database URL: {}", database_url);
    let pool = match setup_database(&database_url) {
        Ok(pool) => pool,
        Err(e) => {
            error!("Failed to setup database: {}", e);
            return;
        }
    };
    info!("Database pool initialized");

    // Handle a poisoned Mutex
    match POOL.lock() {
        Ok(mut guard) => *guard = Some(pool.clone()),
        Err(poisoned) => {
            error!("Mutex was poisoned. Recovering...");
            let mut guard = poisoned.into_inner();
            *guard = Some(pool.clone());
        }
    }

    // Setup the api-rate limiter
    info!("Setting up the global API rate limiter...");

    let pool_arc = Arc::new(POOL.clone().lock().unwrap().clone().unwrap());
    let manager = setup_global_api_limiter(pool_arc);

    match GLOBAL_API_LIMITER.lock() {
        Ok(mut guard) => *guard = manager,
        Err(poisoned) => {
            error!("Mutex was poisoned. Recovering...");
            let mut guard = poisoned.into_inner();
            *guard = manager;
        }
    }

    info!("Global API rate limiter setup complete");

    info!("Starting Tauri application...");
    //Run the tauri application
    tauri::Builder::default()
        .plugin(tauri_plugin_single_instance::init(|app, argv, cwd| {
            info!("New app instance detected with args: {:?}", argv);
            if let Some(window) = app.get_webview_window("main") {
                let _ = window.set_focus();
                let _ = window.show();
                let _ = window.unminimize();
            }
        }))
        .plugin(
            tauri_plugin_autostart::Builder::new()
                .args(["--autostart"])
                .app_name("Exalt Account Manager")
                .build(),
        )
        .plugin(tauri_plugin_deep_link::init())
        .plugin(tauri_plugin_dialog::init())
        .plugin(tauri_plugin_process::init())
        .plugin(tauri_plugin_http::init())
        .plugin(tauri_plugin_shell::init())
        .plugin(tauri_plugin_fs::init())
        .plugin(tauri_plugin_updater::Builder::new().build())
        .plugin(tauri_plugin_drpc::init())
        .invoke_handler(tauri::generate_handler![
            get_current_os,
            add_api_limit_event_listener,   // API Limiter
            create_background_sync_manager, // BackgroundSyncManager
            start_background_sync_manager,
            stop_background_sync_manager,
            is_background_sync_manager_running,
            get_current_background_sync_mode,
            change_background_sync_mode,
            open_url,
            get_save_file_path,
            combine_paths,
            start_application,
            open_folder_in_explorer,
            get_temp_folder_path,
            get_temp_folder_path_with_creation,
            create_folder,
            check_for_game_update,
            perform_game_update,
            send_get_request, // HTTP Requests
            send_get_request_with_json_body,
            send_post_request,
            send_post_request_with_form_url_encoded_data,
            send_post_request_with_json_body,
            send_patch_request_with_json_body,
            get_device_unique_identifier,
            get_os_user_identity,
            quick_hash,
            get_default_game_path,
            get_all_eam_accounts, //EAM ACCOUNTS
            get_eam_account_by_email,
            insert_or_update_eam_account,
            delete_eam_account,
            get_all_eam_groups, //EAM GROUPS
            insert_or_update_eam_group,
            delete_eam_group,
            get_latest_char_list_for_each_account, //CHAR LIST ENTRIES
            get_latest_char_list_dataset_for_each_account,
            insert_char_list_dataset,
            download_and_run_hwid_tool,
            encrypt_string,
            decrypt_string,
            has_old_eam_save_file,
            format_eam_v3_save_file_to_readable_json,
            get_all_audit_logs, //LOGS
            get_audit_log_for_account,
            log_to_audit_log,
            get_all_error_logs,
            log_to_error_log,
            delete_from_error_log,
            get_all_user_data, //USER DATA
            get_user_data_by_key,
            insert_or_update_user_data,
            delete_user_data_by_key,
            needs_to_do_daily_login, //DAILY LOGIN
            start_periodic_daily_login_check,
            get_all_daily_login_reports, //DAILY LOGIN REPORTS
            get_daily_login_reports_of_last_days,
            get_daily_login_report_by_id,
            insert_or_update_daily_login_report,
            get_all_daily_login_report_entries, //DAILY LOGIN REPORT ENTRIES
            get_daily_login_report_entry_by_id,
            get_daily_login_report_entries_by_report_id,
            insert_or_update_daily_login_report_entry,
            check_for_installed_eam_daily_login_task, //DAILY LOGIN TASK
            install_eam_daily_login_task,
            uninstall_eam_daily_login_task,
            run_eam_daily_login_task_now,
            get_current_deep_link,     // Deep link helper
            is_started_with_autostart  // Autostart helper
        ])
        .setup(|app| {
            // Handle deep links when the app starts - register all configured schemes
            #[cfg(any(windows, target_os = "linux"))]
            {
                use tauri_plugin_deep_link::DeepLinkExt;
                app.deep_link().register_all()?;
            }

            // Check if the app was started via autostart and hide window if so
            let args: Vec<String> = std::env::args().collect();
            if args.contains(&"--autostart".to_string()) {
                info!("Application started via autostart, hiding window...");
                if let Some(window) = app.get_webview_window("main") {
                    let _ = window.hide();
                }
            }

            Ok(())
        })
        .run(tauri::generate_context!("./tauri.conf.json"))
        .expect("error while running tauri application");
}

#[tauri::command]
fn get_current_os() -> String {
    info!("Getting current OS...");
    env::consts::OS.to_string()
}

//###################
//#   API Limiter   #
//###################

#[tauri::command]
async fn add_api_limit_event_listener(app: AppHandle) {
    let mut already_registered = HAS_REGISTERED_API_LIMIT_EVENT_LISTENER
        .lock()
        .unwrap_or_else(|e| {
            error!("Mutex was poisoned. Recovering...");
            e.into_inner()
        });

    if *already_registered {
        info!("API limit event listener already registered, skipping...");
        return;
    }

    *already_registered = true;
    info!("Adding API limit event listener...");

    let app = Arc::new(app);
    let app_cooldown = Arc::clone(&app);
    let app_reset = Arc::clone(&app);
    let app_remaining = Arc::clone(&app);

    let mut manager = GLOBAL_API_LIMITER.lock().unwrap();

    manager.register_cooldown_listener(move || {
        info!("API cooldown triggered, notifying app...");
        if let Err(e) = app_cooldown.emit("api-cooldown", true) {
            error!("Failed to emit cooldown event: {}", e);
        }
    });

    manager.register_cooldown_reset_listener(move || {
        info!("API cooldown reset, notifying app...");
        if let Err(e) = app_reset.emit("api-cooldown", false) {
            error!("Failed to emit cooldown reset event: {}", e);
        }
    });

    manager.register_remaining_changed_listener(move |api, remaining, limit| {
        if remaining == 0 {
            info!("API limit reached for {}, notifying app...", api);
        } else {
            info!("API limit remaining for {}: {} / {}", api, remaining, limit);
        }

        if let Err(e) =
            app_remaining.emit("api-remaining-changed", (api.to_string(), remaining, limit))
        {
            error!("Failed to emit remaining-changed event: {}", e);
        }
    });
}

//#############################
//#   BackgroundSyncManager   #
//#############################

#[tauri::command]
async fn create_background_sync_manager(app: AppHandle) -> Result<(), Error> {
    if HAS_REGISTERED_BACKGROUND_SYNC_EVENT_LISTENER.swap(true, Ordering::SeqCst) {
        info!("Background sync event listener already registered, skipping...");
        return Ok(());
    }

    info!("Creating background sync manager...");

    let pool = POOL.lock().unwrap().clone();
    if pool.is_none() {
        return Err(tauri::Error::from(std::io::Error::new(
            ErrorKind::Other,
            "Database pool not initialized",
        )));
    }

    let pool = Arc::new(pool.unwrap());
    let api_limiter = Arc::clone(&GLOBAL_API_LIMITER);

    let manager = BackgroundSyncManager::new(pool, api_limiter).await;
    *BACKGROUND_SYNC_MANAGER.lock().unwrap() = Some(manager.clone());

    manager.get_event_hub().register_listener(move |event| {
        if matches!(
            event,
            eam_background_sync::events::BackgroundSyncEvent::AccountCharListSync { .. }
        ) {
            info!("Background sync AccountCharListSync event received");
        } else {
            info!("Background sync event: {:?}", event);
        }

        // Emit the event to the frontend
        if let Err(e) = app.emit("background-sync-event", event) {
            error!("Failed to emit background sync event: {}", e);
        }
    });

    info!("Background sync manager created successfully");
    Ok(())
}

#[tauri::command]
async fn start_background_sync_manager() -> Result<(), String> {
    info!("Starting background sync manager...");

    let manager_opt = BACKGROUND_SYNC_MANAGER.lock().unwrap().clone();

    if let Some(manager) = manager_opt {
        manager.start();
        info!("Background sync manager started successfully");
        Ok(())
    } else {
        Err("Background sync manager not initialized".into())
    }
}

#[tauri::command]
fn stop_background_sync_manager() -> Result<(), Error> {
    info!("Stopping background sync manager...");

    let mut manager = BACKGROUND_SYNC_MANAGER.lock().unwrap();
    if let Some(ref mut m) = *manager {
        m.stop();
        info!("Background sync manager stopped successfully");
        Ok(())
    } else {
        Err(tauri::Error::from(std::io::Error::new(
            ErrorKind::Other,
            "Background sync manager not initialized",
        )))
    }
}

#[tauri::command]
fn is_background_sync_manager_running() -> bool {
    info!("Checking if background sync manager is running...");
    let manager = BACKGROUND_SYNC_MANAGER.lock().unwrap();
    if let Some(ref m) = *manager {
        m.is_running()
    } else {
        false
    }
}

#[tauri::command]
fn get_current_background_sync_mode() -> SyncMode {
    info!("Getting current background sync mode...");
    let manager = BACKGROUND_SYNC_MANAGER.lock().unwrap();
    if let Some(ref m) = *manager {
        m.get_current_mode()
    } else {
        SyncMode::Stopped
    }
}

#[tauri::command]
fn change_background_sync_mode(mode: SyncMode) -> Result<(), Error> {
    info!("Changing background sync mode to {:?}", mode);

    let mut manager = BACKGROUND_SYNC_MANAGER.lock().unwrap();
    if let Some(ref mut m) = *manager {
        m.switch_mode(mode);
        info!("Background sync mode changed successfully");
        Ok(())
    } else {
        Err(tauri::Error::from(std::io::Error::new(
            ErrorKind::Other,
            "Background sync manager not initialized",
        )))
    }
}

#[tauri::command]
async fn open_url(url: String) -> Result<(), Error> {
    info!("Opening URL: {}", &url);
    Ok(open::that(url)?)
}

#[tauri::command]
async fn check_for_game_update(force: bool) -> Result<bool, Error> {
    let (tx, rx) = channel();
    info!("Checking for game update...");
    thread::spawn(move || match POOL.lock() {
        Ok(pool) => {
            tx.send(check_for_game_update_impl(pool, force)).unwrap();
        }
        Err(poisoned) => {
            error!("Mutex was poisoned. Recovering...");
            let pool = poisoned.into_inner();
            tx.send(check_for_game_update_impl(pool, force)).unwrap();
        }
    });

    match rx.recv().unwrap() {
        Ok(Ok(files)) => Ok(files.len() > 0),
        Ok(Err(e)) => Err(tauri::Error::from(std::io::Error::new(
            ErrorKind::Other,
            e.to_string(),
        ))),
        Err(e) => {
            error!("Error while checking for game update: {}", e);
            Err(tauri::Error::from(std::io::Error::new(
                ErrorKind::Other,
                e.to_string(),
            )))
        }
    }
}

fn check_for_game_update_impl(
    pool: MutexGuard<Option<Pool<ConnectionManager<SqliteConnection>>>>,
    force: bool,
) -> Result<Result<Vec<FileData>, UpdaterError>, Error> {
    if let Some(ref pool) = *pool {
        let files = eam_commons::rotmg_updater::get_game_files_to_update(pool, force);
        return Ok(files);
    }

    error!("Database pool not initialized");
    return Err(UpdaterError::from(std::io::Error::new(
        ErrorKind::Other,
        "Database pool not initialized".to_string(),
    )))
    .unwrap();
}

#[tauri::command]
async fn perform_game_update() -> Result<bool, Error> {
    let (tx, rx) = channel();
    info!("Performing game update...");
    thread::spawn(move || match POOL.lock() {
        Ok(pool) => {
            tx.send(perform_game_update_impl(pool)).unwrap();
        }
        Err(poisoned) => {
            error!("Mutex was poisoned. Recovering...");
            let pool = poisoned.into_inner();
            tx.send(perform_game_update_impl(pool)).unwrap();
        }
    });

    match rx.recv().unwrap() {
        Ok(result) => Ok(result),
        Err(e) => {
            error!("Error while performing game update: {}", e);
            Err(tauri::Error::from(std::io::Error::new(
                ErrorKind::Other,
                e.to_string(),
            )))
        }
    }
}

fn perform_game_update_impl(
    pool: MutexGuard<Option<Pool<ConnectionManager<SqliteConnection>>>>,
) -> Result<bool, UpdaterError> {
    if let Some(ref pool) = *pool {
        let result = eam_commons::rotmg_updater::perform_game_update(pool);
        info!("Game update performed successfully");
        return result;
    }

    error!("Database pool not initialized");
    Err(UpdaterError::from(std::io::Error::new(
        ErrorKind::Other,
        "Database pool not initialized".to_string(),
    )))
}

#[tauri::command]
fn get_save_file_path() -> String {
    info!("Getting save file path...");
    eam_commons::paths::get_save_file_path()
}

pub fn get_database_path() -> PathBuf {
    let mut path = PathBuf::from(get_save_file_path());
    path.push("exalt_account_manager.db");
    path
}

#[tauri::command]
fn combine_paths(path1: String, path2: String) -> Result<String, Error> {
    info!("Combining paths...");
    let mut path_buf = PathBuf::from(path1);
    path_buf.push(&path2);

    Ok(path_buf.to_string_lossy().to_string())
}

#[tauri::command]
fn start_application(
    application_path: String,
    start_parameters: String,
    current_directory: Option<String>,
) -> Result<(), tauri::Error> {
    info!("Starting application...");

    //Check if the application exists
    if !Path::new(&application_path).exists() {
        return Err(tauri::Error::from(std::io::Error::new(
            ErrorKind::NotFound,
            format!("Application not found at path: {}", application_path),
        )));
    }

    match std::env::consts::OS {
        "windows" => {
            let mut cmd = std::process::Command::new(&application_path);
            cmd.arg(start_parameters);
            if let Some(dir) = &current_directory {
                cmd.current_dir(dir);
            }
            let _child = cmd.spawn().expect("Failed to start process");
        }
        "macos" => {
            let mut cmd = std::process::Command::new("open");
            cmd.arg("-a");
            cmd.arg(&application_path);
            cmd.arg("--args");
            cmd.arg(&start_parameters);
            if let Some(dir) = &current_directory {
                cmd.current_dir(dir);
            }
            let _child = cmd.spawn().expect("Failed to start process");
        }
        _ => {
            let mut cmd = std::process::Command::new(&application_path);
            cmd.arg(&start_parameters);
            if let Some(dir) = &current_directory {
                cmd.current_dir(dir);
            }
            let _child = cmd.spawn().expect("Failed to start process");
        }
    };

    Ok(())
}

#[tauri::command]
fn open_folder_in_explorer(path: String) -> Result<(), Error> {
    info!("Opening folder in explorer...");
    match std::env::consts::OS {
        "windows" => {
            let mut cmd = std::process::Command::new("explorer");
            cmd.arg(&path);
            let _child = cmd.spawn().expect("Failed to start process");
        }
        "macos" => {
            let mut cmd = std::process::Command::new("open");
            cmd.arg("-R");
            cmd.arg(&path);
            let _child = cmd.spawn().expect("Failed to start process");
        }
        _ => {
            let mut cmd = std::process::Command::new("xdg-open");
            cmd.arg(&path);
            let _child = cmd.spawn().expect("Failed to start process");
        }
    };

    Ok(())
}

fn download_file_to_ram(url: &str) -> Result<Vec<u8>, std::io::Error> {
    let response = reqwest::blocking::get(url)
        .map_err(|e| std::io::Error::new(std::io::ErrorKind::Other, e.to_string()))?;

    if !response.status().is_success() {
        return Err(std::io::Error::new(
            std::io::ErrorKind::Other,
            "Failed to download file",
        ));
    }

    Ok(response
        .bytes()
        .map_err(|e| std::io::Error::new(std::io::ErrorKind::Other, e.to_string()))?
        .to_vec())
}

#[tauri::command]
fn get_temp_folder_path_with_creation(sub_path: String) -> String {
    info!("Getting temp folder path with creation...");

    let mut path_buf = get_temp_folder_path();
    path_buf.push_str(&sub_path);
    let _ = create_folder(path_buf.clone());
    path_buf
}

#[tauri::command]
fn create_folder(path: String) -> Result<(), Error> {
    info!("Creating folder...");

    std::fs::create_dir_all(path)?;
    Ok(())
}

#[tauri::command]
fn get_temp_folder_path() -> String {
    info!("Getting temp folder path...");

    let mut path_buf = PathBuf::from(std::env::temp_dir());
    path_buf.push("ExaltAccountManager");
    path_buf.to_string_lossy().to_string()
}

#[tauri::command]
fn get_os_user_identity() -> String {
    info!("Getting user identity...");

    get_os_user_identity_impl()
}

#[cfg(target_os = "windows")]
extern crate winapi;
// #[cfg(target_os = "windows")]
// use std::os::raw::c_void;
#[cfg(target_os = "windows")]
use std::ptr::null_mut;
#[cfg(target_os = "windows")]
use winapi::shared::sddl::ConvertSidToStringSidA;
#[cfg(target_os = "windows")]
use winapi::um::handleapi::CloseHandle;
#[cfg(target_os = "windows")]
use winapi::um::processthreadsapi::OpenProcessToken;
#[cfg(target_os = "windows")]
use winapi::um::securitybaseapi::GetTokenInformation;
#[cfg(target_os = "windows")]
use winapi::um::winnt::{TokenUser, TOKEN_QUERY};

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
        winapi::um::winbase::LocalFree(sid_str_ptr as *mut winapi::ctypes::c_void);
        Some(sid_str)
    }
}

#[cfg(target_family = "windows")]
fn get_os_user_identity_impl() -> String {
    //C#: System.Security.Principal.WindowsIdentity.GetCurrent().User.Value
    //PS: Get-WmiObject win32_useraccount -Filter "name = 'Maik8'" | Select-Object sid
    let id = get_current_user_sid();
    match id {
        Some(id) => {
            let id = id.to_string();
            let id = format!("{}", id);
            id
        }
        None => "error".to_string(),
    }
}

#[cfg(target_os = "macos")]
fn get_os_user_identity_impl() -> String {
    let uid = unsafe { libc::getuid() };
    uid.to_string()
}

use md5;
use num_bigint::{BigUint, ToBigInt};

#[tauri::command]
fn quick_hash(secret: &str) -> String {
    info!("Hashing...");

    let secret_bytes = secret.as_bytes();
    let result = md5::compute(secret_bytes);
    let secret_hash = result.0;
    let reversed_hash: Vec<u8> = secret_hash.iter().rev().cloned().collect();
    let big_int = BigUint::from_bytes_le(&reversed_hash);
    let big_int = big_int.to_bigint().unwrap();
    if big_int.sign() == num_bigint::Sign::Minus {
        let two = BigUint::from(2u32);
        let max_value = two.pow(128).to_bigint().unwrap();
        let big_int = max_value + big_int;
        format!("{:x}", big_int)
    } else {
        format!("{:x}", big_int)
    }
}

//#####################
//#   HTTP Requests   #
//#####################

#[tauri::command]
async fn send_get_request(
    url: String,
    custom_headers: HashMap<String, String>,
) -> Result<String, String> {
    info!("Sending get request...");

    let mut headers = HeaderMap::new();
    custom_headers.into_iter().for_each(|(key, value)| {
        let header_name = HeaderName::from_str(&key).unwrap();
        let header_value = HeaderValue::from_str(&value).unwrap();
        headers.append(header_name, header_value);
    });

    headers.insert(USER_AGENT, HeaderValue::from_static("ExaltAccountManager"));

    let client = reqwest::Client::new();
    let res = client
        .get(&url)
        .headers(headers)
        .send()
        .await
        .map_err(|e| e.to_string())?;

    let body = res.text().await.map_err(|e| e.to_string())?;
    Ok(body)
}

#[tauri::command]
async fn send_get_request_with_json_body(
    url: String,
    data: String,
    custom_headers: HashMap<String, String>,
) -> Result<String, String> {
    info!("Sending get request with json body...");

    let mut headers = HeaderMap::new();
    custom_headers.into_iter().for_each(|(key, value)| {
        let header_name = HeaderName::from_str(&key).unwrap();
        let header_value = HeaderValue::from_str(&value).unwrap();
        headers.append(header_name, header_value);
    });

    headers.insert(USER_AGENT, HeaderValue::from_static("ExaltAccountManager"));
    // Ensure JSON body is recognized
    headers.insert(CONTENT_TYPE, HeaderValue::from_static("application/json"));

    let client = reqwest::Client::new();
    let res = client
        .request(Method::GET, &url)
        .headers(headers)
        .body(data)
        .send()
        .await
        .map_err(|e| e.to_string())?;

    let body = res.text().await.map_err(|e| e.to_string())?;
    Ok(body)
}

#[tauri::command]
async fn send_post_request(
    url: String,
    custom_headers: HashMap<String, String>,
    body: Value,
) -> Result<String, String> {
    info!("Sending post request...");

    let mut headers = HeaderMap::new();
    custom_headers.into_iter().for_each(|(key, value)| {
        let header_name = HeaderName::from_str(&key).unwrap();
        let header_value = HeaderValue::from_str(&value).unwrap();
        headers.append(header_name, header_value);
    });

    headers.insert(USER_AGENT, HeaderValue::from_static("ExaltAccountManager"));

    let client = reqwest::Client::new();
    let res = client
        .post(&url)
        .headers(headers)
        .json(&body)
        .send()
        .await
        .map_err(|e| e.to_string())?;

    let body = res.text().await.map_err(|e| e.to_string())?;
    Ok(body)
}

#[tauri::command]
async fn send_post_request_with_form_url_encoded_data(
    url: String,
    data: HashMap<String, String>,
) -> Result<String, String> {
    info!("Sending post request with form url encoded data...");
    // First: get the limiter key before awaiting anything
    let limiter_key_opt = {
        let manager = GLOBAL_API_LIMITER.lock().unwrap();
        manager.get_limiter_key_from_url(&url)
    };

    if let Some(ref key) = limiter_key_opt {
        let mut manager = GLOBAL_API_LIMITER.lock().unwrap();
        if !manager.can_call(key) {
            info!("Rate limit exceeded for {}", key);
            manager.record_api_use(key, CallResult::RateLimited);
            return Err(format!("Error: Rate limit exceeded for {}", key));
        }
    }

    // Send request AFTER dropping any manager locks
    let mut headers = HeaderMap::new();
    headers.insert(ACCEPT, HeaderValue::from_static("deflate, gzip"));
    headers.insert(
        CONTENT_TYPE,
        HeaderValue::from_static("application/x-www-form-urlencoded"),
    );

    let client = reqwest::Client::new();
    let result = client.post(&url).headers(headers).form(&data).send().await;

    // Check result and re-acquire lock if needed
    match result {
        Ok(res) => {
            let body = res.text().await.map_err(|e| e.to_string())?;
            if let Some(ref key) = limiter_key_opt {
                let mut manager = GLOBAL_API_LIMITER.lock().unwrap();

                if RateLimiterManager::is_rate_limited_response(&body) {
                    manager.record_api_use(key, CallResult::RateLimited);
                    manager.trigger_cooldown();
                    info!("RotMG API rate limit hit: global cooldown triggered.");
                    return Err("RotMG API rate limit hit: global cooldown triggered.".into());
                }
                manager.record_api_use(key, CallResult::Success);
            }

            Ok(body)
        }
        Err(e) => {
            if let Some(ref key) = limiter_key_opt {
                let mut manager = GLOBAL_API_LIMITER.lock().unwrap();
                manager.record_api_use(key, CallResult::Failed);
            }
            error!("Failed to send post request: {}", e);
            Err(e.to_string())
        }
    }
}

#[tauri::command]
async fn send_post_request_with_json_body(
    url: String,
    data: String,
    headers_opt: Option<HashMap<String, String>>,
) -> Result<String, String> {
    info!("Sending post request with json body to: {}", &url);

    let mut headers = HeaderMap::new();
    headers.insert(ACCEPT, HeaderValue::from_static("application/json"));
    headers.insert(USER_AGENT, HeaderValue::from_static("ExaltAccountManager"));
    headers.insert(CONTENT_TYPE, HeaderValue::from_static("application/json"));

    if let Some(custom_headers) = headers_opt {
        custom_headers.into_iter().for_each(|(key, value)| {
            let header_name = HeaderName::from_str(&key).unwrap();
            let header_value = HeaderValue::from_str(&value).unwrap();
            headers.append(header_name, header_value);
        });
    }

    let client = reqwest::Client::new();
    let res = client
        .post(&url)
        .headers(headers)
        .body(data)
        .send()
        .await
        .map_err(|e| e.to_string())?;

    let body = res.text().await.map_err(|e| e.to_string())?;
    Ok(body)
}

#[tauri::command]
async fn send_patch_request_with_json_body(url: String, data: String) -> Result<String, String> {
    info!("Sending patch request with json body...");

    let mut headers = HeaderMap::new();
    headers.insert(ACCEPT, HeaderValue::from_static("application/json"));
    headers.insert(USER_AGENT, HeaderValue::from_static("ExaltAccountManager"));
    headers.insert(CONTENT_TYPE, HeaderValue::from_static("application/json"));

    let client = reqwest::Client::new();
    let res = client
        .patch(&url)
        .headers(headers)
        .body(data)
        .send()
        .await
        .map_err(|e| e.to_string())?;

    let body = res.text().await.map_err(|e| e.to_string())?;
    Ok(body)
}

#[tauri::command]
async fn get_device_unique_identifier() -> Result<String, String> {
    info!("Getting device unique identifier...");

    let device_id = eam_commons::hwid::get_device_unique_identifier().await;
    match device_id {
        Ok(id) => Ok(id),
        Err(e) => Err(e),
    }
}

#[tauri::command]
fn get_default_game_path() -> String {
    eam_commons::paths::get_default_game_path()
}

#[tauri::command]
async fn download_and_run_hwid_tool() -> Result<bool, String> {
    info!("Downloading and running HWID tool...");
    let result = download_and_run_hwid_tool_impl().await;

    match result {
        Ok(result) => Ok(result),
        Err(e) => Err(e),
    }
}

async fn download_and_run_hwid_tool_impl() -> Result<bool, String> {
    let hwid_tool_url_windows = "https://github.com/MaikEight/EAM-GetClientHWID/releases/download/v1.1.0/EAM-GetClientHWID-windows.zip";
    // Mac version not available, as it is not required
    let hwid_tool_path = get_save_file_path();

    let hwid_tool_url = match std::env::consts::OS {
        "windows" => hwid_tool_url_windows,
        _ => return Err("Unsupported OS".to_string()),
    };
    println!("Downloading HWID tool from: {}", hwid_tool_url);

    let (tx, rx) = channel();

    thread::spawn(move || {
        let hwid_tool_data = download_file_to_ram(hwid_tool_url).map_err(|e| e.to_string());
        tx.send(hwid_tool_data).unwrap();
    });

    let hwid_tool_data = rx.recv().unwrap();

    println!("Downloaded HWID tool");
    unzip_data_to_path(hwid_tool_data?, Path::new(&hwid_tool_path)).map_err(|e| e.to_string())?;
    println!("Unzipped HWID tool");
    let save_file_path = get_save_file_path();

    let hwid_tool_executable = match std::env::consts::OS {
        "windows" => "EAM-GetClientHWID\\EAM-GetClientHWID.exe",
        "macos" => "EAM-GetClientHWID-mac", //TODO: Correct file name once known
        _ => return Err("Unsupported OS".to_string()),
    };

    let mut hwid_tool_path = PathBuf::from(&hwid_tool_path);
    hwid_tool_path.push(hwid_tool_executable);

    let mut file_path = PathBuf::from(&save_file_path);
    file_path.push("EAM.HWID");
    let file_path_str = file_path
        .to_str()
        .expect("Failed to convert path to string")
        .to_owned();

    let file_path_str_clone = file_path_str.clone();
    //If the EAM.HWID file already exists, delete it
    let path = PathBuf::from(&file_path);
    println!(
        "Deleting old EAM.HWID file if it exists: {}",
        &file_path.clone().to_str().unwrap()
    );

    if path.exists() {
        println!("Old EAM.HWID file exists, deleting it");
        fs::remove_file(file_path_str.clone()).map_err(|e| e.to_string())?;
    }

    println!("Starting HWID tool");
    let hwid_tool_path_str = hwid_tool_path
        .to_str()
        .ok_or("Failed to convert path to string".to_string())?
        .to_string();
    start_application(
        hwid_tool_path_str,
        format!("-batchmode {{{}}}", save_file_path),
        None,
    )
    .map_err(|e| e.to_string())?;

    println!("Waiting for EAM.HWID file to be created");
    let file_created = wait_for_file_creation(&file_path_str_clone, 60);

    //Delete the hwid-tool
    hwid_tool_path.pop(); //Remove the executable name
    println!(
        "Deleting HWID tool: {}",
        &hwid_tool_path.clone().to_str().unwrap()
    );
    let _ = fs::remove_dir_all(&hwid_tool_path);

    Ok(file_created)
}

fn unzip_data_to_path(data: Vec<u8>, output_path: &Path) -> Result<(), Box<dyn StdError>> {
    let reader = std::io::Cursor::new(data);
    let mut archive = ZipArchive::new(reader)?;

    for i in 0..archive.len() {
        let mut file = archive.by_index(i)?;
        let outpath = output_path.join(file.mangled_name());

        if (&*file.name()).ends_with('/') {
            std::fs::create_dir_all(&outpath)?;
        } else {
            if let Some(p) = outpath.parent() {
                if !p.exists() {
                    std::fs::create_dir_all(&p)?;
                }
            }
            let mut outfile = File::create(&outpath)?;
            std::io::copy(&mut file, &mut outfile)?;
        }
    }

    Ok(())
}

fn wait_for_file_creation(file_path: &str, timeout: u64) -> bool {
    let start = Instant::now();
    while start.elapsed().as_secs() < timeout {
        if fs::metadata(file_path).is_ok() {
            return true;
        }
        sleep(Duration::from_secs(1));
    }
    false
}

#[cfg(target_os = "macos")]
#[tauri::command]
fn install_eam_daily_login_task() -> Result<bool, tauri::Error> {
    info!("This function is only available on Windows");
    Ok(false)
}

#[cfg(target_os = "windows")]
#[tauri::command]
fn install_eam_daily_login_task() -> Result<bool, tauri::Error> {
    info!("Installing EAM daily login task...");

    if std::env::consts::OS != "windows" {
        warn!("This function is only available on Windows");
        return Err(tauri::Error::from(std::io::Error::new(
            ErrorKind::Other,
            "This function is only available on Windows",
        )));
    }

    let result = eam_commons::windows_specifics::install_eam_daily_login_task(
        "explorer.exe",
        Some("eam:start-daily-login-task"),
    );

    match result {
        Ok(_) => Ok(true),
        Err(e) => {
            error!("Failed to install EAM daily login task: {}", e.to_string());
            Err(tauri::Error::from(std::io::Error::new(
                ErrorKind::Other,
                e.to_string(),
            )))
        }
    }
}

#[tauri::command]
#[cfg(target_os = "macos")]
fn uninstall_eam_daily_login_task(uninstall_old_versions: bool) -> Result<bool, String> {
    info!("This function is only available on Windows");
    Ok(false)
}

#[tauri::command]
#[cfg(target_os = "windows")]
fn uninstall_eam_daily_login_task(uninstall_old_versions: bool) -> Result<bool, String> {
    if std::env::consts::OS != "windows" {
        info!("This function is only available on Windows");
        return Err("This function is only available on Windows".to_string());
    }

    let result =
        eam_commons::windows_specifics::uninstall_eam_daily_login_task(uninstall_old_versions);

    match result {
        Ok(_) => Ok(true),
        Err(e) => {
            error!(
                "Failed to uninstall EAM daily login task: {}",
                e.to_string()
            );
            Err(e.to_string())
        }
    }
}

#[tauri::command]
#[cfg(target_os = "macos")]
fn check_for_installed_eam_daily_login_task(check_for_old_versions: bool) -> Result<bool, String> {
    info!("This function is only available on Windows");
    Ok(false)
}

#[tauri::command]
#[cfg(target_os = "windows")]
fn check_for_installed_eam_daily_login_task(check_for_old_versions: bool) -> Result<bool, String> {
    if std::env::consts::OS != "windows" {
        info!("This function is only available on Windows");
        return Ok(false);
    }

    let result = eam_commons::windows_specifics::check_for_installed_eam_daily_login_task(
        check_for_old_versions,
    );

    match result {
        Ok(result) => Ok(result),
        Err(e) => Err(e.to_string()),
    }
}

#[tauri::command]
fn run_eam_daily_login_task_now() -> Result<bool, String> {
    info!("Running EAM daily login task now...");

    if std::env::consts::OS != "windows" {
        info!("This function is only available on Windows");
        return Err("This function is only available on Windows".to_string());
    }

    let save_file_path = get_save_file_path();
    let path = Path::new(&save_file_path).join("EAM_Daily_Auto_Login.exe");

    if !path.exists() {
        error!("EAM_Daily_Auto_Login.exe does not exist");
        return Err("EAM_Daily_Auto_Login.exe does not exist".to_string());
    }

    let mut cmd = std::process::Command::new(&path);
    cmd.arg("-force");
    let _child = cmd.spawn().expect("Failed to start process");

    Ok(true)
}

//########################
//#       UserData       #
//########################

#[tauri::command]
async fn get_all_user_data() -> Result<Vec<models::UserData>, tauri::Error> {
    info!("Getting all user data...");

    match POOL.lock() {
        Ok(pool) => get_all_user_data_impl(pool),
        Err(poisoned) => {
            error!("Mutex was poisoned. Recovering...");
            let pool = poisoned.into_inner();
            return get_all_user_data_impl(pool);
        }
    }
}

fn get_all_user_data_impl(
    pool: MutexGuard<Option<Pool<ConnectionManager<SqliteConnection>>>>,
) -> Result<Vec<models::UserData>, tauri::Error> {
    if let Some(ref pool) = *pool {
        return diesel_functions::get_all_user_data(pool)
            .map_err(|e| tauri::Error::from(std::io::Error::new(ErrorKind::Other, e.to_string())));
    }

    error!("Database pool not initialized");
    Err(tauri::Error::from(std::io::Error::new(
        ErrorKind::Other,
        "Pool is not initialized",
    )))
}

#[tauri::command]
async fn get_user_data_by_key(key: String) -> Result<models::UserData, tauri::Error> {
    info!("Getting user data by key...");
    match POOL.lock() {
        Ok(pool) => get_user_data_by_key_impl(pool, key),
        Err(poisoned) => {
            error!("Mutex was poisoned. Recovering...");
            let pool = poisoned.into_inner();
            return get_user_data_by_key_impl(pool, key);
        }
    }
}

fn get_user_data_by_key_impl(
    pool: MutexGuard<Option<Pool<ConnectionManager<SqliteConnection>>>>,
    key: String,
) -> Result<models::UserData, tauri::Error> {
    if let Some(ref pool) = *pool {
        return diesel_functions::get_user_data_by_key(pool, key)
            .map_err(|e| tauri::Error::from(std::io::Error::new(ErrorKind::Other, e.to_string())));
    }

    error!("Database pool not initialized");
    Err(tauri::Error::from(std::io::Error::new(
        ErrorKind::Other,
        "Pool is not initialized",
    )))
}

#[tauri::command]
async fn insert_or_update_user_data(user_data: models::UserData) -> Result<usize, tauri::Error> {
    info!("Inserting or updating user data...");

    match POOL.lock() {
        Ok(pool) => insert_or_update_user_data_impl(pool, user_data),
        Err(poisoned) => {
            error!("Mutex was poisoned. Recovering...");
            let pool = poisoned.into_inner();
            return insert_or_update_user_data_impl(pool, user_data);
        }
    }
}

fn insert_or_update_user_data_impl(
    pool: MutexGuard<Option<Pool<ConnectionManager<SqliteConnection>>>>,
    user_data: models::UserData,
) -> Result<usize, tauri::Error> {
    if let Some(ref pool) = *pool {
        return diesel_functions::insert_or_update_user_data(pool, user_data)
            .map_err(|e| tauri::Error::from(std::io::Error::new(ErrorKind::Other, e.to_string())));
    }

    error!("Database pool not initialized");
    Err(tauri::Error::from(std::io::Error::new(
        ErrorKind::Other,
        "Pool is not initialized",
    )))
}

#[tauri::command]
async fn delete_user_data_by_key(key: String) -> Result<usize, tauri::Error> {
    info!("Deleting user data by key...");

    match POOL.lock() {
        Ok(pool) => delete_user_data_by_key_impl(pool, key),
        Err(poisoned) => {
            error!("Mutex was poisoned. Recovering...");
            let pool = poisoned.into_inner();
            return delete_user_data_by_key_impl(pool, key);
        }
    }
}

fn delete_user_data_by_key_impl(
    pool: MutexGuard<Option<Pool<ConnectionManager<SqliteConnection>>>>,
    key: String,
) -> Result<usize, tauri::Error> {
    if let Some(ref pool) = *pool {
        return diesel_functions::delete_user_data_by_key(pool, key)
            .map_err(|e| tauri::Error::from(std::io::Error::new(ErrorKind::Other, e.to_string())));
    }

    error!("Database pool not initialized");
    Err(tauri::Error::from(std::io::Error::new(
        ErrorKind::Other,
        "Pool is not initialized",
    )))
}

//##################
//#   DailyLogin   #
//##################

#[tauri::command]
async fn needs_to_do_daily_login() -> Result<bool, tauri::Error> {
    info!("Checking if daily login is needed...");

    match POOL.lock() {
        Ok(pool) => needs_to_do_daily_login_impl(pool),
        Err(poisoned) => {
            error!("Mutex was poisoned. Recovering...");
            let pool = poisoned.into_inner();
            return needs_to_do_daily_login_impl(pool);
        }
    }
}

fn needs_to_do_daily_login_impl(
    pool: MutexGuard<Option<Pool<ConnectionManager<SqliteConnection>>>>,
) -> Result<bool, tauri::Error> {
    if let Some(ref pool) = *pool {
        let daily_login_report_ret = diesel_functions::get_latest_daily_login(pool);

        if daily_login_report_ret.is_ok() {
            let daily_login_report = daily_login_report_ret.unwrap();

            let daily_login_report_time = daily_login_report.startTime.clone().unwrap();
            let daily_login_report_time =
                DateTime::parse_from_rfc3339(&daily_login_report_time).unwrap();
            let daily_login_report_time = daily_login_report_time.with_timezone(&Utc);

            if daily_login_report_time.date_naive() == Utc::now().date_naive() {
                if daily_login_report.hasFinished {
                    if daily_login_report.amountOfAccountsProcessed
                        == daily_login_report.amountOfAccounts
                    {
                        return Ok(false); // Daily login already done today
                    }

                    if daily_login_report.emailsToProcess != None {
                        return Ok(true);
                    }
                }

                return Ok(true);
            }
        }

        return Ok(true); // No daily login report found, so daily login is needed
    }

    error!("Database pool not initialized");
    Err(tauri::Error::from(std::io::Error::new(
        ErrorKind::Other,
        "Pool is not initialized",
    )))
}

#[derive(serde::Serialize, serde::Deserialize, Debug, Clone)]
struct StartDailyLoginData {
    id: uuid::Uuid,
    timestamp: String,
}

#[tauri::command]
async fn start_periodic_daily_login_check(app: AppHandle) {
    if HAS_STARTED_PERIODIC_DAILY_LOGIN_CHECK.load(Ordering::SeqCst) {
        info!("Periodic daily login check already started, skipping...");
        return;
    }
    HAS_STARTED_PERIODIC_DAILY_LOGIN_CHECK.store(true, Ordering::SeqCst);

    info!("Starting periodic daily login check...");

    // Spawn the periodic check in a separate tokio task
    tokio::spawn(async move {
        let mut interval = interval(Duration::from_secs(15 * 60)); // 15 minutes

        loop {
            interval.tick().await;

            info!("Checking if daily login is needed...");

            let needs_to_do_daily_login_res;

            match POOL.lock() {
                Ok(pool) => {
                    needs_to_do_daily_login_res = needs_to_do_daily_login_impl(pool);
                }
                Err(poisoned) => {
                    error!("Mutex was poisoned. Recovering...");
                    let pool = poisoned.into_inner();
                    needs_to_do_daily_login_res = needs_to_do_daily_login_impl(pool);
                }
            }

            if needs_to_do_daily_login_res.is_err() {
                error!(
                    "Failed to check if daily login is needed: {}",
                    needs_to_do_daily_login_res.unwrap_err()
                );
                continue;
            }

            if needs_to_do_daily_login_res.unwrap() {
                info!("Daily login is needed, starting daily login process...");
                app.emit(
                    "start-daily-login-process",
                    StartDailyLoginData {
                        id: uuid::Uuid::new_v4(),
                        timestamp: Utc::now().to_rfc3339(),
                    },
                )
                .unwrap_or_else(|e| {
                    error!("Failed to emit start-daily-login-process event: {}", e);
                });
            }
        }
    });
}

//#########################
//#   DailyLoginReports   #
//#########################

#[tauri::command]
async fn get_all_daily_login_reports() -> Result<Vec<DailyLoginReports>, tauri::Error> {
    info!("Getting all daily login reports...");

    match POOL.lock() {
        Ok(pool) => get_all_daily_login_reports_impl(pool),
        Err(poisoned) => {
            error!("Mutex was poisoned. Recovering...");
            let pool = poisoned.into_inner();
            return get_all_daily_login_reports_impl(pool);
        }
    }
}

fn get_all_daily_login_reports_impl(
    pool: MutexGuard<Option<Pool<ConnectionManager<SqliteConnection>>>>,
) -> Result<Vec<DailyLoginReports>, tauri::Error> {
    if let Some(ref pool) = *pool {
        return diesel_functions::get_all_daily_login_reports(pool)
            .map_err(|e| tauri::Error::from(std::io::Error::new(ErrorKind::Other, e.to_string())));
    }

    error!("Database pool not initialized");
    Err(tauri::Error::from(std::io::Error::new(
        ErrorKind::Other,
        "Pool is not initialized",
    )))
}

#[tauri::command]
async fn get_daily_login_reports_of_last_days(
    amount_of_days: i64,
) -> Result<Vec<DailyLoginReports>, tauri::Error> {
    info!("Getting daily login reports of last days...");

    match POOL.lock() {
        Ok(pool) => get_daily_login_reports_of_last_days_impl(pool, amount_of_days),
        Err(poisoned) => {
            error!("Mutex was poisoned. Recovering...");
            let pool = poisoned.into_inner();
            return get_daily_login_reports_of_last_days_impl(pool, amount_of_days);
        }
    }
}

fn get_daily_login_reports_of_last_days_impl(
    pool: MutexGuard<Option<Pool<ConnectionManager<SqliteConnection>>>>,
    amount_of_days: i64,
) -> Result<Vec<DailyLoginReports>, tauri::Error> {
    if let Some(ref pool) = *pool {
        return diesel_functions::get_daily_login_reports_of_last_days(pool, amount_of_days)
            .map_err(|e| tauri::Error::from(std::io::Error::new(ErrorKind::Other, e.to_string())));
    }

    error!("Database pool not initialized");
    Err(tauri::Error::from(std::io::Error::new(
        ErrorKind::Other,
        "Pool is not initialized",
    )))
}

#[tauri::command]
async fn get_daily_login_report_by_id(
    report_id: String,
) -> Result<DailyLoginReports, tauri::Error> {
    info!("Getting daily login report by id...");

    match POOL.lock() {
        Ok(pool) => get_daily_login_report_by_id_impl(pool, report_id),
        Err(poisoned) => {
            error!("Mutex was poisoned. Recovering...");
            let pool = poisoned.into_inner();
            return get_daily_login_report_by_id_impl(pool, report_id);
        }
    }
}

fn get_daily_login_report_by_id_impl(
    pool: MutexGuard<Option<Pool<ConnectionManager<SqliteConnection>>>>,
    report_id: String,
) -> Result<DailyLoginReports, tauri::Error> {
    if let Some(ref pool) = *pool {
        return diesel_functions::get_daily_login_report_by_id(pool, report_id)
            .map_err(|e| tauri::Error::from(std::io::Error::new(ErrorKind::Other, e.to_string())));
    }

    error!("Database pool not initialized");
    Err(tauri::Error::from(std::io::Error::new(
        ErrorKind::Other,
        "Pool is not initialized",
    )))
}

#[tauri::command]
async fn insert_or_update_daily_login_report(
    daily_login_report: DailyLoginReports,
) -> Result<usize, tauri::Error> {
    info!("Inserting or updating daily login report...");

    match POOL.lock() {
        Ok(pool) => insert_or_update_daily_login_report_impl(pool, daily_login_report),
        Err(poisoned) => {
            error!("Mutex was poisoned. Recovering...");
            let pool = poisoned.into_inner();
            return insert_or_update_daily_login_report_impl(pool, daily_login_report);
        }
    }
}

fn insert_or_update_daily_login_report_impl(
    pool: MutexGuard<Option<Pool<ConnectionManager<SqliteConnection>>>>,
    daily_login_report: DailyLoginReports,
) -> Result<usize, tauri::Error> {
    if let Some(ref pool) = *pool {
        return diesel_functions::insert_or_update_daily_login_report(pool, daily_login_report)
            .map_err(|e| tauri::Error::from(std::io::Error::new(ErrorKind::Other, e.to_string())));
    }

    error!("Database pool not initialized");
    Err(tauri::Error::from(std::io::Error::new(
        ErrorKind::Other,
        "Pool is not initialized",
    )))
}

// #############################
// #  DailyLoginReportEntries  #
// #############################

#[tauri::command]
async fn get_all_daily_login_report_entries() -> Result<Vec<DailyLoginReportEntries>, tauri::Error>
{
    info!("Getting all daily login report entries...");

    match POOL.lock() {
        Ok(pool) => get_all_daily_login_report_entries_impl(pool),
        Err(poisoned) => {
            error!("Mutex was poisoned. Recovering...");
            let pool = poisoned.into_inner();
            return get_all_daily_login_report_entries_impl(pool);
        }
    }
}

fn get_all_daily_login_report_entries_impl(
    pool: MutexGuard<Option<Pool<ConnectionManager<SqliteConnection>>>>,
) -> Result<Vec<DailyLoginReportEntries>, tauri::Error> {
    if let Some(ref pool) = *pool {
        return diesel_functions::get_all_daily_login_report_entries(pool)
            .map_err(|e| tauri::Error::from(std::io::Error::new(ErrorKind::Other, e.to_string())));
    }

    error!("Database pool not initialized");
    Err(tauri::Error::from(std::io::Error::new(
        ErrorKind::Other,
        "Pool is not initialized",
    )))
}

#[tauri::command]
async fn get_daily_login_report_entry_by_id(
    report_id: i32,
) -> Result<DailyLoginReportEntries, tauri::Error> {
    info!("Getting daily login report entry by id...");

    match POOL.lock() {
        Ok(pool) => get_daily_login_report_entry_by_id_impl(pool, report_id),
        Err(poisoned) => {
            error!("Mutex was poisoned. Recovering...");
            let pool = poisoned.into_inner();
            return get_daily_login_report_entry_by_id_impl(pool, report_id);
        }
    }
}

fn get_daily_login_report_entry_by_id_impl(
    pool: MutexGuard<Option<Pool<ConnectionManager<SqliteConnection>>>>,
    report_id: i32,
) -> Result<DailyLoginReportEntries, tauri::Error> {
    if let Some(ref pool) = *pool {
        return diesel_functions::get_daily_login_report_entry_by_id(pool, Some(report_id))
            .map_err(|e| tauri::Error::from(std::io::Error::new(ErrorKind::Other, e.to_string())));
    }

    error!("Database pool not initialized");
    Err(tauri::Error::from(std::io::Error::new(
        ErrorKind::Other,
        "Pool is not initialized",
    )))
}

#[tauri::command]
async fn get_daily_login_report_entries_by_report_id(
    report_id: String,
) -> Result<Vec<DailyLoginReportEntries>, tauri::Error> {
    info!("Getting daily login report entries by report id...");

    match POOL.lock() {
        Ok(pool) => get_daily_login_report_entries_by_report_id_impl(pool, report_id),
        Err(poisoned) => {
            error!("Mutex was poisoned. Recovering...");
            let pool = poisoned.into_inner();
            return get_daily_login_report_entries_by_report_id_impl(pool, report_id);
        }
    }
}

fn get_daily_login_report_entries_by_report_id_impl(
    pool: MutexGuard<Option<Pool<ConnectionManager<SqliteConnection>>>>,
    report_id: String,
) -> Result<Vec<DailyLoginReportEntries>, tauri::Error> {
    if let Some(ref pool) = *pool {
        return diesel_functions::get_daily_login_report_entries_by_report_id(pool, report_id)
            .map_err(|e| tauri::Error::from(std::io::Error::new(ErrorKind::Other, e.to_string())));
    }

    error!("Database pool not initialized");
    Err(tauri::Error::from(std::io::Error::new(
        ErrorKind::Other,
        "Pool is not initialized",
    )))
}

#[tauri::command]
async fn insert_or_update_daily_login_report_entry(
    daily_login_report_entry: DailyLoginReportEntries,
) -> Result<usize, tauri::Error> {
    info!("Inserting or updating daily login report entry...");

    match POOL.lock() {
        Ok(pool) => insert_or_update_daily_login_report_entry_impl(pool, daily_login_report_entry),
        Err(poisoned) => {
            error!("Mutex was poisoned. Recovering...");
            let pool = poisoned.into_inner();
            return insert_or_update_daily_login_report_entry_impl(pool, daily_login_report_entry);
        }
    }
}

fn insert_or_update_daily_login_report_entry_impl(
    pool: MutexGuard<Option<Pool<ConnectionManager<SqliteConnection>>>>,
    daily_login_report_entry: DailyLoginReportEntries,
) -> Result<usize, tauri::Error> {
    if let Some(ref pool) = *pool {
        return diesel_functions::insert_or_update_daily_login_report_entry(
            pool,
            daily_login_report_entry,
        )
        .map(|i| i as usize)
        .map_err(|e| tauri::Error::from(std::io::Error::new(ErrorKind::Other, e.to_string())));
    }

    error!("Database pool not initialized");
    Err(tauri::Error::from(std::io::Error::new(
        ErrorKind::Other,
        "Pool is not initialized",
    )))
}

//########################
//#      EamAccount      #
//########################

#[tauri::command]
async fn get_all_eam_accounts() -> Result<Vec<models::EamAccount>, tauri::Error> {
    info!("Getting all EAM accounts...");

    match POOL.lock() {
        Ok(pool) => get_all_eam_accounts_impl(pool),
        Err(poisoned) => {
            error!("Mutex was poisoned. Recovering...");
            let pool = poisoned.into_inner();
            return get_all_eam_accounts_impl(pool);
        }
    }
}

fn get_all_eam_accounts_impl(
    pool: MutexGuard<Option<Pool<ConnectionManager<SqliteConnection>>>>,
) -> Result<Vec<models::EamAccount>, tauri::Error> {
    if let Some(ref pool) = *pool {
        return diesel_functions::get_all_eam_accounts(pool)
            .map_err(|e| tauri::Error::from(std::io::Error::new(ErrorKind::Other, e.to_string())));
    }

    error!("Database pool not initialized");
    Err(tauri::Error::from(std::io::Error::new(
        ErrorKind::Other,
        "Pool is not initialized",
    )))
}

#[tauri::command]
async fn get_eam_account_by_email(
    account_email: String,
) -> Result<models::EamAccount, tauri::Error> {
    info!("Getting EAM account by email...");

    match POOL.lock() {
        Ok(pool) => get_eam_account_by_email_impl(pool, account_email),
        Err(poisoned) => {
            error!("Mutex was poisoned. Recovering...");
            let pool = poisoned.into_inner();
            return get_eam_account_by_email_impl(pool, account_email);
        }
    }
}

fn get_eam_account_by_email_impl(
    pool: MutexGuard<Option<Pool<ConnectionManager<SqliteConnection>>>>,
    account_email: String,
) -> Result<models::EamAccount, tauri::Error> {
    if let Some(ref pool) = *pool {
        return diesel_functions::get_eam_account_by_email(pool, account_email)
            .map_err(|e| tauri::Error::from(std::io::Error::new(ErrorKind::Other, e.to_string())));
    }

    error!("Database pool not initialized");
    Err(tauri::Error::from(std::io::Error::new(
        ErrorKind::Other,
        "Pool is not initialized",
    )))
}

#[tauri::command]
async fn insert_or_update_eam_account(
    eam_account: models::EamAccount,
) -> Result<usize, tauri::Error> {
    info!("Inserting or updating EAM account...");

    match POOL.lock() {
        Ok(pool) => insert_or_update_eam_account_impl(pool, eam_account),
        Err(poisoned) => {
            error!("Mutex was poisoned. Recovering...");
            let pool = poisoned.into_inner();
            return insert_or_update_eam_account_impl(pool, eam_account);
        }
    }
}

fn insert_or_update_eam_account_impl(
    pool: MutexGuard<Option<Pool<ConnectionManager<SqliteConnection>>>>,
    eam_account: models::EamAccount,
) -> Result<usize, tauri::Error> {
    if let Some(ref pool) = *pool {
        return diesel_functions::insert_or_update_eam_account(pool, eam_account)
            .map_err(|e| tauri::Error::from(std::io::Error::new(ErrorKind::Other, e.to_string())));
    }

    error!("Database pool not initialized");
    Err(tauri::Error::from(std::io::Error::new(
        ErrorKind::Other,
        "Pool is not initialized",
    )))
}

#[tauri::command]
async fn delete_eam_account(account_email: String) -> Result<usize, tauri::Error> {
    info!("Deleting EAM account...");

    match POOL.lock() {
        Ok(pool) => delete_eam_account_impl(pool, account_email),
        Err(poisoned) => {
            error!("Mutex was poisoned. Recovering...");
            let pool = poisoned.into_inner();
            return delete_eam_account_impl(pool, account_email);
        }
    }
}

fn delete_eam_account_impl(
    pool: MutexGuard<Option<Pool<ConnectionManager<SqliteConnection>>>>,
    account_email: String,
) -> Result<usize, tauri::Error> {
    if let Some(ref pool) = *pool {
        let audit_log_entry = AuditLog {
            id: None,
            time: "".to_string(),
            accountEmail: Some(account_email.clone()),
            message: ("Deleting account from database: ".to_owned() + &account_email).to_string(),
            sender: "tauri".to_string(),
        };
        let _ = diesel_functions::insert_audit_log(pool, audit_log_entry);

        let email_hash = hash_email(&account_email);
        let _ = encryption_utils::delete_data(&email_hash);

        return diesel_functions::delete_eam_account(pool, account_email)
            .map_err(|e| tauri::Error::from(std::io::Error::new(ErrorKind::Other, e.to_string())));
    }

    error!("Database pool not initialized");
    Err(tauri::Error::from(std::io::Error::new(
        ErrorKind::Other,
        "Pool is not initialized",
    )))
}

#[tauri::command]
fn encrypt_string(email: String, data: String) -> Result<String, tauri::Error> {
    info!("Encrypting string...");

    let mut email = email;
    if std::env::consts::OS != "windows" {
        email = hash_email(&email);
    }

    let encrypted_data = encryption_utils::encrypt_data(&email, &data)
        .map_err(|e| tauri::Error::from(std::io::Error::new(ErrorKind::Other, e.to_string())))?;
    Ok(encrypted_data)
}

fn hash_email(email: &str) -> String {
    use sha1::{Digest, Sha1};
    let mut hasher = Sha1::new();
    hasher.update(email.as_bytes());
    let email_hash = hasher.finalize();
    format!("{:x}", email_hash)
}

#[tauri::command]
fn decrypt_string(data: String) -> Result<String, tauri::Error> {
    info!("Decrypting string...");

    let decrypted_data = encryption_utils::decrypt_data(&data)
        .map_err(|e| tauri::Error::from(std::io::Error::new(ErrorKind::Other, e.to_string())))?;
    Ok(decrypted_data)
}

//########################
//#       EamGroup       #
//########################

#[tauri::command]
async fn get_all_eam_groups() -> Result<Vec<models::EamGroup>, tauri::Error> {
    info!("Getting all EAM groups...");

    match POOL.lock() {
        Ok(pool) => get_all_eam_groups_impl(pool),
        Err(poisoned) => {
            error!("Mutex was poisoned. Recovering...");
            let pool = poisoned.into_inner();
            return get_all_eam_groups_impl(pool);
        }
    }
}

fn get_all_eam_groups_impl(
    pool: MutexGuard<Option<Pool<ConnectionManager<SqliteConnection>>>>,
) -> Result<Vec<models::EamGroup>, tauri::Error> {
    if let Some(ref pool) = *pool {
        return diesel_functions::get_all_eam_groups(pool)
            .map_err(|e| tauri::Error::from(std::io::Error::new(ErrorKind::Other, e.to_string())));
    }

    error!("Database pool not initialized");
    Err(tauri::Error::from(std::io::Error::new(
        ErrorKind::Other,
        "Pool is not initialized",
    )))
}

#[tauri::command]
async fn insert_or_update_eam_group(eam_group: models::EamGroup) -> Result<usize, tauri::Error> {
    info!("Inserting or updating EAM group...");

    match POOL.lock() {
        Ok(pool) => insert_or_update_eam_group_impl(pool, eam_group),
        Err(poisoned) => {
            error!("Mutex was poisoned. Recovering...");
            let pool = poisoned.into_inner();
            return insert_or_update_eam_group_impl(pool, eam_group);
        }
    }
}

fn insert_or_update_eam_group_impl(
    pool: MutexGuard<Option<Pool<ConnectionManager<SqliteConnection>>>>,
    eam_group: models::EamGroup,
) -> Result<usize, tauri::Error> {
    if let Some(ref pool) = *pool {
        return diesel_functions::insert_or_update_eam_group(pool, eam_group)
            .map_err(|e| tauri::Error::from(std::io::Error::new(ErrorKind::Other, e.to_string())));
    }

    error!("Database pool not initialized");
    Err(tauri::Error::from(std::io::Error::new(
        ErrorKind::Other,
        "Pool is not initialized",
    )))
}

#[tauri::command]
async fn delete_eam_group(group_id: i32) -> Result<usize, tauri::Error> {
    info!("Deleting EAM group...");

    match POOL.lock() {
        Ok(pool) => delete_eam_group_impl(pool, group_id),
        Err(poisoned) => {
            error!("Mutex was poisoned. Recovering...");
            let pool = poisoned.into_inner();
            return delete_eam_group_impl(pool, group_id);
        }
    }
}

fn delete_eam_group_impl(
    pool: MutexGuard<Option<Pool<ConnectionManager<SqliteConnection>>>>,
    group_id: i32,
) -> Result<usize, tauri::Error> {
    if let Some(ref pool) = *pool {
        return diesel_functions::delete_eam_group(pool, group_id)
            .map_err(|e| tauri::Error::from(std::io::Error::new(ErrorKind::Other, e.to_string())));
    }

    error!("Database pool not initialized");
    Err(tauri::Error::from(std::io::Error::new(
        ErrorKind::Other,
        "Pool is not initialized",
    )))
}

#[tauri::command]
async fn has_old_eam_save_file() -> Result<bool, tauri::Error> {
    info!("Checking for old EAM save file...");

    if std::env::consts::OS != "windows" {
        return Ok(false);
    }

    let save_file_path = get_save_file_path();
    //remove the last folder from the save file path (cd ..)
    let old_save_file_path = Path::new(&save_file_path)
        .parent()
        .unwrap()
        .join("EAM.accounts");
    Ok(old_save_file_path.exists())
}

#[tauri::command]
#[cfg(target_os = "macos")]
async fn format_eam_v3_save_file_to_readable_json() -> Result<String, tauri::Error> {
    info!("Formatting of EAM v3 save file was called, but is not available on macOS.");
    Err(tauri::Error::from(std::io::Error::new(
        ErrorKind::Other,
        "This function is only available on Windows",
    )))
}

#[tauri::command]
#[cfg(target_os = "windows")]
async fn format_eam_v3_save_file_to_readable_json() -> Result<String, tauri::Error> {
    info!("Formatting EAM v3 save file to readable JSON...");

    if std::env::consts::OS != "windows" {
        return Err(tauri::Error::from(std::io::Error::new(
            ErrorKind::Other,
            "This function is only available on Windows",
        )));
    }

    let save_file_path = get_save_file_path();
    let embedded_file_path = Path::new(&save_file_path).join("EAM_Save_File_Converter.exe");
    let mut file = File::create(embedded_file_path.clone())
        .map_err(|e| tauri::Error::from(std::io::Error::new(ErrorKind::Other, e.to_string())))?;
    file.write_all(EAM_SAVE_FILE_CONVERTER)
        .map_err(|e| tauri::Error::from(std::io::Error::new(ErrorKind::Other, e.to_string())))?;
    drop(file);

    let process = std::process::Command::new(embedded_file_path.clone())
        .current_dir(save_file_path.clone())
        .output()
        .map_err(|e| tauri::Error::from(std::io::Error::new(ErrorKind::Other, e.to_string())))?;

    if process.status.success() {
        let output_file_path = Path::new(&save_file_path).join("accountsV3.json");
        let mut file = File::open(output_file_path.clone()).map_err(|e| {
            tauri::Error::from(std::io::Error::new(ErrorKind::Other, e.to_string()))
        })?;
        let mut content = String::new();
        file.read_to_string(&mut content).map_err(|e| {
            tauri::Error::from(std::io::Error::new(ErrorKind::Other, e.to_string()))
        })?;
        fs::remove_file(embedded_file_path).map_err(|e| {
            tauri::Error::from(std::io::Error::new(ErrorKind::Other, e.to_string()))
        })?;
        fs::remove_file(output_file_path).map_err(|e| {
            tauri::Error::from(std::io::Error::new(ErrorKind::Other, e.to_string()))
        })?;
        Ok(content)
    } else {
        let error_message = format!(
            "EAM_Save_File_Converter.exe failed with exit code: {}",
            process.status.code().unwrap_or(100)
        );
        fs::remove_file(embedded_file_path).map_err(|e| {
            tauri::Error::from(std::io::Error::new(ErrorKind::Other, e.to_string()))
        })?;

        let log = ErrorLog {
            id: None,
            time: "".to_string(),
            sender: "Tauri".to_string(),
            message: error_message.clone(),
        };
        let _ = log_to_error_log(log);

        error!("{}", error_message);
        Err(tauri::Error::from(std::io::Error::new(
            ErrorKind::Other,
            error_message,
        )))
    }
}

//#########################
//#   char_list_dataset   #
//#########################

#[tauri::command]
async fn get_latest_char_list_dataset_for_each_account(
) -> Result<Vec<models::CharListDataset>, tauri::Error> {
    info!("Getting latest char list dataset for each account...");

    match POOL.lock() {
        Ok(pool) => get_latest_char_list_dataset_for_each_account_impl(pool),
        Err(poisoned) => {
            error!("Mutex was poisoned. Recovering...");
            let pool = poisoned.into_inner();
            return get_latest_char_list_dataset_for_each_account_impl(pool);
        }
    }
}

fn get_latest_char_list_dataset_for_each_account_impl(
    pool: MutexGuard<Option<Pool<ConnectionManager<SqliteConnection>>>>,
) -> Result<Vec<models::CharListDataset>, tauri::Error> {
    if let Some(ref pool) = *pool {
        return diesel_functions::get_latest_char_list_dataset_for_each_account(pool)
            .map_err(|e| tauri::Error::from(std::io::Error::new(ErrorKind::Other, e.to_string())));
    }

    error!("Database pool not initialized");
    Err(tauri::Error::from(std::io::Error::new(
        ErrorKind::Other,
        "Pool is not initialized",
    )))
}

#[tauri::command]
async fn get_latest_char_list_for_each_account(
) -> Result<Vec<models::CharListEntries>, tauri::Error> {
    info!("Getting latest char list for each account...");

    match POOL.lock() {
        Ok(pool) => get_latest_char_list_for_each_account_impl(pool),
        Err(poisoned) => {
            error!("Mutex was poisoned. Recovering...");
            let pool = poisoned.into_inner();
            return get_latest_char_list_for_each_account_impl(pool);
        }
    }
}

fn get_latest_char_list_for_each_account_impl(
    pool: MutexGuard<Option<Pool<ConnectionManager<SqliteConnection>>>>,
) -> Result<Vec<models::CharListEntries>, tauri::Error> {
    if let Some(ref pool) = *pool {
        return diesel_functions::get_latest_char_list_for_each_account(pool)
            .map_err(|e| tauri::Error::from(std::io::Error::new(ErrorKind::Other, e.to_string())));
    }

    error!("Database pool not initialized");
    Err(tauri::Error::from(std::io::Error::new(
        ErrorKind::Other,
        "Pool is not initialized",
    )))
}

#[tauri::command]
async fn insert_char_list_dataset(dataset: models::CharListDataset) -> Result<usize, tauri::Error> {
    info!("Inserting char list dataset...");

    match POOL.lock() {
        Ok(pool) => insert_char_list_dataset_impl(pool, dataset),
        Err(poisoned) => {
            error!("Mutex was poisoned. Recovering...");
            let pool = poisoned.into_inner();
            return insert_char_list_dataset_impl(pool, dataset);
        }
    }
}

fn insert_char_list_dataset_impl(
    pool: MutexGuard<Option<Pool<ConnectionManager<SqliteConnection>>>>,
    dataset: models::CharListDataset,
) -> Result<usize, tauri::Error> {
    if let Some(ref pool) = *pool {
        return diesel_functions::insert_char_list_dataset(pool, dataset)
            .map_err(|e| tauri::Error::from(std::io::Error::new(ErrorKind::Other, e.to_string())));
    }

    error!("Database pool not initialized");
    Err(tauri::Error::from(std::io::Error::new(
        ErrorKind::Other,
        "Pool is not initialized",
    )))
}

//#########################
//#         Logs          #
//#########################

#[tauri::command]
fn get_all_audit_logs() -> Result<Vec<AuditLog>, tauri::Error> {
    info!("Getting all audit logs...");

    match POOL.lock() {
        Ok(pool) => get_all_audit_logs_impl(pool),
        Err(poisoned) => {
            error!("Mutex was poisoned. Recovering...");
            let pool = poisoned.into_inner();
            return get_all_audit_logs_impl(pool);
        }
    }
}

fn get_all_audit_logs_impl(
    pool: MutexGuard<Option<Pool<ConnectionManager<SqliteConnection>>>>,
) -> Result<Vec<AuditLog>, tauri::Error> {
    if let Some(ref pool) = *pool {
        return diesel_functions::get_all_audit_logs(pool)
            .map_err(|e| tauri::Error::from(std::io::Error::new(ErrorKind::Other, e.to_string())));
    }

    error!("Database pool not initialized");
    Err(tauri::Error::from(std::io::Error::new(
        ErrorKind::Other,
        "Pool is not initialized",
    )))
}

#[tauri::command]
fn get_audit_log_for_account(account_email: String) -> Result<Vec<AuditLog>, tauri::Error> {
    info!("Getting audit log for account...");

    match POOL.lock() {
        Ok(pool) => get_audit_log_for_account_impl(pool, account_email),
        Err(poisoned) => {
            error!("Mutex was poisoned. Recovering...");
            let pool = poisoned.into_inner();
            return get_audit_log_for_account_impl(pool, account_email);
        }
    }
}

fn get_audit_log_for_account_impl(
    pool: MutexGuard<Option<Pool<ConnectionManager<SqliteConnection>>>>,
    account_email: String,
) -> Result<Vec<AuditLog>, tauri::Error> {
    if let Some(ref pool) = *pool {
        return diesel_functions::get_audit_log_for_account(pool, account_email)
            .map_err(|e| tauri::Error::from(std::io::Error::new(ErrorKind::Other, e.to_string())));
    }

    error!("Database pool not initialized");
    Err(tauri::Error::from(std::io::Error::new(
        ErrorKind::Other,
        "Pool is not initialized",
    )))
}

#[tauri::command]
fn log_to_audit_log(log: AuditLog) -> Result<usize, tauri::Error> {
    info!("Logging to audit log...");

    match POOL.lock() {
        Ok(pool) => log_to_audit_log_impl(pool, log),
        Err(poisoned) => {
            error!("Mutex was poisoned. Recovering...");
            let pool = poisoned.into_inner();
            return log_to_audit_log_impl(pool, log);
        }
    }
}

fn log_to_audit_log_impl(
    pool: MutexGuard<Option<Pool<ConnectionManager<SqliteConnection>>>>,
    log: AuditLog,
) -> Result<usize, tauri::Error> {
    if let Some(ref pool) = *pool {
        return diesel_functions::insert_audit_log(pool, log)
            .map_err(|e| tauri::Error::from(std::io::Error::new(ErrorKind::Other, e.to_string())));
    }

    error!("Database pool not initialized");
    Err(tauri::Error::from(std::io::Error::new(
        ErrorKind::Other,
        "Pool is not initialized",
    )))
}

#[tauri::command]
fn get_all_error_logs() -> Result<Vec<ErrorLog>, tauri::Error> {
    info!("Getting all error logs...");

    match POOL.lock() {
        Ok(pool) => get_all_error_logs_impl(pool),
        Err(poisoned) => {
            error!("Mutex was poisoned. Recovering...");
            let pool = poisoned.into_inner();
            return get_all_error_logs_impl(pool);
        }
    }
}

fn get_all_error_logs_impl(
    pool: MutexGuard<Option<Pool<ConnectionManager<SqliteConnection>>>>,
) -> Result<Vec<ErrorLog>, tauri::Error> {
    if let Some(ref pool) = *pool {
        return diesel_functions::get_all_error_logs(pool)
            .map_err(|e| tauri::Error::from(std::io::Error::new(ErrorKind::Other, e.to_string())));
    }

    error!("Database pool not initialized");
    Err(tauri::Error::from(std::io::Error::new(
        ErrorKind::Other,
        "Pool is not initialized",
    )))
}

#[tauri::command]
fn log_to_error_log(log: ErrorLog) -> Result<usize, tauri::Error> {
    info!("Logging to error log...");

    match POOL.lock() {
        Ok(pool) => log_to_error_log_impl(pool, log),
        Err(poisoned) => {
            error!("Mutex was poisoned. Recovering...");
            let pool = poisoned.into_inner();
            return log_to_error_log_impl(pool, log);
        }
    }
}

fn log_to_error_log_impl(
    pool: MutexGuard<Option<Pool<ConnectionManager<SqliteConnection>>>>,
    log: ErrorLog,
) -> Result<usize, tauri::Error> {
    if let Some(ref pool) = *pool {
        return diesel_functions::insert_error_log(pool, log)
            .map_err(|e| tauri::Error::from(std::io::Error::new(ErrorKind::Other, e.to_string())));
    }

    error!("Database pool not initialized");
    Err(tauri::Error::from(std::io::Error::new(
        ErrorKind::Other,
        "Pool is not initialized",
    )))
}

#[tauri::command]
fn delete_from_error_log(days: i64) -> Result<usize, tauri::Error> {
    info!("Deleting from error log...");

    match POOL.lock() {
        Ok(pool) => delete_from_error_log_impl(pool, days),
        Err(poisoned) => {
            error!("Mutex was poisoned. Recovering...");
            let pool = poisoned.into_inner();
            return delete_from_error_log_impl(pool, days);
        }
    }
}

fn delete_from_error_log_impl(
    pool: MutexGuard<Option<Pool<ConnectionManager<SqliteConnection>>>>,
    days: i64,
) -> Result<usize, tauri::Error> {
    if let Some(ref pool) = *pool {
        return diesel_functions::delete_error_logs_older_than_days(pool, days)
            .map_err(|e| tauri::Error::from(std::io::Error::new(ErrorKind::Other, e.to_string())));
    }

    error!("Database pool not initialized");
    Err(tauri::Error::from(std::io::Error::new(
        ErrorKind::Other,
        "Pool is not initialized",
    )))
}

//########################
//#      Deep Links      #
//########################

#[tauri::command]
async fn get_current_deep_link(app: AppHandle) -> Result<Option<String>, tauri::Error> {
    info!("Getting current deep link...");

    #[cfg(desktop)]
    {
        use tauri_plugin_deep_link::DeepLinkExt;
        match app.deep_link().get_current() {
            Ok(Some(urls)) => {
                if urls.is_empty() {
                    Ok(None)
                } else {
                    Ok(Some(urls[0].to_string()))
                }
            }
            Ok(None) => Ok(None),
            Err(e) => {
                error!("Failed to get current deep link: {}", e);
                Ok(None)
            }
        }
    }

    #[cfg(not(desktop))]
    Ok(None)
}

#[tauri::command]
fn is_started_with_autostart() -> bool {
    info!("Checking if started with autostart...");
    let args: Vec<String> = std::env::args().collect();
    args.contains(&"--autostart".to_string())
}
