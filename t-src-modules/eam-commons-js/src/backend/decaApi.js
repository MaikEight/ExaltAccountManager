import { fetch } from '@tauri-apps/plugin-http';
import { invoke } from '@tauri-apps/api/core';
import { logToErrorLog } from '../utils/loggingUtils';
import { ROTMG_BASE_URL, UPDATE_URLS } from '../../constants';
import { xmlToJson } from '../utils/XmlUtils';

/**
 * @deprecated Use AccountsContext.sendAccountVerify() which calls Rust-side 
 * implementation via 'send_account_verify_request_for_account' Tauri command.
 * This function is kept for backward compatibility but should not be used in new code.
 * 
 * @param {Object} account - The account object with email and password
 * @param {string} clientId - The HWID/client token
 * @param {boolean} decryptNeeded - Whether password decryption is needed
 * @returns {Promise<Object|null>} The account verify response or null
 */
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
        const jsonResponse = xmlToJson(response);

        if (sessionStorage.getItem('flag:debug') === 'true') {
            console.log('postAccountVerify response', jsonResponse);
        }

        if (sessionStorage.getItem('flag:copy:account/verify') === 'true') {
            navigator.clipboard.writeText(JSON.stringify(jsonResponse, null, 2))
                .then(() => console.log('postAccountVerify response copied to clipboard'))
                .catch(err => console.error('Failed to copy postAccountVerify response: ', err));
        }

        return jsonResponse || null;
    } catch (error) {
        console.error(`Error: ${error}`);
        logToErrorLog('postAccountVerify', error);
        return error;
    }
}

/**
 * @deprecated Use AccountsContext.sendCharList() which calls Rust-side 
 * implementation via 'send_char_list_request_for_account' Tauri command.
 * This function is kept for backward compatibility but should not be used in new code.
 * The Rust implementation also handles server list storage in the database.
 * 
 * @param {string} accessToken - The access token from account/verify
 * @returns {Promise<Object|null>} The char list response or null
 */
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
        const jsonResponse = xmlToJson(response);

        if (sessionStorage.getItem('flag:debug') === 'true') {
            console.log('postCharList response', jsonResponse);
        }

        if (sessionStorage.getItem('flag:copy:char/list') === 'true') {
            navigator.clipboard.writeText(JSON.stringify(jsonResponse, null, 2))
                .then(() => console.log('postCharList response copied to clipboard'))
                .catch(err => console.error('Failed to copy postCharList response: ', err));
        }

        return jsonResponse || null;
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
        const response = await invoke('send_post_request_with_form_url_encoded_data', { url, data })
            .catch(error => {
                console.error(`Error: ${error}`);
                logToErrorLog('postRegisterAccount', error);
                return 'EAM API error';
            });

        if (sessionStorage.getItem('flag:debug') === 'true') {
            console.log('postRegisterAccount response', response);
        }

        if (sessionStorage.getItem('flag:copy:account/register') === 'true') {
            navigator.clipboard.writeText(JSON.stringify(response, null, 2))
                .then(() => console.log('postRegisterAccount response copied to clipboard'))
                .catch(err => console.error('Failed to copy postRegisterAccount response: ', err));
        }

        return response || null;
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

        if (sessionStorage.getItem('flag:debug') === 'true') {
            console.log('getAppInit response', appSettings);
        }

        if (sessionStorage.getItem('flag:copy:app-init') === 'true') {
            navigator.clipboard.writeText(JSON.stringify(appSettings, null, 2))
                .then(() => console.log('getAppInit response copied to clipboard'))
                .catch(err => console.error('Failed to copy getAppInit response: ', err));
        }

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

        const files = response.data.files;

        if( sessionStorage.getItem('flag:debug') === 'true') {
            console.log('getGameFileList response', files);
        }

        if (sessionStorage.getItem('flag:copy:game-file-list') === 'true') {
            navigator.clipboard.writeText(JSON.stringify(files, null, 2))
                .then(() => console.log('getGameFileList response copied to clipboard'))
                .catch(err => console.error('Failed to copy getGameFileList response: ', err));
        }

        return files;
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