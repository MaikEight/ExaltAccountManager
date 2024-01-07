
import { Box, FormControlLabel, List, ListItem, Switch, TextField, Typography } from '@mui/material';
import ComponentBox from './../components/ComponentBox';
import FolderOutlinedIcon from '@mui/icons-material/FolderOutlined';
import { useEffect, useState } from 'react';
import StyledButton from './../components/StyledButton';
import { dialog } from '@tauri-apps/api';
import useUserSettings from '../hooks/useUserSettings';
import SearchOutlinedIcon from '@mui/icons-material/SearchOutlined';
import RestartAltOutlinedIcon from '@mui/icons-material/RestartAltOutlined';
import ViewColumnOutlinedIcon from '@mui/icons-material/ViewColumnOutlined';

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
        { field: 'performDailyLogin', headerName: 'Daily Login' }
    ];

    const userSettings = useUserSettings();

    useEffect(() => {
        setSettings(userSettings.get);
    }, []);

    useEffect(() => {
        setSettings(userSettings.get);
    }, [userSettings.get]);

    useEffect(() => {
        console.log(settings);
        if (initialSettings) {
            setInitialSettings(false);
            return;
        }
        if (settings === userSettings.get) return;

        userSettings.set(settings);
    }, [settings]);



    return (
        <Box sx={{ width: '100%' }}>
            <ComponentBox
                headline="Game Path"
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
            <ComponentBox
                headline="Accounts columns"
                icon={<ViewColumnOutlinedIcon />}
            >
                <Typography variant="body2" sx={{ mb: 1 }}>
                    Choose which columns should be shown in the accounts table by default.
                </Typography>
                <List>
                    {
                        columns.map((column) =>
                            <ListItem key={column.field}>
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
                            </ListItem>)
                    }
                </List>
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