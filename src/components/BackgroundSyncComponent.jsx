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
    const { getAccountByEmail, updateAccount, accounts } = useAccounts();

    const [uiState, setUiState] = useState({
        email: '',
        state: '',
        updatedAt: null,
    });

    const processBackgroundSyncEvent = async (event) => {
        if (event.payload.AccountCharListSync) {
            const e = event.payload.AccountCharListSync;
            if (!processedCharListSyncEventIds.current.includes(e.id)) {
                processedCharListSyncEventIds.current.push(e.id);

                const charList = xmlToJson(e.dataset);

                const state = getRequestState(charList);
                const acc = getAccountByEmail(e.email);

                if (state === 'AccountSuspended' || state === 'WrongPassword') {
                    console.warn('Account suspended or wrong password for email:', e.email, 'with state:', state, acc);

                    setUiState({
                        email: e.email,
                        state: state,
                        updatedAt: new Date(),
                    });

                    if (Boolean(acc)) {
                        console.log('Updating account state for email:', e.email, 'with state:', state);
                        acc.state = state;
                        await updateAccount(acc);
                    }
                    return;
                }

                if (Boolean(state) && Boolean(acc)) {
                    acc.state = state;
                    acc.lastRefresh = new Date();
                    await updateAccount(acc);
                } else {
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
                console.log('Stored char list for email:', e.email, charList);
                return;
            }
            return;
        }

        if (event.payload.AccountStarted) {
            const e = event.payload.AccountStarted;
            const acc = getAccountByEmail(e);

            if (acc) {
                emailToAccountNameMap.current[e] = acc.name;
                setUiState({
                    email: acc.email,
                    state: 'Started',
                    updatedAt: new Date(),
                });
                return;
            }

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
            return;
        }

        if (event.payload.AccountProgress) {
            const e = event.payload.AccountProgress;
            setUiState({
                email: e[0],
                state: e[1],
                updatedAt: new Date(),
            });
            return;
        }

        if (event.payload.AccountFinished) {
            const e = event.payload.AccountFinished;
            setUiState({
                email: e[0],
                state: e[1],
                updatedAt: new Date(),
            });
            return;
        }

        console.warn('Unknown background sync event received:', event);
        return;
    }

    useEffect(() => {
        console.log('BackgroundSyncComponent mounted with accounts:', accounts?.length || -1);
        let eventListener;

        const registerEventListener = async () => {
            eventListener = await listen('background-sync-event', (event) => {
                processBackgroundSyncEvent(event);
            });
        }

        registerEventListener();

        return () => {
            console.log('Cleaning up background sync event listener', accounts?.length || -1);
            eventListener?.();
        }
    }, [accounts, getAccountByEmail, updateAccount]);

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