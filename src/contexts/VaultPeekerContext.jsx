import { createContext, useEffect, useState } from "react";
import useAccounts from "../hooks/useAccounts";
import { invoke } from "@tauri-apps/api";
import { extractRealmItemsFromCharListDatasets, formatAccountDataFromCharListDatasets } from "../utils/realmItemUtils";
import ItemLocationPopper from "../components/Realm/ItemLocationPopper";
import items from "../assets/constants";

const VaultPeekerContext = createContext();

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
    }
};

function VaultPeekerContextProvider({ children }) {
    const [selectedItem, setSelectedItem] = useState(null);
    const [popperPosition, setPopperPosition] = useState(null);
    const [totalItems, setTotalItems] = useState([]);
    const [accountsData, setAccountsData] = useState([]);
    const [filter, setFilter] = useState(defaultFilter);
    const [filterItemsCallbacks, setFilterItemsCallbacks] = useState({});

    const { getAccountByEmail } = useAccounts();

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
            filteredItemIds = filteredItemIds.filter(itemId => {
                const item = items[itemId];
                if (!item) return false;
                const tier = item[2];
                const flag = item[9];
                const { value, direction, flag: filterFlag } = filter.tier;

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
                        if (value === -1 && filterFlag === 0) {
                            return tier === value && flag === filterFlag;
                        }

                        if (tier === -1) {
                            //UT items are only shown for 'down' filter if the flag is set to 1 (UT)
                            if (flag === 1) {
                                return filterFlag === 1;
                            }

                            if (flag === 2 && (filterFlag >= 1)) {
                                return true;
                            }

                            return false;
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

        return filteredItemIds;
    };

    useEffect(() => {
        const loadItems = async () => {
            const res = await invoke('get_latest_char_list_dataset_for_each_account');
            const items = extractRealmItemsFromCharListDatasets(res);
            setTotalItems(items);
            let accs = formatAccountDataFromCharListDatasets(res);
            if (accs?.length > 0) {
                accs = accs.map((acc) => {
                    return {
                        ...acc,
                        name: getAccountByEmail(acc.email)?.name,
                    }
                });
            }
            setAccountsData(accs);
        };
        loadItems();
    }, []);

    useEffect(() => {
        Object.keys(filterItemsCallbacks).forEach((id) => {
            const { callback, items } = filterItemsCallbacks[id];
            if (callback && items && items.length > 0) {
                callback(applyFilter(items));
            }
        });
    }, [filter]);

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
