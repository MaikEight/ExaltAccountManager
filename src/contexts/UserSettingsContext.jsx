import { useEffect, useState, createContext } from "react";
import _ from "lodash";
import { invoke } from '@tauri-apps/api/tauri';
import { logToAuditLog } from "../utils/loggingUtils";

const UserSettingsContext = createContext();

const getGameExePath = async () => {
    return invoke('get_user_data_by_key', { key: 'game_exe_path' })
        .then((res) => {
            if (res && res.dataValue) {
                return res.dataValue;
            }
            return invoke('get_default_game_path')
                .then((res) => {
                    return res;
                })
                .catch((err) => {
                    console.error('Failed to get game exe path', err);
                    return null;
                });
        })
        .catch((err) => {
            console.warn('Failed to get game exe path (1/2)', err);
            return invoke('get_default_game_path')
                .then((res) => {
                    console.log('Got game exe path (2/2)', res);
                    return res;
                })
                .catch((err) => {
                    console.error('Failed to get game exe path (2/2)', err);
                    return null;
                });
        });
};

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

    if (settings?.game?.exePath) {
        delete settings.game.exePath;
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
        const lastUpdated = sessionStorage.getItem("lastUserSettingsUpdated");
        if (lastUpdated && new Date().getTime() - new Date(lastUpdated).getTime() < 15000) {
            //When the user settings where updated less than 15 seconds ago, don't log it to the audit log
            //This is to prevent spamming the audit log with user settings updates
            return;
        }

        sessionStorage.setItem("lastUserSettingsUpdated", new Date().toISOString());
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
        getByKeyAndSubKey: (key, subKey) => {
            if (key === 'game' && subKey === 'exePath') {
                return getGameExePath();
            }

            return userSettingsData && userSettingsData[key] ? userSettingsData[key][subKey] : null;
        },
        setByKeyAndSubKey: (key, subKey, value) => {
            if (key === 'game' && subKey === 'exePath') { //C:\\Users\\Maik8\\Documents\\RealmOfTheMadGod\\Production\\RotMG Exalt.exe
                invoke('insert_or_update_user_data', { userData: { dataKey: 'game_exe_path', dataValue: value } });
                return;
            }
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