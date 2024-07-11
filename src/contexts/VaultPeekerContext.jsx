import { createContext, useEffect, useState } from "react";
import useAccounts from "../hooks/useAccounts";
import { invoke } from "@tauri-apps/api";
import { extractRealmItemsFromCharListDatasets, formatAccountDataFromCharListDatasets } from "../utils/realmItemUtils";
import ItemLocationPopper from "../components/Realm/ItemLocationPopper";
import items from "../assets/constants";

const VaultPeekerContext = createContext();

const defaultFilter = {
    search: '',
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
        
        // TEXT SEARCH
        if (filter.search !== '') {
            const searchTextLower = filter.search.toLowerCase();
            filteredItemIds = itemIds.filter((itemId) => {
                const item = items[itemId];
                if (!item) return false;
                return item[0].toLowerCase().includes(searchTextLower);
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
