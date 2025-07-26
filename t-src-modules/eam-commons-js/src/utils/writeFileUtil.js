import { writeFile as fsWriteFile } from '@tauri-apps/plugin-fs';
import { logToErrorLog } from './loggingUtils';

export async function writeFileUTF8(filePath, content, stringifyAsJSON = false) {
    try {
        const stringContent = stringifyAsJSON ? JSON.stringify(content) : content;
        const data = new TextEncoder().encode(stringContent);

        await fsWriteFile(
            filePath,
            data
        );
    } catch (error) {
        console.error('Error writing file (UTF8):', error);
        logToErrorLog('writeFileUTF8', `Error writing file (UTF8): ${error}`);
    }
}

export async function writeFile(filePath, content) {
    try {       
        let uint8ArrayContent = new Uint8Array(content);

        await fsWriteFile(
            filePath,
            uint8ArrayContent,
        );
    } catch (error) {
        console.error('Error writing file:', error);
        logToErrorLog('writeFile', `Error writing file: ${error}`);
    }
}