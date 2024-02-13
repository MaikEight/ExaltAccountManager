import { useTheme } from "@emotion/react";
import { Box, Typography } from "@mui/material"

function SideBarLogo() {
    const theme = useTheme();

    const handleContextMenu = (event) => {
        event.preventDefault(); // Prevent the default context menu
    };

    return (
        <Box
            data-tauri-drag-region
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
                data-tauri-drag-region
                src={theme.palette.mode === 'dark' ? '/logo/logo_inner.png' : '/logo/logo_inner_dark.png'}
                alt="EAM-Logo"
                style={{
                    width: 64,
                    height: 64,
                    userSelect: "none",
                }}
                draggable="false"
                onContextMenu={handleContextMenu}
            />
            <Typography
                data-tauri-drag-region
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