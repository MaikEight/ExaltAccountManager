import { Box } from "@mui/material";
import AccountGrid from "../components/AccountGrid";
import { useEffect, useState } from "react";
import { ACCOUNTS_FILE_PATH, SAVE_FILE_NAME, SAVE_FILE_PATH } from "../constants";
import { readFileUTF8 } from "../utils/readFileUtil";
import AccountDetails from "../components/AccountDetails/AccountDetails";
import { useSearchParams } from "react-router-dom";
import { writeFileUTF8 } from "../utils/writeFileUtil";
import AddNewAccount from "../components/AddNewAccount";

function AccountsPage() {
    const [accounts, setAccounts] = useState([]);
    const [selectedAccount, setSelectedAccount] = useState(null);
    const [showAddNewAccount, setShowAddNewAccount] = useState(false);

    const [searchParams, setSearchParams] = useSearchParams();

    useEffect(() => {
        const fetchData = async () => {
            try {
                const filePath = await ACCOUNTS_FILE_PATH();
                const accounts = await readFileUTF8(filePath, true);

                if (accounts) {
                    setAccounts(accounts);
                    handleSelectedAccountParameter(accounts);
                }
            } catch (error) {
                console.error('Error fetching data:', error);
            }
        };

        fetchData();
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

    const handleSelectedAccountParameter = (accs) => {
        if (searchParams.has('selectedAccount')) {
            const selectedAccountEmail = searchParams.get('selectedAccount');
            const selectedAccount = accs.find((account) => account.email === selectedAccountEmail);
            if (selectedAccount) {
                setSelectedAccount(selectedAccount);
            }
        }
    }

    const getNextUniqueAccountId = () => {
        let nextId = 1;
        while (accounts.find((account) => account.id === nextId)) {
            nextId++;
        }
        return nextId;
    }

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

    const saveAccounts = (accs) => {
        setAccounts(accs);
        ACCOUNTS_FILE_PATH()
            .then((filePath) => {
                writeFileUTF8(filePath, accs, true);
            });
    };

    return (
        <Box id="accountspage"
            sx={{
                width: '100%',
                p: 2,
            }}
        >
            <AccountGrid acc={accounts} selected={selectedAccount} setSelected={setSelectedAccount} onAccountChanged={(updatedAccount) => updateAccount(updatedAccount, false)} setShowAddNewAccount={setShowAddNewAccount} />
            <AccountDetails acc={showAddNewAccount ? null : selectedAccount} onClose={() => setSelectedAccount(null)} onAccountChanged={(updatedAccount) => updateAccount(updatedAccount, false)} onAccountDeleted={(email) => saveAccounts(accounts.map((a) => a.email !== email ? a : null).filter((a) => a !== null)) } />
            <AddNewAccount isOpen={showAddNewAccount} onClose={() => setShowAddNewAccount(false)} onSave={(newAccount) => updateAccount(newAccount, true)} />
        </Box>
    );
}

export default AccountsPage;