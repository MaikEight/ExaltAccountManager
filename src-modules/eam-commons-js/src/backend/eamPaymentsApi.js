import { invoke } from "@tauri-apps/api/core";
import { logToErrorLog } from "../utils/loggingUtils";

async function getEamPlusPrices() {
    const requestHeaders = new Map();
    requestHeaders.set("Origin", "https://exaltaccountmanager.com");
    requestHeaders.set("X-CLIENT-APPLICATION", "ExaltAccountManager");

    try {
        return await invoke('send_get_request', {
            url: 'https://payments.exaltaccountmanager.com/eam-plus-prices',
            customHeaders: requestHeaders
        })
        .catch(error => {
            console.error(`Error: ${error}`);
            logToErrorLog('getEamPlusPrices', error);
            return { error: 'EAM Payments-API error' };
        });
    } catch (error) {
        console.error(`Error: ${error}`);
        logToErrorLog('getEamPlusPrices', error);
        return { error: 'EAM Payments-API error' };
    }
}

export { getEamPlusPrices };