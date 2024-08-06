import { fetch } from "@tauri-apps/api/http";
import { EAM_BASE_URL } from "../constants";
import { invoke } from "@tauri-apps/api/tauri";
import { Body } from "@tauri-apps/api/http"
import { logToErrorLog } from "../utils/loggingUtils";

async function getLatestEamVersion() {
    return await fetch(`${EAM_BASE_URL}v1/ExaltAccountManager/version`,
        {
            method: 'GET',
            headers: {
                'Accept': 'application/json',
                'User-Agent': 'ExaltAccountManager'
            }
        })
        .catch(error => {
            console.error(`Error: ${error}`);
            logToErrorLog('getLatestEamVersion', error);
            return null;
        })
        .then(response => response.data);
}

async function startSession(amountOfAccounts, clientIdHash, clientVersion) {
    if (clientIdHash === null || clientIdHash === undefined) {
        console.log("HWID is null or undefined, not starting session");
        return "";
    }

    const url = `${EAM_BASE_URL}v1/Analytics/session/start`;

    const data = JSON.stringify({
        ClientIdHash: clientIdHash,
        ClientVersion: clientVersion,
        AmountOfAccounts: amountOfAccounts,
    });

    const response = await invoke('send_post_request_with_json_body', { url, data })
        .catch(error => { logToErrorLog('startSession', error); });
    return response ? JSON.parse(response) : "";
}

async function heartBeat() {
    const sessionId = sessionStorage.getItem('sessionId');
    if (sessionId === null || sessionId === undefined) {
        return null;
    }

    const url = `${EAM_BASE_URL}v1/Analytics/heartbeat`;
    const data = `"${sessionId}"`;

    try {
        const response = await invoke('send_patch_request_with_json_body', { url, data });
        return response ? JSON.parse(response) : "";
    } catch (error) {
        logToErrorLog('heartBeat', error);
        return null;
    }
}

async function endSession(sessionId) {
    if (sessionId === null || sessionId === undefined) {
        console.log("SessionId is null or undefined, not ending session");
        return;
    }

    const data = {
        SessionId: sessionId
    }

    return await fetch(`${EAM_BASE_URL}v1/Analytics/session/end`,
        {
            method: 'POST',
            headers: {
                'Accept': 'application/json',
                'User-Agent': 'ExaltAccountManager',
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(data)
        })
        .catch(error => { logToErrorLog('endSession', error); })
        .then(response => response.data);
}

async function sendFeedback(feedback) {
    if (feedback === null || feedback === undefined) {
        console.log("Feedback is null or undefined, not sending feedback");
        return;
    }

    return await fetch(`${EAM_BASE_URL}v1/ExaltAccountManager/Feedback`,
        {
            method: 'POST',
            headers: {
                'Accept': 'application/json',
                'User-Agent': 'ExaltAccountManager',
                'Content-Type': 'application/json'
            },
            body: Body.json(feedback)
        })
        .catch(error => { logToErrorLog('sendFeedback', error); })
        .then(response => response.data);
}

export {
    getLatestEamVersion,
    startSession,
    heartBeat,
    endSession,
    sendFeedback
};