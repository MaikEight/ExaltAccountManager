import { createContext, useEffect, useState } from "react";
import {
    GAME_DATA_CACHED_TEXTS,
    GAME_DATA_STATUS_IMAGES,
    GAME_DATA_UNAVAILABLE_TEXTS,
} from "../constants";

/**
 * How the current game data was obtained.
 *
 * The provider lives above MainProviders, because the manifest is resolved
 * before the application renders. A default is supplied so components rendered
 * outside the provider, such as the fatal error page, can read it safely.
 */
const GameDataStatusContext = createContext({
    state: "ready",
    message: null,
    isRefreshing: false,
    flavour: null,
    retry: () => null,
});

const pickFlavour = (state) => {
    const texts = state === "degraded" ? GAME_DATA_UNAVAILABLE_TEXTS : GAME_DATA_CACHED_TEXTS;
    return {
        state,
        ...texts[Math.floor(Math.random() * texts.length)],
        image: GAME_DATA_STATUS_IMAGES[Math.floor(Math.random() * GAME_DATA_STATUS_IMAGES.length)],
    };
};

function GameDataStatusContextProvider({ status, isRefreshing, onRetry, children }) {
    const state = status?.state ?? "ready";
    const isProblem = state === "cached" || state === "degraded";
    const [flavour, setFlavour] = useState(null);

    useEffect(() => {
        if (!isProblem) {
            // Cleared on recovery, so the next occurrence draws again rather
            // than repeating whatever was shown last time.
            setFlavour(null);
            return;
        }

        // Drawn once per occurrence: hovering the chip repeatedly keeps saying
        // the same thing, but a problem that comes back later reads differently.
        // Re-drawn when the state changes, because each state has its own texts.
        setFlavour((current) => (current?.state === state ? current : pickFlavour(state)));
    }, [isProblem, state]);

    const contextValue = {
        state,
        message: status?.message ?? null,
        isRefreshing: Boolean(isRefreshing),
        flavour,
        retry: onRetry ?? (() => null),
    };

    return (
        <GameDataStatusContext.Provider value={contextValue}>
            {children}
        </GameDataStatusContext.Provider>
    );
}

export default GameDataStatusContext;
export { GameDataStatusContextProvider };
