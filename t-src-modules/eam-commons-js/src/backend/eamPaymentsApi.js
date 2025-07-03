import { invoke } from "@tauri-apps/api/core";
import { logToErrorLog } from "../utils/loggingUtils";

async function getEamPlusPrices() {
    const requestHeaders = new Map();
    requestHeaders.set("Origin", "https://exaltaccountmanager.com");
    requestHeaders.set("X-CLIENT-APPLICATION", "ExaltAccountManager");

    try {
        const response = await invoke('send_get_request', {
            url: 'https://payments.exaltaccountmanager.com/eam-plus-prices',
            customHeaders: requestHeaders
        })
        .catch(error => {
            console.error(`Error: ${error}`);
            logToErrorLog('getEamPlusPrices', error);
            return { error: 'EAM Payments-API error' };
        });

        if (sessionStorage.getItem('flag:debug') === 'true') {
            console.log('getEamPlusPrices response', response);
        }

        if (sessionStorage.getItem('flag:copy:eam-plus-prices') === 'true') {
            navigator.clipboard.writeText(JSON.stringify(response, null, 2))
                .then(() => console.log('getEamPlusPrices response copied to clipboard'))
                .catch(err => console.error('Failed to copy getEamPlusPrices response: ', err));
        }

        return response || null;
    } catch (error) {
        console.error(`Error: ${error}`);
        logToErrorLog('getEamPlusPrices', error);
        return { error: 'EAM Payments-API error' };
    }
}

export { getEamPlusPrices };