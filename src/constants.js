import { invoke } from '@tauri-apps/api/core';

export const APP_VERSION = '4.2.3b';
export const APP_VERSION_RELEASE_DATE = '15.04.2025';
export const IS_PRE_RELEASE = true;

export function isUpdateAvailable(latestVersion) {
    if (!latestVersion) return false;

    const current = APP_VERSION.split('.');
    const latest = latestVersion.split('.');
    
    for (let i = 0; i < current.length; i++) {
        if (parseInt(current[i], 10) < parseInt(latest[i], 10)) return true;
        if (parseInt(current[i], 10) > parseInt(latest[i], 10)) return false;
    }

    return false;
}

export const SAVE_FILE_PATH = async () => await invoke('get_save_file_path');

export const HWID_FILE_PATH = async () => await invoke('combine_paths', { path1: await SAVE_FILE_PATH(), path2: HWID_FILE_NAME });
export const SERVER_LIST_FILE_PATH = async () => await invoke('combine_paths', { path1: await SAVE_FILE_PATH(), path2: SERVER_LIST_FILE_NAME });

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
        case 1: {
            if (!values) return updateBaseUrls[index];
            const v = updateBaseUrls[index].replace("{0}", values);        
            return v;
        }
        case 2:
            if (values.length < 2) return updateBaseUrls[index];
            return updateBaseUrls[index].replace("{0}", values[0]).replace("{1}", values[1]);
        default:
            return null;
    }
}

export const AUTH0_CLIENT_ID = 'o1W1coVQMV9qrIg4G2SmZJbz1G5vRCpZ';
export const AUTH0_DOMAIN = 'https://login.exaltaccountmanager.com';
export const STRIPE_CUSTOMER_PORTAL_URL = 'https://billing.stripe.com/p/login/test_dR63erdeEeUo5nGcMM';

export const DISCORD_APPLICATION_ID = '1069308775854526506';