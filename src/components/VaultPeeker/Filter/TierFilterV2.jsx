import { useMemo } from "react";
import { Chip } from "@mui/material";
import { useTheme } from "@emotion/react";
import useVaultPeeker from "../../../hooks/useVaultPeeker";
import items from "../../../assets/constants";
import MultiSelectFilter from "./MultiSelectFilter";

/**
 * TierFilterV2 - Multi-select tier filter
 * Allows selecting multiple tiers (T0-TXX, UT, ST)
 */
function TierFilterV2() {
    const { filter, changeFilter } = useVaultPeeker();
    const theme = useTheme();

    // Build tier options dynamically from items
    const tierOptions = useMemo(() => {
        const maxTier = Object.keys(items).reduce((max, itemId) => {
            const itemTier = items[itemId][2];
            return itemTier > max ? itemTier : max;
        }, 0);

        const options = [];
        
        // Add regular tiers (reversed so highest first)
        for (let i = maxTier; i >= 0; i--) {
            options.push({
                value: `t${i}`,
                tier: i,
                flag: 0,
                name: `T${i}`,
            });
        }
        
        // Add UT and ST at the end
        options.push({
            value: 'ut',
            tier: -1,
            flag: 1,
            name: 'UT',
            color: theme.palette.primary.main,
        });
        options.push({
            value: 'st',
            tier: -1,
            flag: 2,
            name: 'ST',
            color: theme.palette.warning.main,
        });

        return options;
    }, [theme.palette.primary.main, theme.palette.warning.main]);

    // Convert filter.tier.values to selectedValues (array of value strings)
    const selectedValues = useMemo(() => {
        if (!filter.tier.values || filter.tier.values.length === 0) return [];
        return filter.tier.values.map(v => {
            if (v.flag === 1) return 'ut';
            if (v.flag === 2) return 'st';
            return `t${v.tier}`;
        });
    }, [filter.tier.values]);

    // Handle selection change
    const handleChange = (newValues) => {
        const tierValues = newValues.map(val => {
            const option = tierOptions.find(opt => opt.value === val);
            return option ? { tier: option.tier, flag: option.flag, name: option.name } : null;
        }).filter(Boolean);

        changeFilter('tier', {
            enabled: tierValues.length > 0,
            values: tierValues,
        });
    };

    // Custom chip renderer for colored UT/ST
    const renderChip = (option, value, index) => {
        if (!option) return null;
        
        // If 3+ selected, show "x selected" only once
        if (selectedValues.length >= 3) {
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
        
        return (
            <Chip
                key={value}
                label={option.name}
                size="small"
                sx={{
                    height: 20,
                    fontSize: '0.7rem',
                    ...(option.color && {
                        borderColor: option.color,
                        color: option.color,
                    }),
                }}
                variant="outlined"
            />
        );
    };

    return (
        <MultiSelectFilter
            label="Tier"
            options={tierOptions}
            selectedValues={selectedValues}
            onChange={handleChange}
            width={120}
            renderChip={renderChip}
        />
    );
}

export default TierFilterV2;
