
import { Box, Tooltip } from '@mui/material';
import useWidgets from '../../../hooks/useWidgets';
import DragIndicatorIcon from '@mui/icons-material/DragIndicator';
import { useSortable } from '@dnd-kit/sortable';
import { CSS } from '@dnd-kit/utilities';
import RemoveCircleOutlineRoundedIcon from '@mui/icons-material/RemoveCircleOutlineRounded';
import EamToggle from '../../EamToggle';
import ExpandRoundedIcon from '@mui/icons-material/ExpandRounded';
import UnfoldLessRoundedIcon from '@mui/icons-material/UnfoldLessRounded';
import EamIconButton from '../../EamIconButton';

function WidgetBase({ children, type, widgetId, sx }) {
    const { getWidgetConfiguration, widgetBarConfig, removeWidgetFromBar, widgetBarState, updateWidgetConfiguration } = useWidgets();

    const {
        attributes,
        listeners,
        setNodeRef,
        transform,
        transition,
        isDragging,
    } = useSortable({
        id: widgetId,
        disabled: !widgetBarState?.editMode
    });

    const config = getWidgetConfiguration(type);
    const widgetSlots = config?.slots || type?.defaultConfig?.slots || 1;
    const maxBarSlots = widgetBarConfig?.slots || 1;

    // Ensure widget doesn't span more columns than available in the bar
    const gridColumnSpan = Math.min(widgetSlots, maxBarSlots);

    const handleRemove = () => {
        removeWidgetFromBar(type.type);
    };

    const handleToggleSize = () => {
        const newSlots = widgetSlots === 1 ? type.maxSlots : 1;
        updateWidgetConfiguration(type, { slots: newSlots });
    };

    const TitleIcon = () => {
        if (!type.icon || widgetBarState?.editMode) {
            return null;
        }

        const Icon = type.icon;
        return <Icon fontSize="small" />
    }

    const style = {
        transform: CSS.Transform.toString(transform),
        transition,
    };

    return (
        <Box
            ref={setNodeRef}
            style={style}
            sx={{
                display: 'flex',
                flexDirection: 'column',
                height: 'fit-content',
                width: '100%',
                gridColumn: `span ${gridColumnSpan}`,
                border: (theme) => `1px solid ${theme.palette.divider}`,
                borderRadius: (theme) => `${theme.shape.borderRadius * 2}px`,
                p: 1,
                backgroundColor: 'background.paper',
                position: 'relative',
                opacity: isDragging ? 0.5 : 1,
                cursor: isDragging ? 'grabbing' : 'default',
                ...sx,
            }}
        >
            <Box
                sx={{
                    display: 'flex',
                    justifyContent: 'space-between',
                    alignItems: 'center',
                    mb: 1
                }}
            >
                <Box
                    sx={{
                        display: 'flex',
                        alignItems: 'center',
                    }}
                >
                    {
                        widgetBarState?.editMode &&
                        <Box
                            {...attributes}
                            {...listeners}
                            sx={{
                                display: 'flex',
                                alignItems: 'center',
                                cursor: 'grab',
                                touchAction: 'none',
                                '&:active': {
                                    cursor: 'grabbing',
                                },
                            }}
                        >
                            <DragIndicatorIcon sx={{ mr: 1 }} />
                        </Box>
                    }
                    <Box sx={{ display: 'flex', alignItems: 'center', justifyContent: 'start', gap: 1, fontWeight: 'bold' }}>
                        {TitleIcon()}
                        {type.name}
                    </Box>
                </Box>
                {/* Options */}
                <Box
                    sx={{
                        display: 'flex',
                        alignItems: 'center',
                    }}
                >
                    {
                        type.maxSlots > 1 &&
                        widgetBarState?.editMode &&
                        <Tooltip title={"Toggle widget size"}>
                            <Box>
                                <EamToggle
                                    isToggled={config?.slots === type.maxSlots || false}
                                    iconLeft={<UnfoldLessRoundedIcon fontSize="small" sx={{ rotate: '90deg' }} />}
                                    iconRight={<ExpandRoundedIcon fontSize="small" sx={{ rotate: '90deg' }} />}
                                    onToggle={handleToggleSize}
                                />
                            </Box>
                        </Tooltip>
                    }
                    <EamIconButton
                        icon={<RemoveCircleOutlineRoundedIcon fontSize="small" />}
                        tooltip={'Remove Widget'}
                        tooltipDirection='left'
                        onClick={handleRemove}
                        sx={{
                            ml: 0.5
                        }}
                    />
                </Box>
            </Box>
            {children}
        </Box>
    );
}

export default WidgetBase;