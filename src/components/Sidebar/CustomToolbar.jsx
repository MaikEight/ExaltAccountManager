import { Box, Button, ButtonGroup, IconButton, Typography } from "@mui/material";
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
                {/* WATERMARK */}
            {/* <Box>
                <a href="https://github.com/MaikEight/ExaltAccountManager" target="_blank" rel="noopener noreferrer">
                    <Typography
                        sx={{
                            position: "absolute",
                            top: 2,
                            left: 242,
                            color: theme.palette.text.primary,
                            fontSize: 12,
                            fontWeight: 500,
                            letterSpacing: 0.5,
                            ml: 0.5,
                            mt: 0.25,
                        }}
                    >
                        EAM DEVELOPMENT BUILD v4.0.0.4 by Maik8
                    </Typography>
                </a>
            </Box> */}

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
                        color: theme.palette.text.primary
                    }}
                >
                    <CloseIcon />
                </Button>

            </ButtonGroup>
        </Box>
    );
}

export default CustomToolbar;