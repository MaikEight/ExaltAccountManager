import { useCallback, useEffect, useMemo, useState } from "react";
import { Box, Chip, Skeleton, Tooltip, Typography } from "@mui/material";
import { useTheme } from "@emotion/react";
import { classes } from "../../../assets/constants";
import { useColorList } from "../../../hooks/useColorList";
import { isCrucibleActive, isStatMaxed, getXof8OfCharacter } from "../../../utils/realmCharacterUtils";
import CharacterPortrait from "../../Realm/CharacterPortrait";
import ItemGridV2 from "./ItemGridV2";
import useVaultPeeker from "../../../hooks/useVaultPeeker";
import useUserSettings from "../../../hooks/useUserSettings";

/**
 * CharacterCardV2 - Displays a single character with equipment, stats, and inventory
 * 
 * Layout:
 * - Row 1: Portrait | className & level | character stats | x/8 & Fame
 * - Row 2: Seasonal Chip | Crucible Chip (optional)
 * - Row 3: Equipment (with special background styling)
 * - Row 4: Inventory
 * - Row 5: Backpack (optional - first 8 slots)
 * - Row 6: Backpack Extender (optional - slots 9-16)
 * 
 * @param {Object} props
 * @param {Object} props.character - Character data from the account
 * @param {Function} props.onItemClick - Callback when an item is clicked
 */
function CharacterCardV2({ character, onItemClick }) {
    const theme = useTheme();
    const { filter, selectItem } = useVaultPeeker();
    const seasonalChipColor = useColorList(1);
    const crucibleChipColor = useColorList(3);
    const { getByKeyAndSubKey } = useUserSettings();
    const density = getByKeyAndSubKey('vaultPeeker', 'density') || 'comfortable';

    const CARD_WIDTH = useMemo(() => {
        switch (density) {
            case 'dense':
                return 346;
            case 'comfortable':
                return 380;
            case 'spacious':
                return 426;
            default:
                return 380;
        }
    }, [density]);

    // Check if this character should be shown based on filter
    const shouldShow = useMemo(() => {
        if (!filter.characterType.enabled) return true;
        const value = filter.characterType.value;
        if (value === 0) return true; // All
        if (value === 1) return character.seasonal; // Seasonal only
        if (value === 2) return !character.seasonal; // Normal only
        if (value === 3) return false; // Not on character - hide all
        return true;
    }, [filter.characterType, character.seasonal]);

    // Calculate stats
    const xof8 = useMemo(() => getXof8OfCharacter(character), [character]);
    const isActiveCrucible = useMemo(() => isCrucibleActive(character), [character]);

    // Get class info
    const charClass = useMemo(() => classes[character.char_class], [character.char_class]);
    const className = charClass?.[0] || 'Unknown';

    // Helper to map items (including empty slots)
    const mapItems = (itemArray) => {
        if (!itemArray) return [];
        return itemArray.map((item) => ({
            itemId: item.item_id,
            count: 1,
            maxRarity: item.enchant_ids?.length ? Math.min(4, item.enchant_ids.length) : 0,
            parsedItem: item,
        }));
    };

    // Prepare equipment items (4 slots)
    const equipmentItems = useMemo(() => mapItems(character.equipment), [character.equipment]);

    // Prepare inventory items (8 slots)
    const inventoryItems = useMemo(() => mapItems(character.inventory), [character.inventory]);

    // Prepare backpack items (first 8 slots)
    const backpackItems = useMemo(() => {
        if (!character.backpacks || character.backpacks.length === 0) return [];
        return mapItems(character.backpacks.slice(0, 8));
    }, [character.backpacks]);

    // Prepare backpack extender items (slots 9-16)
    const backpackExtenderItems = useMemo(() => {
        if (!character.backpacks || character.backpacks.length <= 8) return [];
        return mapItems(character.backpacks.slice(8, 16));
    }, [character.backpacks]);

    // Handle item click
    const handleItemClick = useCallback((itemId, position, itemData) => {
        if (onItemClick) {
            onItemClick(itemId, position, itemData);
        } else {
            selectItem(itemId, position, itemData);
        }
    }, [onItemClick, selectItem]);

    // Get stat display
    const getStatUI = (statName, statValue, isMax) => (
        <Box
            sx={{
                display: 'flex',
                flexDirection: 'row',
                alignItems: 'center',
                justifyContent: 'flex-end',
                gap: 0.5,
            }}
        >
            <Typography fontWeight={400} variant="caption" color="text.secondary">
                {statName}:
            </Typography>
            <Typography fontWeight={400} variant="caption" color={isMax ? "warning.main" : "text.primary"}>
                {statValue}
            </Typography>
        </Box>
    );

    if (!shouldShow) {
        return null;
    }

    const hasEquipment = equipmentItems.length > 0;
    const hasInventory = inventoryItems.length > 0;
    const hasBackpack = backpackItems.length > 0;
    const hasBackpackExtender = backpackExtenderItems.length > 0;

    return (
        <Box
            sx={{
                display: 'flex',
                flexDirection: 'column',
                gap: 1,
                p: 1.5,
                backgroundColor: theme.palette.background.default,
                borderRadius: 1,
                border: '1px solid',
                borderColor: 'divider',
                width: CARD_WIDTH,
                minWidth: CARD_WIDTH,
                maxWidth: CARD_WIDTH,
            }}
        >
            {/* Row 1: Portrait | className & level | character stats */}
            <Box
                sx={{
                    display: 'flex',
                    flexDirection: 'row',
                    alignItems: 'center',
                    gap: 1,
                }}
            >
                {/* Character Portrait */}
                <CharacterPortrait
                    type={character.char_class}
                    skin={character.texture}
                    tex1={character.tex1}
                    tex2={character.tex2}
                    adjust={false}
                />

                {/* Class Name and Level */}
                <Box sx={{ display: 'flex', flexDirection: 'column', minWidth: 60 }}>
                    <Typography variant="body2" fontWeight="bold" noWrap>
                        {className}
                    </Typography>
                    <Typography variant="caption" color="text.secondary">
                        Lv. {character.level}
                    </Typography>
                </Box>

                {/* Stats Grid */}
                <Box
                    sx={{
                        display: 'grid',
                        gridTemplateColumns: 'repeat(4, 1fr)',
                        gridTemplateRows: 'repeat(2, auto)',
                        gap: 0.5,
                        flex: 1,
                    }}
                >
                    {getStatUI("HP", character.max_hit_points, isStatMaxed(character, "max_hit_points"))}
                    {getStatUI("MP", character.max_magic_points, isStatMaxed(character, "max_magic_points"))}
                    {getStatUI("ATK", character.attack, isStatMaxed(character, "attack"))}
                    {getStatUI("DEF", character.defense, isStatMaxed(character, "defense"))}
                    {getStatUI("SPD", character.speed, isStatMaxed(character, "speed"))}
                    {getStatUI("DEX", character.dexterity, isStatMaxed(character, "dexterity"))}
                    {getStatUI("VIT", character.hp_regen, isStatMaxed(character, "hp_regen"))}
                    {getStatUI("WIS", character.mp_regen, isStatMaxed(character, "mp_regen"))}
                </Box>
            </Box>

            {/* Row 2: Seasonal Chip | Crucible Chip | x/8 & Fame (at far right) */}
            <Box sx={{ display: 'flex', flexDirection: 'row', gap: 0.5, minHeight: 20, alignItems: 'center' }}>
                {character.seasonal && (
                    <Chip
                        sx={{ ...seasonalChipColor, height: 20 }}
                        label="Seasonal"
                        size="small"
                    />
                )}
                {isActiveCrucible && (
                    <Tooltip title={`Crucible active until ${new Date(character.crucible_active * 1000).toLocaleDateString()}`}>
                        <Chip
                            sx={{ ...crucibleChipColor, height: 20 }}
                            label="Crucible"
                            size="small"
                        />
                    </Tooltip>
                )}
                {/* Spacer */}
                <Box sx={{ flex: 1 }} />
                {/* X/8 and Fame */}
                <Box sx={{ display: 'flex', alignItems: 'center', gap: 1 }}>
                    <Typography variant="caption" fontWeight="bold" color={xof8 === 8 ? 'warning.main' : 'inherit'}>
                        {xof8}/8
                    </Typography>
                    <Box sx={{ display: 'flex', alignItems: 'center', gap: 0.25 }}>
                        <Typography variant="caption">
                            {character.current_fame}
                        </Typography>
                        <img src="/realm/fame.png" alt="Fame" width={12} height={12} />
                    </Box>
                </Box>
            </Box>

            {/* Row 3: Equipment (with special background styling, fit-content width) */}
            {hasEquipment && (
                <Box sx={{ display: 'flex', flexDirection: 'column', width: 'fit-content' }}>
                    <Typography variant="caption" color="text.secondary" sx={{ mb: 0.25 }}>
                        Equipment
                    </Typography>
                    <Box
                        sx={{
                            backgroundColor: theme.palette.background.paperLight,
                            borderRadius: 1,
                            border: '1px solid',
                            borderColor: theme.palette.divider,
                            p: 0.25,
                            ml: -0.375,
                        }}
                    >
                        <ItemGridV2
                            items={equipmentItems}
                            onItemClick={handleItemClick}
                            showCounts={false}
                            showEmptySlots={true}
                            columns={4}
                        />
                    </Box>
                </Box>
            )}

            {/* Row 4: Inventory */}
            {hasInventory && (
                <Box sx={{ display: 'flex', flexDirection: 'column' }}>
                    <Typography variant="caption" color="text.secondary" sx={{ mb: 0.25 }}>
                        Inventory
                    </Typography>
                    <ItemGridV2
                        items={inventoryItems}
                        onItemClick={handleItemClick}
                        showCounts={false}
                        showEmptySlots={true}
                        columns={8}
                    />
                </Box>
            )}

            {/* Row 5: Backpack (first 8 slots) */}
            {hasBackpack && (
                <Box sx={{ display: 'flex', flexDirection: 'column' }}>
                    <Typography variant="caption" color="text.secondary" sx={{ mb: 0.25 }}>
                        Backpack
                    </Typography>
                    <ItemGridV2
                        items={backpackItems}
                        onItemClick={handleItemClick}
                        showCounts={false}
                        showEmptySlots={true}
                        columns={8}
                    />
                </Box>
            )}

            {/* Row 6: Backpack Extender (slots 9-16) */}
            {hasBackpackExtender && (
                <Box sx={{ display: 'flex', flexDirection: 'column' }}>
                    <Typography variant="caption" color="text.secondary" sx={{ mb: 0.25 }}>
                        Backpack Extender
                    </Typography>
                    <ItemGridV2
                        items={backpackExtenderItems}
                        onItemClick={handleItemClick}
                        showCounts={false}
                        showEmptySlots={true}
                        columns={8}
                    />
                </Box>
            )}
        </Box>
    );
}

export default CharacterCardV2;
