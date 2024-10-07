import AccountView from "./AccountView";
import useVaultPeeker from "../../hooks/useVaultPeeker";
import ComponentBox from "../ComponentBox";
import { Fragment } from "react";

function AccountsView() {
    const { accountsData } = useVaultPeeker();

    return (
        <Fragment>
            {
                accountsData ?
                    accountsData.map((accountData, index) => {
                        return <AccountView key={index} account={accountData} />
                    })
                    :
                    <ComponentBox title='No Accounts Found' isCollapseable={true} sx={{ mx: 0 }} />
            }
        </Fragment>
    );
}

export default AccountsView;