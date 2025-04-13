import { Box, Typography } from "@mui/material";
import NoAccountsSvg from "../Illustrations/NoAccountsSvg";

function LogsNoRowsOverlay({ text }) {
    return (
        <Box
            sx={{
                position: 'relative',
                display: 'flex',
                flexDirection: 'column',
                alignItems: 'center',
                justifyContent: 'center',
                height: '100%',
                width: '100%',
            }}
        >
            <Box
                sx={{
                    maxWidth: 'calc(min(30%, 300px))',
                }}
            >
                <NoAccountsSvg w={'100%'} h={'100%'} />
            </Box>
            <Typography variant="h6">
                {text}
            </Typography>
        </Box>
    );
}

export default LogsNoRowsOverlay;

