import { useEffect, useState, createContext } from "react";
import _ from "lodash";
import { invoke } from '@tauri-apps/api/core';
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

const getDisableAutoHideOnDailyLoginStartup = async () => {
    return invoke('get_user_data_by_key', { key: 'disable_auto_hide_on_daily_login_startup' })
        .then((res) => {
            if (res && res.dataValue) {
                return res.dataValue === 'true';
            }
            return false;
        })
        .catch(() => {
            invoke('insert_or_update_user_data', { userData: { dataKey: 'disable_auto_hide_on_daily_login_startup', dataValue: 'false' } }).catch(console.error);
            return false;
        });
}

const defaultSettings = {
    general: {
        theme: "dark",
        minimizeToTray: true,
        hideOnAutostart: false,
        discordRichPresenceEnabled: true,
    },
    accounts: {
        columnsHidden: {
            state: false,
            comment: false,
            orderId: false,
        },
        hideEmojis: false,
    },
    game: {
        defaultServer: "Last server",
        //HIDDEN: exePath: string // Path to the game executable, store in the database
    },
    vaultPeeker: {
        collapsedFileds: {
            filter: true,
            totals: false,
            accounts: [] //Array of account emails that are collapsed
        },
        accountView: {
            hiddenVaults: [], //Array of vault names that are hidden
        },
        rowsPerPage: 10,
    },
    backgroundSync: {
        enabled: true,
    },
    dailyLogin: {
        closeAfterFinish: false,
        //HIDDEN: disableAutoHideOnDailyLoginStartup: boolean // Whether to disable auto-hide on daily login startup, store in the database
    },
    widgets: {
        widgetBars: {

        },
        widgets: {
            
        }
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

        const userSettingsDataToSave = { ...userSettingsData };
        // Remove any transient data
        if (userSettingsDataToSave?.game?.exePath) {
            delete userSettingsDataToSave.game.exePath;
        }
        if (userSettingsDataToSave?.dailyLogin?.disableAutoHideOnDailyLoginStartup !== null) {
            delete userSettingsDataToSave.dailyLogin.disableAutoHideOnDailyLoginStartup;
        }

        localStorage.setItem("userSettings", JSON.stringify(userSettingsDataToSave));

        const lastUpdated = sessionStorage.getItem("lastUserSettingsUpdated");
        if (lastUpdated && new Date().getTime() - new Date(lastUpdated).getTime() < 30_000) {
            //When the user settings where updated less than 30 seconds ago, don't log it to the audit log
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
            const request = { key, subKey };
            switch (true) {
                case request.key === 'game' && request.subKey === 'exePath': {
                    if (userSettingsData && userSettingsData[key] && userSettingsData[key][subKey]) {
                        return userSettingsData[key][subKey];
                    }

                    const gameExePath = getGameExePath();
                    if (gameExePath) {
                        setUserSettingsData({ ...userSettingsData, [key]: { ...userSettingsData?.[key], [subKey]: gameExePath } });
                    }

                    return gameExePath;
                }
                case request.key === 'dailyLogin' && request.subKey === 'disableAutoHideOnDailyLoginStartup':
                    return getDisableAutoHideOnDailyLoginStartup();
                default: {
                    return userSettingsData && userSettingsData[key] ? userSettingsData[key][subKey] : null;
                }
            }
        },
        setByKeyAndSubKey: (key, subKey, value) => {
            switch (true) {
                case key === 'game' && subKey === 'exePath':
                    invoke('insert_or_update_user_data', { userData: { dataKey: 'game_exe_path', dataValue: value } });
                    return;
                case key === 'dailyLogin' && subKey === 'disableAutoHideOnDailyLoginStartup':
                    console.log('Setting daily_login_disable_auto_hide to', value);
                    invoke('insert_or_update_user_data', { userData: { dataKey: 'daily_login_disable_auto_hide', dataValue: value?.toString() } });
                    return;
                default: {
                    const data = { ...userSettingsData };
                    data[key] ? data[key][subKey] = value : data[key] = { [subKey]: value };
                    setUserSettingsData(data);
                }
            }
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

export { UserSettingsProvider, UserSettingsContext };