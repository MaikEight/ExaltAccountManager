import { invoke } from '@tauri-apps/api/core';
import { logToErrorLog, readFileUTF8 } from 'eam-commons-js';
import { HWID_FILE_PATH, MASCOT_NAME } from '../constants';
import isMacOS from '../utils/isMacOS';
import useAccounts from './useAccounts';
import useSnack from './useSnack';
import useUserSettings from './useUserSettings';
import { Box, Typography } from '@mui/material';
import StyledButton from '../components/StyledButton';
import { useNavigate } from 'react-router-dom';

/**
 * Hook to handle starting the game with an account
 * Extracted from BasicActionsWidget for reuse across the app
 */
function useStartGame() {
    const { updateAccount, sendAccountVerify, sendCharList } = useAccounts();
    const { showSnackbar, closeSnackbar } = useSnack();
    const settings = useUserSettings();
    const navigate = useNavigate();

    const hasHwidFile = async () => {
        const path = await HWID_FILE_PATH();
        const _hwid = await readFileUTF8(path, false);
        const hasHwidFile = _hwid !== null && _hwid !== undefined && _hwid !== '';
        return hasHwidFile;
    };

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
                            Don't show again
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

                if (startGameHWIDWarnings
                    && !startGameHWIDWarnings.hide
                    && !startGameHWIDWarnings.hasHWIDFile
                    && (startGameHWIDWarnings.amount <= 5 || startGameHWIDWarnings.lastCheck < new Date(Date.now() - 12 * 60 * 60 * 1000 /* 12 hours */))
                    && localStorage.getItem('isMacOs') !== 'true') // Don't show on MacOS as it's not needed there
                {
                    startGameHWIDWarnings.amount = (startGameHWIDWarnings.amount || 0) + 1;
                    startGameHWIDWarnings.lastCheck = new Date();

                    if (await hasHwidFile()) {
                        startGameHWIDWarnings.hasHWIDFile = true;
                        localStorage.setItem('startGameHWIDWarnings', JSON.stringify(startGameHWIDWarnings));
                        return;
                    }
                    startGameHWIDWarnings.hasHWIDFile = false;
                    localStorage.setItem('startGameHWIDWarnings', JSON.stringify(startGameHWIDWarnings));
                    showHwidWarning();
                }
                return;
            }

            const _hasHwidFile = await hasHwidFile();
            const startGameHWIDWarnings = {
                amount: 1,
                hide: false,
                lastCheck: new Date(),
                hasHWIDFile: _hasHwidFile,
            };
            localStorage.setItem('startGameHWIDWarnings', JSON.stringify(startGameHWIDWarnings));

            if (_hasHwidFile) {
                return;
            }

            showHwidWarning();
        } catch (error) {
            console.error("Error checking for HWID file after game start:", error);
        }
    };

    const getServerToJoin = (account) => {
        if (account?.serverName && account.serverName !== "Default" && account.serverName !== "Last Server") {
            return account.serverName;
        }

        const serverToJoin = settings.getByKeyAndSubKey("game", "defaultServer");
        return serverToJoin === "Last Server" ? "" : serverToJoin;
    };

    const startGame = async (account) => {
        if (!account) {
            showSnackbar("No account provided", 'error');
            return { success: false };
        }

        try {
            const gameExePath = await settings.getByKeyAndSubKey("game", "exePath");
            if (!gameExePath) {
                showSnackbar("Game executable path not set", 'error');
                return { success: false };
            }

            const accResponse = await sendAccountVerify(account.email, true, true);
            if (accResponse === null || !accResponse.success || !accResponse.data.Account) {
                logToErrorLog("start game", "Failed to refresh data for " + account.email);
                showSnackbar("Failed to fetch access token", 'error');
                return { success: false };
            }

            const token = {
                AccessToken: accResponse.data.Account.AccessToken,
                AccessTokenTimestamp: accResponse.data.Account.AccessTokenTimestamp,
                AccessTokenExpiration: accResponse.data.Account.AccessTokenExpiration,
            };

            showSnackbar("Starting the game...");
            const args = `data:{platform:Deca,guid:${btoa(account.email)},token:${btoa(token.AccessToken)},tokenTimestamp:${btoa(token.AccessTokenTimestamp)},tokenExpiration:${btoa(token.AccessTokenExpiration)},env:4,serverName:${getServerToJoin(account)}}`;

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
                logToErrorLog("start game", "Failed to refresh data for " + account.email);
                showSnackbar("Failed to refresh data", 'error');
            }

            if (!isMacOS()) {
                checkForHwidFile();
            }

            return { success: true };
        } catch (e) {
            console.error(e);
            showSnackbar("Failed to start the game", 'error');
            return { success: false };
        }
    };

    return {
        startGame,
    };
}

export default useStartGame;
