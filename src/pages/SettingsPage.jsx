
import { Box, Checkbox, FormControlLabel, Paper, Popover, Switch, Table, TableBody, TableCell, TableContainer, TableHead, TableRow, TextField, Tooltip, Typography } from '@mui/material';
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
import { ColorContext } from 'eam-commons-js';
import DnsOutlinedIcon from '@mui/icons-material/DnsOutlined';
import useSnack from '../hooks/useSnack';
import useServerList from '../hooks/useServerList';
import ServerListSelect from '../components/ServerListSelect';
import VaultPeekerLogo from '../components/VaultPeekerLogo';
import { useTheme } from '@emotion/react';
import useAccounts from '../hooks/useAccounts';
import { TableVirtuoso } from 'react-virtuoso';
import * as dialog from "@tauri-apps/plugin-dialog"
import useApplySettingsToHeaderName from '../hooks/useApplySettingsToHeaderName';

function SettingsPage() {
    const [initialSettings, setInitialSettings] = useState(true);
    const [settings, setSettings] = useState({});
    const [gameExePath, setGameExePath] = useState("");
    const [openVaultPeekerAccountsPoppover, setOpenVaultPeekerAccountsPoppover] = useState(false);
    const openVaultPeekerButtonRef = useRef(null);

    const { applySettingsToHeaderName } = useApplySettingsToHeaderName();

    const columns = useMemo(() => {
        return [
            { field: 'orderId', headerName: applySettingsToHeaderName('🆔 Order ID') },
            { field: 'group', headerName: applySettingsToHeaderName('👥 Group') },
            { field: 'name', headerName: applySettingsToHeaderName('🧑‍💼 Accountname') },
            { field: 'email', headerName: applySettingsToHeaderName('📧 Email') },
            { field: 'lastLogin', headerName: applySettingsToHeaderName('⏰ Last Login') },
            { field: 'serverName', headerName: applySettingsToHeaderName('🌐 Server') },
            { field: 'lastRefresh', headerName: applySettingsToHeaderName('🔄 Refresh') },
            { field: 'performDailyLogin', headerName: applySettingsToHeaderName('📅 Daily Login') },
            { field: 'state', headerName: applySettingsToHeaderName('📊 Last State') },
            { field: 'comment', headerName: applySettingsToHeaderName('💬 Comment') },
        ]
    }, [settings]);

    const userSettings = useUserSettings();
    const colorContext = useContext(ColorContext);
    const { serverList } = useServerList();
    const { showSnackbar } = useSnack();
    const theme = useTheme();

    const setTheSettings = async () => {
        const s = userSettings.get;
        const _gameExePath = await userSettings.getByKeyAndSubKey('game', 'exePath');
        setSettings(s);
        setGameExePath(_gameExePath);
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

    const isDarkMode = () => {
        if (settings === undefined || settings.general === undefined || settings.general.theme === undefined)
            return true;

        return settings.general.theme === 'dark';
    };

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
            >
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
