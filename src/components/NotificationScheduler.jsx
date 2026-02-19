import { useCallback, useEffect, useRef } from "react";
import useUserSettings from "../hooks/useUserSettings";
import useSnack from "../hooks/useSnack";
import { getEndOfMonthNotificationInfo, scheduleEndOfMonthNotification } from "../utils/notificationUtils";
import { resolveResource } from "@tauri-apps/api/path";

/**
 * NotificationScheduler — invisible component that:
 * 1. On mount, schedules the end-of-month OS toast notification (if enabled in settings).
 * 2. Runs a periodic check (~1 minute) to display the same message as an in-app snackbar
 *    when the scheduled time arrives while EAM is open.
 *
 * Should be rendered once, high in the component tree (e.g. next to DeepLinkingComponent).
 */
function NotificationScheduler() {
    const userSettings = useUserSettings();
    const { showSnackbar } = useSnack();
    const hasScheduled = useRef(false);
    const hasShownSnackbar = useRef(false);
    const intervalRef = useRef(null);

    const settings = userSettings?.get;
    const isEnabled = settings?.notifications?.endOfMonthReminderEnabled ?? true;

    // Schedule the OS toast notification on mount
    const scheduleOsNotification = useCallback(async () => {
        if (hasScheduled.current || !isEnabled) return;
        hasScheduled.current = true;

        try {
            // Resolve the resource base path (where bundled mascot images live)
            // In Tauri v2, resolveResource resolves relative to the resource dir
            const resourceBasePath = await resolveResource("");

            const result = await scheduleEndOfMonthNotification(resourceBasePath);
            console.log("[NotificationScheduler] End-of-month notification scheduled:", result);
        } catch (err) {
            // This is expected if the date is already past
            console.log("[NotificationScheduler] Could not schedule end-of-month notification:", err);
        }
    }, [isEnabled]);

    // Check if it's time to show the in-app snackbar
    const checkForSnackbar = useCallback(async () => {
        if (hasShownSnackbar.current || !isEnabled) return;

        try {
            const content = await getEndOfMonthNotificationInfo();
            const scheduledDate = new Date(content.scheduled_at);
            const now = new Date();

            // Show snackbar if we're past the scheduled time (within the same day)
            if (now >= scheduledDate) {
                // Check if we already showed this month's snackbar (stored in sessionStorage)
                const shownKey = `eam-eom-snackbar-${content.tag}`;
                if (sessionStorage.getItem(shownKey)) {
                    hasShownSnackbar.current = true;
                    return;
                }

                // Show the snackbar
                showSnackbar(`${content.title} — ${content.body}`, 'default', false);
                sessionStorage.setItem(shownKey, 'true');
                hasShownSnackbar.current = true;
            }
        } catch (err) {
            console.warn("[NotificationScheduler] Error checking for snackbar:", err);
        }
    }, [isEnabled, showSnackbar]);

    useEffect(() => {
        scheduleOsNotification();

        // Check every 60 seconds
        checkForSnackbar();
        intervalRef.current = setInterval(checkForSnackbar, 60_000);

        return () => {
            if (intervalRef.current) {
                clearInterval(intervalRef.current);
            }
        };
    }, [scheduleOsNotification, checkForSnackbar]);

    // If the setting is toggled off while running, clear the interval
    useEffect(() => {
        if (!isEnabled && intervalRef.current) {
            clearInterval(intervalRef.current);
            intervalRef.current = null;
        }
    }, [isEnabled]);

    return null; // Invisible component
}

export default NotificationScheduler;
