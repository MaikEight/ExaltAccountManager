
import { Box, Chip, FormControl, FormControlLabel, InputLabel, List, ListItem, MenuItem, OutlinedInput, Select, Switch, TextField, Tooltip, Typography } from '@mui/material';
import ComponentBox from './../components/ComponentBox';
import FolderOutlinedIcon from '@mui/icons-material/FolderOutlined';
import { useContext, useEffect, useState } from 'react';
import StyledButton from './../components/StyledButton';
import { dialog } from '@tauri-apps/api';
import useUserSettings from '../hooks/useUserSettings';
import SearchOutlinedIcon from '@mui/icons-material/SearchOutlined';
import RestartAltOutlinedIcon from '@mui/icons-material/RestartAltOutlined';
import ViewColumnOutlinedIcon from '@mui/icons-material/ViewColumnOutlined';
import VisibilityOutlinedIcon from '@mui/icons-material/VisibilityOutlined';
import VisibilityOffOutlinedIcon from '@mui/icons-material/VisibilityOffOutlined';
import DarkModeOutlinedIcon from '@mui/icons-material/DarkModeOutlined';
import ColorContext from '../contexts/ColorContext';
import DnsOutlinedIcon from '@mui/icons-material/DnsOutlined';
import ServerContext from '../contexts/ServerContext';
import { useTheme } from '@emotion/react';
import ServerChip from '../components/GridComponents/ServerChip';

const ITEM_HEIGHT = 48;
const ITEM_PADDING_TOP = 8;
const MenuProps = {
    PaperProps: {
        style: {
            maxHeight: ITEM_HEIGHT * 4.5 + ITEM_PADDING_TOP,
            width: 250,
        },
    },
};

function SettingsPage() {
    const [initialSettings, setInitialSettings] = useState(true);
    const [settings, setSettings] = useState({});

    const columns = [
        { field: 'group', headerName: 'Group' },
        { field: 'name', headerName: 'Accountname' },
        { field: 'email', headerName: 'Email' },
        { field: 'lastLogin', headerName: 'Last Login' },
        { field: 'serverName', headerName: 'Server' },
        { field: 'lastRefresh', headerName: 'Last refresh' },
        { field: 'performDailyLogin', headerName: 'Daily Login' },
        { field: 'state', headerName: 'Last State' }
    ];
    const theme = useTheme();
    const userSettings = useUserSettings();
    const colorContext = useContext(ColorContext);
    const { serverList } = useContext(ServerContext);

    useEffect(() => {
        setSettings(userSettings.get);
    }, []);

    useEffect(() => {
        setSettings(userSettings.get);
    }, [userSettings.get]);

    useEffect(() => {
        if (initialSettings) {
            setInitialSettings(false);
            return;
        }
        if (settings === userSettings.get) return;

        userSettings.set(settings);
    }, [settings]);

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
                        value={settings?.game?.exePath ? settings.game.exePath : ""}
                        onChange={(event) => setSettings({ ...settings, game: { ...settings.game, exePath: event.target.value } })}
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
                                    setSettings({ ...settings, game: { ...settings.game, exePath: filePath } })
                                }
                            }}
                        >
                            Choose file
                        </StyledButton>
                        <StyledButton
                            color="secondary"
                            startIcon={<RestartAltOutlinedIcon />}
                            onClick={() => {
                                let newSettings = settings;
                                delete newSettings.game.exePath;
                                setSettings(userSettings.addDefaults(newSettings));
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
                <Box sx={{ width: '100%', display: 'flex', flexWrap: 'wrap', gap: 1 }}>
                    {
                        columns.map((column) =>
                            <Box key={column.field} sx={{ flexBasis: '20%' }}>
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
                            delete newSettings.accounts.columnsHidden;
                            setSettings(userSettings.addDefaults(newSettings));
                        }}
                    >
                        show all columns
                    </StyledButton>
                </Box>
            </ComponentBox>

            {/* Default Server */}
            <ComponentBox
                title="Default Server"
                icon={<DnsOutlinedIcon />}
            >
                <Typography variant="body2" color="text.secondary" sx={{ mb: 2 }}>
                    Choose which server should be used by default.
                </Typography>
                {
                    (serverList && serverList.length > 0) ? null :
                        <Typography variant="body2" color="text.secondary" sx={{ mb: 2 }}>
                            To add servers to this list, please add an account and click on "Refresh data".
                        </Typography>
                }
                <FormControl sx={{ m: 1, width: 175 }}>
                    <InputLabel id="default-server-list-label">Default server</InputLabel>
                    <Select
                        labelId="default-server-list-label"
                        id="default-server-list"
                        value={settings?.game?.defaultServer ? settings.game.defaultServer : "Last server"}
                        onChange={(event) => { setSettings({ ...settings, game: { ...settings.game, defaultServer: event.target.value } }) }}
                        input={<OutlinedInput id="select-default-server-list" label="Default server" />}
                        renderValue={(selected) => (
                            <ServerChip key={"key-" + selected} params={{ value: selected }} />
                        )}
                        MenuProps={MenuProps}
                    >
                        {
                            [
                                { Name: 'Last server', DNS: 'LAST' },
                                ...(serverList && serverList.length > 0 ? serverList : [])
                            ].map((server) => (
                                <MenuItem
                                    key={server.DNS}
                                    value={server.Name}
                                    style={{
                                        fontWeight:
                                            settings?.game?.defaultServer === server.Name
                                                ? theme.typography.fontWeightRegular
                                                : theme.typography.fontWeightMedium,
                                    }}
                                >
                                    {server.Name}
                                </MenuItem>
                            ))}
                    </Select>
                </FormControl>
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

function ColumnSwitch({ label, checked, onChange }) {
    return (
        <FormControlLabel sx={{ gap: 0.5 }} control={<Switch size="small" checked={checked} onChange={onChange} />} label={label} />
    );
}

export default SettingsPage;