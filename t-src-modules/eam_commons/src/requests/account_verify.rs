use std::collections::HashMap;
use std::sync::{Arc, Mutex};
use crate::diesel_functions;
use crate::diesel_setup;
use crate::encryption_utils;
use crate::limiter;
use crate::models;
use crate::parser::{parse_request_state, parse_account_name, RequestState};

use crate::types::{ApiLimiterBlocked, GameAccessToken};
use crate::utils::send_post_request_with_form_url_encoded_data;
use diesel_functions::get_eam_account_by_email;
use diesel_setup::DbPool;
use limiter::manager::RateLimiterManager;
use models::CallResult;

use roxmltree::Document;

const BASE_URL: &str = "https://www.realmofthemadgod.com";

/// Sends an account/verify request and returns:
/// - `Ok((Some(token), RequestState::Success, Some(name)))` on success
/// - `Ok((None, request_state, None))` on API errors (wrong password, captcha, etc.)
/// - `Err(ApiLimiterBlocked)` on rate limit or request failures
pub async fn send_account_verify_request(
    pool: Arc<DbPool>,
    account_email: String,
    hwid: String,
    global_api_limiter: Arc<Mutex<RateLimiterManager>>,
) -> Result<(Option<GameAccessToken>, RequestState, Option<String>), ApiLimiterBlocked> {
    // Step 1: Check if the API can be called
    {
        let mut limiter = global_api_limiter.lock().unwrap();
        if !limiter.can_call("account/verify") {
            return Err(ApiLimiterBlocked::CooldownActive);
        }
    }

    // Step 2: Get account from DB
    let acc = get_eam_account_by_email(&pool, account_email.clone()).map_err(|_| {
        ApiLimiterBlocked::RequestFailed("Failed to get_eam_account_by_email".to_string())
    })?;

    let pw = encryption_utils::decrypt_data(&acc.password)
        .map_err(|_| ApiLimiterBlocked::RequestFailed("Failed to decrypt_data".to_string()))?;

    // Step 3: Build request payload
    let mut data = HashMap::new();
    data.insert("guid".to_string(), account_email.clone());

    if acc.isSteam {
        data.insert(
            "steamid".to_string(),
            acc.steamId.clone().ok_or(ApiLimiterBlocked::RequestFailed(
                "Failed to clone acc_steamId".to_string(),
            ))?,
        );
        data.insert("secret".to_string(), pw);
    } else {
        data.insert("password".to_string(), pw);
    }

    data.insert("clientToken".to_string(), hwid);
    data.insert(
        "game_net".to_string(),
        if acc.isSteam { "Unity_steam" } else { "Unity" }.to_string(),
    );
    data.insert(
        "play_platform".to_string(),
        if acc.isSteam { "Unity_steam" } else { "Unity" }.to_string(),
    );
    data.insert(
        "game_net_user_id".to_string(),
        if acc.isSteam {
            acc.steamId.ok_or(ApiLimiterBlocked::RequestFailed(
                "Failed to clone isSteam".to_string(),
            ))?
        } else {
            "".to_string()
        },
    );

    // Step 4: Send request
    let url = format!("{}/account/verify", BASE_URL);
    let response = send_post_request_with_form_url_encoded_data(url, data)
        .await
        .map_err(|_| {
            ApiLimiterBlocked::RequestFailed(
                "Failed to send_post_request_with_form_url_encoded_data".to_string(),
            )
        })?;

    // Step 5: Parse response
    let request_state = parse_request_state(&response);
    let account_name = parse_account_name(&response);
    let token = get_access_token(&response);
    
    let mut limiter = global_api_limiter.lock().unwrap();

    match request_state {
        RequestState::Success => {
            limiter.record_api_use("account/verify", CallResult::Success);
            Ok((token, RequestState::Success, account_name))
        }
        RequestState::TooManyRequests => {
            limiter.record_api_use("account/verify", CallResult::RateLimited);
            limiter.trigger_cooldown();
            Err(ApiLimiterBlocked::RateLimitHit)
        }
        _ => {
            // For all other error states (WrongPassword, Captcha, etc.),
            // return the state but don't consider it a "failed" API call for rate limiting
            limiter.record_api_use("account/verify", CallResult::Failed);
            Ok((None, request_state, None))
        }
    }
}

fn get_access_token(xml: &str) -> Option<GameAccessToken> {
    let doc = Document::parse(xml).ok()?;
    let mut access_token = GameAccessToken {
        access_token: "".to_string(),
        access_token_timestamp: "".to_string(),
        access_token_expiration: "".to_string(),
    };

    for node in doc.descendants() {
        if node.has_tag_name("AccessToken") {
            access_token.access_token = node.text()?.to_string();
        } else if node.has_tag_name("AccessTokenTimestamp") {
            access_token.access_token_timestamp = node.text()?.to_string();
        } else if node.has_tag_name("AccessTokenExpiration") {
            access_token.access_token_expiration = node.text()?.to_string();
        }
    }

    if access_token.access_token.is_empty()
        || access_token.access_token_timestamp.is_empty()
        || access_token.access_token_expiration.is_empty()
    {
        None
    } else {
        Some(access_token)
    }
}
