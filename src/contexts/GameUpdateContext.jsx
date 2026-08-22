import { createContext, useCallback, useEffect, useMemo, useRef, useState } from "react";
import { getCurrentWindow } from "@tauri-apps/api/window";
import { checkForUpdates, updateGame } from "eam-commons-js";

/**
 * The state of the Realm game updater, shared by everything that displays it.
 *
 * Previously each consumer polled `sessionStorage` on its own interval and kept
 * a local copy of the answer, which meant an update triggered anywhere other
 * than the Realm Updater card was invisible, a check finishing inside the poll
 * interval never rendered at all, and the taskbar was only ever driven by that
 * one card's buttons.
 *
 * Everything now goes through `checkForUpdate` and `performUpdate` here, so the
 * origin of the trigger stops mattering: the daily login, the background sync
 * and the button all produce the same visible state.
 */
const GameUpdateContext = createContext({
    isCheckingForUpdate: false,
    isUpdating: false,
    isBusy: false,
    isUpdateAvailable: false,
    lastCheckedAt: null,
    checkForUpdate: async () => false,
    performUpdate: async () => false,
});

/** Reads the flags written by eam-commons-js, for state set before we mounted. */
const readPersistedState = () => ({
    isCheckingForUpdate: sessionStorage.getItem('updateCheckInProgress') === 'true',
    isUpdating: sessionStorage.getItem('updateInProgress') === 'true',
    isUpdateAvailable: localStorage.getItem('updateNeeded') === 'true',
    lastCheckedAt: localStorage.getItem('lastUpdateCheck'),
});

function GameUpdateContextProvider({ children }) {
    const [state, setState] = useState(readPersistedState);
    const inFlight = useRef(null);

    const syncFromStorage = useCallback(() => setState(readPersistedState()), []);

    // A single indeterminate taskbar state for the whole application, driven by
    // the shared flags rather than by whichever component started the work. It
    // is cleared on unmount so a reload cannot leave the taskbar stuck.
    const isBusy = state.isCheckingForUpdate || state.isUpdating;
    useEffect(() => {
        getCurrentWindow()
            .setProgressBar({
                status: isBusy ? 'indeterminate' : 'none',
                value: isBusy ? 50 : 0,
            })
            .catch((error) => console.error('Failed to set the taskbar progress', error));
    }, [isBusy]);

    useEffect(() => () => {
        getCurrentWindow().setProgressBar({ status: 'none', value: 0 }).catch(() => null);
    }, []);

    /**
     * Runs one updater operation, keeping the shared state in step with it.
     *
     * A second caller receives the promise already running instead of starting
     * another operation, so the daily login and a button press cannot both drive
     * the updater. The library also refuses to re-enter, but returning the same
     * promise means the second caller still learns the outcome.
     */
    const run = useCallback(async (kind, operation) => {
        if (inFlight.current) {
            return inFlight.current.promise;
        }

        setState((current) => ({
            ...current,
            isCheckingForUpdate: kind === 'check',
            isUpdating: kind === 'update',
        }));

        const promise = (async () => {
            try {
                return await operation();
            } finally {
                inFlight.current = null;
                // The library owns the flags and clears them in its own finally,
                // so read them back rather than assuming what they became.
                syncFromStorage();
            }
        })();

        inFlight.current = { kind, promise };
        return promise;
    }, [syncFromStorage]);

    const checkForUpdate = useCallback(
        (force = true) => run('check', async () => {
            const updateNeeded = await checkForUpdates(force);
            return Boolean(updateNeeded);
        }),
        [run]);

    const performUpdate = useCallback(
        () => run('update', async () => {
            const updateSucceeded = await updateGame();
            return Boolean(updateSucceeded);
        }),
        [run]);

    // Covers updates started outside the application, such as by a scheduled
    // daily login task, and keeps a second window in step. Only a fallback:
    // anything going through this provider updates immediately.
    useEffect(() => {
        const intervalId = setInterval(() => {
            if (inFlight.current) return;
            setState((current) => {
                const next = readPersistedState();
                const changed = current.isCheckingForUpdate !== next.isCheckingForUpdate
                    || current.isUpdating !== next.isUpdating
                    || current.isUpdateAvailable !== next.isUpdateAvailable
                    || current.lastCheckedAt !== next.lastCheckedAt;
                return changed ? next : current;
            });
        }, 1000);
        return () => clearInterval(intervalId);
    }, []);

    const contextValue = useMemo(() => ({
        ...state,
        isBusy,
        checkForUpdate,
        performUpdate,
    }), [state, isBusy, checkForUpdate, performUpdate]);

    return (
        <GameUpdateContext.Provider value={contextValue}>
            {children}
        </GameUpdateContext.Provider>
    );
}

export default GameUpdateContext;
export { GameUpdateContextProvider };
