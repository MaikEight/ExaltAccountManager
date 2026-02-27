import { Box, Button } from '@mui/material';
import RealmUpdater from '../components/RealmUpdater';
import HwidTool from '../components/HwidTool';
import { useSearchParams } from 'react-router-dom';
import { useEffect, useState } from 'react';
import isMacOS from './../utils/isMacOS';

function UtilitiesPage() {
    const [searchParams] = useSearchParams();
    const [runHwidReader, setRunHwidReader] = useState(false);
    const isMac = isMacOS();
    useEffect(() => {
        const runHwidReader = searchParams.get('runHwidReader');
        if (runHwidReader === 'true') {
            setRunHwidReader(true);
        }
    }, []);

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
            {
                !isMac &&
                <HwidTool runHwidReader={runHwidReader} />
            }
        </Box>
    );
}

export default UtilitiesPage;