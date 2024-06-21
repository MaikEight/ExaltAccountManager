import { Box, Button, ButtonGroup } from "@mui/material";
import { appWindow } from '@tauri-apps/api/window'
import MinimizeIcon from '@mui/icons-material/Minimize';
import CloseIcon from '@mui/icons-material/Close';
import CropSquareIcon from '@mui/icons-material/CropSquare';
import { useTheme } from "@emotion/react";

function CustomToolbar(props) {
    const theme = useTheme();

    return (
        <Box
            sx={{
                display: "flex",
                justifyContent: "end",
                height: 30,
                ...props.sx
            }}
        >
            <ButtonGroup
                disableElevation
                variant="text"
                size="small"
                aria-label="Toolbar buttons"
                sx={{
                    mt: 0.25,
                    mr: 0.25,
                    height: 25,
                }}
            >
                <Button
                    onClick={() => appWindow.minimize()}
                    sx={{
                        color: theme.palette.text.primary,
                    }}
                >
                    <MinimizeIcon />
                </Button>
                <Button
                    onClick={() => appWindow.toggleMaximize()}
                    sx={{
                        color: theme.palette.text.primary
                    }}
                >
                    <CropSquareIcon />
                </Button>
                <Button
                    onClick={() => appWindow.close()}
                    sx={{
                        color: theme.palette.text.primary,
                        borderRadius: `0px 9px ${theme.shape.borderRadius}px 0px`,
                    }}
                >
                    <CloseIcon />
                </Button>

            </ButtonGroup>
        </Box>
    );
}

export default CustomToolbar;