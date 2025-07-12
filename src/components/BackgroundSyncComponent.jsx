import { useEffect, useRef, useState } from "react";
import { invoke } from '@tauri-apps/api/core';
import { listen } from '@tauri-apps/api/event';
import { getRequestState, storeCharList, xmlToJson } from "eam-commons-js";
import useAccounts from "../hooks/useAccounts";
import { Chip } from "@mui/material";
import SyncRoundedIcon from '@mui/icons-material/SyncRounded';

function BackgroundSyncComponent() {
    const processedCharListSyncEventIds = useRef([]);
    const emailToAccountNameMap = useRef({});
    const { loadAccountByEmail, updateAccount, accounts } = useAccounts();

    const [uiState, setUiState] = useState({
        email: '',
        state: '',
        updatedAt: null,
    });

    const processBackgroundSyncEvent = async (event) => {
        const debugFlag = sessionStorage.getItem('flag:debug') === 'true';
        if (debugFlag) {
            console.log('Processing background sync event:', event);
        }

        if (event.payload.AccountCharListSync) {
            const e = event.payload.AccountCharListSync;
            if (!processedCharListSyncEventIds.current.includes(e.id)) {
                processedCharListSyncEventIds.current.push(e.id);

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
            return;
        }

        if (event.payload.AccountStarted) {
            const e = event.payload.AccountStarted;
            const acc = await loadAccountByEmail(e);

            if (acc) {
                emailToAccountNameMap.current[e] = acc.name;
                setUiState({
                    email: acc.email,
                    state: 'Started',
                    updatedAt: new Date(),
                });
                return;
            }

            if (debugFlag) {
                console.log('AccountStarted Debug:', {
                    eventEmail: e,
                    emailType: typeof e,
                    emailLength: e?.length,
                    foundAccount: Boolean(acc),
                    accountEmail: acc?.email,
                    totalAccountsAvailable: accounts?.length || 0,
                    availableEmails: accounts?.map(a => a.email) || [],
                    emailToAccountMapSize: Object.keys(emailToAccountNameMap.current).length
                });
                console.warn('Account not found for email:', e);
            }
            return;
        }

        if (event.payload.AccountProgress) {
            const e = event.payload.AccountProgress;
            
            let state = "";
            if (e[1].startsWith("Failed")){
                //example e[1] = "Failed("<Error>WebChangePasswordDialog.passwordError</Error>")"
                const errorXml = e[1].substring(e[1].indexOf('(') + 1, e[1].lastIndexOf(')'));
                const errorJson = xmlToJson(errorXml);
                state = getRequestState(errorJson);                
            } else if (e[1].startsWith("Success")) {
                state = "Success";
            } else {
                state = "BGSyncError";
            }

            setUiState({
                email: e[0],
                state: e[1],
                updatedAt: new Date(),
            });
            return;
        }

        if (event.payload.AccountFinished) {
            const e = event.payload.AccountFinished;
            if (debugFlag) {
                console.log('AccountFinished Debug:', {
                    eventEmail: e[0],
                    emailType: typeof e[0],
                    emailLength: e[0]?.length,
                    state: e[1],
                    totalAccountsAvailable: accounts?.length || 0,
                    availableEmails: accounts?.map(a => a.email) || [],
                    emailToAccountMapSize: Object.keys(emailToAccountNameMap.current).length
                });
            }

            setUiState({
                email: e[0],
                state: e[1],
                updatedAt: new Date(),
            });
            return;
        }

        if (event.payload.AccountFailed) {
            const e = event.payload.AccountFailed;
            const errorXml = e[1];
            const errorJson = xmlToJson(errorXml);
            let acc = await loadAccountByEmail(e[0]);
            let state = getRequestState(errorJson);
            if (!state || state === 'Success') {
                state = "BGSyncError";
            }

            if (debugFlag) {
                console.warn('AccountFailed Debug:', {
                    eventEmail: e[0],
                    emailType: typeof e[0],
                    emailLength: e[0]?.length,
                    state: state,
                    foundAccount: Boolean(acc),
                    account: acc,
                    totalAccountsAvailable: accounts?.length || 0,
                    availableEmails: accounts?.map(a => a.email) || [],
                    emailToAccountMapSize: Object.keys(emailToAccountNameMap.current).length
                });
                console.warn('Account failed for email:', e[0], 'with state:', state, acc);
            }

            if (acc) {
                emailToAccountNameMap.current[e[0]] = acc.name;
                acc.state = state;
                await updateAccount(acc);
            }

            setUiState({
                email: e[0],
                state: "Failed",
                updatedAt: new Date(),
            });
            return;
        }

        if (event.payload.ModeChanged) {
            const e = event.payload.ModeChanged;
            if (debugFlag) {
                console.log('Background Sync Mode Changed:', e);
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
        const timeoutId = setTimeout(() => {
            setUiState({
                email: '',
                state: '',
                updatedAt: null,
            });
        }, 7_500); // Clear state after 7.5 seconds
        return () => {
            clearTimeout(timeoutId);
        };
    }, [uiState]);

    useEffect(() => {
        const createAndStartBackgroundSyncManager = async () => {
            try {
                await invoke('create_background_sync_manager');
                await invoke('start_background_sync_manager');
            } catch (error) {
                console.error('Failed to create background sync manager:', error);
            }
        }
        createAndStartBackgroundSyncManager();
    }, []);

    return <>
        {
            uiState.email && uiState.state &&
            uiState.updatedAt &&
            <Chip
                label={`Background Sync: ${emailToAccountNameMap?.current?.[uiState.email] ? emailToAccountNameMap?.current?.[uiState.email] : uiState.email} - ${uiState.state}`}
                color="primary"
                variant="outlined"
                size="small"
                icon={<SyncRoundedIcon />}
                onClick={() => null}
                clickable={false}
            />
        }

    </>;
}

export default BackgroundSyncComponent;