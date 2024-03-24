import { useEffect, useState, createContext } from "react";
import _ from "lodash";
import { invoke } from '@tauri-apps/api/tauri';
import { logToAuditLog } from "../utils/loggingUtils";
const UserSettingsContext = createContext();

const defaultSettings = {
    general: {
        theme: "dark",
    },
    accounts: {
        columnsHidden: {
            state: false,
        },
    },
    game: {
        defaultServer: "Last server",
    }
};

function expandSettings(_settings) {
    const settings = _settings ? _settings : defaultSettings;
    if (settings?.gatewayTable?.grouping === 'NONE') settings.gatewayTable.grouping = 'None';
    if(!settings?.game.exePath) {
        invoke('get_default_game_path')
        .then((res) => {
            settings.game.exePath = res;
        });
    }

    return _.defaultsDeep(settings, defaultSettings);
};

function UserSettingsProvider({ children }) {
    const [userSettingsData, setUserSettingsData] = useState(null);
    const [isFirstLoad, setIsFirstLoad] = useState(true);

    useEffect(() => {
        if (!userSettingsData) return;

        if (isFirstLoad) {
            setIsFirstLoad(false);
            return;
        }

        localStorage.setItem("userSettings", JSON.stringify(userSettingsData));
        logToAuditLog('UserSettingsProvider', 'User settings updated');
    }, [userSettingsData]);

    const initializeUserSettings = () => {
        if (userSettingsData) return;
        
        const storedConfig = localStorage.getItem("userSettings");
        
        let hasLocalConfig = false;
        if (storedConfig) {
            const data = expandSettings(JSON.parse(storedConfig));
            if (data) {
                setUserSettingsData(data);
                hasLocalConfig = true;
            }
        }
        if (!hasLocalConfig) {
            setUserSettingsData(defaultSettings);
        }
    };
    initializeUserSettings();

    const userSettings = {
        get: userSettingsData,
        set: (newUserSettings) => setUserSettingsData({ ...userSettingsData, ...newUserSettings }),
        addDefaults: newUserSettings => setUserSettingsData(expandSettings(newUserSettings)),
        getByKey: (key) => { return userSettingsData ? userSettingsData[key] : null; },
        setByKey: (key, value) => { setUserSettingsData({ ...userSettingsData, [key]: value }); },
        getByKeyAndSubKey: (key, subKey) => { return userSettingsData && userSettingsData[key] ? userSettingsData[key][subKey] : null; },
        setByKeyAndSubKey: (key, subKey, value) => {
            const data = { ...userSettingsData };
            data[key] ? data[key][subKey] = value : data[key] = { [subKey]: value };
            setUserSettingsData(data);
        },
        reset: () => {
            setUserSettingsData(expandSettings(defaultSettings));
            return true;
        }
    };

    return (
        <UserSettingsContext.Provider value={userSettings}>
            {children}
        </UserSettingsContext.Provider>
    );
}

export { UserSettingsProvider };
export default UserSettingsContext;