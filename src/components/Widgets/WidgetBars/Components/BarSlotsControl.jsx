import useWidgets from "../../../../hooks/useWidgets";
import { Box } from '@mui/material';
import ChevronLeftRoundedIcon from '@mui/icons-material/ChevronLeftRounded';
import ChevronRightRoundedIcon from '@mui/icons-material/ChevronRightRounded';

function BarSlotsControl() {
    const { widgetBarState, widgetBarConfig, updateWidgetBarConfig } = useWidgets();

    const updateWidgetBarSlots = (newSlots) => {
        if (!widgetBarState?.type) return;

        newSlots = Math.max(1, Math.min(newSlots, widgetBarState.type.maxSlots));
        const newConfig = {
            slots: newSlots,
        };

        updateWidgetBarConfig(newConfig);
    }

    if (!widgetBarState?.type) {
        return null;
    }

    const canGrowSlots = widgetBarConfig?.slots < widgetBarState.type.maxSlots;
    const canShrinkSlots = widgetBarConfig?.slots > 1;

    return (
        <Box
            id="barslotscontrol"
            sx={{
                position: 'absolute',
                top: '50%',
                left: '-19px',
                transform: 'translateY(-50%)',
                zIndex: 100,
                display: 'flex',
                flexDirection: 'column',
                alignItems: 'center',
                width: '20px',
                borderTop: (theme) => `1px solid ${theme.palette.divider}`,
                borderBottom: (theme) => `1px solid ${theme.palette.divider}`,
                borderLeft: (theme) => `1px solid ${theme.palette.divider}`,
                borderRadius: (theme) => `${theme.shape.borderRadius * 0.5}px 0 0 ${theme.shape.borderRadius * 0.5}px`,
                backgroundColor: 'background.default',
                overflow: 'hidden'
            }}
        >
            {
                canGrowSlots &&
                <Box
                    sx={{
                        opacity: canGrowSlots ? 1 : 0.3,
                        cursor: canGrowSlots ? 'pointer' : 'default',
                        display: 'flex',
                        alignItems: 'center',
                        justifyContent: 'center',
                        maxWidth: '100%',
                        ...(!canShrinkSlots ? {
                            py: 2.5,
                        } : {
                            height: '36px',
                        }),
                        '&:hover': {
                            ...(canGrowSlots && {
                                backgroundColor: 'action.hover',
                            })
                        }
                    }}
                    onClick={() => {
                        if (canGrowSlots) {
                            updateWidgetBarSlots(widgetBarConfig.slots + 1);
                        }
                    }}
                >
                    <ChevronLeftRoundedIcon />
                </Box>
            }
            {
                canShrinkSlots &&
                <Box
                    sx={{
                        opacity: canShrinkSlots ? 1 : 0.3,
                        cursor: canShrinkSlots ? 'pointer' : 'default',
                        display: 'flex',
                        alignItems: 'center',
                        justifyContent: 'center',
                        maxWidth: '100%',
                        ...(!canGrowSlots ? {
                            py: 2.5,
                        } : {
                            height: '36px',
                        }),
                        '&:hover': {
                            ...(canShrinkSlots && {
                                backgroundColor: 'action.hover',
                            })
                        }
                    }}
                    onClick={() => {
                        if (canShrinkSlots) {
                            updateWidgetBarSlots(widgetBarConfig.slots - 1);
                        }
                    }}
                >
                    <ChevronRightRoundedIcon />
                </Box>}
        </Box>
    );
}

export default BarSlotsControl;