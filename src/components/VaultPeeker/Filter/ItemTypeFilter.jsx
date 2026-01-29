import { Box, FormControl, Input, MenuItem, Select, Typography } from "@mui/material";
import useVaultPeeker from "../../../hooks/useVaultPeeker";

const ITEM_HEIGHT = 48;
const ITEM_PADDING_TOP = 8;
const MenuProps = {
    slotProps: {
        paper: {
            sx: {
                maxHeight: ITEM_HEIGHT * 4.5 + ITEM_PADDING_TOP,
                width: 130,
                backgroundColor: 'background.paper',
                border: '1px solid',
                borderColor: 'divider',
            }
        }
    }
};

function ItemTypeFilter() {
    const { filter, changeFilter, slotMapFilter } = useVaultPeeker();

    return (
        <Box
            sx={{
                display: 'flex',
                flexDirection: 'column',
                gap: 0.25,
            }}
        >
            <Typography
                sx={{
                    ml: 0.5,
                }}
            >
                Type
            </Typography>
            <FormControl sx={{ width: 130 }}>
                <Select
                    id="select-type-value"
                    value={filter.itemType.key}
                    onChange={(event) => {
                        const key = event.target.value;
                        changeFilter('itemType', {
                            enabled: key !== 'all',
                            value: slotMapFilter[key].slotType,
                            key: key,
                        });
                    }}
                    input={
                        <Input
                            id="select-type-value-input"
                            disableUnderline
                            sx={{
                                backgroundColor: theme => theme.palette.background.backdrop,
                                borderRadius: theme => `${theme.shape.borderRadius}px`,
                                height: '39px'
                            }}
                        />
                    }
                    renderValue={(selected) => (
                        <Box
                            sx={{
                                display: 'flex',
                                justifyContent: 'center',
                                alignItems: 'center',
                                width: '100%',
                                height: '100%',
                                pl: 0.5,
                            }}
                        >
                            {slotMapFilter[selected].name}
                        </Box>
                    )}
                    MenuProps={MenuProps}
                >
                    {
                        Object.keys(slotMapFilter).map((type, index) => {
                            return (
                                <MenuItem
                                    key={index}
                                    value={type}
                                    sx={{
                                        display: 'flex',
                                        flexDirection: 'column',
                                        justifyContent: 'center',
                                        alignItems: 'center',
                                    }}
                                >
                                    {slotMapFilter[type].name}
                                </MenuItem>
                            )
                        })
                    }
                </Select>
            </FormControl>
        </Box>
    );
}

export default ItemTypeFilter;