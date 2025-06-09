import { useContext } from "react";
import VaultPeekerPopperContext from "../contexts/VaultPeekerPopperContext";

function useVaultPeekerPopper() {
    return useContext(VaultPeekerPopperContext);
}

export default useVaultPeekerPopper;