import { useTheme } from "@emotion/react";
import { Box, Fade, Paper, Popover, Table, TableBody, TableCell, TableContainer, TableHead, TableRow, Tooltip, Typography } from "@mui/material";
import { useEffect, useState } from "react";
import { getItemById } from "../../utils/realmItemUtils";
import { useNavigate } from "react-router-dom";
import LockOutlinedIcon from '@mui/icons-material/LockOutlined';
import useAccounts from './../../hooks/useAccounts';

function ItemLocationPopper({ open, position, selectedItem, onClose }) {
    const [item, setItem] = useState(null);
    const theme = useTheme();
    const navigate = useNavigate();
    const { getAccountByEmail } = useAccounts();
    const [accounts, setAccounts] = useState([]);

    useEffect(() => {
        if (!selectedItem) {
            if (item) {
                const timeout = setTimeout(() => {
                    setItem(null);
                }, 150);
                return () => clearTimeout(timeout);
            }
            return;
        }

        setItem(getItemById(selectedItem.itemId));
        const acc = [];
        if (!selectedItem.totals.location) {
            setAccounts(acc);
            return;
        }
        Object.keys(selectedItem.totals.location).forEach((location) => {
            acc.push(getAccountByEmail(location));
        });
        setAccounts(acc);
    }, [selectedItem]);

    const getTierText = () => {
        if (!item) {
            return null;
        }

        if (item[2] === -1) {
            // UT / ST
            if (item[9] === 0) {
                return null;
            }

            return (
                <Typography
                    variant="h6"
                    sx={{
                        color: item[9] === 2 ? theme.palette.warning.main : theme.palette.primary.main,
                    }}
                >
                    {item[9] === 2 ? 'ST' : 'UT'}
                </Typography>
            );
        }

        return (
            <Typography
                variant="h6"
            >
                T{item[2]}
            </Typography>
        );
    };

    const getContent = () => {
        if (!item) {
            return null;
        }

        return (
            <Paper
                sx={{
                    padding: item ? 0.25 : 0,
                    backgroundColor: theme.palette.background.paper,
                    borderRadius: `${theme.shape.borderRadius}px`,
                }}
            >
                {/* Headline */}
                <Box
                    sx={{
                        display: 'flex',
                        flexDirection: 'row',
                        justifyContent: 'space-between',
                        alignItems: 'center',
                        pr: 1,
                    }}
                >
                    <Box
                        sx={{
                            display: 'flex',
                            flexDirection: 'row',
                            alignItems: 'center',
                            padding: 1,
                            gap: 1,
                        }}
                    >
                        {getTierText()}
                        <Typography variant="h6">
                            {item[0]}
                        </Typography>
                    </Box>
                    {
                        item[8] === true &&
                        <Tooltip title="Soulbound">
                            <LockOutlinedIcon />
                        </Tooltip>
                    }
                </Box>
                {/* Location */}
                <Box
                    sx={{
                        padding: 1,
                        borderRadius: `${theme.shape.borderRadius}px`,
                        backgroundColor: theme.palette.background.default,
                    }}
                >
                    {
                        selectedItem?.totals.location &&
                        <Box
                            sx={{
                                pl: 1,
                                pr: 1,
                                pt: 0.5,
                            }}
                        >
                            <Typography fontWeight={100}>
                                You own {selectedItem?.totals.amount} of this item in total acros {Object.keys(selectedItem?.totals?.location).length} accounts.
                            </Typography>
                        </Box>
                    }
                    {
                        selectedItem?.totals.location &&
                        <TableContainer>
                            <Table
                                sx={{
                                    '& thead th': {
                                        borderBottom: 'none',
                                    },
                                    '& tbody tr:last-child td, & tbody tr:last-child th': {
                                        borderBottom: 'none',
                                    },
                                }}
                            >
                                <TableHead>
                                    <TableRow>
                                        <TableCell>
                                            <Typography fontSize={'14px'} fontWeight={'bold'}>
                                                Account
                                            </Typography>
                                        </TableCell>
                                        <TableCell>
                                            <Tooltip title="Vault">
                                                <img src="realm/vault_portal.png" alt="Vault" style={{ padding: '0', maxWidth: 32, maxHeight: 32 }} />
                                            </Tooltip>
                                        </TableCell>
                                        <TableCell>
                                            <Tooltip title="Gift Chest">
                                                <img src="realm/gift_chest.png" alt="Vault" style={{ padding: '0', maxWidth: 32, maxHeight: 32 }} />
                                            </Tooltip>
                                        </TableCell>
                                        <TableCell>
                                            <Tooltip title="Material Storage">
                                                <img src="realm/material_storage.png" alt="Vault" style={{ padding: '0', maxWidth: 32, maxHeight: 32 }} />
                                            </Tooltip>
                                        </TableCell>
                                        <TableCell>
                                            <Tooltip title="Temporary Gifts">
                                                <img src="realm/chest.png" alt="Vault" style={{ padding: '0', maxWidth: 32, maxHeight: 32 }} />
                                            </Tooltip>
                                        </TableCell>
                                        <TableCell>
                                            <Tooltip title="Potion Storage">
                                                <img src="realm/potion_storage_small.png" alt="Vault" style={{ padding: '0', maxWidth: 32, maxHeight: 32 }} />
                                            </Tooltip>
                                        </TableCell>
                                    </TableRow>
                                </TableHead>
                                <TableBody>
                                    {
                                        Object.keys(selectedItem.totals.location).map((location) => {
                                            return (
                                                <TableRow
                                                    key={location}
                                                    hover
                                                    sx={{
                                                        cursor: 'pointer',
                                                        userSelect: 'none',
                                                    }}
                                                    onClick={() => {
                                                        navigate(`/account?selectedAccount=${location}`);
                                                        onClose();
                                                    }}
                                                >
                                                    <TableCell sx={{ borderBottom: 'none', textAlign: 'start', borderRadius: `${theme.shape.borderRadius}px 0 0 ${theme.shape.borderRadius}px` }}>
                                                        {accounts?.find(acc => acc.email === location)?.name || location}
                                                    </TableCell>
                                                    <TableCell sx={{ borderBottom: 'none', textAlign: 'center' }}>
                                                        {selectedItem.totals.location[location].vault}
                                                    </TableCell>
                                                    <TableCell sx={{ borderBottom: 'none', textAlign: 'center' }}>
                                                        {selectedItem.totals.location[location].gift}
                                                    </TableCell>
                                                    <TableCell sx={{ borderBottom: 'none', textAlign: 'center' }}>
                                                        {selectedItem.totals.location[location].materialStorage}
                                                    </TableCell>
                                                    <TableCell sx={{ borderBottom: 'none', textAlign: 'center' }}>
                                                        {selectedItem.totals.location[location].temporaryGifts}
                                                    </TableCell>
                                                    <TableCell sx={{ borderBottom: 'none', textAlign: 'center', borderRadius: `0 ${theme.shape.borderRadius}px ${theme.shape.borderRadius}px 0` }}>
                                                        {selectedItem.totals.location[location].potions}
                                                    </TableCell>
                                                </TableRow>
                                            );
                                        })
                                    }
                                </TableBody>
                            </Table>
                        </TableContainer>
                    }
                </Box>
            </Paper>
        );
    };

    return (
        <Popover
            open={open && Boolean(item)}
            anchorReference="anchorPosition"
            anchorPosition={position}
            anchorOrigin={{
                vertical: 'top',
                horizontal: 'left',
            }}
            placement='top'
            transition
            onClose={onClose}
        >
            {getContent()}
        </Popover>
    );
}

export default ItemLocationPopper;