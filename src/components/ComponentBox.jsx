import { useTheme } from "@emotion/react";
import { Box, Paper } from "@mui/material";

function ComponentBox({children, sx}) {
    const theme = useTheme();
    
    return (
        <Paper sx={{
            display: "flex",
            flexGrow: 1,
            borderRadius: 1.5,
            paddingLeft: 1.5,
            paddingRight: 1.5,
            m: 3,
            background: theme.palette.background.paper,
            ...sx
        
        }}>
            {children}
        </Paper>
    );
}

export default ComponentBox