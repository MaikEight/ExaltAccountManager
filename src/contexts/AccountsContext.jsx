import { createContext, useEffect, useState } from "react";
import { ACCOUNTS_FILE_PATH, APP_VERSION } from "../constants";
import { readFileUTF8 } from "../utils/readFileUtil";
import { startSession } from "../backend/eamApi";
import { useSearchParams } from "react-router-dom";
import { writeFileUTF8 } from "../utils/writeFileUtil";

const AccountsContext = createContext();

function AccountsContextProvider({ children }) {
    const [accounts, setAccounts] = useState([]);
    const [selectedAccount, setSelectedAccount] = useState(null);
    const [searchParams, setSearchParams] = useSearchParams();

    const getNextUniqueAccountId = () => {
        let nextId = 1;
        while (accounts.find((account) => account.id === nextId)) {
            nextId++;
        }
        return nextId;
    }

    const loadAccounts = async () => {
        try {
            const filePath = await ACCOUNTS_FILE_PATH();
            const accounts = await readFileUTF8(filePath, true);

            if (accounts) {
                setAccounts(accounts);
                return accounts;
            }
            return [];
        } catch (error) {
            console.error('Error loading accounts:', error);
            return [];
        }
    }

    const saveAccounts = (accs) => {
        setAccounts(accs);
        ACCOUNTS_FILE_PATH()
            .then((filePath) => {
                writeFileUTF8(filePath, accs, true);
            });
    };

    const updateAccount = (updatedAccount, isNewAccount) => {

        const updatedAccounts = !isNewAccount ? accounts.map((account) => {
            if (account.email === updatedAccount.email) {
                return updatedAccount;
            }
            return account;
        }) : [...accounts, { ...updatedAccount, id: getNextUniqueAccountId() }];

        saveAccounts(updatedAccounts);

        if (selectedAccount && selectedAccount.email === updatedAccount.email) {
            setSelectedAccount(updatedAccount);
        }
    };

    const handleSelectedAccountParameter = (accs) => {
        if (searchParams.has('selectedAccount')) {
            const selectedAccountEmail = searchParams.get('selectedAccount');
            const selAccount = accs.find((account) => account.email === selectedAccountEmail);
            if (selAccount) {
                setSelectedAccount(selAccount);
            }
        }
    }

    useEffect(() => {
        loadAccounts()
            .then((accounts) => {
                if (!sessionStorage.getItem('sessionId')) {
                    const apiHwidHash = localStorage.getItem('apiHwidHash');
                    if (apiHwidHash) {
                        startSession(accounts ? accounts.length : 0, apiHwidHash, APP_VERSION)
                            .then((res) => {
                                const sessionId = res.sessionId;
                                if (sessionId && sessionId.length > 0 && sessionId !== 'null' && sessionId !== 'undefined') {
                                    sessionStorage.setItem('sessionId', sessionId);
                                }
                            })
                            .catch((err) => {
                                console.error('startSession error:', err);
                            });
                    }
                }
            });
    }, []);

    useEffect(() => {
        handleSelectedAccountParameter(accounts);
    }, [searchParams]);

    useEffect(() => {
        handleSelectedAccountParameter(accounts);
    }, [accounts]);

    useEffect(() => {
        const updatedSearchParams = new URLSearchParams(searchParams.toString());

        if (!selectedAccount || !selectedAccount.email) {
            updatedSearchParams.delete('selectedAccount');
            setSearchParams(updatedSearchParams.toString());
            return;
        }

        updatedSearchParams.set('selectedAccount', selectedAccount.email);
        setSearchParams(updatedSearchParams.toString());
        searchParams.set('selectedAccount', selectedAccount.email);
    }, [selectedAccount]);

    const value = {
        accounts,
        selectedAccount,

        setSelectedAccount,

        updateAccount,
        reloadAccounts: loadAccounts,
        saveAccounts
    };

    return (
        <AccountsContext.Provider value={value}>
            {children}
        </AccountsContext.Provider>
    )
}

export default AccountsContext
export { AccountsContextProvider }