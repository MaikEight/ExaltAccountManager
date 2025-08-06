use std::collections::HashMap;
use std::sync::{Arc, Mutex};

use eam_commons::limiter::manager::RateLimiterManager;
use eam_commons::models::CallResult;
use crate::types::{ApiLimiterBlocked, GameAccessToken};
use crate::utils::send_post_request_with_form_url_encoded_data;

const BASE_URL: &str = "https://www.realmofthemadgod.com";

/// Attempts to send a /char/list request using a given access token.
///
/// Returns:
/// - `Ok(response)` on success
/// - `Err(ApiLimiterBlocked::RateLimitHit)` if the API returned a rate limit error
/// - `Err(ApiLimiterBlocked::CooldownActive)` if local cooldown is active
/// - `Err(ApiLimiterBlocked::RequestFailed)` for other failures
pub async fn send_char_list_request(
    access_token: GameAccessToken,
    global_api_limiter: Arc<Mutex<RateLimiterManager>>,
) -> Result<String, ApiLimiterBlocked> {
    {
        let mut limiter = global_api_limiter.lock().unwrap();
        if !limiter.can_call("char/list") {
            return Err(ApiLimiterBlocked::CooldownActive);
        }
    }

    let url = format!("{}/char/list", BASE_URL);
    let mut data = HashMap::new();
    data.insert("accessToken".to_string(), access_token.access_token.clone());
    data.insert("game_net".to_string(), "Unity".to_string());
    data.insert("play_platform".to_string(), "Unity".to_string());
    data.insert("game_net_user_id".to_string(), "".to_string());
    data.insert("muleDump".to_string(), "true".to_string());
    data.insert("__source".to_string(), "ExaltAccountManager".to_string());

    let response = send_post_request_with_form_url_encoded_data(url, data)
        .await
        .map_err(|_| ApiLimiterBlocked::RequestFailed("Failed to send_post_request_with_form_url_encoded_data".to_string()))?;

    if response.contains("please wait 5 minutes") {
        let mut limiter = global_api_limiter.lock().unwrap();
        limiter.record_api_use("char/list", CallResult::RateLimited);
        limiter.trigger_cooldown();
        return Err(ApiLimiterBlocked::RateLimitHit);
    }

    let mut limiter = global_api_limiter.lock().unwrap();
    limiter.record_api_use("char/list", CallResult::Success);
    Ok(response)
}
