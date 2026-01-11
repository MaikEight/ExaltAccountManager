import { Box, Paper, Skeleton, Typography, useTheme } from '@mui/material';
import ChangelogEntry from './ChangelogEntry';
import { useState } from 'react';

function ChangelogVersion4_4_0() {
    const theme = useTheme();

    const [imageLoaded, setImageLoaded] = useState(false);

    const title = [
        "Exalt Account Manager v4.4.0",
        "New Logo, Widgets and improvements!",
    ];

    return (
        <Paper
            sx={{
                display: 'flex',
                flexDirection: 'column',
                alignItems: 'center',
                justifyContent: 'flex-start',
                borderRadius: `${theme.shape.borderRadius * 2}px`,
                width: '925px',
                maxHeight: '95vh',
                maxWidth: '90vw',
                overflow: 'auto',
            }}
        >
            {
                !imageLoaded && (
                    <Skeleton
                        variant="rectangular"
                        sx={{
                            width: '100%',
                            height: '525px',
                            minHeight: '525px',
                            borderRadius: `0 0 ${theme.shape.borderRadius * 2}px ${theme.shape.borderRadius * 2}px`,
                            flexShrink: 0,
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

                }}
            />
            <Box
                sx={{
                    position: 'sticky',
                    top: 0,
                    display: 'flex',
                    width: '100%',
                    borderRadius: `${theme.shape.borderRadius - 1}px`,
                    pt: 0,
                    px: 2,
                    pb: 1,
                    zIndex: 1,
                }}
            >
                {/* HEADLINE */}
                <Box
                    sx={{
                        pt: 2,
                        display: 'flex',
                        flexDirection: 'column',
                        width: '100%',
                        alignItems: 'start',
                        justifyContent: 'center',
                        backgroundColor: theme.palette.background.paperLight,
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
                    title={'Miscellaneous'}
                    listOfChanges={[
                        "",
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


export default ChangelogVersion4_4_0;