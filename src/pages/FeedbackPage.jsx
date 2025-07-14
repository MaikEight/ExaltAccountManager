import { Box, Rating, TextField, Typography } from "@mui/material";
import ComponentBox from "../components/ComponentBox";
import FavoriteBorderOutlinedIcon from '@mui/icons-material/FavoriteBorderOutlined';
import FavoriteIcon from '@mui/icons-material/Favorite';
import FavoriteBorderIcon from '@mui/icons-material/FavoriteBorder';
import { useEffect, useState } from "react";
import { styled } from "@mui/material/styles";
import InsertCommentOutlinedIcon from '@mui/icons-material/InsertCommentOutlined';
import ThumbUpAltOutlinedIcon from '@mui/icons-material/ThumbUpAltOutlined';
import ThumbDownAltOutlinedIcon from '@mui/icons-material/ThumbDownAltOutlined';
import StyledButton from "../components/StyledButton";
import SendOutlinedIcon from '@mui/icons-material/SendOutlined';
import { sendFeedback } from "../backend/eamApi";
import DoneAllOutlinedIcon from '@mui/icons-material/DoneAllOutlined';
import HandymanOutlinedIcon from '@mui/icons-material/HandymanOutlined';

const StyledRating = styled(Rating)({
    '& .MuiRating-iconFilled': {
        color: '#ff6d75',
    },
    '& .MuiRating-iconHover': {
        color: '#ff3d47',
    },
});

function FeedbackPage() {
    const [isSending, setIsSending] = useState(false);
    const [didSend, setDidSend] = useState(false);
    const [value1, setValue1] = useState(0);
    const [value2, setValue2] = useState('');
    const [value3, setValue3] = useState('');
    const [value4, setValue4] = useState('');

    useEffect(() => {
        const feedbackSent = localStorage.getItem('feedbackSent');
        if (feedbackSent) {
            const date = new Date(feedbackSent);
            const now = new Date();
            const diff = now.getTime() - date.getTime();
            const hours = Math.floor(diff / 1000 / 60 / 60);
            if (hours < 3) {
                setDidSend(true);
            }
        }
    }, []);


    const formatFeedback = () => {
        const rating = value1 && value1 > 0 ? `**Rating**\n${value1} / 5` : '';
        const likes = value2 && value2.length > 2 ? `**Likes**\n${value2}` : '';
        const dislikes = value3 && value3.length > 2 ? `**Dislikes**\n${value3}` : '';
        const feature = value4 && value4.length > 2 ? `**Feature**\n${value4}` : '';

        return `${rating}${rating ? '\n\n' : ''}${likes}${likes ? '\n\n' : ''}${dislikes}${dislikes ? '\n\n' : ''}${feature}`;
    }

    const handleSubmit = async () => {
        setIsSending(true);
        const feedback = formatFeedback();
        console.log(feedback);
        await sendFeedback(feedback);
        localStorage.setItem('feedbackSent', new Date().toISOString());
        setIsSending(false);
        setDidSend(true);
    }

    if (didSend) {
        return (
            <Box
                sx={{
                    display: 'flex',
                    flexDirection: 'column',
                    width: '100%',
                    pt: 2,
                    pl: 2,
                    pr: 2
                }}
            >
                <ComponentBox
                    title="Feedback sent"
                    icon={<DoneAllOutlinedIcon />}
                    sx={{ userSelect: "none" }}
                >
                    <Typography variant="body2" color="text.secondary" sx={{
                        display: 'flex',
                        flexDirection: 'row',
                        gap: 1,
                        justifyContent: 'start',
                        alignItems: 'center'
                    }}
                    >
                        <FavoriteIcon color="error" />
                        Thank you for your feedback! We appreciate your input and will take it into consideration.
                    </Typography>
                </ComponentBox>
            </Box>
        );
    }

    return (
        <Box sx={{ width: '100%', overflow: 'auto' }}>
            <ComponentBox
                title="Share your thoughts"
                icon={<InsertCommentOutlinedIcon />}
                sx={{ userSelect: "none" }}
                innerSx={{
                    display: 'flex',
                    flexDirection: 'column',
                }}
            >
                <Typography variant="body2" color="text.secondary">
                    We value your input! Please take a moment to leave some feedback about Exalt Account Manager.
                    Your insights are crucial to us and will contribute to enhancing the software for everyone.
                </Typography>
                <Typography>
                    Whether it's a bug report, a feature request, or just a general comment, all feedback is welcome and appreciated.
                </Typography>
                <Typography>
                    Remember, no fields are required, so feel free to share your thoughts openly.
                </Typography>
            </ComponentBox>
            <ComponentBox
                title="How do you like the look and feel of the new EAM version?"
                icon={<FavoriteBorderOutlinedIcon />}
                sx={{ userSelect: "none" }}
            >
                <Box
                    sx={{
                        display: 'flex',
                        flexDirection: 'column',
                        gap: 2
                    }}
                >

                    <Typography variant="body2" color="text.secondary">
                        Simply use the rating system below to give us a quick overview of your opinion about the new version of Exalt Account Manager.
                    </Typography>
                    <Box
                        sx={{
                            alignContent: 'center',
                            justifyContent: 'start',
                            display: 'flex',
                        }}
                    >
                        <StyledRating
                            precision={0.5}
                            icon={<FavoriteIcon fontSize="inherit" />}
                            emptyIcon={<FavoriteBorderIcon fontSize="inherit" />}
                            max={5}
                            value={value1}
                            onChange={(event, newValue) => {
                                setValue1(newValue);
                            }}
                        />
                    </Box>
                </Box>
            </ComponentBox>

            <ComponentBox
                title="What do you like about EAM?"
                icon={<ThumbUpAltOutlinedIcon />}
                sx={{ userSelect: "none" }}
            >
                <Box
                    sx={{
                        display: 'flex',
                        flexDirection: 'column',
                        gap: 2
                    }}
                >
                    <Typography variant="body2" color="text.secondary">
                        We're eager to hear what you enjoy about Exalt Account Manager.
                        Whether it's the new interface, helpful features, or seamless user experience, share your thoughts with us!
                    </Typography>
                    <TextField
                        label="What do you like about EAM?"
                        multiline
                        value={value2}
                        onChange={(event) => setValue2(event.target.value)}
                        variant="outlined"
                        fullWidth
                    />
                </Box>
            </ComponentBox>
            <ComponentBox
                title="What do you dislike about EAM?"
                icon={<ThumbDownAltOutlinedIcon />}
                sx={{ userSelect: "none" }}
            >
                <Box
                    sx={{
                        display: 'flex',
                        flexDirection: 'column',
                        gap: 2
                    }}
                >
                    <Typography variant="body2" color="text.secondary">
                        We're committed to improving your experience with Exalt Account Manager.
                        Please share any aspects you'd like to see improved or features you find less satisfactory.
                    </Typography>
                    <TextField
                        label="What do you dislike about EAM?"
                        multiline
                        value={value3}
                        onChange={(event) => setValue3(event.target.value)}
                        variant="outlined"
                        fullWidth
                    />
                </Box>
            </ComponentBox>
            <ComponentBox
                title="What feature would you like to see next / be improved upon?"
                icon={<HandymanOutlinedIcon />}
                sx={{ userSelect: "none" }}
            >
                <Box
                    sx={{
                        display: 'flex',
                        flexDirection: 'column',
                        gap: 2
                    }}
                >
                    <Typography variant="body2" color="text.secondary">
                        We're constantly striving to enhance Exalt Account Manager to better meet your needs.
                        Please take a moment to share any features you'd like to see added or improved upon in the future.
                    </Typography>
                    <TextField
                        label="What feature to add / improve?"
                        multiline
                        value={value4}
                        onChange={(event) => setValue4(event.target.value)}
                        variant="outlined"
                        fullWidth
                    />
                </Box>
            </ComponentBox>
            <Box
                sx={{
                    display: 'flex',
                    flexDirection: 'column',
                    width: '100%',
                    alignItems: 'center',
                    pb: 2,
                    pl: 2,
                    pr: 2
                }}
            >
                <StyledButton
                    disabled={isSending || formatFeedback().length === 0}
                    startIcon={<SendOutlinedIcon />}
                    sx={{
                        borderRadius: "30px",
                        width: "100%",
                    }}
                    onClick={handleSubmit}
                >
                    Send Feedback
                </StyledButton>
            </Box>
        </Box>
    );
}

export default FeedbackPage;