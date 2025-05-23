import { useTheme } from "@emotion/react";
import { Box, IconButton, Paper, Popover, Table, TableBody, TableCell, TableContainer, TableHead, TableRow, Tooltip, Typography } from "@mui/material";
import { useEffect, useState } from "react";
import { getItemById } from "../../utils/realmItemUtils";
import { useNavigate } from "react-router-dom";
import LockOutlinedIcon from '@mui/icons-material/LockOutlined';
import useAccounts from './../../hooks/useAccounts';
import SingleItemCanvas from "./SingleItemCanvas";

function ItemLocationPopper({ open, position, selectedItem, onClose }) {
    const [item, setItem] = useState(null);
    const [totals, setTotals] = useState(null);
    const theme = useTheme();
    const navigate = useNavigate();
    const { getAccountByEmail } = useAccounts();
    const [accounts, setAccounts] = useState([]);

    useEffect(() => {
        if (!selectedItem) {
            if (item) {
                const timeout = setTimeout(() => {
                    setItem(null);
                    setTotals(null);
                }, 150);
                return () => clearTimeout(timeout);
            }
            return;
        }

        setItem(getItemById(selectedItem.itemId));
        const acc = [];
        if (!selectedItem.totals?.location) {
            setAccounts(acc);
            return;
        }
        setTotals(selectedItem.totals);
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
                    backgroundColor: theme.palette.mode === 'dark' ? theme.palette.background.paper : theme.palette.background.paper,
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
                            gap: 1,
                            px: 0.5,
                        }}
                    >
                        {
                            item[0] !== 'Empty Slot' &&
                            <Box
                                sx={{
                                    my: 0.5,
                                    height: '40px',
                                    width: '40px',
                                    display: 'flex',
                                    justifyContent: 'center',
                                    alignItems: 'center',
                                    borderRadius: `${theme.shape.borderRadius}px`,
                                }}
                            >
                                <SingleItemCanvas item={item} doTransition/>
                            </Box>
                        }
                        {getTierText()}
                        <Typography variant="h6">
                            {item[0]}
                        </Typography>
                    </Box>
                    <Box
                        sx={{
                            display: 'flex',
                            flexDirection: 'row',
                            alignItems: 'center',
                            gap: 1,
                        }}
                    >
                        {
                            item[6] > 0 &&
                            <Tooltip title="Feed Power">
                                <Typography
                                    variant="h6"
                                    sx={{

                                    }}
                                >
                                    {item[6]}FP
                                </Typography>
                            </Tooltip>
                        }
                        {
                            item[8] === true &&
                            <Tooltip title="Soulbound">
                                <LockOutlinedIcon />
                            </Tooltip>
                        }
                    </Box>
                </Box>
                {/* Location */}
                <Box
                    sx={{
                        display: 'flex',
                        flexDirection: 'column',
                        overflowX: 'auto',
                        maxHeight: '500px',
                    }}
                >
                    {
                        selectedItem?.itemId !== -1 &&
                        totals?.location &&
                        <Box
                            sx={{
                                padding: 1,
                                borderRadius: `${theme.shape.borderRadius - 2}px`,
                                backgroundColor: theme.palette.mode === 'dark' ? theme.palette.background.default : theme.palette.background.paperLight,
                            }}
                        >
                            {
                                totals?.location &&
                                <Box
                                    sx={{
                                        display: 'flex',
                                        flexDirection: 'row',
                                        justifyContent: 'space-between',
                                        pl: 1,
                                        pt: 0.5,
                                    }}
                                >
                                    <Typography fontWeight={100}>
                                        You own {totals?.amount} of this item in total across {Object.keys(totals?.location).length} accounts.
                                    </Typography>
                                    <Tooltip title="Search on realmeye">
                                        <a href={`https://www.realmeye.com/wiki-search?q=${encodeURI(item[0])}`} target="_blank" rel="noreferrer">
                                            <IconButton
                                                size="small"
                                                sx={{ zIndex: 10, mt: -0.5 }}
                                            >
                                                <img src="realm/eye-big.png" alt="Realmeye" style={{ padding: '0', maxWidth: 30, maxHeight: 30 }} />
                                            </IconButton>
                                        </a>
                                    </Tooltip>
                                </Box>
                            }
                            {
                                totals?.location &&
                                <TableContainer
                                    sx={{
                                        maxHeight: '280px',
                                        overflowY: 'auto',
                                        mt: -1,
                                    }}
                                >
                                    <Table
                                        stickyHeader
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
                                                        <img src="realm/gift_chest.png" alt="Gift" style={{ padding: '0', maxWidth: 32, maxHeight: 32 }} />
                                                    </Tooltip>
                                                </TableCell>
                                                <TableCell>
                                                    <Tooltip title="Material Storage">
                                                        <img src="realm/material_storage.png" alt="Mat. Storage" style={{ padding: '0', maxWidth: 32, maxHeight: 32 }} />
                                                    </Tooltip>
                                                </TableCell>
                                                <TableCell>
                                                    <Tooltip title="Seasonal Spoils">
                                                        <img src="realm/seasonal_spoils_chest.png" alt="Ssnl. Spoils" style={{ padding: '0', maxWidth: 32, maxHeight: 32 }} />
                                                    </Tooltip>
                                                </TableCell>
                                                <TableCell>
                                                    <Tooltip title="Potion Storage">
                                                        <img src="realm/potion_storage_small.png" alt="Pot. Storage" style={{ padding: '0', maxWidth: 32, maxHeight: 32 }} />
                                                    </Tooltip>
                                                </TableCell>
                                                <TableCell>
                                                    <Tooltip title="Character">
                                                        <img src="realm/character.png" alt="Chars" style={{ padding: '0', maxWidth: 32, maxHeight: 32 }} />
                                                    </Tooltip>
                                                </TableCell>
                                            </TableRow>
                                        </TableHead>
                                        <TableBody>
                                            {
                                                Object.keys(totals.location).map((location) => {
                                                    return (
                                                        <Tooltip
                                                            title={"Click to open the Account Details of this account"}
                                                            placement="bottom-end"
                                                            key={location}
                                                        >
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
                                                                    {totals.location[location].vault}
                                                                </TableCell>
                                                                <TableCell sx={{ borderBottom: 'none', textAlign: 'center' }}>
                                                                    {totals.location[location].gift}
                                                                </TableCell>
                                                                <TableCell sx={{ borderBottom: 'none', textAlign: 'center' }}>
                                                                    {totals.location[location].materialStorage}
                                                                </TableCell>
                                                                <TableCell sx={{ borderBottom: 'none', textAlign: 'center' }}>
                                                                    {totals.location[location].temporaryGifts}
                                                                </TableCell>
                                                                <TableCell sx={{ borderBottom: 'none', textAlign: 'center' }}>
                                                                    {totals.location[location].potions}
                                                                </TableCell>
                                                                <TableCell sx={{ borderBottom: 'none', textAlign: 'center', borderRadius: `0 ${theme.shape.borderRadius}px ${theme.shape.borderRadius}px 0` }}>
                                                                    {
                                                                        totals.location[location].character
                                                                    }
                                                                </TableCell>
                                                            </TableRow>
                                                        </Tooltip>
                                                    );
                                                })
                                            }
                                        </TableBody>
                                    </Table>
                                </TableContainer>

                            }
                        </Box>
                    }
                </Box>
            </Paper >
        );
    };

    return (
        <Popover
            open={open && Boolean(item)}
            anchorReference="anchorPosition"
            transformOrigin={{
                vertical: 'top',
                horizontal: position?.isLeftHalf ? 'left' : 'right',
            }}
            anchorPosition={position}
            placement='top'
            onClose={onClose}
        >
            {getContent()}
        </Popover>
    );
}

export default ItemLocationPopper;