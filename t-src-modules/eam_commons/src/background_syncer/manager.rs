use std::sync::{Arc, Mutex};
use std::thread;
use std::time::Duration;

use crate::background_syncer::events::*;
use crate::background_syncer::process_account::process_account;
use crate::background_syncer::types::*;
use crate::background_syncer::utils::get_save_file_path;
use crate::diesel_functions::get_next_eam_account_for_background_sync;
use crate::diesel_setup::DbPool;
use create::hwid::get_device_unique_identifier;

#[derive(Clone)]
pub struct BackgroundSyncManager {
    pub pool: Arc<DbPool>,
    pub event_hub: BackgroundSyncEventHub,

    hwid: String,
    current_mode: Arc<Mutex<SyncMode>>,
    config: Arc<Mutex<BackgroundSyncConfig>>,
    should_stop: Arc<Mutex<bool>>,
}

impl BackgroundSyncManager {
    pub async fn new(pool: Arc<DbPool>, event_hub: BackgroundSyncEventHub) -> Self {
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
            event_hub,
            hwid,
            current_mode: Arc::new(Mutex::new(SyncMode::Default)),
            config: Arc::new(Mutex::new(BackgroundSyncConfig {
                enabled: true,
                speed: SyncSpeed::Normal,
            })),
            should_stop: Arc::new(Mutex::new(false)),
        }
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
    }

    pub fn start(&self) {
        let manager = self.clone();
        thread::spawn(move || {
            manager.run_loop();
        });
    }

    fn run_loop(&self) {
        loop {
            if *self.should_stop.lock().unwrap() {
                break;
            }

            let mode = self.current_mode.lock().unwrap().clone();
            match mode {
                SyncMode::Default => self.run_default_mode(),
                SyncMode::DailyLogin => self.run_daily_login_mode(),
            }

            thread::sleep(Duration::from_secs(1));
        }
    }

    fn run_default_mode(&self) {
        let config = self.config.lock().unwrap().clone();
        if !config.enabled {
            return;
        }

        loop {
            if *self.should_stop.lock().unwrap() {
                return;
            }

            let mut limiter = GLOBAL_API_LIMITER.lock().unwrap();
            let can_call = limiter.can_call("account/verify") && limiter.can_call("char/list");
            drop(limiter); // release lock early before sleeping

            if !can_call {
                info!("[BGRSYNC] API limiter active, waiting...");
                thread::sleep(Duration::from_secs(10));
                continue;
            }

            let account = match get_next_eam_account_for_background_sync(&self.pool) {
                Ok(Some(acc)) => acc,
                Ok(None) => return,
                Err(e) => {
                    log::error!("Failed to get next account: {}", e);
                    return;
                }
            };
            info!("[BGRSYNC] Processing account: {}", account.email);

            let (result, dataset_opt) = process_account(
                Arc::clone(&self.pool),
                account,
                hwid.clone(),
                &self.event_hub,
            );

            match &result {
                SyncResult::Success => {
                    info!("[BGRSYNC] Account {} synced successfully", email);
                    if let Some(dataset) = dataset_opt {
                        // TODO: emit dataset to frontend or store in DB
                    }
                }
                SyncResult::Failure(reason) => {
                    warn!("[BGRSYNC] Account {} failed: {}", email, reason);
                }
            }

            //TODO: handle result and emit events

            thread::sleep(Duration::from_secs(2));
        }
    }

    fn run_daily_login_mode(&self) {
        // TODO: implement
    }
}
