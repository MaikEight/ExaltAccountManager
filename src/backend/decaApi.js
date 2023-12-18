import axios from 'axios';
import { xmlToJson } from '../utils/XmlUtils';

// const BASE_URL = 'https://www.realmofthemadgod.com';

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
        url: `/api/account/verify`,
        headers: {
            'Content-Type': 'application/x-www-form-urlencoded'
        },
        data: params.toString()
    })
        .then(function (response) {
            const xml = response.data;
            const parser = new DOMParser();
            const xmlDoc = parser.parseFromString(xml, "text/xml");

            // Convert XML Document to JavaScript object
            const result = xmlToJson(xmlDoc);
            console.log(result);
            return result;
        })
        .catch(function (error) {
            console.log(error);
        });
}

/*
    values = new Dictionary<string, string>
                    {
                        { "do_login", "false" },
                        { "accessToken", accessToken.token },
                        { "game_net", "Unity" },
                        { "play_platform", "Unity" },
                        { "game_net_user_id", "" },
                        { "muleDump", "true" },
                        { "__source", "jakcodex-v965" }
                    };
                    content = new FormUrlEncodedContent(values);
                    content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");

                    response = SendPostRequest("https://www.realmofthemadgod.com/char/list", content, "form");
*/

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
        url: `/api/char/list`,
        headers: {
            'Content-Type': 'application/x-www-form-urlencoded'
        },
        data: params.toString()
    })
        .then(function (response) {
            const xml = response.data;
            const parser = new DOMParser();
            const xmlDoc = parser.parseFromString(xml, "text/xml");

            // Convert XML Document to JavaScript object
            const result = xmlToJson(xmlDoc);
            console.log(result);
            return result;
        })
        .catch(function (error) {
            console.log(error);
        });
}

export { postAccountVerify, postCharList };