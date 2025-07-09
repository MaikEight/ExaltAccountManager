import { TrayIcon } from '@tauri-apps/api/tray';
import { getCurrentWindow } from '@tauri-apps/api/window'
import { defaultWindowIcon } from '@tauri-apps/api/app';
import { Menu } from '@tauri-apps/api/menu';

async function addSystemTray() {

    const menu = await Menu.new({
        items: [
            {
                id: 'open',
                text: 'Open Exalt Account Manager',
                action: () => onTrayMenuClick('open'),
            },
            {
                id: 'quit',
                text: 'Quit Exalt Account Manager',
                action: () => onTrayMenuClick('quit'),
            },
        ],
    });

    const onTrayMenuClick = async (itemId) => {
        switch (itemId) {
            case 'open':
                await getCurrentWindow().show().catch(console.error);
                const isMinimized = await getCurrentWindow().isMinimized().catch(console.error);
                if (isMinimized) {
                    await getCurrentWindow().unminimize().catch(console.error);
                }
                await getCurrentWindow().setFocus().catch(console.error);
                break;
            case 'quit':
                getCurrentWindow().close().catch(console.error);
                break;
            default:
                console.warn(`Unknown tray menu item clicked: `, itemId);
                break;
        }
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