import { invoke } from "@tauri-apps/api";
import ImportOldAccountsPopup from '../components/Popups/ImportOldAccountsPopup';
import WelcomeToEamPopup from '../components/Popups/WelcomeToEamPopup';
import useAccounts from "./useAccounts";

function useStartupPopups() {
    const { accounts } = useAccounts();

    const checkForFirstEamStart = () => {
        const firstEamStart = localStorage.getItem('firstEamStart');
        if (firstEamStart === null) {
            return true;
        }
        return false;
    };

    const getFirstWelcomePopup = async () => {
        const hasOldAccountData = await invoke('has_old_eam_save_file');
        if (hasOldAccountData && accounts.length === 0) {
            return {
                preventClose: true,
                content: (<ImportOldAccountsPopup />),
            };
        }

        return {
            preventClose: true,
            content: (<WelcomeToEamPopup />),
        };
    };

    const performPopupCheck = async () => {
        const isFirstEamStart = checkForFirstEamStart();
        if (isFirstEamStart) {
            return await getFirstWelcomePopup();
        }

        return null;
    }

    return {
        performPopupCheck: performPopupCheck,
    };
}

export default useStartupPopups;