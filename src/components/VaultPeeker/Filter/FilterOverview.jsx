import { Box, Chip, Tooltip, Typography } from "@mui/material";
import { useEffect, useState } from "react";
import useVaultPeeker from "../../../hooks/useVaultPeeker";
import { useTheme } from "@emotion/react";


const directions = {
    'equal': '=',
    'up': '>=',
    'down': '<='
};
function FilterOverview() {
    const [filterChipEntries, setFilterChipEntries] = useState([]);
    const { filter, resetFilterType } = useVaultPeeker();
    const theme = useTheme();

    useEffect(() => {
        const filterChipEntries = [];
        const dir = directions[filter.tier.direction];
        const tierText = filter.tier.value === -1 ? 'None' : `T${filter.tier.value}`;
        if (filter.tier.enabled) {
            let tier = <>{dir} {tierText}</>;
            if (filter.tier.flag === 1 || filter.tier.flag === 2) {
                tier = (
                    <Box
                        sx={{
                            display: 'flex',
                            flexDirection: 'row',
                            justifyContent: 'center',
                            alignItems: 'center',
                            textAlign: 'center',
                            gap: 0.5,
                            py: 0.25,
                        }}
                    >
                        {dir}
                        <Typography
                            variant="body2"
                            color={filter.tier.flag === 1 ? "primary" : theme.palette.warning.main}
                        >
                            {
                                filter.tier.flag === 1 ? 'UT' : 'ST'
                            }
                        </Typography>
                    </Box>
                );
            }

            filterChipEntries.push({
                title: 'Tier',
                delteKey: 'tier',
                value: tier,
            });
        }
        setFilterChipEntries(filterChipEntries);
    }, [filter]);

    return (
        <Box
            sx={{
                display: 'flex',
                flexDirection: 'column',
                gap: 0.5,
            }}
        >
            {
                filterChipEntries.map((entry, index) => (
                    <Tooltip key={index} title={entry.title}>
                        <Chip
                            label={entry.value}
                            size="small"
                            onDelete={() => { 
                                resetFilterType(entry.delteKey);
                            }}
                        />
                    </Tooltip>
                ))
            }
        </Box>
    );
}

export default FilterOverview;