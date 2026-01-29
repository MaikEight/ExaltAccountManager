import useWidgets from "../../../hooks/useWidgets";
import StyledButton from "../../StyledButton";
import WidgetBase from "./WidgetBase";
import { Box, Grid, Tooltip, Typography } from "@mui/material";
import WarningAmberOutlinedIcon from '@mui/icons-material/WarningAmberOutlined';
import PlayCircleFilledWhiteOutlinedIcon from '@mui/icons-material/PlayCircleFilledWhiteOutlined';
import RefreshOutlinedIcon from '@mui/icons-material/RefreshOutlined';
import DeleteOutlineOutlinedIcon from '@mui/icons-material/DeleteOutlineOutlined';
import { useEffect, useState } from "react";
import useAccounts from "../../../hooks/useAccounts";
import { logToErrorLog } from 'eam-commons-js';
import useSnack from "../../../hooks/useSnack";
import { invoke } from '@tauri-apps/api/core';
import isMacOS from "../../../utils/isMacOS";
import { HWID_FILE_PATH } from "../../../constants";
import { readFileUTF8 } from 'eam-commons-js';
import { useNavigate } from "react-router-dom";
import useUserSettings from "../../../hooks/useUserSettings";
import usePopups from "../../../hooks/usePopups";
import DeleteAccountWarning from "../Components/DeleteAccountWarning";

function BasicActionsWidget({ type, widgetId }) {
    const { getWidgetConfiguration, closeWidgetBar, widgetBarState } = useWidgets();
    const { updateAccount, sendAccountVerify, sendCharList, refreshData } = useAccounts();
    const { showSnackbar, closeSnackbar } = useSnack();
    const settings = useUserSettings();
    const { showPopup, closePopup } = usePopups();
    const navigate = useNavigate();

    const [isLoading, setIsLoading] = useState(false);
    const [isLoadingRefresh, setIsLoadingRefresh] = useState(false);
    const [isDeleteMode, setIsDeleteMode] = useState(false);
    const [updateInProgress, setUpdateInProgress] = useState(false);
    const [gameExePath, setGameExePath] = useState("");

    const config = getWidgetConfiguration(type);

    const account = widgetBarState.data;


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

    const hasHwidFile = async () => {
        const path = await HWID_FILE_PATH();
        const _hwid = await readFileUTF8(path, false);
        const hasHwidFile = _hwid !== null && _hwid !== undefined && _hwid !== '';
        return hasHwidFile;
    }

    const showHwidWarning = () => {
        let snackbarKey = null;
        snackbarKey = showSnackbar(
            (
                <Box
                    sx={{
                        display: 'flex',
                        flexDirection: 'column',
                        alignItems: 'center',
                        justifyContent: 'center',
                        gap: 2,
                    }}
                >
                    <img
                        src="/mascot/Info/notification_simple_very_low_res.png"
                        alt={`${MASCOT_NAME} notifies you`}
                        style={{ width: '120px' }}
                    />
                    <Box
                        sx={{
                            display: 'flex',
                            flexDirection: 'column',
                            alignItems: 'center',
                            justifyContent: 'center',
                            textAlign: 'center',
                        }}
                    >
                        <Typography variant="body1">
                            Got the <b>Token for different machine</b> Error?
                        </Typography>
                        <Typography variant="body2">
                            Run the HWID-Reader to fix it.
                        </Typography>
                    </Box>
                    <Box
                        sx={{
                            display: 'flex',
                            flexDirection: 'row',
                            justifyContent: 'center',
                            alignItems: 'center',
                            gap: 2,
                        }}
                    >
                        <StyledButton
                            variant="contained"
                            size="small"
                            onClick={() => {
                                navigate('/utilities?runHwidReader=true');
                                if (snackbarKey) {
                                    closeSnackbar(snackbarKey);
                                }
                            }}
                        >
                            Run HWID-Reader
                        </StyledButton>
                        <StyledButton
                            variant="outlined"
                            color="warning"
                            size="small"
                            onClick={() => {
                                const startGameHWIDWarningsStr = localStorage.getItem('startGameHWIDWarnings');
                                if (startGameHWIDWarningsStr) {
                                    const startGameHWIDWarnings = JSON.parse(startGameHWIDWarningsStr);
                                    startGameHWIDWarnings.hide = true;
                                    localStorage.setItem('startGameHWIDWarnings', JSON.stringify(startGameHWIDWarnings));
                                }

                                if (snackbarKey) {
                                    closeSnackbar(snackbarKey);
                                }
                            }}
                        >
                            Dont show again
                        </StyledButton>
                    </Box>
                </Box>
            ),
            'message',
            true //persistent
        );
    };

    const checkForHwidFile = async () => {
        try {
            const startGameHWIDWarningsStr = localStorage.getItem('startGameHWIDWarnings');
            if (startGameHWIDWarningsStr) {
                const startGameHWIDWarnings = JSON.parse(startGameHWIDWarningsStr);
                console.log("startGameHWIDWarnings", startGameHWIDWarnings);

                if (startGameHWIDWarnings
                    && !startGameHWIDWarnings.hide
                    && !startGameHWIDWarnings.hasHWIDFile
                    && (startGameHWIDWarnings.amount <= 5 || startGameHWIDWarnings.lastCheck < new Date(Date.now() - 12 * 60 * 60 * 1000 /* 12 hours */))
                    && localStorage.getItem('isMacOs') !== 'true') // Dont show on MacOS as its not needed there
                {

                    console.log("Checking for HWID file after game start...");
                    startGameHWIDWarnings.amount = (startGameHWIDWarnings.amount || 0) + 1;
                    startGameHWIDWarnings.lastCheck = new Date();

                    if (await hasHwidFile()) {
                        console.log("HWID file found after game start, updating localStorage.");
                        startGameHWIDWarnings.hasHWIDFile = true;
                        localStorage.setItem('startGameHWIDWarnings', JSON.stringify(startGameHWIDWarnings));
                        return;
                    }
                    console.log("HWID file not found after game start, showing warning.");
                    startGameHWIDWarnings.hasHWIDFile = false;
                    localStorage.setItem('startGameHWIDWarnings', JSON.stringify(startGameHWIDWarnings));
                    showHwidWarning();
                }
                return;
            }

            console.log("startGameHWIDWarningsStr not found or conditions not met, checking for HWID file...");
            const _hasHwidFile = await hasHwidFile();
            const startGameHWIDWarnings = {
                amount: 1,
                hide: false,
                lastCheck: new Date(),
                hasHWIDFile: _hasHwidFile,
            }
            localStorage.setItem('startGameHWIDWarnings', JSON.stringify(startGameHWIDWarnings));

            if (_hasHwidFile) {
                return;
            }

            showHwidWarning();
        } catch (error) {
            console.error("Error checking for HWID file after game start:", error);
        }
    };
    
    const getServerToJoin = () => {
        if (account?.serverName && account.serverName !== "Default" && account.serverName !== "Last Server") {
            return account.serverName;
        }

        const serverToJoin = settings.getByKeyAndSubKey("game", "defaultServer");
        return serverToJoin === "Last Server" ? "" : serverToJoin;
    };

    const startGame = async () => {
        if (!account) {
            return;
        }

        try {
            const accResponse = await sendAccountVerify(account.email, true, true);
            if (accResponse === null || !accResponse.success || !accResponse.data.Account) {
                logToErrorLog("refresh Data", "Failed to refresh data for " + account.email);
                showSnackbar("Failed to fetch access token", 'error');
                return;
            }

            const token = {
                AccessToken: accResponse.data.Account.AccessToken,
                AccessTokenTimestamp: accResponse.data.Account.AccessTokenTimestamp,
                AccessTokenExpiration: accResponse.data.Account.AccessTokenExpiration,
            };

            showSnackbar("Starting the game...");
            const args = `data:{platform:Deca,guid:${btoa(account.email)},token:${btoa(token.AccessToken)},tokenTimestamp:${btoa(token.AccessTokenTimestamp)},tokenExpiration:${btoa(token.AccessTokenExpiration)},env:4,serverName:${getServerToJoin()}}`;

            // Extract the directory from gameExePath by removing the filename
            const currentDirectory = gameExePath ? gameExePath.substring(0, gameExePath.lastIndexOf('\\')) : "";

            invoke(
                "start_application",
                {
                    applicationPath: gameExePath,
                    startParameters: args,
                    currentDirectory: currentDirectory,
                }
            );

            const acc = { ...account, lastLogin: new Date() };
            await updateAccount(acc, false);

            const charList = await sendCharList(account.email, token.AccessToken);
            if (charList === null || !charList.success) {
                logToErrorLog("refresh Data", "Failed to refresh data for " + account.email);
                showSnackbar("Failed to refresh data", 'error');
            }

            if (isMacOS()) {
                return;
            }

            checkForHwidFile();
        } catch (e){
            console.error(e);
            showSnackbar("Failed to start the game", 'error')
        }
    };

    return (
        <WidgetBase type={type} widgetId={widgetId}>
            <Box
                sx={{
                    width: '100%',
                }}
            >
                <Grid container spacing={2}>
                    <Grid size={12}>
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
                                    disabled={isLoading || isLoadingRefresh || updateInProgress || account.state === 'Registered'}
                                    fullWidth={true}
                                    sx={{ height: 55 }}
                                    onClick={async () => {
                                        setIsLoading(true);
                                        await startGame();
                                        setIsLoading(false);
                                    }}
                                    loading={isLoading}
                                >
                                    <PlayCircleFilledWhiteOutlinedIcon size='large' sx={{ mr: 1 }} />
                                    start game
                                </StyledButton>
                            </span>
                        </Tooltip>
                    </Grid>
                    <Grid size={6}>
                        <StyledButton
                            disabled={isLoading || isLoadingRefresh}
                            fullWidth={true}
                            startIcon={<RefreshOutlinedIcon />}
                            color={account.state === 'Registered' ? "primary" : "secondary"}
                            onClick={async () => {
                                setIsLoadingRefresh(true);
                                const response = await refreshData(account.email);
                                if (response) {
                                    showSnackbar("Refreshing finished");
                                }
                                setIsLoadingRefresh(false);
                            }}
                            loading={isLoadingRefresh}
                        >
                            refresh data
                        </StyledButton>
                    </Grid>
                    <Grid size={6}>
                        {
                            <StyledButton fullWidth={true} startIcon={<DeleteOutlineOutlinedIcon />} color="secondary" sx={{
                                '&:hover': {
                                    backgroundColor: theme => theme.palette.error.main,
                                },
                            }}
                                onClick={() => {
                                    const data = {
                                        content: <DeleteAccountWarning email={account.email} onClose={closePopup} closeWidgetBar={closeWidgetBar}   />
                                    }
                                    showPopup(data);
                                }}
                            >
                                delete account
                            </StyledButton>
                        }
                    </Grid>
                </Grid>
            </Box>
        </WidgetBase>
    );
}

export default BasicActionsWidget;