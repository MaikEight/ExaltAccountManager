import { useTheme } from "@emotion/react";
import { Box, Button } from "@mui/material";
import FavoriteBorderOutlinedIcon from '@mui/icons-material/FavoriteBorderOutlined';
import { useNavigate } from "react-router-dom";
import styled from "styled-components";

const GradientBorderButton = styled(Box)(({ theme, selected }) => ({
    position: 'relative',
    color: !selected ? theme.palette.text.primary : theme.palette.mode === 'light' ? theme.palette.background.default : theme.palette.text.primary,
    textTransform: 'none',
    borderRadius: '30px',
    cursor: 'pointer',
    padding: '2px',
    background: 'linear-gradient(98deg, rgb(198, 167, 254), rgba(145, 85, 253) 94%)',
    '&:hover': {
        color: theme.palette.mode === 'light' ? theme.palette.background.default : theme.palette.text.primary,
    },
}));

const GradientBorderButtonInner = styled(Box)(({ theme, selected }) => ({
    display: 'flex',
    justifyContent: 'center',
    alignItems: 'center',
    padding: '6px',
    gap: '8px',
    borderRadius: '30px',
    background: selected ? 'linear-gradient(98deg, rgb(198, 167, 254), rgba(145, 85, 253) 94%)' : theme.palette.background.default,
    transition: 'background 0.25s',
    '&:hover': {
        background: 'linear-gradient(98deg, rgb(198, 167, 254), rgba(145, 85, 253) 94%)',
    }
}));

function FeedbackButton({ action }) {
    const selected = window.location.pathname === '/feedback';
    const navigate = useNavigate();
    const theme = useTheme();

    return (
        <GradientBorderButton
            selected={selected}
            onClick={() => {
                navigate('/feedback');
                action?.();
            }}
        >
            <GradientBorderButtonInner 
                selected={selected}
            >
                <Box
                    sx={{                        
                        display: 'flex',
                        gap: '8px',
                        alignItems: 'center',
                        justifyContent: 'center',
                        ml: 0.25,
                        mr: 0.5,
                    }}
                >
                    <FavoriteBorderOutlinedIcon />
                    Feedback
                </Box>
            </GradientBorderButtonInner>
        </GradientBorderButton>
    );
}

export default FeedbackButton;