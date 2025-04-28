import { createContext, useEffect, useState } from "react";
import { start, setActivity } from "tauri-plugin-drpc";
import { Activity, Assets, Button, Timestamps, ActivityType } from "tauri-plugin-drpc/activity";
import { DISCORD_APPLICATION_ID } from "../constants";
import { useLocation } from "react-router-dom";

const DiscordContext = createContext();

function DiscordContextProvider({ children }) {
    const location = useLocation();

    const [state, setState] = useState("EAM by Maik8");
    const [details, setDetails] = useState("The better rotmg-launcher! 💪");
    const [largeImageKey, setLargeImageKey] = useState("eam_darkmode");
    const [largeImageText, setLargeImageText] = useState("Exalt Account Manager");
    const [smallImageKey, setSmallImageKey] = useState("");
    const [smallImageText, setSmallImageText] = useState("");
    const [startTimestamp, setStartTimestamp] = useState(Date.now());
    const [endTimestamp, setEndTimestamp] = useState(0);

    const updateActivity = async () => {
        const assets = new Assets();
        if (largeImageKey) assets.setLargeImage(largeImageKey);
        if (largeImageText) assets.setLargeText(largeImageText);
        if (smallImageKey) assets.setSmallImage(smallImageKey);
        if (smallImageText) assets.setSmallText(smallImageText);

        const timestamps = new Timestamps(
            startTimestamp ? startTimestamp : Date.now(),
            endTimestamp ? endTimestamp : null,
        );

        const activity = new Activity()
            .setActivity(ActivityType.Playing)
            .setAssets(assets)
            .setTimestamps(timestamps)
            .setButton([
                new Button("Get Exalt Account Manager here", "https://exaltaccountmanager.com")
            ]);

        if (state) activity.setState(state);
        if (details) activity.setDetails(details);

        await setActivity(activity);
    }

    const getDefaultStateForPath = (path) => {
        let state = "EAM by Maik8";
        switch (path) {
            case '/':
                state = 'Selecting accounts 📁';
                break;
            case '/error':
                state = 'Encountering an error 🚨';
                break;
            case '/accounts':
                state = 'Selecting accounts 📁';
                break;
            case '/vaultPeeker':
                state = 'Peeking into the vault 🔍';
                break;
            case '/dailyLogins':
                state = 'Viewing the Daily auto Login 📅';
                break;
            case '/utilities':
                state = 'Using the utilities ⚒️';
                break;
            case '/settings':
                state = 'Playing with settings ⚙️';
                break;
            case '/logs':
                state = 'Living in the past 📜';
                break;
            case '/profile':
                state = 'Viewing your profile 👤';
                break;
            case '/about':
                state = 'Reading about EAM 📖';
                break;
            case '/feedback':
                state = 'Giving feedback to the devs 📝';
                break;
            case '/importer':
                state = 'Importing accounts 📥';
                break;
            case '/payment/successful':
                state = 'Upgrading to EAM Plus 🥳';
                break;
            default:
                state = 'EAM by Maik8';
                break;
        }
        return state;
    }

    const resetState = (path) => {
        if (!path) {
            path = location.pathname;
        }
        const _state = getDefaultStateForPath(path);
        setState(_state);
    }

    const contextValue = {
        state, setState,
        details, setDetails,
        largeImageKey, setLargeImageKey,
        largeImageText, setLargeImageText,
        smallImageKey, setSmallImageKey,
        smallImageText, setSmallImageText,
        startTimestamp, setStartTimestamp,
        endTimestamp, setEndTimestamp,

        resetState,
    };

    useEffect(() => {
        const startup = async () => {
            await start(DISCORD_APPLICATION_ID);
        }
        startup();
    }, []);

    useEffect(() => {
        resetState(location.pathname);
    }, [location.pathname]);

    useEffect(() => {
        const update = async () => {
            await updateActivity();
        }
        update();
    }, [state, details, largeImageKey, largeImageText, smallImageKey, smallImageText, startTimestamp, endTimestamp]);

    return (
        <DiscordContext.Provider value={contextValue}>
            {children}
        </DiscordContext.Provider>
    );
}

export { DiscordContextProvider };
export default DiscordContext;