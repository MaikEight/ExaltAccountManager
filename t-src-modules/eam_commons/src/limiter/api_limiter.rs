use crate::diesel_functions;
use crate::diesel_setup::DbPool;
use crate::models::CallResult;
use chrono::Utc;
use std::sync::Arc;

pub struct ApiLimiter {
    pub api_name: String,
    pub limit: usize,
    pub interval_secs: i64,
    pub pool: Arc<DbPool>,
}

impl ApiLimiter {
    pub fn is_allowed(&self, min_timestamp: Option<i64>) -> bool {
        let default_since = Utc::now().timestamp_millis() - (self.interval_secs * 1000);
        let since = min_timestamp
            .map(|t| t.max(default_since))
            .unwrap_or(default_since);

        let calls = diesel_functions::get_recent_requests(&self.pool, &self.api_name, since);
        calls.len() < self.limit
    }

    pub fn record(&self, result: CallResult) {
        let pool = Arc::clone(&self.pool);
        let _ = diesel_functions::insert_api_request(&pool, &self.api_name, result);
    }
}
