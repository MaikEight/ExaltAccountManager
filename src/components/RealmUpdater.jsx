import StyledButton from '../components/StyledButton';
import useUserSettings from '../hooks/useUserSettings';
import { Box, Table, TableBody, TableCell, TableContainer, TableRow, Typography } from '@mui/material';
import { useEffect, useState } from 'react';
import ComponentBox from '../components/ComponentBox';
import SystemUpdateAltOutlinedIcon from '@mui/icons-material/SystemUpdateAltOutlined';
import AccessTimeOutlinedIcon from '@mui/icons-material/AccessTimeOutlined';
import BeenhereOutlinedIcon from '@mui/icons-material/BeenhereOutlined';
import NewReleasesOutlinedIcon from '@mui/icons-material/NewReleasesOutlined';
import { checkForUpdates, updateGame } from 'eam-commons-js';
import useSnack from '../hooks/useSnack';
import SearchOutlinedIcon from '@mui/icons-material/SearchOutlined';
import { getCurrentWindow } from '@tauri-apps/api/window'

function RealmUpdater() {
    const settings = useUserSettings();
    const [isLoading, setIsLoading] = useState(false);
    const [updateRequired, setUpdateRequired] = useState(false);
    const [lastUpdateCheck, setLastUpdateCheck] = useState('never');

    const { showSnackbar } = useSnack();

    const updateProgressbar = async (show) => await getCurrentWindow().setProgressBar({
        status: show ? 'indeterminate' : 'none',
        value: show ? 50 : 0,
    });

    useEffect(() => {
        const checkSessionStorage = () => {
            const updateCheckInProgress = sessionStorage.getItem('updateCheckInProgress');
            const updateInProgress = sessionStorage.getItem('updateInProgress');

            setIsLoading(updateCheckInProgress === 'true' || updateInProgress === 'true');
            setUpdateRequired(localStorage.getItem('updateNeeded') === 'true');
        };
        checkSessionStorage();

        const intervalId = setInterval(checkSessionStorage, 750);
        return () => { clearInterval(intervalId); }
    }, []);

    useEffect(() => {
        const lastUpdateText = getLastUpdateCheckText();
        setLastUpdateCheck(lastUpdateText);
    }, [localStorage.getItem('lastUpdateCheck')]);

    const getLastUpdateCheckText = () => {
        const lastUpdateCheck = localStorage.getItem('lastUpdateCheck');
        if (lastUpdateCheck) {
            return lastUpdateCheck;
        }
        return 'never';
    };

    return (
        <Box
            sx={{
                minWidth: '300px',
                maxWidth: '300px',
                height: '220px',
            }}
        >
            <ComponentBox
                title="Realm Updater"
                icon={<SystemUpdateAltOutlinedIcon />}
                sx={{
                    display: "flex",
                    flexDirection: "column",
                    m: 0,
                }}
                innerSx={{
                    display: "flex",
                    flexDirection: "column",
                    gap: 1,
                }}
                isLoading={isLoading}
            >
                <Typography variant="body1" >
                    Search for updates and corrupted files.
                </Typography>

                <TableContainer component={Box}>
                    <Table
                        sx={{
                            '& tbody  td, & tbody th': {
                                borderBottom: 'none',
                                padding: 0,
                                paddingTop: 0.25,
                                paddingBottom: 1,
                            },
                        }}
                        density="compact"
                    >
                        <TableBody>
                            <TableRow>
                                <TableCell component="th" scope="row"   >
                                    <Box
                                        sx={{
                                            display: 'flex',
                                            flexDirection: 'row',
                                            gap: 0.5,
                                            textAlign: 'center'
                                        }}
                                    >
                                        <AccessTimeOutlinedIcon />
                                        <Typography variant="body1" fontWeight={300}>
                                            Last checked
                                        </Typography>
                                    </Box>
                                </TableCell>
                                <TableCell align="left">
                                    {lastUpdateCheck}
                                </TableCell>
                            </TableRow>
                            <TableRow>
                                <TableCell component="th" scope="row">
                                    <Box
                                        sx={{
                                            display: 'flex',
                                            flexDirection: 'row',
                                            gap: 0.5,
                                            textAlign: 'center'
                                        }}
                                    >
                                        {updateRequired > 0 ? <NewReleasesOutlinedIcon /> : <BeenhereOutlinedIcon />}
                                        <Typography variant="body1" fontWeight={300}>
                                            State
                                        </Typography>
                                    </Box>
                                </TableCell>
                                <TableCell align="left">
                                    {updateRequired ? 'Update available' : 'Up to date'}
                                </TableCell>
                            </TableRow>
                        </TableBody>
                    </Table>
                </TableContainer>
                {
                    updateRequired &&
                    <StyledButton
                        disabled={isLoading}
                        fullWidth
                        onClick={async () => {
                            setIsLoading(true);
                            updateProgressbar(true);
                            try {
                                await updateGame();
                            } catch (error) {
                                console.error('Failed to update the game', error);
                                showSnackbar('Failed to update the game', 'error');
                            } finally {
                                setIsLoading(false);
                                updateProgressbar(false);
                            }
                        }}
                        startIcon={<SystemUpdateAltOutlinedIcon />}
                    >
                        update game
                    </StyledButton>
                }
                <StyledButton
                    disabled={isLoading}
                    fullWidth
                    color={updateRequired ? 'secondary' : 'primary'}
                    onClick={async () => {
                        setIsLoading(true);
                        updateProgressbar(true);
                        try {
                            const updateNeeded = await checkForUpdates(true);
                            setUpdateRequired(updateNeeded);
                        } catch (error) {
                            console.error('Failed to check for updates', error);
                            showSnackbar('Failed to check for game updates', 'error');
                        } finally {
                            setIsLoading(false);
                            updateProgressbar(false);
                        }
                    }}
                    startIcon={<SearchOutlinedIcon />}
                >
                    search for updates
                </StyledButton>
            </ComponentBox >
        </Box>
    );
}

export default RealmUpdater;