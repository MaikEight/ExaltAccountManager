import { getLatestEamVersion, startSession } from "../backend/eamApi";
import { isUpdateAvailable } from "../constants";
import { appWindow, PhysicalSize } from '@tauri-apps/api/window';
import { getAPIClientIdHash } from "./testUtils";
import { invoke } from '@tauri-apps/api/tauri';

async function onStartUp() {
    appWindow.setMinSize(new PhysicalSize(850, 600));

    getLatestEamVersion()
        .then((version) => {
            if (isUpdateAvailable(version)) {
                localStorage.setItem("EAMUpdateAvailable", true);
            }
            else if (localStorage.getItem("EAMUpdateAvailable")) {
                localStorage.removeItem("EAMUpdateAvailable");
            }
        });
}

function setApiHwidHash(hwid) {

    if (hwid === null || hwid === undefined) {
        console.log("HWID is null or undefined, not setting apiHwidHash");
        return;
    }

    const calculcateApiHwidHash = async () => {
        const hash = await getAPIClientIdHash();
        while (hwid === null || hwid === undefined) {
            await new Promise(resolve => setTimeout(resolve, 100));
        }
        const compSecret = hwid + hash;
        const hwidHash = await invoke('quick_hash', { secret: compSecret });

        const stored = localStorage.getItem("apiHwidHash");
        if (hwidHash !== null && (stored === null || stored !== hwidHash)) {
            localStorage.setItem("apiHwidHash", hwidHash);
        }
    }
    calculcateApiHwidHash();
}

export { onStartUp, setApiHwidHash };