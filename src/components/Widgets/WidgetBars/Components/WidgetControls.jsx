import { Box, IconButton, Menu, MenuItem, Divider, Typography } from '@mui/material';
import AddIcon from '@mui/icons-material/Add';
import { useState } from 'react';
import useWidgets from '../../../../hooks/useWidgets';

/**
 * Component that provides controls for adding widgets to the widget bar
 */
function WidgetControls() {
    const { widgetBarState, addWidgetToBar, widgetBarConfig } = useWidgets();
    const [anchorEl, setAnchorEl] = useState(null);
    const open = Boolean(anchorEl);

    const handleClick = (event) => {
        setAnchorEl(event.currentTarget);
    };

    const handleClose = () => {
        setAnchorEl(null);
    };

    const handleAddWidget = (widgetType) => {
        addWidgetToBar(widgetType);
        handleClose();
    };

    // Get available widgets from the current widget bar type
    const availableWidgets = widgetBarState?.type?.availableWidgets || [];

    const activeWidgetTypes = widgetBarConfig?.activeWidgets || [];

    if(availableWidgets.length === 0
        || availableWidgets.every(widget => activeWidgetTypes.includes(widget.type))
    ) {
        return null;
    }

    return (
        <>
            <IconButton
                onClick={handleClick}
                size="small"
                sx={{
                    position: 'absolute',
                    bottom: 8,
                    right: 8,
                    bgcolor: (theme) => theme.palette.background.default,
                    border: (theme) => `1px solid ${theme.palette.divider}`,
                    zIndex: 11,
                    '&:hover': {
                        backgroundColor: (theme) => theme.palette.background.paperLight,
                        border: (theme) => `1px solid ${theme.palette.divider}`,
                    }
                }}
            >
                <AddIcon />
            </IconButton>
            <Menu
                anchorEl={anchorEl}
                open={open}
                onClose={handleClose}
                anchorOrigin={{
                    vertical: 'bottom',
                    horizontal: 'right',
                }}
                transformOrigin={{
                    vertical: 'top',
                    horizontal: 'right',
                }}
            >
                <Typography variant="caption" sx={{ px: 2, py: 1, color: 'text.secondary' }}>
                    Add Widget
                </Typography>
                <Divider />
                {availableWidgets.map((widget) => {
                    const isActive = activeWidgetTypes.includes(widget.type);
                    if (isActive) {
                        return null;
                    }

                    return (
                        <MenuItem
                            key={widget.type}
                            onClick={() => handleAddWidget(widget.type)}
                            disabled={isActive}
                        >
                            {widget.name} {isActive && '(Active)'}
                        </MenuItem>
                    );
                })}
            </Menu>
        </>
    );
}

export default WidgetControls;
