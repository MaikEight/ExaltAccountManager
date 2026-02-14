import { useMemo } from "react";
import { Chip } from "@mui/material";
import useVaultPeeker from "../../../hooks/useVaultPeeker";
import MultiSelectFilter from "./MultiSelectFilter";

/**
 * ItemTypeFilterV2 - Multi-select item type (slot) filter
 * Allows selecting multiple item types at once
 */
function ItemTypeFilterV2() {
    const { filter, changeFilter, slotMapFilter } = useVaultPeeker();

    // Build options from slotMapFilter (excluding 'all')
    const typeOptions = useMemo(() => {
        return Object.keys(slotMapFilter)
            .filter(key => key !== 'all')
            .map(key => ({
                value: key,
                name: slotMapFilter[key].name,
                slotType: slotMapFilter[key].slotType,
            }));
    }, [slotMapFilter]);

    // Get selected values from filter
    const selectedValues = useMemo(() => {
        return filter.itemType.values || [];
    }, [filter.itemType.values]);

    // Handle selection change
    const handleChange = (newValues) => {
        changeFilter('itemType', {
            enabled: newValues.length > 0,
            values: newValues,
        });
    };

    // Custom chip renderer with dynamic text display
    const renderChip = (option, value, index) => {
        if (!option) return null;
        
        // Always show if only 1 selected
        if (selectedValues.length === 1) {
            return (
                <Chip
                    key={value}
                    label={option.name}
                    size="small"
                    sx={{
                        height: 20,
                        fontSize: '0.7rem',
                    }}
                    variant="outlined"
                />
            );
        }
        
        // For 2+ selections, check combined length
        const combinedLength = selectedValues.reduce((total, val) => {
            const opt = typeOptions.find(o => o.value === val);
            return total + (opt?.name?.length || 0);
        }, 0);
        
        // If combined length > 15 chars, show "x selected"
        if (combinedLength > 11) {
            if (index === 0) {
                return (
                    <Chip
                        key="count"
                        label={`${selectedValues.length} selected`}
                        size="small"
                        sx={{
                            height: 20,
                            fontSize: '0.7rem',
                        }}
                        variant="outlined"
                    />
                );
            }
            return null;
        }
        
        // Otherwise show individual chips
        return (
            <Chip
                key={value}
                label={option.name}
                size="small"
                sx={{
                    height: 20,
                    fontSize: '0.7rem',
                }}
                variant="outlined"
            />
        );
    };

    return (
        <MultiSelectFilter
            label="Type"
            options={typeOptions}
            selectedValues={selectedValues}
            onChange={handleChange}
            width={140}
            renderChip={renderChip}
        />
    );
}

export default ItemTypeFilterV2;
