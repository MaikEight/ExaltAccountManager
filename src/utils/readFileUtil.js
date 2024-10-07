import { logToErrorLog } from "eam-commons-js";

async function readFileUTF8(filePath, parseAsJSON = false) {
    try {        
        if (window.__TAURI__.fs.exists(filePath)) {
            const bytes = await window.__TAURI__.fs.readBinaryFile(filePath);
            const stringContent = new TextDecoder().decode(bytes);
            return parseAsJSON ? JSON.parse(stringContent) : stringContent;
        }
        console.warn("File does not exist:", filePath);
        logToErrorLog('readFileUTF8', `File does not exist: ${filePath}`);
    } catch (error) {
        if(error.includes('os error 2')) return;

        console.error('Error reading file:',filePath, error);
        logToErrorLog('readFileUTF8', `Error reading file: ${filePath} - ${error}`);
    }
    return null;
};

export { readFileUTF8 };