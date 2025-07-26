import { Chip, Box, Tooltip, Typography } from '@mui/material';
import AddCircleOutlineOutlinedIcon from '@mui/icons-material/AddCircleOutlineOutlined';
import { useUserLogin } from "eam-commons-js";
import { useTheme } from '@emotion/react';
import { useColorList } from '../hooks/useColorList';
import RefreshIcon from '@mui/icons-material/Refresh';
import { darken, lighten } from '@mui/material';
import { useState } from 'react';
import useSnack from './../hooks/useSnack';
import { MASCOT_NAME } from '../constants';

function ProfilePlanChip() {
    const { user, isPlusUser, checkForPlusUserSubscription } = useUserLogin();
    const { showSnackbar } = useSnack();
    const planChipColor = useColorList(isPlusUser ? 0 : 4);
    const theme = useTheme();
    const [isLoading, setIsLoading] = useState(false);

    if (!user) {
        return null;
    }

    return (
        <>
            <Chip
                label={user.subName ?? 'Default User'}
                color={isPlusUser ? 'primary' : 'default'}
                icon={isPlusUser ? <AddCircleOutlineOutlinedIcon /> : null}
                sx={{
                    ...planChipColor,
                    ...(isPlusUser && {
                        border: `1px solid ${theme.palette.primary.main}`,
                    }),
                }}
                clickable={false}
                size="small"
            />
            {
                !isPlusUser && (
                    <Tooltip title="Check for Plus subscription | Restore purchase">
                        <Box
                            sx={{
                                borderRadius: '50%',
                                ...planChipColor,
                                width: 24,
                                height: 24,
                                display: 'flex',
                                justifyContent: 'center',
                                alignItems: 'center',
                                cursor: isLoading ? 'not-allowed' : 'pointer',
                                '&:hover': {
                                    ...(isLoading ? {} : {
                                        backgroundColor: theme.palette.mode === 'dark' ? darken(planChipColor.background, 0.2) : lighten(planChipColor.background, 0.2),
                                        color: theme.palette.mode === 'dark' ? darken(planChipColor.color, 0.2) : lighten(planChipColor.color, 0.2),
                                    }),
                                },
                            }}
                            onClick={async () => {
                                setIsLoading(true);
                                const result = await checkForPlusUserSubscription();

                                if (sessionStorage.getItem('flag:debug') === 'true') {
                                    console.log("Plus subscription check result:", result);
                                }

                                if (result) {
                                    showSnackbar(
                                        (
                                            <Box
                                                sx={{
                                                    display: 'flex',
                                                    flexDirection: 'column',
                                                    alignItems: 'center',
                                                    justifyContent: 'center',
                                                    gap: 1,
                                                }}
                                            >
                                                <img
                                                    src="/mascot/Happy/cheer_very_low_res.png"
                                                    alt={`${MASCOT_NAME} the mascot`}
                                                    style={{ width: '120px' }}
                                                />
                                                <Typography variant="body2">
                                                    {`${MASCOT_NAME} is happy! You have an active Plus subscription.`}
                                                </Typography>
                                            </Box>
                                        ),
                                        'message',
                                        true
                                    );
                                }
                                else {
                                    showSnackbar("Checking for Plus subscription suggests that you do not have an active subscription.");
                                }

                                setIsLoading(false);
                            }}
                        >
                            <RefreshIcon
                                sx={{
                                    fontSize: 20,
                                }}
                            />
                        </Box>
                    </Tooltip>
                )
            }
        </>
    );
}

export default ProfilePlanChip;