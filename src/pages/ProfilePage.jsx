import { Box, Typography, Skeleton } from "@mui/material";
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
import { CACHE_PREFIX, STRIPE_CUSTOMER_PORTAL_URL } from "../constants";
import ProfilePlanChip from './../components/ProfilePlanChip';
import EngineeringIcon from '@mui/icons-material/Engineering';
import AutoAwesomeOutlinedIcon from '@mui/icons-material/AutoAwesomeOutlined';
import { useNavigate } from "react-router-dom";

function ProfilePage() {
    const { isAuthenticated } = useUserLogin();
    const navigate = useNavigate();
    const theme = useTheme();

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
            <ComponentBox
                title="Nothing to see here just yet"
                icon={<EngineeringIcon />}
                sx={{ mb: 0 }}
                innerSx={{
                    display: 'flex',
                    flexDirection: 'column',
                    justifyContent: 'center',
                    gap: 1,
                }}
            >
                <Typography variant="body1">
                    If you choose to login, you will be able to get verified in the Discord server.
                </Typography>
                <Typography variant="body2">
                    The verification process should be automatic, but in case it does not work, please use the <code>/verify</code> command in the server to get verified.
                </Typography>
                <Typography variant="body1">
                    There is also a hidden role, can you find it? 😉
                </Typography>
            </ComponentBox>
            <ComponentBox
                title="Thank you for your testing!"
                icon={<AutoAwesomeOutlinedIcon />}
            >
                <Typography variant="body1">
                    Thank you for testing the new EAM features!
                </Typography>
                <Typography variant="body1">
                    We are working hard to polish them.
                </Typography>
                <Typography variant="body1">
                    If you have any feedback, please let us know via the
                    <span
                        style={{
                            cursor: 'pointer',
                            textDecoration: 'underline',
                            color: theme.palette.mode === 'dark' ? '#9E9EFF' : '#0000EE',
                            paddingLeft: '0.25rem',
                            paddingRight: '0.25rem'
                        }}
                        onClick={() => navigate("/feedback")}
                    >
                        Feedback page
                    </span>
                    or write us directly in the
                    <a
                        style={{
                            paddingLeft: '0.25rem',
                        }}
                        href="https://discord.exalt-account-manager.eu"
                        target="_blank"
                        rel="noopener noreferrer"
                    >
                        discord
                    </a>.
                </Typography>
            </ComponentBox>
            {/* <EamPlusComparisonTable /> */}
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
                                {/* <ProfilePlanChip /> */}
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