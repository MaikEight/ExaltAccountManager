import { Box, Divider, LinearProgress, Paper, Popover, Stack, Typography, useTheme } from "@mui/material";
import useBackgroundSync from './../hooks/useBackgroundSync';
import { SyncMode } from './../contexts/BackgroundSyncContext';
import QueryBuilderOutlinedIcon from '@mui/icons-material/QueryBuilderOutlined';
import useAccounts from "../hooks/useAccounts";
import { useEffect, useMemo, useState } from "react";
import SyncOutlinedIcon from '@mui/icons-material/SyncOutlined';
import PriorityHighOutlinedIcon from '@mui/icons-material/PriorityHighOutlined';
import PublishedWithChangesOutlinedIcon from '@mui/icons-material/PublishedWithChangesOutlined';
import PauseIcon from '@mui/icons-material/Pause';

function BackgroundSyncComponent() {
    const theme = useTheme();
    const { getAccountByEmail } = useAccounts();
    const { syncMode, uiState, dailyLoginProgressData, emailToAccountNameMap } = useBackgroundSync();

    const [anchorEl, setAnchorEl] = useState(null);
    const [ownUiState, setOwnUiState] = useState({
        email: '',
        state: '',
        updatedAt: new Date(),
    });

    useEffect(() => {
        let timeoutId = null;
        if (!uiState.updatedAt) {
            timeoutId = setTimeout(() => {
                setOwnUiState(uiState);
            }, 300);
        } else {
            setOwnUiState(uiState);
        }
        return () => {
            if (timeoutId) {
                clearTimeout(timeoutId);
            }
        }
    }, [uiState]);

    const getIconToDisplay = (bigSize = false) => {
        const sx = { fontSize: bigSize ? '24px' : '16px' };
        if (syncMode === SyncMode.Stopped) {
            return <PauseIcon sx={sx} />;
        }

        if (Date.now() > ownUiState.updatedAt + (1000 * 5)) {
            return <QueryBuilderOutlinedIcon sx={sx} />;
        }

        if (!ownUiState.state) {
            return <QueryBuilderOutlinedIcon sx={sx} />;
        }

        if (['Started',
            'Processing..',
            'Processing...',
            'Processing....'
        ].includes(ownUiState.state)) {
            return <SyncOutlinedIcon
                sx={{
                    ...sx,
                    animation: 'spin 2s linear infinite',
                    '@keyframes spin': {
                        '0%': { transform: 'rotate(360deg)' },
                        '100%': { transform: 'rotate(0deg)' },
                    },
                }}
            />;
        }

        if (['Waiting'].includes(uiState.state)) {
            return <QueryBuilderOutlinedIcon sx={sx} />;
        }

        if (ownUiState.state === 'Success') {
            return <PublishedWithChangesOutlinedIcon sx={sx} />;
        }

        // Failed, BGSyncError, WrongPassword,
        // TooManyRequests, Captcha, AccountSuspended
        // AccountInUse, Error, RateLimitExceeded, BGSyncError
        return <PriorityHighOutlinedIcon sx={sx} />;
    }

    const getStateToDisplay = () => {
        if (!ownUiState.state) {
            return 'Waiting...';
        }

        switch (ownUiState.state) {
            case 'Started':
                return 'Starting';
            case 'Processing..':
            case 'Processing...':
            case 'Processing....':
                return 'Processing';
            case 'Waiting':
                return 'Waiting';
            case 'Success':
                return 'Success';
            default:
                return ownUiState.state?.replaceAll('.', '') || '';
        }
    }

    const accountName = useMemo(() => {
        if (!ownUiState.email) return '';

        if (emailToAccountNameMap[ownUiState.email] && emailToAccountNameMap[ownUiState.email] !== '') {
            return emailToAccountNameMap[ownUiState.email];
        }

        const account = getAccountByEmail(uiState.email);
        return account ? account.name : uiState.email;
    }, [ownUiState.email, getAccountByEmail, emailToAccountNameMap]);

    const handlePopoverOpen = (event) => {
        setAnchorEl(event.currentTarget);
    };

    const handlePopoverClose = () => {
        setAnchorEl(null);
    };

    const open = Boolean(anchorEl);
    const isShown = useMemo(() => {
        return (syncMode !== SyncMode.Stopped
            && Boolean(accountName)
            && Boolean(uiState.state));
    }, [syncMode, accountName, uiState]);

    const popoverContent = useMemo(() => {
        if (!isShown) return null;

        // Background Sync
        if (syncMode === SyncMode.Default) {
            return (
                <Box
                    sx={{
                        display: 'flex',
                        flexDirection: 'column',
                        gap: 1,
                    }}
                >
                    <Typography variant="body1">
                        Background Sync
                    </Typography>
                    <Box
                        sx={{
                            display: 'flex',
                            flexDirection: 'row',
                            alignItems: 'stretch',
                            justifyContent: 'center',
                            gap: 1,
                            p: 0
                        }}
                    >
                        <Box
                            sx={{
                                display: 'flex',
                                flexDirection: 'row',
                                alignItems: 'center',
                                justifyContent: 'center',
                            }}
                        >
                            {getIconToDisplay(true)}
                        </Box>
                        <Divider orientation="vertical" flexItem variant="middle" />
                        <Box
                            sx={{
                                display: 'flex',
                                flexDirection: 'column',
                                justifyContent: 'space-between',
                            }}
                        >
                            <Typography variant="subtitle1">
                                {accountName}
                            </Typography>
                            <Typography variant="subtitle2">
                                {ownUiState.email}
                            </Typography>
                        </Box>
                        <Divider orientation="vertical" flexItem variant="middle" />
                        <Box
                            sx={{
                                display: 'flex',
                                flexDirection: 'column',
                                justifyContent: 'space-between',
                                pt: 0.25,
                            }}
                        >
                            <Typography variant="caption">
                                State
                            </Typography>
                            <Typography variant="body2">
                                {getStateToDisplay()}
                            </Typography>
                        </Box>
                    </Box>
                </Box>
            )
        }

        const progressValue = dailyLoginProgressData
            && dailyLoginProgressData.done !== null
            && dailyLoginProgressData.left !== null
            ? ((dailyLoginProgressData.done / (dailyLoginProgressData.done + dailyLoginProgressData.left)) * 100)
            : 0;

        if (sessionStorage.getItem('flag:debug')) {
            console.log('Daily Login Progress Data:', dailyLoginProgressData);
            console.log('Progress Value:', progressValue);
        }

        //Daily Login Progress
        if (syncMode === SyncMode.DailyLogin) {
            return (
                <Box
                    sx={{
                        display: 'flex',
                        flexDirection: 'column',
                        gap: 1,
                    }}
                >
                    <Typography variant="body1">
                        Daily Login
                    </Typography>
                    <Box
                        sx={{
                            display: 'flex',
                            flexDirection: 'row',
                            alignItems: 'stretch',
                            justifyContent: 'center',
                            gap: 1,
                            p: 0
                        }}
                    >
                        <Box
                            sx={{
                                display: 'flex',
                                flexDirection: 'row',
                                alignItems: 'center',
                                justifyContent: 'center',
                            }}
                        >
                            {getIconToDisplay(true)}
                        </Box>
                        <Divider orientation="vertical" flexItem variant="middle" />
                        <Box
                            sx={{
                                display: 'flex',
                                flexDirection: 'column',
                                justifyContent: 'space-between',
                            }}
                        >
                            <Typography variant="subtitle1">
                                {accountName}
                            </Typography>
                            <Typography variant="subtitle2">
                                {ownUiState.email}
                            </Typography>
                        </Box>
                        <Divider orientation="vertical" flexItem variant="middle" />
                        <Box
                            sx={{
                                display: 'flex',
                                flexDirection: 'column',
                                justifyContent: 'space-between',
                                pt: 0.25,
                            }}
                        >
                            <Typography variant="caption">
                                State
                            </Typography>
                            <Typography variant="body2">
                                {getStateToDisplay()}
                            </Typography>
                        </Box>
                    </Box>
                    <Divider flexItem variant="middle" />
                    {/* Daily Login State */}
                    <Box
                        sx={{
                            display: 'flex',
                            flexDirection: 'column',
                            gap: 1,
                        }}
                    >
                        <Typography variant="body1">
                            Progress Report
                        </Typography>
                        <Box>
                            <Box
                                sx={{
                                    display: 'flex',
                                    flexDirection: 'row',
                                    justifyContent: 'space-between',
                                    alignItems: 'center',
                                    gap: 1,
                                }}
                            >
                                <Typography variant="caption">
                                    {dailyLoginProgressData?.done}
                                </Typography>
                                <LinearProgress
                                    variant="determinate"
                                    value={progressValue}
                                    sx={{
                                        width: '100%',
                                        mt: 0.5,
                                        borderRadius: theme.shape.borderRadius,
                                    }}
                                />
                                <Typography variant="caption">
                                    {dailyLoginProgressData?.done + dailyLoginProgressData?.left}
                                </Typography>
                            </Box>
                            <Typography variant="caption">
                                {
                                    `Estimated finish time: ${dailyLoginProgressData?.estimatedTime ?
                                        dailyLoginProgressData?.estimatedTime?.toLocaleTimeString([], {
                                            hour: '2-digit',
                                            minute: '2-digit'
                                        }) : 'N/A'}`
                                }
                            </Typography>
                        </Box>
                        <Box
                            sx={{
                                display: 'flex',
                                flexDirection: 'row',
                                alignItems: 'start',
                                justifyContent: 'space-between',
                                gap: 1,
                            }}
                        >
                            {
                                dailyLoginProgressData?.leftAccountNames &&
                                dailyLoginProgressData?.leftAccountNames.length > 0 &&
                                <Box
                                    sx={{
                                        display: 'flex',
                                        flexDirection: 'column',
                                        alignItems: 'start',
                                        gap: 1,
                                    }}
                                >
                                    <Typography variant="body2">
                                        Accounts to process
                                    </Typography>
                                    <Box
                                        sx={{
                                            display: 'flex',
                                            flexDirection: 'column',
                                            alignItems: 'start',
                                            gap: 0,
                                            pl: 0.5,
                                        }}
                                    >
                                        {
                                            dailyLoginProgressData?.leftAccountNames.map(async (name, index) => {
                                                if (!name || name === '') {
                                                    const n = getAccountByEmail(dailyLoginProgressData?.leftEmails[index]);
                                                    if (n && n.name) {
                                                        name = n.name || dailyLoginProgressData?.leftEmails[index];
                                                    }
                                                }

                                                return (
                                                    <Typography
                                                        key={name + index}
                                                        variant="caption"
                                                        sx={{
                                                            textOverflow: 'ellipsis',
                                                            overflow: 'hidden',
                                                            whiteSpace: 'nowrap',
                                                        }}
                                                    >
                                                        {name}
                                                    </Typography>
                                                )
                                            })
                                        }
                                    </Box>
                                </Box>
                            }
                            {
                                dailyLoginProgressData?.leftAccountNames &&
                                dailyLoginProgressData?.leftAccountNames.length > 0 &&
                                dailyLoginProgressData?.failedAccountNames &&
                                dailyLoginProgressData?.failedAccountNames.length > 0 &&
                                <Divider
                                    flexItem
                                    variant="middle"
                                    orientation="vertical"
                                />
                            }
                            {
                                dailyLoginProgressData?.failedAccountNames &&
                                dailyLoginProgressData?.failedAccountNames.length > 0 &&
                                <Box
                                    sx={{
                                        display: 'flex',
                                        flexDirection: 'column',
                                        alignItems: 'start',
                                        gap: 1,
                                    }}
                                >
                                    <Typography variant="body2">
                                        Failed Accounts
                                    </Typography>
                                    <Box
                                        sx={{
                                            display: 'flex',
                                            flexDirection: 'column',
                                            alignItems: 'start',
                                            gap: 0,
                                            ...(dailyLoginProgressData?.leftAccountNames &&
                                                dailyLoginProgressData?.leftAccountNames.length > 0
                                                ? { pr: 0.5 } : { pl: 0.5 }),
                                        }}
                                    >
                                        {
                                            dailyLoginProgressData?.failedAccountNames.map((name, index) => {
                                                if (!name || name === '') {
                                                    const n = getAccountByEmail(dailyLoginProgressData?.failedEmails[index]);
                                                    if (n && n.name) {
                                                        name = n.name || dailyLoginProgressData?.failedEmails[index];
                                                    }
                                                }

                                                return (
                                                    <Typography
                                                        key={name + index}
                                                        variant="caption"
                                                        sx={{
                                                            textOverflow: 'ellipsis',
                                                            overflow: 'hidden',
                                                            whiteSpace: 'nowrap',
                                                        }}
                                                    >
                                                        {name}
                                                    </Typography>
                                                )
                                            })
                                        }
                                    </Box>
                                </Box>
                            }
                        </Box>
                    </Box>
                </Box>
            );
        }
    }, [ownUiState, syncMode, dailyLoginProgressData]);

    return (
        <Box
            id="background-sync-component"
            aria-owns={open ? 'bgr-sync-mouse-over-popover' : undefined}
            aria-haspopup="true"
            sx={{
                position: 'absolute',
                top: !isShown ? -24 : 0,
                left: 226,
                height: 'fit-content',
                width: 'fit-content',
                border: `1px solid ${theme.palette.divider}`,
                borderTop: 'none',
                borderColor: theme.palette.divider,
                borderRadius: `0 0 ${theme.shape.borderRadius}px ${theme.shape.borderRadius}px`,
                zIndex: 0,
                p: 0.125,
                px: 0.75,
                cursor: 'default',
                transition: 'top 0.3s ease',
                overflow: 'hidden',
            }}
            onMouseEnter={handlePopoverOpen}
            onMouseLeave={handlePopoverClose}
        >
            <Box
                sx={{
                    display: 'flex',
                    alignItems: 'center',
                    gap: 0.5,
                    color: theme.palette.text.secondary,
                }}
            >
                {getIconToDisplay()}
                <Divider orientation="vertical" flexItem variant="middle" sx={{ my: '6px' }} />
                <Typography variant="body2">
                    {accountName}
                </Typography>
                <Divider orientation="vertical" flexItem variant="middle" sx={{ my: '6px' }} />
                <Typography variant="body2">
                    {getStateToDisplay()}
                </Typography>
            </Box>
            <Popover
                id="bgr-sync-mouse-over-popover"
                open={open && isShown}
                anchorEl={anchorEl}
                sx={{
                    backgroundColor: 'transparent',
                    pointerEvents: 'none',
                    p: 0,
                }}
                onClose={handlePopoverClose}
                anchorOrigin={{
                    vertical: 'bottom',
                    horizontal: 'center',
                }}
                transformOrigin={{
                    vertical: 'top',
                    horizontal: 'center',
                }}
                slotProps={{
                    paper: {
                        sx: {
                            m: 0,
                            mt: 0.75,
                            p: 0.25,
                            backgroundColor: theme.palette.background.paper,
                            borderRadius: `${theme.shape.borderRadius}px`,
                            height: 'fit-content',
                            width: 'fit-content',
                            overflow: 'hidden',
                        }
                    }
                }}
                disableRestoreFocus
            >
                <Box
                    sx={{
                        backgroundColor: theme.palette.background.default,
                        borderRadius: `${theme.shape.borderRadius - 2}px`,
                        p: 1,
                        m: 0,
                    }}
                >
                    {popoverContent}
                </Box>
            </Popover>
        </Box>
    );
}

export default BackgroundSyncComponent;