import { Box } from "@mui/material";
import { VaultPeekerContextProvider } from "../contexts/VaultPeekerContext";
import TotalsView from "../components/VaultPeeker/TotalsView";
import AccountsView from "../components/VaultPeeker/AccountsView";
import { ItemCanvasContextProvider } from "../contexts/ItemCanvasContext";
import ItemFilterBox from "../components/VaultPeeker/ItemFilterBox";
import InverseBorderRadiusSpacer from "../components/InverseBorderRadiusSpacer";

function VaultPeekerPage() {

    return (
        <VaultPeekerContextProvider>

            {
                window.portraitReady &&
                <ItemCanvasContextProvider>
                    <Box
                        sx={{
                            width: '100%',
                            display: 'flex',
                            flexDirection: 'column',
                        }}
                    >
                        <Box sx={{ width: '100%', position: 'relative', overflow: 'auto' }}>
                            <Box
                                id="filter-box-root"
                                sx={{
                                    position: 'sticky',
                                    top: 0,
                                    zIndex: 10,
                                    backgroundColor: 'transparent',
                                }}
                            >
                                <Box
                                    sx={{
                                        backgroundColor: 'background.default',
                                    }}
                                >
                                    <ItemFilterBox />
                                </Box>
                                <InverseBorderRadiusSpacer
                                    sx={{
                                        mx: 2,
                                    }}
                                />
                            </Box>
                            <Box
                                id="vault-peeker-content-root"
                                sx={{
                                    borderRadius: '9px',
                                    mx: 2,
                                    mt: 0,
                                }}
                            >
                                {/* Totals */}
                                <TotalsView />

                                {/* Accounts */}
                                <AccountsView />
                            </Box>
                        </Box>
                    </Box>
                </ItemCanvasContextProvider>
            }
        </VaultPeekerContextProvider>
    );
}

export default VaultPeekerPage;