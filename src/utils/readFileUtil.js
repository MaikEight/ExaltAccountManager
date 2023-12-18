
async function readFileUTF8(filePath, parseAsJSON = false) {
    try {
        const bytes = await window.__TAURI__.fs.readBinaryFile(filePath);
        const stringContent = new TextDecoder().decode(bytes);
        return parseAsJSON ? JSON.parse(stringContent) : stringContent;
    } catch (error) {
        console.error('Error reading file:', error);
    }
};

export { readFileUTF8 };