import { Box, Drawer, IconButton, Input, Table, TableBody, TableContainer, TableHead, TableRow, Tooltip, Typography, Zoom } from "@mui/material";
import { Grid2 } from "@mui/material";
import { useEffect, useRef, useState } from "react";
import { useTheme } from "@emotion/react";
import ComponentBox from "../ComponentBox";
import PaddedTableCell from "./PaddedTableCell";
import TextTableRow from "./TextTableRow";
import ServerTableRow from "./ServerTableRow";
import DailyLoginCheckBoxTableRow from "./DailyLoginCheckBoxTableRow";
import StyledButton from './../StyledButton';
import CloseIcon from '@mui/icons-material/Close';
import PlayCircleFilledWhiteOutlinedIcon from '@mui/icons-material/PlayCircleFilledWhiteOutlined';
import RefreshOutlinedIcon from '@mui/icons-material/RefreshOutlined';
import DeleteOutlineOutlinedIcon from '@mui/icons-material/DeleteOutlineOutlined';
import useUserSettings from "../../hooks/useUserSettings";
import ArticleOutlinedIcon from '@mui/icons-material/ArticleOutlined';
import EditOutlinedIcon from '@mui/icons-material/EditOutlined';
import SaveOutlinedIcon from '@mui/icons-material/SaveOutlined';
import GroupRow from "./GroupRow";
import useSnack from "../../hooks/useSnack";
import SteamworksRow from "./SteamworksRow";
import WarningAmberRoundedIcon from '@mui/icons-material/WarningAmberRounded';
import useAccounts from "../../hooks/useAccounts";
import { logToErrorLog, formatTime, useGroups } from "eam-commons-js";
import WarningAmberOutlinedIcon from '@mui/icons-material/WarningAmberOutlined';
import { invoke } from '@tauri-apps/api/core';

function AccountDetails({ acc, onClose }) {
    const [account, setAccount] = useState(null);
    const [accountOrg, setAccountOrg] = useState(null);
    const [isOpen, setIsOpen] = useState(false);
    const [isEditMode, setIsEditMode] = useState(false);
    const [isDeleteMode, setIsDeleteMode] = useState(false);
    const [updateInProgress, setUpdateInProgress] = useState(false);
    const [isLoading, setIsLoading] = useState(false);
    const [decryptedPassword, setDecryptedPassword] = useState("");
    const [newDecryptedPassword, setNewDecryptedPassword] = useState("");
    const [gameExePath, setGameExePath] = useState("");

    const { groups } = useGroups();

    const { updateAccount, deleteAccount, sendAccountVerify, sendCharList, refreshData } = useAccounts();
    const { showSnackbar } = useSnack();

    const group = account?.group ? groups?.find((g) => g.name === account.group) : null;

    const settings = useUserSettings();
    const theme = useTheme();
    const containerRef = useRef(null);

    useEffect(() => {
        const checkSessionStorage = () => {
            const updInProgress = sessionStorage.getItem('updateInProgress');
            setUpdateInProgress(updInProgress === 'true');
        };
        checkSessionStorage();

        const getGameExePathAsync = async () => {
            const _gameExePath = await settings.getByKeyAndSubKey("game", "exePath");
            setGameExePath(_gameExePath);
        };
        getGameExePathAsync();

        const intervalId = setInterval(checkSessionStorage, 750);
        return () => { clearInterval(intervalId); }
    }, []);

    useEffect(() => {
        setAccountOrg(acc);
        setIsEditMode(false);
        setIsDeleteMode(false);

        if (acc) {
            setAccount(acc);
            invoke("decrypt_string", { data: acc.password }).then((res) => {
                setDecryptedPassword(res);
            });
            const timeoutId = setTimeout(() => {
                setIsOpen(true);
            }, 25);
            return () => clearTimeout(timeoutId);
        }

        setDecryptedPassword("");
        setIsOpen(false);
        const timeoutId = setTimeout(() => {
            setAccount(null);
        }, 500);

        return () => { clearTimeout(timeoutId); };
    }, [acc]);

    useEffect(() => {
        setNewDecryptedPassword(decryptedPassword);
    }, [decryptedPassword]);

    const handleAccountEdit = (acc) => {
        setAccount(acc);
    };

    const getServerToJoin = () => {
        if (acc?.serverName && acc.serverName !== "Default" && acc.serverName !== "Last Server") {
            return acc.serverName;
        }

        const serverToJoin = settings.getByKeyAndSubKey("game", "defaultServer");
        return serverToJoin === "Last Server" ? "" : serverToJoin;
    };

    // const refreshData = async () => {
    //     if (!account) {
    //         return;
    //     }

    //     const accResponse = await sendAccountVerify(account.email);
    //     if (accResponse === null || !accResponse.success) {
    //         logToErrorLog("refresh Data", "Failed to refresh data for " + account.email);
    //         showSnackbar("Failed to refresh data", 'error');
    //         return;
    //     }

    //     const token = {
    //         AccessToken: accResponse.data.Account.AccessToken,
    //         AccessTokenTimestamp: accResponse.data.Account.AccessTokenTimestamp,
    //         AccessTokenExpiration: accResponse.data.Account.AccessTokenExpiration,
    //     };

    //     const charList = await sendCharList(account.email, token.AccessToken);
    //     if (charList === null || !charList.success) {
    //         logToErrorLog("refresh Data", "Failed to refresh data for " + account.email);
    //         showSnackbar("Failed to refresh data", 'error');
    //         return;
    //     }

    //     return token;
    // };

    const startGame = async () => {
        if (!account) {
            return;
        }

        const accResponse = await sendAccountVerify(account.email);
        if (accResponse === null || !accResponse.success || !accResponse.data.Account) {
            logToErrorLog("refresh Data", "Failed to refresh data for " + account.email);
            showSnackbar("Failed to refresh data", 'error');
            return;
        }

        const token = {
            AccessToken: accResponse.data.Account.AccessToken,
            AccessTokenTimestamp: accResponse.data.Account.AccessTokenTimestamp,
            AccessTokenExpiration: accResponse.data.Account.AccessTokenExpiration,
        };

        showSnackbar("Starting the game...");
        const args = `data:{platform:Deca,guid:${btoa(account.email)},token:${btoa(token.AccessToken)},tokenTimestamp:${btoa(token.AccessTokenTimestamp)},tokenExpiration:${btoa(token.AccessTokenExpiration)},env:4,serverName:${getServerToJoin()}}`;

        invoke(
            "start_application",
            { applicationPath: gameExePath, startParameters: args }
        );

        const charList = await sendCharList(account.email, token.AccessToken);
        if (charList === null || !charList.success) {
            logToErrorLog("refresh Data", "Failed to refresh data for " + account.email);
            showSnackbar("Failed to refresh data", 'error');
            return;
        }
    };

    if (!account) {
        return null;
    }

    return (
        <Box ref={containerRef} sx={{ overflow: 'hidden', }}>
            <Drawer
                sx={{
                    width: 500,
                    '& .MuiDrawer-paper': {
                        width: 500,
                        backgroundColor: theme.palette.background.default,
                        borderRadius: `${theme.shape.borderRadius}px 0 0 ${theme.shape.borderRadius}px`,
                        overflow: 'hidden',
                    },
                }}
                PaperProps={{ elevation: 0, square: false, sx: { overflow: 'hidden' } }}
                SlideProps={{ container: containerRef.current }}
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
                <Box
                    sx={{
                        display: 'flex',
                        flexDirection: 'row',
                        justifyContent: 'center',
                        alignContent: 'center',
                        minHeight: 44,
                        maxHeight: 44,
                        pt: 0.5,
                        backgroundColor: theme.palette.background.paperLight,
                        position: 'sticky',
                        top: 0,
                    }}
                >
                    <IconButton
                        sx={{ position: 'absolute', left: 5, top: 5, marginLeft: 0, marginRight: 2 }}
                        size="small"
                        onClick={() => onClose()}
                    >
                        <CloseIcon sx={{ fontSize: 21 }} />
                    </IconButton>
                    <Typography variant="h6" component="div" sx={{ textAlign: 'center' }}>
                        Account details
                    </Typography>
                </Box>
                <Box
                    sx={{
                        flexGrow: 1,
                        display: 'flex',
                        flexDirection: 'column',
                        overflow: 'auto',
                    }}
                >
                    {/* 2. */}
                    <Box
                        sx={{
                            width: '100%',
                            height: '100%',
                            pr: 2,
                            pl: 2,
                            pb: 2,
                        }}
                    >
                        <Box
                            sx={{
                                display: 'flex',
                                flexDirection: 'column',
                                justifyContent: 'center',
                                alignItems: 'center',
                                width: '100%',
                            }}
                        >
                            <ComponentBox
                                title={
                                    <Box sx={{ display: 'flex', flexDirection: 'row' }}>
                                        <Typography variant="h6" component="div" sx={{ textAlign: 'center' }}>
                                            Details
                                        </Typography>
                                        <Box sx={{ position: 'absolute', right: 0, marginRight: '12px' }} >
                                            <Zoom direction="left" in={isEditMode} mountOnEnter unmountOnExit>
                                                <Tooltip title="Save account">
                                                    <IconButton
                                                        sx={{ color: theme.palette.text.primary }}
                                                        size="small"
                                                        onClick={() => {
                                                            console.log("save account", account);
                                                            if (newDecryptedPassword !== decryptedPassword) {
                                                                invoke("encrypt_string", { data: newDecryptedPassword }).then((res) => {
                                                                    const newAcc = ({ ...account, password: res });
                                                                    updateAccount(newAcc);
                                                                });
                                                            } else {
                                                                updateAccount(account);
                                                            }
                                                            setIsEditMode(!isEditMode);
                                                            showSnackbar("Account saved", 'success');
                                                        }}
                                                    >
                                                        <SaveOutlinedIcon />
                                                    </IconButton>
                                                </Tooltip>
                                            </Zoom>
                                            <Tooltip title={isEditMode ? "Cancel" : "Edit account"}>
                                                <IconButton
                                                    sx={{ color: theme.palette.text.primary }}
                                                    size="small"
                                                    onClick={() => {
                                                        if (isEditMode) {
                                                            setAccount(accountOrg);
                                                        }
                                                        setIsEditMode(!isEditMode);
                                                    }}
                                                >
                                                    {isEditMode ? <CloseIcon /> : <EditOutlinedIcon />}
                                                </IconButton>
                                            </Tooltip>
                                        </Box>
                                    </Box>
                                }
                                icon={<ArticleOutlinedIcon />}
                                sx={{
                                    width: '100%',
                                    transition: 'height 0.5s',
                                }}
                            >

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
                                                <PaddedTableCell sx={{ width: '125px' }}>Attribute</PaddedTableCell>
                                                <PaddedTableCell>Value</PaddedTableCell>
                                            </TableRow>
                                        </TableHead>
                                        <TableBody>
                                            <GroupRow
                                                key='group'
                                                editMode={isEditMode}
                                                group={group}
                                                onChange={(value) => handleAccountEdit({ ...account, group: value })}
                                            />
                                            <TextTableRow key='name' keyValue={"Accountname"} value={account.name} editMode={isEditMode} onChange={(value) => handleAccountEdit({ ...account, name: value })} allowCopy={account.name && account.name.length > 0} />
                                            {!account.isSteam ? <TextTableRow key='email' keyValue={"Email"} value={account.email} allowCopy={true} /> : <SteamworksRow guid={account.email} />}
                                            {isEditMode && <TextTableRow key='password' keyValue={"Password"} editMode={isEditMode} isPassword={true} value={newDecryptedPassword} onChange={(value) => setNewDecryptedPassword(value)} />}
                                            {!isEditMode && <TextTableRow key='lastLogin' keyValue={"Last login"} value={formatTime(account.lastLogin)} />}
                                            <ServerTableRow
                                                key='server'
                                                keyValue={"Server"}
                                                value={account.serverName}
                                                onChange={(value) => handleAccountEdit({ ...account, serverName: value })}
                                                showSaveButton={accountOrg?.serverName !== account?.serverName}
                                                onSave={() => {
                                                    const newAcc = ({ ...accountOrg, serverName: account.serverName });
                                                    updateAccount(newAcc, false);
                                                }}
                                            />
                                            <DailyLoginCheckBoxTableRow key='dailyLogin' keyValue={"Daily login"}
                                                value={account.performDailyLogin ? account.performDailyLogin : false}
                                                onChange={(event) => {
                                                    const newAcc = ({ ...accountOrg, performDailyLogin: event.target.checked });
                                                    updateAccount(newAcc, false);
                                                }}
                                            />
                                            {!isEditMode && <TextTableRow key='state' keyValue={"Last state"} value={account.state} innerSx={{ pb: 0 }} />}
                                        </TableBody>
                                    </Table>
                                </TableContainer>
                            </ComponentBox>
                        </Box>

                        {/* 3. */}
                        <Grid2 container spacing={2}>
                            <Grid2 size={12}>
                                <Tooltip
                                    title={account.state === 'Registered' ?
                                        (
                                            <Box sx={{
                                                display: 'flex',
                                                flexDirection: 'row',
                                                alignItems: 'center',
                                                justifyContent: 'center',
                                                textAlign: 'left',
                                                gap: 1,
                                            }}
                                            >
                                                <WarningAmberOutlinedIcon color="warning" />
                                                Please confirm the email by following the instructions and click "REFRESH DATA" afterwards
                                            </Box>
                                        ) : ""}
                                >
                                    <span>
                                        <StyledButton
                                            disabled={isLoading || updateInProgress || account.state === 'Registered'}
                                            fullWidth={true}
                                            sx={{ height: 55 }}
                                            onClick={async () => {
                                                setIsLoading(true);
                                                await startGame();
                                                setIsLoading(false);
                                            }}
                                        >
                                            <PlayCircleFilledWhiteOutlinedIcon size='large' sx={{ mr: 1 }} />
                                            start game
                                        </StyledButton>
                                    </span>
                                </Tooltip>
                            </Grid2>
                            <Grid2 size={6}>
                                <StyledButton
                                    disabled={isLoading}
                                    fullWidth={true}
                                    startIcon={<RefreshOutlinedIcon />}
                                    color={account.state === 'Registered' ? "primary" : "secondary"}
                                    onClick={async () => {
                                        setIsLoading(true);
                                        const response = await refreshData(account.email);
                                        if (response) {
                                            showSnackbar("Refreshing finished");
                                        }
                                        setIsLoading(false);
                                    }}
                                >
                                    refresh data
                                </StyledButton>
                            </Grid2>
                            <Grid2 size={6}>
                                {
                                    !isDeleteMode ?
                                        <StyledButton fullWidth={true} startIcon={<DeleteOutlineOutlinedIcon />} color="secondary" sx={{
                                            '&:hover': {
                                                backgroundColor: theme => theme.palette.error.main,
                                            },
                                        }}
                                            onClick={() => {
                                                setIsDeleteMode(true);
                                            }}
                                        >
                                            delete account
                                        </StyledButton>
                                        :
                                        <ComponentBox
                                            title="Are you sure?"
                                            icon={<WarningAmberRoundedIcon />}
                                            sx={{
                                                width: '100%',
                                                height: '100%',
                                                m: 0,
                                                transition: 'height 0.5s',
                                            }}
                                        >
                                            <Typography variant="body2" component="div">
                                                This action cannot be undone.
                                            </Typography>
                                            <Box sx={{ display: 'flex', flexDirection: 'row', justifyContent: 'space-around', mt: 1 }}>
                                                <IconButton
                                                    color='error'
                                                    size="small"
                                                    onClick={() => {
                                                        deleteAccount(account.email);
                                                        onClose();
                                                    }}
                                                >
                                                    <DeleteOutlineOutlinedIcon />
                                                </IconButton>
                                                <IconButton
                                                    sx={{ color: theme.palette.text.primary }}
                                                    size="small"
                                                    onClick={() => setIsDeleteMode(false)}
                                                >
                                                    <CloseIcon />
                                                </IconButton>
                                            </Box>
                                        </ComponentBox>
                                }
                            </Grid2>
                            <Grid2 size={12}>
                                <Input
                                    id="comment"
                                    name="comment"
                                    placeholder="Comment..."
                                    type="text"
                                    sx={{
                                        borderRadius: `${theme.shape.borderRadius}px`,
                                        p: 1.2,
                                        backgroundColor: theme.palette.background.paper,
                                    }}
                                    value={account.comment ?? ''}
                                    onChange={(event) => handleAccountEdit({ ...account, comment: event.target.value })}
                                    endAdornment={
                                        <IconButton
                                            disabled={account?.comment === accountOrg?.comment}
                                            onClick={() => {
                                                updateAccount({ ...accountOrg, comment: account.comment });
                                                showSnackbar("Comment saved", 'success');
                                            }}
                                        >
                                            <SaveOutlinedIcon />
                                        </IconButton>
                                    }
                                    fullWidth
                                    multiline
                                    minRows={3}
                                    disableUnderline
                                />
                            </Grid2>
                        </Grid2>
                    </Box>
                </Box>
            </Drawer>
        </Box>
    );
}

export default AccountDetails;