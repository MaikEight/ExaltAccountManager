import { fetch } from "@tauri-apps/api/http";
import { logToErrorLog } from "eam-commons-js";

async function getGitHubStars() {
    if (sessionStorage.getItem('githubStars')) {
        return sessionStorage.getItem('githubStars');
    }

    return await fetch('https://api.github.com/repos/MaikEight/ExaltAccountManager',
        {
            method: 'GET',
            headers: {
                'Accept': 'application/vnd.github.v3+json',
                'User-Agent': 'ExaltAccountManager'
            }
        })
        .then(response => {
            if (response.status === 200) {
                sessionStorage.setItem('githubStars', response.data.stargazers_count);   
                return response.data.stargazers_count
            }
        })
        .catch(error => logToErrorLog('getGitHubStars', error));
}

export { getGitHubStars };