use log::{info, warn};
use std::io::Error;

use objc2::rc::Retained;
use objc2::runtime::Bool;
use objc2_foundation::{
    NSArray, NSBundle, NSDateComponents, NSError, NSSet, NSString, NSURL,
};
use objc2_user_notifications::{
    UNAuthorizationOptions, UNCalendarNotificationTrigger, UNMutableNotificationContent,
    UNNotificationAction, UNNotificationActionOptions, UNNotificationCategory,
    UNNotificationCategoryOptions, UNNotificationRequest, UNNotificationSound,
    UNTimeIntervalNotificationTrigger, UNUserNotificationCenter,
};
use block2::RcBlock;
use chrono::{Datelike, Timelike};

use super::{ScheduledNotificationInfo, ToastAction, ToastNotification};

/// Show a toast notification immediately.
///
/// On macOS, this uses `UNUserNotificationCenter` with a minimal time-interval trigger
/// (1 second) to display the notification right away.
#[cfg(target_os = "macos")]
pub fn show_toast_notification(notification: &ToastNotification) -> Result<(), Error> {
    info!("Showing toast notification on macOS: {}", notification.title);

    ensure_notification_authorization()?;

    let content = build_notification_content(notification)?;

    // Fire in 1 second (UNTimeIntervalNotificationTrigger requires > 0)
    let trigger =
        UNTimeIntervalNotificationTrigger::triggerWithTimeInterval_repeats(1.0, false);

    let identifier = NSString::from_str(&format!("eam-instant-{}", uuid::Uuid::new_v4()));

    let request = UNNotificationRequest::requestWithIdentifier_content_trigger(
        &identifier,
        &content,
        Some(&trigger),
    );

    let center = get_notification_center()?;

    // Use a simple synchronous approach: add the request and log errors
    // UNUserNotificationCenter's addNotificationRequest runs asynchronously,
    // but we fire-and-forget here since there's no meaningful recovery.
    center.addNotificationRequest_withCompletionHandler(&request, None);

    info!("Toast notification requested on macOS.");
    Ok(())
}

/// Schedule a toast notification for a future date/time.
///
/// * `notification` — The notification content.
/// * `scheduled_at` — ISO 8601 datetime string in local time (e.g. "2026-02-28T12:00:00").
/// * `tag` — A unique identifier for this scheduled notification (used to cancel it).
#[cfg(target_os = "macos")]
pub fn schedule_toast_notification(
    notification: &ToastNotification,
    scheduled_at: &str,
    tag: &str,
) -> Result<ScheduledNotificationInfo, Error> {
    info!(
        "Scheduling toast notification on macOS for {} with tag '{}'.",
        scheduled_at, tag
    );

    ensure_notification_authorization()?;

    let content = build_notification_content(notification)?;

    // Parse the datetime
    let dt = chrono::NaiveDateTime::parse_from_str(scheduled_at, "%Y-%m-%dT%H:%M:%S")
        .or_else(|_| chrono::NaiveDateTime::parse_from_str(scheduled_at, "%Y-%m-%dT%H:%M:%S%.f"))
        .map_err(|e| {
            Error::new(
                std::io::ErrorKind::InvalidInput,
                format!("Failed to parse scheduled_at datetime '{}': {}", scheduled_at, e),
            )
        })?;

    // Build NSDateComponents for the calendar trigger
    let date_components = {
        let components = NSDateComponents::new();
        components.setYear(dt.date().year() as isize);
        // chrono months are 1-based, same as NSDateComponents
        components.setMonth(dt.date().month() as isize);
        components.setDay(dt.date().day() as isize);
        components.setHour(dt.time().hour() as isize);
        components.setMinute(dt.time().minute() as isize);
        components.setSecond(dt.time().second() as isize);
        components
    };

    let trigger = UNCalendarNotificationTrigger::triggerWithDateMatchingComponents_repeats(
        &date_components,
        false, // Don't repeat
    );

    let identifier = NSString::from_str(tag);

    let request = UNNotificationRequest::requestWithIdentifier_content_trigger(
        &identifier,
        &content,
        Some(&trigger),
    );

    let center = get_notification_center()?;

    center.addNotificationRequest_withCompletionHandler(&request, None);

    info!(
        "Toast notification scheduled on macOS for {} with tag '{}'.",
        scheduled_at, tag
    );

    Ok(ScheduledNotificationInfo {
        tag: tag.to_string(),
        scheduled_at: scheduled_at.to_string(),
        title: notification.title.clone(),
        body: notification.body.clone(),
        hero_image_path: notification.hero_image_path.clone(),
    })
}

/// Cancel a previously scheduled toast notification by its tag.
#[cfg(target_os = "macos")]
pub fn cancel_scheduled_toast_notification(tag: &str) -> Result<bool, Error> {
    info!(
        "Cancelling scheduled toast notification on macOS with tag '{}'.",
        tag
    );

    let center = get_notification_center()?;

    let identifier = NSString::from_str(tag);
    let identifiers = NSArray::from_retained_slice(&[identifier]);

    center.removePendingNotificationRequestsWithIdentifiers(&identifiers);

    info!("Cancelled scheduled notification with tag '{}' on macOS.", tag);
    Ok(true)
}

// ---------------------------------------------------------------------------
// Internal helpers
// ---------------------------------------------------------------------------

/// Safely obtain the `UNUserNotificationCenter`.
///
/// `currentNotificationCenter()` throws an Objective-C exception
/// (`NSInternalInconsistencyException`) when the binary is not running
/// inside a proper `.app` bundle (e.g. during `cargo run` / `tauri dev`).
/// We detect this up-front by checking for a bundle identifier.
fn get_notification_center() -> Result<Retained<UNUserNotificationCenter>, Error> {
    let bundle = NSBundle::mainBundle();
    if bundle.bundleIdentifier().is_none() {
        return Err(Error::new(
            std::io::ErrorKind::Unsupported,
            "UNUserNotificationCenter is unavailable: the process is not \
             running inside an application bundle (missing bundle identifier). \
             Notifications are only supported in bundled .app builds.",
        ));
    }
    Ok(UNUserNotificationCenter::currentNotificationCenter())
}

/// Request notification authorization from the user (if not already granted).
/// This is a best-effort operation — if the user denies, we still proceed
/// (the notification just won't be displayed).
fn ensure_notification_authorization() -> Result<(), Error> {
    let center = get_notification_center()?;

    // Request alert + sound + badge authorization
    // This is idempotent — if already authorized, it returns immediately.
    let options = UNAuthorizationOptions::Alert
        | UNAuthorizationOptions::Sound
        | UNAuthorizationOptions::Badge;

    let handler = RcBlock::new(|_granted: Bool, _error: *mut NSError| {
        // Fire-and-forget: nothing to do on completion
    });
    center.requestAuthorizationWithOptions_completionHandler(options, &handler);

    Ok(())
}

/// Build the `UNMutableNotificationContent` from our cross-platform `ToastNotification`.
fn build_notification_content(
    notification: &ToastNotification,
) -> Result<Retained<UNMutableNotificationContent>, Error> {
    let content = UNMutableNotificationContent::new();

    let title = NSString::from_str(&notification.title);
    let body = NSString::from_str(&notification.body);

    content.setTitle(&title);
    content.setBody(&body);
    content.setSound(Some(&UNNotificationSound::defaultSound()));

    // Add image attachment if provided
    if let Some(ref hero_path) = notification.hero_image_path {
        match create_image_attachment(hero_path) {
            Ok(attachment) => {
                let attachments = NSArray::from_retained_slice(&[attachment]);
                content.setAttachments(&attachments);
            }
            Err(e) => {
                warn!(
                    "Failed to attach hero image '{}' to notification: {}. Continuing without image.",
                    hero_path, e
                );
            }
        }
    }

    // Register action category if there are actions
    if !notification.actions.is_empty() {
        register_notification_category(&notification.actions)?;
        let category_id = NSString::from_str("eam-notification-actions");
        content.setCategoryIdentifier(&category_id);
    }

    Ok(content)
}

/// Create a `UNNotificationAttachment` from a local file path.
fn create_image_attachment(
    image_path: &str,
) -> Result<Retained<objc2_user_notifications::UNNotificationAttachment>, Error> {
    let path_str = NSString::from_str(image_path);
    let url = NSURL::fileURLWithPath(&path_str);

    let identifier = NSString::from_str("hero-image");

    let attachment = unsafe {
        objc2_user_notifications::UNNotificationAttachment::attachmentWithIdentifier_URL_options_error(
            &identifier,
            &url,
            None,
        )
    }
    .map_err(|e| {
        Error::new(
            std::io::ErrorKind::Other,
            format!("Failed to create notification attachment: {:?}", e),
        )
    })?;

    Ok(attachment)
}

/// Register a notification category with the specified actions.
/// macOS requires categories to be registered ahead of time for action buttons to work.
fn register_notification_category(actions: &[ToastAction]) -> Result<(), Error> {
    let center = get_notification_center()?;

    let mut ns_actions: Vec<Retained<UNNotificationAction>> = Vec::new();

    for (i, action) in actions.iter().enumerate() {
        let identifier = NSString::from_str(&format!("action-{}", i));
        let title = NSString::from_str(&action.label);

        let ns_action = UNNotificationAction::actionWithIdentifier_title_options(
            &identifier,
            &title,
            UNNotificationActionOptions::Foreground,
        );
        ns_actions.push(ns_action);
    }

    let action_refs: Vec<&UNNotificationAction> = ns_actions.iter().map(|a| a.as_ref()).collect();
    let actions_array = NSArray::from_slice(&action_refs);

    let category_id = NSString::from_str("eam-notification-actions");

    let category =
        UNNotificationCategory::categoryWithIdentifier_actions_intentIdentifiers_options(
            &category_id,
            &actions_array,
            &NSArray::new(),
            UNNotificationCategoryOptions::empty(),
        );

    let categories_set = NSSet::from_retained_slice(&[category]);

    center.setNotificationCategories(&categories_set);

    Ok(())
}


