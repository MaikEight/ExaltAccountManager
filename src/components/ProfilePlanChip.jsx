import { Chip } from '@mui/material';
import AddCircleOutlineOutlinedIcon from '@mui/icons-material/AddCircleOutlineOutlined';
import { useUserLogin, useColorList } from "eam-commons-js";
import { useTheme } from '@emotion/react';

function ProfilePlanChip() {
    const { user } = useUserLogin();
    const planChipColor = useColorList(user?.isPlusUser ? 0 : 4);
    const theme = useTheme();
    
    if (!user) {
        return null;
    }

    return (
        <Chip
            label={user.subName ?? 'Default User'}
            color={user.isPlusUser ? 'primary' : 'default'}
            icon={user.isPlusUser ? <AddCircleOutlineOutlinedIcon /> : null}
            sx={{
                ...planChipColor,
                ...(user.isPlusUser && {
                    border: `1px solid ${theme.palette.primary.main}`,
                }),
            }}
            clickable={false}
            size="small"
        />
    );
}

export default ProfilePlanChip;