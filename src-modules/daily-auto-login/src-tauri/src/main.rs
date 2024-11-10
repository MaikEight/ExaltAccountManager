// Prevents additional console window on Windows in release, DO NOT REMOVE!!
#![cfg_attr(not(debug_assertions), windows_subsystem = "windows")]

extern crate dirs;

use diesel::r2d2::ConnectionManager;
use eam_commons::diesel_functions;
use eam_commons::encryption_utils;
use eam_commons::models;
use eam_commons::models::DailyLoginReportEntries;
use eam_commons::models::DailyLoginReports;
use eam_commons::models::{AuditLog, ErrorLog};
use eam_commons::rotmg_updater::FileData;
use eam_commons::rotmg_updater::UpdaterError;
use eam_commons::setup_database;
use eam_commons::DbPool;

use diesel::r2d2::Pool;
use diesel::SqliteConnection;
use lazy_static::lazy_static;
use log::{error, info};
use reqwest::header::{HeaderMap, HeaderValue, ACCEPT, CONTENT_TYPE, USER_AGENT};
use simplelog::*;
use serde_json::Value;
use tauri::http::HeaderName;
use std::collections::HashMap;
use std::env;
use std::error::Error as StdError;
use std::fs;
use std::fs::File;
use std::io::{ErrorKind, Read, Write};
use std::path::Path;
use std::path::PathBuf;
use std::str::FromStr;
use std::sync::mpsc::channel;
use std::sync::{Arc, Mutex, MutexGuard};
use std::thread;
use std::thread::sleep;
use std::time::{Duration, Instant};
use tauri::Error;
use zip::read::ZipArchive;

lazy_static! {
    static ref POOL: Arc<Mutex<Option<DbPool>>> = Arc::new(Mutex::new(None));
}

fn main() {
    //Create the save file directory if it does not exist
    let save_file_path = get_save_file_path();
    if !Path::new(&save_file_path).exists() {
        fs::create_dir_all(&save_file_path).unwrap();
    }

    // Initialize the logger
    let log_file = File::create(save_file_path + "\\log.txt").unwrap();
    CombinedLogger::init(vec![WriteLogger::new(
        LevelFilter::Info,
        Config::default(),
        log_file,
    )])
    .unwrap();

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
        Ok(mut guard) => *guard = Some(pool),
        Err(poisoned) => {
            error!("Mutex was poisoned. Recovering...");
            let mut guard = poisoned.into_inner();
            *guard = Some(pool);
        }
    }

    //Run the tauri application
   tauri::Builder::default()
   .plugin(tauri_plugin_single_instance::init(|_app, argv, _cwd| {
       println!("a new app instance was opened with {argv:?} and the deep link event was already triggered");
   }))
   .plugin(tauri_plugin_deep_link::init())
   .plugin(tauri_plugin_dialog::init())
   .plugin(tauri_plugin_process::init())
   .plugin(tauri_plugin_http::init())
   .plugin(tauri_plugin_shell::init())
   .plugin(tauri_plugin_fs::init())
   .plugin(tauri_plugin_updater::Builder::new().build())
   .invoke_handler(tauri::generate_handler![
            get_save_file_path,
            get_all_eam_groups, //EAM GROUPS
            insert_or_update_eam_group,
            check_for_game_update, //GAME UPDATER
            perform_game_update,
        ])
        .run(tauri::generate_context!("./tauri.conf.json"))
        .expect("error while running tauri application");
}

pub fn get_database_path() -> PathBuf {
    let mut path = PathBuf::from(get_save_file_path());
    path.push("exalt_account_manager.db");
    path
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
    //OS dependent fixed path
    //Windows: C:\Users\USERNAME\AppData\Local\ExaltAccountManager\v4\
    //Mac: /Users/USERNAME/Library/Application Support/ExaltAccountManager/v4/
    let mut path = dirs::data_local_dir().unwrap();
    path.push("ExaltAccountManager");
    path.push("v4");
    path.to_str().unwrap().to_string()
}

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
