import { logToErrorLog } from "./loggingUtils";
import { exists, readFile as tauriReadFile, BaseDirectory } from '@tauri-apps/plugin-fs';

async function readFileUTF8(filePath, parseAsJSON = false) {
    try {
        if (await exists(filePath, { dir: BaseDirectory.AppData })) {
            const bytes = await tauriReadFile(filePath, { dir: BaseDirectory.AppData });
            const stringContent = new TextDecoder().decode(bytes);
            return parseAsJSON ? JSON.parse(stringContent) : stringContent;
        }        
    } catch (error) {
        if ((error).includes('os error 2')) return;

        console.error('Error reading file:', filePath, error);
        logToErrorLog('readFileUTF8', `Error reading file: ${filePath} - ${error}`);
    }
    return null;
};

export { readFileUTF8 };