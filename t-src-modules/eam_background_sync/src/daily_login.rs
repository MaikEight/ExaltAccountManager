use crate::events::{AccountProgressState, BackgroundSyncEvent, BackgroundSyncEventHub};

use chrono::Utc;
use log::{error, info};
use std::sync::{Arc, Mutex};
use std::time::Duration;
use uuid::Uuid;

use eam_commons::char_list::parse_char_list_request;
use eam_commons::diesel_functions::{self, delete_all_servers, insert_char_list_dataset, insert_servers};
use eam_commons::diesel_setup::DbPool;
use eam_commons::insert_or_update_daily_login_report_entry;
use eam_commons::limiter::manager::RateLimiterManager;
use eam_commons::models::DailyLoginReportEntries;
use eam_commons::models::EamAccount;
use eam_commons::models::UserData;
use eam_commons::utils::log_to_audit_log;
use eam_plus_lib::daily_login::daily_login::{DailyLoginError, DailyLoginResult};

/// Cooldown after each account's daily login, to stay well within DECA's API
/// rate limits.
const DAILY_LOGIN_TIMEOUT: u64 = 60;

/// Performs the daily login for a single account.
///
/// Every account — regardless of EAM Plus status, and including Steam accounts —
/// goes through the eam_plus_lib API-based daily login (account/verify +
/// char/list with `do_login`). EAM never starts the game executable for the
/// daily login (DECA ToS compliance).
pub async fn perform_daily_login_for_account(
    pool: &DbPool,
    account: EamAccount,
    start_time: Option<String>,
    entry_id: i32,
    daily_login_report_id: String,
    hwid: String,
    event_hub: &BackgroundSyncEventHub,
    global_api_limiter: Arc<Mutex<RateLimiterManager>>,
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

    let account_email = account.email.clone();
    let result = perform_daily_login_via_api(pool, account, hwid, global_api_limiter).await;

    let success;
    match result {
        Ok(daily_login_report) => {
            if daily_login_report.success && !daily_login_report.char_list.is_empty() {
                event_hub.emit(BackgroundSyncEvent::AccountProgress {
                    id: Uuid::new_v4(),
                    email: account_email.clone(),
                    state: AccountProgressState::SyncingCharList,
                });

                // Parse and insert char list dataset directly
                match parse_char_list_request(&account_email, None, daily_login_report.char_list.to_string()).await {
                    Ok((dataset, servers, _request_state)) => {
                        // Insert dataset into database
                        if let Err(e) = insert_char_list_dataset(pool, dataset) {
                            error!("[BGRSYNC][DL] Failed to insert char list dataset for {}: {}", &account_email, e);
                        }

                        // Update servers if present
                        if !servers.is_empty() {
                            if let Err(e) = delete_all_servers(pool) {
                                error!("[BGRSYNC][DL] Failed to delete servers: {:?}", e);
                            } else if let Err(e) = insert_servers(pool, servers) {
                                error!("[BGRSYNC][DL] Failed to insert servers: {:?}", e);
                            }
                        }
                    }
                    Err(e) => {
                        error!("[BGRSYNC][DL] Failed to parse char list for {}: {}", &account_email, e);
                    }
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

    tokio::time::sleep(Duration::from_secs(DAILY_LOGIN_TIMEOUT)).await;

    event_hub.emit(BackgroundSyncEvent::AccountProgress {
        id: Uuid::new_v4(),
        email: account_email.clone(),
        state: AccountProgressState::Done,
    });

    Ok(success)
}

/// Runs the eam_plus_lib API daily login for an account. Works for all accounts
/// (plus or free, Steam or not). The stored JWT is passed through only for
/// bookkeeping; it is not required and does not gate the login.
async fn perform_daily_login_via_api(
    pool: &DbPool,
    account: EamAccount,
    hwid: String,
    global_api_limiter: Arc<Mutex<RateLimiterManager>>,
) -> Result<DailyLoginResult, DailyLoginError> {
    let jwt = diesel_functions::get_user_data_by_key(&pool, "jwtSignature".to_string())
        .unwrap_or_else(|_| UserData {
            dataKey: "jwtSignature".to_string(),
            dataValue: String::new(),
        });

    let result: Result<DailyLoginResult, DailyLoginError> =
        eam_plus_lib::daily_login::daily_login::perform_daily_login(
            jwt.dataValue.clone(),
            account.email.clone(),
            hwid,
            pool,
            global_api_limiter,
        )
        .await;

    match result {
        Ok(daily_login_report) => {
            info!(
                "[BGRSYNC][DL] Daily login completed for account: {}",
                account.email.clone()
            );
            log_to_audit_log(
                pool,
                ("Daily login completed for account: ".to_owned() + &account.email).to_string(),
                Some(account.email.clone()),
            );

            Ok(daily_login_report)
        }
        Err(e) => {
            error!(
                "[BGRSYNC][DL] Error during daily login for account: {}",
                e.to_string()
            );
            log_to_audit_log(
                pool,
                ("Error during daily login for account: ".to_owned() + &e.to_string()).to_string(),
                Some(account.email.clone()),
            );
            Err(e)
        }
    }
}
