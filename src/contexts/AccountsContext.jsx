import { createContext, useEffect, useState } from "react";
import { APP_VERSION } from "../constants";
import { startSession } from "../backend/eamApi";
import { useSearchParams } from "react-router-dom";
import { invoke } from "@tauri-apps/api";
import { logToAuditLog } from "../utils/loggingUtils";

const AccountsContext = createContext();

function AccountsContextProvider({ children }) {
    const [accounts, setAccounts] = useState([]);
    const [selectedAccount, setSelectedAccount] = useState(null);
    const [searchParams, setSearchParams] = useSearchParams();

    const loadAccounts = async () => {
        try {
            const response = await invoke('get_all_eam_accounts');
            const accounts = response.map((acc) => {
                if (!!acc.token) {
                    const token = JSON.parse(acc.token);
                    acc.token = token;
                }
                return enhanceAccountData(acc);
            });

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

    const saveAccount = async (acc) => {
        await invoke('insert_or_update_eam_account', { eamAccount: acc });
        return enhanceAccountData(await invoke('get_eam_account_by_email', { accountEmail: acc.email }));
    }

    const updateAccount = (updatedAccount, encryptPassword = false) => {
        if (!updatedAccount) return updatedAccount;

        let token = null;
        if (updatedAccount.token) {
            try {
                token = JSON.stringify(updatedAccount.token);
            } catch (error) {
                console.error('Error parsing token:', error);
                token = null;
            }
        }

        const getPassword = async () => {
            if (encryptPassword) {
                return await invoke('encrypt_string', { data: updatedAccount.password }).then((res) => res);
            }

            return updatedAccount.password;
        };

        getPassword()
            .then((pw) => {
                const acc = { ...updatedAccount, token: token, password: pw };
                saveAccount(acc)
                    .then((updAccount) => {
                        const updatedAccountToUse = !!updAccount ? updAccount : updatedAccount;

                        if (selectedAccount && selectedAccount.email === updatedAccount.email) {
                            setSelectedAccount(updatedAccountToUse);
                        }

                        loadAccounts();
                    });
            });
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

    const deleteAccount = async (email) => {        
        logToAuditLog('deleteAccount', `Deleting account ${email}`, email);
        
        await invoke('delete_eam_account', { accountEmail: email });
        const updatedAccounts = accounts.filter((account) => account.email !== email);
        setAccounts(updatedAccounts);
        if (selectedAccount && selectedAccount.email === email) {
            setSelectedAccount(null);
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
        deleteAccount,
    };

    return (
        <AccountsContext.Provider value={value}>
            {children}
        </AccountsContext.Provider>
    )
}

function enhanceAccountData(acc) {
    if (!acc) return acc;

    acc.lastLogin = acc.lastLogin ? new Date(acc.lastLogin) : null;
    acc.lastRefresh = acc.lastRefresh ? new Date(acc.lastRefresh) : null;
    return acc;
}

export default AccountsContext
export { AccountsContextProvider }