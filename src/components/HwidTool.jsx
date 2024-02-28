import ComponentBox from './ComponentBox';
import { Box, Typography } from '@mui/material';
import StyledButton from './StyledButton';
import { tauri } from '@tauri-apps/api';
import InfoOutlinedIcon from '@mui/icons-material/InfoOutlined';
import DownloadingOutlinedIcon from '@mui/icons-material/DownloadingOutlined';
import { useState } from 'react';
import useSnack from '../hooks/useSnack';

function HwidTool() {
    const [isLoading, setIsLoading] = useState(false);
    const { showSnackbar } = useSnack();

    return (
        <Box
            sx={{
                minHeight: '240px',
            }}
        >
            <ComponentBox
                title={
                    <Typography variant="h6" component="div" sx={{ textAlign: 'center' }}>
                        HWID-Tool
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
                            If you encounter the <strong>Token for different machine</strong> error, consider using the HWID Tool.
                            This tool provides a more accurate HWID for use in EAM.
                        </Typography>
                        <Typography variant="body2" color="text.secondary" fontWeight={700}>
                            Note
                        </Typography>
                        <Typography variant="body2" color="text.secondary">
                            The HWID-Tool only needs to be executed once, or after any changes to your PC hardware.
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
                            onClick={() => {
                                setIsLoading(true);
                                tauri.invoke("download_and_run_hwid_tool")
                                    .then((response) => {
                                        setIsLoading(false);
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
                                    });
                            }}
                        >
                            {!isLoading ? "Grab & Launch HWID Tool" : "Hang tight! Doing our magic..."}
                        </StyledButton>
                    </Box>
                </Box>
            </ComponentBox>
        </Box>
    );
}

export default HwidTool;