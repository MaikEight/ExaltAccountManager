import { CACHE_PREFIX } from "../constants";
import {
    getRuntimeAssetCacheKey,
    getRuntimeSpriteHash,
    MISSING_ITEM_SPRITE_SOURCE,
} from "../assets/runtimeAssets";
import { getRuntimeItemSpriteSource } from "../backend/assetApi";

export const RARITY_IMAGE_SOURCES = {
    0: {
        rarity: "common",
        source: null,
        width: 0,
        height: 0,
    },
    1: {
        rarity: "uncommon",
        source: "/realm/enchantments/uncommon.png",
        width: 8,
        height: 8,
    },
    2: {
        rarity: "rare",
        source: "/realm/enchantments/rare.png",
        width: 16,
        height: 8,
    },
    3: {
        rarity: "legendary",
        source: "/realm/enchantments/legendary.png",
        width: 16,
        height: 12,
    },
    4: {
        rarity: "divine",
        source: "realm/enchantments/divine.png",
        width: 16,
        height: 16,
    },
};

export const drawItemPromise = async (item, rarity = 0, itemPadding = 5) => {
    if (!item) return null;

    const isShiny = item[10];
    const spriteHash = getRuntimeSpriteHash(item) || "missing";
    const runtimeCacheKey = getRuntimeAssetCacheKey();
    const cacheKey = `${CACHE_PREFIX}drawItem:${runtimeCacheKey}-${spriteHash}-${rarity}-${itemPadding}-${isShiny ? 1 : 0}`;
    const cachedData = localStorage.getItem(cacheKey);
    if (cachedData) {
        try {
            const cachedObject = JSON.parse(cachedData);
            const maxCacheDuration = 1000 * 60 * 60 * 24 * 7;
            if (cachedObject?.image && Date.now() - cachedObject.time < maxCacheDuration) {
                return cachedObject.image;
            }
        } catch (error) {
            // Ignore malformed browser cache entries and regenerate below.
        }
        localStorage.removeItem(cacheKey);
    }

    const spriteSource = await getRuntimeItemSpriteSource(item);
    const isMissingSprite = spriteSource === MISSING_ITEM_SPRITE_SOURCE;
    return new Promise((resolve, reject) => {
        const itemSize = 40;
        const canvasSize = itemSize + (2 * itemPadding);
        const canvas = document.createElement("canvas");
        canvas.width = canvasSize;
        canvas.height = canvasSize;
        const ctx = canvas.getContext("2d");

        const img = new Image();
        img.src = spriteSource;

        img.onload = () => {
            ctx.clearRect(0, 0, canvas.width, canvas.height);
            ctx.drawImage(img, itemPadding, itemPadding, itemSize, itemSize);

            const finalize = () => {
                const imageUrl = canvas.toDataURL("image/png");
                if (!isMissingSprite) {
                    try {
                        const cacheObject = { time: Date.now(), image: imageUrl };
                        localStorage.setItem(cacheKey, JSON.stringify(cacheObject));
                    } catch (error) {
                        console.warn("Unable to cache rendered item image", error);
                    }
                }
                resolve(imageUrl);
            };

            const drawShiny = (callback) => {
                if (!isShiny) return callback();
                const shinyImg = new Image();
                shinyImg.src = "/realm/shiny.png";
                shinyImg.onload = () => {
                    ctx.drawImage(shinyImg, 0, 0, 50, 50, itemPadding, itemPadding, 16, 16);
                    callback();
                };
                shinyImg.onerror = (error) => {
                    console.error("Failed to load shiny image", error);
                    callback();
                };
            };

            // Draw rarity image if it exists
            const rarityConfig = RARITY_IMAGE_SOURCES[rarity];
            if (rarityConfig && rarityConfig.source) {
                const rarityImg = new Image();
                rarityImg.src = rarityConfig.source;

                rarityImg.onload = () => {
                    // Position at bottom right, respecting padding
                    const rarityX = canvasSize - itemPadding - rarityConfig.width;
                    const rarityY = canvasSize - itemPadding - rarityConfig.height;
                    ctx.drawImage(rarityImg, rarityX, rarityY, rarityConfig.width, rarityConfig.height);
                    drawShiny(finalize);
                };

                rarityImg.onerror = (error) => {
                    console.error("Failed to load rarity image", rarityConfig.source, error);
                    drawShiny(finalize);
                };
            } else {
                drawShiny(finalize);
            }
        };

        img.onerror = (error) => {
            console.error("Failed to load live item sprite", spriteHash, error);
            reject(error);
        };
    });
};

export const drawItem = (item, callback, rarity = 0, itemPadding = 5) => {
    drawItemPromise(item, rarity, itemPadding)
        .then((result) => callback(result))
        .catch((error) => callback(null));
};

export const drawItemAsync = async (item, rarity = 0, itemPadding = 5) => {
    return await drawItemPromise(item, rarity, itemPadding);
};

/** Crops EAM-owned UI sprite sheets such as the empty equipment silhouettes. */
export const drawSpriteSheetItemAsync = (source, item, itemPadding = 5) => {
    if (!source || !item) return Promise.resolve(null);
    return new Promise((resolve, reject) => {
        const itemSize = 40;
        const canvas = document.createElement("canvas");
        canvas.width = itemSize + (2 * itemPadding);
        canvas.height = itemSize + (2 * itemPadding);
        const context = canvas.getContext("2d");
        const image = new Image();
        image.onload = () => {
            context.drawImage(
                image,
                item[3],
                item[4],
                itemSize,
                itemSize,
                itemPadding,
                itemPadding,
                itemSize,
                itemSize,
            );
            resolve(canvas.toDataURL("image/png"));
        };
        image.onerror = reject;
        image.src = source;
    });
};

export const getItemRarity = (itemData) => {
    if (!itemData || !itemData.enchant_ids || itemData.enchant_ids.length === 0) return 0; // No enchantments, no rarity - common
    const minEnchantId = 16; // Every id below does not count towards rarity
    // Return the length of enchantments above minEnchantId as rarity
    return Math.min(4, itemData.enchant_ids.filter(id => id >= minEnchantId).length);
};
