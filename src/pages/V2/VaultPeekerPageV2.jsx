import { Box } from "@mui/material";
import { VaultPeekerContextProvider } from "../../contexts/VaultPeekerContext";
import { ItemCanvasContextProvider } from "../../contexts/ItemCanvasContext";
import usePortraitReady from "../../hooks/usePortraitReady";
import InverseBorderRadiusSpacer from "../../components/InverseBorderRadiusSpacer";

// V2 Components
import TotalsViewV2 from "../../components/VaultPeeker/V2/TotalsViewV2";
import AccountsViewV2 from "../../components/VaultPeeker/V2/AccountsViewV2";
import ItemFilterBoxV2 from "../../components/VaultPeeker/V2/ItemFilterBoxV2";
import ItemDetailPopoverV2 from "../../components/VaultPeeker/V2/ItemDetailPopoverV2";

/**
 * VaultPeekerPageV2 - Complete rewrite of Vault Peeker page
 * 
 * Features:
 * - Infinite scroll via react-virtuoso (no pagination)
 * - Canvas + CSS overlay item rendering for performance
 * - Filter presets
 * - EAM Plus export customization
 */
function VaultPeekerPageV2() {
    const isPortraitReady = usePortraitReady();

    return (
        <VaultPeekerContextProvider>
            {isPortraitReady && (
                <ItemCanvasContextProvider>
                    <Box
                        sx={{
                            width: '100%',
                            display: 'flex',
                            flexDirection: 'column',
                            minHeight: '100%',
                        }}
                    >
                        <Box
                            sx={{
                                width: '100%',
                                position: 'relative',
                                overflow: 'auto',
                                minHeight: '100%',
                            }}
                        >
                            {/* Sticky Filter Box */}
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
                                    <ItemFilterBoxV2 />
                                </Box>
                                <InverseBorderRadiusSpacer
                                    sx={{
                                        mx: 2,
                                    }}
                                />
                            </Box>

                            {/* Main Content */}
                            <Box
                                id="vault-peeker-content-root"
                                sx={{
                                    borderRadius: '9px',
                                    mx: 2,
                                    mt: 0,
                                    mb: 2,
                                    minHeight: '100%',
                                }}
                            >
                                {/* Totals View */}
                                <TotalsViewV2 />

                                {/* Accounts View (Virtuoso infinite scroll) */}
                                <AccountsViewV2 />
                            </Box>
                        </Box>

                        {/* Item Detail Popover (renders when item is selected) */}
                        <ItemDetailPopoverV2 />
                    </Box>
                </ItemCanvasContextProvider>
            )}
        </VaultPeekerContextProvider>
    );
}

export default VaultPeekerPageV2;
