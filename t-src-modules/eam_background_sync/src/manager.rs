use crate::daily_login;
use crate::events::*;
use crate::process_account::process_account;
use crate::types::*;
use crate::utils::{get_save_file_path, log_to_audit_log};

use chrono::{DateTime, Utc};
use log::{debug, error, warn, info};
use sha1::{Digest, Sha1};
use std::fs::File;
use std::io::{BufRead, BufReader};
use std::path::{Path, PathBuf};
use std::sync::atomic::{AtomicBool, Ordering};
use std::sync::{Arc, Mutex};
use std::time::Duration;
use uuid::Uuid;

use eam_commons::diesel_functions::{
    self, get_all_eam_accounts_for_daily_login, get_next_eam_account_for_background_sync,
};
use eam_commons::diesel_setup::DbPool;
use eam_commons::get_eam_account_by_email;
use eam_commons::get_latest_daily_login;
use eam_commons::get_user_data_by_key;
use eam_commons::hwid::get_device_unique_identifier;
use eam_commons::insert_or_update_daily_login_report;
use eam_commons::insert_or_update_daily_login_report_entry;
use eam_commons::limiter::manager::RateLimiterManager;
use eam_commons::models::{DailyLoginReportEntries, DailyLoginReports, UserData};
use eam_plus_lib::user_status_utils;

#[derive(Clone)]
pub struct BackgroundSyncManager {
    pub pool: Arc<DbPool>,
    pub event_hub: BackgroundSyncEventHub,
    pub api_limiter: Arc<Mutex<RateLimiterManager>>,

    hwid: String,
    current_mode: Arc<Mutex<SyncMode>>,
    config: Arc<Mutex<BackgroundSyncConfig>>,
    should_stop: Arc<Mutex<bool>>,
    is_running: Arc<AtomicBool>,
    is_plus_user: Arc<AtomicBool>,
}

impl BackgroundSyncManager {
    /// Helper to retry daily login report updates
    async fn retry_daily_login_report_update(&self, daily_login_report: DailyLoginReports) {
        let mut retry_count = 0;
        let max_retries = 3;
        loop {
            match insert_or_update_daily_login_report(&self.pool, daily_login_report.clone()) {
                Ok(_) => break,
                Err(db_err) => {
                    retry_count += 1;
                    let retry_error_msg = db_err.to_string();

                    if retry_count >= max_retries {
                        error!("[BGRSYNC][DL] Failed to update daily login report after {} retries: {}", max_retries, retry_error_msg);
                        break;
                    }

                    let wait_time = Duration::from_millis(100 * (2_u64.pow(retry_count - 1)));
                    error!("[BGRSYNC][DL] Failed to update daily login report (attempt {}), retrying in {:?}: {}", retry_count, wait_time, retry_error_msg);
                    tokio::time::sleep(wait_time).await;
                }
            }
        }
    }

    /// Helper to retry daily login report entry updates
    async fn retry_daily_login_entry_update(&self, report_entry: DailyLoginReportEntries) {
        let mut retry_count = 0;
        let max_retries = 3;
        loop {
            match insert_or_update_daily_login_report_entry(&self.pool, report_entry.clone()) {
                Ok(_) => break,
                Err(db_err) => {
                    retry_count += 1;
                    let retry_error_msg = db_err.to_string();

                    if retry_count >= max_retries {
                        error!("[BGRSYNC][DL] Failed to update daily login report entry after {} retries: {}", max_retries, retry_error_msg);
                        break;
                    }

                    let wait_time = Duration::from_millis(100 * (2_u64.pow(retry_count - 1)));
                    error!("[BGRSYNC][DL] Failed to update daily login report entry (attempt {}), retrying in {:?}: {}", retry_count, wait_time, retry_error_msg);
                    tokio::time::sleep(wait_time).await;
                }
            }
        }
    }

    /// Process login result without holding error types across await boundaries
    async fn process_login_result(
        &self,
        login_result: Result<bool, String>,
        daily_login_report: &mut DailyLoginReports,
        emails_vec: &mut Vec<String>,
        entry_id: i32,
        start_time: Option<String>,
        account_email: &str,
    ) {
        match login_result {
            Ok(success) => {
                info!(
                    "[BGRSYNC][DL] Successfully performed daily login with account: {}",
                    account_email
                );
                log_to_audit_log(
                    &self.pool,
                    ("Successfully performed daily login with account: ".to_owned()
                        + account_email)
                        .to_string(),
                    Some(account_email.to_string()),
                );

                //Save the daily_login_report
                let index = emails_vec
                    .iter()
                    .position(|x| *x == *account_email)
                    .unwrap();
                emails_vec.remove(index);
                daily_login_report.emailsToProcess = Some(emails_vec.join(", "));
                daily_login_report.amountOfAccountsProcessed += 1;
                daily_login_report.amountOfAccountsSucceeded += 1;

                // Use helper function to avoid holding error across await
                self.retry_daily_login_report_update(daily_login_report.clone())
                    .await;

                self.event_hub.emit(BackgroundSyncEvent::AccountFinished {
                    id: Uuid::new_v4(),
                    email: account_email.to_string(),
                    result: if success {
                        "Succeeded".to_string()
                    } else {
                        "Failed".to_string()
                    },
                });

                self.event_hub
                    .emit(BackgroundSyncEvent::DailyLoginProgress {
                        id: Uuid::new_v4(),
                        done: daily_login_report.amountOfAccountsProcessed as usize,
                        left: emails_vec.len(),
                        left_emails: emails_vec.clone(),
                        failed_emails: Vec::new(),
                        estimated_time: self.get_estimated_time(emails_vec.len()),
                    });
            }
            Err(error_message) => {
                // Continue processing with string error message
                self.handle_login_failure(
                    error_message,
                    daily_login_report,
                    emails_vec,
                    entry_id,
                    start_time,
                    account_email,
                )
                .await;
            }
        }
    }

    /// Helper to handle login failures without holding error types across await
    async fn handle_login_failure(
        &self,
        error_message: String,
        daily_login_report: &mut DailyLoginReports,
        emails_vec: &mut Vec<String>,
        entry_id: i32,
        start_time: Option<String>,
        account_email: &str,
    ) {
        error!(
            "[BGRSYNC][DL] Error while performing daily login: {}",
            error_message
        );
        log_to_audit_log(
            &self.pool,
            ("Error while performing daily login: ".to_owned() + &error_message).to_string(),
            None,
        );

        // Remove the failed account from emails_vec and update the report
        let index = emails_vec
            .iter()
            .position(|x| *x == *account_email)
            .unwrap();
        emails_vec.remove(index);
        daily_login_report.emailsToProcess = Some(emails_vec.join(", "));
        daily_login_report.amountOfAccountsFailed += 1;
        daily_login_report.amountOfAccountsProcessed += 1;

        // Use helper function to avoid holding error across await
        self.retry_daily_login_report_update(daily_login_report.clone())
            .await;

        let failed_entry = DailyLoginReportEntries {
            id: Some(entry_id),
            reportId: Some(daily_login_report.id.clone()),
            startTime: start_time.clone(),
            endTime: Some(Utc::now().to_rfc3339()),
            accountEmail: Some(account_email.to_string()),
            status: "Failed".to_string(),
            errorMessage: Some(
                "Failed due to unknown reason.".to_string() + "Error thrown: " + &error_message,
            ),
        };

        // Use helper function to avoid holding error across await
        self.retry_daily_login_entry_update(failed_entry).await;

        // Emit events for failed account
        self.event_hub.emit(BackgroundSyncEvent::AccountFinished {
            id: Uuid::new_v4(),
            email: account_email.to_string(),
            result: "Failed".to_string(),
        });

        self.event_hub
            .emit(BackgroundSyncEvent::DailyLoginProgress {
                id: Uuid::new_v4(),
                done: daily_login_report.amountOfAccountsProcessed as usize,
                left: emails_vec.len(),
                left_emails: emails_vec.clone(),
                failed_emails: Vec::new(), // You might want to track failed emails separately
                estimated_time: self.get_estimated_time(emails_vec.len()),
            });
    }

    pub async fn new(pool: Arc<DbPool>, api_limiter: Arc<Mutex<RateLimiterManager>>) -> Self {
        let hwid = Self::read_hwid().await;

        let jwt = diesel_functions::get_user_data_by_key(&pool, "jwtSignature".to_string())
            .unwrap_or_else(|_| {
                info!("[BGRSYNC] No JWT signature found.");
                UserData {
                    dataKey: "jwtSignature".to_string(),
                    dataValue: "".to_string(),
                }
            });

        let is_plus_user = if jwt.dataValue.is_empty() {
            false
        } else {
            user_status_utils::is_plus_user(&jwt.dataValue, &pool).await
        };

        Self {
            pool,
            event_hub: BackgroundSyncEventHub::new(),
            api_limiter,
            hwid,
            current_mode: Arc::new(Mutex::new(SyncMode::Stopped)),
            config: Arc::new(Mutex::new(BackgroundSyncConfig {
                enabled: true,
                speed: SyncSpeed::Normal,
            })),
            should_stop: Arc::new(Mutex::new(false)),
            is_running: Arc::new(AtomicBool::new(false)),
            is_plus_user: Arc::new(AtomicBool::new(is_plus_user)),
        }
    }

    fn get_fallback_hash() -> String {
        let mut hasher = Sha1::new();
        let random_string = format!("{}{}", Uuid::new_v4(), "Fallback HWID");
        hasher.update(random_string);
        let result = hasher.finalize();
        let hashed =format!("{:x}", result);
        info!("Using fallback HWID: {}", hashed);
        hashed
    }

    async fn read_hwid() -> String {
        let mut hwid_file_path = PathBuf::from(get_save_file_path());
        hwid_file_path.push("EAM.HWID");
        
        if Path::new(&hwid_file_path).exists() {
            // Try to read from existing file with proper error handling
            match File::open(&hwid_file_path) {
                Ok(file) => {
                    let reader = BufReader::new(file);
                    match reader.lines().next() {
                        Some(Ok(hwid)) => {
                            if !hwid.trim().is_empty() {
                                return hwid.trim().to_string();
                            } else {
                                warn!("HWID file exists but is empty, using fallback HWID.");
                                return Self::get_fallback_hash();
                            }
                        }
                        Some(Err(e)) => {
                            error!("Failed to read HWID file: {}", e);
                            return Self::get_fallback_hash();
                        }
                        None => {
                            warn!("HWID file exists but is empty, using fallback HWID.");
                            return Self::get_fallback_hash();
                        }
                    }
                }
                Err(e) => {
                    error!("Failed to open HWID file: {}, using fallback HWID.", e);
                    return Self::get_fallback_hash();
                }
            }
        }
        
        // Generate new HWID (either file doesn't exist or reading failed)
        let hwid = get_device_unique_identifier().await;
        match hwid {
            Ok(hwid) => hwid,
            Err(e) => {
                error!("Failed to get HWID: {}", e);
                Self::get_fallback_hash()
            }
        }
    }

    pub fn is_running(&self) -> bool {
        self.is_running.load(Ordering::SeqCst)
    }

    pub fn get_current_mode(&self) -> SyncMode {
        self.current_mode.lock().unwrap().clone()
    }

    pub fn get_event_hub(&self) -> BackgroundSyncEventHub {
        self.event_hub.clone()
    }

    pub fn switch_mode(&self, mode: SyncMode) {
        let mut current = self.current_mode.lock().unwrap();
        if *current != mode {
            *current = mode.clone();
            self.event_hub.emit(BackgroundSyncEvent::ModeChanged {
                id: Uuid::new_v4(),
                mode: format!("{:?}", mode),
            });
        }
    }

    pub fn stop(&self) {
        let mut flag = self.should_stop.lock().unwrap();
        *flag = true;
        self.is_running.store(true, Ordering::SeqCst);
        info!("[BGRSYNC] Stopping background sync manager...");

        let mut current = self.current_mode.lock().unwrap();
        *current = SyncMode::Stopped;
        drop(current);

        self.event_hub.emit(BackgroundSyncEvent::ModeChanged {
            id: Uuid::new_v4(),
            mode: "Stopped".to_string(),
        });
    }

    pub fn start(&self) {
        if self.is_running.load(Ordering::SeqCst) {
            log::warn!(
                "[BGRSYNC] Attempted to start background sync manager but it's already running."
            );
            return;
        }
        info!("[BGRSYNC] Starting background sync manager...");

        self.is_running.store(true, Ordering::SeqCst);
        let manager = self.clone();

        tokio::spawn(async move {
            manager.run_loop().await;
            manager.is_running.store(false, Ordering::SeqCst);
        });
    }

    async fn run_loop(&self) {
        let mut last_mode = self.current_mode.lock().unwrap().clone();
        loop {
            if *self.should_stop.lock().unwrap() {
                break;
            }

            let mode = self.current_mode.lock().unwrap().clone();
            if last_mode != mode {
                last_mode = mode.clone();
                self.event_hub.emit(BackgroundSyncEvent::ModeChanged {
                    id: Uuid::new_v4(),
                    mode: format!("{:?}", mode),
                });
            }
            match mode {
                SyncMode::Default => self.run_default_mode().await,
                SyncMode::DailyLogin => self.run_daily_login_mode(false).await,
                SyncMode::Stopped => {
                    break;
                }
            }

            tokio::time::sleep(Duration::from_secs(1)).await;
        }
    }

    async fn run_default_mode(&self) {
        let config = self.config.lock().unwrap().clone();
        if !config.enabled {
            return;
        }

        loop {
            if *self.should_stop.lock().unwrap() {
                return;
            }

            let can_call = {
                let limiter = self.api_limiter.lock().unwrap();

                match (
                    limiter.api_limits("account/verify"),
                    limiter.api_limits("char/list"),
                ) {
                    (Some((limit_v, remain_v)), Some((limit_c, remain_c))) => {
                        limit_v == remain_v && limit_c == remain_c
                    }
                    _ => false,
                }
            };

            if !can_call {
                debug!("[BGRSYNC] API limiter active, waiting...");
                tokio::time::sleep(Duration::from_secs(10)).await;
                continue;
            }

            info!("[BGRSYNC] API limiter is clear, proceeding with account sync.");

            let account = match get_next_eam_account_for_background_sync(&self.pool) {
                Ok(Some(acc)) => acc,
                Ok(None) => {
                    info!("[BGRSYNC] No more accounts to process, waiting 5 minutes.");
                    tokio::time::sleep(Duration::from_secs(300)).await;
                    continue;
                }
                Err(e) => {
                    error!("Failed to get next account: {}", e);
                    return;
                }
            };

            info!("[BGRSYNC] Processing account: {}", account.email);

            let (result, dataset_opt) = process_account(
                Arc::clone(&self.pool),
                account.clone(),
                self.hwid.clone(),
                &self.event_hub,
                Arc::clone(&self.api_limiter),
            )
            .await;

            match result.clone() {
                SyncResult::Success => {
                    info!(
                        "[BGRSYNC] Successfully processed account: {}",
                        account.email
                    );
                    if let Some(dataset) = dataset_opt {
                        let uuid = Uuid::new_v4();
                        self.event_hub
                            .emit(BackgroundSyncEvent::AccountCharListSync {
                                id: uuid,
                                email: account.email.clone(),
                                dataset: dataset.to_string(),
                            });
                    }
                }
                SyncResult::RateLimited => {
                    info!("[BGRSYNC] Rate limit hit for account: {}", account.email);
                    self.event_hub.emit(BackgroundSyncEvent::AccountFailed {
                        id: Uuid::new_v4(),
                        email: account.email.clone(),
                        error: "Rate limited".to_string(),
                    });
                }
                SyncResult::Failed(msg) => {
                    error!(
                        "[BGRSYNC] Failed to process account {}: {}",
                        account.email, msg
                    );
                    self.event_hub.emit(BackgroundSyncEvent::AccountFailed {
                        id: Uuid::new_v4(),
                        email: account.email.clone(),
                        error: msg,
                    });
                }
                SyncResult::Skipped => {
                    info!("[BGRSYNC] Skipped account: {}", account.email);
                }
                SyncResult::Cancelled => {
                    info!(
                        "[BGRSYNC] Processing cancelled for account: {}",
                        account.email
                    );
                }
            }

            self.event_hub.emit(BackgroundSyncEvent::AccountFinished {
                id: Uuid::new_v4(),
                email: account.email.clone(),
                result: format!("{:?}", result),
            });

            tokio::time::sleep(Duration::from_secs(1)).await;
        }
    }

    /// Runs the daily login mode, which processes accounts for daily login rewards.
    /// FOR NON-PLUS USERS: The Game files need to be up-to-date for this to work.
    async fn run_daily_login_mode(&self, do_force_run: bool) {
        info!("[BGRSYNC][DL] Running daily login mode...");

        //SETUP
        //Check if daily login did already run today and if so, if it finished
        //Load accounts for daily login
        //Store DailyLoginsReport in DB (start with empty report / use the existing one if it exists)

        let daily_login_report_ret = get_latest_daily_login(&self.pool);
        let mut daily_login_report: DailyLoginReports;
        let mut accounts_to_perform_daily_login_with =
            get_all_eam_accounts_for_daily_login(&self.pool).unwrap();

        if daily_login_report_ret.is_ok() {
            daily_login_report = daily_login_report_ret.unwrap();
            let daily_login_report_time = daily_login_report.startTime.clone().unwrap();
            let daily_login_report_time =
                DateTime::parse_from_rfc3339(&daily_login_report_time).unwrap();
            let daily_login_report_time = daily_login_report_time.with_timezone(&Utc);

            if daily_login_report_time.date_naive() == Utc::now().date_naive() {
                let mut is_force_run = false;
                let force_run = do_force_run;
                if daily_login_report.hasFinished {
                    if daily_login_report.amountOfAccountsProcessed
                        == daily_login_report.amountOfAccounts
                    {
                        if !force_run {
                            info!("[BGRSYNC][DL] Todays daily login did already run successfully, exiting.");
                            log_to_audit_log(
                                &self.pool,
                                "Todays daily login did already run successfully, exiting."
                                    .to_string(),
                                None,
                            );
                            return;
                        }

                        info!("[BGRSYNC][DL] Forcing daily login to run again.");
                        log_to_audit_log(
                            &self.pool,
                            "Forcing daily login to run again.".to_string(),
                            None,
                        );
                        is_force_run = true;
                    } else if daily_login_report.emailsToProcess != None {
                        info!("[BGRSYNC][DL] Last daily login did not finish processing all accounts, continuing...");
                        log_to_audit_log(&self.pool, "Last daily login did not finish processing all accounts, continuing...".to_string(), None);
                    } else {
                        if !force_run {
                            info!("[BGRSYNC][DL] Last daily login did finish, exiting.");
                            log_to_audit_log(
                                &self.pool,
                                "Last daily login did finish, exiting.".to_string(),
                                None,
                            );
                            return;
                        }

                        info!("[BGRSYNC][DL] Forcing daily login to run again.");
                        log_to_audit_log(
                            &self.pool,
                            "Forcing daily login to run again.".to_string(),
                            None,
                        );
                        is_force_run = true;
                    }
                } else {
                    info!("[BGRSYNC][DL] Last daily login did not finish, continuing...");
                    log_to_audit_log(
                        &self.pool,
                        "Last daily login did not finish, continuing...".to_string(),
                        None,
                    );
                }

                if is_force_run {
                    let report_uuid = Uuid::new_v4().to_string();
                    daily_login_report = DailyLoginReports {
                        id: report_uuid,
                        startTime: Some(Utc::now().to_rfc3339()),
                        endTime: None,
                        hasFinished: false,
                        emailsToProcess: Some(
                            accounts_to_perform_daily_login_with
                                .iter()
                                .map(|acc| acc.email.clone())
                                .collect::<Vec<String>>()
                                .join(", "),
                        ),
                        amountOfAccounts: accounts_to_perform_daily_login_with.len() as i32,
                        amountOfAccountsProcessed: 0,
                        amountOfAccountsFailed: 0,
                        amountOfAccountsSucceeded: 0,
                    };
                    let _ =
                        insert_or_update_daily_login_report(&self.pool, daily_login_report.clone());
                }

                let accounts_email_list = daily_login_report.emailsToProcess.clone().unwrap();
                let accounts_email_list = accounts_email_list.split(", ");
                accounts_to_perform_daily_login_with = Vec::new();
                for account_email in accounts_email_list {
                    let acc =
                        get_eam_account_by_email(&self.pool, account_email.to_string()).unwrap();
                    accounts_to_perform_daily_login_with.push(acc);
                }
            } else {
                let report_uuid = Uuid::new_v4().to_string();
                daily_login_report = DailyLoginReports {
                    id: report_uuid,
                    startTime: Some(Utc::now().to_rfc3339()),
                    endTime: None,
                    hasFinished: false,
                    emailsToProcess: Some(
                        accounts_to_perform_daily_login_with
                            .iter()
                            .map(|acc| acc.email.clone())
                            .collect::<Vec<String>>()
                            .join(", "),
                    ),
                    amountOfAccounts: accounts_to_perform_daily_login_with.len() as i32,
                    amountOfAccountsProcessed: 0,
                    amountOfAccountsFailed: 0,
                    amountOfAccountsSucceeded: 0,
                };
                let _ = insert_or_update_daily_login_report(&self.pool, daily_login_report.clone());
            }

            if accounts_to_perform_daily_login_with.len() == 0 {
                info!("[BGRSYNC][DL] No accounts to perform daily login with, exiting.");
                log_to_audit_log(
                    &self.pool,
                    "No accounts to perform daily login with, exiting.".to_string(),
                    None,
                );

                daily_login_report.hasFinished = true;
                daily_login_report.endTime = Some(Utc::now().to_rfc3339());
                daily_login_report.emailsToProcess = None;

                let _ = insert_or_update_daily_login_report(&self.pool, daily_login_report);

                self.event_hub
                    .emit(BackgroundSyncEvent::DailyLoginDone { id: Uuid::new_v4() });
                return;
            }

            daily_login_report.hasFinished = false;
            daily_login_report.endTime = None;

            let _ = insert_or_update_daily_login_report(&self.pool, daily_login_report.clone());

            let left = accounts_to_perform_daily_login_with.len();
            self.event_hub
                .emit(BackgroundSyncEvent::DailyLoginProgress {
                    id: Uuid::new_v4(),
                    done: 0,
                    left: left,
                    left_emails: accounts_to_perform_daily_login_with
                        .iter()
                        .map(|acc| acc.email.clone())
                        .collect::<Vec<String>>(),
                    failed_emails: Vec::new(),
                    estimated_time: self.get_estimated_time(left),
                });
        } else {
            if accounts_to_perform_daily_login_with.len() == 0 {
                info!("[BGRSYNC][DL] No accounts to perform daily login with, exiting.");
                log_to_audit_log(
                    &self.pool,
                    "No accounts to perform daily login with, exiting.".to_string(),
                    None,
                );
                return;
            }

            let report_uuid = Uuid::new_v4().to_string();
            daily_login_report = DailyLoginReports {
                id: report_uuid,
                startTime: Some(Utc::now().to_rfc3339()),
                endTime: None,
                hasFinished: false,
                emailsToProcess: Some(
                    accounts_to_perform_daily_login_with
                        .iter()
                        .map(|acc| acc.email.clone())
                        .collect::<Vec<String>>()
                        .join(", "),
                ),
                amountOfAccounts: accounts_to_perform_daily_login_with.len() as i32,
                amountOfAccountsProcessed: 0,
                amountOfAccountsFailed: 0,
                amountOfAccountsSucceeded: 0,
            };
            let _ = insert_or_update_daily_login_report(&self.pool, daily_login_report.clone());

            let left = accounts_to_perform_daily_login_with.len();
            self.event_hub
                .emit(BackgroundSyncEvent::DailyLoginProgress {
                    id: Uuid::new_v4(),
                    done: 0,
                    left: left,
                    left_emails: accounts_to_perform_daily_login_with
                        .iter()
                        .map(|acc| acc.email.clone())
                        .collect::<Vec<String>>(),
                    failed_emails: Vec::new(),
                    estimated_time: self.get_estimated_time(left),
                });
        }

        let game_exe_path = get_user_data_by_key(&self.pool, "game_exe_path".to_string())
            .unwrap()
            .dataValue;

        if game_exe_path.is_empty() {
            error!("[BGRSYNC][DL] No game.exe file found, exiting.");
            log_to_audit_log(
                &self.pool,
                "No game.exe file found, exiting.".to_string(),
                None,
            );
            return;
        }

        let mut emails_vec = accounts_to_perform_daily_login_with
            .iter()
            .map(|acc| acc.email.clone())
            .collect::<Vec<String>>();

        //END OF SETUP

        let mut last_account_finish_time = std::time::Instant::now();

        'account_loop: for account in accounts_to_perform_daily_login_with {
            let mut can_call = false;
            while !can_call {
                {
                    let limiter = self.api_limiter.lock().unwrap();
                    match (
                        limiter.api_limits("account/verify"),
                        limiter.api_limits("char/list"),
                    ) {
                        (Some((_limit_v, remain_v)), Some((_limit_c, remain_c))) => {
                            can_call = remain_v >= 2 && remain_c >= 2;
                        }
                        _ => {
                            can_call = false;
                        }
                    }
                }

                if !can_call {
                    info!("[BGRSYNC][DL] API limiter active, waiting...");
                    tokio::time::sleep(Duration::from_secs(10)).await;
                }
            }
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
            // Retry logic for inserting daily login report entry
            let entry_id = {
                let mut retry_count = 0;
                let max_retries = 3;
                loop {
                    let result =
                        insert_or_update_daily_login_report_entry(&self.pool, report_entry.clone());
                    match result {
                        Ok(id) => break id,
                        Err(e) => {
                            retry_count += 1;
                            let error_msg = e.to_string();
                            drop(e); // Explicitly drop the error

                            if retry_count >= max_retries {
                                error!("[BGRSYNC][DL] Failed to insert daily login report entry for account {} after {} retries: {}", account_email, max_retries, error_msg);
                                log_to_audit_log(
                                    &self.pool,
                                    format!("Failed to insert daily login report entry for account {} after {} retries: {}", account_email, max_retries, error_msg),
                                    Some(account_email.clone()),
                                );
                                continue 'account_loop;
                            }

                            let wait_time =
                                Duration::from_millis(100 * (2_u64.pow(retry_count - 1)));
                            info!("[BGRSYNC][DL] Failed to insert daily login report entry for account {} (attempt {}), retrying in {:?}: {}", account_email, retry_count, wait_time, error_msg);
                            tokio::time::sleep(wait_time).await;
                        }
                    }
                }
            };

            self.event_hub.emit(BackgroundSyncEvent::AccountStarted {
                id: Uuid::new_v4(),
                email: account_email.clone(),
            });

            // Perform login and immediately convert error to string to avoid Send issues
            let string_result = {
                let login_result = daily_login::perform_daily_login_for_account(
                    &self.pool,
                    account.clone(),
                    start_time.clone(),
                    entry_id.clone(),
                    daily_login_report.id.clone(),
                    self.hwid.clone(),
                    game_exe_path.clone(),
                    &self.event_hub,
                    Arc::clone(&self.api_limiter),
                    self.is_plus_user.clone(),
                )
                .await;

                // Convert immediately to avoid holding non-Send error type
                match login_result {
                    Ok(success) => Ok(success),
                    Err(e) => Err(e.to_string()),
                }
            };

            // Process the result in a separate function to avoid Send issues
            self.process_login_result(
                string_result,
                &mut daily_login_report,
                &mut emails_vec,
                entry_id,
                start_time,
                &account.email,
            )
            .await;

            // Ensure minimum 60 seconds between account completions
            let elapsed_since_last = last_account_finish_time.elapsed();
            let minimum_wait_time_in_seconds = if self.is_plus_user.load(Ordering::SeqCst) { 60 } else { 90 };
            let min_wait_time = Duration::from_secs(minimum_wait_time_in_seconds);

            if elapsed_since_last < min_wait_time {
                let remaining_wait = min_wait_time - elapsed_since_last;
                info!("[BGRSYNC][DL] Waiting {} seconds before processing next account (ensuring {}s minimum interval)...", remaining_wait.as_secs(), minimum_wait_time_in_seconds);

                self.event_hub.emit(BackgroundSyncEvent::AccountProgress {
                    id: Uuid::new_v4(),
                    email: account.email.clone(),
                    state: AccountProgressState::WaitingForCooldown,
                });
                
                tokio::time::sleep(remaining_wait).await;
            }
            
            // Update the last account finish time
            last_account_finish_time = std::time::Instant::now();
        }

        info!(
            "[BGRSYNC][DL] Finished daily login. {} accounts failed.",
            daily_login_report.amountOfAccountsFailed
        );
        log_to_audit_log(
            &self.pool,
            "Finished daily login. ".to_owned()
                + &daily_login_report.amountOfAccountsFailed.to_string()
                + " accounts failed.",
            None,
        );

        daily_login_report.hasFinished = true;
        daily_login_report.endTime = Some(Utc::now().to_rfc3339());
        daily_login_report.emailsToProcess = None;
        let _ = insert_or_update_daily_login_report(&self.pool, daily_login_report);

        self.event_hub
            .emit(BackgroundSyncEvent::DailyLoginDone { id: Uuid::new_v4() });

        tokio::time::sleep(Duration::from_secs(3)).await;

        info!("[BGRSYNC][DL] Daily login mode finished, switching back to default.");
        let _ = &self.switch_mode(SyncMode::Default);

        tokio::time::sleep(Duration::from_secs(3)).await;
    }

    /// Calculates the estimated time for the daily login based on the number of accounts left.
    /// For Plus users, it assumes 80 seconds per account, otherwise 102 seconds.
    /// The time per account is based on the average time it takes to process an account.
    /// 2 seconds for the web request
    /// 90 seconds for the game to start and process the daily login
    /// 60 seconds to wait for the API cooldown
    /// # Arguments:
    /// * `left` - The number of accounts left to process.
    /// # Returns: A DateTime<Utc> representing the estimated completion time.
    /// # Example:
    /// ```rust
    /// let estimated_time = manager.get_estimated_time(5);
    /// ```
    /// Returns the estimated time for processing 5 accounts.
    ///
    fn get_estimated_time(&self, left: usize) -> DateTime<Utc> {
        if left == 0 {
            return Utc::now();
        }

        if self.is_plus_user.load(Ordering::SeqCst) {
            return Utc::now() + Duration::from_secs(80 * left as u64);
        }
        Utc::now() + Duration::from_secs(102 * left as u64)
    }
}
