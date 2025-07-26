import { Box, Button, ButtonGroup, Paper } from "@mui/material";
import ArrowUpwardOutlinedIcon from '@mui/icons-material/ArrowUpwardOutlined';
import ArrowDownwardOutlinedIcon from '@mui/icons-material/ArrowDownwardOutlined';
import { useTheme } from "@emotion/react";
import useAccounts from "../../hooks/useAccounts";

function FloatingSelectedRowComponent({ account, position, onAccountUpdated }) {
    const theme = useTheme();
    const { accounts, updateAccount, updateAccounts } = useAccounts();
    // Search for the maximum orderId in accounts
    const maxOrderId = Math.max(...accounts.map(acc => acc.orderId, ...accounts.map(acc => acc.id)), 0);
    if (!account || !position) return null;

    // Check if the account is actually at the first or last visual position
    const currentIndex = accounts.findIndex(acc => acc.id === account.id);
    const isVisuallyFirst = currentIndex === 0;
    const isVisuallyLast = currentIndex === accounts.length - 1;

    // Swap orderId of the account when clicking the buttons
    // So the account moves up or down in the list
    // We need to update the current account's orderId and the orderId of the adjacent account (up or down)
    // If the account is already at the top or bottom, disable the respective button    
    const handleClick = async (isUp) => {
        const currentOrderId = account.orderId;
        // const newOrderId = isUp ? Math.max(account.orderId - 1, 0) : Math.min(account.orderId + 1, maxOrderId);
        const currentIndex = accounts.findIndex(acc => acc.id === account.id);
        const adjacentAcc = accounts[currentIndex + (isUp ? -1 : 1)];
        const newOrderId = adjacentAcc ? adjacentAcc.orderId : (isUp ? 0 : maxOrderId);
        console.log(`Swapping accounts: ${account.name} (orderId: ${currentOrderId}) with ${adjacentAcc ? adjacentAcc.name : 'none'} (orderId: ${newOrderId})`);

        // Check if we need to handle duplicate orderIds (either same orderIds or duplicates at target position)
        const hasCurrentDuplicates = accounts.filter(acc => acc.orderId === currentOrderId).length > 1;
        const hasTargetDuplicates = adjacentAcc && accounts.filter(acc => acc.orderId === newOrderId).length > 1;
        
        if(newOrderId === currentOrderId || hasCurrentDuplicates || hasTargetDuplicates) {            
            // If this happens, we need to swap more than just these two accounts orderIds, we need to update all accounts above / below with a new orderId until we reach a gap in the orderIds
            // Find all accounts that need to be updated and update their orderIds (either up or down by 1)
            // We need to update all accounts until we find a free orderId ( the list may be 1, 2, 3, 4, 6 ...)
            // This means we need to update until we find the free orderId (5 in this case)
            // After the update we will not have a duplicate orderId anymore
            const accountsToUpdate = [];
            
            // Collect ALL accounts with duplicate orderIds from both the current and target positions
            // This ensures we handle cases where either account has duplicates
            
            // First, collect all accounts with the current orderId
            for (let i = 0; i < accounts.length; i++) {
                if (accounts[i].orderId === currentOrderId) {
                    accountsToUpdate.push(accounts[i]);
                }
            }
            
            // Then, collect all accounts with the target orderId (if different)
            if (newOrderId !== currentOrderId) {
                for (let i = 0; i < accounts.length; i++) {
                    if (accounts[i].orderId === newOrderId && !accountsToUpdate.find(acc => acc.id === accounts[i].id)) {
                        accountsToUpdate.push(accounts[i]);
                    }
                }
            }
            
            // Sort the collected accounts by their current position in the accounts array to maintain visual order
            accountsToUpdate.sort((a, b) => {
                const indexA = accounts.findIndex(acc => acc.id === a.id);
                const indexB = accounts.findIndex(acc => acc.id === b.id);
                return indexA - indexB;
            });
            
            // Find the starting orderId (lowest position for our group)
            let startOrderId = 0;
            if (accountsToUpdate.length > 0) {
                const firstAccountIndex = accounts.findIndex(acc => acc.id === accountsToUpdate[0].id);
                if (firstAccountIndex > 0) {
                    const prevAccount = accounts[firstAccountIndex - 1];
                    startOrderId = prevAccount.orderId + 1;
                }
            }
            
            // Now reposition the accounts: swap the current account with its adjacent account
            const currentAccountIndex = accountsToUpdate.findIndex(acc => acc.id === account.id);
            const adjacentAccountIndex = adjacentAcc ? accountsToUpdate.findIndex(acc => acc.id === adjacentAcc.id) : -1;
            
            if (currentAccountIndex !== -1 && adjacentAccountIndex !== -1) {
                // Swap the two accounts in the array
                [accountsToUpdate[currentAccountIndex], accountsToUpdate[adjacentAccountIndex]] = 
                [accountsToUpdate[adjacentAccountIndex], accountsToUpdate[currentAccountIndex]];
            } else if (currentAccountIndex !== -1) {
                // If adjacent account is not in our list, move current account to appropriate position
                const currentAcc = accountsToUpdate.splice(currentAccountIndex, 1)[0];
                if (isUp) {
                    // Move to the beginning
                    accountsToUpdate.unshift(currentAcc);
                } else {
                    // Move to the end
                    accountsToUpdate.push(currentAcc);
                }
            }
            
            // Now assign sequential orderIds
            accountsToUpdate.forEach((acc, index) => {
                acc.orderId = startOrderId + index;
            });

            console.log(`Updating accounts with new orderIds: ${accountsToUpdate.map(acc => `${acc.name}: ${acc.orderId}`).join(', ')}`);
            
            await updateAccounts(accountsToUpdate);
            console.log(`Done updating accounts with new orderIds.`);
            
            onAccountUpdated?.(crypto.randomUUID());
            return;
        }
        
        account.orderId = newOrderId;

        if (adjacentAcc) {
            adjacentAcc.orderId = currentOrderId; // Set the adjacent account's orderId to the current account's orderId
            await updateAccounts([account, adjacentAcc]);

            onAccountUpdated?.(crypto.randomUUID());
            return;
        }

        await updateAccount(account);
        onAccountUpdated?.(crypto.randomUUID());
    }

    return (
        <Paper
            sx={{
                position: 'absolute',
                top: position.top,
                left: position.left,
                backgroundColor: 'background.paper',
                color: 'primary.contrastText',
                borderRadius: `${theme.shape.borderRadius}px`,
                zIndex: 1000,
                transformOrigin: 'center',
                animation: 'growIn 0.2s ease-out',
                '@keyframes growIn': {
                    '0%': {
                        transform: 'scale(0)',
                        opacity: 0,
                    },
                    '100%': {
                        transform: 'scale(1)',
                        opacity: 1,
                    },
                },
            }}
        >
            <Box
                sx={{
                    display: 'flex',
                    backgroundColor: 'background.paper',
                    borderRadius: `${theme.shape.borderRadius}px`,
                }}
            >
                <ButtonGroup
                    variant="outlined"
                    aria-label="Basic button group"
                    color="primary"
                    size="small"
                >
                    <Button
                        disabled={isVisuallyFirst}
                        onClick={() => handleClick(true)}
                        size="small"
                        sx={{
                            borderRadius: `${theme.shape.borderRadius - 2}px 0 0 ${theme.shape.borderRadius}px`,
                        }}
                    >
                        <ArrowUpwardOutlinedIcon />
                    </Button>
                    <Button
                        disabled={isVisuallyLast}
                        onClick={() => handleClick(false)}
                        size="small"
                        sx={{
                            borderRadius: `0 ${theme.shape.borderRadius - 2}px ${theme.shape.borderRadius}px 0`,
                        }}
                    >
                        <ArrowDownwardOutlinedIcon />
                    </Button>
                </ButtonGroup>
            </Box>
        </Paper>
    );
};

export default FloatingSelectedRowComponent;