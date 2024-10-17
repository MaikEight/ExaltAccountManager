import { fetch } from "@tauri-apps/plugin-http";
import { logToErrorLog } from "eam-commons-js";

async function getGitHubStars() {
    if (sessionStorage.getItem('githubStars')) {
        return sessionStorage.getItem('githubStars');
    }

    const response = await fetch('https://api.github.com/repos/MaikEight/ExaltAccountManager',
        {
            method: 'GET',
            headers: {
                'Accept': 'application/vnd.github.v3+json',
                'User-Agent': 'ExaltAccountManager'
            }
        })
        .catch (error => logToErrorLog('getGitHubStars', error));

    if (response.status === 200) {
        const body = await response.json();
        console.log('body', body);
        const stars = body.stargazers_count;
        sessionStorage.setItem('githubStars', stars);
        return stars;
    }
}

export { getGitHubStars };