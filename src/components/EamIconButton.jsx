import { Box, Typography } from "@mui/material";

function EamIconButton({ icon, tooltip, tooltipDirection = 'left', tooltipBackground = 'background.paperLight', onClick, sx }) {
    tooltipDirection = ['left', 'right'].includes(tooltipDirection) ? tooltipDirection : 'left';
    
    return (
        <Box
            sx={{
                position: 'relative',
                display: 'inline-flex',
                zIndex: 1,
                '&:hover': {
                    zIndex: 10,
                },
                '&:hover .tooltip-box': {
                    maxWidth: '200px',
                    opacity: 1,
                },
                '&:hover .icon-box': {
                    transform: 'scale(1.1)',
                },
                ...sx
            }}
        >
            {/* Icon - no background */}
            <Box
                className="icon-box"
                sx={{
                    display: 'flex',
                    justifyContent: 'center',
                    alignItems: 'center',
                    height: '30px',
                    width: '30px',
                    cursor: 'pointer',
                    position: 'relative',
                    zIndex: 2,
                    transition: 'transform 0.2s ease-in-out',
                    ...(!tooltip && {
                        '&:hover': {
                            backgroundColor: 'action.hover',
                            borderRadius: '50%',
                            border: '1px solid',
                            borderColor: 'divider',
                        }
                    })
                }}
                onClick={onClick}
            >
                {icon}
            </Box>
            
            {/* Tooltip box that slides out */}
            {tooltip && (
                <Box
                    className="tooltip-box"
                    onClick={onClick}
                    sx={{
                        position: 'absolute',
                        top: 0,
                        [tooltipDirection === 'left' ? 'right' : 'left']: 0,
                        height: '30px',
                        display: 'flex',
                        alignItems: 'center',
                        backgroundColor: tooltipBackground,
                        border: '1px solid',
                        borderColor: 'divider',
                        borderRadius: '15px',
                        overflow: 'hidden',
                        maxWidth: '30px',
                        opacity: 0,
                        transition: 'max-width 0.3s ease-in-out, opacity 0.2s ease-in-out',
                        cursor: 'pointer',
                        zIndex: 1,
                        [tooltipDirection === 'right' ? 'paddingLeft' : 'paddingRight']: '20px',
                        [tooltipDirection === 'left' ? 'justifyContent' : 'justifyContent']: 'flex-start',
                    }}
                >
                    <Typography
                        variant="body2"
                        sx={{
                            whiteSpace: 'nowrap',
                            px: 1.5,
                        }}
                    >
                        {tooltip}
                    </Typography>
                </Box>
            )}
        </Box>
    );
}

export default EamIconButton;