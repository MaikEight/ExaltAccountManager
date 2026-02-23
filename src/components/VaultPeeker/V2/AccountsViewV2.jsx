import { useMemo, useState, useCallback } from "react";
import { Box, Typography, Pagination, Select, MenuItem, FormControl, InputLabel } from "@mui/material";
import { useTheme } from "@emotion/react";
import useVaultPeeker from "../../../hooks/useVaultPeeker";
import AccountViewV2 from "./AccountViewV2";
import useDebugLogs from "../../../hooks/useDebugLogs";
import useUserSettings from "../../../hooks/useUserSettings";

// Page size options
const PAGE_SIZE_OPTIONS = [25, 50, 100, 'all'];
const DEFAULT_PAGE_SIZE = 25;

/**
 * AccountsViewV2 - Paginated list of accounts
 * 
 * Features paginated display while keeping totals calculated from all accounts
 */
function AccountsViewV2() {
    const theme = useTheme();
    const { accountsData, filter, isLoading } = useVaultPeeker();
    const [collapsedAccounts, setCollapsedAccounts] = useState(new Set());
    const { getByKeyAndSubKey, setByKeyAndSubKey } = useUserSettings();
    const rowsPerPage = getByKeyAndSubKey('vaultPeeker', 'rowsPerPage') || DEFAULT_PAGE_SIZE;
    const { debugLogs } = useDebugLogs();
    
    // Pagination state
    const [page, setPage] = useState(1);
    const [pageSize, setPageSize] = useState(rowsPerPage);
    // Filter accounts based on current filter state
    const filteredAccounts = useMemo(() => {
        if (!accountsData?.length) return [];

        // If character type filter is set to "not on character", we might want to hide accounts with no matching items
        // For now, we show all accounts and let AccountViewV2 handle item filtering
        return accountsData;
    }, [accountsData, filter]);

    // Calculate pagination
    const totalAccounts = filteredAccounts.length;
    const effectivePageSize = pageSize === 'all' ? totalAccounts : pageSize;
    const totalPages = effectivePageSize > 0 ? Math.ceil(totalAccounts / effectivePageSize) : 1;
    
    // Reset page if it exceeds total pages
    const currentPage = Math.min(page, totalPages || 1);
    
    // Get accounts for current page
    const paginatedAccounts = useMemo(() => {
        if (pageSize === 'all') return filteredAccounts;
        
        const startIndex = (currentPage - 1) * pageSize;
        const endIndex = startIndex + pageSize;
        return filteredAccounts.slice(startIndex, endIndex);
    }, [filteredAccounts, currentPage, pageSize]);

    // Debug logging
    if (debugLogs) {
        console.log('[AccountsViewV2] State:', {
            isLoading,
            accountsDataLength: accountsData?.length || 0,
            filteredAccountsLength: filteredAccounts.length,
            paginatedAccountsLength: paginatedAccounts.length,
            currentPage,
            totalPages,
            pageSize,
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

    const handlePageChange = useCallback((event, newPage) => {
        setPage(newPage);
        // Scroll to top of accounts list
        document.getElementById('vault-peeker-content-root')?.scrollIntoView({ behavior: 'smooth' });
    }, []);

    const handlePageSizeChange = useCallback((event) => {
        const newSize = event.target.value;
        setPageSize(newSize);
        setPage(1);
        setByKeyAndSubKey('vaultPeeker', 'rowsPerPage', newSize);
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
            {/* Accounts List */}
            {paginatedAccounts.map((account, index) => (
                <AccountViewV2
                    key={account.email || index}
                    accountData={account}
                    collapsed={collapsedAccounts.has(account.email)}
                    onToggleCollapse={(collapsed) => handleToggleCollapse(account.email, collapsed)}
                />
            ))}

            {/* Pagination Controls */}
            {totalAccounts > 0 && totalPages > 1 && (
                <Box
                    sx={{
                        display: 'flex',
                        flexDirection: 'row',
                        alignItems: 'center',
                        justifyContent: 'space-between',
                        gap: 2,
                        mt: 2,
                        p: 1.5,
                        backgroundColor: theme.palette.background.paper,
                        borderRadius: 1,
                        border: '1px solid',
                        borderColor: 'divider',
                    }}
                >
                    {/* Page info */}
                    <Typography variant="body2" color="text.secondary">
                        Showing {((currentPage - 1) * effectivePageSize) + 1}-{Math.min(currentPage * effectivePageSize, totalAccounts)} of {totalAccounts} accounts
                    </Typography>

                    {/* Pagination */}
                    <Box sx={{ display: 'flex', alignItems: 'center', gap: 2 }}>
                        {/* Page size selector */}
                        <FormControl size="small" sx={{ minWidth: 100 }}>
                            <InputLabel id="page-size-label">Per page</InputLabel>
                            <Select
                                labelId="page-size-label"
                                value={pageSize}
                                label="Per page"
                                onChange={handlePageSizeChange}
                                sx={{ fontSize: '0.875rem' }}
                            >
                                {PAGE_SIZE_OPTIONS.map((option) => (
                                    <MenuItem key={option} value={option}>
                                        {option === 'all' ? 'All' : option}
                                    </MenuItem>
                                ))}
                            </Select>
                        </FormControl>

                        {/* Page navigation */}
                        {pageSize !== 'all' && (
                            <Pagination
                                count={totalPages}
                                page={currentPage}
                                onChange={handlePageChange}
                                color="primary"
                                size="small"
                                showFirstButton
                                showLastButton
                            />
                        )}
                    </Box>
                </Box>
            )}
        </Box>
    );
}

export default AccountsViewV2;
