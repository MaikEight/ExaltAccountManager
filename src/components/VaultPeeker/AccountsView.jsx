import AccountView from "./AccountView";
import useVaultPeeker from "../../hooks/useVaultPeeker";
import ComponentBox from "../ComponentBox";
import { Fragment, useMemo } from "react";

function AccountsView() {
    const { accountsOfCurrentPage, filter } = useVaultPeeker();

    const accountsUi = useMemo(() => {
        return (
            <Fragment>
                {
                    accountsOfCurrentPage ?
                        accountsOfCurrentPage.map((accountData, index) => {
                            return <AccountView key={accountData.email || index} account={accountData} />
                        })
                        :
                        <ComponentBox title='No Accounts Found' isCollapseable={true} sx={{ mx: 0 }} />
                }
            </Fragment>
        );
    }, [accountsOfCurrentPage, filter]);

    return accountsUi;
}

export default AccountsView;