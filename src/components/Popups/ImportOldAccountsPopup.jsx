import { Typography } from "@mui/material";
import StyledButton from "../StyledButton";
import PopupBase from "./PopupBase";
import { invoke } from "@tauri-apps/api/core";
import { useState } from "react";
import usePopups from "../../hooks/usePopups";
import useAccounts from "../../hooks/useAccounts";

function ImportOldAccountsPopup() {
    const [isLoading, setIsLoading] = useState(false);
    const { closePopup } = usePopups();
    const { updateAccount } = useAccounts();

    return (
        <PopupBase
            title="Welcome to the all new Exalt Account Manager!"
        >
            <Typography
                variant="body1"
                component="div"
            >
                This new version of EAM has been completely rewritten from scratch to enhance performance, bolster security, and soon™ to introduce exciting new features.
            </Typography>
            <Typography
                variant="body1"
                component="div"
            >
                Unfortunately, it's not backward compatible with older versions. You'll need to migrate your accounts to use them in this version.
                If you wish to proceed, you can conveniently do so by using the button below for automated migration.<br />
                Rest assured, older versions of EAM will continue to function, allowing you to access your accounts there as well.
            </Typography>
            <Typography
                variant="body1"
                component="div"
            >
                If you prefer not to migrate your accounts, you can simply start fresh.
            </Typography>
            <StyledButton
                disabled={isLoading}
                onClick={async () => {
                    setIsLoading(true);

                    const oldAccountsString = await invoke('format_eam_v3_save_file_to_readable_json');
                    const oldAccounts = JSON.parse(oldAccountsString);

                    oldAccounts.forEach((oldAccount) => {
                        const acc = {
                            ...oldAccount,
                            isSteam: false,
                            isDeleted: false,
                        }
                        updateAccount(acc, true);
                    });

                    localStorage.setItem('firstEamStart', 'false');

                    setIsLoading(false);
                    closePopup();
                }}
            >
                Import accounts
            </StyledButton>

            <StyledButton
                disabled={isLoading}
                color='secondary'
                onClick={() => {
                    localStorage.setItem('firstEamStart', 'false');
                    closePopup();
                }}
            >
                start fresh
            </StyledButton>
        </PopupBase>
    );
}

export default ImportOldAccountsPopup;