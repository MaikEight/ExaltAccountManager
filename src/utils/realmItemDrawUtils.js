import { CACHE_PREFIX } from "../constants";

const drawItemPromise = (imgSrc, item, itemPadding = 5) => {
    return new Promise((resolve, reject) => {
        if (!item) {
            return resolve(null);
        }
        if (!imgSrc) {
            console.error("Image source is not provided");
            return resolve(null);
        }

        const cacheKey = `${CACHE_PREFIX}drawItem:${imgSrc}-${item[3]}-${item[4]}`;
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
            // Convert canvas to an image URL and cache it
            const imageUrl = canvas.toDataURL("image/png");
            const cacheObject = {
                time: Date.now(),
                image: imageUrl,
            };
            localStorage.setItem(cacheKey, JSON.stringify(cacheObject));
            resolve(imageUrl);
        };

        img.onerror = (error) => {
            console.error("Failed to load image", imgSrc, error);
            reject(error);
        };
    });
};

export const drawItem = (imgSrc, item, callback, itemPadding = 5) => {
    drawItemPromise(imgSrc, item, itemPadding)
        .then((result) => callback(result))
        .catch((error) => callback(null));
};

export const drawItemAsync = async (imgSrc, item, itemPadding = 5) => {
    return await drawItemPromise(imgSrc, item, itemPadding);
};