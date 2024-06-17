import { useTheme } from "@emotion/react";
import { Box, Typography } from "@mui/material";

function SteamworksMailColumn(params) {
    const id = params.params.value
        && params.params.value.startsWith('steamworks:')
        ? params.params.value.split(':')[1]
        : params.params.row.email;
    const theme = useTheme();

    return (
        <Box
            sx={{
                display: 'flex',
                alignItems: 'center',
                textAlign: 'center',
                height: '100%',
            }}
        >
            <img src={theme.palette.mode === 'dark' ? "/steam.svg" : "/steam_light_mode.svg"} alt="Steam Logo" height='20px' width='20px' style={{ marginRight: '6px' }} />
            <Typography variant="body1" sx={{ fontSize: '0.875rem', fontFamily: 'Roboto', }}>
                {id}
            </Typography>
        </Box>
    );
}

export default SteamworksMailColumn;