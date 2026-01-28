use crate::events::{AccountProgressState, BackgroundSyncEvent, BackgroundSyncEventHub};
use crate::types::{SyncResult};
use eam_commons::account_verify::send_account_verify_request;
use eam_commons::char_list::send_char_list_request;
use eam_commons::diesel_setup::DbPool;
use eam_commons::limiter::manager::RateLimiterManager;
use eam_commons::models::EamAccount;
use eam_commons::ApiLimiterBlocked;
use eam_commons::parser::RequestState;

use std::sync::{Arc, Mutex};
use uuid::Uuid;

/// Processes an account in the background syncer.
///
/// Emits:
/// - `AccountStarted`
/// - `AccountProgress` (for each step)
/// - Returns:
///     - `SyncResult`: success or failure with a message
///     - `Option<CharListDataset>`: result if char/list succeeded
pub async fn process_account(
    pool: Arc<DbPool>,
    account: EamAccount,
    hwid: String,
    event_hub: &BackgroundSyncEventHub,
    global_api_limiter: Arc<Mutex<RateLimiterManager>>,
) -> (SyncResult, Option<String>) {
    let email = account.email.clone();

    // Step 1: Fire AccountStarted
    event_hub.emit(BackgroundSyncEvent::AccountStarted {
        id: Uuid::new_v4(),
        email: email.clone(),
    });

    // Step 2: Fire AccountProgress → FetchingAccount
    event_hub.emit(BackgroundSyncEvent::AccountProgress {
        id: Uuid::new_v4(),
        email: email.clone(),
        state: AccountProgressState::FetchingAccount,
    });

    // Step 3: Perform account/verify
    let verify_result = send_account_verify_request(
        Arc::clone(&pool),
        email.clone(),
        hwid,
        Arc::clone(&global_api_limiter),
    )
    .await;

    let access_token = match verify_result {
        Ok((Some(token), RequestState::Success, _account_name)) => token,
        Ok((None, request_state, _)) => {
            let msg = format!("Account verify failed: {}", request_state);
            return (
                SyncResult::Failed(msg.clone()),
                Some(msg),
            );
        }
        Ok((Some(_), request_state, _)) => {
            // Received a token but not success state (unlikely but handle it)
            let msg = format!("Account verify returned non-success state: {}", request_state);
            return (
                SyncResult::Failed(msg.clone()),
                Some(msg),
            );
        }
        Err(err) => {
            match err {
                ApiLimiterBlocked::CooldownActive => {
                    return (SyncResult::RateLimited, None);
                }
                ApiLimiterBlocked::RateLimitHit => {
                    return (SyncResult::RateLimited, None);
                }
                ApiLimiterBlocked::RequestFailed(msg) => {
                    return (SyncResult::Failed(msg.clone()), Some(msg));
                }
            }
        }
    };

    // Step 4: Fire AccountProgress → FetchingCharList
    event_hub.emit(BackgroundSyncEvent::AccountProgress {
        id: Uuid::new_v4(),
        email: email.clone(),
        state: AccountProgressState::FetchingCharList,
    });

    // Step 5: Perform char/list
    let char_list_result = send_char_list_request(
        access_token,
        Arc::clone(&global_api_limiter),
    )
    .await;

    let char_list_response = match char_list_result {
        Ok(response) => response,
        Err(err) => {
            return (SyncResult::Failed(format!("Char list error: {}", err.to_string())), None);
        }
    };

    // Step 6: Fire AccountProgress → SyncingCharList
    event_hub.emit(BackgroundSyncEvent::AccountProgress {
        id: Uuid::new_v4(),
        email: email.clone(),
        state: AccountProgressState::SyncingCharList,
    });

    (SyncResult::Success, Some(char_list_response))
}
