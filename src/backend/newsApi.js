import { APP_VERSION, EAM_NEWS_BASE_URL } from "../constants";
import { fetch } from "@tauri-apps/plugin-http";

async function getLatestPopup(lastSeenPopupId) {
    const url = `${EAM_NEWS_BASE_URL}popups${lastSeenPopupId ? `?id=${lastSeenPopupId}` : ""}`;
    const headers = {
        'eam-application-id': 'ExaltAccountManager',
        'eam-application-version': APP_VERSION || '0.0.0'
    };

    //Returns either the latest popup or 204 when no new popup is available
    const response = await fetch(url, {
        method: 'GET',
        headers,
    })
        .catch(error => {
            console.error("Error fetching latest popup:", error);
            return {
                error: error.message || "Unknown error",
                status: error.status || 500
            };
        });

    if (response && response.status === 204) {
        return null;
    }

    if (response && response.status !== 200) {
        return {
            error: `Failed to fetch latest popup. Status code: ${response.status}`,
            status: response.status
        };
    }

    const data = await response.json();
    
    if(data.createdAt) {
        data.timestamp = new Date(data.createdAt);
    }

    return data;
}

export {
    getLatestPopup
};
