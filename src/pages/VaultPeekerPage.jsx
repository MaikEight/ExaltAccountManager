import { Box, Typography } from "@mui/material";
import ItemCanvas from "../components/Realm/ItemCanvas";
import items from "../assets/constants";
import ComponentBox from "../components/ComponentBox";
import FilterListOutlinedIcon from '@mui/icons-material/FilterListOutlined';
import { useEffect, useState } from "react";
import Searchbar from './../components/GridComponents/Searchbar';
import { invoke } from "@tauri-apps/api";
import { extractRealmItemsFromCharListDatasets, formatAccountDataFromCharListDataset, formatAccountDataFromCharListDatasets } from './../utils/realmItemUtils';
import { useTheme } from "@emotion/react";
import useAccounts from "../hooks/useAccounts";
import Character from "../components/Realm/Character";

function VaultPeekerPage() {
    const [totalItems, setTotalItems] = useState([]);
    const [accountsData, setAccountsData] = useState([]);
    const [filteredTotals, setFilteredTotals] = useState([]);
    const theme = useTheme();
    const { getAccountByEmail } = useAccounts();

    useEffect(() => {
        const loadItems = async () => {
            const res = await invoke('get_latest_char_list_dataset_for_each_account');
            const items = extractRealmItemsFromCharListDatasets(res);
            setTotalItems(items);
            let accs = formatAccountDataFromCharListDatasets(res);
            if (accs?.length > 0) {
                accs = accs.map((acc) => {
                    return {
                        ...acc,
                        name: getAccountByEmail(acc.email)?.name,
                    }
                });
            }
            setAccountsData(accs);
        };
        loadItems();
    }, []);

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
                <Character character={null} />
            </ComponentBox>

            {/* Totals */}
            <ComponentBox
                title='Totals'
                isCollapseable={true}
                defaultCollapsed
                innerSx={{ position: 'relative', overflow: 'hidden', }}
            >
                <ItemCanvas
                    imgSrc="renders.png"
                    itemIds={totalItems?.itemIds ? totalItems.itemIds : []}
                    items={items} totals={totalItems?.totals ? totalItems.totals : {}}
                    setPopperAnchor={(target) => setPopperAnchor(target)}
                />

                <img
                    src={theme.palette.mode === 'dark' ? '/logo/logo_inner_big.png' : '/logo/logo_inner_big_dark.png'}
                    alt="EAM Logo"
                    style={{
                        maxHeight: '50%',
                        maxWidth: '50%',
                        position: 'absolute',
                        top: '50%',
                        left: '50%',
                        transform: 'translate(-50%, -50%)',
                        opacity: theme.palette.mode === 'dark' ? 0.05 : 0.1,
                        pointerEvents: 'none',
                        zIndex: 1,
                    }}
                />
            </ComponentBox>

            {/* Accounts */}
            {
                accountsData &&
                accountsData.map((accountData, index) => {
                    return (
                        <ComponentBox
                            key={index}
                            title={accountData.name ? accountData.name : accountData.email}
                            isCollapseable={true}
                            innerSx={{
                                dispaly: 'flex',
                                flexDirection: 'coulmn',
                                gap: 1
                            }}
                        >

                            {/* Characters */}
                            <Box
                                sx={{
                                    display: 'flex',
                                    flexDirection: 'row',                                    
                                    gap: 1,
                                    flexWrap: 'wrap',
                                }}
                            >
                                {
                                    accountData.character &&
                                    accountData.character.map((char, index) => {
                                        return (
                                            <Character key={index} character={char} />
                                        );
                                    })
                                }
                            </Box>


                        </ComponentBox>
                    );
                })
            }
        </Box>
    );
}

export default VaultPeekerPage;