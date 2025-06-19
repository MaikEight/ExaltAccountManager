use crate::limiter::api_limiter::ApiLimiter;
use crate::limiter::manager::RateLimiterManager;
use crate::DbPool;
use std::collections::HashMap;
use std::sync::Arc;

pub fn setup(pool: Arc<DbPool>) -> RateLimiterManager {
    let mut sub_limiters = HashMap::new();

    let account_verify = ApiLimiter {
        api_name: "account/verify".to_string(),
        limit: 30,
        interval_secs: 5 * 60, // 5 minutes
        pool: pool.clone(),
    };

    let char_list = ApiLimiter {
        api_name: "char/list".to_string(),
        limit: 5,
        interval_secs: 5 * 60, // 5 minutes
        pool: pool.clone(),
    };

    let account_register = ApiLimiter {
        api_name: "account/register".to_string(),
        limit: 3,
        interval_secs: 5 * 60, // 5 minutes
        pool: pool.clone(),
    };

    sub_limiters.insert(account_verify.api_name.clone(), account_verify);
    sub_limiters.insert(char_list.api_name.clone(), char_list);
    sub_limiters.insert(account_register.api_name.clone(), account_register);

    let limited_endpoints = vec![
        ("account/verify".to_string(), "account/verify".to_string()),
        ("char/list".to_string(), "char/list".to_string()),
        ("account/register".to_string(), "account/register".to_string()),
    ];

    RateLimiterManager {
        cooldown_seconds: 5 * 60 + 15, // 5 minutes + 15 seconds as a buffer
        sub_limiters,
        cooldown_until: None,
        last_cooldown_end: None,
        limited_endpoints: limited_endpoints,
    }
}
