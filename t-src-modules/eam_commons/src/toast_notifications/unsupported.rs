use std::io::Error;

use super::{ScheduledNotificationInfo, ToastNotification};

/// Stub: Toast notifications are not supported on this platform.
pub fn show_toast_notification(_notification: &ToastNotification) -> Result<(), Error> {
    Err(Error::new(
        std::io::ErrorKind::Unsupported,
        "Toast notifications are not supported on this platform.",
    ))
}

/// Stub: Scheduled toast notifications are not supported on this platform.
pub fn schedule_toast_notification(
    _notification: &ToastNotification,
    _scheduled_at: &str,
    _tag: &str,
) -> Result<ScheduledNotificationInfo, Error> {
    Err(Error::new(
        std::io::ErrorKind::Unsupported,
        "Scheduled toast notifications are not supported on this platform.",
    ))
}

/// Stub: Cancelling scheduled toast notifications is not supported on this platform.
pub fn cancel_scheduled_toast_notification(_tag: &str) -> Result<bool, Error> {
    Err(Error::new(
        std::io::ErrorKind::Unsupported,
        "Cancelling scheduled toast notifications is not supported on this platform.",
    ))
}
