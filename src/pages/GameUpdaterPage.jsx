
import { tauri } from '@tauri-apps/api';
import { getAppInit, getGameFileList } from '../backend/decaApi';
import StyledButton from './../components/StyledButton';
import useUserSettings from '../hooks/useUserSettings';
import { Box, Table, TableBody, TableCell, TableContainer, TableRow, Typography } from '@mui/material';
import { useEffect, useState } from 'react';
import { UPDATE_URLS } from '../constants';
import ComponentBox from '../components/ComponentBox';
import SystemUpdateAltOutlinedIcon from '@mui/icons-material/SystemUpdateAltOutlined';
import { getCurrentTime } from '../utils/timeUtils';
import AccessTimeOutlinedIcon from '@mui/icons-material/AccessTimeOutlined';
import BeenhereOutlinedIcon from '@mui/icons-material/BeenhereOutlined';
import NewReleasesOutlinedIcon from '@mui/icons-material/NewReleasesOutlined';

function GameUpdaterPage() {
    const settings = useUserSettings();
    const [isLoading, setIsLoading] = useState(false);

    const [isInitialLoading, setIsInitialLoading] = useState(true);
    const [updateRequired, setUpdateRequired] = useState(false);
    const [filesToUpdate, setFilesToUpdate] = useState([]);
    const [buildHash, setBuildHash] = useState('');
    const [lastUpdateCheck, setLastUpdateCheck] = useState('never');

    useEffect(() => {
        const updateNeeded = localStorage.getItem('updateNeeded');
        console.log('updateNeeded', updateNeeded);
        if (updateNeeded) {
            setUpdateRequired(true);
        }
    }, []);


    useEffect(() => {
        const lastUpdateText = getLastUpdateCheckText();
        setLastUpdateCheck(lastUpdateText);
    }, [localStorage.getItem('lastUpdateCheck')]);

    useEffect(() => {
        if (isInitialLoading) {
            setIsInitialLoading(false);
            return;
        }
        setUpdateRequired(filesToUpdate.length > 0);
    }, [filesToUpdate]);

    useEffect(() => {
        if (isInitialLoading) { return; }
        
        if (updateRequired) {
            localStorage.setItem('updateNeeded', updateRequired);
            return;
        }
        localStorage.removeItem('updateNeeded');
    }, [updateRequired]);

    const searchForUpdates = async () => {
        const buildHash = await getClientBuildHash();
        const fileList = await getFileList(buildHash);
        localStorage.setItem('lastUpdateCheck', getCurrentTime());
        return fileList;
    };

    const getClientBuildHash = async () => {
        return await getAppInit()
            .then(async (appInit) => {
                const appSettings = appInit.AppSettings;
                console.log('appSettings', appSettings);
                setBuildHash(appSettings.BuildHash);
                return appSettings.BuildHash;
            });
    };

    const getFileList = async (buildHash) => {
        return await getGameFileList(buildHash)
            .then(async (fileList) => {
                console.log('fileList', fileList);
                const file = await tauri.invoke(
                    'get_game_files_to_update',
                    {
                        args: {
                            game_exe_path: settings.getByKeyAndSubKey('game', 'exePath'),
                            game_files_data: JSON.stringify(fileList),
                        }
                    });
                setFilesToUpdate(file);
                setIsLoading(false);
                return file;
            });
    };

    const getLastUpdateCheckText = () => {
        const lastUpdateCheck = localStorage.getItem('lastUpdateCheck');
        if (lastUpdateCheck) {
            return lastUpdateCheck;
        }
        return 'never';
    };

    return (
        <Box>
            <ComponentBox
                headline="Realm Updater"
                icon={<SystemUpdateAltOutlinedIcon />}
                sx={{
                    display: "flex",
                    flexDirection: "column",
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
                            console.time("update game");
                            console.log("update game...");
                            setIsLoading(true);
                            let fileList = filesToUpdate;
                            if (filesToUpdate.length === 0) {
                                console.log("searching for updates");
                                fileList = await searchForUpdates();
                                console.log("searching for updates done");
                                setIsLoading(true);
                            }

                            const updateFiles = fileList.map((file) => {
                                return {
                                    ...file,
                                    url: UPDATE_URLS(2, [buildHash, file.file]),
                                };
                            });

                            tauri.invoke('perform_game_update', {
                                args: {
                                    game_exe_path: settings.getByKeyAndSubKey('game', 'exePath'),
                                    game_files_data: JSON.stringify(updateFiles),
                                }
                            }).then((result) => {
                                console.log(result);
                                console.timeEnd("update game");
                                setFilesToUpdate([]);
                                setUpdateRequired(false);
                                setIsLoading(false);
                            })
                                .catch((error) => {
                                    console.timeEnd("update game");
                                    console.error(error);
                                    setFilesToUpdate([]);
                                    setIsLoading(false);
                                });
                        }}
                    >
                        update game
                    </StyledButton>
                }
                <StyledButton
                    disabled={isLoading}
                    fullWidth
                    color={updateRequired ? 'secondary' : 'primary'}
                    onClick={async () => {
                        console.log("search for updates...");
                        setIsLoading(true);
                        await searchForUpdates();
                        console.log("search for updates done");
                    }}
                >
                    search for updates
                </StyledButton>
            </ComponentBox >
        </Box>
    );
}

export default GameUpdaterPage;