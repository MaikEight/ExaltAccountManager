import { Box, Typography } from "@mui/material";
import SideBarLogo from "../components/Sidebar/SideBarLogo";
import CustomToolbar from "../components/Sidebar/CustomToolbar";
import StyledButton from "../components/StyledButton";
import { relaunch } from '@tauri-apps/plugin-process';
import RestartAltOutlinedIcon from '@mui/icons-material/RestartAltOutlined';
import WarningAmberOutlinedIcon from '@mui/icons-material/WarningAmberOutlined';
import DiscordLogoFull from "../components/DiscordLogoFull";
import ContactMailOutlinedIcon from '@mui/icons-material/ContactMailOutlined';

function FatalErrorPage() {

    return (
        <Box
            data-tauri-drag-region
            sx={{
                display: "flex",
                flexDirection: "column",
                pb: 2,
            }}
        >
            {/* HEADER */}
            <Box
                data-tauri-drag-region
                sx={{
                    display: "flex",
                    flexDirection: "row",
                    width: "100%",
                    justifyContent: "space-between",
                    transition: (theme) => theme.transitions.create(["background-color", "color"]),
                }}
            >
                <Box
                    sx={{
                        pt: 1.5,
                    }}
                >
                    <SideBarLogo />
                </Box>
                <CustomToolbar />
            </Box>
            {/* CONTENT */}
            <Box
                sx={{
                    display: "flex",
                    flexDirection: "column",
                    width: "100%",
                    gap: 2,
                    ml: 2,
                }}
            >
                <Typography
                    variant={"h4"}
                    color={'error'}
                    sx={{
                        display: 'flex',
                        mt: 2,
                    }}
                >
                    <WarningAmberOutlinedIcon sx={{ fontSize: 32, mr: 1, mt: 0.5 }} />
                    Fatal Error
                </Typography>
                <Box
                    sx={{
                        display: "flex",
                        flexDirection: "column",
                    }}
                >
                    <Typography variant="body1">A fatal error has occurred that EAM can't recover from, please restart the application.</Typography>
                    <Typography variant="body1">If the error persists, please contact the support using the buttons bellow.</Typography>
                </Box>
                <StyledButton
                    sx={{ width: '152px' }}
                    color='warning'
                    onClick={async () => await relaunch()}
                    startIcon={<RestartAltOutlinedIcon />}
                >
                    Restart EAM
                </StyledButton>
                <Box
                    sx={{
                        display: "flex",
                        flexDirection: "row",
                        gap: 2,
                    }}
                >

                    <a href="https://discord.exalt-account-manager.eu" target="_blank" rel="noopener noreferrer">
                        <StyledButton
                            sx={{
                                width: '152px',
                                height: '100%',
                            }}
                        >
                            <DiscordLogoFull
                                sx={{
                                    display: 'flex',
                                    width: '100%',
                                    height: '20px',
                                    opacity: 1,
                                    transition: 'opacity 0.3s ease',
                                }}
                            />
                        </StyledButton>
                    </a>
                    <a href="mailto:mail@maik8.de" target="_blank" rel="noopener noreferrer">
                        <StyledButton
                            sx={{
                                width: '152px',
                                height: '100%',
                            }}
                            color={'secondary'}
                            startIcon={<ContactMailOutlinedIcon />}
                        >
                            Write a Mail
                        </StyledButton>
                    </a>
                </Box>
            </Box>
        </Box>
    )
}

export default FatalErrorPage;