import { useCallback, useEffect, useMemo, useRef, useState } from "react";
import { Box, Skeleton, Tooltip, Typography } from "@mui/material";
import { useTheme } from "@emotion/react";
import { drawItemAsync } from "../../../utils/realmItemDrawUtils";
import items from "../../../assets/constants";
import useVaultPeeker from "../../../hooks/useVaultPeeker";
import { TooltipUiForItem } from "../../Widgets/Widgets/Components/InventoryRender";

const ITEM_BASE_SIZE = 40;
const DEFAULT_ITEM_PADDING = 2;

/**
 * ItemGridV2 - A performant grid component for rendering realm items
 * 
 * Uses a hybrid approach:
 * - Items are rendered as individual <img> elements (leveraging browser's native image caching)
 * - A transparent overlay grid handles hover effects and click detection
 * - Items are drawn asynchronously with caching in localStorage
 * 
 * @param {Object} props
 * @param {Array} props.items - Array of { itemId, count?, maxRarity?, parsedItem? }
 * @param {Function} props.onItemClick - Callback when an item is clicked (itemId, event, itemData)
 * @param {boolean} props.showCounts - Whether to show item counts on the grid
 * @param {number} props.itemPadding - Padding around each item (0, 2, or 5)
 * @param {boolean} props.showEmptySlots - Whether to render empty slot placeholders
 * @param {string} props.emptySlotImage - Custom image for empty slots
 * @param {number} props.columns - Fixed number of columns (if not set, uses flex wrap)
 * @param {boolean} props.showTooltips - Whether to show item name tooltips on hover
 */
function ItemGridV2({
    items: itemsData = [],
    onItemClick,
    showCounts = true,
    itemPadding: propPadding,
    showEmptySlots = false,
    emptySlotImage,
    columns,
    showTooltips = false,
}) {
    const theme = useTheme();
    const containerRef = useRef(null);
    const { itemPadding: contextPadding } = useVaultPeeker() || {};
    
    const itemPadding = propPadding ?? contextPadding ?? DEFAULT_ITEM_PADDING;
    const itemSize = ITEM_BASE_SIZE + (2 * itemPadding);

    // State for rendered item images
    const [renderedItems, setRenderedItems] = useState(new Map());
    const [hoveredIndex, setHoveredIndex] = useState(null);
    const [isLoading, setIsLoading] = useState(true);

    // Determine empty slot image based on theme
    const emptySlotSrc = useMemo(() => {
        if (emptySlotImage) return emptySlotImage;
        return theme.palette.mode === 'dark' 
            ? '/realm/itemSlot.png' 
            : '/realm/itemSlot_light.png';
    }, [emptySlotImage, theme.palette.mode]);

    // Filter and prepare items for rendering
    const displayItems = useMemo(() => {
        if (!itemsData?.length) return [];
        
        return itemsData.map((item, index) => {
            const itemId = item.itemId ?? item.item_id ?? item;
            const isEmptySlot = itemId === -1;
            
            // Skip empty slots if not showing them
            if (isEmptySlot && !showEmptySlots) return null;

            return {
                index,
                itemId,
                count: item.count ?? 1,
                maxRarity: item.maxRarity ?? item.rarity ?? 0,
                isEmptySlot,
                parsedItem: item.parsedItem ?? item,
                originalData: item,
            };
        }).filter(Boolean);
    }, [itemsData, showEmptySlots]);

    // Render all items asynchronously
    useEffect(() => {
        if (!displayItems.length) {
            setRenderedItems(new Map());
            setIsLoading(false);
            return;
        }

        setIsLoading(true);
        const newRenderedItems = new Map();
        let completedCount = 0;
        const totalItems = displayItems.length;

        displayItems.forEach(async (displayItem) => {
            const { itemId, maxRarity, isEmptySlot, index } = displayItem;

            try {
                let imageSrc;
                
                if (isEmptySlot) {
                    imageSrc = emptySlotSrc;
                } else {
                    const itemData = items[itemId];
                    if (itemData) {
                        imageSrc = await drawItemAsync("renders.png", itemData, maxRarity, itemPadding);
                    }
                }

                if (imageSrc) {
                    newRenderedItems.set(index, imageSrc);
                }
            } catch (error) {
                console.error(`Failed to render item ${itemId}:`, error);
            }

            completedCount++;
            
            // Update state periodically to show progress
            if (completedCount === totalItems || completedCount % 50 === 0) {
                setRenderedItems(new Map(newRenderedItems));
            }
            
            if (completedCount === totalItems) {
                setIsLoading(false);
            }
        });

        return () => {
            // Cleanup if component unmounts during rendering
        };
    }, [displayItems, itemPadding, emptySlotSrc]);

    // Handle item click
    const handleItemClick = useCallback((displayItem, event) => {
        if (!onItemClick) return;
        
        const rect = event.currentTarget.getBoundingClientRect();
        const position = {
            top: rect.top,
            left: rect.left + rect.width / 2,
            isLeftHalf: rect.left < window.innerWidth / 2,
        };
        
        onItemClick(displayItem.itemId, position, displayItem.originalData);
    }, [onItemClick]);

    // Show loading state
    if (isLoading && displayItems.length > 0 && renderedItems.size === 0) {
        return (
            <Box
                ref={containerRef}
                sx={{
                    display: columns ? 'grid' : 'flex',
                    ...(columns ? {
                        gridTemplateColumns: `repeat(${columns}, ${itemSize}px)`,
                        gridAutoRows: `${itemSize}px`,
                    } : {
                        flexWrap: 'wrap',
                    }),
                    gap: 0,
                    minHeight: itemSize,
                }}
            >
                {displayItems.slice(0, columns || 20).map((_, i) => (
                    <Skeleton
                        key={i}
                        variant="rectangular"
                        width={itemSize}
                        height={itemSize}
                        sx={{ borderRadius: 0.5 }}
                    />
                ))}
            </Box>
        );
    }

    if (!displayItems.length) {
        return null;
    }

    return (
        <Box
            ref={containerRef}
            sx={{
                display: columns ? 'grid' : 'flex',
                ...(columns ? {
                    gridTemplateColumns: `repeat(${columns}, ${itemSize}px)`,
                    gridAutoRows: `${itemSize}px`,
                } : {
                    flexWrap: 'wrap',
                }),
                gap: 0,
                position: 'relative',
            }}
        >
            {displayItems.map((displayItem) => {
                const imageSrc = renderedItems.get(displayItem.index);
                const isHovered = hoveredIndex === displayItem.index;
                const showCount = showCounts && displayItem.count > 1 && !displayItem.isEmptySlot;

                const itemContent = items[displayItem.itemId];
                const tooltipElement = showTooltips && !displayItem.isEmptySlot && itemContent
                    ? <TooltipUiForItem item={itemContent} />
                    : null;

                const itemBox = (
                    <Box
                        key={displayItem.index}
                        onClick={(e) => handleItemClick(displayItem, e)}
                        onMouseEnter={() => setHoveredIndex(displayItem.index)}
                        onMouseLeave={() => setHoveredIndex(null)}
                        sx={{
                            width: itemSize,
                            height: itemSize,
                            position: 'relative',
                            cursor: onItemClick ? 'pointer' : 'default',
                            transition: 'transform 0.1s ease-in-out',
                            transform: isHovered ? 'scale(1.05)' : 'scale(1)',
                            zIndex: isHovered ? 10 : 1,
                            '&:hover': {
                                '& .item-hover-overlay': {
                                    opacity: 1,
                                },
                            },
                        }}
                    >
                        {/* Item Image */}
                        {imageSrc ? (
                            <img
                                src={imageSrc}
                                alt={`Item ${displayItem.itemId}`}
                                width={itemSize}
                                height={itemSize}
                                style={{
                                    display: 'block',
                                    imageRendering: 'pixelated',
                                }}
                                draggable={false}
                            />
                        ) : (
                            <Skeleton
                                variant="rectangular"
                                width={itemSize}
                                height={itemSize}
                                sx={{ borderRadius: 0.5 }}
                            />
                        )}

                        {/* Hover Overlay */}
                        <Box
                            className="item-hover-overlay"
                            sx={{
                                position: 'absolute',
                                top: 0,
                                left: 0,
                                right: 0,
                                bottom: 0,
                                backgroundColor: 'rgba(255, 255, 255, 0.15)',
                                borderRadius: 0.5,
                                opacity: 0,
                                transition: 'opacity 0.1s ease-in-out',
                                pointerEvents: 'none',
                            }}
                        />

                        {/* Item Count Badge */}
                        {showCount && (
                            <Typography
                                variant="caption"
                                sx={{
                                    position: 'absolute',
                                    bottom: itemPadding + 1,
                                    right: itemPadding + 1,
                                    backgroundColor: 'rgba(0, 0, 0, 0.7)',
                                    color: 'white',
                                    px: 0.5,
                                    py: 0,
                                    borderRadius: 0.5,
                                    fontSize: '0.65rem',
                                    fontWeight: 'bold',
                                    lineHeight: 1.2,
                                    minWidth: '14px',
                                    textAlign: 'center',
                                    pointerEvents: 'none',
                                }}
                            >
                                {displayItem.count > 999 ? '999+' : displayItem.count}
                            </Typography>
                        )}
                    </Box>
                );

                return tooltipElement
                    ? <Tooltip key={displayItem.index} title={tooltipElement}>{itemBox}</Tooltip>
                    : itemBox;
            })}
        </Box>
    );
}

export default ItemGridV2;
