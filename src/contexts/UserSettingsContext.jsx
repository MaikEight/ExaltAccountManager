import { useEffect, useState, createContext } from "react";
import _ from "lodash";

const UserSettingsContext = createContext();

const defaultSettings = {
    general: {
        theme: "dark",
    },
    game: {
        exePath: "C:\\Users\\Maik8\\Documents\\RealmOfTheMadGod\\Production\\RotMG Exalt.exe",
    }
};

function expandSettings(settings) {
    if (!settings) return defaultSettings;
    if (settings?.gatewayTable?.grouping === 'NONE') settings.gatewayTable.grouping = 'None';
    return _.defaultsDeep(settings, defaultSettings);
};

function UserSettingsProvider({ children }) {
    const [userSettingsData, setUserSettingsData] = useState(null);

    useEffect(() => {
        if (!userSettingsData) return;
        localStorage.setItem("userSettings", JSON.stringify(userSettingsData));

        //TODO: Save user settings
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
        getByKey: (key) => { return userSettingsData ? userSettingsData[key] : null; },
        setByKey: (key, value) => { setUserSettingsData({ ...userSettingsData, [key]: value }); },
        getByKeyAndSubKey: (key, subKey) => { return userSettingsData && userSettingsData[key] ? userSettingsData[key][subKey] : null; },
        setByKeyAndSubKey: (key, subKey, value) => {
            const data = { ...userSettingsData };
            data[key] ? data[key][subKey] = value : data[key] = { [subKey]: value };
            setUserSettingsData(data);
        },
        reset: () => {
            setUserSettingsData(defaultSettings);
            //TODO: Save user settings
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