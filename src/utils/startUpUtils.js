import { getLatestEamVersion } from "../backend/eamApi";
import { isUpdateAvailable } from "../constants";
import { appWindow, PhysicalSize } from '@tauri-apps/api/window';

async function onStartUp() {    
    appWindow.setMinSize(new PhysicalSize(850, 600));

    getLatestEamVersion()
        .then((version) => {
            if (isUpdateAvailable(version)) {
                console.log("Update available!");
                localStorage.setItem("EAMUpdateAvailable", true);
            }
            else if (localStorage.getItem("EAMUpdateAvailable")) {
                localStorage.removeItem("EAMUpdateAvailable");
            }
        });
}

export { onStartUp };