import { invoke } from '@tauri-apps/api/core';
import { getCurrentTime } from './timeUtils';
import { isDebugLoggingEnabled, logToErrorLog } from './loggingUtils';

/**
 * Checks whether the game needs updating.
 *
 * @returns {Promise<boolean>} whether an update is needed.
 */
async function checkForUpdates(force) {
    const debugFlag = isDebugLoggingEnabled();
    if (sessionStorage.getItem('updateCheckInProgress') === 'true' ||
        sessionStorage.getItem('updateInProgress') === 'true') {
        // Report what is already known rather than undefined. A caller storing
        // the result would otherwise show "up to date" while a check is running.
        return localStorage.getItem('updateNeeded') === 'true';
    }

    sessionStorage.setItem('updateCheckInProgress', 'true');
    if (debugFlag) {
        console.log('Checking for updates...');
    }

    try {
        const updateNeeded = await invoke('check_for_game_update', { force: Boolean(force) });

        if (debugFlag) {
            console.log('Update needed:', updateNeeded);
        }

        localStorage.setItem('updateNeeded', updateNeeded ? 'true' : 'false');
        localStorage.setItem('lastUpdateCheck', getCurrentTime());
        return Boolean(updateNeeded);
    } catch (error) {
        logToErrorLog('checkForUpdates', error);
        return false;
    } finally {
        if (debugFlag) {
            console.log('Update check completed');
        }

        sessionStorage.setItem('updateCheckInProgress', 'false');
    }
}

/**
 * Updates the game, resolving once it has finished.
 *
 * @returns {Promise<boolean>} whether the update completed successfully.
 */
async function updateGame() {
    if (sessionStorage.getItem('updateCheckInProgress') === 'true' ||
        sessionStorage.getItem('updateInProgress') === 'true') {
        return false;
    }

    sessionStorage.setItem('updateInProgress', 'true');

    try {
        // Awaited deliberately. This previously started the update without
        // waiting for it, so the finally below cleared the in-progress flag
        // microseconds later: callers returned before the game was updated, and
        // nothing in the UI ever observed an update running.
        const updateSucceeded = await invoke('perform_game_update');
        if (updateSucceeded) {
            localStorage.removeItem('updateNeeded');
        }
        return Boolean(updateSucceeded);
    } catch (error) {
        logToErrorLog('updateGame', error);
        return false;
    } finally {
        sessionStorage.setItem('updateInProgress', 'false');
    }
}

export {
    checkForUpdates,
    updateGame
};
