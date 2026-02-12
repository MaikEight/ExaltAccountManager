import { createContext, useCallback, useEffect, useMemo, useReducer, useState } from "react";
import { invoke } from "@tauri-apps/api/core";
import items from "../assets/constants";
import { useGroups } from 'eam-commons-js';
import useUserSettings from "../hooks/useUserSettings";
import useAccounts from "../hooks/useAccounts";
import useDebugLogs from "../hooks/useDebugLogs";

const VaultPeekerContext = createContext();

// ==================== CONSTANTS ====================

const FEED_POWER_FILTER_OPTIONS = [
    { feedPower: -1, name: 'None' },
    { feedPower: 1300, name: '1300+' },
    { feedPower: 1000, name: '1000' },
    { feedPower: 900, name: '900' },
    { feedPower: 800, name: '800' },
    { feedPower: 700, name: '700' },
    { feedPower: 650, name: '650' },
    { feedPower: 600, name: '600' },
    { feedPower: 550, name: '550' },
    { feedPower: 500, name: '500' },
    { feedPower: 450, name: '450' },
    { feedPower: 400, name: '400' },
    { feedPower: 300, name: '300' },
    { feedPower: 200, name: '200' },
];

const SLOT_MAP_FILTER = {
    all: { slotType: 0, name: 'All' },
    items: { slotType: 10, name: 'Items' },
    swords: { slotType: 1, name: 'Swords' },
    daggers: { slotType: 2, name: 'Daggers' },
    bows: { slotType: 3, name: 'Bows' },
    wands: { slotType: 8, name: 'Wands' },
    staves: { slotType: 17, name: 'Staves' },
    katanas: { slotType: 24, name: 'Katanas' },
    lightarmor: { slotType: 6, name: 'Light Armor' },
    heavyarmor: { slotType: 7, name: 'Heavy Armor' },
    robes: { slotType: 14, name: 'Robes' },
    rings: { slotType: 9, name: 'Rings' },
    tomes: { slotType: 4, name: 'Tomes' },
    shields: { slotType: 5, name: 'Shields' },
    spells: { slotType: 11, name: 'Spells' },
    seals: { slotType: 12, name: 'Seals' },
    cloaks: { slotType: 13, name: 'Cloaks' },
    quivers: { slotType: 15, name: 'Quivers' },
    helms: { slotType: 16, name: 'Helms' },
    poisons: { slotType: 18, name: 'Poisons' },
    skulls: { slotType: 19, name: 'Skulls' },
    traps: { slotType: 20, name: 'Traps' },
    orbs: { slotType: 21, name: 'Orbs' },
    prisms: { slotType: 22, name: 'Prisms' },
    scepters: { slotType: 23, name: 'Scepters' },
    stars: { slotType: 25, name: 'Stars' },
    wakis: { slotType: 27, name: 'Wakis' },
    lutes: { slotType: 28, name: 'Lutes' },
    maces: { slotType: 29, name: 'Maces' },
    sheaths: { slotType: 30, name: 'Sheaths' },
    eggs: { slotType: 26, name: 'Eggs' },
};

const DEFAULT_FILTER = {
    search: {
        enabled: false,
        value: '',
    },
    tier: {
        enabled: false,
        values: [], // Array of selected tiers: { tier, flag, name }
    },
    soulbound: {
        enabled: false,
        value: 2, // 2 = all, 0 = soulbound, 1 = tradeable
    },
    feedPower: {
        enabled: false,
        value: 0,
    },
    itemType: {
        enabled: false,
        values: [], // Array of selected item type keys
    },
    characterType: {
        enabled: false,
        value: 0, // 0 = all, 1 = seasonal, 2 = normal, 3 = not on character
    },
    rarity: {
        enabled: false,
        values: [], // Array of selected rarities: 0=common, 1=uncommon, 2=rare, 3=legendary, 4=divine
    },
};

// Rarity filter options (Divine to Common order)
const RARITY_FILTER_OPTIONS = [
    { value: 4, name: 'Divine', color: '#ff9800' },
    { value: 3, name: 'Legendary', color: '#9c27b0' },
    { value: 2, name: 'Rare', color: '#2196f3' },
    { value: 1, name: 'Uncommon', color: '#4caf50' },
    { value: 0, name: 'Common', color: '#9e9e9e' },
];

const ITEM_PADDING_OPTIONS = [0, 2, 5];

// Density to padding mapping
const DENSITY_TO_PADDING = {
    'dense': 0,
    'comfortable': 2,
    'spacious': 5,
};

const PADDING_TO_DENSITY = {
    0: 'dense',
    2: 'comfortable',
    5: 'spacious',
};

// ==================== REDUCER ====================

const filterReducer = (state, action) => {
    switch (action.type) {
        case 'SET_FILTER':
            return {
                ...state,
                [action.filterType]: action.value,
            };
        case 'RESET_FILTER':
            return {
                ...state,
                [action.filterType]: DEFAULT_FILTER[action.filterType],
            };
        case 'RESET_ALL':
            return DEFAULT_FILTER;
        case 'LOAD_PRESET':
            return action.preset;
        default:
            return state;
    }
};

// ==================== UTILITY FUNCTIONS ====================

/**
 * Calculate item rarity based on enchant_ids count
 * 0 = common, 1 = uncommon, 2 = rare, 3 = legendary, 4 = divine
 */
export const getItemRarity = (parsedItem) => {
    if (!parsedItem?.enchant_ids?.length) return 0;
    return Math.min(4, parsedItem.enchant_ids.length);
};

/**
 * Create a unique key for an enchanted item variant
 */
const getEnchantKey = (enchantIds) => {
    if (!enchantIds || enchantIds.length === 0) return 'none';
    return [...enchantIds].sort((a, b) => a - b).join(',');
};

/**
 * Aggregate items from all accounts into totals map
 * Returns: Map<itemId, { count, maxRarity, locations[], enchantVariants: Map<enchantKey, { count, enchantIds, rarity, locations[] }> }>
 */
const aggregateItems = (datasets, groups, getAccountByEmail) => {
    const totalsMap = new Map();
    let emptySlotCount = 0;

    datasets.forEach((dataset) => {
        const email = dataset.email;
        const eamAccount = getAccountByEmail?.(email);
        const group = eamAccount?.group ? groups?.find((g) => g.name === eamAccount?.group) : null;
        const accountName = eamAccount?.name || email;

        dataset.items.forEach((parsedItem) => {
            const itemId = parsedItem.item_id;

            // Count empty slots separately
            if (itemId === -1) {
                emptySlotCount++;
                return;
            }

            const rarity = getItemRarity(parsedItem);
            const enchantKey = getEnchantKey(parsedItem.enchant_ids);
            const location = {
                email,
                accountName,
                group,
                storageTypeId: parsedItem.storage_type_id,
                containerIndex: parsedItem.container_index,
                slotIndex: parsedItem.slot_index,
                enchantIds: parsedItem.enchant_ids,
                rarity,
            };

            if (!totalsMap.has(itemId)) {
                totalsMap.set(itemId, {
                    count: 0,
                    maxRarity: 0,
                    locations: [],
                    enchantVariants: new Map(),
                });
            }

            const itemData = totalsMap.get(itemId);
            itemData.count++;
            itemData.maxRarity = Math.max(itemData.maxRarity, rarity);
            itemData.locations.push(location);

            // Track enchant variants
            if (!itemData.enchantVariants.has(enchantKey)) {
                itemData.enchantVariants.set(enchantKey, {
                    count: 0,
                    enchantIds: parsedItem.enchant_ids || [],
                    rarity,
                    locations: [],
                });
            }
            const variant = itemData.enchantVariants.get(enchantKey);
            variant.count++;
            variant.locations.push(location);
        });
    });

    // Add empty slot placeholder at the end
    if (emptySlotCount > 0) {
        totalsMap.set(-1, {
            count: emptySlotCount,
            maxRarity: 0,
            locations: [],
            enchantVariants: new Map(),
        });
    }

    return totalsMap;
};

/**
 * Process datasets into account-specific data structure
 */
const processAccountsData = (datasets, groups, getAccountByEmail) => {
    return datasets.map((dataset) => {
        const email = dataset.email;
        const eamAccount = getAccountByEmail?.(email);
        const group = eamAccount?.group ? groups?.find((g) => g.name === eamAccount?.group) : null;

        // Group items by storage type
        const storageContainers = {
            vault: [],
            gifts: [],
            material_storage: [],
            temporary_gifts: [],
            potions: [],
        };
        const characterItems = new Map(); // charId -> items[]

        dataset.items.forEach((parsedItem) => {
            const storageTypeId = parsedItem.storage_type_id;

            if (storageTypeId.startsWith('char:')) {
                const charId = parseInt(storageTypeId.split(':')[1], 10);
                if (!characterItems.has(charId)) {
                    characterItems.set(charId, []);
                }
                characterItems.get(charId).push(parsedItem);
            } else if (storageContainers[storageTypeId] !== undefined) {
                storageContainers[storageTypeId].push(parsedItem);
            }
        });

        // Sort items within each container by slot_index
        Object.keys(storageContainers).forEach((key) => {
            storageContainers[key].sort((a, b) => {
                const containerDiff = (a.container_index || 0) - (b.container_index || 0);
                if (containerDiff !== 0) return containerDiff;
                return (a.slot_index || 0) - (b.slot_index || 0);
            });
        });

        // Process characters
        const characters = dataset.character.map((char) => {
            const charItems = characterItems.get(char.char_id) || [];

            // Sort and group character items
            const equipment = charItems
                .filter((i) => i.container_index === 0)
                .sort((a, b) => (a.slot_index || 0) - (b.slot_index || 0));

            const inventory = charItems
                .filter((i) => i.container_index === 1)
                .sort((a, b) => (a.slot_index || 0) - (b.slot_index || 0));

            const backpacks = charItems
                .filter((i) => i.container_index >= 2)
                .sort((a, b) => {
                    const containerDiff = (a.container_index || 0) - (b.container_index || 0);
                    if (containerDiff !== 0) return containerDiff;
                    return (a.slot_index || 0) - (b.slot_index || 0);
                });

            return {
                ...char,
                equipment,
                inventory,
                backpacks,
                allItems: charItems,
            };
        });

        return {
            email,
            name: eamAccount?.name || email,
            group,
            account: dataset.account,
            classStats: dataset.class_stats,
            characters,
            storageContainers,
            rawItems: dataset.items,
        };
    });
};

/**
 * Apply filters to items and return filtered item IDs
 */
const applyFiltersToItems = (totalsMap, filter, accountsData) => {
    const filteredItemIds = [];

    totalsMap.forEach((itemData, itemId) => {
        // Skip empty slot placeholder during filtering (we'll add it back at the end)
        if (itemId === -1) return;

        const item = items[itemId];
        if (!item) return;

        // TEXT SEARCH FILTER
        if (filter.search.enabled && filter.search.value !== '') {
            const searchTextLower = filter.search.value.toLowerCase();
            if (!item[0].toLowerCase().includes(searchTextLower)) {
                return;
            }
        }

        // TIER FILTER (multi-select)
        if (filter.tier.enabled && filter.tier.values.length > 0) {
            const tier = item[2];
            const flag = item[9];

            // Check if item matches any of the selected tier options
            const matchesTier = filter.tier.values.some(selectedTier => {
                // UT match (flag === 1)
                if (selectedTier.flag === 1) return flag === 1;
                // ST match (flag === 2)
                if (selectedTier.flag === 2) return flag === 2;
                // Regular tier match (flag === 0, compare tier value)
                if (selectedTier.flag === 0 && selectedTier.tier >= 0) {
                    return tier === selectedTier.tier && flag === 0;
                }
                return false;
            });
            if (!matchesTier) return;
        }

        // RARITY FILTER (multi-select)
        if (filter.rarity.enabled && filter.rarity.values.length > 0) {
            const maxRarity = itemData.maxRarity || 0;
            if (!filter.rarity.values.includes(maxRarity)) return;
        }

        // SOULBOUND FILTER
        if (filter.soulbound.enabled) {
            const { value } = filter.soulbound;
            const soulbound = item[8];
            if (value !== 2) {
                if (value === 0 && !soulbound) return;
                if (value === 1 && soulbound) return;
            }
        }

        // FEED POWER FILTER
        if (filter.feedPower.enabled) {
            const { value } = filter.feedPower;
            const minFilterPower = FEED_POWER_FILTER_OPTIONS[value].feedPower;
            const feedPower = item[6];
            if (feedPower < minFilterPower) return;
        }

        // ITEM TYPE FILTER (multi-select)
        if (filter.itemType.enabled && filter.itemType.values.length > 0) {
            const itemSlotType = item[1];
            // Check if item's slot type matches any selected type
            const matchesType = filter.itemType.values.some(typeKey => {
                const slotConfig = SLOT_MAP_FILTER[typeKey];
                return slotConfig && slotConfig.slotType === itemSlotType;
            });
            if (!matchesType) return;
        }

        // CHARACTER TYPE FILTER
        if (filter.characterType.enabled && filter.characterType.value !== 0) {
            const { value } = filter.characterType;
            const hasMatchingLocation = itemData.locations.some((loc) => {
                if (value === 1 || value === 2) {
                    // Seasonal or normal characters
                    if (!loc.storageTypeId.startsWith('char:')) return false;
                    const charId = parseInt(loc.storageTypeId.split(':')[1], 10);
                    const account = accountsData.find((a) => a.email === loc.email);
                    const char = account?.characters?.find((c) => c.char_id === charId);
                    if (!char) return false;
                    if (value === 1) return char.seasonal;
                    if (value === 2) return !char.seasonal;
                } else if (value === 3) {
                    // Not on character
                    return !loc.storageTypeId.startsWith('char:');
                }
                return true;
            });
            if (!hasMatchingLocation) return;
        }

        filteredItemIds.push(itemId);
    });

    // Add empty slot placeholder at the end if it exists
    if (totalsMap.has(-1)) {
        filteredItemIds.push(-1);
    }

    return filteredItemIds;
};

// ==================== CONTEXT PROVIDER ====================

function VaultPeekerContextProvider({ children }) {
    const { getByKeyAndSubKey, setByKeyAndSubKey } = useUserSettings();
    const { groups } = useGroups();
    const { accounts, getAccountByEmail } = useAccounts();
    const { debugLogs } = useDebugLogs();

    // Raw data from Tauri
    const [rawDatasets, setRawDatasets] = useState([]);
    const [isLoading, setIsLoading] = useState(true); // Start as true to show loading initially

    // Filter state
    const [filter, dispatchFilter] = useReducer(filterReducer, DEFAULT_FILTER);

    // Selected item for popover
    const [selectedItem, setSelectedItem] = useState(null);
    const [popperPosition, setPopperPosition] = useState(null);

    // Settings
    const [itemPadding, setItemPadding] = useState(2);

    // Filter presets
    const filterPresets = useMemo(() => {
        return getByKeyAndSubKey('vaultPeeker', 'filterPresets') || [];
    }, [getByKeyAndSubKey]);

    // ==================== COMPUTED DATA ====================

    // Aggregate all items into totals map
    const totalsMap = useMemo(() => {
        if (!rawDatasets.length) return new Map();
        const start = performance.now();
        const result = aggregateItems(rawDatasets, groups, getAccountByEmail);
        if (debugLogs) {
            console.log(`[VaultPeeker] Aggregation took ${(performance.now() - start).toFixed(2)}ms (${result.size} items)`);
        }
        return result;
    }, [rawDatasets, groups, getAccountByEmail]);

    // Process accounts data
    const accountsData = useMemo(() => {
        if (!rawDatasets.length) return [];
        const start = performance.now();
        const result = processAccountsData(rawDatasets, groups, getAccountByEmail);
        if (debugLogs) {
            console.log(`[VaultPeeker] Account processing took ${(performance.now() - start).toFixed(2)}ms (${result.length} accounts)`);
        }
        return result;
    }, [rawDatasets, groups, getAccountByEmail]);

    // Filtered item IDs for totals view
    const filteredItemIds = useMemo(() => {
        if (!totalsMap.size) return [];
        const start = performance.now();
        const result = applyFiltersToItems(totalsMap, filter, accountsData);
        if (debugLogs) {
            console.log(`[VaultPeeker] Filtering took ${(performance.now() - start).toFixed(2)}ms (${result.length} items passed)`);
        }
        return result;
    }, [totalsMap, filter, accountsData]);

    // Totals items for rendering (with count and maxRarity)
    const totalsItems = useMemo(() => {
        const start = performance.now();
        const result = filteredItemIds.map((itemId) => {
            const data = totalsMap.get(itemId);
            return {
                itemId,
                count: data?.count || 0,
                maxRarity: data?.maxRarity || 0,
                locations: data?.locations || [],
                enchantVariants: data?.enchantVariants || new Map(),
            };
        });
        if (debugLogs) {
            console.log(`[VaultPeeker] Totals mapping took ${(performance.now() - start).toFixed(2)}ms`);
        }
        return result;
    }, [filteredItemIds, totalsMap]);

    // ==================== DATA FETCHING ====================

    const refreshItemData = useCallback(async () => {
        if (debugLogs) {
            console.log('[VaultPeekerV2] Starting data fetch...');
        }
        setIsLoading(true);
        try {
            const res = await invoke('get_latest_char_list_dataset_for_each_account');
            if (debugLogs) {
                console.log('[VaultPeekerV2] Received datasets:', res?.length || 0, 'accounts');
            }
            if (debugLogs && res?.length > 0) {
                    console.log('[VaultPeekerV2] First dataset sample:', {
                        email: res[0].email,
                        characterCount: res[0].character?.length || 0,
                        itemCount: res[0].items?.length || 0,
                });
            }
            setRawDatasets(res || []);
        } catch (error) {
            console.error('[VaultPeekerV2] Failed to fetch vault peeker data:', error);
            setRawDatasets([]);
        } finally {
            setIsLoading(false);
        }
    }, []);

    // Initial data fetch
    useEffect(() => {
        refreshItemData();
    }, [refreshItemData]);

    // Refresh when accounts change
    useEffect(() => {
        if (accounts?.length > 0) {
            refreshItemData();
        }
    }, [accounts, refreshItemData]);

    // Load item padding from density setting
    useEffect(() => {
        const density = getByKeyAndSubKey('vaultPeeker', 'density');
        if (density && DENSITY_TO_PADDING[density] !== undefined) {
            setItemPadding(DENSITY_TO_PADDING[density]);
        }
    }, [getByKeyAndSubKey]);

    // ==================== FILTER ACTIONS ====================

    const changeFilter = useCallback((filterType, value) => {
        dispatchFilter({ type: 'SET_FILTER', filterType, value });
    }, []);

    const resetFilterType = useCallback((filterType) => {
        dispatchFilter({ type: 'RESET_FILTER', filterType });
    }, []);

    const resetAllFilters = useCallback(() => {
        dispatchFilter({ type: 'RESET_ALL' });
    }, []);

    const loadFilterPreset = useCallback((preset) => {
        if (preset?.filter) {
            dispatchFilter({ type: 'LOAD_PRESET', preset: preset.filter });
        }
    }, []);

    const saveFilterPreset = useCallback((name) => {
        const newPreset = {
            id: crypto.randomUUID(),
            name,
            filter: { ...filter },
        };
        const updatedPresets = [...filterPresets, newPreset];
        setByKeyAndSubKey('vaultPeeker', 'filterPresets', updatedPresets);
        return newPreset;
    }, [filter, filterPresets, setByKeyAndSubKey]);

    const deleteFilterPreset = useCallback((presetId) => {
        const updatedPresets = filterPresets.filter((p) => p.id !== presetId);
        setByKeyAndSubKey('vaultPeeker', 'filterPresets', updatedPresets);
    }, [filterPresets, setByKeyAndSubKey]);

    // ==================== SETTINGS ACTIONS ====================

    const updateItemPadding = useCallback((padding) => {
        if (ITEM_PADDING_OPTIONS.includes(padding)) {
            setItemPadding(padding);
            // Save as density setting
            const density = PADDING_TO_DENSITY[padding];
            if (density) {
                setByKeyAndSubKey('vaultPeeker', 'density', density);
            }
        }
    }, [setByKeyAndSubKey]);

    // ==================== ITEM SELECTION ====================

    const selectItem = useCallback((itemId, position, itemData) => {
        const data = totalsMap.get(itemId);
        setSelectedItem({
            itemId,
            item: items[itemId],
            ...data,
            ...itemData,
        });
        setPopperPosition(position);
    }, [totalsMap]);

    const clearSelectedItem = useCallback(() => {
        setSelectedItem(null);
        setPopperPosition(null);
    }, []);

    // ==================== CONTEXT VALUE ====================

    const contextValue = useMemo(() => ({
        // Data
        isLoading,
        totalsMap,
        totalsItems,
        filteredItemIds,
        accountsData,
        rawDatasets,

        // Filter
        filter,
        changeFilter,
        resetFilterType,
        resetAllFilters,

        // Filter presets
        filterPresets,
        loadFilterPreset,
        saveFilterPreset,
        deleteFilterPreset,

        // Item selection
        selectedItem,
        popperPosition,
        selectItem,
        clearSelectedItem,

        // Settings
        itemPadding,
        updateItemPadding,
        itemPaddingOptions: ITEM_PADDING_OPTIONS,

        // Refresh
        refreshItemData,

        // Constants
        feedPowerFilterOptions: FEED_POWER_FILTER_OPTIONS,
        slotMapFilter: SLOT_MAP_FILTER,
        rarityFilterOptions: RARITY_FILTER_OPTIONS,
    }), [
        isLoading,
        totalsMap,
        totalsItems,
        filteredItemIds,
        accountsData,
        rawDatasets,
        filter,
        changeFilter,
        resetFilterType,
        resetAllFilters,
        filterPresets,
        loadFilterPreset,
        saveFilterPreset,
        deleteFilterPreset,
        selectedItem,
        popperPosition,
        selectItem,
        clearSelectedItem,
        itemPadding,
        updateItemPadding,
        refreshItemData,
    ]);

    return (
        <VaultPeekerContext.Provider value={contextValue}>
            {children}
        </VaultPeekerContext.Provider>
    );
}

export default VaultPeekerContext;
export { VaultPeekerContextProvider, DEFAULT_FILTER, FEED_POWER_FILTER_OPTIONS, SLOT_MAP_FILTER, ITEM_PADDING_OPTIONS, DENSITY_TO_PADDING, PADDING_TO_DENSITY, RARITY_FILTER_OPTIONS };
