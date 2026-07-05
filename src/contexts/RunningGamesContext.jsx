import { createContext, useCallback, useEffect, useMemo, useState } from "react";
import { invoke } from "@tauri-apps/api/core";
import { listen } from "@tauri-apps/api/event";

const RunningGamesContext = createContext();

/**
 * Tracks which accounts currently have the RotMG game running.
 *
 * The Rust backend detects running game processes, decodes the account email
 * from each process' launch parameters, and emits the full set of running
 * emails via the `running-game-accounts-changed` event whenever it changes.
 * We also fetch the current set once on mount so the state is correct even if
 * the game was already running before this provider mounted.
 */
function RunningGamesContextProvider({ children }) {
    const [runningEmails, setRunningEmails] = useState(() => new Set());

    useEffect(() => {
        let mounted = true;
        let unlisten;
        let gotLiveUpdate = false;

        // Attach the live listener FIRST, then seed the initial snapshot. This
        // ordering guarantees no change emitted after mount is dropped, and the
        // guard below ensures the (older) snapshot can never overwrite a live event.
        listen('running-game-accounts-changed', (event) => {
            const emails = event?.payload?.emails;
            if (Array.isArray(emails)) {
                gotLiveUpdate = true;
                if (mounted) setRunningEmails(new Set(emails));
            }
        })
            .then((fn) => {
                if (!mounted) {
                    // Provider unmounted before the listener resolved.
                    fn();
                    return;
                }
                unlisten = fn;

                // Initial state (covers games already running before we mounted),
                // applied only if no live event has already superseded it.
                return invoke('get_running_game_accounts').then((emails) => {
                    if (mounted && !gotLiveUpdate && Array.isArray(emails)) {
                        setRunningEmails(new Set(emails));
                    }
                });
            })
            .catch((err) => console.error('Failed to initialize running game accounts:', err));

        return () => {
            mounted = false;
            if (unlisten) unlisten();
        };
    }, []);

    const isAccountRunning = useCallback(
        (email) => !!email && runningEmails.has(email),
        [runningEmails]
    );

    const contextValue = useMemo(
        () => ({ runningEmails, isAccountRunning }),
        [runningEmails, isAccountRunning]
    );

    return (
        <RunningGamesContext.Provider value={contextValue}>
            {children}
        </RunningGamesContext.Provider>
    );
}

export default RunningGamesContext;
export { RunningGamesContextProvider };
