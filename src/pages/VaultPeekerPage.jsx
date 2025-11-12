import { Box } from "@mui/material";
import { VaultPeekerContextProvider } from "../contexts/VaultPeekerContext";
import TotalsView from "../components/VaultPeeker/TotalsView";
import AccountsView from "../components/VaultPeeker/AccountsView";
import { ItemCanvasContextProvider } from "../contexts/ItemCanvasContext";
import ItemFilterBox from "../components/VaultPeeker/ItemFilterBox";
import InverseBorderRadiusSpacer from "../components/InverseBorderRadiusSpacer";
import usePortraitReady from "../hooks/usePortraitReady";
import AccountsPagination from "../components/VaultPeeker/AccountsPagination";

function VaultPeekerPage() {
    const isPortraitReady = usePortraitReady();

    return (
        <VaultPeekerContextProvider>
            {
                isPortraitReady &&
                <ItemCanvasContextProvider>
                    <Box
                        sx={{
                            width: '100%',
                            display: 'flex',
                            flexDirection: 'column',
                            minHeight: '100%',
                        }}
                    >
                        <Box sx={{ width: '100%', position: 'relative', overflow: 'auto', minHeight: '100%', }}>
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
                                    mb: -2,                                    
                                    minHeight: '100%',
                                }}
                            >
                                {/* Totals */}
                                <TotalsView />

                                {/* Accounts */}
                                <AccountsView />
                            </Box>

                            {/* Pagination */}
                            <AccountsPagination />
                        </Box>
                    </Box>
                </ItemCanvasContextProvider>
            }
        </VaultPeekerContextProvider>
    );
}

export default VaultPeekerPage;