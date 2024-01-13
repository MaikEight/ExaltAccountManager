import { Box, Button, Typography } from "@mui/material";
import ComponentBox from './../components/ComponentBox';
import NumbersOutlinedIcon from '@mui/icons-material/NumbersOutlined';
import CodeOutlinedIcon from '@mui/icons-material/CodeOutlined';
import { useTheme } from "@emotion/react";
import GitHubStars from "../components/GitHubStars";
import CelebrationOutlinedIcon from '@mui/icons-material/CelebrationOutlined';
import { useEffect } from "react";
import StyledButton from "../components/StyledButton";

function AboutPage() {
    const theme = useTheme();

    return (
        <Box sx={{ width: '100%', overflow: 'auto' }}>
            <ComponentBox
                headline="About Exalt Account Manager"
                icon={<img src={theme.palette.mode === 'dark' ? '/logo/logo_inner.png' : '/logo/logo_inner_dark.png'} alt="EAM Logo" height='35.18px' />}
                sx={{ userSelect: "none" }}
            >
                <Typography >
                    Exalt Account Manager was first released in mid 2020 as a simple tool to manage multiple accounts for the game Realm of the Mad God Exalt.
                    Since then it has grown to a fully fledged software with many features and an active user base.
                </Typography>
            </ComponentBox>
            <Box sx={{ display: 'flex', flexDirection: 'row', mt: -2, mb: -2 }}>
                <ComponentBox
                    headline="Version"
                    icon={<NumbersOutlinedIcon />}
                    sx={{ mr: 0, userSelect: "none", flexGrow: 1 }}
                >
                    <Typography>
                        Exalt Account Manager version 4.0.0 Beta
                        <br />Released on 20.01.2024
                        <br />This build is for testing purposes only.
                    </Typography>
                </ComponentBox>
                <ComponentBox
                    headline="Developer"
                    icon={<CodeOutlinedIcon />}
                    sx={{ userSelect: "none" }}
                >
                    <Box sx={{ display: 'flex', flexDirection: 'column', justifyContent: 'center', alignItems: 'center', gap: 1 }}>
                        <Typography >
                            EAM is developed and maintained with passion by <a href="https://github.com/MaikEight" target="_blank" rel="noopener noreferrer">MaikEight</a>.
                        </Typography>
                        <a href="https://github.com/MaikEight" target="_blank" rel="noopener noreferrer">
                            <img src="/logo/Logo_NameOnly_2_Medium.jpg" alt="Github Logo" height='40px' style={{ borderRadius: '6px' }} />
                        </a>
                    </Box>

                </ComponentBox>
            </Box>
            <ComponentBox
                headline="Open Source"
                icon={<CodeOutlinedIcon />}
                sx={{ userSelect: "none" }}
            >
                <Typography>
                    EAM is open source and available on <a href="https://github.com/MaikEight/ExaltAccountManager" target="_blank" rel="noopener noreferrer">GitHub</a>.
                    <br />Feel free to contribute!
                </Typography>

                <GitHubStars style={{ position: 'absolute', top: '25%', right: 24 }} />
            </ComponentBox>
            <ComponentBox
                headline="Want to support this project?"
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
                            <img src="/support/kofi.png" alt="Ko-fi Logo" height='30px' width='30px' style={{ borderRadius: '6px' }} />Support me on Ko-fi
                        </StyledButton>
                    </a>
                    <a href="https://www.buymeacoffee.com/maik8" target="_blank" rel="noopener noreferrer">
                        <StyledButton>
                            <img src="/support/bmc.svg" alt="buymeacoffee Logo" height='30px' width='20.73px' style={{ borderRadius: '6px' }} />Buy me a coffee
                        </StyledButton>
                    </a>
                </Box>
            </ComponentBox>
        </Box>
    );
}

export default AboutPage;
