import { Box, Typography } from "@mui/material";
import ComponentBox from "../ComponentBox";
import Searchbar from "../GridComponents/Searchbar";
import useVaultPeeker from "../../hooks/useVaultPeeker";
import FilterListOutlinedIcon from '@mui/icons-material/FilterListOutlined';

function ItemFilterBox() {
    const { filter, changeFilter } = useVaultPeeker();

    const searchChanged = (search) => {
        changeFilter('search', search);
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
                justifyContent: 'center',
            }}
        >
            <Typography variant="h6">Filters are coming soon&#x2122;</Typography>
        </ComponentBox>
    );
}

export default ItemFilterBox;