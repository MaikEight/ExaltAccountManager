import { Box, Typography } from "@mui/material";
import MuledumpCanvas from "../components/RealmItems/MuledumpCanvas";
import items from "../assets/constants";
import ComponentBox from "../components/ComponentBox";
import FilterListOutlinedIcon from '@mui/icons-material/FilterListOutlined';
import { useEffect, useState } from "react";
import Searchbar from './../components/GridComponents/Searchbar';

const chunkArray = (array, chunkSize) => {
    const chunks = [];
    for (let i = 0; i < array.length; i += chunkSize) {
        chunks.push(array.slice(i, i + chunkSize));
    }
    return chunks;
};

function VaultPeekerPage() {
    const [filteredTotals, setFilteredTotals] = useState([]);
    const chunkSize = 1000;
    const itemIds = Object.keys(items);
    const itemChunks = chunkArray(itemIds, chunkSize);
    const totals = {
        2591: 8,
        2592: 2,
        2593: 3,
        2594: 1111,
        2595: 11,
    };

    useEffect(() => {
        setFilteredTotals(itemChunks[0]);
    }, []);

    const searchChanged = (search) => {
        if (search === '') {
            setFilteredTotals(itemChunks[0]);
            return;
        }

        const filteredItems = itemIds.filter((id) => {
            return items[id][0].toLowerCase().includes(search.toLowerCase());
        });
        setFilteredTotals(filteredItems);
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
                    </Box>}
                icon={<FilterListOutlinedIcon />}
                isCollapseable={true}
            >

            </ComponentBox>

            <ComponentBox
                title='Totals'
                isCollapseable={true}
            >
                <MuledumpCanvas imgSrc="renders.png" itemIds={filteredTotals} items={items} totals={totals} />
            </ComponentBox>
        </Box>
    );
}

export default VaultPeekerPage;