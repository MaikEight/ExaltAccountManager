import React, { useEffect, useMemo, useRef, useState } from 'react';
import { Box } from '@mui/material';
import { useTheme } from '@mui/material/styles';
import throttle from 'lodash.throttle';
import { generateItemCanvasImage } from '../../utils/itemCanvasUtils';
import useVaultPeekerPopper from '../../hooks/useVaultPeekerPopper';
import useVaultPeeker from '../../hooks/useVaultPeeker';

const ItemCanvas = ({
    canvasIdentifier,
    itemIds,
    items,
    imgSrc,
    overrideItemImages = {},
    totals = {},
    filteredTotals = {},
    override = {},
    overrideTotals = null,
    filter = [],
    saveCanvas,
}) => {
    const theme = useTheme();
    const containerRef = useRef(null);
    const imageRef = useRef(null);
    const [imageWidth, setImageWidth] = useState(null);
    const [baseImageUrl, setBaseImageUrl] = useState(null);
    const [itemPositions, setItemPositions] = useState([]);
    const [hoverStyle, setHoverStyle] = useState(null);
    const [isHovering, setIsHovering] = useState(false);
    const { setSelectedItem } = useVaultPeeker();
    const { setPopperPosition } = useVaultPeekerPopper();

    // Track width with ResizeObserver
    useEffect(() => {
        const observer = new ResizeObserver(([entry]) => {
            const newWidth = Math.floor(entry.contentRect.width); // important to floor
            setImageWidth((prev) => (prev !== newWidth ? newWidth : prev));
        });

        if (imageRef.current) {
            observer.observe(imageRef.current);
        }

        return () => {
            observer.disconnect();
        };
    }, []);

    const drawDeps = JSON.stringify({
        itemIds,
        items,
        totals,
        filteredTotals,
        override,
        overrideTotals,
        filter,
        imgSrc,
    });

    useEffect(() => {
        requestIdleCallback(() => {
            containerRef.current?.getBoundingClientRect();
        });
    }, []);

    // Generate static canvas image
    useEffect(() => {
        if (canvasIdentifier === 'machschohn3@web.de_Vault') {
            console.log("Generating item canvas image", { canvasIdentifier, itemIds, items, imgSrc, overrideItemImages, totals, filteredTotals, override, overrideTotals, filter });
        }

        generateItemCanvasImage({
            width: containerRef.current?.clientWidth || imageWidth,
            itemIds,
            items,
            imgSrc,
            overrideItemImages,
            totals,
            filteredTotals,
            override,
            overrideTotals,
            filter,
            theme
        }).then(({ imageUrl, itemPositions }) => {
            setBaseImageUrl(imageUrl);
            setItemPositions(itemPositions);
        });
    }, [
        imageWidth,
        drawDeps,
        theme.palette.mode
    ]);

    const handleMouseMove = throttle((e) => {
        const container = containerRef.current;
        if (!container) return;

        const rect = container.getBoundingClientRect();
        const x = e.clientX - rect.left;
        const y = e.clientY - rect.top;

        const found = itemPositions.find(
            (pos) =>
                x >= pos.x &&
                x <= pos.x + pos.width &&
                y >= pos.y &&
                y <= pos.y + pos.height
        );

        if (found) {
            setHoverStyle({
                position: 'absolute',
                left: found.x - 2,
                top: found.y - 2,
                width: found.width + 4,
                height: found.height + 4,
                backgroundColor:
                    theme.palette.mode === 'dark'
                        ? 'rgba(220,220,220,0.2)'
                        : 'rgba(0,0,0,0.2)',
                borderRadius: `${theme.shape.borderRadius + 2}px`,
                pointerEvents: 'none',
                transition: 'all 10ms ease-in-out',
                zIndex: 2
            });
        }
    }, 10);

    const handleClick = (e) => {
        const container = containerRef.current;
        if (!container) return;

        const rect = container.getBoundingClientRect();
        const x = e.clientX - rect.left;
        const y = e.clientY - rect.top;
        const isLeftHalf = x < rect.width / 2.5;

        for (const pos of itemPositions) {
            if (
                x >= pos.x &&
                x <= pos.x + pos.width &&
                y >= pos.y &&
                y <= pos.y + pos.height
            ) {
                if (pos.id === -1) return;

                const offset = 3;
                const popperY =
                    rect.top + (pos.y > 0 ? pos.y - offset : pos.y + pos.height + offset);
                const popperX =
                    rect.left +
                    (isLeftHalf ? pos.x + pos.width + offset : pos.x - offset);

                const setAsyncPopperPosition = async () => {
                    setPopperPosition({
                        top: popperY,
                        left: popperX,
                        isLeftHalf
                    });

                    setSelectedItem({
                        itemId: pos.id,
                        uniqueId: pos.uniqueId,
                        totals: totals[pos.id]
                    });
                }
                setAsyncPopperPosition();

                break;
            }
        }
    };

    const memoizedRender = useMemo(() => {
        return (
            <Box
                id="itemCanvasBox"
                ref={containerRef}
                sx={{
                    width: '100%',
                    height: 'fit-content',
                    position: 'relative',
                    p: 0,
                    m: 0,
                    overflow: 'hidden'
                }}
                onMouseEnter={() => setIsHovering(true)}
                onMouseLeave={() => {
                    setIsHovering(false);
                    setHoverStyle(null);
                }}
                onMouseMove={handleMouseMove}
                onClick={handleClick}
            >
                <img
                    src={baseImageUrl}
                    alt="Item Grid"
                    ref={imageRef}
                    style={{
                        opacity: baseImageUrl ? 1 : 0,
                        width: '100%',
                        display: 'block',
                        userSelect: 'none',
                        pointerEvents: 'none'
                    }}
                />
                {isHovering && hoverStyle && <div style={hoverStyle} />}
            </Box>
        );
    }, [baseImageUrl, itemPositions, isHovering, hoverStyle, theme]);

    console.log("Rendering item canvas");
    return memoizedRender;
};

export default React.memo(ItemCanvas);
