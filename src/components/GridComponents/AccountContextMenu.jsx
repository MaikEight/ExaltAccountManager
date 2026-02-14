import { MenuItem, ListItemIcon, ListItemText, Divider, Paper, MenuList, Popper, ClickAwayListener, Box, Typography, IconButton } from '@mui/material';
import PlayCircleFilledWhiteOutlinedIcon from '@mui/icons-material/PlayCircleFilledWhiteOutlined';
import ArrowUpwardOutlinedIcon from '@mui/icons-material/ArrowUpwardOutlined';
import ArrowDownwardOutlinedIcon from '@mui/icons-material/ArrowDownwardOutlined';
import GroupOutlinedIcon from '@mui/icons-material/GroupOutlined';
import { useState, useEffect, useRef } from 'react';
import useStartGame from '../../hooks/useStartGame.jsx';
import useAccounts from '../../hooks/useAccounts';
import { useGroups } from 'eam-commons-js';
import SwapVertOutlinedIcon from '@mui/icons-material/SwapVertOutlined';
import { GroupUI } from './GroupUI.jsx';
import CloseIcon from '@mui/icons-material/CloseOutlined';

/**
 * Context menu for DataGrid rows
 * Provides quick actions for individual accounts
 */
function AccountContextMenu({ anchorPosition, onClose, account, accounts }) {
    const { startGame } = useStartGame();
    const { updateAccount, updateAccounts } = useAccounts();
    const { groups } = useGroups();
    const [groupMenuAnchor, setGroupMenuAnchor] = useState(null);
    const [orderMenuAnchor, setOrderMenuAnchor] = useState(null);
    const [adjustedTop, setAdjustedTop] = useState(null);
    const [adjustedLeft, setAdjustedLeft] = useState(null);
    const menuRef = useRef(null);

    // Close submenus when the account or position changes (e.g., right-clicking a different row)
    useEffect(() => {
        setOrderMenuAnchor(null);
        setGroupMenuAnchor(null);
        setAdjustedTop(null);
        setAdjustedLeft(null);
    }, [account, anchorPosition]);

    // Adjust position if menu would overflow bottom or right of viewport
    useEffect(() => {
        if (anchorPosition && menuRef.current) {
            const menuHeight = menuRef.current.offsetHeight;
            const menuWidth = menuRef.current.offsetWidth;
            const viewportHeight = window.innerHeight;
            const viewportWidth = window.innerWidth;
            const bottomSpace = viewportHeight - anchorPosition.top;
            const rightSpace = viewportWidth - anchorPosition.left;
            
            // Check bottom overflow
            if (menuHeight > bottomSpace) {
                // Menu would overflow, position it 8px from bottom
                const newTop = viewportHeight - menuHeight - 8;
                setAdjustedTop(Math.max(8, newTop)); // Ensure it's at least 8px from top too
            } else {
                setAdjustedTop(anchorPosition.top);
            }

            // Check right overflow
            if (menuWidth > rightSpace) {
                // Menu would overflow, position it 8px from right
                const newLeft = viewportWidth - menuWidth - 8;
                setAdjustedLeft(Math.max(8, newLeft)); // Ensure it's at least 8px from left too
            } else {
                setAdjustedLeft(anchorPosition.left);
            }
        }
    }, [anchorPosition, menuRef.current]);

    const handleStartGame = async () => {
        onClose();
        await startGame(account);
    };

    const handleMainMenuClose = () => {
        setOrderMenuAnchor(null);
        setGroupMenuAnchor(null);
        onClose();
    };

    const handleChangeOrder = async (isUp) => {
        setOrderMenuAnchor(null);
        onClose();

        if (!account) return;

        const maxOrderId = Math.max(...accounts.map(acc => acc.orderId, ...accounts.map(acc => acc.id)), 0);
        const currentIndex = accounts.findIndex(acc => acc.id === account.id);
        const adjacentAcc = accounts[currentIndex + (isUp ? -1 : 1)];
        const currentOrderId = account.orderId;
        const newOrderId = adjacentAcc ? adjacentAcc.orderId : (isUp ? 0 : maxOrderId);

        // Check if we need to handle duplicate orderIds
        const hasCurrentDuplicates = accounts.filter(acc => acc.orderId === currentOrderId).length > 1;
        const hasTargetDuplicates = adjacentAcc && accounts.filter(acc => acc.orderId === newOrderId).length > 1;

        if (newOrderId === currentOrderId || hasCurrentDuplicates || hasTargetDuplicates) {
            // Handle duplicates by reordering all affected accounts
            const accountsToUpdate = [];

            // Collect all accounts with the current orderId
            for (let i = 0; i < accounts.length; i++) {
                if (accounts[i].orderId === currentOrderId) {
                    accountsToUpdate.push(accounts[i]);
                }
            }

            // Collect all accounts with the target orderId (if different)
            if (newOrderId !== currentOrderId) {
                for (let i = 0; i < accounts.length; i++) {
                    if (accounts[i].orderId === newOrderId && !accountsToUpdate.find(acc => acc.id === accounts[i].id)) {
                        accountsToUpdate.push(accounts[i]);
                    }
                }
            }

            // Sort by current position
            accountsToUpdate.sort((a, b) => {
                const indexA = accounts.findIndex(acc => acc.id === a.id);
                const indexB = accounts.findIndex(acc => acc.id === b.id);
                return indexA - indexB;
            });

            // Find starting orderId
            let startOrderId = 0;
            if (accountsToUpdate.length > 0) {
                const firstAccountIndex = accounts.findIndex(acc => acc.id === accountsToUpdate[0].id);
                if (firstAccountIndex > 0) {
                    const prevAccount = accounts[firstAccountIndex - 1];
                    startOrderId = prevAccount.orderId + 1;
                }
            }

            // Swap the current account with its adjacent account
            const currentAccountIndex = accountsToUpdate.findIndex(acc => acc.id === account.id);
            const adjacentAccountIndex = adjacentAcc ? accountsToUpdate.findIndex(acc => acc.id === adjacentAcc.id) : -1;

            if (currentAccountIndex !== -1 && adjacentAccountIndex !== -1) {
                [accountsToUpdate[currentAccountIndex], accountsToUpdate[adjacentAccountIndex]] =
                    [accountsToUpdate[adjacentAccountIndex], accountsToUpdate[currentAccountIndex]];
            } else if (currentAccountIndex !== -1) {
                const currentAcc = accountsToUpdate.splice(currentAccountIndex, 1)[0];
                if (isUp) {
                    accountsToUpdate.unshift(currentAcc);
                } else {
                    accountsToUpdate.push(currentAcc);
                }
            }

            // Assign sequential orderIds
            accountsToUpdate.forEach((acc, index) => {
                acc.orderId = startOrderId + index;
            });

            await updateAccounts(accountsToUpdate);
            return;
        }

        // Simple swap
        account.orderId = newOrderId;
        if (adjacentAcc) {
            adjacentAcc.orderId = currentOrderId;
            await updateAccounts([account, adjacentAcc]);
            return;
        }

        await updateAccount(account);
    };

    const handleChangeGroup = async (groupName) => {
        setGroupMenuAnchor(null);
        onClose();

        if (!account) return;

        const updatedAccount = { ...account, group: groupName };
        await updateAccount(updatedAccount, false);
    };

    // Check if account can move up or down
    const currentIndex = account ? accounts.findIndex(acc => acc.id === account.id) : -1;
    const canMoveUp = currentIndex > 0;
    const canMoveDown = currentIndex < accounts.length - 1 && currentIndex !== -1;

    // Determine if menu was repositioned (moved up or left)
    const wasRepositioned = adjustedTop !== null && anchorPosition && adjustedTop !== anchorPosition.top;

    return (
        <>
            {/* Main Context Menu */}
            {anchorPosition && (
                <ClickAwayListener onClickAway={handleMainMenuClose}>
                    <Box>
                        <Paper
                            ref={menuRef}
                            sx={{
                                position: 'fixed',
                                top: adjustedTop !== null ? adjustedTop : anchorPosition.top,
                                left: adjustedLeft !== null ? adjustedLeft : anchorPosition.left,
                                zIndex: 1300,
                                background: theme => theme.palette.background.default,
                                borderRadius: 1,
                                border: theme => `1px solid ${theme.palette.divider}`,
                            }}
                        >
                            <Box>
                                <Box
                                    sx={{
                                        display: 'flex',
                                        justifyContent: 'space-between',
                                        pl: 1,
                                        pr: 0.25,
                                        pt: 0.25,
                                    }}
                                >

                                <Typography variant="caption">{(account.name ? account.name : account.email) || 'Unknown Account'}</Typography>
                                <IconButton size="small" onClick={handleMainMenuClose} sx={{ color: theme => theme.palette.text.secondary, width: 16, height: 16 }}>
                                    <CloseIcon sx={{ fontSize: 14 }} />
                                </IconButton>
                                </Box>
                                <Divider />
                            </Box>
                            <MenuList>
                                <MenuItem onClick={handleStartGame}>
                                    <ListItemIcon>
                                        <PlayCircleFilledWhiteOutlinedIcon fontSize="small" />
                                    </ListItemIcon>
                                    <ListItemText>Start Game</ListItemText>
                                </MenuItem>

                                <Divider />

                                <MenuItem
                                    onMouseEnter={(e) => {
                                        setGroupMenuAnchor(null);
                                        setOrderMenuAnchor(e.currentTarget);
                                    }}
                                >
                                    <ListItemIcon>
                                        <SwapVertOutlinedIcon fontSize="small" />
                                    </ListItemIcon>
                                    <ListItemText>Change Order</ListItemText>
                                </MenuItem>

                                <MenuItem
                                    onMouseEnter={(e) => {
                                        setOrderMenuAnchor(null);
                                        setGroupMenuAnchor(e.currentTarget);
                                    }}
                                >
                                    <ListItemIcon>
                                        <GroupOutlinedIcon fontSize="small" />
                                    </ListItemIcon>
                                    <ListItemText>Change Group</ListItemText>
                                </MenuItem>
                            </MenuList>
                        </Paper>

                        {/* Submenu for Change Order */}
                        {orderMenuAnchor && (
                            <Popper
                                open={Boolean(orderMenuAnchor)}
                                anchorEl={orderMenuAnchor}
                                placement="right-start"
                                style={{ zIndex: 1301 }}
                                modifiers={[
                                    {
                                        name: 'offset',
                                        options: {
                                            offset: [0, 0],
                                        },
                                    },
                                ]}
                            >
                                <Paper
                                    sx={{
                                        background: theme => theme.palette.background.default,
                                        borderRadius: theme => wasRepositioned 
                                            ? `0 ${theme.shape.borderRadius}px ${theme.shape.borderRadius}px 0`
                                            : `0 ${theme.shape.borderRadius}px ${theme.shape.borderRadius}px ${theme.shape.borderRadius}px`,
                                        border: theme => `1px solid ${theme.palette.divider}`,
                                    }}
                                >
                                    <MenuList>
                                        <MenuItem
                                            onClick={() => handleChangeOrder(true)}
                                            disabled={!canMoveUp}
                                        >
                                            <ListItemIcon>
                                                <ArrowUpwardOutlinedIcon fontSize="small" />
                                            </ListItemIcon>
                                            <ListItemText>Move Up</ListItemText>
                                        </MenuItem>
                                        <MenuItem
                                            onClick={() => handleChangeOrder(false)}
                                            disabled={!canMoveDown}
                                        >
                                            <ListItemIcon>
                                                <ArrowDownwardOutlinedIcon fontSize="small" />
                                            </ListItemIcon>
                                            <ListItemText>Move Down</ListItemText>
                                        </MenuItem>
                                    </MenuList>
                                </Paper>
                            </Popper>
                        )}

                        {/* Submenu for Change Group */}
                        {groupMenuAnchor && (
                            <Popper
                                open={Boolean(groupMenuAnchor)}
                                anchorEl={groupMenuAnchor}
                                placement="right-start"
                                style={{ zIndex: 1301 }}
                                modifiers={[
                                    {
                                        name: 'offset',
                                        options: {
                                            offset: [0, 0],
                                        },
                                    },
                                ]}
                            >
                                <Paper
                                    sx={{
                                        background: theme => theme.palette.background.default,
                                        borderRadius: theme => wasRepositioned 
                                            ? `${theme.shape.borderRadius}px ${theme.shape.borderRadius}px ${theme.shape.borderRadius}px 0`
                                            : `0 ${theme.shape.borderRadius}px ${theme.shape.borderRadius}px ${theme.shape.borderRadius}px`,
                                        border: theme => `1px solid ${theme.palette.divider}`,
                                    }}
                                >
                                    <MenuList>
                                        {groups.map((group) => (
                                            <MenuItem
                                                key={group.name}
                                                onClick={() => handleChangeGroup(group.name)}
                                            >
                                                <Box sx={{scale: 0.85}}><GroupUI group={group}/></Box>
                                                <ListItemText sx={{ marginLeft: 1 }}>{group.name}</ListItemText>
                                            </MenuItem>
                                        ))}
                                    </MenuList>
                                </Paper>
                            </Popper>
                        )}
                    </Box>
                </ClickAwayListener>
            )}
        </>
    );
}

export default AccountContextMenu;
