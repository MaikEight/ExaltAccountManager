use crate::background_syncer::events::{AccountProgressState, BackgroundSyncEvent, BackgroundSyncEventHub};
use crate::background_syncer::types::{SyncResult};
use crate::background_syncer::account_verify::send_account_verify_request;
use crate::background_syncer::char_list::send_char_list_request;
use crate::diesel_setup::DbPool;
use crate::limiter::manager::RateLimiterManager;
use crate::models::EamAccount;

use std::sync::{Arc, Mutex};

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
    event_hub.emit(BackgroundSyncEvent::AccountStarted(email.clone()));

    // Step 2: Fire AccountProgress → FetchingAccount
    event_hub.emit(BackgroundSyncEvent::AccountProgress(
        email.clone(),
        AccountProgressState::FetchingAccount,
    ));

    // Step 3: Perform account/verify
    let verify_result = send_account_verify_request(
        Arc::clone(&pool),
        email.clone(),
        hwid,
        Arc::clone(&global_api_limiter),
    )
    .await;

    let access_token = match verify_result {
        Ok(Some(token)) => token,
        Ok(None) => {
            return (
                SyncResult::Failed("No token returned from verify".into()),
                None,
            );
        }
        Err(err) => {
            return (SyncResult::Failed(format!("Verify error: {}", err.to_string())), None);
        }
    };

    // Step 4: Fire AccountProgress → FetchingCharList
    event_hub.emit(BackgroundSyncEvent::AccountProgress(
        email.clone(),
        AccountProgressState::FetchingCharList,
    ));

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
    event_hub.emit(BackgroundSyncEvent::AccountProgress(
        email.clone(),
        AccountProgressState::SyncingCharList,
    ));

    (SyncResult::Success, Some(char_list_response))
}
