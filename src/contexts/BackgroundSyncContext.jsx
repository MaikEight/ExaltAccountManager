import { createContext, useEffect, useRef, useState } from "react";
import { invoke } from '@tauri-apps/api/core';
import { listen } from '@tauri-apps/api/event';
import { getRequestState, storeCharList, xmlToJson } from "eam-commons-js";
import useAccounts from "../hooks/useAccounts";
import BackgroundSyncComponent from "../components/BackgroundSyncComponent";
import { Box, Typography } from "@mui/material";
import { DAILY_LOGIN_COMPLETED_MESSAGES, MASCOT_NAME } from "../constants";
import useSnack from "../hooks/useSnack";
import { validatePlusToken, checkForUpdates, updateGame } from 'eam-commons-js';
import useUserSettings from "../hooks/useUserSettings";
import { getCurrentWindow } from "@tauri-apps/api/window";

const getRandomMessage = () => {
    const randomIndex = Math.floor(Math.random() * DAILY_LOGIN_COMPLETED_MESSAGES.length);
    return DAILY_LOGIN_COMPLETED_MESSAGES[randomIndex];
};

const BackgroundSyncContext = createContext();

const SyncMode = {
    Stopped: 'Stopped',
    Default: 'Default',
    DailyLogin: 'DailyLogin',
}

function BackgroundSyncProvider({ children }) {
    const processedEventIds = useRef({});
    const emailToAccountNameMap = useRef({});

    const { loadAccountByEmail, updateAccount, accounts } = useAccounts();
    const { showSnackbar } = useSnack();
    const { getByKeyAndSubKey } = useUserSettings();

    const [syncMode, setSyncMode] = useState(SyncMode.Stopped);
    const [uiState, setUiState] = useState({
        email: '',
        state: '',
        updatedAt: null,
    });
    const [dailyLoginProgressData, setDailyLoginProgressData] = useState(null);

    const checkAndAddEventId = (eventName, id, debugFlag = false) => {
        const MAX_EVENT_IDS = 1280;
        const BATCH_SIZE = 200;

        if (!processedEventIds.current[eventName]) {
            processedEventIds.current[eventName] = [];
        }

        if (!processedEventIds.current[eventName].includes(id)) {
            processedEventIds.current[eventName].push(id);

            // prune oldest IDs to prevent unbounded memory growth
            const ids = processedEventIds.current[eventName];
            if (ids.length > MAX_EVENT_IDS) {
                ids.splice(0, BATCH_SIZE);
                processedEventIds.current[eventName] = ids;
            }

            if (debugFlag) {
                console.log('Processing background sync event:', eventName, 'with ID:', id);
            }

            return true;
        }

        return false;
    }

    const processBackgroundSyncEvent = async (event) => {
        const debugFlag = sessionStorage.getItem('flag:debug') === 'true';

        if (!event || !event.payload) {
            console.warn('Received invalid background sync event:', event);
            return;
        }

        if (event.payload.AccountCharListSync) {
            const e = event.payload.AccountCharListSync;
            if (!checkAndAddEventId('AccountCharListSync', e.id, debugFlag)) {
                return;
            }

            try {
                const charList = xmlToJson(e.dataset);

                const state = getRequestState(charList) || "BGSyncError";
                const acc = await loadAccountByEmail(e.email);

                if (state === 'AccountSuspended' || state === 'WrongPassword') {
                    if (debugFlag) {
                        console.warn('Account suspended or wrong password for email:', e.email, 'with state:', state, acc);
                    }

                    setUiState({
                        email: e.email,
                        state: state,
                        updatedAt: new Date(),
                    });

                    if (Boolean(acc)) {
                        if (debugFlag) {
                            console.log('Updating account state for email:', e.email, 'with state:', state);
                        }

                        acc.state = state;
                        await updateAccount(acc);
                    }
                    return;
                }

                if (Boolean(state) && Boolean(acc)) {
                    acc.state = state;
                    acc.lastRefresh = new Date();
                    await updateAccount(acc);
                } else if (debugFlag) {
                    console.log('AccountCharListSync Debug:', {
                        eventEmail: e.email,
                        emailType: typeof e.email,
                        emailLength: e.email?.length,
                        state: state,
                        foundAccount: Boolean(acc),
                        accountEmail: acc?.email,
                        totalAccountsAvailable: accounts?.length || 0,
                        availableEmails: accounts?.map(a => a.email) || [],
                        emailToAccountMapSize: Object.keys(emailToAccountNameMap.current).length
                    });
                    console.warn('Account not found or state is undefined for email:', e.email, 'with state:', state, acc);
                }

                await storeCharList(charList, e.email);
                if (debugFlag) {
                    console.log('Stored char list for email:', e.email, charList);
                }
            } catch (error) {
                console.error('Error processing AccountCharListSync event:', error);
                if (debugFlag) {
                    console.error('Event data:', event);
                }
            }
            return;
        }

        if (event.payload.AccountStarted) {
            const e = event.payload.AccountStarted;

            if (!e || !e.id || !e.email) {
                console.warn('Received AccountStarted event without payload:', event);
                return;
            };


            if (!checkAndAddEventId('AccountStarted', e.id, debugFlag)) {
                return;
            }

            const acc = await loadAccountByEmail(e.email);

            if (acc) {
                emailToAccountNameMap.current[e.email] = acc.name;
                setUiState({
                    email: acc.email,
                    state: 'Started',
                    updatedAt: new Date(),
                });
                return;
            }

            if (debugFlag) {
                console.log('AccountStarted Debug:', {
                    eventEmail: e.email,
                    emailType: typeof e.email,
                    emailLength: e.email?.length,
                    foundAccount: Boolean(acc),
                    accountEmail: acc?.email,
                    totalAccountsAvailable: accounts?.length || 0,
                    availableEmails: accounts?.map(a => a.email) || [],
                    emailToAccountMapSize: Object.keys(emailToAccountNameMap.current).length
                });
                console.warn('Account not found for email:', e.email);
            }
            return;
        }

        if (event.payload.AccountProgress) {
            const e = event.payload.AccountProgress;

            if (!e || !e.id || !e.email) {
                console.warn('Received AccountProgress event with missing data:', event);
                return;
            }

            if (!checkAndAddEventId('AccountProgress', e.id, debugFlag)) {
                return;
            }

            let state = "";
            switch (e.state) {
                case 'FetchingAccount':
                    state = 'Processing..';
                    break;
                case 'FetchingCharList':
                    state = 'Processing...';
                    break;
                case 'SyncingCharList':
                    state = 'Processing....';
                    break;
                case 'WaitingForCooldown':
                    state = 'Waiting';
                    break;
                case 'Done':
                    state = 'Success';
                    break;
                case 'Failed':
                    state = 'Failed';
                    break;
                default:
                    state = "BGSyncError";
            }

            setUiState({
                email: e.email,
                state: state,
                updatedAt: new Date(),
            });
            return;
        }

        if (event.payload.AccountFinished) {
            const e = event.payload.AccountFinished;

            if (!e || !e.id || !e.email) {
                console.warn('Received AccountFinished event with missing data:', event);
                return;
            }

            if (!checkAndAddEventId('AccountFinished', e.id, debugFlag)) {
                return;
            }

            if (debugFlag) {
                console.log('AccountFinished Debug:', {
                    eventEmail: e.email,
                    emailType: typeof e.email,
                    emailLength: e.email?.length,
                    state: e.result,
                    totalAccountsAvailable: accounts?.length || 0,
                    availableEmails: accounts?.map(a => a.email) || [],
                    emailToAccountMapSize: Object.keys(emailToAccountNameMap.current).length
                });
            }

            setUiState({
                email: e.email,
                state: e.result,
                updatedAt: new Date(),
            });
            return;
        }

        if (event.payload.AccountFailed) {
            const e = event.payload.AccountFailed;

            if (!checkAndAddEventId('AccountFailed', e.id, debugFlag)) {
                return;
            }

            if (debugFlag) {
                console.warn('Processing AccountFailed event:', e);
            }

            try {
                const errorXml = e.error;
                const errorJson = xmlToJson(errorXml);
                let acc = await loadAccountByEmail(e.email);
                let state = getRequestState(errorJson);
                if (!state || state === 'Success') {
                    state = "BGSyncError";
                }

                if (debugFlag) {
                    console.warn('AccountFailed Debug:', {
                        eventEmail: e.email,
                        emailType: typeof e.email,
                        emailLength: e.email?.length,
                        state: state,
                        foundAccount: Boolean(acc),
                        account: acc,
                        totalAccountsAvailable: accounts?.length || 0,
                        availableEmails: accounts?.map(a => a.email) || [],
                        emailToAccountMapSize: Object.keys(emailToAccountNameMap.current).length
                    });
                    console.warn('Account failed for email:', e.email, 'with state:', state, acc);
                }

                if (acc) {
                    emailToAccountNameMap.current[e.email] = acc.name;
                    acc.state = state;
                    await updateAccount(acc);
                }

                setUiState({
                    email: e.email,
                    state: "Failed",
                    updatedAt: new Date(),
                });
            } catch (error) {
                console.error('Error processing AccountFailed event:', error);
                if (debugFlag) {
                    console.error('Event data:', event);
                }
            }
            return;
        }

        //Daily Login Requests

        if (event.payload.DailyLoginProgress) {
            const e = event.payload.DailyLoginProgress;

            if (!e || !e.id || !e.left_emails || !e.failed_emails || !e.done || !e.left || !e.estimated_time) {
                if (debugFlag) {
                    console.warn('Received DailyLoginProgress event with missing data:', event);
                }
                return;
            }

            if (!checkAndAddEventId('DailyLoginProgress', e.id, debugFlag)) {
                return;
            }

            const getAccNameByEmail = async (email) => {
                if (!email) {
                    return "";
                }

                try {
                    if (emailToAccountNameMap.current[email]) {
                        return emailToAccountNameMap.current[email];
                    }
                    const acc = await loadAccountByEmail(email);
                    if (acc && acc.name) {
                        emailToAccountNameMap.current[email] = acc.name;
                    }
                    return acc ? acc.name : email;
                } catch (error) {
                    console.error('Error getting account name by email:', email, error);
                    return email; // Fallback to email if there's an error
                }
            }

            const accNamesPromises = e.left_emails?.map(email => getAccNameByEmail(email)) || [];
            const failedAccNamesPromises = e.failed_emails?.map(email => getAccNameByEmail(email)) || [];

            const accNames = await Promise.all(accNamesPromises);
            const failedAccNames = await Promise.all(failedAccNamesPromises);

            setDailyLoginProgressData({
                done: e.done,
                left: e.left,
                leftEmails: e.left_emails,
                leftAccountNames: accNames,
                failedEmails: e.failed_emails,
                failedAccountNames: failedAccNames,
                estimatedTime: e.estimated_time ? new Date(e.estimated_time) : null,
            });
            return;
        }

        if (event.payload.DailyLoginDone) {
            const e = event.payload.DailyLoginDone;

            if (!e || !e.id) {
                console.warn('Received DailyLoginDone event with missing data:', event);
                return;
            }

            if (!checkAndAddEventId('DailyLoginDone', e.id, debugFlag)) {
                return;
            }

            if (debugFlag) {
                console.log('🥳 Daily Login Done:', e);
            }

            setDailyLoginProgressData(null);
            setUiState({
                email: '',
                state: '',
                updatedAt: null,
            });

            showSnackbar(
                (
                    <Box
                        sx={{
                            display: 'flex',
                            flexDirection: 'column',
                            alignItems: 'center',
                            justifyContent: 'center',
                            gap: 1,
                        }}
                    >
                        <img
                            src="/mascot/Happy/cheer_very_low_res.png"
                            alt={`${MASCOT_NAME} is happy`}
                            style={{ width: '120px' }}
                        />
                        <Typography variant="body2">
                            {getRandomMessage()}
                        </Typography>
                    </Box>
                ),
                'message',
                true //persistent
            );

            const closeEamAfterDailyLogin = getByKeyAndSubKey('dailyLogin', 'closeAfterFinish');
            if (closeEamAfterDailyLogin === true) {
                setTimeout(async () => {
                    await getCurrentWindow().close();
                }, 5_000);
            }

            return;
        }

        if (event.payload.ModeChanged) {
            const e = event.payload.ModeChanged;

            if (!e || !e.id || !e.mode) {
                console.warn('Received ModeChanged event with missing data:', event);
                return;
            }

            if (!checkAndAddEventId('ModeChanged', e.id, debugFlag)) {
                return;
            }

            if (debugFlag) {
                console.log('Background Sync Mode Changed:', e);
            }

            switch (e.mode) {
                case 'Stopped':
                    setSyncMode(SyncMode.Stopped);
                    break;
                case 'Default':
                    setSyncMode(SyncMode.Default);
                    break;
                case 'DailyLogin':
                    setSyncMode(SyncMode.DailyLogin);
                    break;
                default:
                    console.warn('Unknown sync mode received:', e.mode);
                    break;
            }

            return;
        }

        if (debugFlag) {
            console.warn('Unknown background sync event received:', event);
        }
        return;
    }

    const checkIfUserIsPlusUser = async () => {
        let jwtSignature = null;
        const sessionJwtToken = sessionStorage.getItem('jwtSignature');
        if (sessionJwtToken) {
            jwtSignature = sessionJwtToken;
        }
        if (!jwtSignature) {
            const jwtDataValue = await invoke('get_user_data_by_key', { key: 'jwtSignature' }).catch((error) => { return null; });
            if (jwtDataValue && jwtDataValue.dataValue) {
                jwtSignature = jwtDataValue.dataValue;
            }
        }

        if (!jwtSignature) {
            return false;
        }

        const deviceId = await invoke('get_device_unique_identifier').catch((error) => {
            console.error('Error getting device unique identifier:', error);
            const apiHwidHash = localStorage.getItem('apiHwidHash');
            if (apiHwidHash) {
                console.warn('Fallback: Using stored API HWID hash for Plus token validation:', apiHwidHash);
                return apiHwidHash;
            }
            console.error('No device ID available for Plus token validation');
            return '';
        });
        const response = await validatePlusToken(jwtSignature, deviceId).catch((error) => {
            console.error('Error validating Plus token:', error);
            return null;
        });

        return Boolean(response && response.isValidToken && response.isSubscribed);
    }

    useEffect(() => {
        const debugFlag = sessionStorage.getItem('flag:debug') === 'true';

        let eventListener;
        let dailyLoginEventListener;

        const registerEventListener = async () => {
            try {
                eventListener = await listen('background-sync-event', async (event) => {
                    try {
                        await processBackgroundSyncEvent(event);
                    } catch (error) {
                        console.error('Error processing background sync event:', error);
                    }
                });

                dailyLoginEventListener = await listen('start-daily-login-process', async (event) => {
                    try {
                        if (debugFlag) {
                            console.log('Received start-daily-login-process event:', event);
                        }
                        const e = event.payload;

                        if (!checkAndAddEventId('start-daily-login-process', e.id)) {
                            return;
                        }

                        if (syncMode === SyncMode.DailyLogin) { // Already in daily login mode, ignore the event
                            return;
                        }

                        // Start the daily login process
                        const isRunning = await invoke('is_background_sync_manager_running').catch((error) => {
                            console.error('Error checking if background sync manager is running:', error);
                            return true; // Assume true if there's an error
                        });

                        const currentMode = await invoke('get_current_background_sync_mode').catch((error) => {
                            console.error('Error getting current background sync mode:', error);
                            return SyncMode.Default; // Default to Default if there's an error just to be safe
                        });

                        if (isRunning) {
                            if (currentMode === SyncMode.DailyLogin) {
                                // The manager is running as expected.
                                return;
                            }
                            await invoke('stop_background_sync_manager').catch(console.error);
                        }

                        await invoke('change_background_sync_mode', { mode: SyncMode.DailyLogin }).catch(console.error);
                        await new Promise(resolve => setTimeout(resolve, 3000));
                        await invoke('start_background_sync_manager').catch(console.error);
                    } catch (error) {
                        console.error('Error processing start-daily-login-process event:', error);
                    }
                });
            } catch (error) {
                console.error('Error registering background sync event listener:', error);
            }
        }

        registerEventListener();

        return () => {
            try {
                if (eventListener) {
                    eventListener();
                }
            } catch (error) {
                console.error('Error cleaning up event listeners:', error);
            }

            try {
                if (dailyLoginEventListener) {
                    dailyLoginEventListener();
                }
            } catch (error) {
                console.error('Error cleaning up daily login event listener:', error);
            }
        }
    }, [accounts, loadAccountByEmail, updateAccount, syncMode]);

    useEffect(() => {
        if (syncMode === SyncMode.Default || syncMode === SyncMode.Stopped) {
            const timeoutId = setTimeout(() => {
                setUiState({
                    email: '',
                    state: '',
                    updatedAt: null,
                });
            }, uiState.state === 'Waiting' ? 315_000 : 7_500); // Clear state after 7.5 seconds or 5 minutes if waiting

            return () => {
                if (timeoutId) {
                    clearTimeout(timeoutId);
                }
            };
        }
    }, [uiState]);

    useEffect(() => {
        const createAndStartBackgroundSyncManager = async () => {
            try {
                const debugFlag = sessionStorage.getItem('flag:debug') === 'true';
                await invoke('create_background_sync_manager').catch(console.error);

                const needsDailyLogin = await invoke('needs_to_do_daily_login').catch((error) => {
                    console.error('Error checking daily login needs:', error)
                    return false;
                });

                const isRunning = await invoke('is_background_sync_manager_running').catch((error) => {
                    console.error('Error checking if background sync manager is running:', error);
                    return true; // Assume true if there's an error
                });

                if (needsDailyLogin) {
                    const currentMode = await invoke('get_current_background_sync_mode').catch((error) => {
                        console.error('Error getting current background sync mode:', error);
                        return SyncMode.Default; // Default to Default if there's an error just to be safe
                    });

                    // Check the EAM Plus status
                    const isPlusUser = await checkIfUserIsPlusUser().catch((error) => {
                        console.error('Error checking if user is Plus user:', error);
                        return false; // Assume it's not a Plus user if there's an error
                    });

                    if (!isPlusUser) {
                        //Non-Plus users need to update the game for the daily login to work
                        if (debugFlag) {
                            console.info('Non-Plus user detected, stopping background sync manager for daily login.');
                        }

                        if (isRunning) {
                            await invoke('stop_background_sync_manager').catch(console.error);
                        }

                        let updateNeeded = await checkForUpdates(false)
                        if (updateNeeded === null) {
                            const storedState = localStorage.getItem('updateNeeded');
                            updateNeeded = storedState === 'true';
                        }

                        if (updateNeeded) {
                            if (debugFlag) {
                                console.info('Update needed for daily login, performing game update...');
                            }
                            await updateGame();
                            console.log('Game update completed, starting background sync manager for daily login.');
                        }
                    }

                    if (isRunning) {
                        if (currentMode === SyncMode.DailyLogin) {
                            // The manager is running as expected.
                            return;
                        }
                        await invoke('stop_background_sync_manager').catch(console.error);
                    }

                    await invoke('change_background_sync_mode', { mode: SyncMode.DailyLogin }).catch(console.error);
                    await new Promise(resolve => setTimeout(resolve, 3000));
                    await invoke('start_background_sync_manager').catch(console.error);
                    return;
                }

                await invoke('change_background_sync_mode', { mode: SyncMode.Default }).catch(console.error);
                await new Promise(resolve => setTimeout(resolve, 3000));
                await invoke('start_background_sync_manager').catch(console.error);
            } catch (error) {
                console.error('Failed to create background sync manager:', error);
            }
        }

        const startPeriodicDailyLoginCheck = async () => {
            try {
                await invoke('start_periodic_daily_login_check').catch((error) => {
                    console.error('Error starting periodic daily login check:', error)
                });
            } catch (error) {
                console.error('Failed to start periodic daily login check:', error);
            }
        }

        createAndStartBackgroundSyncManager();
        startPeriodicDailyLoginCheck();
    }, []);

    const contextValue = {
        syncMode,
        setSyncMode,
        uiState,
        setUiState,
        dailyLoginProgressData,
        emailToAccountNameMap: emailToAccountNameMap?.current || {}
    };

    return (
        <BackgroundSyncContext.Provider value={contextValue}>
            {children}
            <BackgroundSyncComponent />
        </BackgroundSyncContext.Provider>
    );
}

export default BackgroundSyncContext;
export { BackgroundSyncProvider, SyncMode };