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
    const { getAccountByEmail, updateAccount } = useAccounts();

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
                if (Boolean(state) && Boolean(acc)) {
                    acc.state = state;
                    acc.lastRefresh = new Date();
                    await updateAccount(acc);
                } else {
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
        let eventListener;

        const registerEventListener = async () => {
            eventListener = await listen('background-sync-event', (event) => {
                processBackgroundSyncEvent(event);
            });
        }

        const createAndStartBackgroundSyncManager = async () => {
            try {
                await invoke('create_background_sync_manager');
                await invoke('start_background_sync_manager');
            } catch (error) {
                console.error('Failed to create background sync manager:', error);
            }
        }
        registerEventListener();
        createAndStartBackgroundSyncManager();

        return () => {
            eventListener?.();
        }
    }, []);

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