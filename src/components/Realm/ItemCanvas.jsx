import React, { useEffect, useRef, useState } from 'react';
import throttle from 'lodash.throttle';
import { useTheme } from '@mui/material/styles';
import ItemLocationPopper from './ItemLocationPopper';
import { Box } from '@mui/material';

const ItemCanvas = ({ itemIds, items, imgSrc, totals = {} }) => {
    const canvasRef = useRef(null);
    const baseCanvasRef = useRef(null); // To store the base image
    const itemPositionsRef = useRef([]); // To store item positions and ids
    const [hoveredItem, setHoveredItem] = useState(null); // Track the hovered item
    const theme = useTheme();
    const [selectedItem, setSelectedItem] = useState(null);
    const [poppoverPosition, setPopperPosition] = useState(null);

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
            //this is a workaround to fix that
            if (canvas.width === 0 || canvas.height === 0) {
                return;
            }

            // Clear the base canvas
            baseCt.clearRect(0, 0, baseCanvas.width, baseCanvas.height);

            let x = 2;
            let y = 2;

            itemPositionsRef.current = [];

            const filter = {};
            const override = {
                totals: true,
                fillStyle: '#ffcd57',
                fillNumbers: true,
                minTotal: 1,
            };

            for (let i = 0; i < itemIds.length; i++) {
                const id = itemIds[i];
                const it = items[id] ? items[id] : items[0];

                if (!it) {
                    console.log(`Item with ID ${id} not found in items`);
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

                if (id in filter) {
                    baseCt.fillStyle = override.fillStyle;
                    baseCt.fillRect(0, 0, itemBoxSize, itemBoxSize);
                }

                // Draw the item in the center of the box
                baseCt.drawImage(img, it[3], it[4], itemSize, itemSize, itemPadding, itemPadding, itemSize, itemSize);

                // Store the position and ID of the item
                itemPositionsRef.current.push({ id, x, y, width: itemBoxSize, height: itemBoxSize, name: it[0] });

                if (override.fillNumbers !== false && totals[id]?.amount > (override.minTotal || 1)) {
                    baseCt.save();
                    baseCt.fillStyle = 'white';
                    baseCt.strokeStyle = 'black';
                    baseCt.lineWidth = 3;
                    baseCt.shadowBlur = 3;
                    baseCt.font = '16px Roboto, bold';
                    baseCt.textAlign = 'right';
                    baseCt.strokeText(totals[id].amount, itemBoxSize - 3, itemBoxSize - 5);
                    baseCt.fillText(totals[id].amount, itemBoxSize - 3, itemBoxSize - 5);
                    baseCt.restore();
                }
                baseCt.restore();

                x += spacing;
            }

            // Draw the base canvas to the main canvas initially
            ct.drawImage(baseCanvas, 0, 0);
        };

        img.onload = () => {
            console.log('Image loaded successfully');
            drawBaseImage();
            redrawCanvas(hoveredItem);
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
                hoverId = position.id;
                break;
            }
        }

        setHoveredItem(hoverId);
    }, 100); // Throttle mouse move event to every 100ms

    const handleClick = (event) => {
        const canvas = canvasRef.current;
        const rect = canvas.getBoundingClientRect();
        const x = event.clientX - rect.left;
        const y = event.clientY - rect.top;
        setSelectedItem({
            itemId: hoveredItem,
            totals: totals[hoveredItem]
        });

        for (let position of itemPositionsRef.current) {
            if (
                x >= position.x &&
                x <= position.x + position.width &&
                y >= position.y &&
                y <= position.y + position.height
            ) {
                const offset = 3;
                setPopperPosition({ top: position.y + rect.top - offset, left: position.x + rect.left + position.width + offset });
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
            const _hoveredPosition = itemPositionsRef.current.find(item => item.id === hoverId);
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
        <Box style={{ width: '100%', height: '100%', position: 'relative' }}>
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
            <ItemLocationPopper
                open={Boolean(selectedItem)}
                position={poppoverPosition}
                selectedItem={selectedItem}
                onClose={() => {
                    setSelectedItem(null);
                    setPopperPosition(null);
                }}
            />
        </Box>
    );
};

export default ItemCanvas;
