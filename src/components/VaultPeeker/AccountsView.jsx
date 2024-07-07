import { Box } from "@mui/material";
import AccountView from "./AccountView";
import useVaultPeeker from "../../hooks/useVaultPeeker";
import ComponentBox from "../ComponentBox";

function AccountsView() {
    const { accountsData } = useVaultPeeker();

    return(
        <Box>
            {
                accountsData ?
                accountsData.map((accountData, index) => {
                    return <AccountView key={index} account={accountData} />
                })
                :
                <ComponentBox title='No Accounts Found' isCollapseable={true} />
                //TODO: Add no accounts found message
            }
        </Box>
    );
}

export default AccountsView;