import { Box, Typography } from "@mui/material";
import { useEffect, useState } from "react";
import Confetti from "react-confetti";
import { EAM_PLUS_PURCHASE_SUCCESS_MESSAGES, MASCOT_NAME } from "../constants";
import { useRef } from "react";

function PaymentSuccessful() {
    const [windowSize, setWindowSize] = useState({
        width: window.innerWidth,
        height: window.innerHeight,
    });

    const textToDisplay = useRef(
        EAM_PLUS_PURCHASE_SUCCESS_MESSAGES[Math.floor(Math.random() * EAM_PLUS_PURCHASE_SUCCESS_MESSAGES.length)]
    );

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
                }}
            >
                <Typography>Thank you for your purchase and support!</Typography>
                <Typography>Enjoy the additional features and benefits of EAM Plus.</Typography>
                <img
                    src="/mascot/Happy/cheer_very_low_res.png"
                    alt={`${MASCOT_NAME} is happy to see you using EAM Plus!`}
                    style={{
                        marginTop: '24px',
                    }}
                />
                <Typography sx={{ marginTop: '16px' }}>
                    {textToDisplay?.current}
                </Typography>
            </Box>
        </Box>
    );
}

export default PaymentSuccessful;