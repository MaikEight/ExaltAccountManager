import { useContext } from "react";
import AccountsContext from "../contexts/AccountsContext";

function useAccounts() {
    return useContext(AccountsContext);
}

export default useAccounts;