use reqwest::Error;
use serde::Deserialize;
use thiserror::Error;

#[derive(Error, Debug)]
pub enum FetchValidateJwtError {
    #[error("🔴 Request error: {0}")]
    RequestError(#[from] Error),
    #[error("🔴 Unexpected status code: {0}")]
    UnexpectedStatusCode(reqwest::StatusCode),
    #[error("🔴 Device ID error: {0}")]
    DeviceIdError(String),
}

#[derive(Deserialize, Debug, Clone)]
#[allow(non_snake_case)]
pub struct ValidationResponse {
    pub isValidToken: bool, 
    pub isSubscribed: bool,
    pub sub: String,
}


pub async fn validate_jwt(_jwt: &str) -> Result<ValidationResponse, FetchValidateJwtError> {
    return Err(FetchValidateJwtError::DeviceIdError("🔵 This version uses a EAM-PLUS Mock version and all plus features are disabled!".to_string()));
}
