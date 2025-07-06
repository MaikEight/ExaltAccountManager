use crate::utils::log_to_audit_log;
use crate::account_verify;
use crate::types::GameAccessToken;

use base64::prelude::*;
use chrono::Utc;
use std::io::ErrorKind;
use std::process::Command;
use std::sync::{Arc, Mutex};
use std::time::Duration;

use eam_commons::diesel_setup::DbPool;
use eam_commons::models::DailyLoginReportEntries;
use eam_commons::models::EamAccount;
use eam_commons::insert_or_update_daily_login_report_entry; 
use eam_commons::limiter::manager::RateLimiterManager;

const GAME_START_TIMEOUT: u64 = 90;

pub async fn perform_daily_login_for_account(
    pool: &DbPool,
    account: EamAccount,
    start_time: Option<String>,
    entry_id: i32,
    daily_login_report_id: String,
    hwid: String,
    game_exe_path: String,
    global_api_limiter: Arc<Mutex<RateLimiterManager>>
) -> Result<bool, Box<dyn std::error::Error>> {
    println!(
        "Performing daily login with account: {}",
        account.email.clone()
    );
    log_to_audit_log(
        pool,
        ("Performing daily login with account: ".to_owned() + &account.email).to_string(),
        Some(account.email.clone()),
    );

    let pool_arc = Arc::new(pool.clone());
    // let access_token_opt: Option<GameAccessToken> = send_account_verify_request(pool_arc, account.email.clone(), hwid.clone()).await;
    let acc_verify_result = account_verify::send_account_verify_request(
        pool_arc,
        account.email.clone(),
        hwid.clone(),
        global_api_limiter.clone(),
    ).await;
    
    let access_token_opt: Option<GameAccessToken> = match acc_verify_result {
        Ok(token) => token,
        Err(e) => {
            println!("Error during account verification: {:?}", e);
            log_to_audit_log(
                pool,
                ("Error during account verification: ".to_owned() + &e.to_string()).to_string(),
                Some(account.email.clone()),
            );
            return Err(Box::new(std::io::Error::new(
                ErrorKind::Other,
                "Failed to verify account.",
            )));
        }
    };

    if access_token_opt.is_none() {
        println!("Failed to get access token for account: {}", account.email);
        log_to_audit_log(
            pool,
            ("Failed to get access token for account: ".to_owned() + &account.email).to_string(),
            Some(account.email.clone()),
        );

        let report_entry = DailyLoginReportEntries {
            id: Some(entry_id),
            reportId: Some(daily_login_report_id.clone()),
            startTime: start_time.clone(),
            endTime: Some(Utc::now().to_rfc3339()),
            accountEmail: Some(account.email.clone()),
            status: "Failed".to_string(),
            errorMessage: Some("Failed to get access token.".to_string()),
        };
        let _ = insert_or_update_daily_login_report_entry(pool, report_entry);

        return Err(Box::new(std::io::Error::new(
            ErrorKind::Other,
            "Failed to get access token.",
        )));
    }
    let access_token = access_token_opt.unwrap();
    let args = format!(
        "data:{{platform:Deca,guid:{},token:{},tokenTimestamp:{},tokenExpiration:{},env:4,serverName:{}}}",
        BASE64_STANDARD.encode(&account.email.clone()),
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
    tokio::time::sleep(Duration::from_secs(GAME_START_TIMEOUT)).await;

    //Close the game
    child.kill().expect("Failed to close the game.");

    let report_entry = DailyLoginReportEntries {
        id: Some(entry_id),
        reportId: Some(daily_login_report_id.clone()),
        startTime: start_time,
        endTime: Some(Utc::now().to_rfc3339()),
        accountEmail: Some(account.email.clone()),
        status: "Succeeded".to_string(),
        errorMessage: None,
    };
    let _ = insert_or_update_daily_login_report_entry(pool, report_entry);

    Ok(true)
}
