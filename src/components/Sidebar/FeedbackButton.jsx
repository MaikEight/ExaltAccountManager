import { Box } from "@mui/material";
import FavoriteBorderOutlinedIcon from '@mui/icons-material/FavoriteBorderOutlined';
import { useNavigate } from "react-router-dom";
import { useState, useEffect } from "react";
import GradientBorderButton from "./ButtonParts/GradientBorderButton";
import GradientBorderButtonInner from "./ButtonParts/GradientBorderButtonInner";

function FeedbackButton({ smallSize, action }) {
    const selected = window.location.pathname === '/feedback';
    const navigate = useNavigate();
    const [showText, setShowText] = useState(true);

    useEffect(() => {

        const timeOut = setTimeout(() => {
            setShowText(smallSize);
        }, smallSize ? 25 : 150);

        return () => {
            clearTimeout(timeOut);
        };
    }, [smallSize]);

    return (
        <Box
            sx={{
                width: smallSize ? 44 : 122,
                transition: theme => theme.transitions.create('width'),
            }}
        >
            <GradientBorderButton
                selected={selected}
                onClick={() => {
                    navigate('/feedback');
                    action?.();
                }}
            >
                <GradientBorderButtonInner
                    selected={selected}
                    sx={{
                        height: '36px'
                    }}
                >
                    <Box
                        sx={{
                            display: 'flex',
                            gap: '8px',
                            alignItems: 'center',
                            justifyContent: 'center',
                            mx: 0.25,
                        }}
                    >
                        <FavoriteBorderOutlinedIcon />
                        {!showText && 'Feedback'}
                    </Box>
                </GradientBorderButtonInner>
            </GradientBorderButton>
        </Box>
    );
}

export default FeedbackButton;