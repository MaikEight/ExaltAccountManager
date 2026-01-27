use reqwest::header::{HeaderMap, HeaderValue, ACCEPT, CONTENT_TYPE};
use std::collections::HashMap;
use crate::DbPool;
use crate::diesel_functions;
use crate::models;

use diesel_functions::insert_audit_log;
use models::AuditLog;

/// Sends a POST request with form-url-encoded data and returns the response body as a String.
pub async fn send_post_request_with_form_url_encoded_data(
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

    res.text().await.map_err(|e| e.to_string())
}

pub fn log_to_audit_log(pool: &DbPool, message: String, account_email: Option<String>) {
    let log = AuditLog {
        id: None,
        sender: "BGRSYNC".to_string(),
        message: message,
        accountEmail: account_email,
        time: "".to_string(),
    };
    let _ = insert_audit_log(pool, log).unwrap();
}
