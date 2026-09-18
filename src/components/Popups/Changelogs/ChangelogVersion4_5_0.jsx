import { Box, Paper, Skeleton, Typography, useTheme } from '@mui/material';
import ChangelogEntry from './ChangelogEntry';
import { useState } from 'react';
import { ExternalLink } from '../CreditsPopup';
import { MASCOT_NAME } from '../../../constants';

function ChangelogVersion4_5_0() {
    const theme = useTheme();

    const [imageLoaded, setImageLoaded] = useState(false);

    const title = [
        "Exalt Account Manager v4.5.0",
        "Starting the launcher, character selection and premium daily logins are now free!",
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
                        "Recent changes in DECA's policy made it mandatory to use the official launcher to start the game. EAM now supports starting the game through the official launcher.",
                        "You can now use the new dropdown menu to the right of the 'Start Game' button to choose the character you want to start the game with.",
                    ]}
                />
                <Box
                    sx={{
                        mt: -1,
                        ml: 1,
                        display: 'flex',
                        flexDirection: 'row',
                        alignItems: 'start',
                        justifyContent: 'start',
                        gap: 0.5,
                    }}
                >
                    <Typography variant="subtitle1">
                        Read more about this change in the
                    </Typography>
                    <Box
                        sx={{ mt: 0.25 }}
                    >
                        <ExternalLink url="https://hub.realmofthemadgod.com/news0/news1/guardians" title="official blogpost" />
                    </Box>
                    <Typography variant="subtitle1">
                        from june 2026.
                    </Typography>
                </Box>
                <Box
                    sx={{
                        p: 1,
                        pl: 2,
                        display: 'flex',
                        flexDirection: 'row',
                        width: '100%',
                        alignItems: 'start',
                        justifyContent: 'center',
                        borderRadius: `${theme.shape.borderRadius * 0.5}px ${theme.shape.borderRadius}px ${theme.shape.borderRadius}px ${theme.shape.borderRadius * 0.5}px`,
                        borderLeft: `3px solid ${theme.palette.primary.main}`,
                        backgroundColor: theme.palette.background.default,
                        gap: 1.5,
                    }}
                >
                    <img
                        src="/mascot/Info/notification_very_low_res.png"
                        alt="Error Mascot"
                        style={{ height: 'auto', width: '39px', marginTop: '-2px',  }}
                    />
                    <Box
                        sx={{
                            display: 'flex',
                            flexDirection: 'column',
                            width: '100%',
                            alignItems: 'start',
                            justifyContent: 'center',
                            gap: 1,
                        }}
                    >
                        <Typography variant="body1">
                            DECA reviewed a pre-release build of <b>v4.5.0</b>. They don't endorse EAM — they don't
                            endorse any third-party software — but they don't forbid EAM either.
                        </Typography>
                        <Typography variant="body1">
                            In an official statement, DECA's community manager <b>Tiramisu</b> confirmed that DECA
                            will <b>not</b> take action against accounts for using EAM, as long as EAM stays free of
                            cheats and doesn't bypass the Terms of Service.
                        </Typography>
                        <Typography variant="body1">
                            This reflects DECA's current position. We'll keep monitoring the situation and let you
                            know if anything changes.
                        </Typography>
                    </Box>
                </Box>

                <ChangelogEntry
                    title={'Game asset updater'}
                    listOfChanges={[
                        "Game assets are now updated at runtime, meaning EAM automatically updates them when they are outdated.",
                    ]}
                />
                <Box
                    sx={{
                        mt: -1,
                        ml: 1,
                        display: 'flex',
                        flexDirection: 'row',
                        alignItems: 'start',
                        justifyContent: 'start',
                        gap: 0.5,
                    }}
                >
                    <Typography variant="subtitle1">
                        This <b>big</b> improvement is only possible thanks to our new contributor and <b>long</b>-time EAM supporter
                    </Typography>
                    <Box
                        sx={{ mt: 0.25 }}
                    >
                        <ExternalLink url="https://github.com/TadusPro" title="Tadus" image="https://avatars.githubusercontent.com/u/22742194?v=4" />
                    </Box>
                </Box>

                <ChangelogEntry
                    title={'Daily Logins'}
                    listOfChanges={[
                        "The daily login no longer starts the game, which means the free version now uses the same method as EAM Plus.",
                        "🕑 The Plus variant is still a bit faster than the free version."
                    ]}
                />

                <ChangelogEntry
                    title={'Daily Login Rewards'}
                    listOfChanges={[
                        "The current month's daily login rewards are now displayed on the daily login page.",
                        "A new Daily Login Rewards widget has been added; it shows an account's daily login reward state for the current month."
                    ]}
                />

                <ChangelogEntry
                    title={'Miscellaneous'}
                    listOfChanges={[
                        "Accounts that currently have the game running are now marked in the accounts table.",
                        "Updated all dependencies to their latest versions to stay as secure as possible.",
                        "Updated the Credits & Thanks popup.",
                        "Fixed the Game Updater's loading state not being displayed correctly.",
                        "A new code signing certificate has been issued for EAM. This is a security measure to ensure that the application is not tampered with.",
                    ]}
                />

                <Box>
                    <Typography variant="h6" color="primary">
                        Privacy Policy and Terms of Service Update
                    </Typography>
                    <Box
                        sx={{
                            display: 'flex',
                            flexDirection: 'row',
                            alignItems: 'start',
                            justifyContent: 'start',
                            width: '100%',
                            gap: 2,
                        }}
                    >
                        <img
                            src="https://exaltaccountmanager.com/okta/variants/sleepy_320.png"
                            alt="Sleepy Variant"
                            style={{ width: 'auto', height: '100px' }}
                        />
                        <Box
                            sx={{
                                mt: 2,
                            }}
                        >
                            <Typography variant="body1" align="start" color="textSecondary">
                                Please read them carefully ... or not. {MASCOT_NAME} does not judge you for that.
                            </Typography>
                            <Typography variant="subtitle2" align="start" color="textSecondary">
                                But we would like to inform you that by using EAM, you agree to our Privacy Policy and Terms of Service.
                            </Typography>
                            <Box
                                sx={{
                                    display: 'flex',
                                    flexDirection: 'row',
                                    justifyContent: 'start',
                                    gap: 4,
                                    mt: 1,
                                }}
                            >
                                <ExternalLink url="https://exaltaccountmanager.com/privacy-policy" title="Privacy Policy" />
                                <ExternalLink url="https://exaltaccountmanager.com/terms-of-service" title="Terms of Service" />
                            </Box>
                        </Box>
                    </Box>
                </Box>

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
                                <li>K3y0708</li>
                            </ul>
                        </Typography>
                        <Typography component={'span'} variant="body2" fontWeight={'bold'} color="textSecondary">
                            <ul>
                                <li><span style={{ color: theme.palette.primary.main }}>Contributor</span> Tadus</li>
                                <Typography component={'span'} variant="body2" fontWeight={'bold'} color="textSecondary">
                                    Thank you for your contribution to EAM!
                                </Typography>
                                <li><span style={{ color: '#f50' }}>DECA</span> Tiramisu</li>
                                <Typography component={'span'} variant="body2" fontWeight={'bold'} color="textSecondary">
                                    Thank you for actively helping EAM stay alive!
                                </Typography>
                            </ul>
                        </Typography>
                        <Box
                            sx={{
                                my: 'auto',
                                ml: 0.25,
                                mr: -0.25
                            }}
                        >
                            <img
                                src="/mascot/Happy/happy_very_low_res.png"
                                alt="Okta"
                                style={{ width: '80px', height: '80px' }}
                            />
                        </Box>
                    </Box>
                </Box>
            </Box>
        </Paper>
    );
}


export default ChangelogVersion4_5_0;