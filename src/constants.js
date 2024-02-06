import { invoke } from '@tauri-apps/api';

export const APP_NAME = 'Exalt Account Manager';
export const APP_VERSION = '4.0.0';

export function isUpdateAvailable(latestVersion) {
    if (!latestVersion) return false;

    const current = APP_VERSION.split('.');
    const latest = latestVersion.split('.');
    
    for (let i = 0; i < current.length; i++) {
        if (parseInt(current[i]) < parseInt(latest[i])) return true;
        if (parseInt(current[i]) > parseInt(latest[i])) return false;
    }

    return false;
}

export const SAVE_FILE_PATH = async () => await invoke('get_save_file_path');

export const ACCOUNTS_FILE_PATH = async () => await invoke('combine_paths', { path1: await SAVE_FILE_PATH(), path2: SAVE_FILE_NAME });
export const GROUPS_FILE_PATH = async () => await invoke('combine_paths', { path1: await SAVE_FILE_PATH(), path2: GROUPS_FILE_NAME });
export const HWID_FILE_PATH = async () => await invoke('combine_paths', { path1: await SAVE_FILE_PATH(), path2: HWID_FILE_NAME });
export const SERVER_LIST_FILE_PATH = async () => await invoke('combine_paths', { path1: await SAVE_FILE_PATH(), path2: SERVER_LIST_FILE_NAME });

export const SAVE_FILE_NAME = 'accounts.json';
export const GROUPS_FILE_NAME = 'groups.json';
export const HWID_FILE_NAME = 'EAM.HWID';
export const SERVER_LIST_FILE_NAME = 'serverList.json';

// export const EAM_BASE_URL = 'http://localhost:5066/'; //For development purposes only
export const EAM_BASE_URL = 'https://api.exalt-account-manager.eu/';

export const ROTMG_BASE_URL = 'https://www.realmofthemadgod.com';

const updateBaseUrls = [
    "https://www.realmofthemadgod.com/app/init?platform=standalonewindows64&key=9KnJFxtTvLu2frXv",
    "{0}/rotmg-exalt-win-64/checksum.json", //TODO: add Mac and Linux support
    "build-release/{0}/rotmg-exalt-win-64/{1}.gz" //TODO: add Mac and Linux support
];

export function UPDATE_URLS(index, values) {
    switch (index) {
        case 0:
            return updateBaseUrls[index];
        case 1:
            if (!values) return updateBaseUrls[index];
            const v = updateBaseUrls[index].replace("{0}", values);        
            return v;
        case 2:
            if (values.length < 2) return pdateBaseUrls[index];
            return updateBaseUrls[index].replace("{0}", values[0]).replace("{1}", values[1]);
        default:
            return null;
    }
}