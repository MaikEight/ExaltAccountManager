use crate::background_syncer::events::BackgroundSyncEvent;
use crate::background_syncer::types::{SyncProgressState, SyncResult};
use crate::diesel_functions::get_eam_account_by_email;
use crate::models::{EamAccount, CharListDataset};
use crate::background_syncer::account_verify::send_account_verify_request;
use crate::background_syncer::char_list::send_char_list_request;
use crate::diesel_setup::DbPool;
use std::sync::Arc;

/// Returns a tuple of:
/// - SyncResult (Success / Failure with reason)
/// - Option<CharListDataset> if char/list succeeded
pub fn process_account(
    pool: Arc<DbPool>,
    account: EamAccount,
    hwid: String,
    event_hub: &BackgroundSyncEventHub,
) -> (SyncResult, Option<CharListDataset>) {
    let email = account.email.clone();

    // Step 1: Start event
    event_hub.emit(BackgroundSyncEvent::AccountStarted(email.clone()));

    // Step 2: Progress - FetchingAccount
    event_hub.emit(BackgroundSyncEvent::AccountProgress(
        email.clone(),
        SyncProgressState::FetchingAccount.to_string(),
    ));

    // Step 3: Verify
    let verify_result = send_account_verify_request(pool.clone(), email.clone(), hwid);

    let access_token = match verify_result {
        Ok(Some(token)) => token,
        Ok(None) => {
            return (
                SyncResult::Failure("No token returned from verify".into()),
                None,
            );
        }
        Err(err) => {
            return (SyncResult::Failure(format!("Verify error: {}", err)), None);
        }
    };

    // Step 4: Progress - FetchingCharList
    event_hub.emit(BackgroundSyncEvent::AccountProgress(
        email.clone(),
        SyncProgressState::FetchingCharList.to_string(),
    ));

    // Step 5: Char list
    let char_list_result =
        send_char_list_request(access_token.clone(), email.clone(), pool.clone());

    let char_list = match char_list_result {
        Ok(list) => list,
        Err(err) => {
            return (SyncResult::Failure(format!("Char list error: {}", err)), None);
        }
    };

    // Step 6: SyncingCharList
    event_hub.emit(BackgroundSyncEvent::AccountProgress(
        email.clone(),
        SyncProgressState::SyncingCharList.to_string(),
    ));

    // Step 7: Store to DB happens outside

    // Step 8: Finished
    event_hub.emit(BackgroundSyncEvent::AccountFinished(
        email,
        SyncResult::Success.to_string(),
    ));

    (SyncResult::Success, Some(char_list))
}
