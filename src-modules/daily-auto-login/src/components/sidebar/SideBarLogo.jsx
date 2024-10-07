import { useTheme } from "@emotion/react";
import { Box, Typography } from "@mui/material"
import CalendarMonthOutlinedIcon from '@mui/icons-material/CalendarMonthOutlined';

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
                paddingLeft: 1.5,
                gap: 0.5,
                zIndex: 999999,
            }}
        >
            <Box
                sx={{
                    position: "relative",
                }}
            >
                <img
                    data-tauri-drag-region
                    src={theme.palette.mode === 'dark' ? '/logo/logo_inner_big_dl.png' : '/logo/logo_inner_big_dl_dark.png'}
                    alt="EAM-Logo"
                    style={{
                        width: 64,
                        height: 64,
                        userSelect: "none",
                    }}
                    draggable="false"
                    onContextMenu={handleContextMenu}
                />
            </Box>
            <Typography
                data-tauri-drag-region
                variant="p"
                fontWeight={'bold'}
                component="span"
                sx={{
                    ml: 1.5,
                    textAlign: "center",
                    userSelect: "none",
                    whiteSpace: "pre-line",
                    '& > *': {
                        display: 'block',
                    },
                }}
            >
                {'E A M,Daily Login'.split(',').map(word => (
                    <span key={word}>{word}</span>
                ))}
            </Typography>
        </Box>
    )
}

export default SideBarLogo