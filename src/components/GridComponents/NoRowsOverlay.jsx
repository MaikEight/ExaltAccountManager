import { Box, Typography } from "@mui/material";
import StyledButton from "../StyledButton";
import AddCircleIcon from '@mui/icons-material/AddCircle';
import useAccounts from "../../hooks/useAccounts";
import { MASCOT_NAME, NO_ACCOUNTS_FOUND_TEXTS } from "../../constants";
import { useRef } from "react";

function NoRowsOverlay({ onAddNew }) {
    const { accounts } = useAccounts();
    const textToDisplay = useRef(
        NO_ACCOUNTS_FOUND_TEXTS[Math.floor(Math.random() * NO_ACCOUNTS_FOUND_TEXTS.length)]
    );
    const imageRef = useRef(Math.random() < 0.5 ? '/mascot/Search/no_accounts_1_small_very_low_res.png' : '/mascot/Search/no_accounts_2_very_low_res.png');

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
                    <img
                        src={imageRef?.current ? imageRef.current : "/mascot/Search/no_accounts_1_small_very_low_res.png"}
                        alt="No accounts found"
                        style={{
                            width: '100%',
                            height: 'auto',
                            maxHeight: '100%',
                        }}
                    />
                </Box>
                <Typography variant="h6">
                    {accounts.length === 0 ? 'No accounts found' : textToDisplay?.current ? textToDisplay?.current : `${MASCOT_NAME} could not find anything here`}
                </Typography>
                {
                    accounts.length === 0 &&
                    <StyledButton
                        sx={{ mt: 2 }}
                        startIcon={<AddCircleIcon />}
                        onClick={() => onAddNew?.()}
                        color="primary"
                        variant="contained"
                    >
                        Add your first one here
                    </StyledButton>
                }
            </Box>
        </Box>
    );
}

export default NoRowsOverlay;

