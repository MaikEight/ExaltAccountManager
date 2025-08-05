import ComponentBox from './ComponentBox';
import { Box, Typography } from '@mui/material';
import StyledButton from './StyledButton';
import InfoOutlinedIcon from '@mui/icons-material/InfoOutlined';
import DownloadingOutlinedIcon from '@mui/icons-material/DownloadingOutlined';
import { useEffect, useState } from 'react';
import useSnack from '../hooks/useSnack';
import { invoke } from '@tauri-apps/api/core';
import useHWID from '../hooks/useHWID';

function HwidTool({ runHwidReader }) {
    const [isLoading, setIsLoading] = useState(false);
    const { showSnackbar } = useSnack();
    const { readHwidFile } = useHWID();

    useEffect(() => {
        if (runHwidReader) {
            doHwidReading();
        }
    }, [runHwidReader]);

    const doHwidReading = () => {
        setIsLoading(true);
        invoke("download_and_run_hwid_tool")
            .then((response) => {
                if (response === true) {
                    showSnackbar('HWID successfully read', 'success');
                } else {
                    showSnackbar('Failed to read HWID', 'error');
                }
            })
            .catch((error) => {
                setIsLoading(false);
                console.error('Failed to read HWID', error);
                showSnackbar('Failed to read HWID', 'error');
            })
            .finally(() => {
                setIsLoading(false);
                readHwidFile().catch(console.error);
            });
    }

    return (
        <Box
            sx={{
                minHeight: '240px',
            }}
        >
            <ComponentBox
                title={
                    <Typography variant="h6" component="div" sx={{ textAlign: 'center' }}>
                        HWID-Reader
                    </Typography>
                }
                icon={<InfoOutlinedIcon />}
                sx={{
                    m: 0,
                    minHeight: '240px',
                }}
                isLoading={isLoading}
            >
                <Box
                    sx={{
                        display: "flex",
                        flexDirection: "column",
                        justifyContent: "space-between",
                        height: "100%",
                        minHeight: "169px",
                    }}
                >
                    <Box
                        sx={{
                            display: "flex",
                            flexDirection: "column",
                            height: "100%"
                        }}
                    >
                        <Typography variant="body2" color="text.secondary" sx={{ mb: 2 }}>
                            If you encounter the <strong>Token for different machine</strong> error, consider using the HWID-Reader.
                            This tool provides a more accurate HWID for use in EAM.
                        </Typography>
                        <Typography variant="body2" color="text.secondary" fontWeight={700}>
                            Note
                        </Typography>
                        <Typography variant="body2" color="text.secondary">
                            The HWID-Reader only needs to be executed once, or after you made any changes to your PC hardware.
                        </Typography>
                    </Box>
                    <Box
                        sx={{
                            display: "flex",
                            flexDirection: "column",
                            height: "100%"
                        }}
                    >
                        <StyledButton
                            fullWidth={true}
                            startIcon={<DownloadingOutlinedIcon />}
                            color="primary"
                            disabled={isLoading}
                            onClick={doHwidReading}
                        >
                            {!isLoading ? "Run HWID Reader" : "Doing magic..."}
                        </StyledButton>
                    </Box>
                </Box>
            </ComponentBox>
        </Box>
    );
}

export default HwidTool;