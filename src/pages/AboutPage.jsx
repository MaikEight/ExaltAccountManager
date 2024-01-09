import { Box, Typography } from "@mui/material";
import ComponentBox from './../components/ComponentBox';
import NumbersOutlinedIcon from '@mui/icons-material/NumbersOutlined';
import CodeOutlinedIcon from '@mui/icons-material/CodeOutlined';

function AboutPage() {

    return (
        <Box sx={{ width: '100%', overflow: 'auto' }}>
            <ComponentBox
                headline="About Exalt Account Manager"
                icon={<img src="/logo/logo_inner.png" alt="EAM Logo" width='30px' />}
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
                    sx={{ mr: 0, userSelect: "none" }}
                >
                    <Typography>
                        EAM V4.0.0
                        <br />Released 20.01.2024
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

            </ComponentBox>

        </Box>
    );
}

export default AboutPage;