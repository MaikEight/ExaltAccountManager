use reqwest::Error;
use serde::Deserialize;
use serde_json::Value;
use crate::constants;
use thiserror::Error;

#[derive(Error, Debug)]
pub enum FetchUserDataError {
    #[error("🔴 Request error: {0}")]
    RequestError(#[from] Error),
    #[error("🔴 Unexpected status code: {0}")]
    UnexpectedStatusCode(reqwest::StatusCode),
}

#[derive(Deserialize, Debug)]
#[allow(non_snake_case)]
pub struct Identity {
    pub provider: String,
    pub user_id: String,
    pub connection: String,
    pub isSocial: bool,
}

#[derive(Deserialize, Debug)]
pub struct UserData {
    pub created_at: String,
    pub email: String,
    pub email_verified: bool,
    pub identities: Vec<Identity>,
    pub name: String,
    pub nickname: String,
    pub picture: String,
    pub updated_at: String,
    pub user_id: String,
    pub user_metadata: Option<Value>,
    pub app_metadata: Option<Value>,
    pub last_ip: String,
    pub last_login: String,
    pub logins_count: u32,
}

pub async fn fetch_user_data(id_token: &str) -> Result<UserData, FetchUserDataError> {
    let url = format!("{}user", constants::EAM_USERMANAGEMENT_URL);
    let client = reqwest::Client::new();
    let response = client
        .get(&url)
        .bearer_auth(id_token)
        .send()
        .await?;

    if response.status() != reqwest::StatusCode::OK {
        return Err(FetchUserDataError::UnexpectedStatusCode(response.status()));
    }

    let user_data = response.json::<UserData>().await?;
    Ok(user_data)
}