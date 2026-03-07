import { useMemo, useCallback, useState, useEffect } from "react";
import { Box, Divider, Typography, IconButton, Tooltip, Chip } from "@mui/material";
import { useTheme } from "@emotion/react";
import FilterListOutlinedIcon from '@mui/icons-material/FilterListOutlined';
import ClearIcon from '@mui/icons-material/Clear';
import ComponentBox from "../../ComponentBox";
import Searchbar from "../../GridComponents/Searchbar";
import useVaultPeeker from "../../../hooks/useVaultPeeker";
import useUserSettings from "../../../hooks/useUserSettings";
import FilterPresetSelector from "./FilterPresetSelector";

// V2 multi-select filter components
import TierFilterV2 from "../Filter/TierFilterV2";
import ItemTypeFilterV2 from "../Filter/ItemTypeFilterV2";
import RarityFilter from "../Filter/RarityFilter";

// Reuse existing filter components
import SoulboundFilter from "../Filter/SoulboundFilter";
import FeedPowerFilter from "../Filter/FeedPowerFilter";
import CharacterSeasonalTypeFilter from "../Filter/CharacterSeasonalTypeFilter";

/**
 * Filter overview showing active filter count
 */
function FilterOverviewV2() {
    const { filter, resetAllFilters } = useVaultPeeker();

    const activeFilterCount = useMemo(() => {
        let count = 0;
        if (filter.search?.enabled) count++;
        if (filter.tier?.enabled) count++;
        if (filter.soulbound?.enabled) count++;
        if (filter.feedPower?.enabled) count++;
        if (filter.itemType?.enabled) count++;
        if (filter.characterType?.enabled) count++;
        if (filter.rarity?.enabled) count++;
        return count;
    }, [filter]);

    if (activeFilterCount === 0) return null;

    return (
        <Box sx={{ display: 'flex', alignItems: 'center', gap: 0.5 }}>
            <Chip
                label={`${activeFilterCount} active`}
                size="small"
                color="primary"
                variant="outlined"
                sx={{ fontSize: '0.75rem' }}
            />
            <Tooltip title="Clear all filters">
                <IconButton size="small" onClick={resetAllFilters}>
                    <ClearIcon fontSize="small" />
                </IconButton>
            </Tooltip>
        </Box>
    );
}

/**
 * ItemFilterBoxV2 - Filter controls for Vault Peeker V2
 * 
 * Features:
 * - Search with debouncing (handled by Searchbar component)
 * - Tier filter (T0-TXX, UT, ST)
 * - Soulbound/Tradeable filter
 * - Feed power filter
 * - Item type (slot) filter
 * - Character type filter (all, seasonal, normal, not on character)
 * - Filter presets (save, load, delete)
 */
function ItemFilterBoxV2() {
    const { filter, changeFilter } = useVaultPeeker();
    const collapsedFields = useUserSettings().getByKeyAndSubKey('vaultPeeker', 'collapsedFileds');

    // Debounced search handler
    const [searchDebounce, setSearchDebounce] = useState(null);
    
    const searchChanged = useCallback((search) => {
        // Clear previous debounce timer
        if (searchDebounce) {
            clearTimeout(searchDebounce);
        }
        
        // Set new debounced update (300ms)
        const timer = setTimeout(() => {
            changeFilter('search', {
                enabled: search && search !== '',
                value: search || '',
            });
        }, 300);
        
        setSearchDebounce(timer);
    }, [searchDebounce, changeFilter]);

    // Cleanup debounce timer on unmount
    useEffect(() => {
        return () => {
            if (searchDebounce) {
                clearTimeout(searchDebounce);
            }
        };
    }, [searchDebounce]);

    return (
        <ComponentBox
            title={
                <Box
                    sx={{
                        display: 'flex',
                        justifyContent: 'space-between',
                        alignItems: 'center',
                        width: '100%',
                        gap: 1,
                    }}
                >
                    <Typography
                        variant="h6"
                        sx={{
                            fontWeight: 600,
                            textAlign: 'center',
                        }}
                    >
                        Filter
                    </Typography>
                    
                    {/* Filter Overview */}
                    <FilterOverviewV2 />
                    
                    {/* Spacer */}
                    <Box sx={{ flex: 1 }} />
                    
                    {/* Filter Preset Selector */}
                    <Box onClick={(e) => e.stopPropagation()}>
                        <FilterPresetSelector />
                    </Box>
                    
                    {/* Searchbar */}
                    <Box onClick={(e) => e.stopPropagation()}>
                        <Searchbar
                            onSearchChanged={searchChanged}
                            initialValue={filter.search?.value || ''}
                        />
                    </Box>
                </Box>
            }
            icon={<FilterListOutlinedIcon />}
            isCollapseable={true}
            defaultCollapsed={collapsedFields !== undefined ? collapsedFields.filter : true}
            sx={{
                mb: 0,
            }}
            innerSx={{
                display: 'flex',
                flexDirection: 'row',
                flexWrap: 'wrap',
                gap: 1,
                alignItems: 'end',
            }}
        >
            {/* Tier Filter (Multi-select) */}
            <TierFilterV2 />
            
            <Divider
                flexItem
                orientation="vertical"
                sx={{ mt: 'auto', mb: '6px', height: '27px', width: '1px' }}
            />
            
            {/* Item Type Filter (Multi-select) */}
            <ItemTypeFilterV2 />
            
            <Divider
                flexItem
                orientation="vertical"
                sx={{ mt: 'auto', mb: '6px', height: '27px', width: '1px' }}
            />
            
            {/* Rarity Filter (Multi-select) */}
            <RarityFilter />
            
            <Divider
                flexItem
                orientation="vertical"
                sx={{ mt: 'auto', mb: '6px', height: '27px', width: '1px' }}
            />
            
            {/* Feed Power Filter */}
            <FeedPowerFilter />
            
            <Divider
                flexItem
                orientation="vertical"
                sx={{ mt: 'auto', mb: '6px', height: '27px', width: '1px' }}
            />
            
            {/* Soulbound Filter */}
            <SoulboundFilter />
            
            <Divider
                flexItem
                orientation="vertical"
                sx={{ mt: 'auto', mb: '6px', height: '27px', width: '1px' }}
            />
            
            {/* Character Type Filter */}
            <CharacterSeasonalTypeFilter />
        </ComponentBox>
    );
}

export default ItemFilterBoxV2;
