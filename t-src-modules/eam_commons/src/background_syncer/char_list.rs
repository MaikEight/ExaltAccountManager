use std::collections::HashMap;
use std::sync::Arc;

use crate::api_limiter::{CallResult, GLOBAL_API_LIMITER};
use crate::diesel_setup::DbPool;
use crate::models::ApiRequestStatus;
use crate::networking_utils::send_post_request_with_form_url_encoded_data;
use crate::types::GameAccessToken;

use quick_xml::Document;

/// Attempts to send a /char/list request using a given access token.
///
/// Returns `Ok(Some(response))` on success,
/// `Ok(None)` if a rate limit was hit (i.e., retry possible),
/// or `Err(reason)` if the request failed for another reason.
pub async fn send_char_list_request(
    access_token: GameAccessToken,
    account_email: String,
    pool: Arc<DbPool>,
) -> ApiRequestStatus<String> {
    let mut limiter = GLOBAL_API_LIMITER.lock().unwrap();
    if !limiter.can_call("char/list") {
        return ApiRequestStatus::RateLimited;
    }

    drop(limiter); // release lock early before async ops

    let url = format!("{}/char/list", crate::constants::BASE_URL);
    let mut data = HashMap::new();
    data.insert("do_login".to_string(), "true".to_string());
    data.insert("accessToken".to_string(), access_token.access_token.clone());
    data.insert("game_net".to_string(), "Unity".to_string());
    data.insert("play_platform".to_string(), "Unity".to_string());
    data.insert("game_net_user_id".to_string(), "".to_string());
    data.insert("muleDump".to_string(), "true".to_string());
    data.insert("__source".to_string(), "ExaltAccountManager".to_string());

    let response = match send_post_request_with_form_url_encoded_data(url, data).await {
        Ok(response) => response,
        Err(e) => {
            let mut limiter = GLOBAL_API_LIMITER.lock().unwrap();
            limiter.record_api_use("char/list", CallResult::Failed);
            return ApiRequestStatus::Failed(e);
        }
    };

    if response.contains("Internal error, please wait 5 minutes to try again!") {
        let mut limiter = GLOBAL_API_LIMITER.lock().unwrap();
        limiter.record_api_use("char/list", CallResult::RateLimited);
        limiter.trigger_cooldown();
        return ApiRequestStatus::RateLimited;
    }

    let mut limiter = GLOBAL_API_LIMITER.lock().unwrap();
    limiter.record_api_use("char/list", CallResult::Success);
    ApiRequestStatus::Success(response)
}
