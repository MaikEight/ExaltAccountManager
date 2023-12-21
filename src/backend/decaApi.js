import axios from 'axios';
import { xmlToJson } from '../utils/XmlUtils';
import { UPDATE_URLS } from '../constants';
import { fetch, ResponseType } from '@tauri-apps/api/http';

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

    return await axios({
        method: 'post',
        url: `/rotmg/account/verify`,
        headers: {
            'Content-Type': 'application/x-www-form-urlencoded'
        },
        data: params.toString()
    })
        .then(function (response) {
            return xmlToJson(response.data);
        })
        .catch(function (error) {
            console.log(error);
        });
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

    return await axios({
        method: 'post',
        url: `/rotmg/char/list`,
        headers: {
            'Content-Type': 'application/x-www-form-urlencoded'
        },
        data: params.toString()
    })
        .then(function (response) {
            return xmlToJson(response.data);
        })
        .catch(function (error) {
            console.log(error);
        });
}

// async function getAppInit() {    
//     return await axios({
//         method: 'get',
//         url: UPDATE_URLS(0),
//     })
//         .then(function (response) {
//             console.log('getAppInit:response', response);
//             return xmlToJson(response.data);
//         })
//         .catch(function (error) {
//             console.log(error);
//         });
// }
async function getAppInit() {
    try {
        const response = await fetch(
            UPDATE_URLS(0),
            {
                method: 'GET',
                responseType: ResponseType.Text
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