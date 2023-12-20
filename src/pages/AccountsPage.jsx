import { Box } from "@mui/material";
import AccountGrid from "../components/AccountGrid";
import { useEffect, useState } from "react";
import { ACCOUNTS_FILE_PATH, SAVE_FILE_NAME, SAVE_FILE_PATH } from "../constants";
import { readFileUTF8 } from "../utils/readFileUtil";
import AccountDetails from "../components/AccountDetails/AccountDetails";
import { useSearchParams } from "react-router-dom";
import { writeFileUTF8 } from "../utils/writeFileUtil";

function AccountsPage() {
    const [accounts, setAccounts] = useState([]);
    const [selectedAccount, setSelectedAccount] = useState(null);

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

    const updateAccount = (updatedAccount) => {
        
        const updatedAccounts = accounts.map((account) => {
            if (account.email === updatedAccount.email) {
                return updatedAccount;
            }
            return account;
        });

        setAccounts(updatedAccounts);
        ACCOUNTS_FILE_PATH()
            .then((filePath) => {
                writeFileUTF8(filePath, updatedAccounts, true);
            });
        
        if (selectedAccount && selectedAccount.email === updatedAccount.email) {
            setSelectedAccount(updatedAccount);
        }
    };

    return (
        <Box id="accountspage"
            sx={{
                width: '100%',
                p: 2,
            }}
        >
            <AccountGrid acc={accounts} selected={selectedAccount} setSelected={setSelectedAccount} onAccountChanged={(updatedAccount) => updateAccount(updatedAccount)} />
            <AccountDetails acc={selectedAccount} onClose={() => setSelectedAccount(null)} onAccountChanged={(updatedAccount) => updateAccount(updatedAccount)}/>
        </Box>
    );
}

export default AccountsPage;