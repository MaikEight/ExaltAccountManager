import { useMemo, useState, useCallback } from "react";
import { Box, Typography } from "@mui/material";
import useVaultPeeker from "../../../hooks/useVaultPeeker";
import AccountViewV2 from "./AccountViewV2";
import useDebugLogs from "../../../hooks/useDebugLogs";

/**
 * AccountsViewV2 - Virtualized list of accounts using react-virtuoso
 * 
 * Provides smooth infinite scroll for variable-height account components
 */
function AccountsViewV2() {
    const { accountsData, filter, isLoading } = useVaultPeeker();
    const [collapsedAccounts, setCollapsedAccounts] = useState(new Set());
    const { debugLogs } = useDebugLogs();
    // Filter accounts based on current filter state
    const filteredAccounts = useMemo(() => {
        if (!accountsData?.length) return [];

        // If character type filter is set to "not on character", we might want to hide accounts with no matching items
        // For now, we show all accounts and let AccountViewV2 handle item filtering
        return accountsData;
    }, [accountsData, filter]);

    // Debug logging
    if (debugLogs) {
        console.log('[AccountsViewV2] State:', {
            isLoading,
            accountsDataLength: accountsData?.length || 0,
            filteredAccountsLength: filteredAccounts.length
        });
    }

    const handleToggleCollapse = useCallback((email, collapsed) => {
        setCollapsedAccounts((prev) => {
            const next = new Set(prev);
            if (collapsed) {
                next.add(email);
            } else {
                next.delete(email);
            }
            return next;
        });
    }, []);

    // Show loading state - only show if actually loading and no data yet
    if (isLoading && filteredAccounts.length === 0) {
        return (
            <Box
                sx={{
                    display: 'flex',
                    alignItems: 'center',
                    justifyContent: 'center',
                    py: 4,
                    width: '100%',
                }}
            >
                <Typography variant="body2" color="text.secondary">
                    Loading accounts...
                </Typography>
            </Box>
        );
    }

    // Only show empty state if not loading and truly no accounts
    if (!isLoading && filteredAccounts.length === 0) {
        return (
            <Box
                sx={{
                    display: 'flex',
                    alignItems: 'center',
                    justifyContent: 'center',
                    py: 4,
                    width: '100%',
                }}
            >
                <Typography variant="body2" color="text.secondary">
                    No accounts found. Make sure accounts have been synced.
                </Typography>
            </Box>
        );
    }

    return (
        <Box
            sx={{
                display: 'flex',
                flexDirection: 'column',
                width: '100%',
            }}
        >
            {filteredAccounts.map((account, index) => (
                <AccountViewV2
                    key={account.email || index}
                    accountData={account}
                    collapsed={collapsedAccounts.has(account.email)}
                    onToggleCollapse={(collapsed) => handleToggleCollapse(account.email, collapsed)}
                />
            ))}
        </Box>
    );
}

export default AccountsViewV2;
