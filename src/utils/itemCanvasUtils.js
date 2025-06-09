import { alpha } from '@mui/material/styles';

/**
 * Generates an offscreen canvas image with all items drawn
 * @returns {Promise<{ imageUrl: string, itemPositions: Array }>}
 */
export async function generateItemCanvasImage({
    width = 900,
    itemIds = [],
    items = {},
    imgSrc,
    overrideItemImages = {},
    totals = {},
    filteredTotals = {},
    override = {},
    overrideTotals = null,
    filter = [],
    theme
}) {
    return new Promise((resolve, reject) => {
        // console.log('Generating item canvas image', { width, itemIds, items, imgSrc, overrideItemImages, totals, filteredTotals, override, overrideTotals, filter });
        const canvas = document.createElement('canvas');
        const ctx = canvas.getContext('2d');

        const itemBoxSize = 50;
        const itemSize = 40;
        const itemPadding = (itemBoxSize - itemSize) / 2;
        const spacing = itemBoxSize + 4;

        const canvasWidth = width;
        const itemsPerRow = Math.max(1, Math.floor(width / spacing));
        const totalRows = Math.ceil(itemIds.length / itemsPerRow);

        canvas.width = canvasWidth;
        canvas.height = totalRows * spacing;

        const itemPositions = [];

        const spriteImg = new Image();
        spriteImg.src = imgSrc;

        const overrideImages = {};
        const preloadOverrideImages = Object.entries(overrideItemImages).map(([key, value]) => {
            return new Promise((res) => {
                const img = new Image();
                img.src = value.imgSrc;
                img.onload = () => {
                    overrideImages[key] = img;
                    res();
                };
            });
        });

        Promise.all([new Promise(res => { spriteImg.onload = res; }), ...preloadOverrideImages])
            .then(() => {
                override = {
                    totals: true,
                    fillStyle: alpha(theme.palette.primary.main, 0.3),
                    fillNumbers: true,
                    minTotal: 1,
                    ...override
                };

                let x = 2;
                let y = 2;

                for (let index = 0; index < itemIds.length; index++) {
                    const itemId = itemIds[index];
                    const it = items[itemId] ?? items[0];

                    if (!it) continue;

                    if (x + itemBoxSize > canvas.width) {
                        x = 2;
                        y += spacing;
                    }

                    ctx.save();
                    ctx.translate(x, y);

                    // Background box
                    ctx.fillStyle = '#00000000';
                    ctx.fillRect(0, 0, itemBoxSize, itemBoxSize);

                    // Background override (if any)
                    if (overrideImages['all']) {
                        const { size, padding } = overrideItemImages['all'];
                        ctx.drawImage(overrideImages['all'], 0, 0, size, size, padding, padding, size, size);
                    }

                    // Filter highlight
                    if (filter.includes(itemId)) {
                        ctx.fillStyle = override.fillStyle;
                        ctx.fillRect(0, 0, itemBoxSize, itemBoxSize);
                    }

                    // Main item image
                    if (overrideImages[itemId]) {
                        const { size, padding } = overrideItemImages[itemId];
                        ctx.drawImage(overrideImages[itemId], 0, 0, size, size, padding, padding, size, size);
                    } else {
                        ctx.drawImage(spriteImg, it[3], it[4], itemSize, itemSize, itemPadding, itemPadding, itemSize, itemSize);
                    }

                    // Totals label
                    const totalAmount = (Boolean(overrideTotals) ? overrideTotals[itemId]?.amount : totals[itemId]?.amount) || 0;
                    if (override.fillNumbers !== false && totalAmount > (override.minTotal || 1)) {
                        ctx.save();
                        ctx.fillStyle = 'white';
                        ctx.strokeStyle = 'black';
                        ctx.lineWidth = 3;
                        ctx.shadowBlur = 3;
                        ctx.font = '16px Roboto, bold';
                        ctx.textAlign = 'right';
                        const text = (totalAmount !== filteredTotals[itemId]?.amount && filteredTotals[itemId]?.amount !== undefined)
                            ? `${filteredTotals[itemId].amount} (${totalAmount})`
                            : `${totalAmount}`;
                        ctx.strokeText(text, itemBoxSize - 3, itemBoxSize - 5);
                        ctx.fillText(text, itemBoxSize - 3, itemBoxSize - 5);
                        ctx.restore();
                    }

                    ctx.restore();

                    itemPositions.push({ uniqueId: `${itemId}-${index}`, id: itemId, x, y, width: itemBoxSize, height: itemBoxSize, name: it[0] });

                    x += spacing;
                }

                const imageUrl = canvas.toDataURL('image/png');
                resolve({ imageUrl, itemPositions });
            })
            .catch(err => {
                console.error('Image generation failed', err);
                reject(err);
            });
    });
}
