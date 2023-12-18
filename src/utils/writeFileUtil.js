
export async function writeFileUTF8(filePath, content, stringifyAsJSON = false) {
    try {
        const stringContent = stringifyAsJSON ? JSON.stringify(content) : content;
        const bytes = new TextEncoder().encode(stringContent);

        await window.__TAURI__.fs.writeFile({
            path: filePath,
            contents: stringContent,
        });
    } catch (error) {
        console.error('Error writing file:', error);
    }
}