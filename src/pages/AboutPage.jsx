import { Box, Typography } from "@mui/material";
import ComponentBox from './../components/ComponentBox';
import NumbersOutlinedIcon from '@mui/icons-material/NumbersOutlined';
import CodeOutlinedIcon from '@mui/icons-material/CodeOutlined';
import { useTheme } from "@emotion/react";
import GitHubStars from "../components/GitHubStars";
import CelebrationOutlinedIcon from '@mui/icons-material/CelebrationOutlined';
import { useEffect, useState } from "react";
import StyledButton from "../components/StyledButton";
import FavoriteBorderOutlinedIcon from '@mui/icons-material/FavoriteBorderOutlined';
import usePopups from './../hooks/usePopups';
import CreditsPopup from "../components/Popups/CreditsPopup";
import { APP_VERSION, APP_VERSION_RELEASE_DATE, IS_PRE_RELEASE } from "../constants";
import { useUserLogin, patchUserLlama } from "eam-commons-js";
import Confetti from "react-confetti";
import DeveloperSvg from "../components/Illustrations/DeveloperSvg";
import useStartupPopups from "../hooks/useStartupPopups";
import FeaturedPlayListOutlinedIcon from '@mui/icons-material/FeaturedPlayListOutlined';
import useDiscordRichPresence from './../hooks/useDiscordRichPresence';

function AboutPage() {
    const [showLlama, setShowLlama] = useState(false);
    const [windowSize, setWindowSize] = useState({
        width: window.innerWidth,
        height: window.innerHeight,
    });
    const theme = useTheme();
    const { showPopup } = usePopups();
    const { user, idToken } = useUserLogin();
    const { changelogPopups } = useStartupPopups();
    const { setState, resetState } = useDiscordRichPresence();

    useEffect(() => {
        const handleResize = () => {
            setWindowSize({
                width: window.innerWidth,
                height: window.innerHeight,
            });
        };

        window.addEventListener("resize", handleResize);
        return () => window.removeEventListener("resize", handleResize);
    }, []);

    useEffect(() => {
        const timeout = setTimeout(() => {
            setShowLlama(false);
        }, 5000);

        if (showLlama) {
            setState("Llama is dancing! 🦙");
        }

        if (showLlama &&
            user &&
            idToken
        ) {
            patchUserLlama(idToken);
        }

        return () => {
            clearTimeout(timeout);
        };
    }, [showLlama]);

    return (
        <Box sx={{ width: '100%', overflow: 'auto' }}>
            {
                showLlama &&
                <Confetti
                    width={windowSize.width}
                    height={windowSize.height}
                />
            }
            <ComponentBox
                title="About Exalt Account Manager"
                icon={<img src={theme.palette.mode === 'dark' ? '/logo/logo_inner.png' : '/logo/logo_inner_dark.png'} alt="EAM Logo" height='35.18px' width='35.18px' />}
                sx={{ userSelect: "none" }}
            >
                <Typography>
                    Exalt Account Manager was first released in mid 2020 as a simple tool to manage multiple accounts for the game Realm of the Mad God Exalt.
                    Since then it has grown to a fully fledged software with many features and an active user base.<br />
                </Typography>
                <Box
                    sx={{
                        display: 'flex',
                        justifyContent: 'center',
                        alignItems: 'center',
                        gap: 1,
                    }}
                >
                    <a href="https://exaltaccountmanager.com" target="_blank" rel="noreferrer">ExaltAccountManager.com</a>
                </Box>

            </ComponentBox>
            <Box sx={{ display: 'flex', flexDirection: 'row', mt: -2, mb: -2 }}>
                <ComponentBox
                    title={`Version ${APP_VERSION}${IS_PRE_RELEASE ? ' Preview' : ''}`}
                    icon={<NumbersOutlinedIcon />}
                    sx={{ mr: 0, userSelect: "none", flexGrow: 1 }}
                >
                    <Box
                        id="version-content"
                        sx={{
                            display: 'flex',
                            flexDirection: 'row',
                            gap: 1,
                            height: '100%',

                        }}
                    >
                        <Box
                            sx={{
                                display: 'flex',
                                flexDirection: 'column',
                                gap: 1,
                            }}
                        >
                            <Typography>
                                Released on {APP_VERSION_RELEASE_DATE}.
                            </Typography>
                            <StyledButton
                                startIcon={<FeaturedPlayListOutlinedIcon />}
                                onClick={() => {
                                    const lastChangelog = changelogPopups?.[changelogPopups.length - 1];
                                    if (lastChangelog) {
                                        setState("Viewing the changelog 📜");
                                        showPopup({
                                            ...lastChangelog,
                                            onClose: () => {
                                                resetState();
                                            },
                                        });
                                    }
                                }}
                            >
                                View Changelog
                            </StyledButton>
                        </Box>
                        <Box
                            sx={{
                                display: 'flex',
                                flexGrow: 1,
                                maxHeight: '78px',
                                mr: 0.5,
                                justifyContent: 'flex-end',
                            }}
                        >
                            <DeveloperSvg h='100%' />
                        </Box>
                    </Box>
                </ComponentBox>
                <ComponentBox
                    title="Developer"
                    icon={<CodeOutlinedIcon />}
                    sx={{ userSelect: "none" }}
                >
                    <Box sx={{ display: 'flex', flexDirection: 'column', justifyContent: 'center', alignItems: 'center', gap: 1 }}>
                        <Typography >
                            EAM is developed and maintained with passion by <a href="https://github.com/MaikEight" target="_blank" rel="noopener noreferrer">MaikEight</a>.
                        </Typography>
                        <Box>
                            {
                                showLlama &&
                                <img src="/logo/llama.gif" alt="Llama" height='40px' style={{ borderRadius: theme.shape.borderRadius }} />
                            }
                            <img src="/logo/Logo_NameOnly_2_Medium.jpg"
                                alt="MaikEight Logo"
                                height='40px'
                                width='150.56px'
                                style={{ borderRadius: theme.shape.borderRadius }}
                                onClick={() => { setShowLlama(true); }}
                            />

                            {
                                showLlama &&
                                <img src="/logo/llama.gif" alt="Llama" height='40px' style={{ borderRadius: theme.shape.borderRadius }} />
                            }
                        </Box>
                    </Box>

                </ComponentBox>
            </Box>
            <Box
                sx={{ display: 'flex', flexDirection: 'row', mt: -2, mb: -2 }}
            >
                <ComponentBox
                    title="Open Source"
                    icon={<CodeOutlinedIcon />}
                    sx={{ mr: 0, userSelect: "none", minWidth: '475px', flexGrow: 1 }}
                    innerSx={{
                        display: 'flex',
                        flexDirection: 'row',
                        gap: 1,
                        position: 'relative',
                        height: '100%',
                        justifyContent: 'space-between'
                    }}
                >
                    <Typography>
                        EAM is open source and available on <a href="https://github.com/MaikEight/ExaltAccountManager" target="_blank" rel="noopener noreferrer">GitHub</a>.
                        <br />Feel free to contribute!
                    </Typography>

                    <GitHubStars style={{ marginTop: '-16px' }} />
                </ComponentBox>
                <ComponentBox
                    title="Credits & Thanks"
                    icon={<FavoriteBorderOutlinedIcon />}
                    sx={{ userSelect: "none", flexGrow: 1 }}
                    innerSx={{
                        display: 'flex',
                        flexDirection: 'column',
                        gap: 1,
                        justifyContent: 'space-between',
                    }}
                >
                    <Typography>
                        Special thanks to every Framework, Library, Resource and Person that made this project possible.
                    </Typography>
                    <StyledButton
                        onClick={() => { 
                            setState("Viewing the credits 👥");
                            showPopup({ 
                                content: <CreditsPopup />,
                                onClose: () => { 
                                    resetState();
                                },
                             });
                        }}
                    >
                        Show Credits & Thanks
                    </StyledButton>
                </ComponentBox>
            </Box>
            <ComponentBox
                title="Want to support this project?"
                icon={<CelebrationOutlinedIcon />}
                sx={{ userSelect: "none" }}
            >
                <Typography>
                    If you like this project, consider supporting it by donating.
                    <br />Every donation is greatly appreciated!
                    <br />If you don't want to donate, consider starring the project on <a href="https://github.com/MaikEight/ExaltAccountManager" target="_blank" rel="noopener noreferrer">GitHub</a> and leaving some feedback via {<a href="https://discord.exalt-account-manager.eu" target="_blank" rel="noopener noreferrer">Discord</a>}.
                </Typography>
                <Box
                    sx={{
                        display: 'flex',
                        flexDirection: 'row',
                        gap: 1,
                        justifyContent: 'space-evenly',
                        alignItems: 'center',
                        mt: 3,
                    }}
                >
                    <a href="https://ko-fi.com/maik8" target="_blank" rel="noopener noreferrer">
                        <StyledButton>
                            <img src="/support/kofi.png" alt="Ko-fi Logo" height='30px' width='30px' heigth='30px' style={{ borderRadius: theme.shape.borderRadius, marginRight: '4px' }} />Support me on Ko-fi
                        </StyledButton>
                    </a>
                    <a href="https://www.buymeacoffee.com/maik8" target="_blank" rel="noopener noreferrer">
                        <StyledButton>
                            <img src="/support/bmc.svg" alt="buymeacoffee Logo" height='30px' width='20.73px' heigth='30px' style={{ borderRadius: theme.shape.borderRadius, marginRight: '6px' }} />Buy me a coffee
                        </StyledButton>
                    </a>
                </Box>
            </ComponentBox>
        </Box>
    );
}

export default AboutPage;
