import { Box } from '@mui/material';
import { useState, useEffect } from 'react';
import GradientBorderButton from './ButtonParts/GradientBorderButton';
import GradientBorderButtonInner from './ButtonParts/GradientBorderButtonInner';
import AccountCircleIcon from '@mui/icons-material/AccountCircle';
import StyledButton from '../StyledButton';
import { Typography } from '@mui/material';
import { useNavigate } from 'react-router-dom';
import { useUserLogin, getProfileImage } from "eam-commons-js";
import { useTheme } from '@mui/material/styles';

const GREETING_TEXTS = [
    'Hello, ',
    'Welcome back, ',
    'Hi, ',
    'Greetings, ',
    'Howdy, ',
    'Hey, ',
    'Good day, ',
    'Salutations, ',
    'Missed ya, ',
    'You\'re back, ',
    'Nice to see you, ',
    'Well, hello  ',
    'Hello again, ',
    'Hey there, ',
    'You\'re here, ',
    'Welcome, ',
    'Good to see you, ',
    'Hi there, ',
    'Back again,',
    'Hello once more, ',
    'The legend returns, ',
    'Party time, ',
    'There you are, ',
    'Allons-y, ',
];

function SidebarLoginBox() {
    const theme = useTheme();
    const navigate = useNavigate();
    const { user, isAuthenticated } = useUserLogin();
    const [greetingText, setGreetingText] = useState(null);
    const [profileImage, setProfileImage] = useState(null);

    const selected = window.location.pathname.startsWith('/profile');

    useEffect(() => {
        const fetchImageData = async () => {
            if (!user) {
                setGreetingText(null);
                setProfileImage(null);
                return;
            }

            const storeImage = sessionStorage.getItem('profileImage');
            if (storeImage !== null) {
                setProfileImage(storeImage);
                return;
            }

            const imageData = await getProfileImage(user.picture);
            setProfileImage(`data:image/jpeg;base64,${imageData}`);
            sessionStorage.setItem('profileImage', `data:image/jpeg;base64,${imageData}`);
        }
        fetchImageData();

        if (sessionStorage.getItem('greetingText')) {
            setGreetingText(sessionStorage.getItem('greetingText'));
        } else {
            const greeting = GREETING_TEXTS[Math.floor(Math.random() * GREETING_TEXTS.length)];
            setGreetingText(greeting);
            sessionStorage.setItem('greetingText', greeting);
        }

        const timeoutId = setTimeout(() => {
            setGreetingText(null);
        }, 15_000);

        return () => {
            clearTimeout(timeoutId);
        }
    }, [user]);

    return (
        <Box
            sx={{
                width: 188,
                borderRadius: `${theme.shape.borderRadius}px`,
                overflow: 'hidden',
            }}
        >
            <GradientBorderButton
                selected={!greetingText && selected}
                sx={{
                    transition: theme.transitions.create('background-color'),
                    ...(isAuthenticated && user && greetingText && {
                        cursor: 'default',
                        borderRadius: `${theme.shape.borderRadius}px`,
                    }),
                }}
                onClick={
                    (isAuthenticated &&
                        user &&
                        greetingText)
                        ?
                        null
                        :
                        () => {
                            navigate('/profile');
                        }
                }
            >
                <GradientBorderButtonInner
                    selected={!greetingText && selected}
                    sx={{
                        height: 'fit-content',
                        transition: theme.transitions.create('background-color'),

                        ...(isAuthenticated && user && greetingText && {
                            borderRadius: `${theme.shape.borderRadius - 2}px`,
                            backgroundColor: theme.palette.background.default,
                            '&:hover': {
                                background: theme.palette.background.default,
                            }
                        }),

                        ...(!isAuthenticated && !selected && {
                            '@keyframes fadeInOut': {
                                '0%': { opacity: 0.95 },
                                '50%': { opacity: 1 },
                                '100%': { opacity: 0.95 },
                            },
                            animation: 'fadeInOut 5s infinite',
                        }),
                    }}
                >
                    {
                        isAuthenticated &&
                            user &&
                            greetingText
                            ?
                            <Box
                                sx={{
                                    display: 'flex',
                                    flexDirection: 'row',
                                    alignItems: 'space-between',
                                    p: 1,
                                    height: '100%',
                                    width: '100%',
                                }}
                            >
                                <Box
                                    sx={{
                                        display: 'flex',
                                        flexDirection: 'column',
                                        alignItems: 'center',
                                        justifyContent: 'center',
                                        height: '100%',
                                        width: '100%',
                                        gap: 0.25,
                                    }}
                                >
                                    <Typography
                                        textAlign={'center'}
                                        variant="body1"
                                    >
                                        {greetingText} <strong>{user.name}</strong>!
                                    </Typography>
                                    <Box
                                        sx={{
                                            display: 'flex',
                                            flexDirection: 'row',
                                            alignItems: 'center',
                                            justifyContent: 'center',
                                            width: '100%',
                                            mt: 1,
                                        }}
                                    >
                                        <StyledButton
                                            variant="text"
                                            fullWidth
                                            sx={{ boxShadow: 'none' }}
                                            onClick={() => {
                                                navigate('/profile');
                                            }}
                                        >
                                            View Profile
                                        </StyledButton>
                                    </Box>
                                </Box>
                            </Box>
                            :
                            <Box
                                sx={{
                                    display: 'flex',
                                    flexDirection: 'row',
                                    alignItems: 'center',
                                    justifyContent: 'center',
                                    height: '100%',
                                    gap: 1,
                                }}
                            >
                                {user && profileImage ? <img width={20} src={profileImage} style={{ borderRadius: `50%` }} /> : <AccountCircleIcon />}
                                <Typography variant="body1">
                                    {user ? 'View Profile' : 'EAM Profile'}
                                </Typography>
                            </Box>
                    }
                </GradientBorderButtonInner>
            </GradientBorderButton>
        </Box>
    );
}

export default SidebarLoginBox;