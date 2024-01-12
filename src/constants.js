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

export const EAM_BASE_URL = 'https://api.exalt-account-manager.eu/';

export const ROTMG_BASE_URL = 'https://www.realmofthemadgod.com';

const updateBaseUrls = [
    "https://www.realmofthemadgod.com/app/init?platform=standalonewindows64&key=9KnJFxtTvLu2frXv",
    "https://rotmg-build.decagames.com/build-release/{0}/rotmg-exalt-win-64/checksum.json", //TODO: add Mac and Linux support
    "https://rotmg-build.decagames.com/build-release/{0}/rotmg-exalt-win-64/{1}.gz" //TODO: add Mac and Linux support
];

export function UPDATE_URLS(index, values) {
    switch (index) {
        case 0:
            return updateBaseUrls[index];
        case 1:
            if (!values) return updateBaseUrls[index];
            const v = updateBaseUrls[index].replace("{0}", values);
            console.log(v);            
            return v;
        case 2:
            if (values.length < 2) return pdateBaseUrls[index];
            return updateBaseUrls[index].replace("{0}", values[0]).replace("{1}", values[1]);
        default:
            return null;
    }
}

//TEMPORARY
export const SERVERS = [
    {
        "name": "EUEast",
        "ip": "18.184.218.174",
        "usage": 0
    },
    {
        "name": "EUSouthWest",
        "ip": "35.180.67.120",
        "usage": 0
    },
    {
        "name": "USEast2",
        "ip": "54.209.152.223",
        "usage": 0
    },
    {
        "name": "EUNorth",
        "ip": "18.159.133.120",
        "usage": 0
    },
    {
        "name": "USEast",
        "ip": "54.234.226.24",
        "usage": 75
    },
    {
        "name": "USWest4",
        "ip": "54.235.235.140",
        "usage": 0
    },
    {
        "name": "EUWest2",
        "ip": "52.16.86.215",
        "usage": 0
    },
    {
        "name": "Asia",
        "ip": "3.0.147.127",
        "usage": 0
    },
    {
        "name": "USSouth3",
        "ip": "52.207.206.31",
        "usage": 0
    },
    {
        "name": "EUWest",
        "ip": "15.237.60.223",
        "usage": 0
    },
    {
        "name": "USWest",
        "ip": "54.86.47.176",
        "usage": 0
    },
    {
        "name": "USMidWest2",
        "ip": "3.140.254.133",
        "usage": 0
    },
    {
        "name": "USMidWest",
        "ip": "18.221.120.59",
        "usage": 0
    },
    {
        "name": "USSouth",
        "ip": "3.82.126.16",
        "usage": 0
    },
    {
        "name": "USWest3",
        "ip": "18.144.30.153",
        "usage": 0
    },
    {
        "name": "USSouthWest",
        "ip": "54.153.13.68",
        "usage": 0
    },
    {
        "name": "USNorthWest",
        "ip": "34.238.176.119",
        "usage": 0
    },
    {
        "name": "Australia",
        "ip": "13.236.87.250",
        "usage": 0
    }
];