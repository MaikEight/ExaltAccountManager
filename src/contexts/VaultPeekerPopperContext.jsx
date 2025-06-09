import { createContext, useState } from "react";
import ItemLocationPopper from "../components/Realm/ItemLocationPopper";
import useVaultPeeker from "../hooks/useVaultPeeker";

const VaultPeekerPopperContext = createContext();

function VaultPeekerPopperProvider({ children }) {
    const [popperPosition, setPopperPosition] = useState(null);
    const { selectedItem, setSelectedItem } = useVaultPeeker();

    const popperContextValue = {
        popperPosition,
        setPopperPosition,
    };

    return (
        <VaultPeekerPopperContext.Provider value={popperContextValue}>
            {children}
            <ItemLocationPopper
                open={Boolean(selectedItem)}
                position={popperPosition}
                onClose={() => {
                    setSelectedItem(null);
                    setPopperPosition(null);
                }}
            />
        </VaultPeekerPopperContext.Provider>
    );
}
export { VaultPeekerPopperProvider };
export default VaultPeekerPopperContext;
