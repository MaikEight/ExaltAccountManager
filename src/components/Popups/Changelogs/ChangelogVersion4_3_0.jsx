import { Box, Paper, Typography, useTheme } from '@mui/material';
import ChangelogEntry from './ChangelogEntry';
import { useEffect, useRef, useState } from 'react';
import Confetti from "react-confetti";

function ChangelogVersion4_3_0() {
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
        "Exalt Account Manager v4.3.0",
        "The first MacOS release is here with bug fixes and improvements!",
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
                    src='https://app-data.exaltaccountmanager.com/images/okta/apple_update2_512.png'
                    alt='EAM Logo'
                    height='171px'
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
                    title={'MacOS Support'}
                    listOfChanges={[
                        "After a lot of work, testing and adjustments, Exalt Account Manager is now officially supported on MacOS!",
                        "Please note that this is the first release for MacOS and there might still be some bugs or issues. If you encounter any problems, please report them on our Discord or GitHub.",
                        "Spread the word to your Mac-using friends so they can also enjoy the benefits of Exalt Account Manager!",
                        "Some features still need some more adjustments, like the settings are not MacOS native yet, but we are working on it!",
                    ]}
                />

                <ChangelogEntry
                    title={'Vault Peeker'}
                    listOfChanges={[
                        "Added pagination to the accounts",
                        "Improved performance and reduced memory usage when handling large numbers of accounts.",   
                        "Improved item canvas rendering performance.",
                    ]}
                />

                <ChangelogEntry
                    title={'Daily Login'}
                    listOfChanges={[
                        "Fixed an issue where the daily login process would never register as done and therefore never start on the next day.",
                        "Fixed an issue where the Background Manager did not stop properly for the daily login process to start.",                        
                    ]}
                />

                <ChangelogEntry
                    title={'Miscellaneous'}
                    listOfChanges={[
                        "Improved the logging of many background processes to better identify issues.",
                        "Improved sprite preloading to reduce performance impacts when starting EAM.",
                        "Improved the game updater: increases in logging and stability.",
                        "Fixed an issue where EAM would crash on startup for some users.",
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
                                <li>K3y0708</li>
                                <li>Rakya</li>
                                <li>and many more...</li>
                            </ul>
                        </Typography>
                    </Box>
                </Box>
            </Box>
        </Paper>
    );
}


export default ChangelogVersion4_3_0;