import { Box, Button } from '@mui/material';
import RealmUpdater from '../components/RealmUpdater';
import HwidTool from '../components/HwidTool';
import { useSearchParams } from 'react-router-dom';
import { useEffect, useState } from 'react';
import { invoke } from '@tauri-apps/api/core';

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
        // await invoke('send_toast_notification', {
        //     title: 'Test Notification',
        //     body: 'This is a test toast notification from EAM.',
        //     heroImagePath: null,
        // })
        //     .catch((err) => {
        //         console.error('Failed to send toast notification', err);
        //     });
        await invoke('send_toast_notification', {
            title: 'Last chance for this month\'s rewards!',
            body: 'Okta checked and you still have unclaimed daily login rewards. The calendar waits for no one — claim them now!',
            //heroImagePath: "C:\\Users\\Maik8\\Pictures\\EAM\\Mascot\\Info\\Toasts\\notification.png",
            heroImagePath: "C:\\Users\\Maik8\\Pictures\\EAM\\Mascot\\Info\\Toasts\\reminder.png",
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