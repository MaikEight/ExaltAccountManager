import { invoke } from "@tauri-apps/api/core";
import { EAM_SUBSCRIPTIONS_API } from "../../constants";

async function postPlusToken(id_token, deviceId) {
    const myHeaders = new Map();
    myHeaders.set('Authorization', `Bearer ${id_token}`);

    const body = {
        "deviceId": deviceId
    }

    const response = await invoke('send_post_request_with_json_body', { url: `${EAM_SUBSCRIPTIONS_API}/plus/token`, data: JSON.stringify(body), headersOpt: myHeaders })
        .catch(error => {
            console.error('Error posting Plus token:', error);
            throw error;
        });

    if (!response) {
        console.error('No response received from Plus token API');
        return null;
    }

    try {
        const data = JSON.parse(response);

        if (sessionStorage.getItem('flag:debug')) {
            console.log('Plus token response:', data);
        }

        return data;
    } catch (error) {
        console.error('Error parsing Plus token response:', error);
        return null;
    }

}

export { postPlusToken };