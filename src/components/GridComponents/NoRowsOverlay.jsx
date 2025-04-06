import { Box, Typography } from "@mui/material";
import NoAccountsSvg from "../Illustrations/NoAccountsSvg";
import StyledButton from "../StyledButton";
import AddCircleIcon from '@mui/icons-material/AddCircle';

function NoRowsOverlay({ onAddNew }) {
    return (
        <Box
            sx={{
                display: 'flex',
                alignItems: 'center',
                justifyContent: 'center',
                height: '100%',
                width: '100%',
            }}
        >
            <Box
                sx={{
                    position: 'relative',
                    display: 'flex',
                    flexDirection: 'column',
                    alignItems: 'center',
                    justifyContent: 'center',
                    height: '100%',
                    width: '100%',
                }}
            >
                <Box
                    sx={{
                        maxWidth: 'calc(min(30%, 300px))',
                    }}
                >
                    <NoAccountsSvg w={'100%'} h={'100%'} />
                </Box>
                <Typography variant="h6">
                    No accounts found
                </Typography>
                <StyledButton
                    sx={{ mt: 2 }}
                    startIcon={<AddCircleIcon />}
                    onClick={() => onAddNew?.()}
                    color="primary"
                    variant="contained"
                >
                    Add your first one here
                </StyledButton>
            </Box>
        </Box>
    );
}

export default NoRowsOverlay;

