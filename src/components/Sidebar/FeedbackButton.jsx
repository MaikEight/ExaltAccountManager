import { useTheme } from "@emotion/react";
import { Button } from "@mui/material";
import InsertCommentOutlinedIcon from '@mui/icons-material/InsertCommentOutlined';
import FavoriteBorderOutlinedIcon from '@mui/icons-material/FavoriteBorderOutlined';
import { useNavigate } from "react-router-dom";

function FeedbackButton({action}) {
    const selected = window.location.pathname === '/feedback';
    const navigate = useNavigate();
    const theme = useTheme();

    return (
            <Button
                variant="contained"
                startIcon={<FavoriteBorderOutlinedIcon />}
                sx={{
                    borderRadius: "30px",
                    transition: theme.transitions.create(['color', 'background-color', 'background-image', 'box-shadow']),
                    textTransform: 'none',
                    ...(!selected ?
                        {
                            color: theme.palette.text.primary,
                            backgroundImage: 'linear-gradient(98deg, rgb(198, 167, 254), rgba(145, 85, 253) 94%)',
                            "&:hover": {
                                backgroundImage: 'linear-gradient(98deg, rgb(145, 85, 253), rgb(198, 167, 254) 94%)',
                            }
                        } : {
                            color: theme.palette.mode === 'light' ? theme.palette.background.default : theme.palette.text.primary,
                            backgroundImage: 'linear-gradient(98deg, rgb(198, 167, 254), rgb(145, 85, 253) 94%)',
                            boxShadow: theme.palette.mode === 'light'
                                ? 'rgba(58, 53, 65, 0.42) 0px 4px 8px -4px'
                                : 'rgba(19, 17, 32, 0.42) 0px 4px 8px -4px',
                            "&:hover": {
                                backgroundImage: 'linear-gradient(98deg, rgb(198, 167, 254), rgb(145, 85, 253) 94%)',
                            }
                        })
                }}
                onClick={() => {
                    navigate('/feedback');
                    action?.();
                }}
            >
                Feedback
            </Button>
    );
}

export default FeedbackButton;