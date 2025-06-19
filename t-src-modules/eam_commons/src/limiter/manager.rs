use crate::limiter::api_limiter::ApiLimiter;
use chrono::{DateTime, Duration, Utc};
use std::collections::HashMap;

pub struct RateLimiterManager {
    pub cooldown_seconds: i64,
    pub sub_limiters: HashMap<String, ApiLimiter>,
    pub cooldown_until: Option<DateTime<Utc>>,
    pub last_cooldown_end: Option<DateTime<Utc>>,
    pub limited_endpoints: Vec<(String, String)>,
}

impl RateLimiterManager {
    pub fn is_cooldown(&self) -> bool {
        matches!(self.cooldown_until, Some(until) if Utc::now() < until)
    }

    pub fn trigger_cooldown(&mut self) {
        self.cooldown_until = Some(Utc::now() + Duration::seconds(self.cooldown_seconds));
    }

    pub fn can_call(&self, api: &str) -> bool {
        if self.is_cooldown() {
            return false;
        }

        let reset_ts = self
            .last_cooldown_end
            .filter(|end| *end <= Utc::now()) 
            .map(|dt| dt.timestamp_millis());

        self.sub_limiters
            .get(api)
            .map(|lim| lim.is_allowed(reset_ts))
            .unwrap_or(false)
    }

    pub fn register(&mut self, limiter: ApiLimiter) {
        self.sub_limiters.insert(limiter.api_name.clone(), limiter);
    }

    pub fn get_limiter_key_from_url(&self, url: &str) -> Option<String> {
        self.limited_endpoints
            .iter()
            .find(|(pattern, _)| url.contains(pattern))
            .map(|(_, key)| key.clone())
    }

    pub fn is_rate_limited_response(body: &str) -> bool {
        body.contains("try again later")
            || body.contains("5 minutes")
            || body.contains("too many requests")
    }
}
