import { Box } from '@mui/material';
import RealmUpdater from '../components/RealmUpdater';
import HwidTool from '../components/HwidTool';

function UtilitiesPage() {

    return (
        <Box
            sx={{
                p: 2,
                display: "flex",
                flexDirection: "row",
                gap: 2,
            }}
        >
            <RealmUpdater />
            <HwidTool />
        </Box>
    );
}

export default UtilitiesPage;