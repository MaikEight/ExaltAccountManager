
async function readFileUTF8(filePath, parseAsJSON = false) {
    try {
        const bytes = await window.__TAURI__.fs.readBinaryFile(filePath);
        const stringContent = new TextDecoder().decode(bytes);
        if (!parseAsJSON) {
            return stringContent;
        }
        return JSON.parse(stringContent);
    } catch (error) {
        console.error('Error reading file:', error);
    }
};

export { readFileUTF8 };