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
                <Box sx={{ width: '100%', position: 'relative', overflow: 'auto' }}>
                    <Box
                        id="filter-box-root"
                        sx={{
                            position: 'sticky',
                            top: 0,
                            zIndex: 10,
                            backgroundColor: 'background.default',
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
                        <Box
                            id="filter-box-spacer"
                            sx={{
                                mx: 2,
                                position: 'relative',
                                height: theme => `${theme.spacing(2)}`,
                                backgroundColor: 'background.default',
                                borderRadius: '0 0 0 0',
                            }}
                        >
                            {/* Bottom-left-corner */}
                            <Box                            
                                sx={{
                                    position: 'absolute',
                                    backgroundColor: 'transparent',
                                    bottom: theme => `-${theme.shape.borderRadius * 2}px`,
                                    left: '0',
                                    height: theme => `${theme.shape.borderRadius * 2}px`,
                                    width: '9px',
                                    borderTopLeftRadius: theme => `${theme.shape.borderRadius}px`,
                                    boxShadow: theme => `0 -${theme.shape.borderRadius}px 0 0 ${theme.palette.background.default}`,
                                }}
                            />
                            {/* Bottom-right-corner */}
                            <Box
                                sx={{
                                    position: 'absolute',
                                    backgroundColor: 'transparent',
                                    bottom: theme => `-${theme.shape.borderRadius * 2}px`,
                                    right: '0',
                                    height: theme => `${theme.shape.borderRadius * 2}px`,
                                    width: theme => `${theme.shape.borderRadius}px`,
                                    borderTopRightRadius: theme => `${theme.shape.borderRadius}px`,
                                    boxShadow: theme => `0 -${theme.shape.borderRadius}px 0 0 ${theme.palette.background.default}`,
                                }}
                            />
                        </Box>
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
            </ItemCanvasContextProvider>
        </VaultPeekerContextProvider>
    );
}

export default VaultPeekerPage;