import { Box, FormControl, Input, MenuItem, Select, Typography } from "@mui/material";
import useVaultPeeker from "../../../hooks/useVaultPeeker";

const ITEM_HEIGHT = 48;
const ITEM_PADDING_TOP = 8;
const MenuProps = {
    PaperProps: {
        style: {
            maxHeight: ITEM_HEIGHT * 4.5 + ITEM_PADDING_TOP,
            width: 100,
        },
    },
};

function FeedPowerFilter() {
    const { filter, changeFilter, feedPowerFilterOptions } = useVaultPeeker();

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
                FeedPower
            </Typography>
            <FormControl sx={{ width: 100 }}>
                <Select
                    id="select-fp-value"
                    value={filter.feedPower.value}
                    onChange={(event) => {
                        const index = event.target.value
                        changeFilter('feedPower', {
                            enabled: index !== 0,
                            value: index,
                        });
                    }}
                    input={
                        <Input
                            id="select-fp-value-input"
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
                            {feedPowerFilterOptions[selected]?.name}
                        </Box>
                    )}
                    MenuProps={MenuProps}
                >
                    {
                        feedPowerFilterOptions.map((opt, index) => {
                            return (
                                <MenuItem
                                    key={index}
                                    value={index}
                                    sx={{
                                        display: 'flex',
                                        flexDirection: 'column',
                                        justifyContent: 'center',
                                        alignItems: 'center',
                                    }}
                                >
                                    {opt.name}
                                </MenuItem>
                            )
                        })
                    }
                </Select>
            </FormControl>
        </Box>
    );
}

export default FeedPowerFilter;