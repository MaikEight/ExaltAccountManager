import { useMemo } from "react";
import { Box, Chip, Checkbox, ListItemText, MenuItem, Typography } from "@mui/material";
import useVaultPeeker from "../../../hooks/useVaultPeeker";
import MultiSelectFilter from "./MultiSelectFilter";
import { RARITY_IMAGE_SOURCES } from "../../../utils/realmItemDrawUtils";

/**
 * RarityFilter - Multi-select rarity filter
 * Allows filtering items by enchantment rarity (Common to Divine)
 */
function RarityFilter() {
    const { filter, changeFilter, rarityFilterOptions } = useVaultPeeker();

    // Get selected values from filter
    const selectedValues = useMemo(() => {
        return filter.rarity?.values || [];
    }, [filter.rarity?.values]);

    // Handle selection change
    const handleChange = (newValues) => {
        changeFilter('rarity', {
            enabled: newValues.length > 0,
            values: newValues,
        });
    };

    // Custom chip renderer with rarity images
    const renderChip = (option, value, index) => {
        if (!option) return null;
        
        // If more than 3 selected, show "x selected" only once
        if (selectedValues.length > 3) {
            if (index === 0) {
                return (
                    <Chip
                        key="count"
                        label={`${selectedValues.length} selected`}
                        size="small"
                        sx={{
                            height: 22,
                            fontSize: '0.7rem',
                        }}
                        variant="outlined"
                    />
                );
            }
            return null;
        }
        
        const raritySource = RARITY_IMAGE_SOURCES[option.value];
        const showTextInChip = selectedValues.length === 1;
        
        return (
            <Chip
                key={value}
                label={
                    <Box sx={{ display: 'flex', alignItems: 'center', gap: 0.5 }}>
                        {raritySource?.source && (
                            <Box
                                component="img"
                                src={raritySource.source}
                                alt={option.name}
                                sx={{
                                    width: raritySource.width,
                                    height: raritySource.height,
                                    imageRendering: 'pixelated',
                                }}
                            />
                        )}
                        {showTextInChip && <span>{option.name}</span>}
                    </Box>
                }
                size="small"
                sx={{
                    height: 22,
                    fontSize: '0.7rem',
                    borderColor: option.color,
                    color: option.color,
                }}
                variant="outlined"
            />
        );
    };

    // Custom menu item renderer with rarity images
    const renderOption = (option, isSelected) => {
        const raritySource = RARITY_IMAGE_SOURCES[option.value];
        
        return (
            <MenuItem key={option.value} value={option.value} dense>
                <Checkbox checked={isSelected} size="small" />
                <ListItemText
                    primary={
                        <Box sx={{ display: 'flex', alignItems: 'center', gap: 1 }}>
                            {raritySource?.source ? (
                                <Box
                                    component="img"
                                    src={raritySource.source}
                                    alt={option.name}
                                    sx={{
                                        width: raritySource.width,
                                        height: raritySource.height,
                                        imageRendering: 'pixelated',
                                    }}
                                />
                            ) : (
                                <Box
                                    sx={{
                                        width: 8,
                                        height: 8,
                                        borderRadius: '50%',
                                        backgroundColor: option.color,
                                    }}
                                />
                            )}
                            <Typography
                                variant="body2"
                                sx={{ color: option.color }}
                            >
                                {option.name}
                            </Typography>
                        </Box>
                    }
                />
            </MenuItem>
        );
    };

    return (
        <MultiSelectFilter
            label="Rarity"
            options={rarityFilterOptions}
            selectedValues={selectedValues}
            onChange={handleChange}
            width={140}
            menuWidth={180}
            renderChip={renderChip}
            renderOption={renderOption}
        />
    );
}

export default RarityFilter;
