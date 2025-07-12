use serde::{Serialize, Deserialize};
use std::time::Duration;

/// Sync operation mode
#[derive(Debug, Clone, Copy, PartialEq, Eq, Serialize, Deserialize)]
pub enum SyncMode {
    Default,
    DailyLogin,
}

/// Speed preference for default mode syncing
#[derive(Debug, Clone, Copy, PartialEq, Eq, Serialize, Deserialize)]
pub enum SyncSpeed {
    Slow,
    Normal,
    Faster,
}

/// Configuration stored in the UserData table (as JSON string)
#[derive(Debug, Clone, Serialize, Deserialize)]
pub struct BackgroundSyncConfig {
    pub enabled: bool,
    pub speed: SyncSpeed,
}

/// Progress state during account sync operation
#[derive(Debug, Clone, Copy, PartialEq, Eq, Serialize, Deserialize)]
pub enum SyncProgressState {
    Verifying,
    FetchingCharList,
    Processing,
    Finished,
    Skipped,
    Failed,
}

impl ToString for SyncProgressState {
    fn to_string(&self) -> String {
        match self {
            SyncProgressState::Verifying => "Verifying".to_string(),
            SyncProgressState::FetchingCharList => "FetchingCharList".to_string(),
            SyncProgressState::Processing => "Processing".to_string(),
            SyncProgressState::Finished => "Finished".to_string(),
            SyncProgressState::Skipped => "Skipped".to_string(),
            SyncProgressState::Failed => "Failed".to_string(),
        }
    }
}

/// Result of syncing an account
#[derive(Debug, Clone, PartialEq, Eq, Serialize, Deserialize)]
pub enum SyncResult {
    Success,
    Skipped,
    Failed(String),
    RateLimited,
    Cancelled,
}

impl BackgroundSyncConfig {
    pub fn default_config() -> Self {
        Self {
            enabled: true,
            speed: SyncSpeed::Normal,
        }
    }

    pub fn delay_duration(&self) -> Duration {
        match self.speed {
            SyncSpeed::Slow => Duration::from_secs(90),
            SyncSpeed::Normal => Duration::from_secs(0),
            SyncSpeed::Faster => Duration::from_secs(0),
        }
    }
}

#[derive(Debug)]
pub enum ApiLimiterBlocked {
    CooldownActive,
    RateLimitHit,
    RequestFailed(String),
}

impl ToString for ApiLimiterBlocked {
    fn to_string(&self) -> String {
        match self {
            ApiLimiterBlocked::CooldownActive => "Cooldown active".to_string(),
            ApiLimiterBlocked::RateLimitHit => "Rate limit hit".to_string(),
            ApiLimiterBlocked::RequestFailed(msg) => {
                format!("Request failed: {}", msg)
            }
        }
    }
}

#[derive(Serialize, Deserialize, Clone)]
pub struct GameAccessToken {
    pub access_token: String,
    pub access_token_timestamp: String,
    pub access_token_expiration: String,
}