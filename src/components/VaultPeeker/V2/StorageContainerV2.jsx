import { useState, useMemo, useCallback } from "react";
import { Box, Button, Dialog, DialogContent, DialogTitle, IconButton, Typography } from "@mui/material";
import { useTheme } from "@emotion/react";
import CloseIcon from '@mui/icons-material/Close';
import OpenInFullIcon from '@mui/icons-material/OpenInFull';
import ItemCanvasGridV2 from "./ItemCanvasGridV2";
import useVaultPeeker from "../../../hooks/useVaultPeeker";

/**
 * StorageModalV2 - Full-screen modal for viewing all items in a storage container
 */
function StorageModalV2({ open, onClose, title, itemEntries, icon }) {
    const theme = useTheme();
    const { selectItem } = useVaultPeeker();

    const handleItemClick = useCallback((itemId, data, event) => {
        if (!event) return;
        const rect = event.currentTarget.getBoundingClientRect();
        const isLeftHalf = rect.left < window.innerWidth / 2;
        const position = {
            top: rect.top,
            left: isLeftHalf ? rect.right + 5 : rect.left - 5,
            isLeftHalf,
        };
        selectItem(itemId, position, data.itemData || data);
    }, [selectItem]);

    return (
        <Dialog
            open={open}
            onClose={onClose}
            maxWidth={false}
            slotProps={{
                paper: {
                    sx: {
                        width: '85vw',
                        height: '85vh',
                        maxWidth: '85vw',
                        maxHeight: '85vh',
                        backgroundColor: 'background.default',
                        border: '1px solid',
                        borderColor: theme.palette.divider,
                    },
                },
            }}
        >
            <DialogTitle
                component={Box}
                sx={{
                    display: 'flex',
                    alignItems: 'center',
                    gap: 1,
                    borderBottom: '1px solid',
                    borderColor: 'divider',
                    backgroundColor: 'background.default',
                }}
            >
                {icon && (
                    <img src={icon} alt={title} width={24} height={24} style={{ imageRendering: 'pixelated' }} />
                )}
                <Typography variant="h6" sx={{ flex: 1 }}>
                    {title}
                </Typography>
                <Typography variant="body2" color="text.secondary">
                    {itemEntries.length} items
                </Typography>
                <IconButton onClick={onClose} size="small">
                    <CloseIcon />
                </IconButton>
            </DialogTitle>
            <DialogContent sx={{ p: 2, overflow: 'auto', backgroundColor: 'background.default' }}>
                <ItemCanvasGridV2
                    itemEntries={itemEntries}
                    onItemClick={handleItemClick}
                    showCounts={false}
                    maxHeight={5000}
                    minColumns={8}
                />
            </DialogContent>
        </Dialog>
    );
}

// Storage type configurations
const STORAGE_CONFIG = {
    vault: {
        title: 'Vault',
        icon: '/realm/vault_portal.png',
    },
    gifts: {
        title: 'Gift Chest',
        icon: '/realm/gift_chest.png',
    },
    material_storage: {
        title: 'Material Storage',
        icon: '/realm/material_storage.png',
    },
    temporary_gifts: {
        title: 'Seasonal Spoils',
        icon: '/realm/seasonal_spoils_chest.png',
    },
    potions: {
        title: 'Potion Storage',
        icon: '/realm/potion_storage_small.png',
    },
};

/**
 * StorageContainerV2 - Collapsible storage container preview with "View All" button
 * 
 * @param {Object} props
 * @param {string} props.storageType - Type of storage ('vault', 'gifts', etc.)
 * @param {Array} props.items - Array of ParsedItem objects
 * @param {number} props.previewCount - Number of items to show in preview (default 16)
 */
function StorageContainerV2({ storageType, items = [], previewCount = 16 }) {
    const theme = useTheme();
    const { selectItem } = useVaultPeeker();
    const [modalOpen, setModalOpen] = useState(false);

    const config = STORAGE_CONFIG[storageType] || {
        title: storageType,
        icon: null,
    };

    // Prepare item entries for ItemCanvasGridV2: [itemId, { count, maxRarity, itemData }]
    const itemEntries = useMemo(() => {
        return items
            .filter((item) => item.item_id !== -1) // Filter out empty slots
            .map((item) => [
                item.item_id,
                {
                    count: 1,
                    maxRarity: item.enchant_ids?.length ? Math.min(4, item.enchant_ids.length) : 0,
                    itemData: item,
                },
            ]);
    }, [items]);

    // Preview entries (first N items)
    const previewEntries = useMemo(() => {
        return itemEntries.slice(0, previewCount);
    }, [itemEntries, previewCount]);

    // Count non-empty items
    const nonEmptyCount = itemEntries.length;

    const handleItemClick = useCallback((itemId, data, event) => {
        if (!event) return;
        const rect = event.currentTarget.getBoundingClientRect();
        const isLeftHalf = rect.left < window.innerWidth / 2;
        const position = {
            top: rect.top,
            left: isLeftHalf ? rect.right + 5 : rect.left - 5,
            isLeftHalf,
        };
        selectItem(itemId, position, data.itemData || data);
    }, [selectItem]);

    // Don't render if no items
    if (!items.length || nonEmptyCount === 0) {
        return null;
    }

    const hasMoreItems = itemEntries.length > previewCount;

    return (
        <>
            <Box
                sx={{
                    display: 'flex',
                    flexDirection: 'column',
                    gap: 0.5,
                    p: 1,
                    backgroundColor: theme.palette.background.default,
                    borderRadius: 1,
                    border: '1px solid',
                    borderColor: theme.palette.divider,
                }}
            >
                {/* Header */}
                <Box
                    sx={{
                        display: 'flex',
                        alignItems: 'center',
                        gap: 1,
                    }}
                >
                    {config.icon && (
                        <img
                            src={config.icon}
                            alt={config.title}
                            width={20}
                            height={20}
                            style={{ imageRendering: 'pixelated' }}
                        />
                    )}
                    <Typography variant="body2" fontWeight="bold" sx={{ flex: 1 }}>
                        {config.title}
                    </Typography>
                    <Typography variant="caption" color="text.secondary">
                        {nonEmptyCount} items
                    </Typography>
                    {hasMoreItems && (
                        <Button
                            size="small"
                            variant="text"
                            onClick={() => setModalOpen(true)}
                            startIcon={<OpenInFullIcon sx={{ fontSize: 14 }} />}
                            sx={{
                                minWidth: 'auto',
                                px: 1,
                                py: 0,
                                fontSize: '0.75rem',
                            }}
                        >
                            View All
                        </Button>
                    )}
                </Box>

                {/* Preview Grid */}
                <ItemCanvasGridV2
                    itemEntries={previewEntries}
                    onItemClick={handleItemClick}
                    showCounts={false}
                    maxHeight={500}
                    minColumns={4}
                />

                {/* "More items" indicator */}
                {hasMoreItems && (
                    <Typography
                        variant="caption"
                        color="text.secondary"
                        sx={{ textAlign: 'center', cursor: 'pointer' }}
                        onClick={() => setModalOpen(true)}
                    >
                        + {itemEntries.length - previewCount} more items
                    </Typography>
                )}
            </Box>

            {/* Full View Modal */}
            <StorageModalV2
                open={modalOpen}
                onClose={() => setModalOpen(false)}
                title={config.title}
                itemEntries={itemEntries}
                icon={config.icon}
            />
        </>
    );
}

export default StorageContainerV2;
export { StorageModalV2, STORAGE_CONFIG };
