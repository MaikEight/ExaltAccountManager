import { useEffect } from "react";
import { useState } from "react";
import ComponentBox from "../components/ComponentBox";
import BugReportOutlinedIcon from '@mui/icons-material/BugReportOutlined';
import { Box, Checkbox, Divider, FormControl, FormControlLabel, Typography, useTheme } from "@mui/material";
import { max } from "lodash";

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

    return (
        <Box
            sx={{
                display: 'flex',
                height: '100%',
            }}
        >
            <ComponentBox
                title="Debug Flags"
                icon={<BugReportOutlinedIcon />}
                fullwidth={true}
                sx={{
                    overflow: 'hidden',
                    display: 'flex',
                    flexDirection: 'column',
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
                            This page allows you to enable or disable debug flags that control various features and logging in the application.
                            These flags can be useful for debugging and development purposes.
                        </Typography>
                        <Typography variant="body1" sx={{ marginBottom: 2 }}>
                            To enable a flag, check the corresponding checkbox. The changes will be saved in the session storage and will persist until you close the application.
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
                    <Box
                        id="debug-flags-list"
                        sx={{
                            display: 'flex',
                            flexDirection: 'column',
                            overflowY: 'auto',
                            padding: 2,
                            width: '100%',
                            flexGrow: 1,
                            minHeight: 0,
                            maxHeight: 'calc(100vh - 255px)',
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
            </ComponentBox>
        </Box>
    );
}

export default DebugFlagsPage;