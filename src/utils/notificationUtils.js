import { invoke } from '@tauri-apps/api/core';

/**
 * @typedef {Object} ToastAction
 * @property {string} label - The button label text.
 * @property {string} action_url - The URL to open when clicked (e.g., "eam://daily-login").
 */

/**
 * @typedef {Object} ScheduledNotificationInfo
 * @property {string} tag - Unique tag for cancellation.
 * @property {string} scheduled_at - ISO 8601 datetime of when it fires.
 * @property {string} title - Notification title.
 * @property {string} body - Notification body.
 * @property {string|null} hero_image_path - Hero image path.
 */

/**
 * @typedef {Object} EndOfMonthNotificationContent
 * @property {string} title - Notification title.
 * @property {string} body - Notification body.
 * @property {string} image_filename - Image filename (relative to mascot/Info/).
 * @property {number} month - 1-based month number.
 * @property {number} year - Year.
 * @property {string} scheduled_at - ISO 8601 datetime string.
 * @property {string} tag - Unique tag.
 */

/**
 * Send an instant OS toast notification.
 *
 * @param {string} title - Notification title.
 * @param {string} body - Notification body text.
 * @param {Object} [options] - Optional parameters.
 * @param {string} [options.heroImagePath] - Absolute path to a hero image.
 * @param {string} [options.iconPath] - Absolute path to an icon/logo.
 * @param {ToastAction[]} [options.actions] - Action buttons.
 * @returns {Promise<void>}
 */
export async function sendToastNotification(title, body, options = {}) {
    return invoke('send_toast_notification', {
        title,
        body,
        heroImagePath: options.heroImagePath ?? null,
        iconPath: options.iconPath ?? null,
        actions: options.actions ?? null,
    });
}

/**
 * Schedule a toast notification for a future date/time.
 *
 * @param {string} title - Notification title.
 * @param {string} body - Notification body text.
 * @param {string} scheduledAt - ISO 8601 datetime string (e.g. "2026-02-28T12:00:00").
 * @param {string} tag - Unique identifier (used to cancel).
 * @param {Object} [options] - Optional parameters.
 * @param {string} [options.heroImagePath] - Absolute path to a hero image.
 * @param {string} [options.iconPath] - Absolute path to an icon/logo.
 * @param {ToastAction[]} [options.actions] - Action buttons.
 * @returns {Promise<ScheduledNotificationInfo>}
 */
export async function scheduleToastNotification(title, body, scheduledAt, tag, options = {}) {
    return invoke('schedule_toast_notification_command', {
        title,
        body,
        scheduledAt,
        tag,
        heroImagePath: options.heroImagePath ?? null,
        iconPath: options.iconPath ?? null,
        actions: options.actions ?? null,
    });
}

/**
 * Cancel a previously scheduled toast notification.
 *
 * @param {string} tag - The unique tag of the notification to cancel.
 * @returns {Promise<boolean>} - True if a notification was found and cancelled.
 */
export async function cancelScheduledToastNotification(tag) {
    return invoke('cancel_scheduled_toast_notification_command', { tag });
}

/**
 * Schedule the end-of-month daily login reminder notification.
 * Automatically determines content based on the current month.
 *
 * @param {string} resourceBasePath - Absolute path to app resources directory.
 * @returns {Promise<ScheduledNotificationInfo>}
 */
export async function scheduleEndOfMonthNotification(resourceBasePath) {
    return invoke('schedule_end_of_month_notification', { resourceBasePath });
}

/**
 * Get the end-of-month notification info without scheduling it.
 * Useful for displaying the same content as an in-app snackbar.
 *
 * @returns {Promise<EndOfMonthNotificationContent>}
 */
export async function getEndOfMonthNotificationInfo() {
    return invoke('get_end_of_month_notification_info');
}
