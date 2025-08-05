import { useEffect } from "react";
import { useState } from "react";
import ComponentBox from "../components/ComponentBox";
import BugReportOutlinedIcon from '@mui/icons-material/BugReportOutlined';
import { Box, Checkbox, Divider, FormControl, FormControlLabel, Grid, Tooltip, Typography, useTheme } from "@mui/material";
import StyledButton from './../components/StyledButton';
import { CACHE_PREFIX } from "../constants";

const flags = [
    {
        name: 'Debug',
        description: 'Enable debug mode, which includes additional logging and debugging information in the console.',
        flagKey: 'flag:debug',
        category: 'debug',
    },
    {
        name: 'Account/verify',
        description: 'Copy account verify response (as json) to clipboard after fetching.',
        flagKey: 'flag:copy:account/verify',
        category: 'network',
    },
    {
        name: 'Char/list',
        description: 'Copy character list response (as json) to clipboard after fetching.',
        flagKey: 'flag:copy:char/list',
        category: 'network',
    },
    {
        name: 'Account/register',
        description: 'Copy account register response (as json) to clipboard after fetching.',
        flagKey: 'flag:copy:account/register',
        category: 'network',
    },
    {
        name: 'App-init',
        description: 'Copy app init response (as json) to clipboard after fetching.',
        flagKey: 'flag:copy:app-init',
        category: 'network',
    },
    {
        name: 'Game-file-list',
        description: 'Copy game file list response (as json) to clipboard after fetching.',
        flagKey: 'flag:copy:game-file-list',
        category: 'network',
    },
    {
        name: 'eam-plus-prices',
        description: 'Copy eam-plus prices response (as json) to clipboard after fetching.',
        flagKey: 'flag:copy:eam-plus-prices',
        category: 'network',
    },
    {
        name: 'copy:profile-image',
        description: 'Copy profile image to clipboard after fetching.',
        flagKey: 'flag:copy:profile-image',
        category: 'network',
    },
]

function DebugFlagsPage() {
    const theme = useTheme();

    const [activeFlags, setActiveFlags] = useState([]);
    const [warningActive, setWarningActive] = useState(false);

    useEffect(() => {
        const getAndSetActiveFlags = () => {
            const currentFlags = flags.filter(flag => sessionStorage.getItem(flag.flagKey) === 'true').map(flag => flag.flagKey);
            setActiveFlags(currentFlags);
        };
        const intervaslId = setInterval(() => {
            getAndSetActiveFlags();
        }, 1000);

        getAndSetActiveFlags();
        return () => clearInterval(intervaslId);
    }, []);

    const clearAllCacheItemsWithPrefix = (prefix) => {
        const keysToRemove = Object.keys(localStorage).filter(key => key.startsWith(prefix));
        console.log(`🔥 Clearing cache items with prefix "${prefix}":`, keysToRemove);
        keysToRemove.forEach(key => {
            localStorage.removeItem(key);
            console.log(`🔥 Cleared cache item: ${key}`);
        });
    }

    const getLocalStorageSize = () => {
        let totalBytes = 0;
        for (let key in localStorage) {
            if (localStorage.hasOwnProperty(key)) {
                totalBytes += key.length + localStorage[key].length;
            }
        }
        return totalBytes;
    }

    const getSessionStorageSize = () => {
        let totalBytes = 0;
        for (let key in sessionStorage) {
            if (sessionStorage.hasOwnProperty(key)) {
                totalBytes += key.length + sessionStorage[key].length;
            }
        }
        return totalBytes;
    }

    const formatBytes = (bytes) => {
        if (bytes === 0) return '0 Bytes';
        const k = 1024;
        const sizes = ['Bytes', 'KB', 'MB', 'GB'];
        const i = Math.floor(Math.log(bytes) / Math.log(k));
        return parseFloat((bytes / Math.pow(k, i)).toFixed(2)) + ' ' + sizes[i];
    }

    return (
        <Box
            sx={{
                display: 'flex',
                flexGrow: 1,
                overflowY: 'auto',
            }}
        >
            <ComponentBox
                title="Debug Flags"
                icon={<BugReportOutlinedIcon />}
                fullwidth={true}
                sx={{
                    display: 'flex',
                    flexDirection: 'column',
                    flexGrow: 1,
                    height: 'fit-content',
                }}
            >
                <Box
                    id="debug-flags-box-main"
                    sx={{
                        width: '100%',
                        display: 'flex',
                        flexDirection: 'column',
                        minHeight: 0,
                        flexGrow: 1,
                        flexShrink: 0,
                        gap: 1,
                    }}
                >
                    <Box
                        sx={{
                            flexShrink: 0,
                            display: 'flex',
                            flexDirection: 'column',
                            position: 'relative',
                        }}
                    >
                        <Typography variant="body1" sx={{ marginBottom: 2 }}>
                            This page allows you to manipulate debug flags that control various features and logging in the application. You can also clear your cache and stored values to resolve issues related to stale data or images.
                        </Typography>
                        <Typography variant="body1" sx={{ marginBottom: 2 }}>
                            To view the debug logs, open the developer console by pressing <strong>F12</strong>.
                        </Typography>
                        <img
                            src="mascot/Error/error_mascot_only_2_small_very_low_res.png"
                            alt="Debug Mascot"
                            style={{
                                position: 'absolute',
                                bottom: 0,
                                right: 32,
                                height: '75px'
                            }}
                        />
                    </Box>
                    <Divider flexItem variant="middle" sx={{ mb: 2 }} />
                    <Box
                        id="cache-management"
                        sx={{
                            display: 'flex',
                            flexDirection: 'column',
                            gap: 1,
                        }}
                    >
                        <Typography variant="h5" sx={{ marginBottom: 1 }}>
                            Cache Management
                        </Typography>
                        <Typography variant="body1">
                            Use the buttons below to clear specific cached items or all browser stored values. This can help resolve issues related to stale data or images.
                        </Typography>
                        <Grid
                            container
                            spacing={0.25}
                            sx={{
                                width: '375px',
                            }}
                        >
                            <Grid size={5}>
                                <Typography variant="body1" fontWeight={600}>
                                    Storage type
                                </Typography>
                            </Grid>
                            <Grid size={3}>
                                <Typography variant="body1" fontWeight={600}>
                                    Items
                                </Typography>
                            </Grid>
                            <Grid size={4}>
                                <Typography variant="body1" fontWeight={600}>
                                    Size
                                </Typography>
                            </Grid>
                            <Grid size={5}>
                                <Typography variant="body1">
                                    LocalStorage
                                </Typography>
                            </Grid>
                            <Grid size={3}>
                                <Typography variant="body1">
                                    {Object.keys(localStorage).length}
                                </Typography>
                            </Grid>
                            <Grid size={4}>
                                <Typography variant="body1" fontWeight={600}>
                                    {formatBytes(getLocalStorageSize())}
                                </Typography>
                            </Grid>

                            <Grid size={5}>
                                <Typography variant="body1">
                                    SessionStorage
                                </Typography>
                            </Grid>
                            <Grid size={3}>
                                <Typography variant="body1">
                                    {Object.keys(sessionStorage).length}
                                </Typography>
                            </Grid>
                            <Grid size={4}>
                                <Typography variant="body1" fontWeight={600}>
                                    {formatBytes(getSessionStorageSize())}
                                </Typography>
                            </Grid>
                        </Grid>
                        <Typography variant="body1" sx={{ my: 1 }}>
                            ⚠️ This can not be undone! ⚠️
                        </Typography>
                        <Box
                            sx={{
                                display: 'flex',
                                flexDirection: 'row',
                                gap: 1.5,
                            }}
                        >
                            <StyledButton
                                onClick={() => {
                                    setWarningActive(false);
                                    clearAllCacheItemsWithPrefix(`${CACHE_PREFIX}drawItem`);
                                    clearAllCacheItemsWithPrefix(`${CACHE_PREFIX}single-item`);
                                }}
                                color="secondary"
                                size="small"
                                sx={{
                                    width: 'fit-content',
                                }}
                            >
                                Clear item images
                            </StyledButton>
                            <StyledButton
                                onClick={() => {
                                    setWarningActive(false);
                                    clearAllCacheItemsWithPrefix(`${CACHE_PREFIX}portrait`)}
                                }
                                color="secondary"
                                size="small"
                                sx={{
                                    width: 'fit-content',
                                }}
                            >
                                Clear portrait images
                            </StyledButton>
                            <StyledButton
                                onClick={() => {
                                    setWarningActive(false);
                                    clearAllCacheItemsWithPrefix(`${CACHE_PREFIX}portrait`);
                                }}
                                color="warning"
                                size="small"
                                sx={{
                                    width: 'fit-content',
                                }}
                            >
                                Evict cache
                            </StyledButton>
                            <Tooltip title="This will clear all localStorage and sessionStorage items, including your settings and preferences. Use with caution!">
                                <StyledButton
                                    onClick={() => {
                                        if (!warningActive) {
                                            setWarningActive(true);
                                            return;
                                        }
                                        console.warn('⚠️ Clearing all localStorage & sessionStorage items and cache ⚠️');

                                        sessionStorage.clear();
                                        localStorage.clear();

                                        setWarningActive(false);
                                    }}
                                    color="error"
                                    size="small"
                                    sx={{
                                        width: 'fit-content',
                                    }}
                                >
                                    {!warningActive ? 'Clear ALL stored values' : 'Are you sure?'}
                                </StyledButton>
                            </Tooltip>
                        </Box>
                    </Box>
                    <Divider flexItem variant="middle" sx={{ mt: 2 }} />
                    <Box
                        sx={{
                            display: 'flex',
                            flexDirection: 'column',
                            gap: 1.5,
                            marginTop: 1,
                        }}
                    >
                        <Typography variant="h5" sx={{ marginBottom: 1 }}>
                            Debug Flags
                        </Typography>
                        <Typography variant="body1" sx={{ marginBottom: 2 }}>
                            To enable a flag, check the corresponding checkbox. The changes will be saved in the session storage and will persist until you close the application.
                        </Typography>
                        <Box
                            id="debug-flags-list"
                            sx={{
                                display: 'flex',
                                flexDirection: 'column',
                                overflowY: 'auto',
                                px: 2,
                                pb: 1,
                                width: '100%',
                                flexGrow: 1,
                                minHeight: 0,
                                height: '450px',
                                gap: 1.5,
                            }}
                        >
                            {
                                flags.map((flag, index) => (
                                    <Box
                                        key={'flag-' + index}
                                        sx={{
                                            display: 'flex',
                                            flexDirection: 'column',
                                            padding: 1.25,
                                            backgroundColor: theme.palette.background.default,
                                            borderRadius: `${theme.shape.borderRadius}px`,
                                        }}
                                    >
                                        <Typography variant="h6">
                                            {flag.name}
                                        </Typography>
                                        <FormControlLabel
                                            label={flag.description}
                                            control={
                                                <Checkbox
                                                    checked={activeFlags.includes(flag.flagKey)}
                                                    onChange={(event) => {
                                                        if (event.target.checked) {
                                                            sessionStorage.setItem(flag.flagKey, 'true');
                                                            console.log(`🟢 Flag ${flag.flagKey.replace("flag:", "")} enabled`);
                                                        } else {
                                                            sessionStorage.removeItem(flag.flagKey);
                                                            console.log(`🔵 Flag ${flag.flagKey.replace("flag:", "")} disabled`);
                                                        }

                                                        setActiveFlags(prev => {
                                                            if (event.target.checked) {
                                                                return [...prev, flag.flagKey];
                                                            } else {
                                                                return prev.filter(f => f !== flag.flagKey);
                                                            }
                                                        });
                                                    }}
                                                />
                                            }
                                        />

                                    </Box>
                                ))
                            }
                        </Box>
                    </Box>
                </Box>
            </ComponentBox >
        </Box >
    );
}

export default DebugFlagsPage;