
import { tauri } from '@tauri-apps/api';
import { getAppInit, getGameFileList } from '../backend/decaApi';
import StyledButton from './../components/StyledButton';
import useUserSettings from '../hooks/useUserSettings';
import { LinearProgress } from '@mui/material';
import { useState } from 'react';
import { writeFile } from '../utils/writeFileUtil';
import pako from 'pako';
import { UPDATE_URLS } from '../constants';

function GameUpdaterPage() {
    const settings = useUserSettings();
    const [isLoading, setIsLoading] = useState(false);

    const [filesToUpdate, setFilesToUpdate] = useState([]);
    const [buildHash, setBuildHash] = useState('b745db7679b66d42c1caee6d33edaa9c');

    const getClientBuildHash = () => {
        getAppInit()
            .then((appInit) => {
                const appSettings = appInit.AppSettings;
                console.log('appSettings', appSettings);
                setBuildHash(appSettings.BuildHash);
                getFileList(appSettings.BuildHash);
            });
    };

    const getFileList = (buildHash) => {
        getGameFileList(buildHash)
            .then(async (fileList) => {
                console.log('fileList', fileList);
                setFilesToUpdate(await tauri.invoke(
                    'get_game_files_to_update',
                    {
                        args: {
                            game_exe_path: settings.getByKeyAndSubKey('game', 'exePath'),
                            game_files_data: JSON.stringify(fileList),
                        }
                    }
                ));
                setIsLoading(false);
            });
    };

    return (
        <div>
            <h1>Game Updater Page</h1>
            {isLoading && <LinearProgress />}
            <br />
            <StyledButton
                disabled={isLoading}
                onClick={() => {
                    console.log("search for updates...");
                    setIsLoading(true);
                    getClientBuildHash();
                }}
            >
                search for updates
            </StyledButton>

            <StyledButton
                disabled={filesToUpdate.length === 0 || isLoading}
                sx={{ marginLeft: '10px' }}
                onClick={async () => {
                    console.time("update game");
                    console.log("update game...");
                    setIsLoading(true);
                    // const tempDir = await tauri.invoke('get_temp_folder_path_with_creation', {
                    //     subPath: '/gameFiles' + Date.now(),
                    // });
                    // // const promises = [];
                    // // filesToUpdate.forEach((file) => {
                    // //     promises.push(downloadAndWriteGameFile(tempDir, buildHash, file));
                    // // });
                    // // // promises.push(downloadAndWriteGameFile(tempDir, buildHash, filesToUpdate[3]));
                    // // await Promise.all(promises);

                    // let queue = Promise.resolve();
                    // filesToUpdate.forEach((file) => {
                    //     queue = queue.then(() => downloadAndWriteGameFile(tempDir, buildHash, file));                        
                    // });
                    // await queue;

                    // console.log("downloaded all files");
                    // console.log("unpacking files...", tempDir);
                    // return;

                    // const movePromise = tauri.invoke('unpack_and_move_game_update_files', {
                    //     tempFolder: tempDir,
                    //     gameExePath: settings.getByKeyAndSubKey('game', 'exePath'),
                    // });

                    // movePromise.then((result) => {
                    //     setIsLoading(false);
                    //     console.log(result);
                    //     console.timeEnd("update game");
                    // });


                    ///////////////
                    const updateFiles = filesToUpdate.map((file) => {
                        return {
                            ...file,
                            url: UPDATE_URLS(2, [buildHash, file.file]).replace('/rotmg-build', 'https://rotmg-build.decagames.com')
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
                        setIsLoading(false);
                    })
                    .catch((error) => {
                        console.timeEnd("update game");
                        console.error(error);
                        setIsLoading(false);
                    });
                }}
            >
                update game
            </StyledButton>
        </div>
    );
}

export default GameUpdaterPage;