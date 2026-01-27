use serde::{Deserialize, Serialize};
use std::fmt;

#[derive(Debug)]
pub enum ApiLimiterBlocked {
    CooldownActive,
    RateLimitHit,
    RequestFailed(String),
}

impl fmt::Display for ApiLimiterBlocked {
    fn fmt(&self, f: &mut fmt::Formatter<'_>) -> fmt::Result {
        match self {
            ApiLimiterBlocked::CooldownActive => write!(f, "Cooldown active"),
            ApiLimiterBlocked::RateLimitHit => write!(f, "Rate limit hit"),
            ApiLimiterBlocked::RequestFailed(msg) => {
                write!(f, "Request failed: {}", msg)
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