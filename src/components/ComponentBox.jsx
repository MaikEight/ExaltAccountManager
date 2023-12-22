import { useTheme } from "@emotion/react";
import { Box, LinearProgress, Paper, Typography } from "@mui/material";
function ComponentBox({ children, isLoading, headline, icon, sx, innerSx }) {
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
                (icon || headline) &&
                <Box
                    sx={{
                        display: "flex",
                        flexDirection: "row",
                        alignItems: "center",
                        justifyContent: "start",
                        mb: 2,
                        gap: 1,
                    }}
                >
                    {icon && icon}
                    {
                        headline &&
                        <Typography variant="h6" sx={{ fontWeight: 600, textAlign: 'center' }}>
                            {headline}
                        </Typography>
                    }
                </Box>
            }
            <Box sx={innerSx}>
                {children}
            </Box>
        </Paper>
    );
}

export default ComponentBox