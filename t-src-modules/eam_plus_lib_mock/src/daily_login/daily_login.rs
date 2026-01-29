use chrono::Utc;
use eam_commons::diesel_functions;
use eam_commons::encryption_utils;
use eam_commons::get_eam_account_by_email;
use eam_commons::limiter::manager::RateLimiterManager;
use eam_commons::models::CallResult;
use eam_commons::models::AuditLog;
use eam_commons::DbPool;
use reqwest::header::HeaderMap;
use reqwest::header::HeaderValue;
use reqwest::header::ACCEPT;
use reqwest::header::CONTENT_TYPE;
use roxmltree::Document;
use std::collections::HashMap;
use std::future::Future;
use std::pin::Pin;
use std::sync::{Arc, Mutex};
use std::thread;
use std::time::Duration;
use thiserror::Error;

#[derive(Clone, serde::Deserialize, serde::Serialize, Debug)]
pub struct GameAccessToken {
    pub access_token: String,
    pub access_token_timestamp: String,
    pub access_token_expiration: String,
}

lazy_static! {
    static ref DECA_API_TIMEOUT_UNTIL: Mutex<i64> = Mutex::new(Utc::now().timestamp_millis() - 1);
}

const BASE_URL: &str = "https://www.realmofthemadgod.com";

#[derive(Error, Debug, serde::Deserialize, serde::Serialize)]
pub enum DailyLoginError {
    #[error("🔴 User is not a plus user: {0}")]
    NotAPlusUserError(String),
    #[error("🟠 Rate limit reached, please wait: {0}")]
    RateLimitError(String),
    #[error("{0}")]
    FailedToGetAccessToken(String),
}

#[derive(Debug, serde::Deserialize, serde::Serialize)]
pub struct DailyLoginResult {
    pub email: String,
    pub success: bool,
    pub char_list: String,
}

#[derive(Debug, serde::Deserialize, serde::Serialize)]
pub struct AccountVerifyOutput {
    pub success: bool,
    pub message: Option<String>,
    pub access_token: Option<GameAccessToken>,
}

pub async fn perform_daily_login(
    _id_token: String,
    email: String,
    _hwid: String,
    _pool: &DbPool,
    _global_api_limiter: Arc<Mutex<RateLimiterManager>>,
) -> Result<DailyLoginResult, DailyLoginError> {
    println!("🔵 This version uses a EAM-PLUS Mock version and all plus features are disabled!");
    return Err(DailyLoginError::NotAPlusUserError(email.clone()));
}

pub fn send_account_verify_request(
    pool: &DbPool,
    account_email: String,
    hwid: String,
    global_api_limiter: Arc<Mutex<RateLimiterManager>>,
) -> Pin<Box<dyn Future<Output = AccountVerifyOutput> + Send>> {
    let pool = pool.clone();
    Box::pin(async move {
        let acc = get_eam_account_by_email(&pool, account_email.clone()).unwrap();
        let pw = encryption_utils::decrypt_data(&acc.password).unwrap();

        let url = format!("{}/account/verify", BASE_URL);
        let mut data = HashMap::new();

        data.insert("guid".to_string(), account_email.clone());
        if acc.isSteam {
            data.insert(
                "steamid".to_string(),
                acc.steamId.clone().unwrap_or_default().to_string(),
            );
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
                acc.steamId.clone().unwrap_or_default().to_string()
            } else {
                "".to_string()
            },
        );

        let response = send_post_request_with_form_url_encoded_data(url, data)
            .await
            .unwrap();
        let token = get_access_token(&response);

        if token.is_none() {
            //Check if the API Limit has been reached
            let doc = Document::parse(&response).unwrap();
            for node in doc.descendants() {
                if node.has_tag_name("Error") {
                    let error_message = node.text().map(|s| s.to_string()).unwrap();
                    if error_message.contains("please wait 5 minutes") {
                        let _ = log_to_audit_log(
                            &pool,
                            "API Limit reached, will be handled by upper-level retry logic.".to_string(),
                            Some(account_email.clone()),
                        );
                        println!("🟠 API Limit reached, will be handled by upper-level retry logic");
                        {
                            let mut limiter = global_api_limiter.lock().unwrap();
                            limiter.record_api_use("account/verify", CallResult::RateLimited);
                            limiter.trigger_cooldown();
                            drop(limiter);
                        }
                        return AccountVerifyOutput {
                            success: false,
                            message: Some("Rate limit reached - please wait 5 minutes".to_string()),
                            access_token: None,
                        };
                    } else {
                        log_to_audit_log(&pool, error_message.clone(), Some(account_email.clone()));
                        return AccountVerifyOutput {
                            success: false,
                            message: Some(error_message),
                            access_token: None,
                        };
                    }
                }
            }
        }
        {
            let mut limiter = global_api_limiter.lock().unwrap();
            limiter.record_api_use("account/verify", CallResult::Success);
        }
        AccountVerifyOutput {
            success: token.is_some(),
            message: token
                .is_none()
                .then(|| "Failed to get access token".to_string()),
            access_token: token,
        }
    })
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
            access_token.access_token = node.text().unwrap_or_default().to_string();
        }

        if node.has_tag_name("AccessTokenTimestamp") {
            access_token.access_token_timestamp = node.text().unwrap_or_default().to_string();
        }

        if node.has_tag_name("AccessTokenExpiration") {
            access_token.access_token_expiration = node.text().unwrap_or_default().to_string();
        }
    }

    if access_token.access_token.is_empty()
        || access_token.access_token_timestamp.is_empty()
        || access_token.access_token_expiration.is_empty()
    {
        return None;
    }

    Some(access_token)
}

pub fn send_char_list_request(
    access_token: GameAccessToken,
    account_email: String,
    pool: &DbPool,
    global_api_limiter: Arc<Mutex<RateLimiterManager>>,
) -> Pin<Box<dyn Future<Output = String> + Send>> {
    let url = format!("{}/char/list", BASE_URL);

    let mut data = HashMap::new();
    data.insert("accessToken".to_string(), access_token.clone().access_token);
    data.insert("game_net".to_string(), "Unity".to_string());
    data.insert("play_platform".to_string(), "Unity".to_string());
    data.insert("game_net_user_id".to_string(), "".to_string());
    data.insert("muleDump".to_string(), "true".to_string());
    data.insert("__source".to_string(), "ExaltAccountManager".to_string());

    let _ = log_to_audit_log(
        pool,
        "Sending char/list request.".to_string(),
        Some(account_email.clone()),
    );
    let pool = pool.clone();
    Box::pin(async move {
        let response = send_post_request_with_form_url_encoded_data(url, data)
            .await
            .unwrap();
        let token = get_access_token(&response);
        if token.is_none() {
            //Check if the API Limit has been reached
            let doc = Document::parse(&response).unwrap();
            for node in doc.descendants() {
                if node.has_tag_name("Error") {
                    let error_message = node.text().map(|s| s.to_string()).unwrap();
                    if error_message.contains("please wait 5 minutes") {
                        let _ = log_to_audit_log(
                            &pool,
                            "API Limit reached, will be handled by upper-level retry logic.".to_string(),
                            Some(account_email.clone()),
                        );
                        println!("🟠 API Limit reached, will be handled by upper-level retry logic");
                        {
                            let mut limiter = global_api_limiter.lock().unwrap();
                            limiter.record_api_use("char/list", CallResult::RateLimited);
                            limiter.trigger_cooldown();
                            drop(limiter);
                        }
                        return "Rate limit reached - please wait 5 minutes".to_string();
                    }
                }
            }
        }

        {
            let mut limiter = global_api_limiter.lock().unwrap();
            limiter.record_api_use("char/list", CallResult::Success);
        }
        
        response
    })
}

async fn send_post_request_with_form_url_encoded_data(
    url: String,
    data: HashMap<String, String>,
) -> Result<String, String> {
    let mut headers = HeaderMap::new();
    headers.insert(ACCEPT, HeaderValue::from_static("deflate, gzip"));
    headers.insert(
        CONTENT_TYPE,
        HeaderValue::from_static("application/x-www-form-urlencoded"),
    );

    let client = reqwest::Client::new();
    let res = client
        .post(&url)
        .headers(headers)
        .form(&data)
        .send()
        .await
        .map_err(|e| e.to_string())?;

    let body = res.text().await.map_err(|e| e.to_string())?;
    Ok(body)
}

fn log_to_audit_log(pool: &DbPool, message: String, account_email: Option<String>) {
    let log = AuditLog {
        id: None,
        sender: "eam_plus_lib".to_string(),
        message: message,
        accountEmail: account_email,
        time: "".to_string(),
    };
    
    // Retry up to 3 times with 25ms delay on database lock errors
    for attempt in 1..=3 {
        match diesel_functions::insert_audit_log(pool, log.clone()) {
            Ok(_) => return,
            Err(e) => {
                let error_str = e.to_string();
                if error_str.contains("database is locked") && attempt < 3 {
                    thread::sleep(Duration::from_millis(25));
                    continue;
                } else {
                    eprintln!("Failed to insert audit log after {} attempts: {}", attempt, e);
                    return;
                }
            }
        }
    }
}
