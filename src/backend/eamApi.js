import { fetch } from "@tauri-apps/api/http";
import { EAM_BASE_URL } from "../constants";
import { invoke } from "@tauri-apps/api/tauri";

async function getLatestEamVersion() {
    return await fetch(`${EAM_BASE_URL}v1/ExaltAccountManager/version`,
        {
            method: 'GET',
            headers: {
                'Accept': 'application/json',
                'User-Agent': 'ExaltAccountManager'
            }
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

    const response = await invoke('send_post_request_with_json_body', { url, data });
    return response ? JSON.parse(response) : "";
}

async function heartBeat() {
    const sessionId = sessionStorage.getItem('sessionId');
    if (sessionId === null || sessionId === undefined) {
        return;
    }

    const url = `${EAM_BASE_URL}v1/Analytics/heartbeat`;
    const data = `"${sessionId}"`;

    try {
        const response = await invoke('send_patch_request_with_json_body', { url, data });
        return response ? JSON.parse(response) : "";
    } catch (error) {
        console.error(`Error: ${error}`);
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
        .then(response => response.data);
}

export {
    getLatestEamVersion,
    startSession,
    heartBeat,
    endSession
};