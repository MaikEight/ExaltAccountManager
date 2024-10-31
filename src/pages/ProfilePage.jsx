import { Box, Typography, Skeleton, Chip } from "@mui/material";
import ComponentBox from "../components/ComponentBox";
import AccountCircleIcon from '@mui/icons-material/AccountCircle';
import StyledButton from "../components/StyledButton";
import LoginOutlinedIcon from '@mui/icons-material/LoginOutlined';
import { useState, useEffect } from "react";
import { useTheme } from "@emotion/react";
import LogoutOutlinedIcon from '@mui/icons-material/LogoutOutlined';
import { useUserLogin, getProfileImage } from "eam-commons-js";
import EamPlusComparisonTable from "../components/EamPlusComparisonTable";
import ManageAccountsOutlinedIcon from '@mui/icons-material/ManageAccountsOutlined';
import { STRIPE_CUSTOMER_PORTAL_URL } from "../constants";
import ProfilePlanChip from './../components/ProfilePlanChip';

function ProfilePage() {
    const { isAuthenticated } = useUserLogin();    

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
    const { user, logout, isLoading } = useUserLogin();
    const [profileImage, setProfileImage] = useState(null);

    const theme = useTheme();

    useEffect(() => {
        if (!user) {
            return;
        }

        const fetchImageData = async () => {
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
    }, [user]);

    if (!user) {
        return null;
    }

    return (
        <ComponentBox
            title="Profile"
            icon={<AccountCircleIcon />}
            isLoading={isLoading}
            sx={{ mb: 0 }}
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
                    {
                        profileImage ?
                            <img src={profileImage} width={85} alt={user.name} style={{ borderRadius: `50%`, border: `1px solid ${theme.palette.primary.main}` }} />
                            :
                            <Skeleton variant="circular" width={85} height={85} animation="wave" style={{ border: `1px solid ${theme.palette.primary.main}` }} />
                    }
                    <Box
                        sx={{
                            display: 'flex',
                            flexDirection: 'column',
                            gap: 1.5,
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
                            <Typography
                                sx={{
                                    display: 'flex',
                                    flexDirection: 'row',
                                    gap: 1,
                                    alignItems: 'center',

                                }}
                                variant="h6"
                            >
                                {user.name}
                                <ProfilePlanChip />
                            </Typography>
                            <Typography variant="body2">
                                {user.email}
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
                            {
                                user.isPlusUser &&
                                <a href={`${STRIPE_CUSTOMER_PORTAL_URL}?prefilled_email=${user.email}`} target="_blank" rel="noreferrer">
                                    <StyledButton
                                        startIcon={<ManageAccountsOutlinedIcon />}
                                        color="primary"
                                        sx={{
                                            width: "fit-content",
                                        }}
                                    >
                                        Manage Subscription
                                    </StyledButton>
                                </a>
                            }
                            <StyledButton
                                startIcon={<LogoutOutlinedIcon />}
                                color="secondary"
                                sx={{
                                    width: "fit-content",
                                }}
                                onClick={() => logout()}
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
    const { login, isLoading } = useUserLogin();

    return (
        <ComponentBox
            title="Profile"
            icon={<AccountCircleIcon />}
            sx={{ mb: 0 }}
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
                disabled={isLoading}
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