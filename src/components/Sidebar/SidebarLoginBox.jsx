import { Box } from '@mui/material';
import { useState } from 'react';
import GradientBorderButton from './ButtonParts/GradientBorderButton';
import GradientBorderButtonInner from './ButtonParts/GradientBorderButtonInner';
import AccountCircleIcon from '@mui/icons-material/AccountCircle';
import { useTheme } from 'styled-components';
import StyledButton from '../StyledButton';
import { Typography } from '@mui/material';
import { useNavigate } from 'react-router-dom';
import { useAuth0 } from "@auth0/auth0-react";

function SidebarLoginBox() {
    const theme = useTheme();
    const navigate = useNavigate();
    const { user, isAuthenticated } = useAuth0();

    return (
        <Box
            sx={{
                width: 188,
                borderRadius: `${theme.shape.borderRadius}px`,
                overflow: 'hidden',
            }}
        >
            <GradientBorderButton
                sx={{
                    transition: theme.transitions.create('background-color'),
                    ...(isAuthenticated && {
                        cursor: 'default',
                        borderRadius: `${theme.shape.borderRadius}px`,
                    }),
                }}
            >
                <GradientBorderButtonInner
                    sx={{
                        transition: theme.transitions.create('background-color'),

                        ...(isAuthenticated && {
                            borderRadius: `${theme.shape.borderRadius - 2}px`,
                            backgroundColor: theme.palette.background.default,
                            '&:hover': {
                                background: theme.palette.background.default,
                            }
                        }),
                    }}
                >
                    {
                        isAuthenticated ?
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
                                    {
                                        user &&
                                        <>
                                            <Typography variant="body1">
                                                Hello <strong>{user.name}</strong>!
                                            </Typography>
                                            <Typography variant="body2">
                                                EAM Default User
                                            </Typography>
                                        </>
                                    }
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

                                onClick={() => {
                                    navigate('/profile');
                                }}
                            >
                                <AccountCircleIcon />
                                <Typography variant="body1">
                                    EAM Profile
                                </Typography>
                            </Box>
                    }
                </GradientBorderButtonInner>
            </GradientBorderButton>
        </Box>
    );
}

export default SidebarLoginBox;