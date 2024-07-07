import { createContext, useEffect, useState } from "react";
import useAccounts from "../hooks/useAccounts";
import { invoke } from "@tauri-apps/api";
import { extractRealmItemsFromCharListDatasets, formatAccountDataFromCharListDatasets } from "../utils/realmItemUtils";

const VaultPeekerContext = createContext();

function VaultPeekerContextProvider({ children }) {
    const [totalItems, setTotalItems] = useState([]);
    const [accountsData, setAccountsData] = useState([]);
    const [filteredTotals, setFilteredTotals] = useState([]);
    const { getAccountByEmail } = useAccounts();

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

    const contextValue = {
        totalItems,
        accountsData,
        filteredTotals,
        getAccountByEmail,
    };

    return (
        <VaultPeekerContext.Provider value={contextValue}>
            {children}
        </VaultPeekerContext.Provider>
    );
}

export default VaultPeekerContext;
export { VaultPeekerContextProvider };
