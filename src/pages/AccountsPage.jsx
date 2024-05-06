import { Box } from "@mui/material";
import AccountGrid from "../components/AccountGrid";
import { useState } from "react";
import AccountDetails from "../components/AccountDetails/AccountDetails";
import AddNewAccount from "../components/AddNewAccount";
import useAccounts from "../hooks/useAccounts";

function AccountsPage() {
    const { selectedAccount, setSelectedAccount } = useAccounts();
    const [showAddNewAccount, setShowAddNewAccount] = useState(false);

    return (
        <Box id="accountspage"
            sx={{
                width: '100%', 
                minWidth: '100px',
                overflow: 'hidden',                
                p: 2,
            }}
        >
            <AccountGrid setShowAddNewAccount={setShowAddNewAccount} />
            <AccountDetails acc={showAddNewAccount ? null : selectedAccount} onClose={() => setSelectedAccount(null)} />
            <AddNewAccount isOpen={showAddNewAccount} onClose={() => setShowAddNewAccount(false)} />
        </Box>
    );
}

export default AccountsPage;