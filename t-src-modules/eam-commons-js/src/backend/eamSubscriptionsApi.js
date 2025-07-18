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
            return { error: true, message: error };
        });

    if (!response) {
        console.error('No response received from Plus token API');
        return null;
    }

    if (response.error) {
        console.error('Error in Plus token response:', response.message);
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

async function validatePlusToken(jwt, deviceId) {
    const debugFlag = sessionStorage.getItem('flag:debug') === 'true';
    const myHeaders = new Map();
    myHeaders.set('Authorization', `Bearer ${jwt}`);

    if(debugFlag) {
        console.log('Validating Plus token', jwt, deviceId);
    }

    const body = {
        "deviceId": deviceId
    };

    const response = await invoke('send_get_request_with_json_body', { url: `${EAM_SUBSCRIPTIONS_API}/plus/token/validation`, data: JSON.stringify(body), customHeaders: myHeaders })
        .catch(error => {
            console.error('Error validating Plus token:', error);
            if (debugFlag) {
                console.error(error);
            }
            return { error: true, message: error };
        });

    if (!response) {
        console.error('No response received from Plus token validation API');
        return null;
    }

    if (response.error) {
        console.error('Error in Plus token response:', response.message);
        return null;
    }

    try {
        const data = JSON.parse(response);

        if (debugFlag) {
            console.log('Plus token validation response:', data);
        }

        return data;
    } catch (error) {
        console.error('Error parsing Plus token validation response:', error);
        return null;
    }
}

export { postPlusToken, validatePlusToken };