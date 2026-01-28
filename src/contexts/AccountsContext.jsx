import { createContext, useEffect, useState } from "react";
import { APP_VERSION } from "../constants";
import { startSession } from "../backend/eamApi";
import { useSearchParams } from "react-router-dom";
import { invoke } from "@tauri-apps/api/core";
import { logToAuditLog, logToErrorLog, requestStateToMessage } from "eam-commons-js";
import useServerList from "../hooks/useServerList";
import useSnack from "../hooks/useSnack";

const AccountsContext = createContext();

function AccountsContextProvider({ children }) {
    const [searchParams, setSearchParams] = useSearchParams();
    const { reloadServers } = useServerList();
    const { showSnackbar } = useSnack();

    const [isLoading, setIsLoading] = useState(false);
    const [accounts, setAccounts] = useState([]);
    const [selectedAccount, setSelectedAccount] = useState(null);

    const getAccountByEmail = (email) => accounts.find((acc) => acc.email === email);
    const loadAccountByEmail = async (email, forceReload = false) => {
        if (!email) return null;
        if (!forceReload) {
            const acc = getAccountByEmail(email);
            if (acc) {
                return acc;
            }
        }

        const acc = await invoke('get_eam_account_by_email', { accountEmail: email })
            .catch((err) => {
                console.error('Error loading account by email:', err, email);

                if (err && err === 'database is locked') {
                    return { error: true, waitAndRetry: true, message: 'Database is locked, please try again later.' };
                }

                return null;
            });

        if (!acc) return null;
        if (acc.error && acc.waitAndRetry) {
            console.warn('Database is locked, retrying after a short delay...');
            await new Promise(resolve => setTimeout(resolve, 150));
            const retryAcc = await invoke('get_eam_account_by_email', { accountEmail: email })
                .catch((err) => {
                    console.error('Error loading account by email on retry:', err, email);
                    return null;
                });

            if (!retryAcc) return null;
            return enhanceAccountData(retryAcc);
        }

        return enhanceAccountData(acc);
    }

    const loadAccounts = async () => {
        setIsLoading(true);
        try {
            const response = await invoke('get_all_eam_accounts');
            const accounts = response.map((acc) => {
                if (acc.token) {
                    acc.token = null; // Tokens in the account object are deprecated
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
        finally {
            setIsLoading(false);
        }
    }

    const saveAccount = async (acc) => {
        await invoke('insert_or_update_eam_account', { eamAccount: acc });
        return enhanceAccountData(await invoke('get_eam_account_by_email', { accountEmail: acc.email }));
    }

    const updateAccount = async (updatedAccount, encryptPassword = false, reloadAccounts = true) => {
        if (!updatedAccount) return updatedAccount;

        if (updatedAccount.token) {
            updatedAccount.token = null; // Tokens in the account object are deprecated            
        }

        const getPassword = async () => {
            if (encryptPassword) {
                const data = { email: updatedAccount?.email, data: updatedAccount?.password?.toString() };
                if (!data.data) {
                    console.error('Error encrypting password: data is empty');
                    return -1;
                }
                return await invoke('encrypt_string', data).then((res) => res);
            }

            return updatedAccount?.password;
        };

        const pw = await getPassword()
            .catch((err) => {
                console.error('Error encrypting password:', err);
                return -1;
            });

        if (pw === -1)
            return false;

        const acc = { ...updatedAccount, password: pw };
        const updAccount = await saveAccount(acc)
            .catch((err) => {
                console.error('Error saving account:', err, acc);
                return null;
            });

        if (updAccount === null)
            return false;

        const updatedAccountToUse = updAccount ? updAccount : updatedAccount;

        if (selectedAccount && selectedAccount.email === updatedAccount.email) {
            setSelectedAccount(updatedAccountToUse);
        }

        if (reloadAccounts) {
            loadAccounts();
        }

        return true;
    };

    const updateAccounts = async (_accounts) => {
        for (const acc of _accounts) {
            await updateAccount(acc, false, false);
            await new Promise(resolve => setTimeout(resolve, 50));
        }
        loadAccounts();
    };

    const sendAccountVerify = async (email, updateAccountInDatabase = true, updateLastLogin = false) => {
        const acc = getAccountByEmail(email);

        if (!acc) return { success: false, message: 'Account not found' };

        logToAuditLog('sendAccountVerify', `Sending account/verify request...`, email);
        try {
            const response = await invoke('send_account_verify_request_for_account', { accountEmail: email })
                .catch((err) => {
                    logToErrorLog('sendAccountVerify', `Error sending account/verify request: ${err}`, email);
                    console.error('Error sending account/verify request:', err);
                    return null;
                });

            if (!response) {
                return { success: false, message: 'Failed to verify account' };
            }

            const { access_token, request_state, account_name } = response;
            
            // Handle rate limit errors
            if (request_state === 'RateLimitExceeded') {
                return { success: false, message: 'Rate limit exceeded', requestState: 'RateLimitExceeded' };
            }

            const isSuccess = request_state === 'Success';
            const newAcc = ({ ...acc, state: request_state });
            
            if (updateLastLogin && isSuccess) {
                newAcc.lastLogin = new Date();
            }

            if (account_name) {
                newAcc.name = account_name;
            }

            if (updateAccountInDatabase || !isSuccess) {
                await updateAccount(newAcc);
            }

            if (!isSuccess) {
                logToAuditLog('sendAccountVerify', `Account/verify request failed with state: ${request_state}`, email);
                return { 
                    success: false, 
                    message: requestStateToMessage(request_state) || 'Failed to verify account',
                    requestState: request_state,
                    acc: newAcc
                };
            }

            logToAuditLog('sendAccountVerify', `Account/verify request successful.`, email);

            // Build response data in expected format
            const responseData = {
                Account: {
                    AccessToken: access_token?.access_token,
                    AccessTokenTimestamp: access_token?.access_token_timestamp,
                    AccessTokenExpiration: access_token?.access_token_expiration,
                    Name: account_name
                }
            };

            return { success: true, message: 'Account verified', data: responseData, acc: newAcc, requestState: request_state };
        } catch (error) {
            logToErrorLog('sendAccountVerify', error);
            return { success: false, message: 'Failed to verify account' };
        }
    }

    const sendCharList = async (email, accessToken, acc = null) => {
        if (!acc) {
            acc = getAccountByEmail(email);
        }

        if (!acc) return { success: false, message: 'Account not found' };

        logToAuditLog('sendCharList', `Sending char/list request...`, email);
        let requestState = 'Error';
        try {
            // Build access token object for Rust
            const accessTokenObj = typeof accessToken === 'string' 
                ? { access_token: accessToken, access_token_timestamp: '', access_token_expiration: '' }
                : {
                    access_token: accessToken?.AccessToken || accessToken?.access_token || '',
                    access_token_timestamp: accessToken?.AccessTokenTimestamp || accessToken?.access_token_timestamp || '',
                    access_token_expiration: accessToken?.AccessTokenExpiration || accessToken?.access_token_expiration || ''
                };

            const response = await invoke('send_char_list_request_for_account', { 
                email, 
                accessToken: accessTokenObj 
            })
                .catch((err) => {
                    logToErrorLog('sendCharList', `Error sending char/list request: ${err}`, email);
                    console.error('Error sending char/list request:', err);

                    if (err && typeof err === "string" && err.includes('RateLimitExceeded')) {
                        return { error: true, data: { success: false, message: 'Rate limit exceeded', requestState: 'RateLimitExceeded' } };
                    }

                    return null;
                });

            console.log('sendCharList response', response);

            if (!response) {
                return { success: false, message: 'Failed to get character list', requestState: requestState };
            }

            if (response.error) {
                return response.data || { success: false, message: 'Failed to get character list', requestState: response.requestState || requestState };
            }

            const { dataset, servers, request_state } = response;
            requestState = request_state;
            
            const isSuccess = requestState === 'Success';
            const newAcc = ({ ...acc, state: requestState, lastRefresh: new Date() });
            await updateAccount(newAcc);

            if (!isSuccess) {
                logToAuditLog('sendCharList', `Char/list request failed with state: ${requestState}`, email);
                return { success: false, message: requestStateToMessage(requestState) || 'Failed to get character list', requestState: requestState };
            }

            // Reload servers from database (they were stored by Rust)
            if (servers && servers.length > 0) {
                await reloadServers();
            }

            logToAuditLog('sendCharList', `Char/list request successful.`, email);
            return { success: true, message: 'Character list received', data: dataset, requestState: requestState };
        } catch (error) {
            logToErrorLog('sendCharList', error);
            return { success: false, message: 'Failed to get character list', requestState: requestState };
        };
    }

    const refreshData = async (email) => {
        let acc = getAccountByEmail(email);
        if (!acc) {
            return null;
        }

        const accResponse = await sendAccountVerify(acc.email, false);
        if (accResponse === null || !accResponse.success) {
            logToErrorLog("refresh Data", "Failed to refresh data for " + acc.email);
            showSnackbar("Failed to refresh data", 'error');
            return null;
        }

        const token = {
            AccessToken: accResponse.data.Account.AccessToken,
            AccessTokenTimestamp: accResponse.data.Account.AccessTokenTimestamp,
            AccessTokenExpiration: accResponse.data.Account.AccessTokenExpiration,
        };

        acc = accResponse.acc;

        const charList = await sendCharList(acc.email, token.AccessToken, acc);
        if (charList === null || !charList.success) {
            logToErrorLog("refresh Data", "Failed to refresh data for " + acc.email);
            showSnackbar(`Failed to refresh data${charList?.requestState ? `: ${requestStateToMessage(charList.requestState)}` : ''}`, 'error');
            return null;
        }

        return token;
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
        isLoading,

        setSelectedAccount,

        getAccountByEmail: getAccountByEmail,
        loadAccountByEmail,
        updateAccount,
        updateAccounts,
        reloadAccounts: loadAccounts,
        deleteAccount,
        sendAccountVerify,
        sendCharList,
        refreshData,
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