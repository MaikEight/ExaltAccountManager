import { Box, Typography } from "@mui/material";
import items from "../assets/constants";
import ComponentBox from "../components/ComponentBox";
import FilterListOutlinedIcon from '@mui/icons-material/FilterListOutlined';
import Searchbar from './../components/GridComponents/Searchbar';
import SingleItemCanvas from "../components/Realm/SingleItemCanvas";
import { VaultPeekerContextProvider } from "../contexts/VaultPeekerContext";
import TotalsView from "../components/VaultPeeker/TotalsView";
import AccountsView from "../components/VaultPeeker/AccountsView";
import { ItemCanvasContextProvider } from "../contexts/ItemCanvasContext";

function VaultPeekerPage() {

    const searchChanged = (search) => {
        if (search === '') {
            setFilteredTotals(itemChunks[0]);
            return;
        }

        // const filteredItems = itemIds.filter((id) => {
        //     return items[id][0].toLowerCase().includes(search.toLowerCase());
        // });
        // setFilteredTotals(filteredItems);
    };

    return (
        <VaultPeekerContextProvider>
            <ItemCanvasContextProvider>
                <Box sx={{ width: '100%', overflow: 'auto' }}>
                    <ComponentBox
                        title={
                            <Box
                                sx={{
                                    display: 'flex',
                                    justifyContent: 'space-between',
                                    alignItems: 'center',
                                    width: '100%',
                                }}
                            >
                                <Typography
                                    variant="h6"
                                    sx={{
                                        fontWeight: 600,
                                        textAlign: 'center',
                                    }}
                                >
                                    Filter
                                </Typography>
                                <Box
                                    onClick={(event) => { event.stopPropagation(); }}
                                >
                                    <Searchbar
                                        onSearchChanged={searchChanged}
                                    />
                                </Box>
                            </Box>
                        }
                        icon={<FilterListOutlinedIcon />}
                        isCollapseable={true}
                    >
                        <SingleItemCanvas item={items[2594]} />
                    </ComponentBox>

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