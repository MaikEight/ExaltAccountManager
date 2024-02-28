import { useTheme } from "@emotion/react";
import { Box, Collapse, IconButton, LinearProgress, Paper, Typography } from "@mui/material";
import { useState } from "react";
import KeyboardArrowLeftIcon from '@mui/icons-material/KeyboardArrowLeft';

function ComponentBox({ children, isLoading, title, icon, fullwidth, isCollapseable, defaultCollapsed = false, sx, innerSx }) {
    const [isCollapsed, setIsCollapsed] = useState(defaultCollapsed);
    const theme = useTheme();

    return (
        <Paper
            sx={{
                position: "relative",
                display: "flex",
                flexDirection: "column",
                borderRadius: 1.5,
                paddingLeft: 1.5,
                paddingRight: 1.5,
                paddingTop: 1.5,
                paddingBottom: 1.5,
                m: 2,
                background: theme.palette.background.paper,
                ...(fullwidth && { width: '100%' }),
                ...sx
            }}
        >
            {
                isLoading &&
                <LinearProgress
                    sx={{
                        position: 'absolute',
                        top: 0,
                        left: 0,
                        width: '100%',
                        borderRadius: '6px 6px 0 0',
                        zIndex: 9999
                    }}
                />
            }
            {
                (icon || title) &&
                <Box
                    sx={{
                        display: "flex",
                        flexDirection: "row",
                        alignItems: "center",
                        justifyContent: "start",
                        mb: (isCollapsed ? 0 : 2),
                        gap: 1,
                        transition: 'margin-bottom 0.2s',
                        cursor: isCollapseable ? 'pointer' : '',                        
                    }}
                    onClick={isCollapseable ? () => setIsCollapsed(!isCollapsed) : null}
                >
                    {icon && icon}
                    {
                        title &&
                        <Typography 
                            variant="h6" 
                            sx={{ 
                                fontWeight: 600, 
                                textAlign: 'center', 
                            }}
                            
                        >
                            {title}
                        </Typography>
                    }
                    {
                        isCollapseable &&
                        <IconButton
                            sx={{ 
                                marginLeft: 'auto', 
                                transition: 'transform 0.2s',
                                transform: isCollapsed ? 'rotate(0deg)' : 'rotate(90deg)',
                            }}
                            size="small"
                            onClick={() => setIsCollapsed(!isCollapsed)}
                        >
                            <KeyboardArrowLeftIcon />
                        </IconButton>
                    }
                </Box>
            }
            <Collapse in={!isCollapsed} sx={{ width: '100%' }}>
                <Box sx={innerSx}>
                    {children}
                </Box>
            </Collapse>
        </Paper>
    );
}

export default ComponentBox