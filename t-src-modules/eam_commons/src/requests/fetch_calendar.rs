use std::collections::HashMap;
use std::sync::{Arc, Mutex};

use crate::limiter::manager::RateLimiterManager;
use crate::models::CallResult;
use crate::types::{ApiLimiterBlocked, GameAccessToken};
use crate::utils::send_post_request_with_form_url_encoded_data;

const BASE_URL: &str = "https://www.realmofthemadgod.com";
const LIMITER_KEY: &str = "dailyLogin/fetchCalendar";

/// Sends a `dailyLogin/fetchCalendar` request using a given access token and
/// returns the raw XML body.
///
/// Returns:
/// - `Ok(response)` on success
/// - `Err(ApiLimiterBlocked::RateLimitHit)` if the API returned a rate limit error
/// - `Err(ApiLimiterBlocked::CooldownActive)` if local cooldown is active
/// - `Err(ApiLimiterBlocked::RequestFailed)` for other failures
pub async fn send_fetch_calendar_request(
    access_token: GameAccessToken,
    global_api_limiter: Arc<Mutex<RateLimiterManager>>,
) -> Result<String, ApiLimiterBlocked> {
    {
        let mut limiter = global_api_limiter.lock().unwrap();
        if !limiter.can_call(LIMITER_KEY) {
            return Err(ApiLimiterBlocked::CooldownActive);
        }
    }

    let url = format!("{}/dailyLogin/fetchCalendar", BASE_URL);
    let mut data = HashMap::new();
    data.insert("accessToken".to_string(), access_token.access_token.clone());
    data.insert("__source".to_string(), "ExaltAccountManager".to_string());

    let response = send_post_request_with_form_url_encoded_data(url, data)
        .await
        .map_err(|_| {
            ApiLimiterBlocked::RequestFailed(
                "Failed to send_post_request_with_form_url_encoded_data".to_string(),
            )
        })?;

    if response.contains("please wait 5 minutes") {
        let mut limiter = global_api_limiter.lock().unwrap();
        limiter.record_api_use(LIMITER_KEY, CallResult::RateLimited);
        limiter.trigger_cooldown();
        return Err(ApiLimiterBlocked::RateLimitHit);
    }

    let mut limiter = global_api_limiter.lock().unwrap();
    limiter.record_api_use(LIMITER_KEY, CallResult::Success);
    Ok(response)
}
