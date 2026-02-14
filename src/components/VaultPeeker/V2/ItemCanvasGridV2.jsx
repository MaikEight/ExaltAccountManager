import { useRef, useEffect, useState, useCallback, useLayoutEffect, forwardRef, useImperativeHandle } from 'react';
import { Box, Tooltip } from '@mui/material';
import items from '../../../assets/constants';
import { drawItemPromise, getItemRarity } from '../../../utils/realmItemDrawUtils';
import { TooltipUiForItem } from '../../Widgets/Widgets/Components/InventoryRender';
import useVaultPeeker from '../../../hooks/useVaultPeeker';
import useDebugLogs from './../../../hooks/useDebugLogs';

const SPRITESHEET_SRC = "renders.png";

// In-memory cache for HTMLImageElement objects (survives re-renders, cleared on page reload)
const imageElementCache = new Map();

/**
 * ItemCanvasGridV2 - A performant canvas-based item grid renderer
 * 
 * Key performance improvements:
 * 1. In-memory HTMLImageElement cache (avoids repeated Image() creation)
 * 2. Pre-loads ALL item images in PARALLEL before any drawing
 * 3. Draws ALL items to canvas in ONE synchronous batch (no incremental redraws)
 * 4. Uses ResizeObserver for dynamic width - fills available space
 * 5. Pure CSS hover highlighting - no React state for hover (buttery smooth)
 * 6. Leverages existing localStorage cache from drawItemPromise
 * 7. Supports density-based padding (dense: 0px, comfortable: 2px, spacious: 5px)
 */

const ITEM_SIZE = 40;
const DEFAULT_ITEM_PADDING = 2;

/**
 * Get rarity from item data - supports both direct maxRarity and enchant_ids
 */
const getRarityFromData = (data) => {
    if (!data || typeof data !== 'object') return 0;
    // If maxRarity is directly available (from totalsMap), use it
    if (typeof data.maxRarity === 'number') return data.maxRarity;
    // If itemData is nested (from TotalsViewV2), check there
    if (data.itemData && typeof data.itemData.maxRarity === 'number') return data.itemData.maxRarity;
    // Fallback to calculating from enchant_ids
    return getItemRarity(data);
};

/**
 * Pre-load all item images in parallel, returns Map of index -> HTMLImageElement
 * Uses in-memory cache to avoid repeated Image() creation
 */
const preloadAllItemImages = async (itemEntries, itemPadding, debugLogs = false) => {
    let cacheHits = 0;
    let cacheMisses = 0;

    const imagePromises = itemEntries.map(async ([itemId, data], index) => {
        const item = items[itemId];
        if (!item) return [index, null];

        const rarity = getRarityFromData(data);

        // Create cache key for in-memory lookup
        const memoryCacheKey = `${itemId}-${rarity}-${itemPadding}`;

        // Check in-memory cache first (much faster than localStorage)
        if (imageElementCache.has(memoryCacheKey)) {
            cacheHits++;
            return [index, imageElementCache.get(memoryCacheKey)];
        }

        cacheMisses++;

        try {
            const imageUrl = await drawItemPromise(SPRITESHEET_SRC, item, rarity, itemPadding);
            const img = new Image();
            await new Promise((resolve, reject) => {
                img.onload = resolve;
                img.onerror = reject;
                img.src = imageUrl;
            });

            // Store in memory cache for future renders
            imageElementCache.set(memoryCacheKey, img);

            return [index, img];
        } catch (error) {
            console.error(`Failed to load item ${itemId}:`, error);
            return [index, null];
        }
    });

    const results = await Promise.all(imagePromises);

    // Log cache performance (only when there are misses, to reduce noise)
    if (debugLogs && cacheMisses > 0) {
        console.log(`[VaultPeeker] Image cache: ${cacheHits} hits, ${cacheMisses} misses (${((cacheHits / (cacheHits + cacheMisses)) * 100).toFixed(1)}% hit rate)`);
    }

    return new Map(results);
};

/**
 * Draw all items to canvas in one batch (synchronous after images loaded)
 */
const drawAllItemsToCanvas = (ctx, itemEntries, imageMap, columns, cellSize, itemSize, itemPadding, showCounts) => {
    for (let i = 0; i < itemEntries.length; i++) {
        const [, data] = itemEntries[i];
        const col = i % columns;
        const row = Math.floor(i / columns);
        const x = col * cellSize;
        const y = row * cellSize;

        const img = imageMap.get(i);
        if (img) {
            // Draw the pre-rendered image (already includes padding baked in)
            ctx.drawImage(img, x, y, cellSize, cellSize);
        }

        // Draw count badge
        if (showCounts) {
            const count = typeof data === 'object' ? data.count : data;
            if (count > 1) {
                ctx.save();
                ctx.font = 'bold 11px Arial';
                ctx.textAlign = 'right';
                ctx.textBaseline = 'bottom';

                ctx.strokeStyle = 'rgba(0, 0, 0, 0.8)';
                ctx.lineWidth = 3;
                ctx.strokeText(count.toString(), x + cellSize - 2, y + cellSize - 1);

                ctx.fillStyle = '#fff';
                ctx.fillText(count.toString(), x + cellSize - 2, y + cellSize - 1);
                ctx.restore();
            }
        }
    }
};

const ItemCanvasGridV2 = ({
    itemEntries, // Array of [itemId, count] or [itemId, { count, itemData }]
    onItemClick,
    onItemHover,
    showCounts = true,
    maxHeight = 400,
    minColumns = 4,
    itemPadding: propPadding,
}) => {
    // Sort empty items to the end (itemId -1)
    itemEntries?.sort((a, b) => {
        const idA = a[0];
        const idB = b[0];
        if (idA === -1 && idB !== -1) return 1;
        if (idA !== -1 && idB === -1) return -1;
        return 0;
    });
    const { debugLogs } = useDebugLogs();
    const containerRef = useRef(null);
    const canvasRef = useRef(null);
    const [containerWidth, setContainerWidth] = useState(0);
    const [isRendering, setIsRendering] = useState(false);

    // Get padding from context or prop
    const { itemPadding: contextPadding } = useVaultPeeker();
    const itemPadding = propPadding ?? contextPadding ?? DEFAULT_ITEM_PADDING;

    // Calculate cell size based on padding
    const cellSize = ITEM_SIZE + (2 * itemPadding);

    // Dynamic columns based on container width
    const columns = Math.max(minColumns, Math.floor(containerWidth / cellSize) || minColumns);
    const rows = Math.ceil(itemEntries.length / columns);
    const canvasWidth = columns * cellSize;
    const canvasHeight = rows * cellSize;

    // ResizeObserver for dynamic width
    useLayoutEffect(() => {
        const container = containerRef.current;
        if (!container) return;

        const resizeObserver = new ResizeObserver((entries) => {
            for (const entry of entries) {
                const width = entry.contentRect.width;
                setContainerWidth(width);
            }
        });

        resizeObserver.observe(container);
        // Initial measurement
        setContainerWidth(container.offsetWidth);

        return () => resizeObserver.disconnect();
    }, []);

    // Render all items to canvas - batch load then batch draw
    useEffect(() => {
        const canvas = canvasRef.current;
        if (!canvas || itemEntries.length === 0 || containerWidth === 0) return;

        let cancelled = false;
        setIsRendering(true);

        const renderCanvas = async () => {
            const totalStart = performance.now();

            // Step 1: Pre-load ALL images in parallel (with padding baked in)
            const preloadStart = performance.now();
            const imageMap = await preloadAllItemImages(itemEntries, itemPadding, debugLogs);
            if (debugLogs) {
                console.log(`[VaultPeeker] Image preload took ${(performance.now() - preloadStart).toFixed(2)}ms (${itemEntries.length} items)`);
            }

            if (cancelled) return;

            // Step 2: Draw ALL items in one synchronous batch
            const drawStart = performance.now();
            const ctx = canvas.getContext('2d');
            ctx.clearRect(0, 0, canvasWidth, canvasHeight);
            drawAllItemsToCanvas(ctx, itemEntries, imageMap, columns, cellSize, ITEM_SIZE, itemPadding, showCounts);
            if (debugLogs) {
                console.log(`[VaultPeeker] Canvas draw took ${(performance.now() - drawStart).toFixed(2)}ms`);
                console.log(`[VaultPeeker] Total render took ${(performance.now() - totalStart).toFixed(2)}ms`);
            }
            setIsRendering(false);
        };

        renderCanvas();

        return () => { cancelled = true; };
    }, [itemEntries, columns, canvasWidth, canvasHeight, showCounts, containerWidth, itemPadding, cellSize]);

    // Click handler - find item by position
    const handleOverlayClick = useCallback((index, event) => {
        if (onItemClick && itemEntries[index]) {
            const [itemId, data] = itemEntries[index];
            onItemClick(itemId, data, event);
        }
    }, [itemEntries, onItemClick]);

    // Hover handler for tooltip content
    const getTooltipContent = useCallback((index) => {
        if (!itemEntries[index]) return null;
        const [itemId] = itemEntries[index];
        const item = items[itemId];
        if (!item) return null;

        return <TooltipUiForItem item={item} />;
    }, [itemEntries]);

    if (itemEntries.length === 0) {
        return (
            <Box sx={{ p: 2, textAlign: 'center', color: 'text.secondary' }}>
                No items to display
            </Box>
        );
    }

    return (
        <Box
            id="item-canvas-grid-v2-container"
            ref={containerRef}
            sx={{
                position: 'relative',
                width: '100%',
                minHeight: cellSize,
            }}
        >
            {isRendering && (
                <Box sx={{
                    position: 'absolute',
                    top: 4,
                    right: 0,
                    left: 0,
                    textAlign: 'center',
                    fontSize: '0.75rem',
                    color: 'text.secondary',
                    zIndex: 10,
                }}>
                    Loading...
                </Box>
            )}
            <Box
                id="item-canvas-grid-v2-scroll-container"
                sx={{
                    maxHeight,
                    overflow: 'auto',
                    position: 'relative',
                    // Center content horizontally
                    display: 'flex',
                    justifyContent: 'center',
                }}
            >
                <Box
                    sx={{
                        position: 'relative',
                        width: canvasWidth,
                        height: canvasHeight,
                    }}
                >
                    {/* Main canvas for all items */}
                    <canvas
                        ref={canvasRef}
                        width={canvasWidth}
                        height={canvasHeight}
                        style={{
                            display: 'block',
                            imageRendering: 'pixelated',
                        }}
                    />

                    {/* Transparent overlay grid - pure CSS hover, no React state */}
                    <Box
                        sx={{
                            position: 'absolute',
                            top: 0,
                            left: 0,
                            width: canvasWidth,
                            height: canvasHeight,
                            display: 'grid',
                            gridTemplateColumns: `repeat(${columns}, ${cellSize}px)`,
                            gridAutoRows: `${cellSize}px`,
                        }}
                    >
                        {itemEntries.map((_, index) => (
                            <Tooltip
                                key={index}
                                title={getTooltipContent(index)}
                                placement="bottom"
                                enterDelay={100}
                                leaveDelay={0}
                                slotProps={{
                                    popper: {
                                        modifiers: [{ name: 'offset', options: { offset: [0, -8] } }],                                                                              
                                    },
                                    tooltip: {
                                        sx: {
                                            opacity: 1,
                                            backgroundColor: theme => theme.palette.mode === 'dark' ? theme.palette.background.paper : theme.palette.text.primary,
                                            border: theme => `1px solid ${theme.palette.divider}`,                                            
                                        }
                                    }
                                }}
                            >
                                <Box
                                    onClick={(e) => handleOverlayClick(index, e)}
                                    onMouseEnter={() => onItemHover?.(itemEntries[index][0], itemEntries[index][1])}
                                    sx={{
                                        width: cellSize,
                                        height: cellSize,
                                        cursor: 'pointer',
                                        borderRadius: '4px',
                                        // Pure CSS hover - no React state needed
                                        '&:hover': {
                                            backgroundColor: 'rgba(255, 255, 255, 0.15)',
                                        },
                                    }}
                                />
                            </Tooltip>
                        ))}
                    </Box>
                </Box>
            </Box>
        </Box>
    );
};

export default ItemCanvasGridV2;
