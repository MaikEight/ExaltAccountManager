import React, { useEffect, useRef, useState } from 'react';
import throttle from 'lodash.throttle';
import { alpha, useTheme } from '@mui/material/styles';
import { Box } from '@mui/material';
import useItemCanvas from '../../hooks/useItemCanvas';
import useVaultPeeker from '../../hooks/useVaultPeeker';

const ItemCanvas = ({ canvasIdentifier, itemIds, items, imgSrc, overrideItemImages = {}, totals = {}, filteredTotals = {}, override = {}, filter = [], saveCanvas }) => {
    const canvasRef = useRef(null);
    const baseCanvasRef = useRef(null);
    const itemPositionsRef = useRef([]);
    const [hoveredItem, setHoveredItem] = useState(null);
    const theme = useTheme();
    const { hoveredConvasId, setHoveredConvasId } = useItemCanvas();
    const { setSelectedItem, setPopperPosition } = useVaultPeeker();

    useEffect(() => {
        if (!saveCanvas || saveCanvas === 0) {
            return;
        }
    
        const baseCanvas = baseCanvasRef.current;
    
        if (!baseCanvas) {
            console.error('Canvas element not found');
            return;
        }
    
        const baseCanvasWidth = baseCanvas.width;
        const baseCanvasHeight = baseCanvas.height;
    
        // Create an off-screen canvas
        const offScreenCanvas = document.createElement('canvas');
        offScreenCanvas.width = baseCanvasWidth;
        offScreenCanvas.height = baseCanvasHeight;
        const ctx = offScreenCanvas.getContext('2d');
    
        // Draw the background color
        ctx.fillStyle = theme.palette.background.paper;
        ctx.fillRect(0, 0, baseCanvasWidth, baseCanvasHeight);
    
        // Load the branding logo
        const brandingLogo = new Image();
        brandingLogo.src = theme.palette.mode === 'dark' ? '/logo/logo_inner_big.png' : '/logo/logo_inner_big_dark.png';
        brandingLogo.onload = () => {
            // Calculate the dimensions and position for the branding logo while maintaining aspect ratio
            const aspectRatio = brandingLogo.width / brandingLogo.height;
            let brandingWidth = baseCanvasWidth * 0.75;
            let brandingHeight = brandingWidth / aspectRatio;
    
            if (brandingHeight > baseCanvasHeight * 0.75) {
                brandingHeight = baseCanvasHeight * 0.75;
                brandingWidth = brandingHeight * aspectRatio;
            }
    
            const brandingX = (baseCanvasWidth - brandingWidth) / 2;
            const brandingY = (baseCanvasHeight - brandingHeight) / 2;
    
            // Set the opacity for the branding logo
            ctx.globalAlpha = theme.palette.mode === 'dark' ? 0.075 : 0.2;
    
            // Draw the branding logo
            ctx.drawImage(brandingLogo, brandingX, brandingY, brandingWidth, brandingHeight);
    
            // Reset the opacity
            ctx.globalAlpha = 1.0;
    
            // Draw the baseCanvas image on top of the branding
            const baseCanvasImage = new Image();
            baseCanvasImage.src = baseCanvas.toDataURL('image/png');
            baseCanvasImage.onload = () => {
                ctx.drawImage(baseCanvasImage, 0, 0);
    
                // Save the resulting image
                const img = new Image();
                img.src = offScreenCanvas.toDataURL('image/png');
    
                img.onload = () => {
                    const link = document.createElement('a');
                    link.download = 'totals.png';
                    link.href = img.src;
                    link.click();
                };
            };
        };
    }, [saveCanvas]);

    useEffect(() => {
        const canvas = canvasRef.current;
        const baseCanvas = baseCanvasRef.current;
        if (!canvas || !baseCanvas) {
            console.error('Canvas element not found');
            return;
        }
        const ct = canvas.getContext('2d');
        const baseCt = baseCanvas.getContext('2d');
        if (!ct || !baseCt) {
            console.error('Failed to get canvas context');
            return;
        }

        const img = new Image();
        img.src = imgSrc;

        const overrideImages = {};
        if (overrideItemImages) {
            Object.keys(overrideItemImages)
                .forEach(overrideItem => {
                    const overrideImg = new Image();
                    overrideImg.src = overrideItemImages[overrideItem].imgSrc;
                    overrideImages[overrideItem] = overrideImg;
                    overrideItemImages[overrideItem].img = overrideImages[overrideImages.length - 1];
                });
        }

        const drawBaseImage = () => {
            const { width } = canvas.getBoundingClientRect();
            canvas.width = width;
            baseCanvas.width = width;
            const itemBoxSize = 50;
            const itemSize = 40;
            const itemPadding = (itemBoxSize - itemSize) / 2;
            const spacing = itemBoxSize + 4; // Size + margin
            const itemsPerRow = Math.floor(width / spacing);
            const totalRows = Math.ceil(itemIds.length / itemsPerRow);
            canvas.height = totalRows * spacing;
            baseCanvas.height = canvas.height;

            //if the canvas element has a width or height of 0, the canvas will not be displayed
            if (canvas.width === 0 || canvas.height === 0) {
                return;
            }

            // Clear the base canvas
            baseCt.clearRect(0, 0, baseCanvas.width, baseCanvas.height);

            let x = 2;
            let y = 2;

            itemPositionsRef.current = [];
            
            override = {
                totals: true,
                fillStyle: alpha(theme.palette.primary.main, 0.3),
                fillNumbers: true,
                minTotal: 1,
                ...override
            };

            for (let index = 0; index < itemIds.length; index++) {
                const itemId = itemIds[index];
                const id = itemId;
                const it = items[id] ? items[id] : items[0];

                if (!it) {
                    //Item not found, skip it
                    continue;
                }

                if (x + itemBoxSize > width) {
                    x = 2;
                    y += spacing;
                }

                baseCt.save();
                baseCt.translate(x, y);

                // Draw background box
                baseCt.fillStyle = '#00000000';
                baseCt.fillRect(0, 0, itemBoxSize, itemBoxSize);

                if(overrideImages['all']) {
                    baseCt.drawImage(overrideImages['all'], 0, 0, overrideItemImages['all'].size, overrideItemImages['all'].size, overrideItemImages['all'].padding, overrideItemImages['all'].padding, overrideItemImages['all'].size, overrideItemImages['all'].size);
                }

                if (filter.includes(id)) {
                    baseCt.fillStyle = override.fillStyle;
                    baseCt.fillRect(0, 0, itemBoxSize, itemBoxSize);
                }

                if (overrideImages[id]) {
                    // Draw the item with the custom image                    
                    baseCt.drawImage(overrideImages[id], 0, 0, overrideItemImages[id].size, overrideItemImages[id].size, overrideItemImages[id].padding, overrideItemImages[id].padding, overrideItemImages[id].size, overrideItemImages[id].size);
                } else {
                    // Draw the item in the center of the box
                    baseCt.drawImage(img, it[3], it[4], itemSize, itemSize, itemPadding, itemPadding, itemSize, itemSize);
                }

                // Store the position and ID of the item
                itemPositionsRef.current.push({ uniqueId: `${id}-${index}`, id, x, y, width: itemBoxSize, height: itemBoxSize, name: it[0] });

                if (override.fillNumbers !== false && totals[id]?.amount > (override.minTotal || 1)) {
                    baseCt.save();
                    baseCt.fillStyle = 'white';
                    baseCt.strokeStyle = 'black';
                    baseCt.lineWidth = 3;
                    baseCt.shadowBlur = 3;
                    baseCt.font = '16px Roboto, bold';
                    baseCt.textAlign = 'right';
                    const text = (totals[id].amount !== filteredTotals[id]?.amount && filteredTotals[id]?.amount !== undefined) ? `${filteredTotals[id].amount} (${totals[id]?.amount})` : `${totals[id].amount}`;
                    baseCt.strokeText(text, itemBoxSize - 3, itemBoxSize - 5);
                    baseCt.fillText(text, itemBoxSize - 3, itemBoxSize - 5);
                    baseCt.restore();
                }
                baseCt.restore();

                x += spacing;
            }

            // Draw the base canvas to the main canvas initially
            ct.drawImage(baseCanvas, 0, 0);
        };

        img.onload = () => {
            drawBaseImage();
            redrawCanvas(hoveredItem, true);
        };

        img.onerror = () => {
            console.error('Failed to load image', imgSrc);
        };

        const handleResize = () => {
            drawBaseImage();
            redrawCanvas(hoveredItem);
        };

        window.addEventListener('resize', handleResize);

        return () => {
            window.removeEventListener('resize', handleResize);
        };
    }, [itemIds, items, imgSrc, hoveredItem]);

    useEffect(() => {
        if (hoveredConvasId !== canvasIdentifier) {
            setHoveredItem(null);
        }
    }, [hoveredConvasId]);

    const handleMouseMove = throttle((event) => {
        const canvas = canvasRef.current;
        const rect = canvas.getBoundingClientRect();
        const x = event.clientX - rect.left;
        const y = event.clientY - rect.top;

        let hoverId = null;
        for (let position of itemPositionsRef.current) {
            if (
                x >= position.x &&
                x <= position.x + position.width &&
                y >= position.y &&
                y <= position.y + position.height
            ) {
                hoverId = position.uniqueId;
                break;
            }
        }

        setHoveredItem(hoverId);
        setHoveredConvasId(canvasIdentifier);
    }, 10);

    const handleClick = (event) => {
        const canvas = canvasRef.current;
        const rect = canvas.getBoundingClientRect();
        const x = event.clientX - rect.left;
        const y = event.clientY - rect.top;
        const canvasMidpoint = rect.width / 2.5;
        const isLeftHalf = x < canvasMidpoint;

        for (let position of itemPositionsRef.current) {
            if (
                x >= position.x &&
                x <= position.x + position.width &&
                y >= position.y &&
                y <= position.y + position.height
            ) {
                const offset = 3;
                // Initialize popperY considering the space above the item
                let popperY;
                const spaceAbove = position.y; // Space above the item
                if (spaceAbove < 0) {
                    // Not enough space above, position below the item
                    popperY = position.y + rect.top + position.height + offset;
                } else {
                    // Enough space above, position as before
                    popperY = position.y + rect.top - offset;
                }

                let popperX;
                if (isLeftHalf) {
                    // Position to the right of the item if clicked on the left half
                    popperX = position.x + rect.left + position.width + offset;
                } else {
                    // Position to the left of the item if clicked on the right half
                    popperX = position.x + rect.left - offset;
                }
                setPopperPosition({ top: popperY, left: popperX, isLeftHalf: isLeftHalf });
                setSelectedItem({
                    itemId: position.id,
                    uniqueId: position.uniqueId,
                    totals: totals[position.id]
                });
                break;
            }
        }
    };

    const redrawCanvas = (hoverId) => {        
        const canvas = canvasRef.current;
        const baseCanvas = baseCanvasRef.current;
        if (!canvas || !baseCanvas) return;
        if (canvas.width === 0 || canvas.height === 0) {
            return;
        }

        const ct = canvas.getContext('2d');
        if (!ct) return;

        // Draw the base image
        ct.clearRect(0, 0, canvas.width, canvas.height);
        ct.drawImage(baseCanvas, 0, 0);

        // Highlight the hovered item
        if (hoverId) {
            const _hoveredPosition = itemPositionsRef.current.find(item => item.uniqueId === hoverId);
            const hoveredPosition = _hoveredPosition ? {
                ..._hoveredPosition,
                x: _hoveredPosition.x - 2,
                y: _hoveredPosition.y - 2,
                width: _hoveredPosition.width + 4,
                height: _hoveredPosition.height + 4
            } : null;
            if (hoveredPosition) {
                ct.save();
                ct.translate(hoveredPosition.x, hoveredPosition.y);
                ct.fillStyle = theme.palette.mode === 'dark' ? 'rgba(220, 220, 220, 0.2)' : 'rgba(0, 0, 0, 0.2)';
                ct.beginPath();
                ct.moveTo(hoveredPosition.width - 9, 0);
                ct.arcTo(hoveredPosition.width, 0, hoveredPosition.width, 9, 9);
                ct.lineTo(hoveredPosition.width, hoveredPosition.height - 9);
                ct.arcTo(hoveredPosition.width, hoveredPosition.height, hoveredPosition.width - 9, hoveredPosition.height, 9);
                ct.lineTo(9, hoveredPosition.height);
                ct.arcTo(0, hoveredPosition.height, 0, hoveredPosition.height - 9, 9);
                ct.lineTo(0, 9);
                ct.arcTo(0, 0, 9, 0, 9);
                ct.closePath();
                ct.fill();
                ct.restore();
            }
        }
    };

    useEffect(() => {
        redrawCanvas(hoveredItem);
    }, [hoveredItem]);

    return (
        <Box id="itemCanvasBox" sx={{ width: '100%', height: 'fit-content', position: 'relative', p: 0, m:0, mb: '-7px' }}>
            <canvas
                ref={canvasRef}
                onMouseMove={handleMouseMove}
                onClick={handleClick}
                style={{ width: '100%', zIndex: 2 }}
            />
            <canvas
                ref={baseCanvasRef}
                style={{ display: 'none', zIndex: 2 }}
            />
        </Box>
    );
};

export default ItemCanvas;
