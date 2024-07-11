import { Box } from "@mui/material";
import { VaultPeekerContextProvider } from "../contexts/VaultPeekerContext";
import TotalsView from "../components/VaultPeeker/TotalsView";
import AccountsView from "../components/VaultPeeker/AccountsView";
import { ItemCanvasContextProvider } from "../contexts/ItemCanvasContext";
import ItemFilterBox from "../components/VaultPeeker/ItemFilterBox";

function VaultPeekerPage() {    

    return (
        <VaultPeekerContextProvider>
            <ItemCanvasContextProvider>
                <Box sx={{ width: '100%', overflow: 'auto' }}>
                    <ItemFilterBox />

                    {/* Totals */}
                    <TotalsView />

                    {/* Accounts */}
                    <AccountsView />
                </Box>
            </ItemCanvasContextProvider>
        </VaultPeekerContextProvider>
    );
}

export default VaultPeekerPage;