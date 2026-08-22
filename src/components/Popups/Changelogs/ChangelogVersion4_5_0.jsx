import { Box, Paper, Skeleton, Typography, useTheme } from '@mui/material';
import ChangelogEntry from './ChangelogEntry';
import { useState } from 'react';

function ChangelogVersion4_5_0() {
    const theme = useTheme();

    const [imageLoaded, setImageLoaded] = useState(false);

    const title = [
        "Exalt Account Manager v4.5.0",
        "Starting the Launcher, Character selection and premium daily logins are free now!",
    ];

    return (
        <Paper
            sx={{
                display: 'flex',
                flexDirection: 'column',
                alignItems: 'center',
                justifyContent: 'flex-start',
                borderRadius: `${theme.shape.borderRadius * 2}px`,
                border: `1px solid ${theme.palette.divider}`,
                width: '925px',
                maxHeight: '95vh',
                maxWidth: '90vw',
                overflow: 'auto',
                background: theme.palette.background.paper,
            }}
        >
            <Box
                sx={{
                    backgroundColor: theme.palette.background.default,
                }}
            >
                {
                    !imageLoaded && (
                        <Skeleton
                            variant="rounded"
                            sx={{
                                width: '100%',
                                height: '511.75px',
                                minHeight: '511.75px',
                                borderRadius: `0 0 ${theme.shape.borderRadius * 2}px ${theme.shape.borderRadius * 2}px`,
                                flexShrink: 0,
                                borderBottom: `1px solid ${theme.palette.divider}`,
                            }}
                        />
                    )
                }
                <img
                    src='https://app-data.exaltaccountmanager.com/images/okta/banner_4_5_0.webp'
                    alt='EAM blog post logo'
                    onLoad={() => setImageLoaded(true)}
                    style={{
                        display: imageLoaded ? 'block' : 'none',
                        width: '100%',
                        height: 'auto',
                        marginLeft: '-1px',
                        borderRadius: `${theme.shape.borderRadius * 2}px`,
                        borderTopRightRadius: 0,
                        borderBottom: `1px solid ${theme.palette.divider}`,
                    }}
                />
            </Box>
            <Box
                sx={{
                    position: 'sticky',
                    top: 0,
                    display: 'flex',
                    width: '100%',
                    borderRadius: `${theme.shape.borderRadius - 1}px`,
                    pt: 0,
                    pb: 1,
                    zIndex: 1,
                }}
            >
                {/* HEADLINE */}
                <Box
                    sx={{
                        px: 2,
                        py: 1,
                        display: 'flex',
                        flexDirection: 'column',
                        width: '100%',
                        alignItems: 'start',
                        justifyContent: 'center',
                        backgroundColor: theme.palette.background.default,
                        borderRadius: `0 0 ${theme.shape.borderRadius * 2}px ${theme.shape.borderRadius * 2}px`,
                        borderBottom: `1px solid ${theme.palette.divider}`,
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
                }}
            >
                <ChangelogEntry
                    title={'Game starting'}
                    listOfChanges={[
                        "Recent changes in Deca's policy made it mandatory to use the official launcher to start the game. EAM now supports starting the game through the official launcher.",
                        "You can now use the new dropdown menu at the right od the 'Start Game' button to choose the character you want to start the game with.",
                    ]}
                />
                <Box
                    sx={{
                        mt: -1,
                        ml: 1,
                    }}
                >
                    <Typography variant="subtitle1">
                        Read more about this change in the <a href="https://hub.realmofthemadgod.com/news0/news1/guardians" target="_blank" rel="noopener noreferrer">official blogpost</a>
                    </Typography>
                </Box>

                <ChangelogEntry
                    title={'Daily Logins'}
                    listOfChanges={[
                        "The daily login has also changed to not start the game anymore, meaning that the EAM Plus variant is now also the way used by the free version.",
                        "🕑 The Plus variant is faster than the free version."
                    ]}
                />

                <ChangelogEntry
                    title={'Daily Login Rewards'}
                    listOfChanges={[
                        "The current months daily login rewards are now displayed on the daily login page with a small checkmark on the days the daily login ran.",
                        "A new Daily Login Rewards Widget has been added, it shows the current months daily login reward state of an account (claimed / unclaimed)."
                    ]}
                />

                <ChangelogEntry
                    title={'Miscellaneous'}
                    listOfChanges={[
                    ]}
                />

                <Box
                    sx={{
                        display: 'flex',
                        flexDirection: 'column',
                        width: 'calc(100% + 16px)',
                        alignItems: 'center',
                        justifyContent: 'center',
                        background: theme.palette.background.default,
                        borderRadius: `${(theme.shape.borderRadius * 2) - 2}px`,
                        border: `1px solid ${theme.palette.divider}`,
                        pt: 1,
                        mx: -1,
                        mb: -1,
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
                                <li><span style={{ color: '#f50' }}>DECA</span> Tiramisu</li>
                                <Typography component={'span'} variant="body2" fontWeight={'bold'} color="textSecondary">
                                    Thank you for actively helping EAM to stay alive!
                                </Typography>
                            </ul>
                        </Typography>
                        <Box
                            sx={{
                                my: 'auto'
                            }}
                        >
                            <img
                                src="/mascot/Happy/happy_very_low_res.png"
                                alt="Okta"
                                style={{ width: '56px', height: '56px' }}
                            />
                        </Box>
                    </Box>
                </Box>
            </Box>
        </Paper>
    );
}


export default ChangelogVersion4_5_0;