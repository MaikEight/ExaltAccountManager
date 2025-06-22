import { Box, Button, ButtonGroup, Chip, Tooltip, Typography } from "@mui/material";
import { getCurrentWindow } from '@tauri-apps/api/window'
import MinimizeIcon from '@mui/icons-material/Minimize';
import CloseIcon from '@mui/icons-material/Close';
import CropSquareIcon from '@mui/icons-material/CropSquare';
import { useTheme } from "@emotion/react";
import { listen } from '@tauri-apps/api/event';
import { useEffect, useState } from "react";
import VpnLockOutlinedIcon from '@mui/icons-material/VpnLockOutlined';
import FlagCircleOutlinedIcon from '@mui/icons-material/FlagCircleOutlined';

function CustomToolbar(props) {
    const theme = useTheme();
    const [hasGlobalApiCooldown, setHasGlobalApiCooldown] = useState(false);
    const [apiRemainingLimits, setApiRemainingLimits] = useState(new Map([
        ['account/verify', 30],
        ['char/list', 5],
        ['account/register', 5],
    ]));

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
                    <Tooltip
                        title={"The API is on cooldown, please wait a moment."}
                        component="span"
                        sx={{
                            zIndex: 1001,
                        }}
                    >
                        <Chip
                            variant="outlined"
                            label={"API Cooldown"}
                            size="small"
                            color={"error"}
                            icon={<VpnLockOutlinedIcon sx={{ pl: '2px' }} />}
                            onClick={() => null}
                        />
                    </Tooltip>
                }
            </Box>
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
                        color: theme.palette.text.primary,
                    }}
                >
                    <MinimizeIcon />
                </Button>
                <Button
                    onClick={async () => await getCurrentWindow().toggleMaximize()}
                    sx={{
                        color: theme.palette.text.primary
                    }}
                >
                    <CropSquareIcon />
                </Button>
                <Button
                    onClick={async () => await getCurrentWindow().close()}
                    sx={{
                        color: theme.palette.text.primary,
                        borderRadius: `0px ${theme.shape.borderRadius - 2}px ${theme.shape.borderRadius - 2}px 0px`,
                    }}
                >
                    <CloseIcon />
                </Button>
            </ButtonGroup>
        </Box>
    );
}

export default CustomToolbar;