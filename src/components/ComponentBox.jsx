import { useTheme } from "@emotion/react";
import { Box, Paper, Typography } from "@mui/material";
function ComponentBox({ children, headline, icon, sx }) {
    const theme = useTheme();

    return (
        <Paper sx={{
            display: "flex",
            flexDirection: "column",
            flexGrow: 1,
            borderRadius: 1.5,
            paddingLeft: 1.5,
            paddingRight: 1.5,
            paddingTop: 1.5,
            paddingBottom: 1.5,
            m: 2,
            background: theme.palette.background.paper,
            ...sx

        }}>
            {
                (icon || headline) &&
                <Box 
                    sx={{
                        display: "flex",
                        flexDirection: "row",
                        alignItems: "center",
                        justifyContent: "start",
                        mb: 1,
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
            <Box>
                {children}
            </Box>
        </Paper>
    );
}

export default ComponentBox