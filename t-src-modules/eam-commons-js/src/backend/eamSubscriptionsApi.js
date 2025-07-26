import { invoke } from "@tauri-apps/api/core";
import { EAM_SUBSCRIPTIONS_API } from "../../constants";

async function postPlusToken(id_token, deviceId) {
    const debugFlag = sessionStorage.getItem('flag:debug') === 'true';
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

        if (debugFlag) {
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

    if (debugFlag) {
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
        if (debugFlag) {
            console.log('Plus token validation response:', response);
        }
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

/**
 * Checks if the user has a paid subscription.
 * @param {*} jwt 
 * @returns {Promise<Object|null>} Returns the subscription data if available, otherwise null.
 * Returned object structure:
 * {
 *  "hasValidSubscription": true,
 *  "subscriptionStatus": "active",
 *  "source": "auth0_metadata"
 * }
 */
async function checkForPaidSubscription(jwt) {
    const debugFlag = sessionStorage.getItem('flag:debug') === 'true';
    if (debugFlag) {
        console.log('Checking for paid subscription');
    }
    const myHeaders = new Map();
    myHeaders.set('Authorization', `Bearer ${jwt}`);

    const response = await invoke('send_post_request_with_json_body',
        {
            url: `${EAM_SUBSCRIPTIONS_API}/check-for-paid-subscription`,
            data: "",
            headersOpt: myHeaders
        })
        .catch(error => {
            console.error('Error checking for paid subscription:', error);
            return { error: true, message: error };
        });

    if (!response) {
        console.error('No response received from paid subscription check');
        return null;
    }

    if (response.error) {
        console.error('Error in paid subscription check response:', response.message);
        return null;
    }

    try {
        const data = JSON.parse(response);
        if (debugFlag) {
            console.log('Paid subscription check response:', data);
        }
        return data;
    } catch (error) {
        console.error('Error parsing paid subscription check response:', error, 'response:', response);
        return null;
    }
}

export { postPlusToken, validatePlusToken, checkForPaidSubscription };