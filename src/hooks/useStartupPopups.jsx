import { invoke } from "@tauri-apps/api";
import ImportOldAccountsPopup from '../components/Popups/ImportOldAccountsPopup';
import WelcomeToEamPopup from '../components/Popups/WelcomeToEamPopup';
import useAccounts from "./useAccounts";
import ChangelogVersion4_1_0 from "../components/Popups/Changelogs/ChangelogVersion4_1_0";
import ChangelogVersion4_1_5 from "../components/Popups/Changelogs/ChangelogVersion4_1_5";
import ChangelogVersion4_2_0 from "../components/Popups/Changelogs/ChangelogVersion4_2_0";
import BetaTestPopup from "../components/Popups/BetaTestPopup";

const isBetaVersion = true;

function useStartupPopups() {
    const { accounts } = useAccounts();

    const changelogPopups = [
        {
            version: "4.1.0",
            preventClose: false,
            content: <ChangelogVersion4_1_0 />
        },
        {
            version: "4.1.5",
            preventClose: false,
            content: <ChangelogVersion4_1_5 />
        },
        // {
        //     version: "4.2.0",
        //     preventClose: false,
        //     content: <ChangelogVersion4_2_0 />
        // },
    ];

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

    const getChangelogPopup = () => {
        const lastChangelog = localStorage.getItem('lastChangelog');
        const lastChangelogIndex = changelogPopups.findIndex(changelog => changelog.version === lastChangelog);

        if (lastChangelogIndex === -1 || lastChangelogIndex < changelogPopups.length - 1) {
            localStorage.setItem('lastChangelog', changelogPopups[changelogPopups.length - 1].version);
            return changelogPopups[changelogPopups.length - 1];
        }

        return null;
    }

    const performPopupCheck = async () => {
        if(isBetaVersion) {
            return {
                preventClose: false,
                content: (<BetaTestPopup />)
            }
        }

        const isFirstEamStart = checkForFirstEamStart();
        if (isFirstEamStart) {
            return await getFirstWelcomePopup();
        }

        const changelogPopup = getChangelogPopup();

        if (changelogPopup) {
            return changelogPopup;
        }

        return null;
    }

    return {
        performPopupCheck: performPopupCheck,
    };
}

export default useStartupPopups;