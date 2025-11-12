import { textiles, skinsheets } from '../assets/sheets';
import { skins, textures } from '../assets/constants';
import { CACHE_PREFIX } from '../constants';

// single component
function p_comp(s, x, y, i) {
    return s.data[((s.width * y + x) << 2) + i]
}

// single pixel
function p_dict(s, x, y) {
    var offset = (s.width * y + x) << 2;
    for (var i = 0, d = []; i < 4; i++) d[i] = s.data[offset + i]
    return d
}

// css-compatible
function p_css(s, x, y) {
    var d = p_dict(s, x, y);
    d[3] /= 255;
    return 'rgba(' + d.join(',') + ')'
}

// Initialize ready state and sprites object
let ready = false;
const sprites = {};

// Worker support detection and fallback
const supportsOffscreenCanvas = typeof OffscreenCanvas !== 'undefined';
let workerAvailable = supportsOffscreenCanvas && typeof Worker !== 'undefined';
let worker = null;

// Fallback functions for browsers without OffscreenCanvas support
// These maintain the EXACT same logic as before

// Function to extract sprites (fallback)
function extract_sprites(img, sx, sy) {
    sx = sx || 8;
    sy = sy || sx;
    const sprc = document.createElement('canvas');
    const c = sprc;
    c.crossOrigin = 'anonymous';
    c.width = img.width;
    c.height = img.height;
    const ctx = c.getContext('2d', { willReadFrequently: true });
    ctx.drawImage(img, 0, 0);
    let i = 0;
    const r = [];
    for (let y = 0; y < c.height; y += sy) {
        for (let x = 0; x < c.width; x += sx, i++) {
            r[i] = ctx.getImageData(x, y, sx, sy);
        }
    }
    return r;
}

// Function to extract skins (fallback)
function extract_skins(img, size) {
    size = size || 8;
    const sprc = document.createElement('canvas');
    const c = sprc;
    c.crossOrigin = "anonymous";
    c.width = img.width;
    c.height = img.height;
    const ctx = c.getContext('2d', { willReadFrequently: true });
    ctx.drawImage(img, 0, 0);
    let i = 0;
    const r = [];
    for (let y = 0; y < c.height; y += size * 3, i++) {
        r[i] = ctx.getImageData(0, y, size, size);
    }
    return r;
}

// Function to load image (fallback)
function load_img(src, t, s) {
    return new Promise((resolve, reject) => {
        const img = new Image();
        img.crossOrigin = 'anonymous';
        img.onload = function () {
            resolve(this, t, s);
        };
        img.onerror = function () {
            console.error(`${src} failed to load`);
            reject(src);
        };
        img.src = src;
    });
}

// Function to load sheets (fallback - original implementation)
function load_sheets_fallback() {
    console.time("load_sheets_fallback");
    console.log("Loading skinsheets and textiles (fallback mode)...");

    const skinsheetPromises = Object.keys(skinsheets).map(key => {
        const src = skinsheets[key];
        return load_img(src, key, +key)
            .then(img => {
                const size = key.includes('16') ? 16 : 8;
                sprites[key] = extract_skins(img, size);
            })
            .catch(e => {
                console.error(`Failed to load skinsheet ${key} from ${src}`, e);
            });
    });

    const textilePromises = Object.keys(textiles).map(key => {
        const src = textiles[key];
        return load_img(src, key, +key)
            .then(img => {
                sprites[key] = extract_sprites(img, +key);
            })
            .catch(e => {
                console.warn(`Failed to load textile ${key} from ${src}`, e);
            });
    });

    const result = Promise.all([...skinsheetPromises, ...textilePromises]);
    result.then(() => {
        console.timeEnd("load_sheets_fallback");
    });
    return result;
}

// Function to load sheets using Web Worker
function load_sheets_worker() {
    return new Promise((resolve, reject) => {
        console.log("Loading skinsheets and textiles (worker mode)...");
        
        try {
            // Create worker
            worker = new Worker(new URL('./portraitWorker.js', import.meta.url), { type: 'module' });
            
            // Listen for messages from worker
            worker.onmessage = (event) => {
                const { type, sprites: workerSprites, error } = event.data;
                
                if (type === 'SPRITES_LOADED') {
                    // Sprites are transferred (not copied) from worker using transferable objects
                    // This is a zero-copy operation for maximum performance
                    Object.assign(sprites, workerSprites);
                    console.log('Sprites loaded from worker (transferred via zero-copy)');
                    resolve();
                } else if (type === 'SPRITES_ERROR') {
                    console.error('Worker failed to load sprites:', error);
                    reject(new Error(error));
                }
            };
            
            worker.onerror = (error) => {
                console.error('Worker error:', error);
                workerAvailable = false;
                reject(error);
            };
            
            // Send sheet data to worker
            worker.postMessage({
                type: 'INIT',
                data: {
                    skinsheets,
                    textiles
                }
            });
            
        } catch (error) {
            console.error('Failed to create worker:', error);
            workerAvailable = false;
            reject(error);
        }
    });
}

// Main load sheets function with worker support and fallback
async function load_sheets() {
    if (workerAvailable) {
        try {
            await load_sheets_worker();
        } catch (error) {
            console.warn('Worker failed, falling back to main thread:', error);
            workerAvailable = false;
            await load_sheets_fallback();
        }
    } else {
        if (!supportsOffscreenCanvas) {
            console.warn('OffscreenCanvas not supported, using fallback mode');
        }
        await load_sheets_fallback();
    }
}

const fs = {};
let fsc;

// Function to create pattern from texture
function makeTexPattern(tex, ratio) {
    if (!fs[ratio]) fs[ratio] = {};
    const dict = fs[ratio];
    tex = +tex || 0;
    if (dict[tex]) return dict[tex];
    const i = (tex & 0xff000000) >> 24;
    let c = tex & 0xffffff;
    if (i === 0) return 'transparent';
    if (i === 1) {
        c = c.toString(16);
        while (c.length < 6) c = '0' + c;
        dict[tex] = '#' + c;
        return dict[tex];
    }
    if (!sprites[i]) {
        console.error(`Sprite sheet ${i} not found in sprites.`);
        return 'transparent';
    }
    const spr = sprites[i][c];
    if (!spr) {
        console.error(`Sprite ${c} not found in sheet ${i}`);
        return 'transparent';
    }
    fsc = fsc || document.createElement('canvas');
    const ca = fsc;
    const scale = ratio / 5;
    ca.width = spr.width;
    ca.height = spr.height;
    const cact = ca.getContext('2d');
    cact.imageSmoothingEnabled = false;
    cact.putImageData(spr, 0, 0);
    const p = cact.createPattern(ca, 'repeat');
    ca.width = scale * spr.width;
    ca.height = scale * spr.height;
    cact.scale(scale, scale);
    cact.fillStyle = p;
    cact.fillRect(0, 0, spr.width, spr.height);
    dict[tex] = cact.createPattern(ca, 'repeat');
    return dict[tex];
}

// Function to generate sprite based on skin, textures, etc.
function portrait(type, skin, tex1Id, tex2Id, adjust) {    
    const cacheKey = `${CACHE_PREFIX}portrait:${skin}-${tex1Id}-${tex2Id}-${adjust}`;
    const cachedImageString = localStorage.getItem(cacheKey);
    const cachedObject = cachedImageString ? JSON.parse(cachedImageString) : null;

    if (cachedObject) {
        const cachedTime = cachedObject.time;
        const currentTime = new Date().getTime();
        const timeDiff = currentTime - cachedTime;
        const maxCacheDuration = 1000 * 60 * 60 * 24 * 3; // 3 days
        if (timeDiff > maxCacheDuration) {
            localStorage.removeItem(cacheKey);
            cachedObject.image = null;
        }

        const cachedImage = cachedObject.image;
        if (cachedImage) {
            return cachedImage;
        }
    }

    if (!ready) {
        console.error('Sprites are not ready yet.');
        return '';
    }

    if (!skins[skin]) {
        skin = type;
    }

    let skinData = skins[skin];
    if (!skinData || !sprites[skinData[3]][skinData[1]]) {
        skin = type;
        skinData = skins[skin];
        if (!skinData) {
            console.error('Default skin not found.');
            return '';
        }
    }

    const tex1 = textures[tex1Id] ? textures[tex1Id][0] : null;
    const tex2 = textures[tex2Id] ? textures[tex2Id][2] : null;

    const size = skinData[2] ? 16 : 8;
    const ratio = skinData[2] ? 2 : 4;
    const fs1 = tex1 ? makeTexPattern(tex1Id, ratio) : 'transparent';
    const fs2 = tex2 ? makeTexPattern(tex2Id, ratio) : 'transparent';

    const st = document.createElement('canvas');
    st.width = 34;
    st.height = 34;
    const ctx = st.getContext('2d');
    ctx.save();
    ctx.clearRect(0, 0, st.width, st.height);
    ctx.translate(1, 1);

    const i = skinData[1];
    const sheetName = skinData[3];
    const spr = sprites[sheetName][i];
    const mask = sprites[sheetName + "Mask"][i];

    for (let xi = 0; xi < size; xi++) {
        const x = xi * ratio;
        const w = ratio;
        for (let yi = 0; yi < size; yi++) {
            if (p_comp(spr, xi, yi, 3) < 2) continue;
            const y = yi * ratio;
            const h = ratio;

            ctx.fillStyle = p_css(spr, xi, yi);
            ctx.fillRect(x, y, w, h);

            let vol = 0;
            function dotex(tex) {
                ctx.fillStyle = tex;
                ctx.fillRect(x, y, w, h);
                ctx.fillStyle = 'rgba(0,0,0,' + ((255 - vol) / 255) + ')';
                ctx.fillRect(x, y, w, h);
            }

            if (p_comp(mask, xi, yi, 3) > 1) {
                const red = p_comp(mask, xi, yi, 0);
                const green = p_comp(mask, xi, yi, 1);
                if (red > green && fs1 !== 'transparent') {
                    vol = red;
                    dotex(fs1);
                } else if (green > red && fs2 !== 'transparent') {
                    vol = green;
                    dotex(fs2);
                }
            }

            ctx.save();
            ctx.globalCompositeOperation = 'destination-over';
            ctx.strokeRect(x - 0.5, y - 0.5, w + 1, h + 1);
            ctx.restore();
        }
    }

    ctx.restore();
    const imageDataURL = st.toDataURL();

    // Cache the generated image in localStorage    
    const imageObject = {
        time: new Date().getTime(),
        image: imageDataURL
    };
    localStorage.setItem(cacheKey, JSON.stringify(imageObject));

    return imageDataURL;
}

// Initialize loading sheets
const preload = load_sheets();

// Wait for preload to finish
preload.then(() => {
    ready = true;
    window.portraitReady = true;

    console.log('Loaded sprite types:', Object.keys(sprites).length);
    console.log('Loaded sprites:', Object.keys(Object.keys(sprites).reduce((acc, key) => {
        acc[key] = sprites[key].length;
        return acc;
    }, {})).reduce((acc, key) => acc + sprites[key].length, 0)
);
    
    // Dispatch custom event to notify React components
    window.dispatchEvent(new CustomEvent('portraitReady'));
    
    // Clean up worker if it was used
    if (worker) {
        worker.terminate();
        worker = null;
    }
}).catch((error) => {
    console.error('Failed to load sheets:', error);
});

// Export portrait function and other utilities
export { portrait };