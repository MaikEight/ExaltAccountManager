import { Box, Typography } from "@mui/material";
import { useEffect } from "react";
import { useErrorBoundary } from "react-error-boundary";
import { useLocation } from "react-router-dom";
import ComponentBox from "./ComponentBox";
import WarningAmberOutlinedIcon from '@mui/icons-material/WarningAmberOutlined';
import SupportAgentOutlinedIcon from '@mui/icons-material/SupportAgentOutlined';
import { useTheme } from "@emotion/react";
import QaEngineerSvg from "./Illustrations/QaEngineerSvg";

function ErrorBoundaryFallback() {
    const autoFixLocations = sessionStorage.getItem('autoFixTried');
    const { resetBoundary } = useErrorBoundary();
    const location = useLocation();
    const theme = useTheme();

    useEffect(() => {
        const locs = autoFixLocations ? autoFixLocations.split(',') : [];
        if (locs.includes(location.pathname)) {
            return;
        }
        locs.push(location.pathname);
        sessionStorage.setItem('autoFixTried', locs.join(','));
        const _lastTry = sessionStorage.getItem('lastAutoFixTried');
        if (_lastTry) {
            const lastTry = new Date(_lastTry);
            const now = new Date();
            const diff = now - lastTry;
            if (diff < 3000) { // 5 seconds
                return () => sessionStorage.removeItem('autoFixTried');
            }
        }
        console.log('ErrorBoundaryFallback', location);
        sessionStorage.setItem('lastAutoFixTried', new Date().toISOString());
        resetBoundary();

        return () => {
            console.log('ErrorBoundaryFallback cleanup', location);
            sessionStorage.removeItem('autoFixTried');
        }
    }, [location]);

    return (
        <Box
            sx={{
                display: 'flex',
                flexDirection: 'column',
                height: '100%',
                width: '100%',
                mt: 2,
                gap: 2,
            }}
            innerSx={{
                maxHeight: '100%',
                overflowY: 'auto',
            }}
        >
            <ComponentBox
                title={"A critical error occurred"}
                icon={<WarningAmberOutlinedIcon />}
                sx={{
                    my: 0,
                    color: 'error.main',
                }}
                innerSx={{
                    color: 'text.primary',
                }}
            >
                {
                    <Box>
                        <Typography variant='body1'>
                            Please try to refresh the page by pressing <b>CTRL + R</b> or <b>F5</b>. If the error persists, please contact support.
                        </Typography>
                        <Typography variant='body1'>
                            If the problem still persists, please change the page on the sidebar and try again.
                        </Typography>
                    </Box>
                }
            </ComponentBox>
            <ComponentBox
                title="Contact Support"
                icon={<SupportAgentOutlinedIcon />}
                sx={{
                    my: 0,
                }}
            >
                <Typography variant='body1'>
                    If you can't solve the problem, here are some ways to contact support:
                </Typography>

                <ul>
                    <li>

                        <Typography variant='body1' component={'span'}>
                            <a href="https://discord.exalt-account-manager.eu/" target="_blank" rel="noopener noreferrer">
                                <span
                                    style={{
                                        color: theme.palette.primary.main,
                                        textDecoration: 'underline',
                                        paddingRight: '0.3rem'
                                    }}
                                >
                                    Join our Discord server
                                </span>
                            </a>
                            and ask for help in the support-center or help-request channel.
                        </Typography>
                    </li>
                    <li>
                        <Typography variant='body1' component={'span'}>
                            Send an email to <a style={{ cursor: 'pointer', color: theme.palette.primary.main }} href="mailto:mail@maik8.de" rel="noopener noreferrer">mail@maik8.de</a>
                        </Typography>
                    </li>
                </ul>
            </ComponentBox>
            <Box
                sx={{
                    width: '100%',
                    maxWidth: '100%',
                    height: '100%',
                    display: 'flex',
                    justifyContent: 'center',
                    alignItems: 'center',
                }}
            >
                <Box
                    sx={{
                        width: '100%',
                        maxWidth: '400px',
                        height: '100%',
                    }}
                >
                    <QaEngineerSvg h={'100%'} w={'100%'} />
                </Box>
            </Box>
        </Box>
    );
}

export default ErrorBoundaryFallback;