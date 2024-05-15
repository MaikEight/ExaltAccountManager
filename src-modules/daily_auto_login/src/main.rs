// #![windows_subsystem = "windows"]

extern crate chrono;
extern crate dirs;

use eam_commons::encryption_utils;
use eam_commons::setup_database;
use eam_commons::DbPool;
use eam_commons::models::AuditLog;

use eam_commons::get_all_eam_accounts_for_daily_login;
use eam_commons::get_eam_account_by_email;
use eam_commons::diesel_functions;
use eam_commons::get_latest_daily_login;
use eam_commons::get_user_data_by_key;
use eam_commons::insert_or_update_daily_login_report;
use eam_commons::insert_or_update_daily_login_report_entry;
use eam_commons::models::{DailyLoginReports, DailyLoginReportEntries};

use lazy_static::lazy_static;
use std::sync::{Arc, Mutex};
use std::path::PathBuf;
use std::path::Path;
use std::process::Command;
use std::thread;
use std::time::Duration;
use std::collections::HashMap;
use std::fs::File;
use std::io::BufReader;
use std::future::Future;
use std::pin::Pin;
use chrono::{DateTime, Utc};
use roxmltree::Document;
use reqwest::header::HeaderMap;
use reqwest::header::ACCEPT;
use reqwest::header::HeaderValue;
use reqwest::header::CONTENT_TYPE;
use base64::prelude::*;
use std::io::BufRead;
use uuid::Uuid;

lazy_static! {
    static ref POOL: Arc<Mutex<Option<DbPool>>> = Arc::new(Mutex::new(None));
    static ref DECA_API_TIMEOUT_UNTIL: Mutex<i64> = Mutex::new(Utc::now().timestamp_millis() - 1);
}

pub struct GameAccessToken {
    access_token: String,
    access_token_timestamp: String,
    access_token_expiration: String,
}
 
const GAME_START_TIMEOUT: u64 = 90;

#[tokio::main]
async fn main() {
    println!("Starting daily_auto_login...");
    
    use sysinfo::System;

    let mut system = System::new_all();
    system.refresh_all();

    let current_pid = std::process::id();

    for (pid, process) in system.processes() {
        if pid.as_u32() == current_pid {
            continue;
        }
        if process.name() == "EAM_Daily_Auto_Login.exe" {
            println!("EAM_Daily_Auto_Login.exe is already running with PID {}. Exiting...", pid);
            std::process::exit(1);
        }
    }

    //Initialize the database pool
    let database_url = get_database_path().to_str().unwrap().to_string();
    let pool = setup_database(&database_url);
    *POOL.lock().unwrap() = Some(pool);
    let binding = POOL.lock().unwrap();
    let pool = binding.as_ref().unwrap();

    log_to_audit_log(pool, "Checking for game updates...".to_string(), None);
    let update_required =  eam_commons::rotmg_updater::get_game_files_to_update(pool, true).unwrap().len() > 0;

    if update_required {
        log_to_audit_log(pool, "Game update required.".to_string(), None);
        log_to_audit_log(pool, "Performing game update...".to_string(), None);
        let update_result = eam_commons::rotmg_updater::perform_game_update(pool);
        
        match update_result {
            Ok(_) => {
                log_to_audit_log(pool, "Game update successful.".to_string(), None);
            }
            Err(e) => {
                log_to_audit_log(pool, "Game update failed, exiting.".to_string(), None);
                println!("Game update failed: {}", e);
                return;
            }
        }
    } else {
        log_to_audit_log(pool, "No game update required.".to_string(), None);
    }

    log_to_audit_log(pool, "Starting daily_auto_login.".to_string(), None);

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
                        log_to_audit_log(pool, "Todays daily login did already run successfully, exiting.".to_string(), None);
                        return;                        
                    } 

                    println!("Forcing daily login to run again.");
                    log_to_audit_log(pool, "Forcing daily login to run again.".to_string(), None);
                    is_force_run = true;
                } else if daily_login_report.emailsToProcess != None {
                    println!("Last daily login did not finish processing all accounts, continuing...");
                    log_to_audit_log(pool, "Last daily login did not finish processing all accounts, continuing...".to_string(), None);                    
                } else {
                    if !force_run {
                        println!("Last daily login did finish, exiting.");
                        log_to_audit_log(pool, "Last daily login did finish, exiting.".to_string(), None);
                        return;
                    } 
                    
                    println!("Forcing daily login to run again.");
                    log_to_audit_log(pool, "Forcing daily login to run again.".to_string(), None);
                    is_force_run = true;
                }
            } else {
                println!("Last daily login did not finish, continuing...");
                log_to_audit_log(pool, "Last daily login did not finish, continuing...".to_string(), None);
            }

            if is_force_run {
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
            }

            let accounts_email_list = daily_login_report.emailsToProcess.clone().unwrap();
            let accounts_email_list = accounts_email_list.split(", ");
            accounts_to_perform_daily_login_with = Vec::new();
            for account_email in accounts_email_list {
                let acc = get_eam_account_by_email(pool, account_email.to_string()).unwrap();
                accounts_to_perform_daily_login_with.push(acc);
            }
        } else {
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
        }

        if accounts_to_perform_daily_login_with.len() == 0 {
            println!("No accounts to perform daily login with.");
            log_to_audit_log(pool, "No accounts to perform daily login with, exiting.".to_string(), None);

            daily_login_report.hasFinished = true;
            daily_login_report.endTime = Some(Utc::now().to_rfc3339());
            daily_login_report.emailsToProcess = None;

            let _ = insert_or_update_daily_login_report(pool, daily_login_report);

            return;
        }

        daily_login_report.hasFinished = false;
        daily_login_report.endTime = None;

        let _ = insert_or_update_daily_login_report(pool, daily_login_report.clone());
    } else {

        if accounts_to_perform_daily_login_with.len() == 0 {
            println!("No accounts to perform daily login with.");
            log_to_audit_log(pool, "No accounts to perform daily login with, exiting.".to_string(), None);
            return;
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
    }

    //Get the game path from the database
    let game_exe_path = get_user_data_by_key(pool, "game_exe_path".to_string()).unwrap().dataValue;

    if game_exe_path.is_empty() {
        println!("No game.exe path file found, exiting.");
        log_to_audit_log(pool, "No game.exe file found, exiting.".to_string(), None);
        return;
    }

    //Perform the daily login with the accounts
    //Daily login process:
    //1. send_account_verify_request
    //2. send_char_list
    //3. Start the game
    //4. Wait for the game to automatically login
    //5. Close the game after 90 seconds
    //6. Remove the account from the list
    //7. Save the daily_login_report
    //8. Repeat until all accounts have been logged in

    let mut hwid_file_path = PathBuf::from(get_save_file_path());
    hwid_file_path.push("EAM.HWID");
    let hwid;

    if !Path::new(&hwid_file_path).exists() {
        hwid = get_device_unique_identifier().await.unwrap();        
    } else {
        let file = File::open(&hwid_file_path).unwrap();
        let reader = BufReader::new(file);
        let mut lines = reader.lines();
        hwid = lines.next().unwrap().unwrap();        
    }

    let mut emails_vec = accounts_to_perform_daily_login_with.iter().map(|acc| acc.email.clone()).collect::<Vec<String>>();

    for account in accounts_to_perform_daily_login_with {
        let start_time = Some(Utc::now().to_rfc3339());        
        let account_email = account.email.clone();
        let report_entry = DailyLoginReportEntries {
            id: None,
            reportId: Some(daily_login_report.id.clone()),
            startTime: start_time.clone(),
            endTime: None,
            accountEmail: Some(account_email.clone()),
            status: "Processing".to_string(),
            errorMessage: None,
        };
        let entry_id_res = insert_or_update_daily_login_report_entry(pool, report_entry);
        let entry_id = entry_id_res.unwrap();

        println!("Performing daily login with account: {}", account_email);
        log_to_audit_log(pool, ("Performing daily login with account: ".to_owned() + &account_email).to_string(), Some(account_email.clone()));
        
        let pool_arc = Arc::new(pool.clone());
        let access_token_opt: Option<GameAccessToken> = send_account_verify_request(pool_arc, account_email.clone(), hwid.clone()).await;     

        if access_token_opt.is_none() {
            println!("Failed to get access token for account: {}", account_email);
            log_to_audit_log(pool, ("Failed to get access token for account: ".to_owned() + &account_email).to_string(), Some(account_email.clone()));
            daily_login_report.amountOfAccountsFailed += 1;
            daily_login_report.amountOfAccountsProcessed += 1;
            let _ = insert_or_update_daily_login_report(pool, daily_login_report.clone());
            
            let report_entry = DailyLoginReportEntries {
                id: Some(entry_id),
                reportId: Some(daily_login_report.id.clone()),
                startTime: start_time.clone(),
                endTime: Some(Utc::now().to_rfc3339()),
                accountEmail: Some(account_email.clone()),
                status: "Failed".to_string(),
                errorMessage: Some("Failed to get access token.".to_string()),
            };
            let _ = insert_or_update_daily_login_report_entry(pool, report_entry);
            continue;
        }

        let access_token = access_token_opt.unwrap();

        let args = format!(
            "data:{{platform:Deca,guid:{},token:{},tokenTimestamp:{},tokenExpiration:{},env:4,serverName:{}}}",
            BASE64_STANDARD.encode(&account_email.clone()),
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

        //Save the daily_login_report
        daily_login_report.amountOfAccountsProcessed += 1;
        daily_login_report.amountOfAccountsSucceeded += 1;

        let report_entry = DailyLoginReportEntries {
            id: Some(entry_id),
            reportId: Some(daily_login_report.id.clone()),
            startTime: start_time,
            endTime: Some(Utc::now().to_rfc3339()),
            accountEmail: Some(account_email.clone()),
            status: "Succeeded".to_string(),
            errorMessage: None,
        };
        let _ = insert_or_update_daily_login_report_entry(pool, report_entry);
        
        let index = emails_vec.iter().position(|x| *x == account_email).unwrap();
        emails_vec.remove(index);
        daily_login_report.emailsToProcess = Some(emails_vec.join(", "));
        
        let _ = insert_or_update_daily_login_report(pool, daily_login_report.clone());
    }

    println!("Finished daily login.");
    log_to_audit_log(pool, "Finished daily login. ".to_owned() + &daily_login_report.amountOfAccountsFailed.to_string() + " accounts failed.", None);
    daily_login_report.hasFinished = true;
    daily_login_report.endTime = Some(Utc::now().to_rfc3339());
    daily_login_report.emailsToProcess = None;
    let _ = insert_or_update_daily_login_report(pool, daily_login_report);
}

pub fn get_database_path() -> PathBuf {
    let mut path = PathBuf::from(get_save_file_path());
    path.push("exalt_account_manager.db");
    path
}

pub fn send_account_verify_request(pool: Arc<DbPool>, account_email: String, hwid: String) -> Pin<Box<dyn Future<Output = Option<GameAccessToken>> + Send>> {
    let pool = Arc::clone(&pool);
    Box::pin(async move {
        let acc = get_eam_account_by_email(&pool, account_email.clone()).unwrap();
        let pw = encryption_utils::decrypt_data(&acc.password).unwrap();

        let url = "https://www.realmofthemadgod.com/account/verify".to_string();
        let mut data = HashMap::new();

        data.insert("guid".to_string(), account_email.clone());    
        if acc.isSteam {
            data.insert("steamid".to_string(), acc.steamId.clone()?.to_string());
            data.insert("secret".to_string(), pw);
        } else {
            data.insert("password".to_string(), pw);
        }

        data.insert("clientToken".to_string(), hwid.clone());
        data.insert("game_net".to_string(), if acc.isSteam { "Unity_steam" } else { "Unity" }.to_string());
        data.insert("play_platform".to_string(), if acc.isSteam { "Unity_steam" } else { "Unity" }.to_string());
        data.insert("game_net_user_id".to_string(), if acc.isSteam { acc.steamId? } else { "".to_string() });

        let deca_timeout_time = *DECA_API_TIMEOUT_UNTIL.lock().unwrap();
        if deca_timeout_time > Utc::now().timestamp_millis() {
            println!("API Limit reached, waiting 5 minutes.");
            let _ = log_to_audit_log(&pool, "API Limit reached, waiting 5 minutes.".to_string(), Some(account_email.clone()));
            thread::sleep(Duration::from_secs(315));
        }

        let _ = log_to_audit_log(&pool, "Sending account verify request.".to_string(), Some(account_email.clone()));

        let response = send_post_request_with_form_url_encoded_data(url, data).await.unwrap();    
        let token = get_access_token(&response);
        if token.is_none() {
            //Check if the API Limit has been reached
            let doc = Document::parse(&response).unwrap();
            for node in doc.descendants() {
                if node.has_tag_name("Error") {
                    let error_message = node.text().map(|s| s.to_string()).unwrap();
                    if error_message == "Internal error, please wait 5 minutes to try again!" {
                        let _ = log_to_audit_log(&pool, "API Limit reached, waiting 5 minutes.".to_string(), Some(account_email.clone()));
                        println!("API Limit reached, retrying in 5 minutes...");
                        {
                            let mut deca_timeout_time = DECA_API_TIMEOUT_UNTIL.lock().unwrap();
                            *deca_timeout_time = Utc::now().timestamp_millis() + 315000;
                        }
                        return send_account_verify_request(pool, account_email.clone(), hwid.clone()).await;
                    }              
                }
            }
        }

        token
    })
}

fn get_access_token(xml: &str) -> Option<GameAccessToken> {
    let doc = Document::parse(xml).unwrap();
    let mut access_token = GameAccessToken {
        access_token: "".to_string(),
        access_token_timestamp: "".to_string(),
        access_token_expiration: "".to_string(),
    };
    for node in doc.descendants() {        
        if node.has_tag_name("AccessToken") {
            access_token.access_token = node.text().map(|s| s.to_string()).unwrap();
        }

        if node.has_tag_name("AccessTokenTimestamp") {
            access_token.access_token_timestamp = node.text().map(|s| s.to_string()).unwrap();
        }

        if node.has_tag_name("AccessTokenExpiration") {
            access_token.access_token_expiration = node.text().map(|s| s.to_string()).unwrap();
        }
    }
    
    if access_token.access_token.is_empty() || access_token.access_token_timestamp.is_empty() || access_token.access_token_expiration.is_empty() {
        return None;
    }

    Some(access_token)
}

async fn send_post_request_with_form_url_encoded_data(
    url: String,
    data: HashMap<String, String>,
) -> Result<String, String> {
    let mut headers = HeaderMap::new();
    headers.insert(ACCEPT, HeaderValue::from_static("deflate, gzip"));
    headers.insert(
        CONTENT_TYPE,
        HeaderValue::from_static("application/x-www-form-urlencoded"),
    );

    let client = reqwest::Client::new();
    let res = client
        .post(&url)
        .headers(headers)
        .form(&data)
        .send()
        .await
        .map_err(|e| e.to_string())?;

    let body = res.text().await.map_err(|e| e.to_string())?;
    Ok(body)
}

async fn get_device_unique_identifier() -> Result<String, String> {
    use sha1::{Digest, Sha1};
    use wmi::{COMLibrary, Variant, WMIConnection};

    let com_con = COMLibrary::new().map_err(|e| e.to_string())?;
    let wmi_con = WMIConnection::new(com_con.into()).map_err(|e| e.to_string())?;

    let mut concat_str = String::new();

    let baseboard: Vec<HashMap<String, Variant>> = wmi_con
        .raw_query("SELECT * FROM Win32_BaseBoard")
        .map_err(|e| e.to_string())?;
    for obj in baseboard {
        if let Some(Variant::String(serial)) = obj.get("SerialNumber") {
            concat_str.push_str(&serial);
        }
    }

    let bios: Vec<HashMap<String, Variant>> = wmi_con
        .raw_query("SELECT * FROM Win32_BIOS")
        .map_err(|e| e.to_string())?;
    for obj in bios {
        if let Some(Variant::String(serial)) = obj.get("SerialNumber") {
            concat_str.push_str(&serial);
        }
    }

    let os: Vec<HashMap<String, Variant>> = wmi_con
        .raw_query("SELECT * FROM Win32_OperatingSystem")
        .map_err(|e| e.to_string())?;
    for obj in os {
        if let Some(Variant::String(serial)) = obj.get("SerialNumber") {
            concat_str.push_str(&serial);
        }
    }

    let mut hasher = Sha1::new();
    hasher.update(concat_str);
    let result = hasher.finalize();
    let hashed = format!("{:x}", result);

    Ok(hashed)
}

fn log_to_audit_log(pool: &DbPool, message: String, account_email: Option<String>) {
    let log = AuditLog {
        id: None,
        sender: "daily_auto_login".to_string(),
        message: message,
        accountEmail: account_email,
        time: "".to_string(),
    };
    let _ = diesel_functions::insert_audit_log(pool, log).unwrap();
}

fn get_save_file_path() -> String {
    //OS dependent fixed path
    //Windows: C:\Users\USERNAME\AppData\Local\ExaltAccountManager\v4\
    //Mac: /Users/USERNAME/Library/Application Support/ExaltAccountManager/v4/
    let mut path = dirs::data_local_dir().unwrap();
    path.push("ExaltAccountManager");
    path.push("v4");
    path.to_str().unwrap().to_string()
}
