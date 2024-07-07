
export const drawItem = (imgSrc, item, callback, itemPadding = 5) => {
    if(!item) {
        callback(null);
        return;
    }
    
    const itemSize = 40;
    const canvasSize = itemSize + (2 * itemPadding);
    
    const canvas = document.createElement('canvas');
    const ctx = canvas.getContext('2d');

    canvas.width = canvasSize;
    canvas.height = canvasSize;

    const img = new Image();
    img.src = imgSrc;

    img.onload = () => {
        ctx.clearRect(0, 0, canvas.width, canvas.height);
        ctx.drawImage(img, item[3], item[4], itemSize, itemSize, itemPadding, itemPadding, itemSize, itemSize);

        // Convert canvas to image
        const imageUrl = canvas.toDataURL('image/png');
        callback(imageUrl);
    };

    img.onerror = (error) => {
        console.error('Failed to load image', imgSrc, error);
    };
};