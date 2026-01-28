use crate::limiter;
use crate::models;
use crate::parser;
use std::collections::HashMap;
use std::sync::{Arc, Mutex};

use crate::utils::send_post_request_with_form_url_encoded_data;
use limiter::manager::RateLimiterManager;
use models::CallResult;
use parser::{parse_char_list, parse_servers, parse_request_state, RequestState};
use crate::types::{ApiLimiterBlocked, GameAccessToken};
use crate::models::Server;

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
        .map_err(|_| {
            ApiLimiterBlocked::RequestFailed(
                "Failed to send_post_request_with_form_url_encoded_data".to_string(),
            )
        })?;

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

/// Sends a char/list request, parses the response and returns:
/// - `Ok((dataset, servers, request_state))` on success
/// - `Err(ApiLimiterBlocked)` on rate limit or request failures
pub async fn send_and_parse_char_list_request(
    email: &str,
    access_token: GameAccessToken,
    entry_id: Option<&str>,
    global_api_limiter: Arc<Mutex<RateLimiterManager>>,
) -> Result<(models::CharListDataset, Vec<Server>, RequestState), ApiLimiterBlocked> {
    let response = send_char_list_request(access_token, global_api_limiter).await?;

    // Parse request state first
    let request_state = parse_request_state(&response);
    
    // Parse servers (may be empty if error response)
    let servers = parse_servers(&response);

    // Parse char list dataset
    let dataset = parse_char_list(email, &response, entry_id).map_err(|e| {
        ApiLimiterBlocked::RequestFailed(format!("Failed to parse char list response: {}", e))
    })?;

    Ok((dataset, servers, request_state))
}
