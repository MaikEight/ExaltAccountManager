import { getLatestEamVersion } from "../backend/eamApi";
import { isUpdateAvailable } from "../constants";
import { appWindow, PhysicalSize } from '@tauri-apps/api/window';
import { getAPIClientIdHash } from "./testUtils";
import { invoke } from '@tauri-apps/api/tauri';
import { checkForUpdates } from "./realmUpdaterUtils";

async function onStartUp(gameExePath) {
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

    if (gameExePath !== null && gameExePath !== undefined) {
        const lastUpdateCheck = localStorage.getItem("lastUpdateCheck"); //Format: 30.01.2024 01:39:47
        const lastUpdateCheckDate = new Date(lastUpdateCheck);
        const currentDate = new Date();
        const diff = Math.abs(currentDate - lastUpdateCheckDate);
        const daysSinceLastUpdateCheck = Math.ceil(diff / (1000 * 60 * 60 * 24));
        if (daysSinceLastUpdateCheck >= 1) {
            console.log("Checking for updates on startup");
            checkForUpdates(gameExePath);
        }
    }
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