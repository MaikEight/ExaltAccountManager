import { Box, Typography } from "@mui/material";
import ItemCanvas from "../components/Realm/ItemCanvas";
import items from "../assets/constants";
import ComponentBox from "../components/ComponentBox";
import FilterListOutlinedIcon from '@mui/icons-material/FilterListOutlined';
import { useEffect, useRef, useState } from "react";
import Searchbar from './../components/GridComponents/Searchbar';
import { invoke } from "@tauri-apps/api";
import { extractRealmItemsFromCharListDatasets } from './../utils/realmItemUtils';
import ItemLocationPopper from "../components/Realm/ItemLocationPopper";

const chunkArray = (array, chunkSize) => {
    const chunks = [];
    for (let i = 0; i < array.length; i += chunkSize) {
        chunks.push(array.slice(i, i + chunkSize));
    }
    return chunks;
};

function VaultPeekerPage() {
    const [totalItems, setTotalItems] = useState();
    const [filteredTotals, setFilteredTotals] = useState([]);
    const [popperAnchor, setPopperAnchor] = useState(null);

    useEffect(() => {
        const loadItems = async () => {
            const res = await invoke('get_latest_char_list_dataset_for_each_account');
            const items = extractRealmItemsFromCharListDatasets(res);
            setTotalItems(items);
            console.log(items);
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

            </ComponentBox>

            <ComponentBox
                title='Totals'
                isCollapseable={true}
                innerSx={{ position: 'relative', overflow: 'hidden', }}
            >
                <ItemCanvas
                    imgSrc="renders.png"
                    itemIds={totalItems?.itemIds ? totalItems.itemIds : []}
                    items={items} totals={totalItems?.totals ? totalItems.totals : {}}
                    setPopperAnchor={(target) => setPopperAnchor(target)}
                />

                <img
                    src={'/logo/logo_inner_big.png'}
                    alt="EAM Logo"
                    style={{
                        height: '50%',
                        maxWidth: '50%',
                        position: 'absolute',
                        top: '50%',
                        left: '50%',
                        transform: 'translate(-50%, -50%)',
                        opacity: 0.05,
                        pointerEvents: 'none',
                        zIndex: 1,
                    }}
                />
            </ComponentBox>
        </Box>
    );
}

export default VaultPeekerPage;