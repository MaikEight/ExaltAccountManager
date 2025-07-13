import { createContext, useEffect, useRef, useState } from "react";
import { invoke } from '@tauri-apps/api/core';
import { listen } from '@tauri-apps/api/event';
import { getRequestState, storeCharList, xmlToJson } from "eam-commons-js";
import useAccounts from "../hooks/useAccounts";
import BackgroundSyncComponent from "../components/BackgroundSyncComponent";
import { Box, Typography } from "@mui/material";
import { MASCOT_NAME } from "../constants";
import useSnack from "../hooks/useSnack";

const dailyLoginCompletedMessages = [
    `Mission complete! ${MASCOT_NAME} handled your daily login like a pro.`,
    `All done! ${MASCOT_NAME} just finished your daily login adventure.`,
    `${MASCOT_NAME} checked in for you — smooth, fast, and flawless.`,
    `${MASCOT_NAME} has completed the daily login. Time to reap the rewards!`,
    `Daily login? Handled. ${MASCOT_NAME} was faster than a Realm boss wipe.`,
    `${MASCOT_NAME} did the daily grind so you don't have to. All done!`,
    `The login ritual is complete. ${MASCOT_NAME} bows dramatically.`,
    `${MASCOT_NAME} snuck in, logged you in, and snuck back out. Mission accomplished.`,

    `${MASCOT_NAME} has successfully completed the daily login. Smooth as always.`,
    `Daily login done! ${MASCOT_NAME} handled it behind the scenes.`,
    `${MASCOT_NAME} logged in quietly and efficiently. Mission complete.`,
    `All set! ${MASCOT_NAME} performed the daily login like a true professional.`,
    `${MASCOT_NAME} checked in for the day. Everything's ready.`,
    `The daily login is finished. ${MASCOT_NAME} didn't even trip over any wires!`,
    `${MASCOT_NAME} just wrapped up the daily login routine.`,
    `Done and dusted. ${MASCOT_NAME} took care of the login for you.`,
    `${MASCOT_NAME} handled the daily login while you were doing more important things.`,
    `Routine login complete. ${MASCOT_NAME} was very sneaky about it.`,
];

const getRandomMessage = () => {
    const randomIndex = Math.floor(Math.random() * dailyLoginCompletedMessages.length);
    return dailyLoginCompletedMessages[randomIndex];
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

    const [syncMode, setSyncMode] = useState(SyncMode.Stopped);
    const [uiState, setUiState] = useState({
        email: '',
        state: '',
        updatedAt: null,
    });
    const [dailyLoginProgressData, setDailyLoginProgressData] = useState(null);

    const checkAndAddEventId = (eventName, id, debugFlag = false) => {
        if (!processedEventIds.current[eventName]) {
            processedEventIds.current[eventName] = [];
        }

        if (!processedEventIds.current[eventName].includes(id)) {
            processedEventIds.current[eventName].push(id);

            if (debugFlag) {
                console.log('Processing background sync event:', event);
            }

            return true;
        }

        return false;
    }

    const processBackgroundSyncEvent = async (event) => {
        const debugFlag = sessionStorage.getItem('flag:debug') === 'true';

        if (event.payload.AccountCharListSync) {
            const e = event.payload.AccountCharListSync;
            if (!checkAndAddEventId('AccountCharListSync', e.id, debugFlag)) {
                return;
            }

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
            return;
        }

        if (event.payload.AccountStarted) {
            const e = event.payload.AccountStarted;

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

            // if (e.state.startsWith("Failed")) {
            //     //example e.state = "Failed("<Error>WebChangePasswordDialog.passwordError</Error>")"
            //     const errorXml = e.state.substring(e.state.indexOf('(') + 1, e.state.lastIndexOf(')'));
            //     const errorJson = xmlToJson(errorXml);
            //     state = getRequestState(errorJson);
            // } else if (e.state.startsWith("Success")) {
            //     state = "Success";
            // } else {
            //     state = "BGSyncError";
            // }

            if (debugFlag) {
                console.warn('Processing AccountFailed event:', e);
            }

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
            return;
        }

        //Daily Login Requests

        if (event.payload.DailyLoginProgress) {
            const e = event.payload.DailyLoginProgress;
            if (!checkAndAddEventId('DailyLoginProgress', e.id, debugFlag)) {
                return;
            }

            const getAccNameByEmail = (email) => {
                if (emailToAccountNameMap.current[email]) {
                    return emailToAccountNameMap.current[email];
                }
                const acc = loadAccountByEmail(email);
                if (acc && acc.name) {
                    emailToAccountNameMap.current[email] = acc.name;
                }
                return acc ? acc.name : email;
            }

            const accNames = e.left_emails.map(email => getAccNameByEmail(email));
            const failedAccNames = e.failed_emails.map(email => getAccNameByEmail(email));

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
                            alt="Okta the mascot"
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
            return;
        }

        if (event.payload.ModeChanged) {
            const e = event.payload.ModeChanged;

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

            }

            return;
        }

        if (debugFlag) {
            console.warn('Unknown background sync event received:', event);
        }
        return;
    }

    useEffect(() => {
        let eventListener;

        const registerEventListener = async () => {
            eventListener = await listen('background-sync-event', (event) => {
                processBackgroundSyncEvent(event);
            });
        }

        registerEventListener();        

        return () => {
            eventListener?.();
        }
    }, [accounts, loadAccountByEmail, updateAccount]);

    useEffect(() => {
        let timeoutId;
        if (syncMode === SyncMode.Default || syncMode === SyncMode.Stopped) {
            timeoutId = setTimeout(() => {
                setUiState({
                    email: '',
                    state: '',
                    updatedAt: null,
                });
            }, uiState.state === 'Waiting' ? 315_000 : 7_500); // Clear state after 7.5 seconds or 5 minutes if waiting
        }

        return () => {
            if (timeoutId) {
                clearTimeout(timeoutId);
            }
        };
    }, [uiState]);

    useEffect(() => {
        //DISABLED FOR NOW
        const createAndStartBackgroundSyncManager = async () => {
            try {
                await invoke('create_background_sync_manager');

                const needsDailyLogin = await invoke('needs_to_do_daily_login').catch((error) => {
                    console.error('Error checking daily login needs:', error)
                    return false;
                });
                console.log('Needs to do daily login:', needsDailyLogin);

                const isRunning = await invoke('is_background_sync_manager_running').catch((error) => {
                    console.error('Error checking if background sync manager is running:', error);
                    return true; // Assume true if there's an error
                });
                console.log('Is background sync manager running:', isRunning);

                if (needsDailyLogin) {
                    const currentMode = await invoke('get_current_background_sync_mode').catch((error) => {
                        console.error('Error getting current background sync mode:', error);
                        return SyncMode.Default; // Default to Default if there's an error just to be safe
                    });
                    console.log('Current background sync mode:', currentMode);

                    if (isRunning) {
                        if (currentMode === SyncMode.DailyLogin) {
                            // The manager is running as expected.
                            return;
                        }
                        await invoke('stop_background_sync_manager');
                    }
                    await invoke('change_background_sync_mode', { mode: SyncMode.DailyLogin });
                    await invoke('start_background_sync_manager');
                    return;
                }

                await invoke('change_background_sync_mode', { mode: SyncMode.Default });
                await invoke('start_background_sync_manager');
            } catch (error) {
                console.error('Failed to create background sync manager:', error);
            }
        }
        //createAndStartBackgroundSyncManager();
    }, []);

    const contextValue = {
        syncMode,
        setSyncMode,
        uiState,
        setUiState,
        dailyLoginProgressData,
        emailToAccountNameMap
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