import { Box, Paper, Typography, useTheme } from '@mui/material';
import ChangelogEntry from './ChangelogEntry';
import { useEffect, useRef, useState } from 'react';
import Confetti from "react-confetti";

function ChangelogVersion4_2_7() {
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
        "Exalt Account Manager v4.2.7",
        "Daily Login hotfix, new Settings, Popups and more!",
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
                {
                    <Confetti
                        width={windowSize?.width || 0}
                        height={windowSize?.height || 0}
                        style={{
                            borderRadius: `${theme.shape.borderRadius * 2 - 1}px`,
                            opacity: windowSize.visible ? 1 : 0,
                            transition: 'opacity 1s ease-in-out',
                        }}
                    />
                }
                <img
                    src='https://app-data.exaltaccountmanager.com/images/eam/logo/EAM_logo_stroke.png'
                    alt='Okta is happy!'
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
                    src='/mascot/Happy/happy_very_low_res.png'
                    alt='Okta holding the EAM Logo'
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
                    <Typography variant="body2" color="textSecondary">
                        ℹ️ This is the changelog since the last full release: EAM v4.2.5.
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
                    title={'Daily Login'}
                    listOfChanges={[
                        "Fixed an issue where the Daily Login task would stop working after 20-40 accounts and never finish / fail on every following account.",
                        "Removed the 'Start Daily Login' button from the Daily Login page, since it is now automatically started when EAM starts. (Will be back soon)",
                    ]}
                />

                <ChangelogEntry
                    title={'Accounts'}
                    listOfChanges={[
                        "Added a notification when starting the game, telling users about a fix regarding the token for different machine error.",
                        "Changing the columns on the accounts tab now saves the current state to the settings (Stays persistent)."
                    ]}
                />

                <ChangelogEntry
                    title={'Settings'}
                    listOfChanges={[
                        "Added a setting to disable the Background Sync process.",
                        "Added a setting to close EAM after the Daily Login finishes.",
                        "Added a setting to disable the auto-hide on Daily Login startup.",
                        "Added a setting to hide EAM when started via the autostart.",
                        "Added a setting to disable the Discord Rich Presence.",
                        "Improved the Analytics opt-out mechanism by showing Okta for feedback."
                    ]}
                />

                <ChangelogEntry
                    title={'News & Popups'}
                    listOfChanges={[
                        "Added a new server-side popup and news system.",
                        "Added the popup system into EAM to display important announcements when needed."
                    ]}
                />

                <ChangelogEntry
                    title={'Miscellaneous'}
                    listOfChanges={[
                        "Stabilized every database operation by integrating a retry mechanism and enabling WAL mode.",
                        "Added emoji support to the log table.",
                        "Improved the Credits & Thanks section.",
                        "Fixed an issue where the game updater would not work correctly when no game exe path was specified in the settings just yet.",
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
                                <li>
                                    TheMelonPerson - For the cute new mascot
                                    <img
                                        src="/mascot/okta_super_low_res.png"
                                        alt="EAM Mascot"
                                        height='20px'
                                        style={{
                                            marginBottom: '-4px',
                                            marginLeft: '2px',
                                        }}
                                    />
                                </li>
                                <li>059 - For providing updated item renders and constants.</li>
                                <li>Pro90</li>
                            </ul>
                        </Typography>
                        <Typography component={'span'} variant="body2" fontWeight={'bold'} color="textSecondary">
                            <ul>
                                <li>TheDangerScrew</li>
                                <li>n1k-o</li>
                            </ul>
                        </Typography>
                        <Typography component={'span'} variant="body2" fontWeight={'bold'} color="textSecondary">
                            <ul>
                                <li>ikke</li>
                                <li>Riot Phreak</li>
                                <li>and many more...</li>
                            </ul>
                        </Typography>
                    </Box>
                </Box>
            </Box>
        </Paper>
    );
}


export default ChangelogVersion4_2_7;