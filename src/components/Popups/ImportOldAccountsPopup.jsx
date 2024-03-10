import { Typography } from "@mui/material";
import StyledButton from "../StyledButton";
import PopupBase from "./PopupBase";
import { invoke } from "@tauri-apps/api";
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
                This is a new version of the EAM application.
                It has been rewritten from scratch to improve performance, security and (soon™) to add new and exciting features.
            </Typography>
            <Typography
                variant="body1"
                component="div"
            >
                Sadly it is not compatible with the older versions, so you need to import your accounts to use them here.
                If you want to do that, you can use the button below to do that automatically.<br />
                Older versions of the EAM will still work and you can use it to access your accounts.
            </Typography>
            <Typography
                variant="body1"
                component="div"
            >
                If you don't want to import your old accounts, you can just close this popup and start using the application as if it was a new installation.
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
                        }
                        updateAccount(acc);
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