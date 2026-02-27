use chrono::Datelike;
use serde::{Deserialize, Serialize};

/// The mascot name, matching the frontend constant.
#[allow(dead_code)]
const MASCOT_NAME: &str = "Okta";

/// End-of-month notification tag prefix. The full tag includes the year and month.
pub const END_OF_MONTH_TAG_PREFIX: &str = "eam-end-of-month-reminder";

/// Funny end-of-month reminder messages.
/// The selection is deterministic per month: `messages[month_index % len]`.
const END_OF_MONTH_MESSAGES: &[(&str, &str)] = &[
    // (title, body)
    (
        "Don't forget your daily login rewards!",
        "The month is almost over! Okta is tapping its foot impatiently — go grab those rewards before they vanish into the void.",
    ),
    (
        "Tick tock, rewards are waiting!",
        "Okta politely reminds you: daily login rewards expire with the month. Don't leave loot on the table!",
    ),
    (
        "Last chance for this month's rewards!",
        "Okta checked and you still have unclaimed daily login rewards. The calendar waits for no one — claim them now!",
    ),
    (
        "Okta's Monthly Reminder",
        "The month ends soon and so do your daily login rewards. Okta would never let good loot go to waste. Would you?",
    ),
    (
        "Rewards expiring soon!",
        "Psst! Okta here. Your daily login calendar is about to reset. Make sure you've grabbed everything before it's gone!",
    ),
    (
        "Monthly loot alert!",
        "Okta did the math — there's still time to claim your daily login rewards. But not much. Chop chop!",
    ),
    (
        "Your rewards miss you!",
        "Okta noticed some daily login rewards are feeling lonely. Give them a good home before the month resets!",
    ),
    (
        "Calendar reset incoming!",
        "Okta's crystal ball says the month is ending. Your daily login rewards won't survive the reset — rescue them now!",
    ),
];

/// The notification images to use for end-of-month reminders.
/// These are filenames relative to the mascot/Notification/ resource directory.
/// Selection: `images[month_index % len]`.
const END_OF_MONTH_IMAGES: &[&str] = &[
    "notification.png",
    "reminder.png",
];

/// Information returned about the end-of-month notification content.
#[derive(Debug, Clone, Serialize, Deserialize)]
pub struct EndOfMonthNotificationContent {
    /// The notification title.
    pub title: String,
    /// The notification body text.
    pub body: String,
    /// The filename of the hero image (relative to mascot/Notification/ resource dir).
    pub image_filename: String,
    /// The 1-based month number used for selection.
    pub month: u32,
    /// The year used for selection.
    pub year: i32,
    /// The ISO 8601 datetime string when the notification should fire.
    pub scheduled_at: String,
    /// The unique tag for this notification.
    pub tag: String,
}

/// Get the end-of-month notification content for the current (or next) month-end.
///
/// The text and image are chosen deterministically based on the month number,
/// so every device shows the same content for the same month.
///
/// Returns `None` if the current month's last day has already passed the notification time.
/// In that case, call with the next month.
pub fn get_end_of_month_notification_content(
    year: i32,
    month: u32,
) -> EndOfMonthNotificationContent {
    // Month-based index for deterministic selection (0-indexed)
    // Using (year * 12 + month) ensures different content across years too
    let month_index = (year as usize * 12 + month as usize) as usize;

    let (title, body) = END_OF_MONTH_MESSAGES[month_index % END_OF_MONTH_MESSAGES.len()];
    let image_filename = END_OF_MONTH_IMAGES[month_index % END_OF_MONTH_IMAGES.len()];

    // Calculate the last day of the month
    let last_day = last_day_of_month(year, month);

    // Schedule at ~12:00 local time on the last day
    let scheduled_at = format!("{:04}-{:02}-{:02}T12:00:00", year, month, last_day);

    let tag = format!("{}-{:04}-{:02}", END_OF_MONTH_TAG_PREFIX, year, month);

    EndOfMonthNotificationContent {
        title: title.to_string(),
        body: body.to_string(),
        image_filename: image_filename.to_string(),
        month,
        year,
        scheduled_at,
        tag,
    }
}

/// Get the last day of a given month.
fn last_day_of_month(year: i32, month: u32) -> u32 {
    // The first day of the next month minus one day
    let (next_year, next_month) = if month == 12 {
        (year + 1, 1)
    } else {
        (year, month + 1)
    };

    let next_month_first = chrono::NaiveDate::from_ymd_opt(next_year, next_month, 1)
        .expect("Invalid date in last_day_of_month");

    let last_day = next_month_first.pred_opt().expect("Failed to get previous day");
    last_day.day()
}

#[cfg(test)]
mod tests {
    use super::*;

    #[test]
    fn test_last_day_of_month() {
        assert_eq!(last_day_of_month(2026, 1), 31);
        assert_eq!(last_day_of_month(2026, 2), 28);
        assert_eq!(last_day_of_month(2024, 2), 29); // Leap year
        assert_eq!(last_day_of_month(2026, 4), 30);
        assert_eq!(last_day_of_month(2026, 12), 31);
    }

    #[test]
    fn test_deterministic_selection() {
        let content1 = get_end_of_month_notification_content(2026, 2);
        let content2 = get_end_of_month_notification_content(2026, 2);
        assert_eq!(content1.title, content2.title);
        assert_eq!(content1.body, content2.body);
        assert_eq!(content1.image_filename, content2.image_filename);
    }

    #[test]
    fn test_different_months_differ() {
        let content_feb = get_end_of_month_notification_content(2026, 2);
        let content_mar = get_end_of_month_notification_content(2026, 3);
        // Adjacent months should produce different content (since 8 messages > 2 adjacent months)
        assert_ne!(content_feb.title, content_mar.title);
    }

    #[test]
    fn test_tag_format() {
        let content = get_end_of_month_notification_content(2026, 2);
        assert_eq!(content.tag, "eam-end-of-month-reminder-2026-02");
    }

    #[test]
    fn test_scheduled_at_format() {
        let content = get_end_of_month_notification_content(2026, 2);
        assert_eq!(content.scheduled_at, "2026-02-28T12:00:00");
    }
}
