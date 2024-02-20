import { Box } from "@mui/material";
import AccountGrid from "../components/AccountGrid";
import { useState } from "react";
import AccountDetails from "../components/AccountDetails/AccountDetails";
import AddNewAccount from "../components/AddNewAccount";
import useAccounts from "../hooks/useAccounts";

function AccountsPage() {

    const { accounts, selectedAccount, setSelectedAccount, saveAccounts, updateAccount } = useAccounts();
    const [showAddNewAccount, setShowAddNewAccount] = useState(false);    

    return (
        <Box id="accountspage"
            sx={{
                width: '100%',
                p: 2,
            }}
        >
            <AccountGrid acc={accounts} selected={selectedAccount} setSelected={setSelectedAccount} setShowAddNewAccount={setShowAddNewAccount} />
            <AccountDetails acc={showAddNewAccount ? null : selectedAccount} onClose={() => setSelectedAccount(null)} onAccountChanged={(updatedAccount) => updateAccount(updatedAccount, false)} />
            <AddNewAccount isOpen={showAddNewAccount} onClose={() => setShowAddNewAccount(false)} />
        </Box>
    );
}

export default AccountsPage;