import { xmlToJson } from '../utils/XmlUtils';
import { ROTMG_BASE_URL, UPDATE_URLS } from '../constants';
import { fetch, ResponseType } from '@tauri-apps/api/http';
import { invoke } from '@tauri-apps/api/tauri';

async function postAccountVerify(account, clientId) {
    if (!account || !clientId) return null;

    const url = `${ROTMG_BASE_URL}/account/verify`;
    const data = {
        guid: account.email,
        password: account.password,
        clientToken: clientId,
        game_net: "Unity",
        play_platform: "Unity",
        game_net_user_id: ""
    };

    try {
        const response = await invoke('send_post_request_with_form_url_encoded_data', { url, data });
        return xmlToJson(response);
    } catch (error) {
        console.error(`Error: ${error}`);
        return null;
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
        __source: "jakcodex-v965"
    };

    try {
        const response = await invoke('send_post_request_with_form_url_encoded_data', { url, data });
        return xmlToJson(response);
    } catch (error) {
        console.error(`Error: ${error}`);
        return null;
    }
}

async function getAppInit() {
    try {
        const response = await fetch(
            UPDATE_URLS(0),
            {
                method: 'POST',
                responseType: ResponseType.Text,
                headers: {
                    'Content-Type': 'application/x-www-form-urlencoded',
                    'Content-Length': '0'
                },
            });

        if (!response.ok) {
            console.log(`HTTP error! status: ${response.status}-${response}`);
            return;
        }

        return xmlToJson(await response.data);
    } catch (error) {
        console.log(error);
        return null;
    }
}

async function getGameFileList(buildHash) {
    try {
        const response = await fetch(
            UPDATE_URLS(1, buildHash),
            {
                method: 'GET',
                responseType: ResponseType.JSON
            });
        return response.data.files;
    } catch (error) {
        console.log(error);
        return null;
    }
}

export {
    postAccountVerify,
    postCharList,
    getAppInit,
    getGameFileList,
};