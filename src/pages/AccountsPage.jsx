import { Box } from "@mui/material";
import AccountGrid from "../components/AccountGrid";

function AccountsPage() {
    return (
        <Box id="accountspage"
            sx={{
                width: '100%',
                p: 2,
            }}
        >
            <AccountGrid />
        </Box>
    );
}

export default AccountsPage;