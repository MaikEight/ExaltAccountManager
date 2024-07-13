import { Box, Chip, FormControl, Input, MenuItem, Select, Typography } from "@mui/material";
import ComponentBox from "../ComponentBox";
import Searchbar from "../GridComponents/Searchbar";
import useVaultPeeker from "../../hooks/useVaultPeeker";
import FilterListOutlinedIcon from '@mui/icons-material/FilterListOutlined';
import { useEffect, useState } from "react";
import items from "../../assets/constants";
import { useTheme } from "@emotion/react";
import TierFilter from "./Filter/TierFilter";
import FilterOverview from "./Filter/FilterOverview";



function ItemFilterBox() {
    const { filter, changeFilter } = useVaultPeeker();

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
            }}
        >
            {/* <TierFilter /> */}
            <TierFilter />
            
        </ComponentBox >
    );
}

export default ItemFilterBox;