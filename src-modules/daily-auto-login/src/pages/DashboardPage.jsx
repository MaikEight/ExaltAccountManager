import { Box, Typography } from '@mui/material';
import { useTheme } from '@emotion/react';

function DashboardPage() {
    const theme = useTheme();
    return (
        <Box
            sx={{
                width: '100%',
                minWidth: '100px',
                overflow: 'hidden',
                p: 2,
            }}
        >
        </Box>
    );
}

export default DashboardPage;