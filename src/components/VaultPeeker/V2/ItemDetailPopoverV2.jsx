import { useMemo, useCallback } from "react";
import {
    Box,
    Popover,
    Typography,
    Divider,
    Chip,
    Tooltip,
    Table,
    TableBody,
    TableCell,
    TableContainer,
    TableHead,
    TableRow,
} from "@mui/material";
import { useTheme } from "@emotion/react";
import { useNavigate } from "react-router-dom";
import items, { enchantments } from "../../../assets/constants";
import useVaultPeeker from "../../../hooks/useVaultPeeker";
import ItemGridV2 from "./ItemGridV2";
import { RARITY_IMAGE_SOURCES } from "../../../utils/realmItemDrawUtils";

// Rarity colors
const RARITY_COLORS = {
    0: '#9e9e9e', // Common (grey)
    1: '#4caf50', // Uncommon (green)
    2: '#2196f3', // Rare (blue)
    3: '#9c27b0', // Legendary (purple)
    4: '#ff9800', // Divine (orange)
};

const RARITY_NAMES = {
    0: 'Common',
    1: 'Uncommon',
    2: 'Rare',
    3: 'Legendary',
    4: 'Divine',
};

// Storage type display config with names and images
const STORAGE_TYPE_CONFIG = {
    vault: { name: 'Vault', image: 'realm/vault_portal.png' },
    gifts: { name: 'Gift Chest', image: 'realm/gift_chest.png' },
    material_storage: { name: 'Material Storage', image: 'realm/material_storage.png' },
    temporary_gifts: { name: 'Seasonal Spoils', image: 'realm/seasonal_spoils_chest.png' },
    potions: { name: 'Potion Storage', image: 'realm/potion_storage_small.png' },
    character: { name: 'Character', image: 'realm/character.png' },
};

// Default fallback
const DEFAULT_STORAGE_IMAGE = 'realm/vault_portal.png';

// Slot types that cannot have rarity (consumables, eggs, etc.)
const NON_ENCHANTABLE_SLOT_TYPES = [10, 26];

/**
 * Get display config for a storage type
 */
const getStorageConfig = (storageTypeId) => {
    if (storageTypeId?.startsWith('char:')) {
        return STORAGE_TYPE_CONFIG.character;
    }
    return STORAGE_TYPE_CONFIG[storageTypeId] || { name: storageTypeId, image: DEFAULT_STORAGE_IMAGE };
};

/**
 * Get enchantment display info
 */
const getEnchantmentInfo = (enchantId) => {
    let enchant = "";
    try {
        enchant = enchantments[enchantId];
    } catch (e) {
        console.error("Error fetching enchantment info for ID:", enchantId, e);
        return { name: `Unknown (${enchantId})`, description: '' };
    }
    if (!enchant) return { name: `Unknown (${enchantId})`, description: '' };
    return {
        name: enchant[0] || `Enchantment ${enchantId}`,
        description: enchant[1] || '',
    };
};

/**
 * Aggregate locations by account for table display
 * Returns map of email -> { accountName, group, vault, gifts, material_storage, temporary_gifts, potions, character }
 */
const aggregateLocationsByAccount = (locations) => {
    if (!locations?.length) return [];

    const accountMap = new Map();

    locations.forEach((loc) => {
        const key = loc.email;
        if (!accountMap.has(key)) {
            accountMap.set(key, {
                email: loc.email,
                accountName: loc.accountName || loc.email,
                group: loc.group,
                vault: 0,
                gifts: 0,
                material_storage: 0,
                temporary_gifts: 0,
                potions: 0,
                character: 0,
            });
        }

        const account = accountMap.get(key);
        const storageType = loc.storageTypeId;

        if (storageType?.startsWith('char:')) {
            account.character++;
        } else if (storageType in account) {
            account[storageType]++;
        }
    });

    return Array.from(accountMap.values()).sort((a, b) => {
        const totalA = a.vault + a.gifts + a.material_storage + a.temporary_gifts + a.potions + a.character;
        const totalB = b.vault + b.gifts + b.material_storage + b.temporary_gifts + b.potions + b.character;
        return totalB - totalA;
    });
};

/**
 * LocationTable - Table showing item locations per account like ItemLocationPopper
 */
function LocationTable({ locations, onNavigate, maxHeight = 280 }) {
    const theme = useTheme();

    const aggregatedLocations = useMemo(() => {
        return aggregateLocationsByAccount(locations);
    }, [locations]);

    if (aggregatedLocations.length === 0) {
        return (
            <Typography variant="caption" color="text.secondary">
                No locations found
            </Typography>
        );
    }

    return (
        <TableContainer sx={{ maxHeight, overflowY: 'auto' }}>
            <Table
                stickyHeader
                size="small"
                sx={{
                    '& thead th': {
                        borderBottom: 'none',
                        backgroundColor: theme.palette.background.default,
                        py: 0.5,
                    },
                    '& tbody tr:last-child td, & tbody tr:last-child th': {
                        borderBottom: 'none',
                    },
                }}
            >
                <TableHead>
                    <TableRow>
                        <TableCell sx={{ pl: 0.5 }}>
                            <Typography fontSize="12px" fontWeight="bold">
                                Account
                            </Typography>
                        </TableCell>
                        <TableCell align="center" sx={{ p: 0.125 }}>
                            <Tooltip title="Vault">
                                <img src="realm/vault_portal.png" alt="Vault" style={{ maxWidth: 20, maxHeight: 20 }} />
                            </Tooltip>
                        </TableCell>
                        <TableCell align="center" sx={{ p: 0.125 }}>
                            <Tooltip title="Gift Chest">
                                <img src="realm/gift_chest.png" alt="Gift" style={{ maxWidth: 20, maxHeight: 20 }} />
                            </Tooltip>
                        </TableCell>
                        <TableCell align="center" sx={{ p: 0.125 }}>
                            <Tooltip title="Material Storage">
                                <img src="realm/material_storage.png" alt="Mat. Storage" style={{ maxWidth: 20, maxHeight: 20 }} />
                            </Tooltip>
                        </TableCell>
                        <TableCell align="center" sx={{ p: 0.125 }}>
                            <Tooltip title="Seasonal Spoils">
                                <img src="realm/seasonal_spoils_chest.png" alt="Ssnl. Spoils" style={{ maxWidth: 20, maxHeight: 20 }} />
                            </Tooltip>
                        </TableCell>
                        <TableCell align="center" sx={{ p: 0.125 }}>
                            <Tooltip title="Potion Storage">
                                <img src="realm/potion_storage_small.png" alt="Pot. Storage" style={{ maxWidth: 20, maxHeight: 20 }} />
                            </Tooltip>
                        </TableCell>
                        <TableCell align="center" sx={{ p: 0.125 }}>
                            <Tooltip title="Character">
                                <img src="realm/character.png" alt="Chars" style={{ maxWidth: 20, maxHeight: 20 }} />
                            </Tooltip>
                        </TableCell>
                    </TableRow>
                </TableHead>
                <TableBody>
                    {aggregatedLocations.map((account) => (
                        <Tooltip
                            title="Click to open Account Details"
                            placement="bottom-end"
                            key={account.email}
                        >
                            <TableRow
                                hover
                                sx={{
                                    cursor: 'pointer',
                                    userSelect: 'none',
                                }}
                                onClick={() => onNavigate?.(account.email)}
                            >
                                <TableCell
                                    sx={{
                                        borderBottom: 'none',
                                        textAlign: 'start',
                                        borderRadius: `${theme.shape.borderRadius}px 0 0 ${theme.shape.borderRadius}px`,
                                        p: 0.5,
                                        pl: 0.5,
                                    }}
                                >
                                    <Box sx={{ display: 'flex', alignItems: 'center', gap: 0.5 }}>
                                        {account.group && (
                                            <Box
                                                sx={{
                                                    width: 6,
                                                    height: 6,
                                                    borderRadius: '50%',
                                                    backgroundColor: account.group.color || theme.palette.primary.main,
                                                    flexShrink: 0,
                                                    mb: 0.25,
                                                }}
                                            />
                                        )}
                                        <Typography variant="caption" noWrap sx={{ maxWidth: 100 }}>
                                            {account.accountName}
                                        </Typography>
                                    </Box>
                                </TableCell>
                                <TableCell sx={{ borderBottom: 'none', textAlign: 'center', p: 0.125 }}>
                                    <Typography variant="caption">{account.vault || '-'}</Typography>
                                </TableCell>
                                <TableCell sx={{ borderBottom: 'none', textAlign: 'center', p: 0.125 }}>
                                    <Typography variant="caption">{account.gifts || '-'}</Typography>
                                </TableCell>
                                <TableCell sx={{ borderBottom: 'none', textAlign: 'center', p: 0.125 }}>
                                    <Typography variant="caption">{account.material_storage || '-'}</Typography>
                                </TableCell>
                                <TableCell sx={{ borderBottom: 'none', textAlign: 'center', p: 0.125 }}>
                                    <Typography variant="caption">{account.temporary_gifts || '-'}</Typography>
                                </TableCell>
                                <TableCell sx={{ borderBottom: 'none', textAlign: 'center', p: 0.125 }}>
                                    <Typography variant="caption">{account.potions || '-'}</Typography>
                                </TableCell>
                                <TableCell
                                    sx={{
                                        borderBottom: 'none',
                                        textAlign: 'center',
                                        borderRadius: `0 ${theme.shape.borderRadius}px ${theme.shape.borderRadius}px 0`,
                                        p: 0.125,
                                    }}
                                >
                                    <Typography variant="caption">{account.character || '-'}</Typography>
                                </TableCell>
                            </TableRow>
                        </Tooltip>
                    ))}
                </TableBody>
            </Table>
        </TableContainer>
    );
}

/**
 * EnchantVariantTooltip - Tooltip content for enchant variant hover
 */
function EnchantVariantTooltip({ variant, locations, onNavigate }) {
    const theme = useTheme();

    const enchantInfoList = useMemo(() => {
        if (!variant.enchantIds?.length) return [];
        return variant.enchantIds.map((id) => getEnchantmentInfo(id));
    }, [variant.enchantIds]);

    // Filter locations for this specific variant
    const variantLocations = useMemo(() => {
        if (!locations?.length) return [];
        return locations.filter(loc => {
            // Match locations that have the same enchantIds
            if (!loc.enchantIds && !variant.enchantIds?.length) return true;
            if (!loc.enchantIds || !variant.enchantIds) return false;
            if (loc.enchantIds.length !== variant.enchantIds.length) return false;
            return loc.enchantIds.every((id, idx) => id === variant.enchantIds[idx]);
        });
    }, [locations, variant.enchantIds]);

    const handleLocationClick = useCallback((e, loc) => {
        e.stopPropagation();
        if (onNavigate && loc.email) {
            onNavigate(loc.email);
        }
    }, [onNavigate]);

    return (
        <Box sx={{ p: 0.5, minWidth: variantLocations.length > 3 ? 340 : 280 }}>
            {/* Enchantments with descriptions */}
            {enchantInfoList.length > 0 && (
                <Box sx={{ mb: 1 }}>
                    <Typography variant="caption" fontWeight="bold" color="text.secondary">
                        Enchantments
                    </Typography>
                    {enchantInfoList.map((enchant, idx) => (
                        <Box key={idx} sx={{ mt: 0.5 }}>
                            <Typography variant="body2" fontWeight="bold">
                                {enchant.name}
                            </Typography>
                            {enchant.description && (
                                <Typography variant="caption" color="text.secondary" sx={{ display: 'block' }}>
                                    {enchant.description}
                                </Typography>
                            )}
                        </Box>
                    ))}
                </Box>
            )}

            {/* Locations - use table for more than 3 items */}
            {variantLocations.length > 0 && (
                <Box>
                    {
                        enchantInfoList.length > 0 &&
                        <Divider sx={{ my: 0.5 }} />
                    }
                    <Typography variant="caption" fontWeight="bold" color="text.secondary">
                        Locations ({variantLocations.length})
                    </Typography>
                    {variantLocations.length > 3 ? (
                        <Box sx={{ mt: 0.5 }}>
                            <LocationTable
                                locations={variantLocations}
                                onNavigate={onNavigate}
                                maxHeight={150}
                            />
                        </Box>
                    ) : (
                        <Box sx={{ mt: 0.5, maxHeight: 150, overflow: 'auto' }}>
                            {variantLocations.map((loc, idx) => {
                                const storageConfig = getStorageConfig(loc.storageTypeId);
                                const isCharacter = loc.storageTypeId?.startsWith('char:');
                                const charId = isCharacter ? loc.storageTypeId.split(':')[1] : null;

                                return (
                                    <Box
                                        key={idx}
                                        onClick={(e) => handleLocationClick(e, loc)}
                                        sx={{
                                            display: 'flex',
                                            alignItems: 'center',
                                            gap: 0.5,
                                            py: 0.25,
                                            px: 0.5,
                                            borderRadius: 0.5,
                                            cursor: 'pointer',
                                            '&:hover': {
                                                backgroundColor: theme.palette.action.hover,
                                            },
                                        }}
                                    >
                                        {loc.group && (
                                            <Box
                                                sx={{
                                                    width: 6,
                                                    height: 6,
                                                    borderRadius: '50%',
                                                    backgroundColor: loc.group.color || theme.palette.primary.main,
                                                    flexShrink: 0,
                                                    mb: 0.25,
                                                }}
                                            />
                                        )}
                                        <Typography variant="caption" sx={{ flex: 1 }}>
                                            {loc.accountName || loc.email}
                                        </Typography>
                                        {isCharacter && charId && (
                                            <Typography variant="caption" color="text.secondary" sx={{ fontSize: '0.65rem' }}>
                                                #{charId}
                                            </Typography>
                                        )}
                                        <Tooltip title={storageConfig.name}>
                                            <img
                                                src={storageConfig.image}
                                                alt={storageConfig.name}
                                                style={{ width: 16, height: 16, flexShrink: 0 }}
                                            />
                                        </Tooltip>
                                    </Box>
                                );
                            })}
                        </Box>
                    )}
                </Box>
            )}
        </Box>
    );
}

/**
 * EnchantVariantRow - Single enchant variant display with tooltip
 */
function EnchantVariantRow({ variant, locations, onNavigate }) {
    const theme = useTheme();
    const rarity = variant.rarity || 0;

    const enchantInfoList = useMemo(() => {
        if (!variant.enchantIds?.length) return [];
        return variant.enchantIds.map((id) => getEnchantmentInfo(id));
    }, [variant.enchantIds]);

    return (
        <Tooltip
            title={<EnchantVariantTooltip variant={variant} locations={locations} onNavigate={onNavigate} />}
            placement="right"
            arrow
            enterDelay={200}
            leaveDelay={0}
            slotProps={{
                tooltip: {
                    sx: {
                        bgcolor: 'background.paper',
                        color: 'text.primary',
                        border: '1px solid',
                        borderColor: 'divider',
                        boxShadow: theme.shadows[4],
                        maxWidth: 360,
                        '& .MuiTooltip-arrow': {
                            color: 'background.paper',
                            '&::before': {
                                border: '1px solid',
                                borderColor: 'divider',
                            },
                        },
                    },
                },
            }}
        >
            <Box
                sx={{
                    display: 'flex',
                    alignItems: 'flex-start',
                    gap: 1,
                    p: 0.5,
                    borderRadius: 0.5,
                    cursor: 'pointer',
                    '&:hover': {
                        backgroundColor: theme.palette.action.hover,
                    },
                }}
            >
                {/* Rarity indicator */}
                {RARITY_IMAGE_SOURCES[rarity]?.source ? (
                    <Box
                        component="img"
                        src={RARITY_IMAGE_SOURCES[rarity].source}
                        alt={RARITY_NAMES[rarity]}
                        sx={{
                            width: RARITY_IMAGE_SOURCES[rarity].width,
                            height: RARITY_IMAGE_SOURCES[rarity].height,
                            flexShrink: 0,
                            mt: 0.5,
                            imageRendering: 'pixelated',
                        }}
                    />
                ) : (
                    <Box
                        sx={{
                            width: 8,
                            height: 8,
                            borderRadius: '50%',
                            backgroundColor: RARITY_COLORS[rarity],
                            flexShrink: 0,
                            mt: 0.5,
                        }}
                    />
                )}

                {/* Enchant names - one per row */}
                <Box sx={{ flex: 1, minWidth: 0 }}>
                    {enchantInfoList.length > 0 ? (
                        enchantInfoList.map((enchant, idx) => (
                            <Typography
                                key={idx}
                                variant="caption"
                                sx={{
                                    display: 'block',
                                    lineHeight: 1.3,
                                    color: idx === 0 ? 'text.primary' : 'text.secondary',
                                }}
                            >
                                {enchant.name}
                            </Typography>
                        ))
                    ) : (
                        <Typography variant="caption" color="text.secondary">
                            No enchantments
                        </Typography>
                    )}
                </Box>

                {/* Count */}
                <Chip
                    label={`×${variant.count}`}
                    size="small"
                    sx={{
                        height: 18,
                        fontSize: '0.7rem',
                        backgroundColor: RARITY_COLORS[rarity],
                        color: '#fff',
                        flexShrink: 0,
                        my: 'auto',
                    }}
                />
            </Box>
        </Tooltip>
    );
}

/**
 * LocationRow - Single account/location display
 */
function LocationRow({ location, onNavigate }) {
    const theme = useTheme();
    const storageConfig = getStorageConfig(location.storageTypeId);
    const isCharacter = location.storageTypeId?.startsWith('char:');
    const charId = isCharacter ? location.storageTypeId.split(':')[1] : null;

    const handleClick = useCallback(() => {
        if (onNavigate && location.email) {
            onNavigate(location.email);
        }
    }, [onNavigate, location.email]);

    return (
        <Box
            onClick={handleClick}
            sx={{
                display: 'flex',
                alignItems: 'center',
                gap: 1,
                p: 0.5,
                borderRadius: 0.5,
                cursor: 'pointer',
                '&:hover': {
                    backgroundColor: theme.palette.action.hover,
                },
            }}
        >
            {/* Group color */}
            {location.group && (
                <Box
                    sx={{
                        width: 4,
                        height: 16,
                        borderRadius: 0.5,
                        backgroundColor: location.group.color || theme.palette.primary.main,
                        flexShrink: 0,
                    }}
                />
            )}

            {/* Account name */}
            <Typography variant="caption" sx={{ flex: 1 }}>
                {location.accountName || location.email}
            </Typography>

            {/* Character ID if applicable */}
            {isCharacter && charId && (
                <Typography variant="caption" color="text.secondary" sx={{ fontSize: '0.65rem' }}>
                    #{charId}
                </Typography>
            )}

            {/* Storage location image */}
            <Tooltip title={storageConfig.name}>
                <img
                    src={storageConfig.image}
                    alt={storageConfig.name}
                    style={{ width: 20, height: 20, flexShrink: 0 }}
                />
            </Tooltip>
        </Box>
    );
}

/**
 * ItemDetailPopoverV2 - Popover showing item details, enchant variants, and locations
 * 
 * Features:
 * - Item sprite, name, tier, feed power, soulbound status
 * - Enchant variants grouped by rarity
 * - Account locations with hover highlighting
 */
function ItemDetailPopoverV2() {
    const theme = useTheme();
    const navigate = useNavigate();
    const { selectedItem, clearSelectedItem, popperPosition, totalsMap } = useVaultPeeker();

    const open = Boolean(selectedItem);
    const itemId = selectedItem?.itemId;

    // Navigation handler for account links
    const handleNavigateToAccount = useCallback((email) => {
        navigate(`/accounts?selectedAccount=${encodeURIComponent(email)}`);
        clearSelectedItem();
    }, [navigate, clearSelectedItem]);

    // Create a virtual anchor element for positioning
    const virtualAnchor = useMemo(() => {
        if (!popperPosition) return null;
        return {
            getBoundingClientRect: () => ({
                top: popperPosition.top,
                left: popperPosition.left,
                right: popperPosition.left,
                bottom: popperPosition.top + 40, // approximate item height
                width: 0,
                height: 40,
            }),
        };
    }, [popperPosition]);

    // Get item data from constants
    const itemData = useMemo(() => {
        if (!itemId || itemId === -1) return null;
        return items[itemId] || null;
    }, [itemId]);

    // Get totals data for this item
    const itemTotals = useMemo(() => {
        if (!totalsMap || !itemId) return null;
        return totalsMap.get(itemId);
    }, [totalsMap, itemId]);

    // Group enchant variants by rarity
    const variantsByRarity = useMemo(() => {
        if (!itemTotals?.enchantVariants) return {};

        const grouped = {};
        for (const [key, variant] of itemTotals.enchantVariants) {
            const rarity = variant.rarity || 0;
            if (!grouped[rarity]) {
                grouped[rarity] = [];
            }
            grouped[rarity].push({ key, ...variant });
        }

        // Sort variants within each rarity by count descending
        Object.keys(grouped).forEach((rarity) => {
            grouped[rarity].sort((a, b) => b.count - a.count);
        });

        return grouped;
    }, [itemTotals]);

    // Get unique locations (deduplicate by email + storageTypeId)
    const uniqueLocations = useMemo(() => {
        if (!itemTotals?.locations) return [];

        const locationMap = new Map();
        itemTotals.locations.forEach((loc) => {
            const key = `${loc.email}-${loc.storageTypeId}`;
            if (!locationMap.has(key)) {
                locationMap.set(key, { ...loc, count: 1 });
            } else {
                locationMap.get(key).count++;
            }
        });

        return Array.from(locationMap.values()).sort((a, b) => b.count - a.count);
    }, [itemTotals]);

    const handleClose = useCallback(() => {
        clearSelectedItem();
    }, [clearSelectedItem]);

    if (!itemData) {
        return null;
    }

    // items[id] = [name, slotType, tier, feedPower, bagType, soulbound]
    const [name, slotType, tier, feedPower, bagType, soulbound] = itemData;

    // Determine if item can have rarity (enchantments)
    const canHaveRarity = !NON_ENCHANTABLE_SLOT_TYPES.includes(slotType);

    // For display, only show rarity section if item can have rarity
    const showEnchantVariants = canHaveRarity && Object.keys(variantsByRarity).length > 0;

    // Show location table for non-enchantable items, or for enchantable items without variants
    const showLocationTable = !canHaveRarity || (!showEnchantVariants && uniqueLocations.length > 0);

    // Tier display
    let tierDisplay = `T${tier}`;
    if (tier === -1) tierDisplay = 'UT';
    if (tier === -2) tierDisplay = 'ST';

    return (
        <Popover
            open={open}
            anchorEl={virtualAnchor}
            onClose={handleClose}
            anchorOrigin={{
                vertical: 'bottom',
                horizontal: 'center',
            }}
            transformOrigin={{
                vertical: 'top',
                horizontal: 'center',
            }}
            slotProps={{
                paper: {
                    sx: {
                        width: 400,
                        maxHeight: 450,
                        overflow: 'hidden',
                        border: '1px solid',
                        borderColor: 'divider',
                        borderRadius: 1,
                        p: 0.125
                    },
                },
            }}
        >
            <Box
                sx={{
                    display: 'flex',
                    flexDirection: 'column',
                    backgroundColor: 'background.default',
                    border: '1px solid',
                    borderColor: 'divider',
                    borderRadius: `${theme.shape.borderRadius - 2}px`,
                }}
            >
                {/* Header */}
                <Box
                    sx={{
                        display: 'flex',
                        alignItems: 'center',
                        gap: 1.5,
                        p: 1.5,
                        borderBottom: '1px solid',
                        borderColor: 'divider',
                    }}
                >
                    {/* Item sprite */}
                    <ItemGridV2
                        items={[{
                            itemId,
                            count: itemTotals?.count || 1,
                            maxRarity: itemTotals?.maxRarity || 0,
                        }]}
                        showCounts={true}
                    />

                    {/* Item info */}
                    <Box sx={{ flex: 1 }}>
                        <Typography variant="subtitle2" fontWeight="bold">
                            {name}
                        </Typography>
                        <Box sx={{ display: 'flex', gap: 0.5, flexWrap: 'wrap', mt: 0.5 }}>
                            <Chip
                                label={tierDisplay}
                                size="small"
                                color={tier === -1 ? 'primary' : tier === -2 ? 'warning' : 'default'}
                                sx={{ height: 18, fontSize: '0.7rem' }}
                            />
                            {feedPower > 0 && (
                                <Chip
                                    label={`${feedPower} FP`}
                                    size="small"
                                    variant="outlined"
                                    sx={{ height: 18, fontSize: '0.7rem' }}
                                />
                            )}
                            {soulbound === 1 && (
                                <Chip
                                    label="SB"
                                    size="small"
                                    color="error"
                                    variant="outlined"
                                    sx={{ height: 18, fontSize: '0.7rem' }}
                                />
                            )}
                        </Box>
                    </Box>

                    {/* Total count - only show if more than 1 */}
                    {(itemTotals?.count || 1) > 1 && (
                        <Typography variant="h6" color="text.secondary">
                            ×{itemTotals?.count}
                        </Typography>
                    )}
                </Box>

                {/* Scrollable content */}
                <Box sx={{ overflow: 'auto', flex: 1, maxHeight: 300 }}>
                    {/* Enchant Variants Section - only for enchantable items */}
                    {showEnchantVariants && (
                        <Box sx={{ p: 1.5 }}>
                            <Typography variant="caption" color="text.secondary" fontWeight="bold">
                                Enchant Variants
                            </Typography>

                            <Box sx={{ maxHeight: 220, overflow: 'auto' }}>
                                {Object.keys(variantsByRarity)
                                    .sort((a, b) => Number(b) - Number(a)) // Sort by rarity descending
                                    .map((rarity) => (
                                        <Box key={rarity} sx={{ mt: 0.5 }}>
                                            <Typography
                                                variant="caption"
                                                sx={{
                                                    color: RARITY_COLORS[rarity],
                                                    fontWeight: 'bold',
                                                    fontSize: '0.65rem',
                                                }}
                                            >
                                                {RARITY_NAMES[rarity]}
                                            </Typography>
                                            {variantsByRarity[rarity].map((variant) => (
                                                <EnchantVariantRow
                                                    key={variant.key}
                                                    variant={variant}
                                                    locations={itemTotals?.locations}
                                                    onNavigate={handleNavigateToAccount}
                                                />
                                            ))}
                                        </Box>
                                    ))}
                            </Box>
                        </Box>
                    )}

                    {/* Location Table Section - for non-enchantable items or items without variants */}
                    {showLocationTable && (
                        <>
                            <Divider />
                            <Box sx={{ p: 1.5 }}>
                                <Typography variant="caption" color="text.secondary" fontWeight="bold" sx={{ mb: 0.5, display: 'block' }}>
                                    Locations ({aggregateLocationsByAccount(itemTotals?.locations).length} accounts)
                                </Typography>
                                <LocationTable
                                    locations={itemTotals?.locations}
                                    onNavigate={handleNavigateToAccount}
                                    maxHeight={200}
                                />
                            </Box>
                        </>
                    )}
                </Box>
            </Box>
        </Popover>
    );
}

export default ItemDetailPopoverV2;
