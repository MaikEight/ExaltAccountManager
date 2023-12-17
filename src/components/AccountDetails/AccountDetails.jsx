import { Box, Drawer, IconButton, Paper, Slide, Table, TableBody, TableCell, TableContainer, TableHead, TableRow, Typography, tableCellClasses } from "@mui/material";
import { Unstable_Grid2 as Grid } from "@mui/material";
import { useEffect, useState } from "react";
import { useTheme } from "@emotion/react";
import ComponentBox from "../ComponentBox";
import PaddedTableCell from "./PaddedTableCell";
import TextTableRow from "./TextTableRow";
import ServerTableRow from "./ServerTableRow";
import DailyLoginCheckBoxTableRow from "./DailyLoginCheckBoxTableRow";
import { formatTime } from "../../utils/timeUtils";
import StyledButton from './../StyledButton';
import CloseIcon from '@mui/icons-material/Close';
import PlayCircleFilledWhiteOutlinedIcon from '@mui/icons-material/PlayCircleFilledWhiteOutlined';
import RefreshOutlinedIcon from '@mui/icons-material/RefreshOutlined';
import DeleteOutlineOutlinedIcon from '@mui/icons-material/DeleteOutlineOutlined';

function AccountDetails({ acc, onClose }) {
    const [account, setAccount] = useState(null);
    const [isOpen, setIsOpen] = useState(false);

    const theme = useTheme();

    useEffect(() => {
        if (acc) {
            setAccount(acc);
            const timeoutId = setTimeout(() => {
                setIsOpen(true);
            }, 25);
            return () => clearTimeout(timeoutId);
        }

        setIsOpen(false);
        const timeoutId = setTimeout(() => {
            setAccount(null);
        }, 500);

        return () => clearTimeout(timeoutId);
    }, [acc]);

    if (!account) {
        return null;
    }

    return (
        <Drawer
            sx={{
                width: 500,
                flexShrink: 0,
                '& .MuiDrawer-paper': {
                    width: 500,
                    boxSizing: 'border-box',
                    backgroundColor: theme.palette.background.paperLight,
                    border: 'none',
                    borderRadius: '6px 0px 0px 6px',
                    boxShadow: ' 0px 0px 20px 10px rgba(0,0,0,0.2)',
                },
            }}
            variant="persistent"
            anchor="right"
            open={isOpen}
        >
            {
                /* 
                    1. close button - Account details
                    2. table with account details
                    3. buttons: play, edit, delete          
                */
            }
            {/* 1. */}
            <Box sx={{ display: 'flex', flexDirection: 'row', justifyContent: 'center', alignContent: 'center', height: 45, pt: 0.5 }}>
                <IconButton
                    sx={{ position: 'absolute', left: 16, marginLeft: 0, marginRight: 2 }}
                    size="small"
                    onClick={() => onClose()}
                >
                    <CloseIcon />
                </IconButton>
                <Typography variant="h6" component="div" sx={{ textAlign: 'center' }}>
                    Account details
                </Typography>
            </Box>
            {/* 2. */}
            <Box sx={{
                backgroundColor: theme.palette.background.default,
                width: '100%',
                height: '100%',
                pr: 2,
                pl: 2,
                pb: 2,
            }}>

                <Box
                    sx={{
                        display: 'flex',
                        flexDirection: 'column',
                        justifyContent: 'center',
                        alignItems: 'center',
                        width: '100%',
                    }}
                >
                    <ComponentBox headline="Details" sx={{ width: '100%' }}>
                        <TableContainer component={Box} sx={{ borderRadius: 0 }}>
                            <Table
                                sx={{
                                    '& tbody tr:last-child td, & tbody tr:last-child th': {
                                        borderBottom: 'none',
                                    },
                                }}
                            >
                                <TableHead>
                                    <TableRow>
                                        <PaddedTableCell>Attribute</PaddedTableCell>
                                        <PaddedTableCell>Value</PaddedTableCell>
                                    </TableRow>
                                </TableHead>
                                <TableBody>
                                    <TextTableRow key='name' keyValue={"Accountname"} value={account.name} />
                                    <TextTableRow key='email' keyValue={"Email"} value={account.email} />
                                    <TextTableRow key='lastLogin' keyValue={"Last login"} value={formatTime(account.lastLogin)} />
                                    <ServerTableRow key='server' keyValue={"Server"} value={account.server} />
                                    <DailyLoginCheckBoxTableRow key='dailyLogin' keyValue={"Daily login"} value={account.performDailyLogin} />
                                    <TextTableRow key='state' keyValue={"Last state"} value={account.state} innerSx={{ pb: 0 }} />
                                </TableBody>
                            </Table>
                        </TableContainer>
                    </ComponentBox>
                </Box>

                {/* 3. */}
                <Grid container spacing={2}>
                    <Grid xs={12}>
                        <StyledButton fullWidth={true}  sx={{height: 55}}>
                        <PlayCircleFilledWhiteOutlinedIcon size='large' sx={{mr: 1}}/>start game
                        </StyledButton>
                    </Grid>
                    <Grid xs={6}>
                        <StyledButton fullWidth={true} startIcon={<RefreshOutlinedIcon />} color="secondary">
                            refresh data
                        </StyledButton>
                    </Grid>
                    <Grid xs={6}>
                    <StyledButton fullWidth={true} startIcon={<DeleteOutlineOutlinedIcon />} color="error">
                        delete account
                    </StyledButton>
                    </Grid>

                </Grid>
                {/* <Box sx={{ display: 'flex', flexDirection: 'column', justifyContent: 'center', alignContent: 'center', gap: 2 }}>


                    
                    {/* <Typography variant="h6" component="div" sx={{ textAlign: 'center' }}>
                    Buttons
                </Typography>
                </Box> */}
            </Box>
        </Drawer >
    );
}

export default AccountDetails;