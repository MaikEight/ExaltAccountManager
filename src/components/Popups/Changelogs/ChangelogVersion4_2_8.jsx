import { Box, Paper, Typography, useTheme } from '@mui/material';
import ChangelogEntry from './ChangelogEntry';
import { Fragment, useEffect, useRef, useState } from 'react';

function ChangelogVersion4_2_8() {
    const theme = useTheme();
    const boxHeaderRef = useRef(null);

    const [windowSize, setWindowSize] = useState({
        width: 0,
        height: 0,
        visible: true,
    });

    useEffect(() => {
        if (!boxHeaderRef.current) {
            return;
        }
        setWindowSize({
            width: boxHeaderRef.current?.offsetWidth || 0,
            height: boxHeaderRef.current?.offsetHeight || 0,
            visible: windowSize.visible,
        });
    }, [boxHeaderRef]);

    useEffect(() => {
        const timeoutId = setTimeout(() => {
            setWindowSize((prev) => ({
                ...prev,
                visible: false,
            }));
        }, 7_500);
        return () => clearTimeout(timeoutId);
    }, []);

    const title = [
        "Exalt Account Manager v4.2.8",
        "Login Hotfix and preparations...",
    ];

    return (
        <Paper
            sx={{
                display: 'flex',
                flexDirection: 'column',
                alignItems: 'center',
                justifyContent: 'center',
                borderRadius: `${theme.shape.borderRadius * 2}px`,
                width: '925px',
                maxHeight: '95vh',
                maxWidth: '90wh',
                overflow: 'hidden',
            }}
        >
            <Box
                ref={boxHeaderRef}
                sx={{
                    position: 'relative',
                    display: 'flex',
                    flexDirection: 'column',
                    alignItems: 'center',
                    justifyContent: 'center',
                    width: '100%',
                    height: '224px',
                    borderRadius: `${theme.shape.borderRadius * 2 - 1}px`,
                    backgroundColor: theme.palette.background.default,
                    p: 2,
                    py: 4,
                    overflow: "hidden",
                }}
            >
                <img
                    src='https://app-data.exaltaccountmanager.com/images/eam/logo/EAM_logo_stroke.png'
                    alt='EAM Logo'
                    style={{
                        position: "absolute",
                        height: "250%",
                        bottom: "50%",
                        left: "50%",
                        transform: "translate(-50%, 50%) rotate(20deg)",
                        opacity: 0.25,
                        mask: "linear-gradient(to bottom, transparent 0%, black 20%, black 20%, transparent 100%)",
                        WebkitMask: "linear-gradient(to bottom, transparent 0%, black 20%, black 20%, transparent 100%)"
                    }}
                />
                <img
                    src='https://app-data.exaltaccountmanager.com/images/okta/error_network_180.png'
                    alt='EAM Logo'
                    height='128px'
                    style={{
                        zIndex: 1001,
                    }}
                />
                <img
                    src='/mascot/floor.png'
                    alt='EAM Mascot Floor'
                    width='196px'
                    style={{
                        position: 'absolute',
                        bottom: 'calc(50% - 48px)',
                        left: '50%',
                        transform: 'translate(-50%, 50%)',
                        zIndex: 1000,
                    }}
                />
            </Box>
            <Box
                sx={{
                    display: 'flex',
                    width: '100%',
                    borderRadius: `${theme.shape.borderRadius - 1}px`,
                    pt: 2,
                    px: 2,
                    pb: 1,
                }}
            >
                {/* HEADLINE */}
                <Box
                    sx={{
                        display: 'flex',
                        flexDirection: 'column',
                        width: '100%',
                        alignItems: 'start',
                        justifyContent: 'center',
                    }}
                >
                    <Typography variant="h6" component="h2" fontWeight="bold" color={theme.palette.primary.main}>
                        {title[0]}
                    </Typography>
                    <Typography variant="subtitle1" color="textSecondary">
                        {title[1]}
                    </Typography>
                    <Typography variant="subtitle2" color="textSecondary">
                        Sorry for the inconvenience, a game update broke the login for some users.
                    </Typography>
                </Box>
            </Box>
            {/* CONTENT */}
            <Box
                sx={{
                    display: 'flex',
                    flexDirection: 'column',
                    width: '100%',
                    p: 2,
                    gap: 2,
                    overflowY: 'auto',
                }}
            >
                <ChangelogEntry
                    title={'Login'}
                    listOfChanges={[
                        "Fixed an issue where the game would crash immediately after starting.",
                    ]}
                />

                <ChangelogEntry
                    title={'Miscellaneous'}
                    listOfChanges={[
                        "Added the first implementation of the new char/list parser (still under development, not in use).",
                    ]}
                />

                <Box
                    sx={{
                        display: 'flex',
                        flexDirection: 'column',
                        width: '100%',
                        alignItems: 'center',
                        justifyContent: 'center',
                    }}
                >
                    <Typography variant="subtitle1" color="primary">
                        Special thanks to all beta testers, helpers and everyone who provided feedback!
                    </Typography>
                    <Box
                        sx={{
                            display: 'flex',
                            flexDirection: 'row',
                            justifyContent: 'start',
                            gap: 1,
                            mt: -0.5,
                            mr: 3,
                        }}
                    >
                        <Typography component={'span'} variant="body2" fontWeight={'bold'} color="textSecondary">
                            <ul>
                                <li>Pro90</li>
                                <li>TheDangerScrew</li>
                                <li>n1k-o</li>
                            </ul>
                        </Typography>
                        <Typography component={'span'} variant="body2" fontWeight={'bold'} color="textSecondary">
                            <ul>
                                <li>TadusPro</li>
                                <li>icanplat</li>
                                <li>and many more...</li>
                            </ul>
                        </Typography>
                    </Box>
                </Box>
            </Box>
        </Paper>
    );
}


export default ChangelogVersion4_2_8;