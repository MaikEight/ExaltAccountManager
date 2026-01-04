import { useEffect, useState } from "react";
import useAccounts from "../../../hooks/useAccounts";
import { Box, Typography } from "@mui/material";
import StyledButton from "../../StyledButton";
import useSnack from "../../../hooks/useSnack";

function DeleteAccountWarning({ email, onClose, closeWidgetBar}) {
    const { getAccountByEmail, deleteAccount } = useAccounts();
    const { showSnackbar } = useSnack();
    const [account, setAccount] = useState(null);

    useEffect(() => {
        if (!email) {
            setAccount(null);
        }

        const acc = getAccountByEmail(email);
        setAccount(acc);
    }, [email])

    if (!account) {
        return null;
    }

    return (
        <Box
            sx={{
                display: 'flex',
                flexDirection: 'row',
                gap: 2,
                backgroundColor: 'background.default',
                borderRadius: 1,
                border: '1px solid',
                borderColor: 'divider',
                p: 2
            }}
        >
            <Box
                sx={{
                    display: 'flex',
                    flexDirection: 'column',
                    gap: 1,
                }}
            >
                <Typography variant="h6">
                    Delete account: {account.name || email}
                </Typography>
                <Typography variant="body1">
                    Are you sure you want to remove the account from EAM?
                </Typography>
                <Typography variant="body1" sx={{ width: '100%', textAlign: 'start' }}>
                    This action can not be undone.
                </Typography>
                <StyledButton
                    color="error"
                    onClick={async () => {
                        try {
                            await deleteAccount(email);
                            closeWidgetBar?.();
                            onClose?.();                            
                            showSnackbar("Account deleted successfully.", "success");
                        } catch (error) {
                            console.error("Error deleting account:", error);
                            showSnackbar("Failed to delete account.", "error");
                        }
                    }}
                >
                    Delete
                </StyledButton>
                <StyledButton
                    color="secondary"
                    onClick={onClose ? onClose : null}
                >
                    Cancel
                </StyledButton>
            </Box>
            <Box
                sx={{
                    display: 'flex',
                    alignItems: 'end',
                    justifyContent: 'end',
                }}
            >
                <img
                    src="/mascot/Error/error_mascot_only_2_small_very_low_res.png"
                    height={'184px'}
                    style={{ marginBottom: '-16px' }}
                />
            </Box>
        </Box>
    );
}

export default DeleteAccountWarning;