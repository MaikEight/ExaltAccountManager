import { CACHE_PREFIX } from "../constants";

const RARITY_IMAGE_SOURCES = {
    0: {
        source: null,
        width: 0,
        height: 0,
    },
    1: {
        source: "/realm/enchantments/uncommon.png",
        width: 8,
        height: 8,
    },
    2: {
        source: "/realm/enchantments/rare.png",
        width: 16,
        height: 8,
    },
    3: {
        source: "/realm/enchantments/legendary.png",
        width: 16,
        height: 12,
    },
    4: {
        source: "realm/enchantments/divine.png",
        width: 16,
        height: 16,
    },
};

const drawItemPromise = (imgSrc, item, rarity = 0, itemPadding = 5) => {
    return new Promise((resolve, reject) => {
        if (!item) {
            return resolve(null);
        }
        if (!imgSrc) {
            console.error("Image source is not provided");
            return resolve(null);
        }

        const cacheKey = `${CACHE_PREFIX}drawItem:${imgSrc}-${item[3]}-${item[4]}-${rarity}-${itemPadding}`;
        const cachedData = localStorage.getItem(cacheKey);
        let cachedObject;
        if (cachedData) {
            try {
                cachedObject = JSON.parse(cachedData);
            } catch (error) {
                localStorage.removeItem(cacheKey);
            }
        }
        if (cachedObject && cachedObject.image) {
            const cachedTime = cachedObject.time;
            const currentTime = Date.now();
            const maxCacheDuration = 1000 * 60 * 60 * 24 * 7; // 7 days
            if (currentTime - cachedTime < maxCacheDuration) {
                return resolve(cachedObject.image);
            }
            localStorage.removeItem(cacheKey);
        }

        const itemSize = 40;
        const canvasSize = itemSize + (2 * itemPadding);
        const canvas = document.createElement("canvas");
        canvas.width = canvasSize;
        canvas.height = canvasSize;
        const ctx = canvas.getContext("2d");

        const img = new Image();
        img.src = imgSrc;

        img.onload = () => {
            ctx.clearRect(0, 0, canvas.width, canvas.height);
            ctx.drawImage(
                img,
                item[3],
                item[4],
                itemSize,
                itemSize,
                itemPadding,
                itemPadding,
                itemSize,
                itemSize
            );

            // Draw rarity image if it exists
            const rarityConfig = RARITY_IMAGE_SOURCES[rarity];
            if (rarityConfig && rarityConfig.source) {
                const rarityImg = new Image();
                rarityImg.src = rarityConfig.source;

                rarityImg.onload = () => {
                    // Position at bottom right, respecting padding
                    const rarityX = canvasSize - itemPadding - rarityConfig.width;
                    const rarityY = canvasSize - itemPadding - rarityConfig.height;

                    ctx.drawImage(
                        rarityImg,
                        rarityX,
                        rarityY,
                        rarityConfig.width,
                        rarityConfig.height
                    );

                    // Convert canvas to an image URL and cache it
                    const imageUrl = canvas.toDataURL("image/png");
                    const cacheObject = {
                        time: Date.now(),
                        image: imageUrl,
                    };
                    localStorage.setItem(cacheKey, JSON.stringify(cacheObject));
                    resolve(imageUrl);
                };

                rarityImg.onerror = (error) => {
                    console.error("Failed to load rarity image", rarityConfig.source, error);
                    // Still resolve with the item image even if rarity fails
                    const imageUrl = canvas.toDataURL("image/png");
                    const cacheObject = {
                        time: Date.now(),
                        image: imageUrl,
                    };
                    localStorage.setItem(cacheKey, JSON.stringify(cacheObject));
                    resolve(imageUrl);
                };
            } else {
                // No rarity image to draw
                const imageUrl = canvas.toDataURL("image/png");
                const cacheObject = {
                    time: Date.now(),
                    image: imageUrl,
                };
                localStorage.setItem(cacheKey, JSON.stringify(cacheObject));
                resolve(imageUrl);
            }
        };

        img.onerror = (error) => {
            console.error("Failed to load image", imgSrc, error);
            reject(error);
        };
    });
};

export const drawItem = (imgSrc, item, callback, rarity = 0, itemPadding = 5) => {
    drawItemPromise(imgSrc, item, rarity, itemPadding)
        .then((result) => callback(result))
        .catch((error) => callback(null));
};

export const drawItemAsync = async (imgSrc, item, rarity = 0, itemPadding = 5) => {
    return await drawItemPromise(imgSrc, item, rarity, itemPadding);
};

export const getItemRarity = (itemData) => {
    if (!itemData || !itemData.enchant_ids || itemData.enchant_ids.length === 0) return 0; // No enchantments, no rarity - common
    const minEnchantId = 16; // Every id below does not count towards rarity
    // Return the length of enchantments above minEnchantId as rarity
    return Math.min(4, itemData.enchant_ids.filter(id => id >= minEnchantId).length);
};