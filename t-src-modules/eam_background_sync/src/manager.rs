use chrono::{DateTime, Utc};
use log::{debug, error, info};
use std::fs::File;
use std::io::{BufRead, BufReader};
use std::path::{Path, PathBuf};
use std::sync::atomic::{AtomicBool, Ordering};
use std::sync::{Arc, Mutex};
use std::time::Duration;
use uuid::Uuid;

use crate::events::*;
use crate::process_account::process_account;
use crate::types::*;
use crate::utils::{get_save_file_path, log_to_audit_log};
use eam_commons::diesel_functions::{
    get_all_eam_accounts_for_daily_login, get_next_eam_account_for_background_sync,
};
use eam_commons::diesel_setup::DbPool;
use eam_commons::get_eam_account_by_email;
use eam_commons::get_latest_daily_login;
use eam_commons::get_user_data_by_key;
use eam_commons::hwid::get_device_unique_identifier;
use eam_commons::insert_or_update_daily_login_report;
use eam_commons::insert_or_update_daily_login_report_entry;
use eam_commons::limiter::manager::RateLimiterManager;
use eam_commons::models::EamAccount;
use eam_commons::models::{DailyLoginReportEntries, DailyLoginReports};

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
}

impl BackgroundSyncManager {
    pub async fn new(pool: Arc<DbPool>, api_limiter: Arc<Mutex<RateLimiterManager>>) -> Self {
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

        Self {
            pool,
            event_hub: BackgroundSyncEventHub::new(),
            api_limiter,
            hwid,
            current_mode: Arc::new(Mutex::new(SyncMode::Default)),
            config: Arc::new(Mutex::new(BackgroundSyncConfig {
                enabled: true,
                speed: SyncSpeed::Normal,
            })),
            should_stop: Arc::new(Mutex::new(false)),
            is_running: Arc::new(AtomicBool::new(false)),
        }
    }

    pub fn get_event_hub(&self) -> BackgroundSyncEventHub {
        self.event_hub.clone()
    }

    pub fn switch_mode(&self, mode: SyncMode) {
        let mut current = self.current_mode.lock().unwrap();
        if *current != mode {
            *current = mode.clone();
            self.event_hub
                .emit(BackgroundSyncEvent::ModeChanged(format!("{:?}", mode)));
        }
    }

    pub fn stop(&self) {
        let mut flag = self.should_stop.lock().unwrap();
        *flag = true;
        self.is_running.store(true, Ordering::SeqCst);
        info!("[BGRSYNC] Stopping background sync manager...");

        self.event_hub
            .emit(BackgroundSyncEvent::ModeChanged("Stopped".to_string()));
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
        self.event_hub
            .emit(BackgroundSyncEvent::ModeChanged("Started".to_string()));

        tokio::spawn(async move {
            manager.run_loop().await;
            manager.is_running.store(false, Ordering::SeqCst);
        });
    }

    async fn run_loop(&self) {
        loop {
            if *self.should_stop.lock().unwrap() {
                break;
            }

            let mode = self.current_mode.lock().unwrap().clone();
            match mode {
                SyncMode::Default => self.run_default_mode().await,
                SyncMode::DailyLogin => self.run_daily_login_mode(false).await,
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
                    info!("[BGRSYNC] No more accounts to process, waiting 30 seconds.");
                    tokio::time::sleep(Duration::from_secs(30)).await;
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

            if let SyncResult::Success = result {
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

            self.event_hub.emit(BackgroundSyncEvent::AccountFinished(
                account.email.clone(),
                format!("{:?}", result),
            ));

            tokio::time::sleep(Duration::from_secs(1)).await;
        }
    }

    /// Runs the daily login mode, which processes accounts for daily login rewards.
    /// FOR NON-PLUS USERS: The Game files need to be up-to-date for this to work.
    async fn run_daily_login_mode(&self, do_force_run: bool) {
        info!("[BGRSYNC] Running daily login mode...");

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
                let mut force_run = do_force_run;
                if daily_login_report.hasFinished {
                    if daily_login_report.amountOfAccountsProcessed
                        == daily_login_report.amountOfAccounts
                    {
                        if !force_run {
                            println!("Todays daily login did already run successfully, exiting.");
                            log_to_audit_log(
                                &self.pool,
                                "Todays daily login did already run successfully, exiting."
                                    .to_string(),
                                None,
                            );
                            return;
                        }

                        println!("Forcing daily login to run again.");
                        log_to_audit_log(
                            &self.pool,
                            "Forcing daily login to run again.".to_string(),
                            None,
                        );
                        is_force_run = true;
                    } else if daily_login_report.emailsToProcess != None {
                        println!("Last daily login did not finish processing all accounts, continuing...");
                        log_to_audit_log(&self.pool, "Last daily login did not finish processing all accounts, continuing...".to_string(), None);
                    } else {
                        if !force_run {
                            println!("Last daily login did finish, exiting.");
                            log_to_audit_log(
                                &self.pool,
                                "Last daily login did finish, exiting.".to_string(),
                                None,
                            );
                            return;
                        }

                        println!("Forcing daily login to run again.");
                        log_to_audit_log(
                            &self.pool,
                            "Forcing daily login to run again.".to_string(),
                            None,
                        );
                        is_force_run = true;
                    }
                } else {
                    println!("Last daily login did not finish, continuing...");
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
                println!("No accounts to perform daily login with.");
                log_to_audit_log(
                    &self.pool,
                    "No accounts to perform daily login with, exiting.".to_string(),
                    None,
                );

                daily_login_report.hasFinished = true;
                daily_login_report.endTime = Some(Utc::now().to_rfc3339());
                daily_login_report.emailsToProcess = None;

                let _ = insert_or_update_daily_login_report(&self.pool, daily_login_report);

                return;
            }

            daily_login_report.hasFinished = false;
            daily_login_report.endTime = None;

            let _ = insert_or_update_daily_login_report(&self.pool, daily_login_report.clone());
        } else {
            if accounts_to_perform_daily_login_with.len() == 0 {
                println!("No accounts to perform daily login with.");
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
        }

        let game_exe_path = get_user_data_by_key(&self.pool, "game_exe_path".to_string())
            .unwrap()
            .dataValue;

        if game_exe_path.is_empty() {
            println!("No game.exe path file found, exiting.");
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
            let entry_id_res = insert_or_update_daily_login_report_entry(&self.pool, report_entry);
            let entry_id = entry_id_res.unwrap();

            let login_result = perform_daily_login_for_account(
                &self.pool,
                account.clone(),
                start_time.clone(),
                entry_id.clone(),
                daily_login_report.id.clone(),
                self.hwid.clone(),
                game_exe_path.clone(),
            )
            .await;

            match login_result {
                Ok(_) => {
                    println!(
                        "Successfully performed daily login with account: {}",
                        account.email
                    );
                    log_to_audit_log(
                        &self.pool,
                        ("Successfully performed daily login with account: ".to_owned()
                            + &account.email)
                            .to_string(),
                        Some(account.email.clone()),
                    );

                    //Save the daily_login_report
                    let index = emails_vec.iter().position(|x| *x == account.email).unwrap();
                    emails_vec.remove(index);
                    daily_login_report.emailsToProcess = Some(emails_vec.join(", "));
                    daily_login_report.amountOfAccountsProcessed += 1;
                    daily_login_report.amountOfAccountsSucceeded += 1;
                    let _ =
                        insert_or_update_daily_login_report(&self.pool, daily_login_report.clone());
                }
                Err(e) => {
                    println!("Error while performing daily login: {}", e);
                    log_to_audit_log(
                        &self.pool,
                        ("Error while performing daily login: ".to_owned() + &e.to_string())
                            .to_string(),
                        None,
                    );

                    daily_login_report.amountOfAccountsFailed += 1;
                    daily_login_report.amountOfAccountsProcessed += 1;
                    let _ =
                        insert_or_update_daily_login_report(&self.pool, daily_login_report.clone());

                    let report_entry = DailyLoginReportEntries {
                        id: Some(entry_id),
                        reportId: Some(daily_login_report.id.clone()),
                        startTime: start_time.clone(),
                        endTime: Some(Utc::now().to_rfc3339()),
                        accountEmail: Some(account.email.clone()),
                        status: "Failed".to_string(),
                        errorMessage: Some(
                            "Failed due to unkown reason.".to_string()
                                + "Error thrown: "
                                + &e.to_string(),
                        ),
                    };
                    let _ = insert_or_update_daily_login_report_entry(&self.pool, report_entry);
                }
            }
        }

        println!("Finished daily login.");
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
    }
}
