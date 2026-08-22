import StyledButton from '../components/StyledButton';
import { Box, Table, TableBody, TableCell, TableContainer, TableRow, Typography } from '@mui/material';
import ComponentBox from '../components/ComponentBox';
import SystemUpdateAltOutlinedIcon from '@mui/icons-material/SystemUpdateAltOutlined';
import AccessTimeOutlinedIcon from '@mui/icons-material/AccessTimeOutlined';
import BeenhereOutlinedIcon from '@mui/icons-material/BeenhereOutlined';
import NewReleasesOutlinedIcon from '@mui/icons-material/NewReleasesOutlined';
import useSnack from '../hooks/useSnack';
import SearchOutlinedIcon from '@mui/icons-material/SearchOutlined';
import useGameUpdate from '../hooks/useGameUpdate';

function RealmUpdater() {
    // The updater state is shared, so an update started by the daily login shows
    // here too, and the taskbar is driven centrally by the provider.
    const {
        isBusy,
        isUpdateAvailable,
        lastCheckedAt,
        checkForUpdate,
        performUpdate,
    } = useGameUpdate();

    const { showSnackbar } = useSnack();
    const lastUpdateCheck = lastCheckedAt || 'never';

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
                isLoading={isBusy}
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
                                        {isUpdateAvailable ? <NewReleasesOutlinedIcon /> : <BeenhereOutlinedIcon />}
                                        <Typography variant="body1" fontWeight={300}>
                                            State
                                        </Typography>
                                    </Box>
                                </TableCell>
                                <TableCell align="left">
                                    {isUpdateAvailable ? 'Update available' : 'Up to date'}
                                </TableCell>
                            </TableRow>
                        </TableBody>
                    </Table>
                </TableContainer>
                {
                    isUpdateAvailable &&
                    <StyledButton
                        disabled={isBusy}
                        fullWidth
                        onClick={async () => {
                            const updateSucceeded = await performUpdate();
                            showSnackbar(
                                updateSucceeded ? 'Realm updated' : 'Failed to update Realm',
                                updateSucceeded ? 'success' : 'error');
                        }}
                        startIcon={<SystemUpdateAltOutlinedIcon />}
                    >
                        update game
                    </StyledButton>
                }
                <StyledButton
                    disabled={isBusy}
                    fullWidth
                    color={isUpdateAvailable ? 'secondary' : 'primary'}
                    onClick={async () => {
                        try {
                            await checkForUpdate(true);
                        } catch (error) {
                            console.error('Failed to check for updates', error);
                            showSnackbar('Failed to check for game updates', 'error');
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
