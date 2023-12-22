import { xmlToJson } from '../utils/XmlUtils';
import { UPDATE_URLS } from '../constants';
import { fetch, ResponseType, Body } from '@tauri-apps/api/http';
import { he } from 'date-fns/locale';

const ROTMG_BASE_URL = 'https://www.realmofthemadgod.com';
async function postAccountVerify(account, clientId) {
    if (!account || !clientId) return null;

    const values = {
        guid: account.email,
        password: account.password,
        clientToken: clientId,
        game_net: "Unity",
        play_platform: "Unity",
        game_net_user_id: ""
    };

    const params = new URLSearchParams();
    for (const key in values) {
        params.append(key, values[key]);
    }

    // try {
    //     const response = await fetch(
    //         `${ROTMG_BASE_URL}/rotmg/account/verify`,
    //         {
    //             method: 'POST',
    //             body: params,
    //             responseType: ResponseType.Text,
    //             headers: {
    //                 'Content-Type': 'application/x-www-form-urlencoded'
    //             },
    //         });

    //     if (!response.ok) {
    //         console.log(`HTTP error! status: ${response.status}-${response}`);
    //         return null;
    //     }

    //     return xmlToJson(await response.data);
    // } catch (error) {
    //     console.log(error);
    //     return null;
    // }
    const body = params.toString();
    console.log('Fetching...')
    const response = await fetch(
        `${ROTMG_BASE_URL}/rotmg/account/verify`,
        {
            method: 'POST',
            body: Body.text(body), 
            responseType: ResponseType.Text,
            headers: {
                'Content-Type': 'application/x-www-form-urlencoded',
                'Content-Length': body.length.toString()
            },
        });
    console.log('Fetched!');
    if (!response.ok) {
        console.log(`HTTP error! status: ${response.status}-${response}`);
        return null;
    }

    const data = xmlToJson(await response.data);
    console.log(data);
    return data;
}

async function postCharList(accessToken) {
    if (!accessToken) return null;

    const values = {
        do_login: "false",
        accessToken: accessToken,
        game_net: "Unity",
        play_platform: "Unity",
        game_net_user_id: "",
        muleDump: "true",
        __source: "jakcodex-v965"
    };

    const params = new URLSearchParams();
    for (const key in values) {
        params.append(key, values[key]);
    }

    try {
        const response = await fetch(
            `${ROTMG_BASE_URL}/rotmg/char/list`,
            {
                method: 'POST',
                body: params.toString(),
                responseType: ResponseType.Text,
                headers: {
                    'Content-Type': 'application/x-www-form-urlencoded'
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