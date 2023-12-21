
export async function writeFileUTF8(filePath, content, stringifyAsJSON = false) {
    try {
        const stringContent = stringifyAsJSON ? JSON.stringify(content) : content;
        const bytes = new TextEncoder().encode(stringContent);

        await window.__TAURI__.fs.writeFile({
            path: filePath,
            contents: stringContent,
        });
    } catch (error) {
        console.error('Error writing file (UTF8):', error);
    }
}

export async function writeFile(filePath, content) {
    try {       
        let uint8ArrayContent = new Uint8Array(content);

        await window.__TAURI__.fs.writeBinaryFile({
            path: filePath,
            contents: uint8ArrayContent,
        });
    } catch (error) {
        console.error('Error writing file:', error);
    }
}