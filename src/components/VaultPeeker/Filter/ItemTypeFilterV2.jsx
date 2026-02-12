import { useMemo } from "react";
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

    return (
        <MultiSelectFilter
            label="Type"
            options={typeOptions}
            selectedValues={selectedValues}
            onChange={handleChange}
            width={140}
        />
    );
}

export default ItemTypeFilterV2;
