import { Box, FormControl, Input, MenuItem, Select, Typography } from "@mui/material";
import useVaultPeeker from "../../../hooks/useVaultPeeker";
import { useEffect, useState } from "react";
import items from "../../../assets/constants";
import { useTheme } from "@emotion/react";

const ITEM_HEIGHT = 48;
const ITEM_PADDING_TOP = 8;
const MenuProps = {
    PaperProps: {
        style: {
            maxHeight: ITEM_HEIGHT * 4.5 + ITEM_PADDING_TOP,
            width: 85,
        },
    },
};

const directions = {
    '=': 'equal',
    '>=': 'up',
    '<=': 'down'
};

function TierFilter() {
    const [tierFilterOptions, setTierFilterOptions] = useState([{ tier: -2, flag: -1, name: 'None' }]);
    const [tierFilter, setTierFilter] = useState({
        direction: '>=',
        option: 0,
    });

    const { filter, changeFilter } = useVaultPeeker();
    const theme = useTheme();

    useEffect(() => {
        const maxTier = Object.keys(items).reduce((max, itemId) => {
            const itemTier = items[itemId][2];
            return itemTier > max ? itemTier : max;
        }, 0);
        const tierOptions = [];
        tierOptions.push({ tier: -1, flag: 0, name: 'None' });
        for (let i = 0; i <= maxTier; i++) {
            tierOptions.push({ tier: i, flag: 0, name: `T${i}` });
        }

        tierOptions.push({ tier: -1, flag: 2, name: <Typography color={theme.palette.warning.main}>ST</Typography> });
        tierOptions.push({ tier: -1, flag: 1, name: <Typography color="primary">UT</Typography> });

        setTierFilterOptions([{ tier: -2, flag: -1, name: 'OFF' }, ...tierOptions.reverse()]);
    }, []);

    useEffect(() => {
        const opt = tierFilterOptions[tierFilter.option];
        changeFilter('tier', {
            enabled: tierFilter.option !== 0,
            direction: directions[tierFilter.direction],
            value: opt.tier,
            flag: opt.flag
        });
    }, [tierFilter]);

    useEffect(() => {
        if(!filter.tier) {
            console.warn("HOW!?");
            return;
        }

        const direction = Object.keys(directions).find(key => directions[key] === filter.tier.direction);
        const option = tierFilterOptions.findIndex(opt => opt.tier === filter.tier.value && opt.flag === filter.tier.flag);
        
        if(direction === undefined || option === -1) return;
        if(direction === tierFilter.direction && option === tierFilter.option) return;

        setTierFilter({ direction, option });
    }, [filter.tier]);

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
                Tier
            </Typography>
            <Box
                sx={{
                    display: 'flex',
                    flexDirection: 'row',
                    gap: 1,
                }}
            >
                {/* TIER FILTER DIRECTION */}
                <FormControl sx={{ width: 65 }}>
                    <Select
                        id="select-tier-direction"
                        value={tierFilter.direction}
                        onChange={(event) => {
                            setTierFilter((prev) => ({ ...prev, direction: event.target.value }))
                        }}
                        input={
                            <Input
                                id="select-tier-direction-input"
                                disableUnderline
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
                                {selected}
                            </Box>
                        )}
                        MenuProps={{
                            ...MenuProps,
                            PaperProps: {
                                ...MenuProps.PaperProps,
                                style: {
                                    ...MenuProps.PaperProps.style,
                                    width: 65,
                                }
                            }
                        }}
                    >
                        {
                            Object.keys(directions).map((dir) => (
                                <MenuItem
                                    key={directions[dir]}
                                    value={dir}
                                    sx={{
                                        display: 'flex',
                                        flexDirection: 'column',
                                        justifyContent: 'center',
                                        alignItems: 'center',
                                    }}
                                >
                                    {dir}
                                </MenuItem>
                            ))
                        }
                    </Select>
                </FormControl>
                {/* TIER FILTER VALUE */}
                <FormControl sx={{ width: 85 }}>
                    <Select
                        id="select-tier-value"
                        value={tierFilter.option}
                        onChange={(event) => {
                            setTierFilter((prev) => ({ ...prev, option: event.target.value }))
                        }}
                        input={
                            <Input
                                id="select-tier-value-input"
                                disableUnderline
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
                                {tierFilterOptions[selected]?.name}
                            </Box>
                        )}
                        MenuProps={MenuProps}
                    >
                        {
                            tierFilterOptions.map((opt, index) => {
                                const option = tierFilterOptions[index];
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
                                        {option.name}
                                    </MenuItem>
                                )
                            })
                        }
                    </Select>
                </FormControl>
            </Box>
        </Box>
    );
}

export default TierFilter;