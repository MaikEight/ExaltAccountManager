use crate::diesel_setup::DbPool;
use crate::diesel_functions::{get_user_data_by_key, insert_or_update_user_data};
use crate::models::UserData;
use log::{error, info};
use serde_json;
use super::types::{BackgroundSyncConfig, SyncSpeed};

const CONFIG_KEY: &str = "background_sync_config";

/// Loads the background sync configuration from the UserData table.
/// If it doesn't exist, returns a default config.
pub fn load_config(pool: &DbPool) -> BackgroundSyncConfig {
    match get_user_data_by_key(pool, CONFIG_KEY.to_string()) {
        Ok(data) => {
            match serde_json::from_str::<BackgroundSyncConfig>(&data.dataValue) {
                Ok(parsed) => parsed,
                Err(e) => {
                    error!("Failed to parse background sync config JSON: {}", e);
                    BackgroundSyncConfig::default()
                }
            }
        }
        Err(_) => {
            info!("No existing config found; using default.");
            BackgroundSyncConfig::default()
        }
    }
}

/// Saves the current configuration to the UserData table.
pub fn save_config(pool: &DbPool, config: &BackgroundSyncConfig) {
    match serde_json::to_string(config) {
        Ok(json) => {
            let data = UserData {
                dataKey: CONFIG_KEY.to_string(),
                dataValue: json,
            };
            if let Err(e) = insert_or_update_user_data(pool, data) {
                error!("Failed to save background sync config: {}", e);
            }
        }
        Err(e) => {
            error!("Failed to serialize config to JSON: {}", e);
        }
    }
}
