import { Box, Typography, Skeleton } from "@mui/material";
import ComponentBox from "../components/ComponentBox";
import AccountCircleIcon from '@mui/icons-material/AccountCircle';
import StyledButton from "../components/StyledButton";
import LoginOutlinedIcon from '@mui/icons-material/LoginOutlined';
import { useState, useEffect } from "react";
import { useTheme } from "@emotion/react";
import LogoutOutlinedIcon from '@mui/icons-material/LogoutOutlined';
import { useUserLogin, getProfileImage } from "eam-commons-js";
import EamPlusComparisonTable from "../components/PlusPage/EamPlusComparisonTable";
import ManageAccountsOutlinedIcon from '@mui/icons-material/ManageAccountsOutlined';
import { CACHE_PREFIX, STRIPE_CUSTOMER_PORTAL_URL } from "../constants";
import ProfilePlanChip from './../components/ProfilePlanChip';
import EamPlusFAQ from "../components/PlusPage/EamPlusFAQ";

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
            <EamPlusFAQ />
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
            const cacheKey = `${CACHE_PREFIX}profileImage`;
            const storeImageString = localStorage.getItem(cacheKey);
            if (storeImageString !== null) {
                const storageImageData = JSON.parse(storeImageString);

                if (storageImageData !== null && storageImageData.time) {
                    const cachedTime = storageImageData.time;
                    const currentTime = new Date().getTime();
                    const timeDiff = currentTime - cachedTime;
                    const maxCacheDuration = 1000 * 60 * 60 * 24 * 3; // 3 days
                    if (timeDiff < maxCacheDuration) {
                        setProfileImage(storageImageData.image);
                        return;
                    }

                    localStorage.removeItem(cacheKey);
                }
            }
            const imageData = await getProfileImage(user.picture)
                .catch((error) => {
                    console.error('Error fetching image data:', error);
                    return null;
                });

            if (imageData === null) {
                return;
            }
            setProfileImage(`data:image/jpeg;base64,${imageData}`);

            const imageObject = {
                image: `data:image/jpeg;base64,${imageData}`,
                time: new Date().getTime(),
            };
            localStorage.setItem(cacheKey, JSON.stringify(imageObject));
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