import { getLatestEamVersion } from "../backend/eamApi";
import { isUpdateAvailable } from "../constants";
import { appWindow, PhysicalSize } from '@tauri-apps/api/window';
import { invoke } from '@tauri-apps/api/tauri';
import { checkForUpdates } from "./realmUpdaterUtils";
import { checkUpdate, installUpdate } from '@tauri-apps/api/updater';
import { logToErrorLog } from "./loggingUtils";

async function performCheckForUpdates() {
    try {
        const update = await checkUpdate();
        if (update.shouldUpdate) {
            console.log(`Installing update ${update.manifest?.version}, ${update.manifest?.date}, ${update.manifest.body}`);
            await installUpdate();
        }
    }
    catch (e) {
        console.error(e);
        logToErrorLog("performCheckForUpdates", e);
    }
}

async function onStartUp() {
    appWindow.setMinSize(new PhysicalSize(850, 600));
    writeStartupLogoToConsole();
    addConsoleLogListener();

    //Check for EAM update
    performCheckForUpdates();
    getLatestEamVersion()
        .then((version) => {
            if (isUpdateAvailable(version)) {
                localStorage.setItem("EAMUpdateAvailable", true);
                performCheckForUpdates();
            }
            else if (localStorage.getItem("EAMUpdateAvailable")) {
                localStorage.removeItem("EAMUpdateAvailable");
            }
        });

    if (localStorage.getItem("lastUpdateCheck") === null) {
        checkForUpdates(false);
        return;
    }

    // Check for game updates every 24 hours
    const lastUpdateCheck = localStorage.getItem("lastUpdateCheck");
    const [day, month, yearTime] = lastUpdateCheck.split(".");
    const [year, time] = yearTime.split(" ");
    const lastUpdateCheckDate = new Date(`${month}.${day}.${year} ${time}`);
    const currentDate = new Date();
    const diff = Math.abs(currentDate - lastUpdateCheckDate);
    const daysSinceLastUpdateCheck = Math.ceil(diff / (1000 * 60 * 60 * 24));

    if (daysSinceLastUpdateCheck > 1) {
        checkForUpdates(false);
    }

    //Delete old Error logs
    invoke('delete_from_error_log', { days: 7 })
        .then((res) => {
            if (res > 0) {
                console.log(`Deleted ${res} old error logs`);
            }
        })
        .catch((e) => {
            console.error(e);
            logToErrorLog("delete_from_error_log", e);
        });
}

function writeStartupLogoToConsole() {
    console.log('');
    console.log('%c _______     ___      .___  ___. \n|   ____|   /   \\     |   \\/   |\n|  |__     /  ^  \\    |  \\  /  |\n|   __|   /  /_\\  \\   |  |\\/|  |\n|  |____ /  _____  \\  |  |  |  |\n|_______/__/     \\__\\ |__|  |__|\n\n                           by MaikEight', 'color: #9155FD;');
    console.log('');

}

function addConsoleLogListener() {
    // Override console methods to log to error log
    ['warn', 'error'].forEach((methodName) => {
        const oldMethod = console[methodName];
        console[methodName] = (...args) => {
            let logSource = "";
            try {
                const originalCallStack = new Error().stack.split('\n');
                const originalCallSource = originalCallStack[2];
                const fileNameAndLine = originalCallSource.split('/').pop().split(':');
                fileNameAndLine[2] = fileNameAndLine[2].substring(0, fileNameAndLine[2].length - 1);
                fileNameAndLine[0] = (fileNameAndLine[0].substring(0, fileNameAndLine[0].lastIndexOf('?')));
                logSource = `${fileNameAndLine[0]},${fileNameAndLine[1]}:${fileNameAndLine[2]}`;
                oldMethod.call(originalCallSource, ...args);
            } catch (e) {
                oldMethod.call(console, ...args);
                console.log(e);
            }
            try {

                if (logSource.startsWith("MainRouter") && args.length > 0 && args[0].startsWith("ErrorBoundary")) {
                    logToErrorLog(logSource, JSON.stringify(args, null, 2));
                    return;
                }
            } catch (e) {
                console.log(e);
            }

            logToErrorLog(logSource, args);
        };
    });
}

function setApiHwidHash(hwid) {

    if (hwid === null || hwid === undefined) {
        console.log("HWID is null or undefined, not setting apiHwidHash");
        return;
    }

    const calculcateApiHwidHash = async () => {
        const hash = await invoke('get_os_user_identity');
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