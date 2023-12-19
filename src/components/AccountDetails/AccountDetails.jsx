import { Box, Drawer, IconButton, Table, TableBody, TableContainer, TableHead, TableRow, Typography } from "@mui/material";
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
import { postAccountVerify, postCharList } from "../../backend/decaApi";
import { tauri } from "@tauri-apps/api";
import useUserSettings from "../../hooks/useUserSettings";

function AccountDetails({ acc, onClose, onAccountChanged }) {
    const [account, setAccount] = useState(null);
    const [isOpen, setIsOpen] = useState(false);

    const settings = useUserSettings();
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
                        <StyledButton
                            fullWidth={true}
                            sx={{ height: 55 }}
                            onClick={() => {
                                // const arg = `data:{platform:Deca,guid:dW5kZWZpbmVk,token:dW5kZWZpbmVk,tokenTimestamp:dW5kZWZpbmVk,tokenExpiration:dW5kZWZpbmVk,env:4,serverName:Default}`;
                                // const path = settings.getByKeyAndSubKey("game", "exePath");
                                // console.log("path", path);
                                // tauri.invoke(
                                //     "start_application",
                                //     { applicationPath: path, startParameters: arg }
                                // );
                                // return;
                                let acc = { ...account };
                                let hasChanged = false;
                                postAccountVerify(account, "546d21e4a644715a33fb007a98371ada4295e29e").then(async (res) => {
                                    if (acc.data === undefined) acc.data = { account: null, charList: null };
                                    acc.data.account = res.Account;
                                    hasChanged = true;
                                    postCharList(res.Account.AccessToken)
                                        .then((charList) => {
                                            console.log("charList", charList);
                                            acc.data.charList = charList.Chars;
                                            onAccountChanged(acc.email, acc);
                                        }).catch((err) => {
                                            console.error("error", err);
                                            if (hasChanged) {
                                                onAccountChanged(acc.email, acc);
                                            }
                                        });
                                    
                                    const args = `data:{platform:Deca,guid:${btoa(acc.data.account.email)},token:${btoa(acc.data.account.AccessToken)},tokenTimestamp:${btoa(acc.data.account.AccessTokenTimestamp)},tokenExpiration:${btoa(acc.data.account.AccessTokenExpiration)},env:4,serverName:${acc.serverName}}`;
                                    tauri.invoke(
                                        "start_application",
                                        { applicationPath: settings.getByKeyAndSubKey("game", "exePath"), startParameters: args }
                                    );
                                })
                                    .then(() => {
                                        console.log("inside acc", acc);

                                        if (hasChanged) {
                                            onAccountChanged(acc.email, acc);
                                        }
                                        console.log(acc);
                                        if (acc.data.account !== null && acc.data.account.AccessToken !== null) {
                                            //Start Game
                                            // const appPath = "C:\\Users\\Maik8\\source\\repos\\StartArgumentsTest\\bin\\Debug\\StartArgumentsTest.exe";

                                            /*
                                            C# equivalent:
                                                 string arguments = string.Format("\"data:{{platform:Deca,guid:{0},token:{1},tokenTimestamp:{2},tokenExpiration:{3},env:4,serverName:{4}}}\"",
                                               StringToBase64String(_info.email), StringToBase64String(_info.accessToken.token), StringToBase64String(_info.accessToken.creationTime), StringToBase64String(_info.accessToken.expirationTime), GetServerName(_info.serverName));
                                            */

                                        }
                                    });
                            }}
                        >
                            <PlayCircleFilledWhiteOutlinedIcon size='large' sx={{ mr: 1 }} />start game
                        </StyledButton>
                    </Grid>
                    <Grid xs={7}>
                        <StyledButton fullWidth={true} startIcon={<RefreshOutlinedIcon />} color="secondary">
                            refresh data
                        </StyledButton>
                    </Grid>
                    <Grid xs={5}>
                        <StyledButton fullWidth={true} startIcon={<DeleteOutlineOutlinedIcon />} color="secondary" sx={{
                            '&:hover': {
                                backgroundColor: theme => theme.palette.error.main,
                            },
                        }}>
                            delete account
                        </StyledButton>
                    </Grid>

                </Grid>
            </Box>
        </Drawer >
    );
}

export default AccountDetails;