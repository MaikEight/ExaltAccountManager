use std::fs::File;
use std::io::{BufRead, BufReader};
use std::path::{Path, PathBuf};
use std::sync::atomic::{AtomicBool, Ordering};
use std::sync::{Arc, Mutex};
use std::time::Duration;
use uuid::Uuid;
use log::{debug, error, info};

use crate::background_syncer::events::*;
use crate::background_syncer::process_account::process_account;
use crate::background_syncer::types::*;
use crate::background_syncer::utils::get_save_file_path;
use crate::diesel_functions::get_next_eam_account_for_background_sync;
use crate::diesel_setup::DbPool;
use crate::hwid::get_device_unique_identifier;
use crate::limiter::manager::RateLimiterManager;

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

        self.event_hub.emit(BackgroundSyncEvent::ModeChanged("Stopped".to_string()));
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
                SyncMode::DailyLogin => self.run_daily_login_mode().await,
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
                Ok(None) => return,
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

    async fn run_daily_login_mode(&self) {
        // TODO: implement
    }
}
