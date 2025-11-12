import { Box, Button, ButtonGroup, Chip, Paper, Popover, Tooltip, Typography } from "@mui/material";
import { getCurrentWindow } from '@tauri-apps/api/window'
import MinimizeIcon from '@mui/icons-material/Minimize';
import CloseIcon from '@mui/icons-material/Close';
import CropSquareIcon from '@mui/icons-material/CropSquare';
import { useTheme } from "@emotion/react";
import { listen } from '@tauri-apps/api/event';
import { useEffect, useState } from "react";
import VpnLockOutlinedIcon from '@mui/icons-material/VpnLockOutlined';
import FlagCircleOutlinedIcon from '@mui/icons-material/FlagCircleOutlined';
import { MASCOT_NAME } from "../../constants";
import useUserSettings from "../../hooks/useUserSettings";
import WindowControls from "./MacOS/WindowControls";
import isMacOS from "../../utils/isMacOS";

function CustomToolbar(props) {
    const theme = useTheme();
    const { getByKeyAndSubKey } = useUserSettings();
    const isMac = isMacOS();

    const [anchorEl, setAnchorEl] = useState(null);
    const [hasGlobalApiCooldown, setHasGlobalApiCooldown] = useState(false);
    const [apiRemainingLimits, setApiRemainingLimits] = useState(new Map([
        ['account/verify', 30],
        ['char/list', 5],
        ['account/register', 5],
    ]));

    const minimizeToTray = getByKeyAndSubKey('general', 'minimizeToTray');

    useEffect(() => {
        let unlistenGlobalApiCooldown;
        let unlistenApiRemainingLimits;

        const registerListeners = async () => {
            unlistenGlobalApiCooldown = await listen('api-cooldown', (event) => {
                setHasGlobalApiCooldown(event.payload);
            });
            unlistenApiRemainingLimits = await listen('api-remaining-changed', (event) => {
                const data = {
                    api: event.payload[0],
                    remaining: event.payload[1],
                    limit: event.payload[2],
                };

                setApiRemainingLimits((prev) => {
                    const newLimits = new Map(prev);
                    newLimits.set(data.api, data.remaining);
                    return newLimits;
                });
            });
        };
        registerListeners()
            .catch(console.error);

        return () => {
            unlistenGlobalApiCooldown?.();
            unlistenApiRemainingLimits?.();
        }
    }, []);

    const handlePopoverOpen = (event) => {
        setAnchorEl(event.currentTarget);
    };

    const handlePopoverClose = () => {
        setAnchorEl(null);
    };

    const open = Boolean(anchorEl);

    const getEmptyApiLimitChip = () => {
        const hasEmptyLimit = Array.from(apiRemainingLimits.values()).some(limit => limit <= 0);

        if (!hasEmptyLimit) {
            return null;
        }

        return (
            <Tooltip
                title={(
                    <Box sx={{ display: 'flex', flexDirection: 'column' }}>
                        <Typography variant="body2" sx={{ mb: 0.5 }}>
                            Limit for the following APIs reached
                        </Typography>
                        {
                            Array.from(apiRemainingLimits.entries())
                                .filter(([_, limit]) => limit <= 0)
                                .map(([apiType]) => (
                                    <Typography
                                        key={apiType}
                                        variant="caption"
                                    >
                                        {apiType}
                                    </Typography>
                                ))
                        }
                    </Box>
                )}
                component="span"
                sx={{
                    zIndex: 1001,
                }}
            >
                <Chip
                    variant="outlined"
                    label={"API Limit reached"}
                    size="small"
                    color={"warning"}
                    icon={<FlagCircleOutlinedIcon sx={{ pl: '2px' }} />}
                    onClick={() => null}
                />
            </Tooltip>
        );
    }

    return (
        <Box
            id="custom-toolbar"
            sx={{
                display: "flex",
                justifyContent: "space-between",
                height: 30,
                ...props.sx
            }}
        >
            {
                isMac === true && (
                    <WindowControls />
                )
            }
            <Box
                sx={{
                    display: "flex",
                    justifyContent: "end",
                    height: 30,
                    ...props.sx
                }}
            >
                {/* STATUS CHIPS */}
                <Box
                    sx={{
                        display: "flex",
                        alignItems: "center",
                        mr: 1,
                        gap: 1,
                    }}
                >
                    {getEmptyApiLimitChip()}
                    {
                        hasGlobalApiCooldown &&
                        <>
                            <Box
                                aria-owns={open ? 'mouse-over-popover' : undefined}
                                onMouseEnter={handlePopoverOpen}
                                onMouseLeave={handlePopoverClose}
                            >
                                <Chip
                                    variant="outlined"
                                    label={"API Cooldown"}
                                    size="small"
                                    color={"error"}
                                    icon={<VpnLockOutlinedIcon sx={{ pl: '2px' }} />}
                                    onClick={() => null}
                                    sx={{
                                        '&:hover': {
                                            cursor: 'default',
                                        },
                                    }}
                                />
                            </Box>
                            <Popover
                                id="mouse-over-popover"
                                sx={{ pointerEvents: 'none' }}
                                open={open}
                                anchorEl={anchorEl}
                                anchorOrigin={{
                                    vertical: 'bottom',
                                    horizontal: 'left',
                                }}
                                transformOrigin={{
                                    vertical: 'top',
                                    horizontal: 'left',
                                }}
                                onClose={handlePopoverClose}
                                disableRestoreFocus
                            >
                                <Paper
                                    sx={{
                                        display: 'flex',
                                        alignItems: 'center',
                                        justifyContent: 'center',
                                        p: 0.25,
                                        borderRadius: `${(theme?.shape?.borderRadius || 9)}px`,
                                        overflow: 'hidden',
                                        height: 'fit-content',
                                        width: 'fit-content',
                                    }}
                                >
                                    <Box
                                        sx={{
                                            backgroundColor: theme?.palette?.background?.default || '#28243D',
                                            borderRadius: `${(theme?.shape?.borderRadius || 9) - 2}px`,
                                            display: 'flex',
                                            flexDirection: 'column',
                                            alignItems: 'center',
                                            justifyContent: 'space-between',
                                            p: 1,
                                            px: 1.5,
                                            gap: 0.75,
                                            height: 'fit-content',
                                            width: 'fit-content',
                                        }}
                                    >
                                        <Typography variant="body1">
                                            Global API Cooldown is active
                                        </Typography>
                                        <Box
                                            sx={{
                                                position: 'relative',
                                            }}
                                        >
                                            <img
                                                src="mascot/Error/error_network_very_low_res.png"
                                                alt="Okta encountered a network error"
                                                style={{
                                                    py: 'auto',
                                                }}
                                            />
                                            <img
                                                src="mascot/floor.png"
                                                alt="Floor"
                                                style={{
                                                    position: 'absolute',
                                                    bottom: 0,
                                                    left: 0,
                                                    width: '100%',
                                                    height: 'auto',
                                                }}
                                            />
                                        </Box>
                                        <Box
                                            sx={{
                                                display: 'flex',
                                                flexDirection: 'column',
                                                alignItems: 'center',
                                                justifyContent: 'center',
                                                textAlign: 'center',
                                            }}
                                        >
                                            <Typography variant="body1">
                                                {`Oops!`}
                                            </Typography>
                                            <Typography variant="body2">
                                                {`Too many requests. ${MASCOT_NAME} is patiently untangling things.`}
                                            </Typography>
                                            <Typography variant="body2">
                                                {`This will take just a few minutes`}
                                            </Typography>
                                        </Box>
                                    </Box>
                                </Paper>
                            </Popover>
                        </>
                    }
                </Box>
                {
                    isMac !== true && (
                        <ButtonGroup
                            disableElevation
                            variant="text"
                            size="small"
                            aria-label="Toolbar buttons"
                            sx={{
                                mt: 0.25,
                                mr: 0.375,
                                height: 25,
                            }}
                        >
                            <Button
                                onClick={async () => await getCurrentWindow().minimize()}
                                sx={{
                                    color: theme?.palette?.text?.primary || '#E7E3FCDE',
                                    borderRadius: `${(theme?.shape?.borderRadius || 9) - 4}px 0px 0px ${(theme?.shape?.borderRadius || 9) - 4}px`,
                                }}
                            >
                                <MinimizeIcon />
                            </Button>
                            <Button
                                onClick={async () => await getCurrentWindow().toggleMaximize()}
                                sx={{
                                    color: theme?.palette?.text?.primary || '#E7E3FCDE'
                                }}
                            >
                                <CropSquareIcon />
                            </Button>
                            <Button
                                onClick={async () => {
                                    if (minimizeToTray) {
                                        await getCurrentWindow().hide();
                                        return;
                                    }

                                    await getCurrentWindow().close();
                                }}
                                sx={{
                                    color: theme?.palette?.text?.primary || '#E7E3FCDE',
                                    borderRadius: `0px ${(theme?.shape?.borderRadius || 9) - 4}px ${(theme?.shape?.borderRadius || 9) - 4}px 0px`,
                                }}
                            >
                                <CloseIcon />
                            </Button>
                        </ButtonGroup>
                    )
                }
            </Box>
        </Box>
    );
}

export default CustomToolbar;