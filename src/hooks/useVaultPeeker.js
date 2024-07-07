import { useContext } from "react";
import VaultPeekerContext from "../contexts/VaultPeekerContext";

function useVaultPeeker() {
    return useContext(VaultPeekerContext);
}

export default useVaultPeeker;