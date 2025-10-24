import { invoke } from '@tauri-apps/api/core';

function isMacOS() {
    const isMacOS = localStorage.getItem('isMacOs');
    if (isMacOS === 'true') {
        return true;
    }

    if (sessionStorage.getItem('checkedIsMacOs') === 'true') {
        return false;
    }

    return invoke('get_current_os')
        .then((os) => {
            sessionStorage.setItem('checkedIsMacOs', 'true');

            if (os === "macos") {
                localStorage.setItem("isMacOs", "true");
                return true;
            }

            return false;
        })
        .catch(console.error);
}

export default isMacOS;