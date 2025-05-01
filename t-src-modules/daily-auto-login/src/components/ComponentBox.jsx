import { useTheme } from "@emotion/react";
import { Box, Collapse, IconButton, LinearProgress, Paper, Typography } from "@mui/material";
import { useEffect, useState } from "react";
import KeyboardArrowLeftIcon from '@mui/icons-material/KeyboardArrowLeft';

function ComponentBox({ children, isLoading, title, icon, fullwidth, isCollapseable, defaultCollapsed = false, setIsCurrentlyCollapsed, sx, innerSx }) {
    const [isCollapsed, setIsCollapsed] = useState(defaultCollapsed);
    const theme = useTheme();

    useEffect(() => {
        if (setIsCurrentlyCollapsed !== undefined) {
            setIsCurrentlyCollapsed(isCollapsed);
        }
    }, [isCollapsed]);

    return (
        <Paper
            sx={{
                position: "relative",
                display: "flex",
                flexDirection: "column",
                borderRadius: `${theme.shape.borderRadius}px`,
                paddingLeft: 1.5,
                paddingRight: 1.5,
                paddingTop: 1.5,
                paddingBottom: 1.5,
                m: 2,
                background: theme.palette.background.paper,
                transition: theme.transitions.create('background-color'),
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
                        right: 0,
                        height: '6px',
                        borderRadius: `${theme.shape.borderRadius}px ${theme.shape.borderRadius}px 0 0`,
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
                        (typeof title === 'string' ?
                            <Typography
                                variant="h6"
                                sx={{
                                    fontWeight: 600,
                                    textAlign: 'center',
                                }}
                            >
                                {title}
                            </Typography>
                            :
                            title
                        )
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
                <Box sx={innerSx ? { ...innerSx} : { }}>
                    {children}
                </Box>
            </Collapse>
        </Paper>
    );
}

export default ComponentBox