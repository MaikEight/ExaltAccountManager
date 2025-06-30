import { Box, Typography } from "@mui/material";
import { useRef } from "react";

function LogsNoRowsOverlay({ text }) {

    const imageRef = useRef(Math.random() < 0.5 ? '/mascot/Search/no_accounts_1_small_very_low_res.png' : '/mascot/Search/no_accounts_2_very_low_res.png');

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
                <img
                    src={imageRef?.current ? imageRef.current : "/mascot/Search/no_accounts_1_small_very_low_res.png"}
                    alt="No accounts found"
                    style={{
                        width: '100%',
                        height: 'auto',
                        maxHeight: '100%',
                    }}

                />
            </Box>
            <Typography variant="h6">
                {text}
            </Typography>
        </Box>
    );
}

export default LogsNoRowsOverlay;

