import { fetch } from '@tauri-apps/plugin-http';
import { invoke } from '@tauri-apps/api/core';
import { logToErrorLog } from '../utils/loggingUtils';
import { ROTMG_BASE_URL, UPDATE_URLS } from '../../constants';
import { xmlToJson } from '../utils/XmlUtils';

async function postAccountVerify(account, clientId, decryptNeeded = true) {
    if (!account || !clientId) return null;

    const url = `${ROTMG_BASE_URL}/account/verify?__source=ExaltAccountManager`;
    const pw = decryptNeeded ? await invoke('decrypt_string', { data: account.password }) : account.password;
    const data = {
        guid: account.email,
        ...(account.isSteam ?
            {
                steamid: account.steamId,
                secret: pw
            } :
            {
                password: pw
            }
        ),
        clientToken: clientId,
        game_net: account.isSteam ? "Unity_steam" : "Unity",
        play_platform: account.isSteam ? "Unity_steam" : "Unity",
        game_net_user_id: account.isSteam ? account.steamId : "",
    };

    try {
        const response = await invoke('send_post_request_with_form_url_encoded_data', { url, data });
        return xmlToJson(response);
    } catch (error) {
        console.error(`Error: ${error}`);
        logToErrorLog('postAccountVerify', error);
        return error;
    }
}

async function postCharList(accessToken) {
    if (!accessToken) return null;

    const url = `${ROTMG_BASE_URL}/char/list`;
    const data = {
        do_login: "false",
        accessToken: accessToken,
        game_net: "Unity",
        play_platform: "Unity",
        game_net_user_id: "",
        muleDump: "true",
        __source: "ExaltAccountManager"
    };

    try {
        const response = await invoke('send_post_request_with_form_url_encoded_data', { url, data });
        return xmlToJson(response);
    } catch (error) {
        console.error(`Error: ${error}`);
        logToErrorLog('postCharList', error);
        return error;
    }
}

async function postRegisterAccount(account) {
    if (!account) return null;

    const url = `${ROTMG_BASE_URL}/account/register`;

    const data = {
        newGUID: account.email,
        newPassword: account.password,
        isAgeVerified: 'true',
        entrytag: '',
        signedUpKabamEmail: '0',
        name: account.name,
        game_net: 'Unity',
        play_platform: 'Unity',
        game_net_user_id: '',
        __source: "ExaltAccountManager"
    }

    try {
        return await invoke('send_post_request_with_form_url_encoded_data', { url, data })
            .catch(error => {
                console.error(`Error: ${error}`);
                logToErrorLog('postRegisterAccount', error);
                return 'EAM API error';
            });
    } catch (error) {
        console.error(`Error: ${error}`);
        logToErrorLog('postCharList', error);
        return null;
    }
}

async function getAppInit() {
    try {
        const response = await fetch(
            UPDATE_URLS(0),
            {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/x-www-form-urlencoded',
                },
            });

        if (!response.ok) {
            console.log(`HTTP error! status: ${response.status}-${response}`);
            return;
        }

        const appSettings = xmlToJson(await response.data);

        sessionStorage.setItem('buildHash', appSettings.BuildHash);
        sessionStorage.setItem('buildCDN', appSettings.BuildCDN ? appSettings.BuildCDN.replace('build-release/', '') : 'https://rotmg-build.decagames.com/');

        return appSettings;
    } catch (error) {
        console.log(error);
        logToErrorLog('getAppInit', error);
        return null;
    }
}

async function getGameFileList(buildHash) {
    if (!buildHash || buildHash === '' && sessionStorage.getItem('buildHash') !== null) {
        buildHash = sessionStorage.getItem('buildHash');
    }

    if (sessionStorage.getItem('buildCDN') === null) {
        await getAppInit();
    }
    const buildCDN = sessionStorage.getItem('buildCDN');

    if (!buildHash || !buildCDN) throw new Error('Build hash or CDN not found');

    try {
        const response = await fetch(
            `${buildCDN}build-release/${UPDATE_URLS(1, buildHash)}`,
            {
                method: 'GET',
            });

        return response.data.files;
    } catch (error) {
        console.log(error);
        logToErrorLog('getGameFileList', error);
        return null;
    }
}

export {
    postAccountVerify,
    postCharList,
    postRegisterAccount,
    getAppInit,
    getGameFileList,
};