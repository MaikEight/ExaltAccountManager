
async function readFileUTF8(filePath, parseAsJSON = false) {
    try {        
        if (window.__TAURI__.fs.exists(filePath)) {
            const bytes = await window.__TAURI__.fs.readBinaryFile(filePath);
            const stringContent = new TextDecoder().decode(bytes);
            return parseAsJSON ? JSON.parse(stringContent) : stringContent;
        }
        console.log("File does not exist:", filePath);
        return null;
    } catch (error) {
        if(error.includes('os error 2')) return;

        console.error('Error reading file:',filePath, error);
    }
};

export { readFileUTF8 };