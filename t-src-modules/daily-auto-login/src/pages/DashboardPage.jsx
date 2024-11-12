import { Box, Typography } from '@mui/material';
import { useTheme } from '@emotion/react';
import { useGroups, GroupUI } from 'eam-commons-js';
import { useContext, useEffect } from 'react';
import WorkerContext from '../contexts/WorkerContext';

function DashboardPage() {

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