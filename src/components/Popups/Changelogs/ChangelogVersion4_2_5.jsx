import { Box, Paper, Typography, useTheme } from '@mui/material';
import ChangelogEntry from './ChangelogEntry';
import { Fragment, useEffect, useRef, useState } from 'react';
import Confetti from "react-confetti";

function ChangelogVersion4_2_5() {
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
        "Exalt Account Manager v4.2.5",
        "Okta, Daily Login integration, Background Sync, bug fixes and more improvements!",
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
                    borderRadius: `${theme.shape.borderRadius * 2 - 1}px`,
                    backgroundColor: theme.palette.background.default,
                    p: 2,
                    py: 4,
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
                    src='/mascot/Happy/happy_eam_low_res.png'
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
                        ℹ️ This is the changelog since the last full release: EAM v4.2.0.
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
                    title={(<>
                        Meet Okta
                        <img
                            src="/mascot/okta_super_low_res.png"
                            alt="EAM Mascot"
                            height='20px'
                            style={{
                                marginBottom: '-4px',
                                marginLeft: '4px',
                            }}
                        />
                    </>
                    )}
                    listOfChanges={[
                        (<Fragment key="Meet Okta #1">Okta is the new mascot of EAM, he is a cute little octopus who will <s>annoy</s> accompany you on your journey through EAM.</Fragment>)
                    ]}
                />

                <ChangelogEntry
                    title={'Daily Login'}
                    listOfChanges={[
                        "The Daily login feature has been integrated into the main EAM app.",
                        "It is now also possible to minimzize the app into the system tray and EAM will do so when starting for the Daily Login.",
                        'Fixed the columns "🕛 End Time" and "⏱️ Duration" to behave correctly while the Daily Login task is running.',
                        (<Fragment key="Daily Login #3">⚠️ Please <b>check</b> if the Daily Login <b>task is installed (updated) correctly</b> by going to the Daily Login settings, if you see a <b>red box, please reinstall</b> the task.</Fragment>),
                    ]}
                />

                <ChangelogEntry
                    title={'Background Sync'}
                    listOfChanges={[
                        "This feature allows EAM to run in the background (task tray) and automatically sync accounts.",
                        "Since EAM does not require much resources (nearly no CPU, ~15MB RAM), it can run in the background without noticeable performance impact.",
                        "To minimize the gameplay impact, the sync is only using a small amount of the available API-Limit and is disabled during the Daily Login.",
                    ]}
                />

                <ChangelogEntry
                    title={'Vault Peeker'}
                    listOfChanges={[
                        "Added a characters group overview in the Account title.",
                        "Added a transition effect to the Item Viewer Popup.",
                        "Added support for hiding vault types.",
                        "Added the first fame bonus feature: Dungeon bonuses. [BETA]",
                        "Improved caching of character portraits and item images.",
                        "Improved the overall loading speed.",
                        (<Fragment key="Vault Peeker #6">Updated the item images and item data. <b>Thanks to 059</b> for providing them!!!</Fragment>),
                        "Fixed an issue where filters were not getting re-applied after refreshing an account.",
                    ]}
                />

                <ChangelogEntry
                    title={'Accounts'}
                    listOfChanges={[
                        "Added the ability to order accounts in the DataGrid by selecting it and using the new up/down buttons.",
                        "Added emojis to the column headers.",
                        "Added a new Comments section to the Account Details view.",
                        "Comments can also be displayed in the Accounts List.",
                        "Changed the exp datatype from int to bigint to support larger values. Thanks @m",
                    ]}
                />

                <ChangelogEntry
                    title={'Settings'}
                    listOfChanges={[
                        "Added the new comments column to the settings (default columns).",
                        "Added the ability to hide emojis in the account grid.",
                        "Added a new 'Autostart & Close behavior' section.",
                        "Added a 'Privacy & Legal' section.",
                        "Added the ability to request and delete your data in compliance with GDPR.",
                        "Improved the settings page.",
                    ]}
                />

                <ChangelogEntry
                    title={'Miscellaneous'}
                    listOfChanges={[
                        "Added new illustrations at some places. Thanks to undraw.co. ✨",
                        "Added support for importing Muledump account export file (accounts.js).",
                        "Added a new 'Fatal Error' page to display when the application crashes.",
                        "Added a new button to the Abount page to open the latest changelog.",
                        "Added task tray support (new default) on closing the application.",
                        "Improved the changelog popup to be more readable and visually appealing going forward.",
                        "Improved the logging of database operations in the log file.",
                        "Improved the updater to not require admin privileges anymore.",
                        "Improved error message when refreshing of an account / logging in fails.",
                        "Fixed an issue where the Group UI was not displayed correctly in the Account Grid on first opening after start.",
                        "Fixed an issue where some equipment items where not drawn correctly.",
                        "Fixed an issue where the server in the account details did not update correctly when switching accounts.",
                        "Fixed an issue with the exp datatype for high-fame characters. Thanks @m.",
                        "Fixed a bug where the Last Login value was not updated correctly or at all.",
                        "Fixed the account name fetching not working.",
                        "Fixed the Thanks & Credits popup width.",
                        "Fixed an issue where the char/list conversion would fail if the account had no character and no classes unlocked.",
                        "Fixed spelling mistakes.",
                    ]}
                />

                <Box
                    sx={{
                        display: 'flex',
                        flexDirection: 'column',
                        alignItems: 'center',
                        width: '100%',
                        gap: 2,
                        my: 1
                    }}
                >
                    <Box>
                        <Typography variant="subtitle1" color="primary">
                            Special thanks to all beta testers, helpers and everyone who provided feedback!
                        </Typography>
                        <Box
                            sx={{
                                display: 'flex',
                                flexDirection: 'row',
                                justifyContent: 'start',
                                gap: 1,
                                mt: -0.5
                            }}
                        >
                            <Typography component={'span'} variant="body2" fontWeight={'bold'} color="textSecondary">
                                <ul>
                                    <li>Nick - For his first contribution to EAM 🎉</li>
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
                                </ul>
                            </Typography>
                            <Typography component={'span'} variant="body2" fontWeight={'bold'} color="textSecondary">
                                <ul>
                                    <li>TheDangerScrew</li>
                                    <li>Pro90</li>
                                    <li>n1k-o</li>
                                </ul>
                            </Typography>
                            <Typography component={'span'} variant="body2" fontWeight={'bold'} color="textSecondary">
                                <ul>
                                    <li>m</li>
                                    <li>and many more...</li>
                                </ul>
                            </Typography>
                        </Box>
                    </Box>
                </Box>
            </Box>
        </Paper>
    );
}


export default ChangelogVersion4_2_5;