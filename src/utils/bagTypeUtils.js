import { CACHE_PREFIX } from "../constants";

const BAG_TYPES_IMAGE_URL = '/realm/bagTypes.png';

async function getBagTypeImage(bagType) {
    return new Promise((resolve, reject) => {
        bagType = bagType || 0;
        bagType = Math.max(0, Math.min(9, bagType));

        const cacheKey = `${CACHE_PREFIX}drawBag:${bagType}`;
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

        let bagPosition = [0, 0];
        switch (bagType) {
            case 0: bagPosition = [0, 0]; break; // Brown Bag 
            case 1: bagPosition = [-26, 0]; break; // Pink Bag
            case 2: bagPosition = [-52, 0]; break; // Purple Bag
            case 3: bagPosition = [-78, 0]; break; // Egg Basket
            case 4: bagPosition = [-26, -26]; break; // Cyan Bag
            case 5: bagPosition = [-52, -26]; break; // Blue Bag
            case 6: bagPosition = [-26, -52]; break; // White Bag
            case 7: bagPosition = [0, -26]; break; // Golden Bag
            case 8: bagPosition = [-78, -26]; break; // Orange Bag
            case 9: bagPosition = [0, -52]; break; // Red Bag
            case 10: bagPosition = [-52, -52]; break; // None
        }

        const bagPadding = 0;
        const imageSize = 26;
        const canvasSize = imageSize;
        const canvas = document.createElement("canvas");
        canvas.width = canvasSize;
        canvas.height = canvasSize;
        const ctx = canvas.getContext("2d");

        const img = new Image();
        img.src = BAG_TYPES_IMAGE_URL;

        img.onload = () => {
            ctx.clearRect(0, 0, canvas.width, canvas.height);
            ctx.drawImage(
                img,
                -bagPosition[0],
                -bagPosition[1],
                imageSize,
                imageSize,
                bagPadding,
                bagPadding,
                imageSize,
                imageSize
            );

            const imageUrl = canvas.toDataURL("image/png");
            const cacheObject = { time: Date.now(), image: imageUrl };
            localStorage.setItem(cacheKey, JSON.stringify(cacheObject));
            resolve(imageUrl);
        };

        img.onerror = (error) => {
            console.error("Failed to load image", BAG_TYPES_IMAGE_URL, error);
            reject(error);
        };
    });
}

export { getBagTypeImage };