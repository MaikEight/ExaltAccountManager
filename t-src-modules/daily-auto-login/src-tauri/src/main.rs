// Prevents additional console window on Windows in release, DO NOT REMOVE!!
#![cfg_attr(not(debug_assertions), windows_subsystem = "windows")]

extern crate dirs;

use eam_plus_lib;
use eam_plus_lib::daily_login::daily_login::DailyLoginError::NotAPlusUserError;
use eam_plus_lib::daily_login::daily_login::GameAccessToken;
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
use eam_commons::get_latest_daily_login;
use eam_commons::get_all_eam_accounts_for_daily_login;
use eam_commons::insert_or_update_daily_login_report;

use base64::prelude::*;
use diesel::r2d2::Pool;
use diesel::SqliteConnection;
use lazy_static::lazy_static;
use log::{error, info};
use reqwest::header::{HeaderMap, HeaderValue, ACCEPT, CONTENT_TYPE, USER_AGENT};
use simplelog::*;
use serde_json::Value;
use tauri::http::HeaderName;
use std::process::Command;
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
use chrono::{DateTime, Utc};
use uuid::Uuid;

lazy_static! {
    static ref POOL: Arc<Mutex<Option<DbPool>>> = Arc::new(Mutex::new(None));
}
const GAME_START_TIMEOUT: u64 = 90;

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

    log_to_audit_log_internal("Starting daily_auto_login.".to_string(), None);
    let is_valid_run = is_valid_run();

    if is_valid_run.is_err() {
        let _ = log_to_error_log(ErrorLog {
            id: None,
            sender: "daily_auto_login".to_string(),
            message: "Failed to check if run is valid".to_string(),
            time: "".to_string(),
        });
        println!("Failed to check if run is valid, exiting.");
        return;
    }

    if !is_valid_run.unwrap() {
        //if is debug mode

        let is_dev: bool = tauri::is_dev();
        if !is_dev {
            println!("Not in debug mode, exiting.");
            return;
        } 

        println!("...but we are in debug mode, so we continue.");
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
            open_url,
            get_save_file_path,
            get_daily_login_report, //DAILY LOGIN REPORT
            get_all_eam_accounts, //EAM ACCOUNTS
            get_eam_account_by_email,
            get_all_eam_groups, //EAM GROUPS
            insert_or_update_eam_group,
            check_for_game_update, //GAME UPDATER
            perform_game_update,
            encrypt_string, //ENCRYPTION
            decrypt_string, 
            combine_paths, //UTILS
            is_plus_user,
            send_get_request, //REQUESTS
            send_post_request, 
            get_all_user_data, //USER DATA
            get_user_data_by_key,
            insert_or_update_user_data,
            delete_user_data_by_key,
            log_to_audit_log, //LOGS
            log_to_error_log
        ])
        .run(tauri::generate_context!("./tauri.conf.json"))
        .expect("error while running tauri application");
}

pub fn is_valid_run() -> Result<bool, tauri::Error> {
    match POOL.lock() {
        Ok(pool) => is_valid_run_impl(pool),
        Err(poisoned) => {
            error!("Mutex was poisoned. Recovering...");
            let pool = poisoned.into_inner();
            return is_valid_run_impl(pool);
        }
    }
}

fn is_valid_run_impl(pool: MutexGuard<Option<Pool<ConnectionManager<SqliteConnection>>>>) -> Result<bool, tauri::Error> {
    if let Some(ref pool) = *pool {
        let daily_login_report_ret = get_latest_daily_login(pool);
        let mut daily_login_report: DailyLoginReports;
        let mut accounts_to_perform_daily_login_with = get_all_eam_accounts_for_daily_login(pool).unwrap();

        if daily_login_report_ret.is_ok() {
            daily_login_report = daily_login_report_ret.unwrap();
            let daily_login_report_time = daily_login_report.startTime.clone().unwrap();
            let daily_login_report_time = DateTime::parse_from_rfc3339(&daily_login_report_time).unwrap();
            let daily_login_report_time = daily_login_report_time.with_timezone(&Utc);

            if daily_login_report_time.date_naive() == Utc::now().date_naive() {
                let args: Vec<String> = std::env::args().collect();
                let mut force_run = false;
                let mut is_force_run = false;
                match args.len() {
                    1 => {}
                    _ => {
                        for arg in &args[1..] {
                            if arg == "-force" {
                                force_run = true;
                            }
                        }
                    }
                }
                if daily_login_report.hasFinished {                
                    if daily_login_report.amountOfAccountsProcessed == daily_login_report.amountOfAccounts {  
                        if !force_run {
                            println!("Todays daily login did already run successfully, exiting.");
                            log_to_audit_log_internal_with_pool(pool, "Todays daily login did already run successfully, exiting.".to_string(), None);
                            return Ok(false);
                        } 

                        println!("Forcing daily login to run again.");
                        log_to_audit_log_internal_with_pool(pool, "Forcing daily login to run again.".to_string(), None);
                        is_force_run = true;
                        return Ok(true);
                    } else if daily_login_report.emailsToProcess != None {
                        println!("Last daily login did not finish processing all accounts, continuing...");
                        log_to_audit_log_internal_with_pool(pool, "Last daily login did not finish processing all accounts, continuing...".to_string(), None);
                        return Ok(true);                    
                    } else {
                        if !force_run {
                            println!("Last daily login did finish, exiting.");
                            log_to_audit_log_internal_with_pool(pool, "Last daily login did finish, exiting.".to_string(), None);
                            return Ok(false);
                        } 

                        println!("Forcing daily login to run again.");
                        log_to_audit_log_internal_with_pool(pool, "Forcing daily login to run again.".to_string(), None);
                        is_force_run = true;
                        return Ok(true);
                    }
                } else {
                    println!("Last daily login did not finish, continuing...");
                    log_to_audit_log_internal_with_pool(pool, "Last daily login did not finish, continuing...".to_string(), None);
                    return Ok(true);
                }

                let accounts_email_list = daily_login_report.emailsToProcess.clone().unwrap();
                let accounts_email_list = accounts_email_list.split(", ");
                accounts_to_perform_daily_login_with = Vec::new();
                for account_email in accounts_email_list {
                    let acc = diesel_functions::get_eam_account_by_email(pool, account_email.to_string()).unwrap();
                    accounts_to_perform_daily_login_with.push(acc);
                }

                if accounts_to_perform_daily_login_with.len() == 0 {
                    println!("No accounts to perform daily login with.");
                    log_to_audit_log_internal_with_pool(pool, "No accounts to perform daily login with, exiting.".to_string(), None);
                    return Ok(false);
                }

                return Ok(true);
            } 
            return Ok(true);        
        }

        if accounts_to_perform_daily_login_with.len() == 0 {
            println!("No accounts to perform daily login with.");
            log_to_audit_log_internal_with_pool(pool, "No accounts to perform daily login with, exiting.".to_string(), None);
            return Ok(false);
        }        

        return Ok(true);
    }

    error!("Database pool not initialized");
    Err(tauri::Error::from(std::io::Error::new(
        ErrorKind::Other,
        "Pool is not initialized",
    )))
}

#[tauri::command]
async fn open_url(url: String) -> Result<(), Error> {
    info!("Opening URL: {}", &url);
    Ok(open::that(url)?)
}

#[tauri::command]
async fn get_daily_login_report() -> Result<DailyLoginReports, tauri::Error> {
    match POOL.lock() {
        Ok(pool) => get_daily_login_report_impl(pool),
        Err(poisoned) => {
            error!("Mutex was poisoned. Recovering...");
            let pool = poisoned.into_inner();
            return get_daily_login_report_impl(pool);
        }
    }
}

fn get_daily_login_report_impl(pool: MutexGuard<Option<Pool<ConnectionManager<SqliteConnection>>>>) -> Result<DailyLoginReports, tauri::Error> {
    if let Some(ref pool) = *pool {
        let daily_login_report_ret = get_latest_daily_login(pool);
        let mut daily_login_report: DailyLoginReports;
        let mut accounts_to_perform_daily_login_with = get_all_eam_accounts_for_daily_login(pool).unwrap();

        if daily_login_report_ret.is_ok() {
            daily_login_report = daily_login_report_ret.unwrap();
            let daily_login_report_time = daily_login_report.startTime.clone().unwrap();
            let daily_login_report_time = DateTime::parse_from_rfc3339(&daily_login_report_time).unwrap();
            let daily_login_report_time = daily_login_report_time.with_timezone(&Utc);

            if daily_login_report_time.date_naive() == Utc::now().date_naive() {
                let args: Vec<String> = std::env::args().collect();             

                let accounts_email_list = daily_login_report.emailsToProcess.clone().unwrap();
                let accounts_email_list = accounts_email_list.split(", ");
                accounts_to_perform_daily_login_with = Vec::new();

                for account_email in accounts_email_list {
                    match diesel_functions::get_eam_account_by_email(pool, account_email.to_string()) {
                        Ok(acc) => accounts_to_perform_daily_login_with.push(acc),
                        Err(e) => {
                            eprintln!("Could not find account: {:?}", e);
                        }
                    }
                }
                return Ok(daily_login_report);
            }

            let report_uuid = Uuid::new_v4().to_string();
            daily_login_report = DailyLoginReports {
                id: report_uuid,
                startTime: Some(Utc::now().to_rfc3339()),
                endTime: None,
                hasFinished: false,
                emailsToProcess: Some(accounts_to_perform_daily_login_with.iter().map(|acc| acc.email.clone()).collect::<Vec<String>>().join(", ")),
                amountOfAccounts: accounts_to_perform_daily_login_with.len() as i32,
                amountOfAccountsProcessed: 0,
                amountOfAccountsFailed: 0,
                amountOfAccountsSucceeded: 0,
            };
            let _ = insert_or_update_daily_login_report(pool, daily_login_report.clone());
            return Ok(daily_login_report);             
        }
    
        let report_uuid = Uuid::new_v4().to_string();
        daily_login_report = DailyLoginReports {
            id: report_uuid,
            startTime: Some(Utc::now().to_rfc3339()),
            endTime: None,
            hasFinished: false,
            emailsToProcess: Some(accounts_to_perform_daily_login_with.iter().map(|acc| acc.email.clone()).collect::<Vec<String>>().join(", ")),
            amountOfAccounts: accounts_to_perform_daily_login_with.len() as i32,
            amountOfAccountsProcessed: 0,
            amountOfAccountsFailed: 0,
            amountOfAccountsSucceeded: 0,
        };
        let _ = insert_or_update_daily_login_report(pool, daily_login_report.clone());
        return Ok(daily_login_report);
    }

    error!("Database pool not initialized");
    Err(tauri::Error::from(std::io::Error::new(
        ErrorKind::Other,
        "Pool is not initialized",
    )))
}


pub fn get_database_path() -> PathBuf {
    let mut path = PathBuf::from(get_save_file_path());
    path.push("exalt_account_manager.db");
    path
}

//#########################
//#      Daily Login      #
//#########################

struct DailyLoginResult {
    account_email: String,
    char_list_xml: String,
    access_token: Option<GameAccessToken>,
    used_plus: bool,
}

#[tauri::command]
async fn perform_daily_login(
    id_token: Option<String>,
    email: String,
    hwid: String
) -> Result<DailyLoginResult, Error> {
    // let (tx, rx) = channel();
    // info!("Performing daily login for account: {} ...", email);
    // thread::spawn(move || match POOL.lock() {
    //     Ok(pool) => {
    //         tx.send(perform_daily_login_impl(pool, id_token, email, hwid)).unwrap().await;
    //     }
    //     Err(poisoned) => {
    //         error!("Mutex was poisoned. Recovering...");
    //         let pool = poisoned.into_inner();
    //         tx.send(perform_daily_login_impl(pool, email, id_token, email, hwid)).unwrap().await;
    //     }
    // });

    // match rx.recv().unwrap() {
    //     Ok(r) => Ok(r),
    //     Err(e) => {
    //         error!("Error while performing daily login: {}", e);
    //         Err(tauri::Error::from(std::io::Error::new(
    //             ErrorKind::Other,
    //             e.to_string(),
    //         )))
    //     }
    // }

    info!("Performing daily login for account: {} ...", email);
    match POOL.lock() {
        Ok(pool) => perform_daily_login_impl(pool, id_token, email, hwid).await,
        Err(poisoned) => {
            error!("Mutex was poisoned. Recovering...");
            let pool = poisoned.into_inner();
            return perform_daily_login_impl(pool, id_token, email, hwid).await;
        }
    }
}

async fn perform_daily_login_impl(
    pool: MutexGuard<'_,Option<Pool<ConnectionManager<SqliteConnection>>>>,
    id_token: Option<String>,
    email: String,
    hwid: String
) -> Result<DailyLoginResult, tauri::Error> {
    if let Some(ref pool) = *pool {
        // Check if id_token exists, if so, use the eam plus login method
        if id_token.is_some() {
            let id_token = id_token.unwrap();
            let result = eam_plus_lib::daily_login::daily_login::perform_daily_login(id_token, email.clone(), hwid.clone(), pool).await;      
            match result {
                Ok(result) => {
                    if result.success {
                        info!("🟢 Daily login performed for account: {}", email.clone());
                        log_to_audit_log_internal_with_pool(
                            pool,
                            ("Daily login performed for account: ".to_owned() + &email).to_string(),
                            Some(email.clone()),
                        );
                    } else {
                        error!("🔴 Failed to perform daily login for account: {}", email.clone());
                        log_to_audit_log_internal_with_pool(
                            pool,
                            ("Failed to perform daily login for account: ".to_owned() + &email).to_string(),
                            Some(email.clone()),
                        );
                    }
                    return Ok(
                        DailyLoginResult {
                            account_email: email.clone(),
                            char_list_xml: result.char_list,
                            access_token: None,
                            used_plus: true
                        }
                    );
                }
                Err(eam_plus_lib::daily_login::daily_login::DailyLoginError::NotAPlusUserError(_)) => {
                    error!("🔴 User is not a plus user");
                    // user normal login method
                }
                Err(eam_plus_lib::daily_login::daily_login::DailyLoginError::FailedToGetAccessToken(_)) => {
                    error!("🔴 Failed to get access token for account: {}", email.clone());
                    log_to_audit_log_internal_with_pool(
                        pool,
                        ("Failed to get access token for account:".to_owned() + &email).to_string(),
                        Some(email.clone()),
                    );
                    return Err(tauri::Error::from(std::io::Error::new(
                        ErrorKind::Other,
                        ("Failed to get access token for account:".to_owned() + &email).to_string(),
                    )));
                }
                Err(e) => {
                    return Err(tauri::Error::from(std::io::Error::new(
                        ErrorKind::Other,
                        e.to_string(),
                    )));
                }
            }
        }

        // Normal login method
        let access_token_opt = eam_plus_lib::daily_login::daily_login::send_account_verify_request(pool, email.clone(), hwid).await;
        match access_token_opt {
            Some(ref token) => {
                let xml_output = eam_plus_lib::daily_login::daily_login::send_char_list_request(token.clone(), email.clone(), pool).await;
                info!("🟢 Daily login performed for account: {}", email.clone());
                log_to_audit_log_internal_with_pool(
                    pool,
                    ("Daily login performed for account: ".to_owned() + &email).to_string(),
                    Some(email.clone()),
                );
                return Ok(DailyLoginResult {
                    account_email: email.clone(),
                    char_list_xml: xml_output,
                    access_token: access_token_opt,
                    used_plus: false
                });
            }
            None => {
                error!("🔴 Failed to get access token for account: {}", email.clone());
                log_to_audit_log_internal_with_pool(
                    pool,
                    ("Failed to get access token for account: ".to_owned() + &email).to_string(),
                    Some(email.clone()),
                );
                return Err(tauri::Error::from(std::io::Error::new(
                    ErrorKind::Other,
                    "Failed to get access token for account: ".to_owned() + &email,
                )))
            }
        }
    }

    error!("Database pool not initialized");
    Err(tauri::Error::from(std::io::Error::new(
        ErrorKind::Other,
        "Pool is not initialized",
    ))) 
}

#[tauri::command]
async fn perform_daily_login_game_run(
    email: String,
    access_token: GameAccessToken
) -> Result<(), Error> {
    info!("Performing daily login game run...");
    match POOL.lock() {
        Ok(pool) => perform_daily_login_game_run_impl(pool, email, access_token).await,
        Err(poisoned) => {
            error!("Mutex was poisoned. Recovering...");
            let pool = poisoned.into_inner();
            return perform_daily_login_game_run_impl(pool, email, access_token).await;
        }
    }
}

async fn perform_daily_login_game_run_impl(
    pool: MutexGuard<'_, Option<Pool<ConnectionManager<SqliteConnection>>>>,
    email: String, 
    access_token: GameAccessToken
) -> Result<(), Error> {
    //Get the game path from the database
    
    if let Some(ref pool) = *pool {        
        let game_exe_path = diesel_functions::get_user_data_by_key(pool, "game_exe_path".to_string())
            .map_err(|e| tauri::Error::from(std::io::Error::new(ErrorKind::Other, e.to_string())));
        let game_exe_path = game_exe_path.unwrap().dataValue;

        if game_exe_path.is_empty() {
            println!("No game.exe path file found, exiting.");
            log_to_audit_log_internal_with_pool(pool, "No game.exe file found, exiting.".to_string(), None);
            return Err(tauri::Error::from(std::io::Error::new(
                ErrorKind::Other,
                "No game.exe file found, exiting.",
            )));
        }

        let args = format!(
            "data:{{platform:Deca,guid:{},token:{},tokenTimestamp:{},tokenExpiration:{},env:4,serverName:{}}}",
            BASE64_STANDARD.encode(&email.clone()),
            BASE64_STANDARD.encode(access_token.access_token),
            BASE64_STANDARD.encode(access_token.access_token_timestamp),
            BASE64_STANDARD.encode(access_token.access_token_expiration),
            "".to_string()
        );

        //Start the game with the args
        let mut child = Command::new(game_exe_path.clone())
            .arg("-batchmode")
            .arg(args)
            .spawn()
            .expect("Failed to start the game.");   

        //Wait for the game to automatically login
        thread::sleep(Duration::from_secs(GAME_START_TIMEOUT));

        //Close the game
        child.kill().expect("Failed to close the game.");
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

#[tauri::command]
fn combine_paths(path1: String, path2: String) -> Result<String, Error> {
    info!("Combining paths...");
    let mut path_buf = PathBuf::from(path1);
    path_buf.push(&path2);

    Ok(path_buf.to_string_lossy().to_string())
}

//########################
//#       EAM PLUS       #
//########################

#[tauri::command]
async fn is_plus_user(id_token: String) -> Result<bool, tauri::Error> {
    info!("Checking if user is a plus user...");

    let pool = match POOL.lock() {
        Ok(pool) => pool.clone(),
        Err(poisoned) => {
            error!("Mutex was poisoned. Recovering...");
            poisoned.into_inner().clone()
        }
    };

    check_plus_user(pool, id_token).await
}

async fn check_plus_user(
    pool: Option<DbPool>,
    id_token: String,
) -> Result<bool, tauri::Error> {
    if let Some(pool) = pool {
        let result = eam_plus_lib::user_status_utils::is_plus_user(&id_token.to_string(), &pool).await;
        return Ok(result);
    }

    error!("Database pool not initialized");
    Err(Error::from(std::io::Error::new(
        ErrorKind::Other,
        "Pool is not initialized",
    )))
}

//########################
//#       REQUESTS       #
//########################

#[tauri::command]
async fn send_get_request(url: String, custom_headers: HashMap<String, String>) -> Result<String, String> {
    info!("Sending get request...");

    let mut headers = HeaderMap::new();
    custom_headers.into_iter().for_each(|(key, value)| {
        let header_name = HeaderName::from_str(&key).unwrap();
        let header_value = HeaderValue::from_str(&value).unwrap();
        headers.append(
            header_name,
            header_value,
        );
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
async fn send_post_request(url: String, custom_headers: HashMap<String, String>, body: Value) -> Result<String, String> {
    info!("Sending post request...");

    let mut headers = HeaderMap::new();
    custom_headers.into_iter().for_each(|(key, value)| {
        let header_name = HeaderName::from_str(&key).unwrap();
        let header_value = HeaderValue::from_str(&value).unwrap();
        headers.append(
            header_name,
            header_value,
        );
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

//#########################
//#      ENCRYPTION       #
//#########################

#[tauri::command]
fn encrypt_string(data: String) -> Result<String, tauri::Error> {
    info!("Encrypting string...");

    let encrypted_data = encryption_utils::encrypt_data(&data)
        .map_err(|e| tauri::Error::from(std::io::Error::new(ErrorKind::Other, e.to_string())))?;
    Ok(encrypted_data)
}

#[tauri::command]
fn decrypt_string(data: String) -> Result<String, tauri::Error> {
    info!("Decrypting string...");

    let decrypted_data = encryption_utils::decrypt_data(&data)
        .map_err(|e| tauri::Error::from(std::io::Error::new(ErrorKind::Other, e.to_string())))?;
    Ok(decrypted_data)
}

//#########################
//#         Logs          #
//#########################

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

fn log_to_audit_log_internal(message: String, account_email: Option<String>) {
    let log = AuditLog {
        id: None,
        sender: "daily_auto_login".to_string(),
        message: message,
        accountEmail: account_email,
        time: "".to_string(),
    };
    let _ = log_to_audit_log(log);
}

fn log_to_audit_log_internal_with_pool(pool: &DbPool, message: String, account_email: Option<String>) {
    let log = AuditLog {
        id: None,
        sender: "daily_auto_login".to_string(),
        message: message,
        accountEmail: account_email,
        time: "".to_string(),
    };
    let _ = diesel_functions::insert_audit_log(pool, log)
        .map_err(|e| tauri::Error::from(std::io::Error::new(ErrorKind::Other, e.to_string())));
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