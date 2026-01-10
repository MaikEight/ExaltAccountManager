use crate::eam_subscriptions_api_utils::FetchValidateJwtError;
use crate::eam_user_api_utils::UserData;
use eam_commons::DbPool;

pub async fn is_plus_user(_id_token: &str, _pool: &DbPool) -> bool {
    println!("🔵 This version uses a EAM-PLUS Mock version and all plus features are disabled!");
    false
}

pub fn is_plus_user_user_data_verification(_user_data: &UserData) -> bool {
    println!("🔵 This version uses a EAM-PLUS Mock version and all plus features are disabled!");
    false
}

pub async fn is_plus_user_jwt_online_validation(_jwt: &str) -> Result<bool, FetchValidateJwtError> {
    println!("🔵 This version uses a EAM-PLUS Mock version and all plus features are disabled!");
    Ok(false)
}

pub async fn is_plus_user_jwt_offline_validation(_jwt: &str) -> bool {
    println!("🔵 This version uses a EAM-PLUS Mock version and all plus features are disabled!");
    false
}