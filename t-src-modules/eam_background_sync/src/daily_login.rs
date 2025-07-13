use crate::account_verify;
use crate::char_list::send_char_list_request;
use crate::events::{AccountProgressState, BackgroundSyncEvent, BackgroundSyncEventHub};
use crate::types::GameAccessToken;
use crate::utils::log_to_audit_log;

use base64::prelude::*;
use chrono::Utc;
use log::{error, info};
use std::io::ErrorKind;
use std::process::Command;
use std::sync::atomic::AtomicBool;
use std::sync::{Arc, Mutex};
use std::time::Duration;
use uuid::Uuid;

use eam_commons::diesel_functions;
use eam_commons::diesel_setup::DbPool;
use eam_commons::insert_or_update_daily_login_report_entry;
use eam_commons::limiter::manager::RateLimiterManager;
use eam_commons::models::DailyLoginReportEntries;
use eam_commons::models::EamAccount;
use eam_commons::models::UserData;
use eam_plus_lib::daily_login::daily_login::{DailyLoginError, DailyLoginResult};
use eam_plus_lib::user_status_utils;

const GAME_START_TIMEOUT: u64 = 90;
const PLUS_USER_TIMEOUT: u64 = 60;

pub async fn perform_daily_login_for_account(
    pool: &DbPool,
    account: EamAccount,
    start_time: Option<String>,
    entry_id: i32,
    daily_login_report_id: String,
    hwid: String,
    game_exe_path: String,
    event_hub: &BackgroundSyncEventHub,
    global_api_limiter: Arc<Mutex<RateLimiterManager>>,
    is_plus_user: Arc<AtomicBool>,
) -> Result<bool, Box<dyn std::error::Error>> {
    info!(
        "[BGRSYNC][DL] Performing daily login with account: {}",
        account.email.clone()
    );
    log_to_audit_log(
        pool,
        ("Performing daily login with account: ".to_owned() + &account.email).to_string(),
        Some(account.email.clone()),
    );

    if is_plus_user.load(std::sync::atomic::Ordering::Relaxed) {
        let account_email = account.email.clone();
        let result = perform_daily_login_for_plus_user(
            pool,
            account,
            hwid,
            global_api_limiter
        )
        .await;

        let success;
        match result {
            Ok(daily_login_report) => {
                if daily_login_report.success {
                    if !daily_login_report.char_list.is_empty() {
                        event_hub.emit(BackgroundSyncEvent::AccountProgress {
                            id: Uuid::new_v4(),
                            email: account_email.clone(),
                            state: AccountProgressState::SyncingCharList,
                        });

                        let uuid = Uuid::new_v4();
                        event_hub.emit(BackgroundSyncEvent::AccountCharListSync {
                            id: uuid,
                            email: account_email.clone(),
                            dataset: daily_login_report.char_list.to_string(),
                        });
                    }
                }

                let report_entry = DailyLoginReportEntries {
                    id: Some(entry_id),
                    reportId: Some(daily_login_report_id.clone()),
                    startTime: start_time.clone(),
                    endTime: Some(Utc::now().to_rfc3339()),
                    accountEmail: Some(account_email.clone()),
                    status: if daily_login_report.success {
                        "Succeeded".to_string()
                    } else {
                        "Failed".to_string()
                    },
                    errorMessage: None,
                };
                let _ = insert_or_update_daily_login_report_entry(pool, report_entry);
                success = daily_login_report.success;
            }
            Err(e) => {
                let report_entry = DailyLoginReportEntries {
                    id: Some(entry_id),
                    reportId: Some(daily_login_report_id.clone()),
                    startTime: start_time.clone(),
                    endTime: Some(Utc::now().to_rfc3339()),
                    accountEmail: Some(account_email.clone()),
                    status: "Failed".to_string(),
                    errorMessage: Some(e.to_string()),
                };
                let _ = insert_or_update_daily_login_report_entry(pool, report_entry);

                return Err(Box::new(e));
            }
        }

        event_hub.emit(BackgroundSyncEvent::AccountProgress {
            id: Uuid::new_v4(),
            email: account_email.clone(),
            state: AccountProgressState::WaitingForCooldown,
        });

        tokio::time::sleep(Duration::from_secs(PLUS_USER_TIMEOUT)).await;

        event_hub.emit(BackgroundSyncEvent::AccountProgress {
            id: Uuid::new_v4(),
            email: account_email.clone(),
            state: AccountProgressState::Done,
        });

        return Ok(success);
    }

    event_hub.emit(BackgroundSyncEvent::AccountProgress {
        id: Uuid::new_v4(),
        email: account.email.clone(),
        state: AccountProgressState::FetchingAccount,
    });

    let pool_arc = Arc::new(pool.clone());
    // let access_token_opt: Option<GameAccessToken> = send_account_verify_request(pool_arc, account.email.clone(), hwid.clone()).await;
    let acc_verify_result = account_verify::send_account_verify_request(
        pool_arc,
        account.email.clone(),
        hwid.clone(),
        global_api_limiter.clone(),
    )
    .await;

    let access_token_opt: Option<GameAccessToken> = match acc_verify_result {
        Ok(token) => token,
        Err(e) => {
            error!(
                "[BGRSYNC][DL] Error during account verification: {}",
                e.to_string()
            );
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
        error!(
            "[BGRSYNC][DL] Failed to get access token for account: {}",
            account.email.clone()
        );
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
        BASE64_STANDARD.encode(access_token.clone().access_token),
        BASE64_STANDARD.encode(access_token.clone().access_token_timestamp),
        BASE64_STANDARD.encode(access_token.clone().access_token_expiration),
        "".to_string()
    );

    //Start the game with the args
    let mut child = Command::new(game_exe_path.clone())
        .arg("-batchmode")
        .arg(args)
        .spawn()
        .expect("Failed to start the game.");

    info!("[BGRSYNC][DL] Game started, waiting for login...");

    event_hub.emit(BackgroundSyncEvent::AccountProgress {
        id: Uuid::new_v4(),
        email: account.email.clone(),
        state: AccountProgressState::FetchingCharList,
    });

    let char_list_result =
        send_char_list_request(access_token, Arc::clone(&global_api_limiter)).await;
    let char_list_response = match char_list_result {
        Ok(response) => response,
        Err(err) => {
            error!(
                "[BGRSYNC][DL] Error during char list request: {}",
                err.to_string()
            );
            log_to_audit_log(
                pool,
                ("Error during char list request: ".to_owned() + &err.to_string()).to_string(),
                Some(account.email.clone()),
            );
            //We can't exit (return) here because the game is still running and we need to close it properly after wait for the game to login
            "".to_string()
        }
    };

    if !char_list_response.is_empty() {
        event_hub.emit(BackgroundSyncEvent::AccountProgress {
            id: Uuid::new_v4(),
            email: account.email.clone(),
            state: AccountProgressState::SyncingCharList,
        });

        let uuid = Uuid::new_v4();
        event_hub.emit(BackgroundSyncEvent::AccountCharListSync {
            id: uuid,
            email: account.email.clone(),
            dataset: char_list_response.to_string(),
        });
    }

    //Wait for the game to automatically login
    tokio::time::sleep(Duration::from_secs(GAME_START_TIMEOUT)).await;

    //Close the game
    child.kill().expect("Failed to close the game.");

    info!(
        "[BGRSYNC][DL] Game closed, daily login completed for account: {}",
        account.email.clone()
    );

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

async fn perform_daily_login_for_plus_user(
    pool: &DbPool,
    account: EamAccount,
    hwid: String,
    global_api_limiter: Arc<Mutex<RateLimiterManager>>,
) -> Result<DailyLoginResult, DailyLoginError> {
    let jwt = diesel_functions::get_user_data_by_key(&pool, "jwtSignature".to_string())
        .unwrap_or_else(|_| {
            error!("Failed to get jwtSignature from user data, using dummy value.");
            UserData {
                dataKey: "jwtSignature".to_string(),
                dataValue: "dummy_id_token".to_string(),
            }
        });

    // If the token is not found or invalid, we continue with the daily login for non plus users
    if jwt.dataValue.is_empty() || jwt.dataValue == "dummy_id_token" {
        error!("No valid JWT token found for Plus user, skipping daily login.");
        return Err(DailyLoginError::FailedToGetAccessToken(
            "No valid JWT token found for Plus user.".to_string(),
        ));
    }

    let is_plus_user = user_status_utils::is_plus_user(&jwt.dataValue, &pool).await;
    if !is_plus_user {
        error!(
            "[BGRSYNC][DL] User is not a Plus user, skipping daily login for account: {}",
            account.email.clone()
        );
        return Err(DailyLoginError::NotAPlusUserError(
            "Failed jwt validation.".to_string(),
        ));
    }

    let result: Result<DailyLoginResult, DailyLoginError> =
        eam_plus_lib::daily_login::daily_login::perform_daily_login(
            jwt.dataValue.clone(),
            account.email.clone(),
            hwid,
            pool,
            global_api_limiter
        )
        .await;

    match result {
        Ok(daily_login_report) => {
            info!(
                "[BGRSYNC][DL] Daily login completed for Plus user: {}",
                account.email.clone()
            );
            log_to_audit_log(
                pool,
                ("Daily login completed for Plus user: ".to_owned() + &account.email).to_string(),
                Some(account.email.clone()),
            );

            Ok(daily_login_report)
        }
        Err(e) => {
            error!(
                "[BGRSYNC][DL] Error during daily login for Plus user: {}",
                e.to_string()
            );
            log_to_audit_log(
                pool,
                ("Error during daily login for Plus user: ".to_owned() + &e.to_string())
                    .to_string(),
                Some(account.email.clone()),
            );
            Err(e)
        }
    }
}
