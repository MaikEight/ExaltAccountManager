import { Box, Typography } from "@mui/material";
import PopupBase from "./PopupBase";

function BetaTestPopup() {
    return (
        <PopupBase
            title={'EAM v4.2.0b - Vault Peeker Beta #1'}
        >
            <Box
                sx={{
                    display: 'flex',
                    flexDirection: 'column',
                    alignItems: 'center',
                    justifyContent: 'center',
                    gap: 1,
                    width: '650px'
                }}
            >
                <Box
                    sx={{
                        display: 'flex',
                        flexDirection: 'column',
                        alignItems: 'center',
                        gap: 1,
                        width: '100%',
                    }}
                >
                    <Typography>
                        Welcome to the first beta test of the Vault Peeker feature of the Exalt Account Manager.
                    </Typography>
                    <Typography>
                    🚧 This feature is still in development and may not work as expected. 🚧
                    </Typography>
                </Box>
                <Typography variant="body2">
                    Please report any bugs or issues in the
                    <a
                        style={{ paddingLeft: '0.25em' }}
                        href={"https://discord.exalt-account-manager.eu"}
                        target="_blank"
                        rel="noopener noreferrer"
                    >
                        Discord server
                    </a>
                    .
                </Typography>
                <Box
                    sx={{
                        display: 'flex',
                        flexDirection: 'column',
                        justifyContent: 'center',
                        alignItems: 'center',
                        width: '100%',
                    }}
                >
                    <Typography variant="h6">
                        ✨ Thank you for your support and happy testing ✨
                    </Typography>
                </Box>
            </Box>
        </PopupBase>
    );
}

export default BetaTestPopup;