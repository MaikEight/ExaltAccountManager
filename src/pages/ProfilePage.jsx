import { Box, Typography } from "@mui/material";
import ComponentBox from "../components/ComponentBox";
import AccountCircleIcon from '@mui/icons-material/AccountCircle';
import StyledButton from "../components/StyledButton";
import LoginOutlinedIcon from '@mui/icons-material/LoginOutlined';
import { useAuth0 } from "@auth0/auth0-react";
import { invoke } from "@tauri-apps/api/core";
import { useEffect } from "react";
import { useTheme } from "@emotion/react";
import LogoutOutlinedIcon from '@mui/icons-material/LogoutOutlined';
import { getProfileImageUrl } from "eam-commons-js";
import EamPlusComparisonTable from "../components/EamPlusComparisonTable";

function ProfilePage() {
    const { user, isAuthenticated } = useAuth0();

    useEffect(() => {
        console.log('isAuthenticated', isAuthenticated);
        console.log('user', user);
    }, [isAuthenticated, user]);

    return (
        <Box
            sx={{
                width: '100%',
                height: '100%',
                display: 'flex',
                flexDirection: 'column',
                overflow: 'auto',
                gap: 0
            }}
        >
            {
                isAuthenticated ?
                    <UserProfileBox />
                    :
                    <NotLoggedInBox />

            }
            <EamPlusComparisonTable />
        </Box>
    );
}

export default ProfilePage;

function UserProfileBox() {
    const { user, logout, isLoading } = useAuth0();
    const theme = useTheme();

    if (!user) {
        return null;
    }

    return (
        <ComponentBox
            title="Profile"
            icon={<AccountCircleIcon />}
            isLoading={isLoading}
            sx={{mb: 0}}
            innerSx={{
                display: 'flex',
                flexDirection: 'column',
                justifyContent: 'center',
                gap: 1.5,
                height: '100%',
            }}
        >
            {
                !isLoading &&
                <Box
                    sx={{
                        display: 'flex',
                        flexDirection: 'row',
                        gap: 1.5,
                        alignItems: 'start',
                        height: '100%',
                    }}
                >
                    {/* <img src={profileImage} alt={user.name} style={{ borderRadius: `50%`, border: `1px solid ${theme.palette.primary.main}` }} /> */}
                    <img src={getProfileImageUrl(user.picture)} alt={user.name} style={{ borderRadius: `50%`, border: `1px solid ${theme.palette.primary.main}` }} />
                    <Box
                        sx={{
                            display: 'flex',
                            flexDirection: 'column',
                            gap: 0.5,
                            justifyContent: 'space-between',
                            alignItems: 'start',
                            height: '100%',
                        }}
                    >
                        <Box
                            sx={{
                                display: 'flex',
                                flexDirection: 'column',
                                gap: 0.5,
                                justifyContent: 'start',
                            }}
                        >
                            <Typography variant="h6">
                                {user.name}
                            </Typography>
                            <Typography variant="body2">
                                {user.email}
                            </Typography>
                            <Typography variant="body1">
                                EAM Default User
                            </Typography>
                        </Box>
                        <Box
                            sx={{
                                display: 'flex',
                                flexDirection: 'row',
                                gap: 1.5,
                                alignItems: 'start',
                                justifyContent: 'end',
                            }}
                        >
                            <StyledButton
                                startIcon={<LogoutOutlinedIcon />}
                                sx={{
                                    width: "fit-content",
                                }}
                                onClick={() => logout({
                                    async openUrl(url) {
                                        invoke('open_url', { url: url });
                                    },
                                    returnTo: 'eam:profile',
                                }
                                )}
                            >
                                Log Out
                            </StyledButton>
                        </Box>
                    </Box>
                </Box>
            }
        </ComponentBox>
    );
}

function NotLoggedInBox() {
    const { loginWithRedirect, isLoading } = useAuth0();

    const login = async () => {
        await loginWithRedirect({
            async openUrl(url) {
                invoke('open_url', { url: url });
            }
        });
    };

    return (
        <ComponentBox
            title="Profile"
            icon={<AccountCircleIcon />}
            sx={{mb: 0}}
            innerSx={{
                display: 'flex',
                flexDirection: 'column',
                justifyContent: 'center',
                gap: 1.5,
            }}
            isLoading={isLoading}
        >
            <Box>
                <Typography>
                    You are not logged in.
                </Typography>
                <Typography>
                    Please log in to view your profile.
                </Typography>
            </Box>
            <StyledButton
                sx={{
                    width: "fit-content"
                }}
                startIcon={<LoginOutlinedIcon />}
                onClick={login}
            >
                Log In
            </StyledButton>
        </ComponentBox>
    );
}