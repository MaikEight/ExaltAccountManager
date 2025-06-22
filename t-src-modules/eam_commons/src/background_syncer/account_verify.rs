use std::collections::HashMap;
use std::sync::Arc;
use std::thread;
use std::time::Duration;

use crate::background_syncer::types::GameAccessToken;
use crate::diesel_functions::get_eam_account_by_email;
use crate::diesel_setup::DbPool;
use crate::encryption_utils;
use crate::limiter::manager::GLOBAL_API_LIMITER;
use crate::models::CallResult;
use crate::background_syncer::utils::send_post_request_with_form_url_encoded_data;

use chrono::Utc;
use scraper::Html;
use std::pin::Pin;
use std::future::Future;

const BASE_URL: &str = "https://www.realmofthemadgod.com";

pub fn send_account_verify_request(
    pool: Arc<DbPool>,
    account_email: String,
    hwid: String,
) -> Pin<Box<dyn Future<Output = Result<Option<GameAccessToken>, ApiLimiterBlocked>> + Send>> {
    let pool = Arc::clone(&pool);

    Box::pin(async move {
        let mut limiter = GLOBAL_API_LIMITER.lock().unwrap();
        if !limiter.can_call("account/verify") {
            return Err(ApiLimiterBlocked::CooldownActive);
        }

        drop(limiter); // release lock early before async ops

        let acc = get_eam_account_by_email(&pool, account_email.clone()).ok_or(ApiLimiterBlocked::RequestFailed)?;
        let pw = encryption_utils::decrypt_data(&acc.password).ok_or(ApiLimiterBlocked::RequestFailed)?;

        let mut data = HashMap::new();
        data.insert("guid".to_string(), account_email.clone());

        if acc.isSteam {
            data.insert("steamid".to_string(), acc.steamId.clone().ok_or(ApiLimiterBlocked::RequestFailed)?);
            data.insert("secret".to_string(), pw);
        } else {
            data.insert("password".to_string(), pw);
        }

        data.insert("clientToken".to_string(), hwid.clone());
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
                acc.steamId.ok_or(ApiLimiterBlocked::RequestFailed)?
            } else {
                "".to_string()
            },
        );

        let url = format!("{}/account/verify", BASE_URL);
        let response = send_post_request_with_form_url_encoded_data(url, data)
            .await
            .map_err(|_| ApiLimiterBlocked::RequestFailed)?;

        let doc = Html::parse_document(&response);

        // Check for rate limit error
        if doc.root_element().text().any(|t| t.contains("wait 5 minutes")) {
            let mut limiter = GLOBAL_API_LIMITER.lock().unwrap();
            limiter.record_api_use("account/verify", CallResult::RateLimited);
            limiter.trigger_cooldown();
            return Err(ApiLimiterBlocked::RateLimitHit);
        }

        // Parse token
        let mut token = GameAccessToken {
            access_token: String::new(),
            access_token_timestamp: String::new(),
            access_token_expiration: String::new(),
        };

        for node in doc.root_element().descendants() {
            if let Some(el) = node.value().as_element() {
                let text = node.text().collect::<String>();
                match el.name() {
                    "AccessToken" => token.access_token = text,
                    "AccessTokenTimestamp" => token.access_token_timestamp = text,
                    "AccessTokenExpiration" => token.access_token_expiration = text,
                    _ => {}
                }
            }
        }

        let mut limiter = GLOBAL_API_LIMITER.lock().unwrap();
        if token.access_token.is_empty() {
            limiter.record_api_use("account/verify", CallResult::Failed);
            Ok(None)
        } else {
            limiter.record_api_use("account/verify", CallResult::Success);
            Ok(Some(token))
        }
    })
}