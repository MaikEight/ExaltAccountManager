use chrono::TimeZone;
use log::{error, info, warn};
use std::io::Error;
use std::sync::Once;

use windows::core::{Interface, HSTRING, PROPVARIANT};
use windows::Data::Xml::Dom::XmlDocument;
use windows::UI::Notifications::{
    ScheduledToastNotification, ToastNotification as WinToastNotification,
    ToastNotificationManager,
};
use windows::Win32::System::Com::{
    CoCreateInstance, CoInitializeEx, CLSCTX_INPROC_SERVER, COINIT_MULTITHREADED,
};
use windows::Win32::UI::Shell::{IShellLinkW, ShellLink};
use windows::Win32::UI::Shell::PropertiesSystem::IPropertyStore;
use windows::Win32::Storage::EnhancedStorage::PKEY_AppUserModel_ID;

use super::{ScheduledNotificationInfo, ToastNotification};

/// The Application User Model ID for EAM.
/// Must match the AUMID set on Start Menu shortcuts by the NSIS installer.
/// This comes from `identifier` in `tauri.conf.json`.
const APP_USER_MODEL_ID: &str = "eu.exalt-account-manager.app";

/// Display name for the shortcut (used in dev mode registration).
const APP_DISPLAY_NAME: &str = "Exalt Account Manager";

/// Ensures the shortcut is only created once per process lifetime.
static ENSURE_SHORTCUT: Once = Once::new();

/// Show a toast notification immediately.
#[cfg(target_os = "windows")]
pub fn show_toast_notification(notification: &ToastNotification) -> Result<(), Error> {
    let xml = build_toast_xml(notification);
    info!("Showing toast notification with XML:\n{}", xml);

    let xml_doc = parse_xml(&xml)?;

    let toast = WinToastNotification::CreateToastNotification(&xml_doc).map_err(|e| {
        Error::new(
            std::io::ErrorKind::Other,
            format!("Failed to create toast notification: {}", e),
        )
    })?;

    let notifier = get_notifier()?;

    notifier.Show(&toast).map_err(|e| {
        Error::new(
            std::io::ErrorKind::Other,
            format!("Failed to show toast notification: {}", e),
        )
    })?;

    info!("Toast notification displayed successfully.");
    Ok(())
}

/// Schedule a toast notification for a future date/time.
///
/// * `notification` — The notification content.
/// * `scheduled_at` — ISO 8601 datetime string in local time (e.g. "2026-02-28T12:00:00").
/// * `tag` — A unique identifier for this scheduled notification (used to cancel it).
#[cfg(target_os = "windows")]
pub fn schedule_toast_notification(
    notification: &ToastNotification,
    scheduled_at: &str,
    tag: &str,
) -> Result<ScheduledNotificationInfo, Error> {
    let xml = build_toast_xml(notification);
    info!(
        "Scheduling toast notification for {} with tag '{}'. XML:\n{}",
        scheduled_at, tag, xml
    );

    let xml_doc = parse_xml(&xml)?;

    // Parse the scheduled time
    let dt = chrono::NaiveDateTime::parse_from_str(scheduled_at, "%Y-%m-%dT%H:%M:%S")
        .or_else(|_| chrono::NaiveDateTime::parse_from_str(scheduled_at, "%Y-%m-%dT%H:%M:%S%.f"))
        .map_err(|e| {
            Error::new(
                std::io::ErrorKind::InvalidInput,
                format!("Failed to parse scheduled_at datetime '{}': {}", scheduled_at, e),
            )
        })?;

    let local_dt = chrono::Local::now()
        .timezone()
        .from_local_datetime(&dt)
        .single()
        .ok_or_else(|| {
            Error::new(
                std::io::ErrorKind::InvalidInput,
                format!("Ambiguous or invalid local datetime: {}", scheduled_at),
            )
        })?;

    // Convert to Windows DateTime (100-nanosecond intervals since 1601-01-01)
    let windows_ticks = chrono_to_windows_datetime(&local_dt)?;
    let delivery_time = windows::Foundation::DateTime {
        UniversalTime: windows_ticks,
    };

    let scheduled_toast =
        ScheduledToastNotification::CreateScheduledToastNotification(&xml_doc, delivery_time)
            .map_err(|e| {
                Error::new(
                    std::io::ErrorKind::Other,
                    format!("Failed to create scheduled toast notification: {}", e),
                )
            })?;

    // Set a tag so we can cancel it later
    let tag_hstring = HSTRING::from(tag);
    scheduled_toast.SetTag(&tag_hstring).map_err(|e| {
        Error::new(
            std::io::ErrorKind::Other,
            format!("Failed to set tag on scheduled notification: {}", e),
        )
    })?;

    let notifier = get_notifier()?;

    notifier
        .AddToSchedule(&scheduled_toast)
        .map_err(|e| {
            Error::new(
                std::io::ErrorKind::Other,
                format!("Failed to schedule toast notification: {}", e),
            )
        })?;

    info!(
        "Toast notification scheduled successfully for {} with tag '{}'.",
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
#[cfg(target_os = "windows")]
pub fn cancel_scheduled_toast_notification(tag: &str) -> Result<bool, Error> {
    info!("Cancelling scheduled toast notification with tag '{}'.", tag);

    let notifier = get_notifier()?;

    let scheduled = notifier.GetScheduledToastNotifications().map_err(|e| {
        Error::new(
            std::io::ErrorKind::Other,
            format!("Failed to get scheduled notifications: {}", e),
        )
    })?;

    let mut found = false;
    for i in 0..scheduled.Size().unwrap_or(0) {
        if let Ok(item) = scheduled.GetAt(i) {
            if let Ok(item_tag) = item.Tag() {
                if item_tag.to_string_lossy() == tag {
                    notifier.RemoveFromSchedule(&item).map_err(|e| {
                        Error::new(
                            std::io::ErrorKind::Other,
                            format!("Failed to remove scheduled notification: {}", e),
                        )
                    })?;
                    found = true;
                    info!("Cancelled scheduled notification with tag '{}'.", tag);
                }
            }
        }
    }

    if !found {
        info!(
            "No scheduled notification found with tag '{}'. It may have already fired.",
            tag
        );
    }

    Ok(found)
}

// ---------------------------------------------------------------------------
// Internal helpers
// ---------------------------------------------------------------------------

/// Get the toast notifier for our app.
/// On first call, ensures a Start Menu shortcut with the correct AUMID exists.
fn get_notifier() -> Result<windows::UI::Notifications::ToastNotifier, Error> {
    // Ensure the Start Menu shortcut with AUMID exists (once per process).
    ENSURE_SHORTCUT.call_once(|| {
        if let Err(e) = ensure_start_menu_shortcut() {
            warn!("Failed to ensure Start Menu shortcut for toast notifications: {}. \
                   Toasts may not display if the app was not installed via the installer.", e);
        }
    });

    let aumid = HSTRING::from(APP_USER_MODEL_ID);
    ToastNotificationManager::CreateToastNotifierWithId(&aumid).map_err(|e| {
        error!("Failed to create ToastNotifier with AUMID '{}': {}", APP_USER_MODEL_ID, e);
        Error::new(
            std::io::ErrorKind::Other,
            format!(
                "Failed to create ToastNotifier. Ensure the app has a Start Menu shortcut \
                 with AUMID '{}'. Error: {}",
                APP_USER_MODEL_ID, e
            ),
        )
    })
}

/// Ensure a Start Menu shortcut exists with the correct AUMID.
///
/// Windows requires a `.lnk` shortcut in the Start Menu with the
/// `System.AppUserModel.ID` property set to the app's AUMID for toast
/// notifications to display. The NSIS installer creates this for production
/// builds, but in development mode no shortcut exists — this function
/// creates one automatically.
///
/// If a shortcut already exists, this is a no-op.
fn ensure_start_menu_shortcut() -> Result<(), Error> {
    let shortcut_path = get_shortcut_path()?;

    if shortcut_path.exists() {
        info!("Start Menu shortcut already exists at: {}", shortcut_path.display());
        return Ok(());
    }

    info!("Creating Start Menu shortcut for toast notifications at: {}", shortcut_path.display());

    // Ensure parent directory exists
    if let Some(parent) = shortcut_path.parent() {
        std::fs::create_dir_all(parent).map_err(|e| {
            Error::new(e.kind(), format!("Failed to create Start Menu directory: {}", e))
        })?;
    }

    let exe_path = std::env::current_exe().map_err(|e| {
        Error::new(e.kind(), format!("Failed to get current executable path: {}", e))
    })?;

    create_shortcut_with_aumid(
        &shortcut_path,
        &exe_path,
        APP_USER_MODEL_ID,
    )?;

    info!("Start Menu shortcut created successfully.");
    Ok(())
}

/// Get the expected path for the Start Menu shortcut.
fn get_shortcut_path() -> Result<std::path::PathBuf, Error> {
    let appdata = std::env::var("APPDATA").map_err(|_| {
        Error::new(std::io::ErrorKind::NotFound, "APPDATA environment variable not set")
    })?;

    let mut path = std::path::PathBuf::from(appdata);
    path.push("Microsoft");
    path.push("Windows");
    path.push("Start Menu");
    path.push("Programs");
    path.push(format!("{}.lnk", APP_DISPLAY_NAME));

    Ok(path)
}

/// Create a Windows `.lnk` shortcut file with the `System.AppUserModel.ID`
/// property set on it, using the Shell COM APIs.
fn create_shortcut_with_aumid(
    shortcut_path: &std::path::Path,
    target_exe: &std::path::Path,
    aumid: &str,
) -> Result<(), Error> {
    unsafe {
        // Initialize COM (harmless if already initialized)
        let _ = CoInitializeEx(None, COINIT_MULTITHREADED);

        // Create IShellLink instance
        let shell_link: IShellLinkW =
            CoCreateInstance(&ShellLink, None, CLSCTX_INPROC_SERVER).map_err(|e| {
                Error::new(
                    std::io::ErrorKind::Other,
                    format!("Failed to create ShellLink COM object: {}", e),
                )
            })?;

        // Set the target executable
        let target_hstring = HSTRING::from(target_exe.to_string_lossy().as_ref());
        shell_link.SetPath(&target_hstring).map_err(|e| {
            Error::new(
                std::io::ErrorKind::Other,
                format!("Failed to set shortcut target path: {}", e),
            )
        })?;

        // Set a description
        let desc = HSTRING::from(APP_DISPLAY_NAME);
        shell_link.SetDescription(&desc).map_err(|e| {
            Error::new(
                std::io::ErrorKind::Other,
                format!("Failed to set shortcut description: {}", e),
            )
        })?;

        // Set the AppUserModelID via IPropertyStore
        let property_store: IPropertyStore = shell_link.cast().map_err(|e| {
            Error::new(
                std::io::ErrorKind::Other,
                format!("Failed to cast ShellLink to IPropertyStore: {}", e),
            )
        })?;

        let aumid_value = PROPVARIANT::from(aumid);
        property_store
            .SetValue(&PKEY_AppUserModel_ID, &aumid_value)
            .map_err(|e| {
                Error::new(
                    std::io::ErrorKind::Other,
                    format!("Failed to set AppUserModel.ID property: {}", e),
                )
            })?;

        property_store.Commit().map_err(|e| {
            Error::new(
                std::io::ErrorKind::Other,
                format!("Failed to commit property store: {}", e),
            )
        })?;

        // Save via IPersistFile
        let persist_file: windows::Win32::System::Com::IPersistFile =
            shell_link.cast().map_err(|e| {
                Error::new(
                    std::io::ErrorKind::Other,
                    format!("Failed to cast ShellLink to IPersistFile: {}", e),
                )
            })?;

        let shortcut_hstring = HSTRING::from(shortcut_path.to_string_lossy().as_ref());
        persist_file
            .Save(
                windows::core::PCWSTR(shortcut_hstring.as_ptr()),
                true,
            )
            .map_err(|e| {
                Error::new(
                    std::io::ErrorKind::Other,
                    format!("Failed to save shortcut file: {}", e),
                )
            })?;
    }

    Ok(())
}

/// Parse an XML string into an `XmlDocument`.
fn parse_xml(xml: &str) -> Result<XmlDocument, Error> {
    let xml_doc = XmlDocument::new().map_err(|e| {
        Error::new(
            std::io::ErrorKind::Other,
            format!("Failed to create XmlDocument: {}", e),
        )
    })?;

    let hxml = HSTRING::from(xml);
    xml_doc.LoadXml(&hxml).map_err(|e| {
        Error::new(
            std::io::ErrorKind::Other,
            format!("Failed to load toast XML: {}", e),
        )
    })?;

    Ok(xml_doc)
}

/// Build the Windows toast XML from a `ToastNotification`.
///
/// Uses the generic toast template (ToastGeneric) with optional hero image and action buttons.
/// Reference: <https://learn.microsoft.com/en-us/windows/apps/design/shell/tiles-and-notifications/adaptive-interactive-toasts>
fn build_toast_xml(notification: &ToastNotification) -> String {
    let mut xml = String::new();

    // Toast root — launch URL can open the app via protocol
    xml.push_str("<toast launch=\"eam://notification-clicked\">\n");

    // Visual binding
    xml.push_str("  <visual>\n");
    xml.push_str("    <binding template=\"ToastGeneric\">\n");

    // Title
    xml.push_str(&format!(
        "      <text>{}</text>\n",
        escape_xml(&notification.title)
    ));

    // Body
    xml.push_str(&format!(
        "      <text>{}</text>\n",
        escape_xml(&notification.body)
    ));

    // App logo (inline image)
    if let Some(ref icon_path) = notification.icon_path {
        xml.push_str(&format!(
            "      <image placement=\"appLogoOverride\" hint-crop=\"circle\" src=\"file:///{}\" />\n",
            escape_xml(&icon_path.replace('\\', "/"))
        ));
    }

    // Hero image (displayed at top of toast, stretches full width)
    if let Some(ref hero_path) = notification.hero_image_path {
        xml.push_str(&format!(
            "      <image placement=\"hero\" src=\"file:///{}\" />\n",
            escape_xml(&hero_path.replace('\\', "/"))
        ));
    }

    xml.push_str("    </binding>\n");
    xml.push_str("  </visual>\n");

    // Actions (buttons)
    if !notification.actions.is_empty() {
        xml.push_str("  <actions>\n");
        for action in &notification.actions {
            xml.push_str(&format!(
                "    <action content=\"{}\" activationType=\"protocol\" arguments=\"{}\" />\n",
                escape_xml(&action.label),
                escape_xml(&action.action_url)
            ));
        }
        xml.push_str("  </actions>\n");
    }

    xml.push_str("</toast>");

    xml
}

/// Convert a chrono `DateTime<Local>` to Windows FILETIME ticks (100-ns intervals since 1601-01-01 UTC).
fn chrono_to_windows_datetime(
    dt: &chrono::DateTime<chrono::Local>,
) -> Result<i64, Error> {

    // Windows epoch: 1601-01-01 00:00:00 UTC
    // Unix epoch:    1970-01-01 00:00:00 UTC
    // Difference in seconds: 11_644_473_600
    const UNIX_TO_WINDOWS_EPOCH_SECONDS: i64 = 11_644_473_600;
    const TICKS_PER_SECOND: i64 = 10_000_000; // 100-nanosecond intervals

    let utc_dt = dt.with_timezone(&chrono::Utc);
    let unix_timestamp = utc_dt.timestamp();

    let windows_ticks = (unix_timestamp + UNIX_TO_WINDOWS_EPOCH_SECONDS) * TICKS_PER_SECOND
        + (utc_dt.timestamp_subsec_nanos() as i64 / 100);

    Ok(windows_ticks)
}

/// Basic XML escaping.
fn escape_xml(s: &str) -> String {
    s.replace('&', "&amp;")
        .replace('<', "&lt;")
        .replace('>', "&gt;")
        .replace('"', "&quot;")
        .replace('\'', "&apos;")
}

