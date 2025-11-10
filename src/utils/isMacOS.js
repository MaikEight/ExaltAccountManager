import { invoke } from '@tauri-apps/api/core';

function isMacOS() {
    const isMacOS = localStorage.getItem('isMacOs');
    if (isMacOS === 'true') {
        return true;
    } 

    if (isMacOS === 'false') {
        return false;
    }

    if (sessionStorage.getItem('checkedIsMacOs') === 'true') {
        return false;
    }

    return invoke('get_current_os')
        .then((os) => {
            sessionStorage.setItem('checkedIsMacOs', 'true');

        localStorage.setItem("isMacOs", (os === "macos").toString());

            if (os === "macos") {
                return true;
            }

            return false;
        })
        .catch(console.error);
}

export default isMacOS;