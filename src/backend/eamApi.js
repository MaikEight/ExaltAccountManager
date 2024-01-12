import { fetch } from "@tauri-apps/api/http";
import { EAM_BASE_URL } from "../constants";

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

export { getLatestEamVersion };