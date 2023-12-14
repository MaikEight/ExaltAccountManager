import { useTheme } from "@emotion/react";
import { Box, Typography } from "@mui/material"

function SideBarLogo() {
    const theme = useTheme();

    const handleContextMenu = (event) => {
        event.preventDefault(); // Prevent the default context menu
    };

    return (
        <Box
            sx={{
                display: "flex",
                justifyContent: "start",
                alignItems: "center",
                height: 64,
                width: 200,
                paddingLeft: 1,
                gap: 0.5,
                zIndex: 999999,
            }}
        >
            <img
                src='/logo/logo_inner.png'
                alt="EAM-Logo"
                style={{
                    width: 64,
                    height: 64,
                    userSelect: "none",
                    ...(theme.palette.mode === 'light' ? { filter: "invert(70%)" } : {})
                }}
                draggable="false"
                onContextMenu={handleContextMenu}
            />
            <Typography
                variant="p"
                fontWeight={'bold'}
                component="span"
                sx={{
                    textAlign: "center",
                    userSelect: "none",
                }}
            >
                Exalt Account
                Manager
            </Typography>
        </Box>
    )
}

export default SideBarLogo