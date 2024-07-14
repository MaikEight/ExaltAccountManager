import { Box, Typography } from "@mui/material";
import ComponentBox from "../ComponentBox";
import Searchbar from "../GridComponents/Searchbar";
import useVaultPeeker from "../../hooks/useVaultPeeker";
import FilterListOutlinedIcon from '@mui/icons-material/FilterListOutlined';
import TierFilter from "./Filter/TierFilter";
import FilterOverview from "./Filter/FilterOverview";
import SoulboundFilter from "./Filter/SoulboundFilter";
import FeedPowerFilter from "./Filter/FeedPowerFilter";
import ItemTypeFilter from "./Filter/ItemTypeFilter";

function ItemFilterBox() {
    const { changeFilter } = useVaultPeeker();

    const searchChanged = (search) => {
        changeFilter('search', {
            enabled: search && search !== '',
            value: search
        });
    };

    return (
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
                    <Box>
                        <FilterOverview />
                    </Box>
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
            defaultCollapsed={true}
            innerSx={{
                display: 'flex',
                flexDirection: 'row',
                gap: 1,
                flexWrap: 'wrap',
                alignItems: 'end',
            }}
        >
            {/* Filter */}
            <TierFilter />
            <SoulboundFilter />
            <FeedPowerFilter />
            <ItemTypeFilter />
        </ComponentBox >
    );
}

export default ItemFilterBox;