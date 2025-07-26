import { Box, Typography } from "@mui/material";
import { useEffect, useState } from "react";
import Confetti from "react-confetti";
import { EAM_PLUS_PURCHASE_SUCCESS_MESSAGES, MASCOT_NAME } from "../constants";
import { useRef } from "react";

function PaymentSuccessful() {


    const textToDisplay = useRef(
        EAM_PLUS_PURCHASE_SUCCESS_MESSAGES[Math.floor(Math.random() * EAM_PLUS_PURCHASE_SUCCESS_MESSAGES.length)]
    );

    const [windowSize, setWindowSize] = useState({
        width: window.innerWidth,
        height: window.innerHeight,
    });

    useEffect(() => {
        const handleResize = () => {
            setWindowSize({
                width: window.innerWidth,
                height: window.innerHeight,
            });
        };

        window.addEventListener("resize", handleResize);
        return () => window.removeEventListener("resize", handleResize);
    }, []);

    return (
        <Box
            sx={{
                width: '100%',
                height: '100%',
                display: 'flex',
                flexDirection: 'column',
                overflow: 'auto',
                alignItems: 'center',
            }}
        >
            <Confetti
                width={windowSize.width}
                height={windowSize.height}
                style={{ zIndex: 0 }}
            />
            <h1>🥳 EAM Plus Successfully activated 🎉</h1>
            <Box
                sx={{
                    display: 'flex',
                    flexDirection: 'column',
                    gap: 0,
                    alignItems: 'center',
                    justifyContent: 'center',
                    p: 0,
                    zIndex: 1
                }}
            >
                <Typography>Thank you for your purchase and support!</Typography>
                <Typography>Enjoy the additional features and benefits of EAM Plus.</Typography>
            </Box>
            <Box
                sx={{
                    display: 'flex',
                    flexDirection: 'column',
                    alignItems: 'center',
                    justifyContent: 'center',
                    width: '100%',
                    my: 'auto',
                    zIndex: 1
                }}
            >
                <img
                    src="/mascot/Happy/cheer_very_low_res.png"
                    alt={`${MASCOT_NAME} is happy to see you using EAM Plus!`}
                    style={{
                        marginTop: '24px',
                    }}
                />
                <Typography sx={{ marginTop: 2 }}>
                    {textToDisplay?.current}
                </Typography>
            </Box>
            <Typography sx={{ mb: 2 }}>
                If your purchase does not activate right away, please try to check for Plus subscription on the profile page.
            </Typography>
        </Box>
    );
}

export default PaymentSuccessful;