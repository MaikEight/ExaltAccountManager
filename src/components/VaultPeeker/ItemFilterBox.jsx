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
import CharacterSeasonalTypeFilter from "./Filter/CharacterSeasonalTypeFilter";
import useUserSettings from "../../hooks/useUserSettings";

function ItemFilterBox() {
    const { changeFilter } = useVaultPeeker();
    const collapsedFileds = useUserSettings().getByKeyAndSubKey('vaultPeeker', 'collapsedFileds');
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
            defaultCollapsed={collapsedFileds !== undefined ? collapsedFileds.filter : true}
            sx={{
                mb: 0,
            }}
            innerSx={{
                display: 'flex',
                flexDirection: 'row',
                gap: 1,
                alignItems: 'end',
            }}
        >
            {/* Filter */}
            <TierFilter />
            <ItemTypeFilter />
            <FeedPowerFilter />
            <SoulboundFilter />
            <CharacterSeasonalTypeFilter />
        </ComponentBox >
    );
}

export default ItemFilterBox;