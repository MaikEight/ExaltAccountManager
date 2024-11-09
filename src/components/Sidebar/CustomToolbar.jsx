import { Box, Button, ButtonGroup } from "@mui/material";
import { getCurrentWindow  } from '@tauri-apps/api/window'
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
                    mr: 0.375,
                    height: 25,
                }}
            >
                <Button
                    onClick={async () => await getCurrentWindow().minimize()}
                    sx={{
                        color: theme.palette.text.primary,
                    }}
                >
                    <MinimizeIcon />
                </Button>
                <Button
                    onClick={async () => await getCurrentWindow().maximize()}
                    sx={{
                        color: theme.palette.text.primary
                    }}
                >
                    <CropSquareIcon />
                </Button>
                <Button
                    onClick={async () => await getCurrentWindow().close()}
                    sx={{
                        color: theme.palette.text.primary,
                        borderRadius: `0px ${theme.shape.borderRadius - 2}px ${theme.shape.borderRadius - 2}px 0px`,
                    }}
                >
                    <CloseIcon />
                </Button>
            </ButtonGroup>
        </Box>
    );
}

export default CustomToolbar;