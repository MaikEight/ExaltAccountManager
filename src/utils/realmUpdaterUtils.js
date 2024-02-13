import { tauri } from '@tauri-apps/api';
import { getAppInit, getGameFileList } from '../backend/decaApi';
import { getCurrentTime } from './timeUtils';
import { UPDATE_URLS } from '../constants';

async function checkForUpdates(gameExePath) {
    if (sessionStorage.getItem('updateCheckInProgress') === 'true' ||
        sessionStorage.getItem('updateInProgress') === 'true') {
        return
    };

    sessionStorage.setItem('updateCheckInProgress', 'true');

    const buildHash = await getClientBuildHash();
    const fileList = await getFileList(buildHash, gameExePath);

    if (fileList.length > 0) {
        localStorage.setItem('updateNeeded', 'true');
    } else {
        localStorage.removeItem('updateNeeded');
    }

    localStorage.setItem('lastUpdateCheck', getCurrentTime());
    sessionStorage.setItem('updateCheckInProgress', 'false');

    return fileList;
}

async function updateGame(gameExePath) {
    if (sessionStorage.getItem('updateCheckInProgress') === 'true' ||
        sessionStorage.getItem('updateInProgress') === 'true') {
        return
    };

    sessionStorage.setItem('updateInProgress', 'true');

    const buildHash = await getClientBuildHash();
    const fileList = await getFileList(buildHash, gameExePath);

    if(!sessionStorage.getItem('buildCDN')){
        await getAppInit();
    }
    const buildCDN = sessionStorage.getItem('buildCDN');

    if(!buildHash || !buildCDN) throw new Error('Build hash or CDN not found', buildHash, buildCDN);

    const updateFiles = fileList.map((file) => {
        return {
            ...file,
            url: `${buildCDN}${UPDATE_URLS(2, [buildHash, file.file])}`,
        };
    });
    
    tauri.invoke('perform_game_update', {
        args: {
            game_exe_path: gameExePath,
            game_files_data: JSON.stringify(updateFiles),
        }
    }).then(() => {
        sessionStorage.removeItem('buildHash');
        sessionStorage.removeItem('fileList');
        localStorage.removeItem('updateNeeded');
    }).finally(() => {
        sessionStorage.setItem('updateInProgress', 'false');
    });
}

async function getClientBuildHash() {

    if (sessionStorage.getItem('buildHash') !== null) {
        return sessionStorage.getItem('buildHash');
    }

    return await getAppInit()
        .then(async (appInit) => {
            console.log('appInit', appInit);
            const appSettings = appInit.AppSettings;
            sessionStorage.setItem('buildHash', appSettings.BuildHash);
            sessionStorage.setItem('buildCDN', appSettings.BuildCDN ? appSettings.BuildCDN.replace('build-release/', '') : 'https://rotmg-build.decagames.com/');
            return appSettings.BuildHash;
        });
};

async function getFileList(buildHash, gameExePath) {
    if (sessionStorage.getItem('fileList') !== null) {
        return JSON.parse(sessionStorage.getItem('fileList'));
    }

    if (!buildHash || buildHash === '' && sessionStorage.getItem('buildHash') !== null) {
        buildHash = sessionStorage.getItem('buildHash');
    }

    if(!buildHash || buildHash === '') {
        buildHash = await getClientBuildHash();
    }

    return await getGameFileList(buildHash)
        .then(async (fileList) => {
            const file = await tauri.invoke(
                'get_game_files_to_update',
                {
                    args: {
                        game_exe_path: gameExePath,
                        game_files_data: JSON.stringify(fileList),
                    }
                });

            if (file !== null && file.length > 0) {
                sessionStorage.setItem('fileList', JSON.stringify(file));
            }

            return file;
        });
};

export {
    checkForUpdates,
    updateGame
};