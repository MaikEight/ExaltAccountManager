import { Box, Button } from '@mui/material';
import RealmUpdater from '../components/RealmUpdater';
import HwidTool from '../components/HwidTool';
import { useSearchParams } from 'react-router-dom';
import { useEffect, useState } from 'react';
import { invoke } from '@tauri-apps/api/core';
import { resolveResource } from '@tauri-apps/api/path';

function UtilitiesPage() {
    const [searchParams] = useSearchParams();
    const [runHwidReader, setRunHwidReader] = useState(false);

    useEffect(() => {
        const runHwidReader = searchParams.get('runHwidReader');
        if (runHwidReader === 'true') {
            setRunHwidReader(true);
        }
    }, []);

    const handleSendToast = async () => {
        console.log('Sending test toast notification...');
        const heroImagePath = await resolveResource('mascot/Info/notification_very_low_res.png');
        await invoke('send_toast_notification', {
            title: 'Last chance for this month\'s rewards!',
            body: 'Okta checked and you still have unclaimed daily login rewards. The calendar waits for no one — claim them now!',
            heroImagePath,
            actions: [
                {
                    label: 'Claim Now',
                    action_url: 'eam://accounts'
                }
            ]
        })
            .catch((err) => {
                console.error('Failed to send toast notification', err);
            });
    };

    return (
        <Box
            sx={{
                display: "flex",
                flexDirection: "column",
                width: "100%",
                gap: 2,
            }}
        >
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
                    localStorage.getItem("isMacOs") !== "true" &&
                    <HwidTool runHwidReader={runHwidReader} />
                }
            </Box>
            <Box>
                <Button onClick={handleSendToast}>
                    Send toast
                </Button>
            </Box>
        </Box>
    );
}

export default UtilitiesPage;