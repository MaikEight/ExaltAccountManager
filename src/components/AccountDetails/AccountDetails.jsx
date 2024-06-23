import { Box, Drawer, IconButton, Table, TableBody, TableContainer, TableHead, TableRow, Tooltip, Typography, Zoom } from "@mui/material";
import { Unstable_Grid2 as Grid } from "@mui/material";
import { useEffect, useRef, useState } from "react";
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
import ArticleOutlinedIcon from '@mui/icons-material/ArticleOutlined';
import EditOutlinedIcon from '@mui/icons-material/EditOutlined';
import SaveOutlinedIcon from '@mui/icons-material/SaveOutlined';
import GroupRow from "./GroupRow";
import useHWID from "../../hooks/useHWID";
import useSnack from "../../hooks/useSnack";
import SteamworksRow from "./SteamworksRow";
import WarningAmberRoundedIcon from '@mui/icons-material/WarningAmberRounded';
import useAccounts from "../../hooks/useAccounts";
import useGroups from "../../hooks/useGroups";
import { getRequestState, storeCharList } from "../../utils/charListUtil";
import useServerList from './../../hooks/useServerList';
import { logToErrorLog } from "../../utils/loggingUtils";
import WarningAmberOutlinedIcon from '@mui/icons-material/WarningAmberOutlined';

function AccountDetails({ acc, onClose }) {
    const [account, setAccount] = useState(null);
    const [accountOrg, setAccountOrg] = useState(null);
    const [isOpen, setIsOpen] = useState(false);
    const [isEditMode, setIsEditMode] = useState(false);
    const [isDeleteMode, setIsDeleteMode] = useState(false);
    const [updateInProgress, setUpdateInProgress] = useState(false);
    const [decryptedPassword, setDecryptedPassword] = useState("");
    const [newDecryptedPassword, setNewDecryptedPassword] = useState("");
    const [gameExePath, setGameExePath] = useState("");

    const { saveServerList } = useServerList();
    const groupsContext = useGroups();
    const { groups } = groupsContext;

    const { updateAccount, deleteAccount } = useAccounts();
    const { showSnackbar } = useSnack();

    const group = account?.group ? groups?.find((g) => g.name === account.group) : null;

    const settings = useUserSettings();
    const hwid = useHWID();
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
            tauri.invoke("decrypt_string", { data: acc.password }).then((res) => {
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

    if (!account) {
        return null;
    }

    return (
        <Box ref={containerRef} sx={{ overflow: 'hidden', borderRadius: '10px', }}>
            <Drawer
                sx={{
                    width: 500,
                    '& .MuiDrawer-paper': {
                        width: 500,
                        backgroundColor: theme.palette.background.default,
                        border: 'none',
                        borderRadius: `${theme.shape.borderRadius}px 10px 10px ${theme.shape.borderRadius}px`,
                        overflow: 'hidden',
                    },
                }}
                PaperProps={{ elevation: 0, square: false, sx: { borderRadius: `${theme.shape.borderRadius}px 10px 10px ${theme.shape.borderRadius}px`, overflow: 'hidden' } }}
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
                                                                tauri.invoke("encrypt_string", { data: newDecryptedPassword }).then((res) => {
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
                                            <TextTableRow key='name' keyValue={"Accountname"} value={account.name} editMode={isEditMode} onChange={(value) => handleAccountEdit({ ...account, name: value })} allowCopy={true} />
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
                        <Grid container spacing={2}>
                            <Grid xs={12}>
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
                                            disabled={updateInProgress || account.state === 'Registered'}
                                            fullWidth={true}
                                            sx={{ height: 55 }}
                                            onClick={() => {
                                                postAccountVerify(account, hwid)
                                                    .then(async (res) => {
                                                        if (res === null) {
                                                            logToErrorLog("Start Game", "Failed to start the game for " + account.email + ", got null response");
                                                            showSnackbar("Failed to start the game", 'error');
                                                            return;
                                                        }

                                                        if (res.Error) {
                                                            const requestState = getRequestState(res);
                                                            logToErrorLog("Start Game", "Failed to start the game for " + account.email + ", got state: " + requestState);
                                                            if (res) {
                                                                showSnackbar("Failed to start the game: " + requestState, 'error');

                                                                const newAcc = ({ ...acc, state: requestState });
                                                                updateAccount(newAcc);
                                                                return;
                                                            }

                                                            showSnackbar("Failed to start the game: " + res.Error, 'error');
                                                            return;
                                                        }

                                                        const token = {
                                                            AccessToken: res.Account.AccessToken,
                                                            AccessTokenTimestamp: res.Account.AccessTokenTimestamp,
                                                            AccessTokenExpiration: res.Account.AccessTokenExpiration,
                                                        };

                                                        showSnackbar("Starting the game...");
                                                        const args = `data:{platform:Deca,guid:${btoa(acc.email)},token:${btoa(token.AccessToken)},tokenTimestamp:${btoa(token.AccessTokenTimestamp)},tokenExpiration:${btoa(token.AccessTokenExpiration)},env:4,serverName:${getServerToJoin()}}`;

                                                        tauri.invoke(
                                                            "start_application",
                                                            { applicationPath: gameExePath, startParameters: args }
                                                        );

                                                        postCharList(token.AccessToken)
                                                            .then((charList) => {
                                                                const state = getRequestState(charList);
                                                                const newAcc = ({ ...acc, state: state, lastRefresh: new Date().toISOString(), token: token });
                                                                updateAccount(newAcc);

                                                                if (state !== "Success") {
                                                                    logToErrorLog("Refresh Data", "Failed to refresh data for " + account.email + ", got state: " + state);
                                                                    showSnackbar("Failed to refresh data: " + state, 'error');
                                                                    return;
                                                                }

                                                                storeCharList(charList, acc.email);
                                                                showSnackbar("Refreshing finished");

                                                                const servers = charList.Chars.Servers.Server;
                                                                if (servers && servers.length > 0) {
                                                                    saveServerList(servers);
                                                                }
                                                            }).catch((err) => {
                                                                console.error("error", err);

                                                                const newAcc = ({ ...acc, token: token });
                                                                updateAccount(newAcc);

                                                                logToErrorLog("Refresh Data", "Failed to refresh data for " + account.email + ", got error: " + err);
                                                                showSnackbar("Failed to refresh data " + res.Error, 'error');
                                                            });
                                                    })
                                            }}
                                        >
                                            <PlayCircleFilledWhiteOutlinedIcon size='large' sx={{ mr: 1 }} />
                                            start game
                                        </StyledButton>
                                    </span>
                                </Tooltip>
                            </Grid>
                            <Grid xs={6}>
                                <StyledButton
                                    fullWidth={true}
                                    startIcon={<RefreshOutlinedIcon />}
                                    color={account.state === 'Registered' ? "primary" : "secondary"}
                                    onClick={() => {
                                        postAccountVerify(account, hwid)
                                            .then(async (res) => {
                                                if (res === null) {
                                                    logToErrorLog("Refresh Data", "Failed to refresh data for " + account.email + ", got null response");
                                                    showSnackbar("Failed to refresh data", 'error');

                                                    if(account.state === 'Registered') {
                                                        return;
                                                    }

                                                    const requestState = getRequestState(res);
                                                    const newAcc = ({ ...acc, state: requestState });
                                                    updateAccount(newAcc);
                                                    return;
                                                }                                               

                                                if (res.Error) {
                                                    const requestState = getRequestState(res);
                                                    logToErrorLog("Refresh Data", "Failed to refresh data for " + account.email + ", got state: " + requestState);
                                                    if (res) {
                                                        
                                                        if (account.state === "Registered") {
                                                            showSnackbar("Failed to refresh data, please activate your account", 'error');
                                                            return;
                                                        }
                                                        showSnackbar("Failed to refresh data: " + requestState, 'error');

                                                        const newAcc = ({ ...acc, state: requestState });
                                                        updateAccount(newAcc);
                                                        return;
                                                    }

                                                    showSnackbar("Failed to refresh data: " + res.Error, 'error');
                                                    return;
                                                }

                                                postCharList(res.Account.AccessToken)
                                                    .then((charList) => {
                                                        const state = getRequestState(charList);
                                                        const newAcc = ({ ...acc, state: state, lastRefresh: new Date().toISOString() });
                                                        updateAccount(newAcc);

                                                        if (state !== "Success") {
                                                            logToErrorLog("Refresh Data", "Failed to refresh data for " + account.email + ", got state: " + state);
                                                            showSnackbar("Failed to refresh data: " + state, 'error');
                                                            return;
                                                        }

                                                        storeCharList(charList, acc.email);
                                                        showSnackbar("Refreshing finished");

                                                        const servers = charList.Chars.Servers.Server;
                                                        if (servers && servers.length > 0) {
                                                            saveServerList(servers);
                                                        }
                                                    }).catch((err) => {
                                                        console.error("error", err);
                                                        logToErrorLog("Refresh Data", "Failed to refresh data for " + account.email + ", got error: " + err);
                                                        showSnackbar("Failed to refresh data " + res.Error, 'error');
                                                    });
                                            })
                                    }}
                                >
                                    refresh data
                                </StyledButton>
                            </Grid>
                            <Grid xs={6}>
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
                            </Grid>
                        </Grid>
                    </Box>
                </Box>
            </Drawer>
        </Box>
    );
}

export default AccountDetails;