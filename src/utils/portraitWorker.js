// portraitWorker.js - Web Worker for loading and processing sprite sheets
// This worker uses OffscreenCanvas to process images off the main thread

// Import the sheet data
// Note: We'll need to receive this data via postMessage since workers can't import ES modules directly
let skinsheets = {};
let textiles = {};

// Extract sprites using OffscreenCanvas - EXACT same logic as main thread
function extract_sprites(imageBitmap, sx, sy) {
    sx = sx || 8;
    sy = sy || sx;
    const canvas = new OffscreenCanvas(imageBitmap.width, imageBitmap.height);
    const ctx = canvas.getContext('2d', { willReadFrequently: true });
    ctx.drawImage(imageBitmap, 0, 0);
    
    let i = 0;
    const r = [];
    for (let y = 0; y < canvas.height; y += sy) {
        for (let x = 0; x < canvas.width; x += sx, i++) {
            r[i] = ctx.getImageData(x, y, sx, sy);
        }
    }
    return r;
}

// Extract skins using OffscreenCanvas - EXACT same logic as main thread
function extract_skins(imageBitmap, size) {
    size = size || 8;
    const canvas = new OffscreenCanvas(imageBitmap.width, imageBitmap.height);
    const ctx = canvas.getContext('2d', { willReadFrequently: true });
    ctx.drawImage(imageBitmap, 0, 0);
    
    let i = 0;
    const r = [];
    for (let y = 0; y < canvas.height; y += size * 3, i++) {
        r[i] = ctx.getImageData(0, y, size, size);
    }
    return r;
}

// Load image using fetch and createImageBitmap (worker-compatible)
async function load_img(src, key) {
    try {
        const response = await fetch(src);
        const blob = await response.blob();
        const imageBitmap = await createImageBitmap(blob);
        return { imageBitmap, key };
    } catch (error) {
        console.error(`Failed to load ${key} from ${src}:`, error);
        throw error;
    }
}

// Main function to load all sheets
async function load_sheets() {    
    const sprites = {};
    
    // Load skinsheets
    const skinsheetPromises = Object.keys(skinsheets).map(async (key) => {
        try {
            const src = skinsheets[key];
            const { imageBitmap } = await load_img(src, key);
            const size = key.includes('16') ? 16 : 8;
            const extracted = extract_skins(imageBitmap, size);
            
            // Store both the regular sprites and create mask key
            sprites[key] = extracted;
            
            // Close the bitmap to free memory
            imageBitmap.close();
            
            return { key, success: true };
        } catch (e) {
            console.error(`[Worker] Failed to load skinsheet ${key}:`, e);
            return { key, success: false };
        }
    });
    
    // Load textiles
    const textilePromises = Object.keys(textiles).map(async (key) => {
        try {
            const src = textiles[key];
            const { imageBitmap } = await load_img(src, key);
            const extracted = extract_sprites(imageBitmap, +key);
            
            sprites[key] = extracted;
            
            // Close the bitmap to free memory
            imageBitmap.close();
            
            return { key, success: true };
        } catch (e) {
            console.warn(`[Worker] Failed to load textile ${key}:`, e);
            return { key, success: false };
        }
    });
    
    // Wait for all to complete
    await Promise.all([...skinsheetPromises, ...textilePromises]);
    
    return sprites;
}

// Listen for messages from main thread
self.addEventListener('message', async (event) => {
    const { type, data } = event.data;
    
    if (type === 'INIT') {
        // Receive sheet data from main thread
        skinsheets = data.skinsheets;
        textiles = data.textiles;
        
        try {
            // Load and process all sheets
            const sprites = await load_sheets();
            
            // Collect all ArrayBuffers for transfer
            // ImageData objects contain ArrayBuffers in their .data property
            const transferList = [];
            Object.keys(sprites).forEach(sheetKey => {
                const spriteArray = sprites[sheetKey];
                if (Array.isArray(spriteArray)) {
                    spriteArray.forEach(imageData => {
                        if (imageData && imageData.data && imageData.data.buffer) {
                            transferList.push(imageData.data.buffer);
                        }
                    });
                }
            });
            
            // Send processed sprites back to main thread using transfer list
            // This transfers ownership without copying - much faster!
            self.postMessage({
                type: 'SPRITES_LOADED',
                sprites: sprites
            }, transferList);
            
        } catch (error) {
            console.error('[Worker] Failed to load sheets:', error);
            self.postMessage({
                type: 'SPRITES_ERROR',
                error: error.message
            });
        }
    }
});
