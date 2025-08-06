use std::collections::HashMap;
use std::sync::{Arc, Mutex};

use crate::types::{ApiLimiterBlocked, GameAccessToken};
use crate::utils::send_post_request_with_form_url_encoded_data;
use eam_commons::diesel_functions::get_eam_account_by_email;
use eam_commons::diesel_setup::DbPool;
use eam_commons::encryption_utils;
use eam_commons::limiter::manager::RateLimiterManager;
use eam_commons::models::CallResult;

use roxmltree::Document;

const BASE_URL: &str = "https://www.realmofthemadgod.com";

pub async fn send_account_verify_request(
    pool: Arc<DbPool>,
    account_email: String,
    hwid: String,
    global_api_limiter: Arc<Mutex<RateLimiterManager>>,
) -> Result<Option<GameAccessToken>, ApiLimiterBlocked> {
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
    let token = get_access_token(&response);
    let mut limiter = global_api_limiter.lock().unwrap();

    match token {
        Some(t) => {
            limiter.record_api_use("account/verify", CallResult::Success);
            Ok(Some(t))
        }
        None => {
            // check for rate limit error in response
            let doc = Document::parse(&response).unwrap();
            for node in doc.descendants() {
                if node.has_tag_name("Error") {
                    let error_message = node.text().unwrap_or("").to_string();
                    if error_message.contains("please wait 5 minutes") {
                        limiter.record_api_use("account/verify", CallResult::RateLimited);
                        limiter.trigger_cooldown();
                        return Err(ApiLimiterBlocked::RateLimitHit);
                    }
                    Err(ApiLimiterBlocked::RequestFailed(response.clone())).map_err(|e| {
                        limiter.record_api_use("account/verify", CallResult::Failed);
                        e
                    })?;
                }
            }

            limiter.record_api_use("account/verify", CallResult::Failed);
            Err(ApiLimiterBlocked::RequestFailed(
                response.clone()
            ))
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
