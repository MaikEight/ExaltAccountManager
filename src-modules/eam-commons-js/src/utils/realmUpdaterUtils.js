import { invoke, tauri } from '@tauri-apps/api';
import { getCurrentTime } from './timeUtils';
import { logToErrorLog } from './loggingUtils';

async function checkForUpdates(force) {
    if (sessionStorage.getItem('updateCheckInProgress') === 'true' ||
        sessionStorage.getItem('updateInProgress') === 'true') {
        return
    };

    sessionStorage.setItem('updateCheckInProgress', 'true');
    console.log('Checking for updates...');
    const updateNeeded = await invoke('check_for_game_update', { force: !!force })
        .catch((error) => {
            logToErrorLog('checkForUpdates', error);
            return false;
        });
    console.log('Update needed:', updateNeeded);

    localStorage.setItem('updateNeeded', updateNeeded ? 'true' : 'false');
    localStorage.setItem('lastUpdateCheck', getCurrentTime());
    sessionStorage.setItem('updateCheckInProgress', 'false');

    return updateNeeded;
}

async function updateGame() {
    if (sessionStorage.getItem('updateCheckInProgress') === 'true' ||
        sessionStorage.getItem('updateInProgress') === 'true') {
        return
    };

    sessionStorage.setItem('updateInProgress', 'true');

    tauri.invoke('perform_game_update').then(() => {
        localStorage.removeItem('updateNeeded');
    }).catch((error) => {
        logToErrorLog('updateGame', error);
    }).finally(() => {
        sessionStorage.setItem('updateInProgress', 'false');
    });
}

export {
    checkForUpdates,
    updateGame
};