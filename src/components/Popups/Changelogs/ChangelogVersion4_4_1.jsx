import { Box, Paper, Skeleton, Typography, useTheme } from '@mui/material';
import ChangelogEntry from './ChangelogEntry';
import { useState } from 'react';

function ChangelogVersion4_4_1() {
    const theme = useTheme();

    const [imageLoaded, setImageLoaded] = useState(false);

    const hotfixTitle = [
        "Exalt Account Manager v4.4.1",
        "Hotfix for adding accounts and purchasing EAM Plus.",
    ]

    const title = [
        "Exalt Account Manager v4.4.0",
        "New Logo, Widgets, Enchantments, Stats and Vault Peeker redesign!",
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
                    src='https://app-data.exaltaccountmanager.com/images/okta/eam_logo_blog_post.png'
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
                    position: 'top',
                    top: 0,
                    display: 'flex',
                    width: '100%',
                    borderRadius: `${theme.shape.borderRadius - 1}px`,
                    pt: 0,
                    pb: 1,
                    zIndex: 1,
                }}
            >
                {/* HEADLINE HOTFIX */}
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
                        {hotfixTitle[0]}
                    </Typography>
                    <Typography variant="subtitle1" color="textSecondary">
                        {hotfixTitle[1]}
                    </Typography>
                </Box>
            </Box>
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
                    title={'Bug Fixes'}
                    listOfChanges={[
                        "Fixed the not working adding of accounts.",
                        "Fixed the not working EAM Plus checkout."
                    ]}
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
                        // borderRadius: `0 0 ${theme.shape.borderRadius * 2}px ${theme.shape.borderRadius * 2}px`,
                        borderRadius: `${theme.shape.borderRadius * 2}px`,
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
                    title={'Widgets'}
                    listOfChanges={[
                        "Added a new Widgets system to customize EAM to your liking.",
                        "Account Details are now using the new Widgets system, add / remove them as you like.",
                        "You can enable Edit Mode for Widget Bars to add, remove or rearrange Widgets. Just click the 'Edit' button in the top right of a Widget Bar.",
                        "Some widgets have its own settings, accessible via the edit icon in the top right of the Widget itself.",
                    ]}
                />

                <ChangelogEntry
                    title={'Stats and Fame Bonuses'}
                    listOfChanges={[
                        "Added new stats parsing and display throughout the app, including character stats and dungeon completions.",
                        "Added support for displaying Fame Bonuses and their conditions in the Character Details popup.",
                        "Added a quick export of dungeons needed to achieve all / specific bonuses.",
                        "Added a Fame Bonus prediction upon death (WIP)"
                    ]}
                />

                <ChangelogEntry
                    title={'Vault Peeker'}
                    listOfChanges={[
                        "Redesigned the whole Vault Peeker interface for better usability and appearance.",
                        "Added support for Enchantments and Rarity display.",
                        "Added support for Bag Types.",
                        "Improved performance when loading many items at once.",
                        "Updated the item images and definitions to the latest version, including new items and fixes to existing ones.",
                    ]}
                />

                <ChangelogEntry
                    title={"Accounts Table"}
                    listOfChanges={[
                        "• Added a new context menu when right-clicking an account in the Accounts Table with various options like starting the game, reopening the account, and more.",
                    ]}
                />

                <ChangelogEntry
                    title={'Notifications'}
                    listOfChanges={[
                        "Added support for toast notifications on Windows and macOS.",
                        "Added a end-of-month reminder toast notification for unclaimed Daily Login rewards. This can be disabled in the Settings.",
                    ]}
                />

                <ChangelogEntry
                    title={'Settings'}
                    listOfChanges={[
                        "Added a new setting to enable or disable the end-of-month reminder toast notification for unclaimed Daily Login rewards.",
                        "Added a new setting to change the item render density.",
                        "Fixed an issue where Okta was not properly being displayed when opting in for analytics.",
                    ]}
                />

                <ChangelogEntry
                    title={'Daily Logins'}
                    listOfChanges={[
                        "Added the option to start the Daily Login process manually via a button in the Daily Logins page.",
                        "Fixed an issue where the Daily Login did not properly start on the second day and onwards.",
                    ]}
                />

                <ChangelogEntry
                    title={'Windows only changes'}
                    listOfChanges={[
                        "• Fixed the uninstaller to also remove the daily login task.",
                    ]}
                />

                <ChangelogEntry
                    title={'Miscellaneous'}
                    listOfChanges={[
                        "Added support for the new Druid class. - Although textures and skins are not yet supported.",
                        "Added support for character statistics and dungeon completions.",
                        "Added a new snackbar for exporting accounts with a button to open the exported file in Explorer/Finder.",
                        "Changed the overall appearance of EAM with a new logo and updated borders.",
                        "Changed the Stripe Logo to the new version.",
                        "Fixed the formatting of the EAM branding in the developer tools on macOS.",
                        "Fixed exporting accounts causing EAM to not work properly until restarted.",
                        "Temporarily removed the rudementary Dungeon Stats popup due to unreliability and its outdated appearance. Will be back soon."
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
                                <li>Ykao</li>
                                <li>Rapshe</li>
                                <li>K3y0708</li>
                            </ul>
                        </Typography>
                        <Typography component={'span'} variant="body2" fontWeight={'bold'} color="textSecondary">
                            <ul>
                                <li>MelonPerson - for creating the all the new Okta related images</li>
                                <li>059 - for providing new constants.js and renders.png</li>
                                <li>Faynt - for helping with constants.js and providing sheets.js</li>
                            </ul>
                        </Typography>
                    </Box>
                </Box>
            </Box>
        </Paper>
    );
}


export default ChangelogVersion4_4_1;