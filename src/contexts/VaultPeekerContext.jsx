import { createContext, useEffect, useState } from "react";
import useAccounts from "../hooks/useAccounts";
import { invoke } from "@tauri-apps/api/core";
import { extractRealmItemsFromCharListDatasets, formatAccountDataFromCharListDatasets } from "../utils/realmItemUtils";
import ItemLocationPopper from "../components/Realm/ItemLocationPopper";
import items from "../assets/constants";
import { useGroups } from 'eam-commons-js';

const VaultPeekerContext = createContext();

const feedPowerFilterOptions = [
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

const slotMapFilter = {
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
}

const defaultFilter = {
    search: {
        enabled: false,
        value: '',
    },
    tier: {
        enabled: false,
        direction: 'up', //up (>= value), down (<= value), equal (== value)
        value: -2,
        flag: -1, //0 = no tier, 1 = UT, 2 = ST (UT and ST > Tier 15 and UT > ST)
    },
    soulbound: {
        enabled: false,
        value: 2, //2 = all, 0 = soulbound, 1 = tradeable
    },
    feedPower: {
        enabled: false,
        value: 0,
    },
    itemType: {
        enabled: false,
        value: 0,
        key: 'all',
    },
    characterType: {
        enabled: false,
        value: 0, // 0 = all, 1 = seasonal, 2 = normal, 3 = not on character
    },
};

function VaultPeekerContextProvider({ children }) {
    const [selectedItem, setSelectedItem] = useState(null);
    const [popperPosition, setPopperPosition] = useState(null);
    const [totalItems, setTotalItems] = useState([]);
    const [filteredTotalItems, setFilteredTotalItems] = useState([]);
    const [accountsData, setAccountsData] = useState([]);
    const [filter, setFilter] = useState(defaultFilter);
    const [filterItemsCallbacks, setFilterItemsCallbacks] = useState({});

    const { groups } = useGroups();
    const { accounts, getAccountByEmail } = useAccounts();

    const refreshItemData = async () => {
        const res = await invoke('get_latest_char_list_dataset_for_each_account');
        const items = extractRealmItemsFromCharListDatasets(res);
        setTotalItems(items);
        setFilteredTotalItems(items);

        let accs = formatAccountDataFromCharListDatasets(res);
        if (accs?.length > 0) {
            accs = accs.map((acc) => {
                const eamAcc = getAccountByEmail(acc.email);
                const group = eamAcc?.group ? groups.find((g) => g.name === eamAcc?.group) : null;
                return {
                    ...acc,
                    name: eamAcc?.name,
                    group: group,
                }
            });
        }
        setAccountsData(accs);
    };

    const applyFilter = (itemIds) => {
        let filteredItemIds = itemIds;

        // TEXT SEARCH FILTER
        if (filter.search.enabled && filter.search !== '') {
            const searchTextLower = filter.search.value.toLowerCase();
            filteredItemIds = itemIds.filter((itemId) => {
                const item = items[itemId];
                if (!item) return false;
                return item[0].toLowerCase().includes(searchTextLower);
            });
        }

        // TIER FILTER
        if (filter.tier.enabled) {
            const { value, direction, flag: filterFlag } = filter.tier;
            filteredItemIds = filteredItemIds.filter(itemId => {
                const item = items[itemId];
                if (!item) return false;
                const tier = item[2];
                const flag = item[9];

                switch (direction) {
                    case 'up':
                        if (value === -1 && filterFlag === 0) {
                            return true;
                        }
                        //UT items are always shown for 'up' filter
                        if (tier === -1) {
                            if (flag === 1) {
                                return true;
                            }

                            if (flag === 2 && (filterFlag === 0 || filterFlag === 2)) {
                                return true;
                            }

                            return false;
                        }

                        if (filterFlag === 2 || filterFlag === 1) {
                            return false;
                        }

                        return tier >= value;
                    case 'down':
                        if (filterFlag > 0) { //filter <= UT / ST
                            if (flag === 0) { //item is not UT or ST so it matches
                                return true;
                            }

                            if (flag === 1) {
                                return filterFlag === 1; //item is UT and we want UT
                            }

                            if (flag === 2) {
                                return filterFlag >= 2; //item is ST
                            }

                            return true; //item is Tiered and we want UT or ST and lower
                        }

                        if (flag > 0) {
                            return false; //item is UT or ST and we want tiered
                        }

                        return tier <= value;
                    case 'equal':
                        if (value === -1) {
                            if (filterFlag === 0) {
                                return tier === value && flag === filterFlag;
                            }

                            return filterFlag === flag;
                        }

                        return tier === value;
                    default:
                        return false;
                }
            });
        }

        // SOULBOUND FILTER
        if (filter.soulbound.enabled) {
            const { value } = filter.soulbound;
            filteredItemIds = filteredItemIds.filter(itemId => {
                const item = items[itemId];
                if (!item) return false;
                const soulbound = item[8];

                if (value === 2) {
                    return true;
                }

                return value === 0 ? soulbound : !soulbound;
            });
        }

        // FEED POWER FILTER
        if (filter.feedPower.enabled) {
            const { value } = filter.feedPower;
            const minFilterPower = feedPowerFilterOptions[value].feedPower;
            filteredItemIds = filteredItemIds.filter(itemId => {
                const item = items[itemId];
                if (!item) return false;
                const feedPower = item[6];

                return feedPower >= minFilterPower;
            });
        }

        // ITEM TYPE FILTER
        if (filter.itemType.enabled) {
            const { value } = filter.itemType;
            filteredItemIds = filteredItemIds.filter(itemId => {
                const item = items[itemId];
                if (!item) return false;
                const itemType = item[1];

                return itemType === value;
            });
        }
        // CHARACTER TYPE FILTER
        if (filter.characterType.enabled && filter.characterType.value !== 0) {
            const { value } = filter.characterType;
            if (value === 1 || value === 2) {
                const allowedItemIdsList = [];
                accountsData.forEach((acc) => {
                    acc.character.forEach((char) => {
                        if (char.seasonal && (value === 1) || !char.seasonal && (value === 2)) {
                            allowedItemIdsList.push(...char.equipment);
                        }
                    });
                });

                const allowedItemIds = [...new Set(allowedItemIdsList)];
                filteredItemIds = filteredItemIds.filter(itemId => {
                    return allowedItemIds.includes(itemId);
                });

                // const newfilteredTotalItems = {};
                // allowedItemIds.forEach((itemId) => {
                //     if(totalItems.totals[itemId]) {
                //         newfilteredTotalItems[itemId] = totalItems.totals[itemId];
                //     }
                // });
                // console.log('newfilteredTotalItems', newfilteredTotalItems);

                // setFilteredTotalItems(newfilteredTotalItems);
            } else if (value === 3) {
                // Show only items not on characters
                const allowedItemIdsList = [];
                accountsData.forEach((acc) => {
                    allowedItemIdsList.push(...acc.account.gifts.itemIds);
                    allowedItemIdsList.push(...acc.account.material_storage.itemIds);
                    allowedItemIdsList.push(...acc.account.potions.itemIds);
                    allowedItemIdsList.push(...acc.account.temporary_gifts.itemIds);
                    allowedItemIdsList.push(...acc.account.vault.itemIds);
                });

                const allowedItemIds = [...new Set(allowedItemIdsList)];
                filteredItemIds = filteredItemIds.filter(itemId => {
                    return allowedItemIds.includes(itemId);
                });

                // const newfilteredTotalItemsKeys = Object.keys(totalItems.totals).filter((itemId) => allowedItemIds.includes(itemId));
                // const newfilteredTotalItems = {};
                // newfilteredTotalItemsKeys.forEach((key) => {
                //     newfilteredTotalItems[key] = totalItems.totals[key];
                // });

                // setFilteredTotalItems(newfilteredTotalItems);
            }
        } else {
            setFilteredTotalItems(totalItems);
        }

        return filteredItemIds;
    };

    useEffect(() => {
        refreshItemData();
    }, [accounts]);

    useEffect(() => {
        Object.keys(filterItemsCallbacks).forEach((id) => {
            const { callback, items } = filterItemsCallbacks[id];
            if (callback && items && items.length > 0) {
                callback(applyFilter(items));
            }
        });
    }, [filter, totalItems]);

    const addItemFilterCallback = (id, callback, items) => {
        setFilterItemsCallbacks((prev) => {
            return {
                ...prev,
                [id]: { callback: callback, items: items },
            };
        });
    };

    const removeItemFilterCallback = (id) => {
        setFilterItemsCallbacks((prev) => {
            const newCallbacks = { ...prev };
            delete newCallbacks[id];
            return newCallbacks;
        });
    };

    const changeFilter = (filterType, value) => {
        setFilter((prev) => {
            return {
                ...prev,
                [filterType]: value,
            };
        });
    };

    const resetFilterType = (filterType) => {
        setFilter((prev) => {
            return {
                ...prev,
                [filterType]: defaultFilter[filterType],
            };
        });
    };

    const contextValue = {
        totalItems,
        filteredTotalItems,
        accountsData,
        getAccountByEmail,
        popperPosition,
        filter,

        setPopperPosition,
        selectedItem,
        setSelectedItem,
        changeFilter,
        resetFilterType,

        addItemFilterCallback,
        removeItemFilterCallback,

        feedPowerFilterOptions,
        slotMapFilter,
    };

    return (
        <VaultPeekerContext.Provider value={contextValue}>
            {children}
            <ItemLocationPopper
                open={Boolean(selectedItem)}
                position={popperPosition}
                selectedItem={selectedItem}
                onClose={() => {
                    setSelectedItem(null);
                    setPopperPosition(null);
                }}
            />
        </VaultPeekerContext.Provider>
    );
}

export default VaultPeekerContext;
export { VaultPeekerContextProvider };
