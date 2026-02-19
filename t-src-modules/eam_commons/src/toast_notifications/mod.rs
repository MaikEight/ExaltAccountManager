use serde::{Deserialize, Serialize};

#[cfg(target_os = "windows")]
mod windows;
#[cfg(target_os = "windows")]
pub use windows::{show_toast_notification, schedule_toast_notification, cancel_scheduled_toast_notification};

#[cfg(target_os = "macos")]
mod macos;
#[cfg(target_os = "macos")]
pub use macos::{show_toast_notification, schedule_toast_notification, cancel_scheduled_toast_notification};

#[cfg(not(any(target_os = "windows", target_os = "macos")))]
mod unsupported;
#[cfg(not(any(target_os = "windows", target_os = "macos")))]
pub use unsupported::{show_toast_notification, schedule_toast_notification, cancel_scheduled_toast_notification};

pub mod content;
pub use content::{get_end_of_month_notification_content, EndOfMonthNotificationContent};

/// An action button displayed on the toast notification.
#[derive(Debug, Clone, Serialize, Deserialize)]
pub struct ToastAction {
    /// The label text displayed on the button.
    pub label: String,
    /// The URL to open when the button is clicked (e.g., "eam://daily-login").
    pub action_url: String,
}

/// The full specification for a toast notification.
#[derive(Debug, Clone, Serialize, Deserialize)]
pub struct ToastNotification {
    /// The notification title.
    pub title: String,
    /// The notification body text.
    pub body: String,
    /// Absolute path to a hero image file (displayed prominently at the top on Windows).
    /// On macOS this is used as a notification attachment.
    pub hero_image_path: Option<String>,
    /// Absolute path to an icon/logo image (displayed inline on Windows, as app icon on macOS).
    pub icon_path: Option<String>,
    /// Optional action buttons for the notification.
    pub actions: Vec<ToastAction>,
}

/// Information about a successfully scheduled notification.
#[derive(Debug, Clone, Serialize, Deserialize)]
pub struct ScheduledNotificationInfo {
    /// The unique tag for this scheduled notification (used to cancel it).
    pub tag: String,
    /// ISO 8601 string of when the notification is scheduled to fire.
    pub scheduled_at: String,
    /// The notification title that will be displayed.
    pub title: String,
    /// The notification body text that will be displayed.
    pub body: String,
    /// The hero image path that will be used.
    pub hero_image_path: Option<String>,
}
