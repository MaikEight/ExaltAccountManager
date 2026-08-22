use std::sync::{Arc, Mutex};

use chrono::{DateTime, Utc};
use log::{error, info};

use crate::diesel_functions;
use crate::diesel_setup::DbPool;
use crate::limiter::manager::RateLimiterManager;
use crate::models::NewAccountLoginReward;
use crate::parser::parse_login_rewards;
use crate::requests::send_fetch_calendar_request;
use crate::types::GameAccessToken;

/// Derives a "YYYY-MM" month key (UTC) from a raw `serverTime` attribute
/// (epoch seconds, possibly with a fractional part). Falls back to the current
/// month if the value can't be parsed.
fn month_from_server_time(server_time: &str) -> String {
    let secs = server_time
        .split('.')
        .next()
        .and_then(|s| s.trim().parse::<i64>().ok());
    let dt = secs
        .and_then(|s| DateTime::from_timestamp(s, 0))
        .unwrap_or_else(Utc::now);
    dt.format("%Y-%m").to_string()
}

/// Fetches the daily-login reward calendar for an account and stores it: the
/// shared global monthly calendar and the per-account claim/unlock status.
///
/// Fully non-fatal — any failure (rate limit, request, parse, DB) is logged and
/// swallowed so the surrounding char/list or daily-login flow still succeeds.
pub async fn fetch_and_store_login_calendar(
    pool: &DbPool,
    access_token: GameAccessToken,
    account_email: String,
    global_api_limiter: Arc<Mutex<RateLimiterManager>>,
) {
    let xml = match send_fetch_calendar_request(access_token, global_api_limiter).await {
        Ok(xml) => xml,
        Err(e) => {
            info!(
                "[LoginRewards] Skipping calendar fetch for {}: {}",
                account_email, e
            );
            return;
        }
    };

    let parsed = match parse_login_rewards(&xml) {
        Ok(p) => p,
        Err(e) => {
            error!(
                "[LoginRewards] Failed to parse calendar for {}: {}",
                account_email, e
            );
            return;
        }
    };

    let month = month_from_server_time(&parsed.server_time);
    let now = Utc::now().to_rfc3339();

    if let Err(e) =
        diesel_functions::upsert_login_rewards_calendar_for_month(pool, &month, &parsed.entries)
    {
        error!(
            "[LoginRewards] Failed to upsert calendar for month {}: {}",
            month, e
        );
    }

    let claimed_days = parsed
        .entries
        .iter()
        .filter(|e| e.claimed)
        .map(|e| e.day.to_string())
        .collect::<Vec<_>>()
        .join(",");

    let row = NewAccountLoginReward {
        account_email: account_email.clone(),
        month: month.clone(),
        unlockable_days: parsed.unlockable_days,
        claimed_days: Some(claimed_days),
        server_time: Some(parsed.server_time.clone()),
        updated_at: now,
    };

    if let Err(e) = diesel_functions::upsert_account_login_rewards(pool, row) {
        error!(
            "[LoginRewards] Failed to upsert account status for {} ({}): {}",
            account_email, month, e
        );
    } else {
        info!(
            "[LoginRewards] Stored calendar for {} ({}), {} logins this month",
            account_email, month, parsed.unlockable_days
        );
    }
}
