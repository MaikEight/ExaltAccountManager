import { Box } from "@mui/material";
import AccountGrid from "../components/AccountGrid";
import { useEffect, useState } from "react";
import AccountDetails from "../components/AccountDetails/AccountDetails";
import AddNewAccount from "../components/AddNewAccount/AddNewAccount";
import useAccounts from "../hooks/useAccounts";
import useWidgets from '../hooks/useWidgets';
import { WidgetBarEvents, WidgetBars } from "../components/Widgets/Widgetbars";

function AccountsPage() {
    const { selectedAccount, setSelectedAccount } = useAccounts();
    const { showWidgetBar, closeWidgetBar, updateWidgetBarData, widgetBarState, subscribeToEvent } = useWidgets() || {};
    const [showAddNewAccount, setShowAddNewAccount] = useState(false);

    useEffect(() => {
        const unsubscribe = subscribeToEvent(WidgetBarEvents.ON_CLOSE, () => {
            setSelectedAccount(null);
        });

        return () => {
            closeWidgetBar();
            unsubscribe();
        };
    }, []);

    useEffect(() => {
        updateWidgetBarData(selectedAccount);

        if (selectedAccount && !widgetBarState?.isOpen) {
            showWidgetBar(WidgetBars.ACCOUNT, selectedAccount);
            return;
        }

        if (!selectedAccount && widgetBarState?.isOpen) {
            closeWidgetBar();
        }
    }, [selectedAccount]);

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
            {/* <AccountDetails acc={showAddNewAccount ? null : selectedAccount} onClose={() => setSelectedAccount(null)} /> */}
            <AddNewAccount isOpen={showAddNewAccount} onClose={() => setShowAddNewAccount(false)} />
        </Box>
    );
}

export default AccountsPage;