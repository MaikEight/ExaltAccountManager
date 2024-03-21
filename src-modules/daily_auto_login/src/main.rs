mod diesel_functions;
mod diesel_setup;
mod encryption_utils;
mod models;
mod schema;

use crate::diesel_setup::setup_database;
use crate::diesel_setup::DbPool;
use crate::diesel_functions::get_eam_account_by_email;
extern crate chrono;
extern crate dirs;

use lazy_static::lazy_static;
use std::sync::{Arc, Mutex};
use std::path::PathBuf;
use std::path::Path;
use std::process::Command;
use std::thread;
use std::time::Duration;
use std::collections::HashMap;
use std::fs;
use std::fs::File;
use std::io::BufReader;
use serde::{Deserialize, Serialize};
use chrono::{DateTime, Utc};
use roxmltree::Document;
use reqwest::header::HeaderMap;
use reqwest::header::ACCEPT;
use reqwest::header::HeaderValue;
use reqwest::header::CONTENT_TYPE;
use base64::prelude::*;
use std::io::BufRead;

lazy_static! {
    static ref POOL: Arc<Mutex<Option<DbPool>>> = Arc::new(Mutex::new(None));
}

#[derive(Serialize, Deserialize, Clone)]
struct DailyAutoLoginSettings {
    last_daily_login_time: DateTime<Utc>,
    game_exe_path: String,    
}

#[derive(Serialize, Deserialize, Clone)]
struct DailyAutoLoginState {
    account_emails: Vec<String>,
    time_started: DateTime<Utc>,
    last_save: DateTime<Utc>,
    failed_accounts: Vec<String>,    
}

struct GameAccessToken {
    access_token: String,
    access_token_timestamp: String,
    access_token_expiration: String,
}

const GAME_START_TIMEOUT: u64 = 90;

#[tokio::main]
async fn main() {
    println!("Starting daily_auto_login...");

    //Initialize the database pool
    let database_url = get_database_path().to_str().unwrap().to_string();
    let pool = setup_database(&database_url);
    *POOL.lock().unwrap() = Some(pool);

    let accounts_to_perform_daily_login_with =
        diesel_functions::get_all_eam_accounts_for_daily_login(
            &POOL.lock().unwrap().as_ref().unwrap(),
        )
        .unwrap();

    if accounts_to_perform_daily_login_with.len() == 0 {
        println!("No accounts to perform daily login with.");
        return;
    }

    let mut account_emails = Vec::new();    

    let mut path = PathBuf::from(get_save_file_path());
    path.push("dailyAutoLoginSettings.json");
    let daily_auto_login_settings_path = path.to_str().unwrap().to_string();

    let settings: DailyAutoLoginSettings;

    if Path::new(&daily_auto_login_settings_path).exists() {
        //Read the settings from the file, it is a json file of DailyAutoLoginSettings
        settings = read_daily_auto_login_settings(&daily_auto_login_settings_path);
    } else {
        println!("No settings file found, exiting.");
        return;
    }

    path = PathBuf::from(get_save_file_path());
    path.push("dailyAutoLoginState.json");
    let daily_auto_login_state_path = path.to_str().unwrap().to_string();  
    
    let mut state: DailyAutoLoginState;
    if Path::new(&daily_auto_login_state_path).exists() {
        //Read the state from the file, it is a json file of DailyAutoLoginState
        state = read_daily_auto_login_state(&daily_auto_login_state_path);
        //State file found, checking if the last daily login was today
        if state.time_started.date_naive() == Utc::now().date_naive() && state.last_save.date_naive() == Utc::now().date_naive() {
            println!("Last daily login was today.");
            if account_emails.len() == 0 {
                println!("No accounts to perform daily login in state found, deleting state.");
                delete_file(&daily_auto_login_state_path);
                return;
            }

            //Perform the daily login with the accounts in the state
            println!("Resuming: Performing daily login with accounts in state.");
            for account_email in state.clone().account_emails {
                account_emails.push(account_email);
            }

            state = save_daily_auto_login_state(&daily_auto_login_state_path, state);
        }
    } else {
        println!("No state file found, creating new state.");
        for account in accounts_to_perform_daily_login_with {
            account_emails.push(account.email.clone());
        }
        state = DailyAutoLoginState {
            account_emails: account_emails.clone(),
            time_started: Utc::now(),
            last_save: Utc::now(),
            failed_accounts: Vec::new(),
        };

        state = save_daily_auto_login_state(&daily_auto_login_state_path, state);
    }

    //Perform the daily login with the accounts
    //Daily login process:
    //1. send_account_verify_request
    //2. send_char_list
    //3. Start the game
    //4. Wait for the game to automatically login
    //5. Close the game after 90 seconds
    //6. Remove the account from the list
    //7. Save the state
    //8. Repeat until all accounts have been logged in

    let game_exe_path = settings.game_exe_path;
    let game_exe_path = Path::new(&game_exe_path);
    let game_exe_path = game_exe_path.to_str().unwrap();
    let game_exe_path = game_exe_path.to_string();

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

    for account_email in account_emails {
        println!("Performing daily login with account: {}", account_email);
        
        let access_token_opt: Option<GameAccessToken> = send_account_verify_request(account_email.clone(), hwid.clone()).await;     

        if access_token_opt.is_none() {
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
            .arg(args)
            .spawn()
            .expect("Failed to start the game.");
        

        //Wait for the game to automatically login
        thread::sleep(Duration::from_secs(GAME_START_TIMEOUT));

        //Close the game
        child.kill().expect("Failed to close the game.");

        //Remove the account from the list
        let index = state.account_emails.iter().position(|x| *x == account_email).unwrap();
        state.account_emails.remove(index);

        //Save the state
        state = save_daily_auto_login_state(&daily_auto_login_state_path, state);
    }
}

pub fn get_database_path() -> PathBuf {
    let mut path = PathBuf::from(get_save_file_path());
    path.push("exalt_account_manager.db");
    path
}

async fn send_account_verify_request(account_email: String, hwid: String) -> Option<GameAccessToken> {
    let acc = get_eam_account_by_email(&POOL.lock().unwrap().as_ref().unwrap(), account_email.clone()).unwrap();
    let pw = encryption_utils::decrypt_data(&acc.password).unwrap();

    let url = "https://www.realmofthemadgod.com/account/verify".to_string();
    let mut data = HashMap::new();

    data.insert("guid".to_string(), account_email);    
    if acc.isSteam {
        data.insert("steamid".to_string(), acc.steamId.clone()?.to_string());
        data.insert("secret".to_string(), pw);
    } else {
        data.insert("password".to_string(), pw);
    }

    data.insert("clientToken".to_string(), hwid);
    data.insert("game_net".to_string(), if acc.isSteam { "Unity_steam" } else { "Unity" }.to_string());
    data.insert("play_platform".to_string(), if acc.isSteam { "Unity_steam" } else { "Unity" }.to_string());
    data.insert("game_net_user_id".to_string(), if acc.isSteam { acc.steamId? } else { "".to_string() });    

    let response = send_post_request_with_form_url_encoded_data(url, data).await.unwrap();    
    let token = get_access_token(&response);    

    token
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

fn read_daily_auto_login_state(path: &str) -> DailyAutoLoginState {
    let file = File::open(path).unwrap();
    let reader = BufReader::new(file);
    let state = serde_json::from_reader(reader).unwrap();
    
    state
}

fn save_daily_auto_login_state(path: &str, mut state: DailyAutoLoginState) -> DailyAutoLoginState {
    if Path::new(&path).exists() {
        delete_file(&path);
    }

    state.last_save = Utc::now();

    let file = File::create(path).unwrap();
    serde_json::to_writer(file, &state).unwrap();
    state
}

fn delete_file(path: &str) {
    fs::remove_file(path).unwrap();
}

fn read_daily_auto_login_settings(path: &str) -> DailyAutoLoginSettings {
    let file = File::open(path).unwrap();
    let reader = BufReader::new(file);
    let settings: DailyAutoLoginSettings = serde_json::from_reader(reader).unwrap();
    settings
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

fn get_save_file_path() -> String {
    //OS dependent fixed path
    //Windows: C:\Users\USERNAME\AppData\Local\ExaltAccountManager\v4\
    //Mac: /Users/USERNAME/Library/Application Support/ExaltAccountManager/v4/
    let mut path = dirs::data_local_dir().unwrap();
    path.push("ExaltAccountManager");
    path.push("v4");
    path.to_str().unwrap().to_string()
}
