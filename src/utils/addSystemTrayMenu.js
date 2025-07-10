import { TrayIcon } from '@tauri-apps/api/tray';
import { getCurrentWindow } from '@tauri-apps/api/window'
import { defaultWindowIcon } from '@tauri-apps/api/app';
import { Menu } from '@tauri-apps/api/menu';
import navigationService from './navigationService';

async function addSystemTray() {
    
    const menu = await Menu.new({
        items: [
            {
                id: 'open',
                text: '📂 Open',                
                action: () => onTrayMenuClick('open'),
            },
            {
                id: 'accounts',
                text: '👥 Accounts',
                action: () => onTrayMenuClick('accounts'),
            },
            {
                id: 'vaultPeeker',
                text: '🔍 Vault Peeker',
                action: () => onTrayMenuClick('vaultPeeker'),
            },
            {
                id: 'dailyLogins',
                text: '📅 Daily Logins',
                action: () => onTrayMenuClick('dailyLogins'),
            },
            {
                id: 'settings',
                text: '⚙️ Settings',
                action: () => onTrayMenuClick('settings'),
            },
            {
                id: 'profile',
                text: '👤 Profile',
                action: () => onTrayMenuClick('profile'),
            },
            {
                id: 'more',
                text: '➕ More',
                items: [
                    {
                        id: 'utilities',
                        text: '🔧 Utilities',
                        action: () => onTrayMenuClick('utilities'),
                    },
                    {
                        id: 'logs',
                        text: '📋 Logs',
                        action: () => onTrayMenuClick('logs'),
                    },
                    {
                        id: 'importer',
                        text: '📥 Importer',
                        action: () => onTrayMenuClick('importer'),
                    },
                    {
                        id: 'feedback',
                        text: '💬 Feedback',
                        action: () => onTrayMenuClick('feedback'),
                    },
                    {
                        id: 'about',
                        text: 'ℹ️ About',
                        action: () => onTrayMenuClick('about'),
                    },
                ]
            },
            {
                id: 'separator',
                text: '—————————————————',
            },
            {
                id: 'quit',
                text: '❎ Quit Exalt Account Manager',
                action: () => onTrayMenuClick('quit'),
            },
        ],
    });

    const onTrayMenuClick = async (itemId) => {
        switch (itemId) {
            case 'open':
                await showAndFocusWindow();
                break;
            case 'accounts':
                await showAndFocusWindow();
                navigationService.goToAccounts();
                break;
            case 'vaultPeeker':
                await showAndFocusWindow();
                navigationService.goToVaultPeeker();
                break;
            case 'dailyLogins':
                await showAndFocusWindow();
                navigationService.goToDailyLogins();
                break;
            case 'utilities':
                await showAndFocusWindow();
                navigationService.goToUtilities();
                break;
            case 'settings':
                await showAndFocusWindow();
                navigationService.goToSettings();
                break;
            case 'logs':
                await showAndFocusWindow();
                navigationService.goToLogs();
                break;
            case 'about':
                await showAndFocusWindow();
                navigationService.goToAbout();
                break;
            case 'feedback':
                await showAndFocusWindow();
                navigationService.goToFeedback();
                break;
            case 'importer':
                await showAndFocusWindow();
                navigationService.goToImporter();
                break;                
            case 'profile':
                await showAndFocusWindow();
                navigationService.goToProfile();
                break;
            case 'quit':
                getCurrentWindow().close().catch(console.error);
                break;
            default:
                console.warn(`Unknown tray menu item clicked: `, itemId);
                break;
        }
    }

    const showAndFocusWindow = async () => {
        await getCurrentWindow().show().catch(console.error);
        const isMinimized = await getCurrentWindow().isMinimized().catch(console.error);
        if (isMinimized) {
            await getCurrentWindow().unminimize().catch(console.error);
        }
        await getCurrentWindow().setFocus().catch(console.error);
    }

    const options = {
        title: 'Exalt Account Manager',
        tooltip: 'Exalt Account Manager',
        icon: await defaultWindowIcon(),
        type: 'icon',
        menu: menu,

    };
    
    const trayIconId = sessionStorage.getItem('trayIconId');
    if (trayIconId) {
        TrayIcon.removeById(trayIconId).catch(console.error);
    }
    
    const tray = await TrayIcon.new(options);
    sessionStorage.setItem('trayIconId', tray.id);
}

export default addSystemTray;