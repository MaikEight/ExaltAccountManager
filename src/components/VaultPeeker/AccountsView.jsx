import AccountView from "./AccountView";
import useVaultPeeker from "../../hooks/useVaultPeeker";
import ComponentBox from "../ComponentBox";
import { Fragment, memo, useMemo } from "react";

function AccountsView() {
    const { accountsData } = useVaultPeeker();

    const accountsComponents = useMemo(() => {
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
    }, [accountsData]);

    return accountsComponents;
}

export default memo(AccountsView);