
import { Box, Checkbox, FormControlLabel, FormGroup, LinearProgress, Paper, Popover, Switch, Table, TableBody, TableCell, TableContainer, TableHead, TableRow, TextField, Tooltip, Typography } from '@mui/material';
import ComponentBox from './../components/ComponentBox';
import FolderOutlinedIcon from '@mui/icons-material/FolderOutlined';
import { forwardRef, useContext, useEffect, useMemo, useRef, useState } from 'react';
import StyledButton from './../components/StyledButton';
import { invoke } from '@tauri-apps/api/core';
import useUserSettings from '../hooks/useUserSettings';
import SearchOutlinedIcon from '@mui/icons-material/SearchOutlined';
import RestartAltOutlinedIcon from '@mui/icons-material/RestartAltOutlined';
import ViewColumnOutlinedIcon from '@mui/icons-material/ViewColumnOutlined';
import VisibilityOutlinedIcon from '@mui/icons-material/VisibilityOutlined';
import VisibilityOffOutlinedIcon from '@mui/icons-material/VisibilityOffOutlined';
import DarkModeOutlinedIcon from '@mui/icons-material/DarkModeOutlined';
import { ColorContext, useUserLogin } from 'eam-commons-js';
import DnsOutlinedIcon from '@mui/icons-material/DnsOutlined';
import useSnack from '../hooks/useSnack';
import useServerList from '../hooks/useServerList';
import ServerListSelect from '../components/ServerListSelect';
import VaultPeekerLogo from '../components/VaultPeekerLogo';
import PolicyOutlinedIcon from '@mui/icons-material/PolicyOutlined';
import FindInPageOutlinedIcon from '@mui/icons-material/FindInPageOutlined';
import GavelOutlinedIcon from '@mui/icons-material/GavelOutlined';
import { useTheme } from '@emotion/react';
import useAccounts from '../hooks/useAccounts';
import { TableVirtuoso } from 'react-virtuoso';
import * as dialog from "@tauri-apps/plugin-dialog"
import useApplySettingsToHeaderName from '../hooks/useApplySettingsToHeaderName';
import TroubleshootOutlinedIcon from '@mui/icons-material/TroubleshootOutlined';
import { fetch } from '@tauri-apps/plugin-http';
import { EAM_PRIVACY_GATE_API } from 'eam-commons-js/constants';
import ServerSvg from '../components/Illustrations/ServerSvg';
import DeleteOutlineOutlinedIcon from '@mui/icons-material/DeleteOutlineOutlined';
import DeleteForeverOutlinedIcon from '@mui/icons-material/DeleteForeverOutlined';
import { MASCOT_NAME } from '../constants';
import PlayCircleOutlineOutlinedIcon from '@mui/icons-material/PlayCircleOutlineOutlined';
import { enable, isEnabled, disable } from '@tauri-apps/plugin-autostart'

function SettingsPage() {
    const userSettings = useUserSettings();
    const colorContext = useContext(ColorContext);
    const { serverList } = useServerList();
    const { showSnackbar } = useSnack();
    const { idToken, isAuthenticated } = useUserLogin();
    const theme = useTheme();

    const openVaultPeekerButtonRef = useRef(null);

    const [isAutostartEnabled, setIsAutostartEnabled] = useState(false);
    const [isUpdateingAutostart, setIsUpdatingAutostart] = useState(false);
    const [initialSettings, setInitialSettings] = useState(true);
    const [settings, setSettings] = useState({});
    const [gameExePath, setGameExePath] = useState("");
    const [openVaultPeekerAccountsPoppover, setOpenVaultPeekerAccountsPoppover] = useState(false);
    const [analyticsSettings, setAnalyticsSettings] = useState(null);
    const [analyticsRequestLoading, setAnalyticsRequestLoading] = useState(false);
    const [dataDeletionRequestLoading, setDataDeletionRequestLoading] = useState(false);
    const [anchorElDeletion, setAnchorElDeletion] = useState(null);
    const [mascotImage, setMascotImage] = useState('/mascot/Error/error_mascot_only_very_small_low_res.png');
    const openDeletionPopup = Boolean(anchorElDeletion);
    const idDeletionPopup = open ? 'data-deletion-popover' : undefined;

    const { applySettingsToHeaderName } = useApplySettingsToHeaderName();

    const columns = useMemo(() => {
        return [
            { field: 'orderId', headerName: applySettingsToHeaderName('🆔 Order ID') },
            { field: 'group', headerName: applySettingsToHeaderName('👥 Group') },
            { field: 'name', headerName: applySettingsToHeaderName('🗣️ Accountname') },
            { field: 'email', headerName: applySettingsToHeaderName('📧 Email') },
            { field: 'lastLogin', headerName: applySettingsToHeaderName('⏰ Last Login') },
            { field: 'serverName', headerName: applySettingsToHeaderName('🌐 Server') },
            { field: 'lastRefresh', headerName: applySettingsToHeaderName('🔄 Refresh') },
            { field: 'performDailyLogin', headerName: applySettingsToHeaderName('📅 Daily Login') },
            { field: 'state', headerName: applySettingsToHeaderName('📊 Last State') },
            { field: 'comment', headerName: applySettingsToHeaderName('💬 Comment') },
        ]
    }, [settings]);

    const updateAutostartStatus = async () => {
        const enabled = await isEnabled();
        setIsAutostartEnabled(enabled);
    }

    const handleAutostartToggle = async () => {
        setIsUpdatingAutostart(true);
        try {

            if (await isEnabled()) {
                await disable();
                setIsAutostartEnabled(false);
                showSnackbar("Autostart disabled", "message");
            } else {
                await enable();
                setIsAutostartEnabled(true);
                showSnackbar("Autostart enabled", "success");
            }
        } catch (error) {
            console.error("Error toggling autostart:", error);
            showSnackbar("Error toggling autostart, please try again later.", "error");
        } finally {
            setIsUpdatingAutostart(false);
            updateAutostartStatus();
        }
    }

    useEffect(() => {
        const intervalId = setInterval(() => {
            setMascotImage(prev => {
                if (prev?.includes('2')) {
                    return '/mascot/Error/error_mascot_only_very_small_low_res.png';
                }

                return '/mascot/Error/error_mascot_only_2_small_very_low_res.png';
            });
        }, 750);
        updateAutostartStatus();

        return () => {
            clearInterval(intervalId);
        }
    }, []);

    const setTheSettings = async () => {
        const s = userSettings.get;
        const _gameExePath = await userSettings.getByKeyAndSubKey('game', 'exePath');
        setSettings(s);
        setGameExePath(_gameExePath);

        // Set the analytics settings
        const analyticsSettingsUserData = await invoke('get_user_data_by_key', { key: 'analytics' })
            .catch(() => {
                return null;
            });

        if (analyticsSettingsUserData) {
            const analyticsSettingsString = analyticsSettingsUserData.dataValue;

            if (analyticsSettingsString === null
                || analyticsSettingsString === undefined) {
                setAnalyticsSettings({ sendAnonymizedData: false, optOut: false });
                return;
            }

            const analyticsSettingsObj = JSON.parse(analyticsSettingsString);
            if (analyticsSettingsObj === null || analyticsSettingsObj === undefined) {
                setAnalyticsSettings({ sendAnonymizedData: false, optOut: false });
                return;
            }

            setAnalyticsSettings(analyticsSettingsObj);
        } else {
            setAnalyticsSettings({ sendAnonymizedData: false, optOut: false });
        }
    };

    useEffect(() => {
        setTheSettings();
    }, []);

    useEffect(() => {
        setTheSettings();
    }, [userSettings.get]);

    useEffect(() => {
        if (initialSettings) {
            setInitialSettings(false);
            return;
        }
        if (settings === userSettings.get) return;

        userSettings.set(settings);
    }, [settings]);

    useEffect(() => {
        if (gameExePath === undefined || gameExePath === "" || gameExePath === null) return;

        userSettings.setByKeyAndSubKey('game', 'exePath', gameExePath);
    }, [gameExePath]);

    useEffect(() => {
        if (!analyticsSettings) {
            return;
        }

        const updateAnalytics = async () => {
            await invoke('insert_or_update_user_data', { userData: { dataKey: 'analytics', dataValue: JSON.stringify(analyticsSettings) } });
        };
        updateAnalytics();
    }, [analyticsSettings]);

    const isDarkMode = () => {
        if (settings === undefined || settings.general === undefined || settings.general.theme === undefined)
            return true;

        return settings.general.theme === 'dark';
    };

    const deleteAllUserData = async () => {
        setDataDeletionRequestLoading(true);

        const headers = {
            "eam-application-id": "ExaltAccountManager",
        };

        if (isAuthenticated) {
            headers.Authorization = `Bearer ${idToken}`;
        }

        const requestOptions = {
            method: "DELETE",
            headers: headers
        };

        try {
            const hwid = localStorage.getItem("apiHwidHash");
            const sessionId = sessionStorage.getItem("sessionId");
            if (!hwid || !sessionId) {
                console.error("No HWID or Session ID found");
                showSnackbar("No HWID or Session ID found, can't request your data.", "error");
                setDataDeletionRequestLoading(false);
                return;
            }
            const url = `${EAM_PRIVACY_GATE_API}/user-data-request?clientIdHash=${hwid}&sessionId=${sessionId}`;
            const response = await fetch(url, requestOptions);

            if (!response.ok) {
                console.error("Error deleting data:", response.statusText);
                showSnackbar("Error deleting your data, please try again later.", "error");
                setDataDeletionRequestLoading(false);
                return;
            }

            showSnackbar("Your data has been deleted.", "success");
        } catch (error) {
            console.error(error);
            showSnackbar("Error requesting your data, please try again later.", "error");
        }
        finally {
            setDataDeletionRequestLoading(false);
        }
    }

    return (
        <Box sx={{ width: '100%', overflow: 'auto' }}>
            {/* Game Path */}
            <ComponentBox
                title="Game Path"
                icon={<FolderOutlinedIcon />}
            >
                <Box
                    sx={{
                        display: 'flex',
                        flexDirection: 'column',
                        justifyContent: 'space-between',
                        gap: 2
                    }}
                >
                    <TextField
                        sx={{ width: '100%' }}
                        id="gameExePath"
                        label="Path to RotMG Exalt.exe"
                        variant="standard"
                        value={gameExePath}
                        onChange={async (event) => {
                            if (event.target.value.endsWith("RotMG Exalt Launcher.exe")) {
                                showSnackbar("You have chosen the launcher instead of the game executable. Please choose the RotMG Exalt.exe instead.", "error");

                                const defaultGamePath = await invoke('get_default_game_path');
                                if (defaultGamePath) {
                                    setGameExePath(defaultGamePath);
                                    return;
                                }
                                setGameExePath("");

                                return;
                            }

                            setGameExePath(event.target.value)
                        }}
                    />
                    <Box
                        sx={{
                            display: 'flex',
                            flexDirection: 'row',
                            justifyContent: 'space-between',
                        }}
                    >
                        <StyledButton
                            color="secondary"
                            startIcon={<SearchOutlinedIcon />}
                            onClick={async () => {
                                const filePath = await dialog.open({ multiple: false });
                                if (filePath) {
                                    if (filePath.endsWith("RotMG Exalt Launcher.exe")) {
                                        showSnackbar("You have chosen the launcher instead of the game executable. Please choose the RotMG Exalt.exe instead.", "error");
                                        return;
                                    }

                                    setGameExePath(filePath);
                                }
                            }}
                        >
                            Choose file
                        </StyledButton>
                        <StyledButton
                            color="secondary"
                            startIcon={<RestartAltOutlinedIcon />}
                            onClick={async () => {
                                const defaultGamePath = await invoke('get_default_game_path');
                                if (defaultGamePath) {
                                    setGameExePath(defaultGamePath);
                                    return;
                                }
                                setGameExePath("");
                            }}
                        >
                            Set to default
                        </StyledButton>
                    </Box>
                </Box>
            </ComponentBox>

            {/* Accounts columns */}
            <ComponentBox
                title="Accounts columns"
                icon={<ViewColumnOutlinedIcon />}
            >
                <Typography variant="body2" color="text.secondary" sx={{ mb: 2 }}>
                    Choose which columns should be shown in the accounts table by default.
                </Typography>
                <Box sx={{ width: '100%', display: 'flex', flexWrap: 'wrap', gap: 1, px: 1.5 }}>
                    {
                        columns.map((column) =>
                            <Box key={column.field} sx={{ flexGrow: 1, flexBasis: '20%' }}>
                                <ColumnSwitch
                                    label={column.headerName}
                                    checked={settings?.accounts?.columnsHidden[column.field] !== undefined ? settings.accounts.columnsHidden[column.field] : true}
                                    onChange={(event) => {
                                        const newSettings = { ...settings };
                                        if (!newSettings.accounts.columnsHidden) newSettings.accounts.columnsHidden = {};
                                        newSettings.accounts.columnsHidden[column.field] = event.target.checked;
                                        setSettings(newSettings);
                                    }}
                                />
                            </Box>
                        )
                    }
                </Box>
                <Box sx={{ display: 'flex', justifyContent: 'space-between', mt: 3 }}>
                    <StyledButton
                        color="secondary"
                        startIcon={<RestartAltOutlinedIcon />}
                        onClick={() => {
                            let newSettings = settings;
                            delete newSettings.accounts.columnsHidden;
                            setSettings(userSettings.addDefaults(newSettings));
                        }}
                    >
                        set to default
                    </StyledButton>
                    <Box
                        sx={{
                            display: 'flex',
                            flexDirection: 'row',
                            gap: 1.5,
                        }}
                    >
                        <StyledButton
                            color="secondary"
                            startIcon={<VisibilityOffOutlinedIcon />}
                            onClick={() => {
                                let newSettings = settings;
                                newSettings.accounts.columnsHidden = {};
                                columns.forEach((column) => newSettings.accounts.columnsHidden[column.field] = false);
                                setSettings(userSettings.addDefaults(newSettings));
                            }}
                        >
                            hide all columns
                        </StyledButton>
                        <StyledButton
                            color="secondary"
                            startIcon={<VisibilityOutlinedIcon />}
                            onClick={() => {
                                let newSettings = settings;
                                newSettings.accounts.columnsHidden = {};
                                columns.forEach((column) => newSettings.accounts.columnsHidden[column.field] = true);
                                setSettings(userSettings.addDefaults(newSettings));
                            }}
                        >
                            show all columns
                        </StyledButton>
                    </Box>
                </Box>
                <Box sx={{ mt: 3 }}>
                    <Typography variant="body2" color="text.secondary" sx={{ mb: 2 }}>
                        Choose if you want the column headers to contain emojis.
                    </Typography>
                    <Box
                        sx={{
                            px: 1.5,
                        }}
                    >
                        <ColumnSwitch
                            label="Hide emojis"
                            checked={settings?.accounts?.hideEmojis !== undefined ? settings.accounts.hideEmojis : false}
                            onChange={(event) => {
                                const newSettings = { ...settings };
                                if (!newSettings.accounts) newSettings.accounts = {};
                                newSettings.accounts.hideEmojis = event.target.checked;
                                setSettings(newSettings);
                            }}
                        />
                    </Box>
                </Box>
            </ComponentBox>

            {/* Default Server */}
            <ComponentBox
                title="Default Server"
                icon={<DnsOutlinedIcon />}
                innerSx={{
                    display: 'flex',
                    flexDirection: 'row',
                    gap: 2,
                    justifyContent: 'space-between',
                }}
            >
                <Box>
                    <Typography variant="body2" color="text.secondary" sx={{ mb: 2 }}>
                        Choose which server should be used by default when starting the game.
                    </Typography>
                    {
                        (serverList && serverList.length > 0) ? null :
                            <Typography variant="body2" color="text.secondary" sx={{ mb: 2 }}>
                                To add servers to this list, please add an account and click on "Refresh data".
                            </Typography>
                    }
                    <ServerListSelect
                        serversToAdd={[{ Name: 'Last server', DNS: 'LAST' },]}
                        selectedServer={settings?.game?.defaultServer ? settings.game.defaultServer : "Last server"}
                        onChange={(server) => { setSettings({ ...settings, game: { ...settings.game, defaultServer: server } }) }}
                        defaultValue={'Last server'}
                    />
                </Box>
                <Box
                    sx={{
                        position: 'relative',
                        height: '75px',
                        width: '135px',
                    }}
                >
                    <Box
                        sx={{
                            position: 'absolute',
                            top: 0,
                            right: 0,
                            height: '75px',
                            zIndex: 1,
                        }}
                    >
                        <ServerSvg w={'100%'} h={'100%'} />
                    </Box>
                    <Box
                        sx={{
                            position: 'absolute',
                            top: 7,
                            left: 2,
                            zIndex: 0,
                        }}
                    >
                        <img
                            src="/mascot/okta_low_res.png"
                            alt="Okta, the mascot"
                            height={70}
                        />
                    </Box>
                </Box>
            </ComponentBox>

            {/* Vault Peeker */}
            <ComponentBox
                title="Vault Peeker"
                icon={
                    <VaultPeekerLogo
                        sx={{ ml: '2px', mt: '6px', width: '20px', mr: 0.25 }}
                        color={theme.palette.text.primary}
                    />
                }
            >
                <Typography variant="body2" color="text.secondary" sx={{ mb: 2 }}>
                    Choose which fields should be collapsed by default in the Vault Peeker.
                </Typography>
                <Box sx={{ display: 'flex', flexDirection: 'row', gap: 1, px: 1.5 }}>
                    <ColumnSwitch
                        label="Filter"
                        checked={settings?.vaultPeeker?.collapsedFileds?.filter !== undefined ? settings.vaultPeeker.collapsedFileds.filter : true}
                        onChange={(event) => {
                            const newSettings = { ...settings };
                            if (!newSettings.vaultPeeker.collapsedFileds) newSettings.vaultPeeker.collapsedFileds = {};
                            newSettings.vaultPeeker.collapsedFileds.filter = event.target.checked;
                            setSettings(newSettings);
                        }}
                    />
                    <ColumnSwitch
                        label="Totals"
                        checked={settings?.vaultPeeker?.collapsedFileds?.totals !== undefined ? settings.vaultPeeker.collapsedFileds.totals : false}
                        onChange={(event) => {
                            const newSettings = { ...settings };
                            if (!newSettings.vaultPeeker.collapsedFileds) newSettings.vaultPeeker.collapsedFileds = {};
                            newSettings.vaultPeeker.collapsedFileds.totals = event.target.checked;
                            setSettings(newSettings);
                        }}
                    />
                </Box>
                <Box ref={openVaultPeekerButtonRef} sx={{ display: 'flex', flexDirection: 'row', mt: 2 }}>
                    <StyledButton
                        color="secondary"
                        startIcon={<VisibilityOutlinedIcon />}
                        onClick={() => {
                            setOpenVaultPeekerAccountsPoppover(true);
                        }}
                    >
                        Select default collapsed accounts
                    </StyledButton>
                    <Popover
                        open={openVaultPeekerAccountsPoppover}
                        anchorReference="anchorPosition"
                        anchorPosition={{ top: 200, left: 380 }}
                        onClose={() => setOpenVaultPeekerAccountsPoppover(false)}
                    >
                        <Paper
                            sx={{
                                p: 0.25,
                                backgroundColor: theme.palette.mode === 'dark' ? theme.palette.background.paper : theme.palette.background.paper,
                                borderRadius: `${theme.shape.borderRadius}px`,
                            }}
                        >
                            <AccountSelectorTable
                                settings={settings}
                                setCheckedMails={(emails) => {
                                    const newSettings = { ...settings };
                                    if (!newSettings.vaultPeeker.collapsedFileds) newSettings.vaultPeeker.collapsedFileds = {};
                                    newSettings.vaultPeeker.collapsedFileds.accounts = emails;
                                    setSettings(newSettings);
                                }}
                            />
                        </Paper>
                    </Popover>
                </Box>
            </ComponentBox>

            {/* Autostart */}
            <ComponentBox
                title="Autostart & Close behavior"
                icon={<PlayCircleOutlineOutlinedIcon />}
            >
                <Typography variant="body2" color="text.secondary">
                    Choose if the application should start automatically when you start your computer.
                </Typography>
                <Typography variant="body2" color="text.secondary" sx={{ mb: 2 }}>
                    This allows EAM to refresh your accounts in the background and always be up to date.
                </Typography>
                <Box
                    sx={{
                        display: 'flex',
                        flexDirection: 'column',
                    }}
                >

                    <FormControlLabel
                        sx={{ gap: 0.5 }}
                        control={
                            <Tooltip
                                title={"Minimizing the application to the tray on close will allow you to keep the application running in the background without having it open in the taskbar. This allows EAM to refresh your accounts in the background and always be up to date."}
                            >
                                <Switch
                                    checked={settings?.general?.minimizeToTray || false}
                                    onChange={() => {
                                        const newSettings = { ...settings };
                                        if (!newSettings.general) newSettings.general = {};
                                        newSettings.general.minimizeToTray = !newSettings.general.minimizeToTray;
                                        setSettings(newSettings);
                                    }}
                                />
                            </Tooltip>
                        }
                        label={'Minimize to tray on close'}
                    />

                    <FormControlLabel
                        sx={{ gap: 0.5 }}
                        control={
                            <Tooltip
                                title={isAutostartEnabled ? "Disable autostart" : "Enable autostart"}
                            >
                                <Switch
                                    disabled={isUpdateingAutostart}
                                    checked={isAutostartEnabled}
                                    onChange={handleAutostartToggle}
                                />
                            </Tooltip>
                        }
                        label={'Autostart'}
                    />
                </Box>
            </ComponentBox>

            {/* Theme */}
            <ComponentBox
                title="Theme"
                icon={<DarkModeOutlinedIcon />}
            >
                <Typography variant="body2" color="text.secondary" sx={{ mb: 2 }}>
                    Choose which theme should be used... of course only dark mode is the correct choice.
                </Typography>
                <Tooltip title={isDarkMode() ? "Burn your eyes!" : "Come to the dark side, we have cookies!"}>
                    <FormControlLabel sx={{ gap: 0.5 }} control={<Switch checked={isDarkMode()} onChange={() => colorContext.toggleColorMode()} />} label={'Darkmode'} />
                </Tooltip>
            </ComponentBox>
            {/* Privacy & Legal */}
            <ComponentBox
                title="Privacy & Legal"
                icon={<PolicyOutlinedIcon />}
                innerSx={{
                    display: 'flex',
                    flexDirection: 'column',
                    gap: 1.5,
                }}
                isCollapseable={true}
                defaultCollapsed={true}
                isLoading={analyticsRequestLoading || dataDeletionRequestLoading}
            >
                <Box
                    sx={{
                        display: 'flex',
                        flexDirection: 'column',
                        gap: 0,
                    }}
                >
                    <Typography variant="body2" color="text.secondary" >
                        Here you can find the privacy policy and the Terms of service... yes we have to have this... yes you should read it.
                    </Typography>
                    <Typography variant="body2" color="text.secondary" >
                        If you have any questions or concerns, please feel free to ask.
                    </Typography>
                </Box>
                <Box
                    sx={{
                        display: 'flex',
                        flexDirection: 'row',
                        gap: 1.5,
                    }}
                >
                    <a href="https://exaltaccountmanager.com/privacy-policy/" target="_blank" rel="noreferrer">
                        <StyledButton
                            color="secondary"
                            startIcon={<FindInPageOutlinedIcon />}
                        >
                            View Privacy Policy
                        </StyledButton>
                    </a>
                    <a href="https://exaltaccountmanager.com/terms-of-service/" target="_blank" rel="noreferrer">
                        <StyledButton
                            color="secondary"
                            startIcon={<GavelOutlinedIcon />}
                        >
                            View Terms of Service
                        </StyledButton>
                    </a>
                </Box>
                {/* ANALYTICS */}
                <Box
                    sx={{
                        mt: 1,
                        display: 'flex',
                        flexDirection: 'column',
                        gap: 0,
                    }}
                >
                    <Typography
                        variant="h6"
                        component="div"
                        color="text.secondary"
                        sx={{
                            display: 'flex',
                            flexDirection: 'row',
                            gap: 1,
                            alignItems: 'center',
                            mb: 1,
                        }}
                    >
                        <TroubleshootOutlinedIcon /> Analytics
                    </Typography>
                    <Typography variant="body2" color="text.secondary" >
                        We collect analytics to improve the application and to see how it is used.
                    </Typography>
                    <Typography variant="body2" color="text.secondary" >
                        If you want, you can request a copy of the data we have collected about you or request a delete of everything we know about you.
                    </Typography>
                    <Box
                        sx={{
                            mt: 1,
                            display: 'flex',
                            flexDirection: 'row',
                            gap: 1.5,
                            mb: 1.5,
                        }}
                    >
                        <StyledButton
                            disabled={analyticsRequestLoading || dataDeletionRequestLoading || analyticsSettings?.optOut || analyticsSettings?.sendAnonymizedData}
                            color="secondary"
                            startIcon={<VisibilityOutlinedIcon />}
                            onClick={async () => {
                                setAnalyticsRequestLoading(true);

                                const headers = {
                                    "eam-application-id": "ExaltAccountManager",
                                };

                                if (isAuthenticated) {
                                    headers.Authorization = `Bearer ${idToken}`;
                                }

                                const requestOptions = {
                                    method: "GET",
                                    headers: headers
                                };

                                try {
                                    const hwid = localStorage.getItem("apiHwidHash");
                                    const sessionId = sessionStorage.getItem("sessionId");
                                    if (!hwid || !sessionId) {
                                        console.error("No HWID or Session ID found");
                                        showSnackbar("No HWID or Session ID found, can't request your data.", "error");
                                        setAnalyticsRequestLoading(false);
                                        return;
                                    }
                                    const url = `${EAM_PRIVACY_GATE_API}/user-data-request?clientIdHash=${hwid}&sessionId=${sessionId}`;
                                    const response = await fetch(url, requestOptions);

                                    if (!response.ok) {
                                        console.error("Error requesting data:", response.statusText);
                                        showSnackbar("Error requesting your data, please try again later.", "error");
                                        setAnalyticsRequestLoading(false);
                                        return;
                                    }

                                    const data = await response.json();
                                    if (!data) {
                                        console.error("Error requesting data: No data returned");
                                        showSnackbar("Error requesting your data, please try again later.", "error");
                                        setAnalyticsRequestLoading(false);
                                        return;
                                    }

                                    //Download the data
                                    const blob = new Blob([JSON.stringify(data)], { type: 'application/json' });
                                    const data_url = URL.createObjectURL(blob);
                                    const a = document.createElement('a');
                                    a.href = data_url;
                                    a.download = 'eam_data.json';
                                    a.click();
                                    URL.revokeObjectURL(data_url);

                                    showSnackbar("Your data has been downloaded as eam_data.json, you can open it using notepad.", "success");
                                } catch (error) {
                                    console.error(error);
                                    showSnackbar("Error requesting your data, please try again later.", "error");
                                } finally {
                                    setAnalyticsRequestLoading(false);
                                }
                            }}
                            loading={analyticsRequestLoading}
                        >
                            Request your data
                        </StyledButton>
                        <StyledButton
                            disabled={analyticsRequestLoading || dataDeletionRequestLoading || analyticsSettings?.optOut || analyticsSettings?.sendAnonymizedData}
                            color="secondary"
                            startIcon={<DeleteOutlineOutlinedIcon />}
                            sx={{
                                '&:hover': {
                                    backgroundColor: theme => theme.palette.error.main,
                                    color: theme => theme.palette.error.contrastText,
                                },
                            }}
                            onClick={(event) => setAnchorElDeletion(event.currentTarget)}
                            loading={dataDeletionRequestLoading}
                        >
                            {
                                !dataDeletionRequestLoading ?
                                    "Delete all your data"
                                    :
                                    "Loading please wait..."
                            }
                        </StyledButton>
                        <Popover
                            id={idDeletionPopup}
                            open={openDeletionPopup}
                            anchorEl={anchorElDeletion}
                            onClose={() => setAnchorElDeletion(null)}
                            anchorOrigin={{
                                vertical: 'bottom',
                                horizontal: 'left',
                            }}
                        >
                            <Paper
                                sx={{
                                    p: 0.25,
                                    backgroundColor: theme => theme.palette.background.paper,
                                    borderRadius: `${theme.shape.borderRadius}px`,
                                }}
                            >
                                <Box
                                    sx={{
                                        display: 'flex',
                                        flexDirection: 'row',
                                        gap: 1.5,
                                        backgroundColor: theme => theme.palette.background.default,
                                        borderRadius: `${theme.shape.borderRadius - 2}px`,
                                        p: 1.25
                                    }}
                                >
                                    <Box
                                        sx={{
                                            display: 'flex',
                                            flexDirection: 'column',
                                            gap: 0,
                                        }}
                                    >
                                        <Typography variant='subtitle1' sx={{ px: 1 }}>
                                            Are you sure you want to delete all your data?
                                        </Typography>
                                        <Typography variant='body2' sx={{ mx: 'auto', mb: 1.5 }}>
                                            This action cannot be undone.
                                        </Typography>
                                        <StyledButton
                                            disabled={analyticsRequestLoading || dataDeletionRequestLoading || analyticsSettings?.optOut || analyticsSettings?.sendAnonymizedData}
                                            color="error"
                                            startIcon={<DeleteForeverOutlinedIcon />}
                                            onClick={deleteAllUserData}
                                        >
                                            Delete all your data
                                        </StyledButton>
                                    </Box>
                                    <Tooltip
                                        title={`${MASCOT_NAME} fears he will forget you if you delete your data, so he will be sad if you do this. But he understands if you want to do it. 😭`}
                                    >
                                        <img
                                            src={mascotImage ? mascotImage : "/mascot/Error/error_mascot_only_2_small_very_low_res.png"}
                                            alt="EAM Mascot"
                                            height='100px'
                                            style={{
                                                marginRight: '4px',
                                            }}
                                        />
                                    </Tooltip>
                                </Box>
                            </Paper>
                        </Popover>
                    </Box>
                    <Typography variant="body2" color="text.secondary" >
                        If you want, you can choose to send only anonymized data or opt-out of the analytics that are collect entirely.
                    </Typography>
                    {
                        analyticsSettings &&
                        <Box
                            sx={{
                                mt: 1,
                                display: 'flex',
                                flexDirection: 'column',
                                gap: 1,
                                px: 1.5
                            }}
                        >
                            <ColumnSwitch
                                label="Send only fully anonymized data"
                                checked={analyticsSettings.sendAnonymizedData}
                                onChange={(event) => {
                                    setAnalyticsSettings({
                                        sendAnonymizedData: event.target.checked,
                                        optOut: analyticsSettings.optOut,
                                    });

                                    if (event.target.checked) {
                                        showSnackbar("You need to restart EAM in order for the changes to take effect.", "success");
                                    }
                                }}
                            />
                            <ColumnSwitch
                                label="Opt-out of all analytics"
                                checked={analyticsSettings.optOut}
                                onChange={(event) => {
                                    setAnalyticsSettings({
                                        sendAnonymizedData: analyticsSettings.sendAnonymizedData,
                                        optOut: event.target.checked,
                                    });

                                    if (event.target.checked) {
                                        showSnackbar("You need to restart EAM in order for the changes to take effect.", "success");
                                    }
                                }}
                            />
                        </Box>
                    }
                </Box>
            </ComponentBox>
        </Box>
    );
}

export default SettingsPage;

function ColumnSwitch({ label, checked, onChange }) {
    return (
        <FormControlLabel
            id={"fcl-check-" + label}
            sx={{ gap: 0.5 }}
            control={<Checkbox sx={{ p: 0, pr: 0.25 }} checked={checked} onChange={onChange} />} label={label}
        />
    );
}

const VirtuosoTableComponents = {
    Scroller: forwardRef((props, ref) => (
        <TableContainer
            component={Paper}
            {...props}
            ref={ref}
            sx={{
                maxHeight: '500px',
                overflowY: 'auto',
                borderRadius: theme => `${theme.shape.borderRadius - 2}px`,
            }}
        />
    )),
    Table: (props) => (
        <Table
            stickyHeader
            {...props}
            sx={{
                borderCollapse: 'separate',
                tableLayout: 'fixed',
                backgroundColor: theme => theme.palette.background.default,
                '& thead th': {
                    borderBottom: 'none',
                    backgroundColor: theme => theme.palette.background.paper,
                },
                '& tbody tr:last-child td, & tbody tr:last-child th': {
                    borderBottom: 'none',
                },
            }}
        />
    ),
    TableHead: forwardRef((props, ref) => <TableHead {...props} ref={ref} />),
    TableRow: forwardRef((props, ref) => <TableRow {...props} ref={ref} />),
    TableBody: forwardRef((props, ref) => <TableBody {...props} ref={ref} />),
};

function AccountSelectorTable({ settings, setCheckedMails }) {
    const [rows, setRows] = useState([]);

    const { accounts } = useAccounts();
    const vaultPeekerSettings = settings.vaultPeeker.collapsedFileds;

    useEffect(() => {
        const emails = vaultPeekerSettings.accounts;
        const newRows = accounts.map((acc) => {
            return {
                checked: emails.includes(acc.email),
                email: acc.email,
                name: acc.name,
            };
        });

        setRows(newRows);
    }, []);

    const tableHeaderContent = () => {
        return (
            <TableRow>
                <TableCell sx={{ minWidth: 100, maxWidth: 100, width: 100, textAlign: 'start' }}>
                    <Typography>
                        Collapsed
                    </Typography>
                </TableCell>
                <TableCell sx={{ minWidth: 175, maxWidth: 175, width: 175, textAlign: 'start' }}>
                    <Typography>
                        Accountname
                    </Typography>
                </TableCell>
                <TableCell sx={{ textAlign: 'start' }}>
                    <Typography>
                        Email
                    </Typography>
                </TableCell>
            </TableRow>
        );
    };

    const rowContent = (_index, row) => {
        return (
            <TableRow
                key={`${row.email}_${_index}`}
            >
                <TableCell sx={{ minWidth: 100, maxWidth: 100, width: 100, borderBottom: 'none', textAlign: 'start', borderRadius: theme => `${theme.shape.borderRadius}px 0 0 ${theme.shape.borderRadius}px` }}>
                    <Checkbox
                        checked={row.checked}
                        onChange={() => {
                            const checkedState = rows.find((r) => r.email === row.email).checked;
                            const r = rows;
                            const index = r.findIndex((r) => r.email === row.email);
                            r[index].checked = !checkedState;
                            setRows(r);

                            const checkedRows = r.filter((r) => r.checked);
                            const emails = checkedRows.map((r) => r.email);
                            setCheckedMails(emails);
                        }}
                    />
                </TableCell>
                <TableCell sx={{ minWidth: 175, maxWidth: 175, width: 175, borderBottom: 'none', textAlign: 'start' }}>
                    {row.name}
                </TableCell>
                <TableCell sx={{ borderBottom: 'none', textAlign: 'start', borderRadius: theme => `0 ${theme.shape.borderRadius}px ${theme.shape.borderRadius}px 0` }}>
                    {row.email}
                </TableCell>
            </TableRow>
        );
    };

    return (
        <Box
            sx={{
                width: '600px',
                height: '500px',
            }}
        >
            <TableVirtuoso
                data={rows}
                components={VirtuosoTableComponents}
                fixedHeaderContent={tableHeaderContent}
                itemContent={rowContent}
            />
        </Box>
    );
}
