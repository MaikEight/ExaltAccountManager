import { Box, Checkbox, FormControlLabel, Tooltip } from "@mui/material";
import LockOpenOutlinedIcon from '@mui/icons-material/LockOpenOutlined';
import LockIcon from '@mui/icons-material/Lock';
import useVaultPeeker from "../../../hooks/useVaultPeeker";

function SoulboundFilter() {
    const { filter, changeFilter } = useVaultPeeker();

    const handleChange = () => {
        const checked = filter.soulbound.value === 2 ? 0 : filter.soulbound.value + 1;
        changeFilter('soulbound', {
            enabled: checked !== 2,
            value: checked,
        });
    };

    const getTooltipTitle = () => {
        switch (filter.soulbound.value) {
            case 0:
                return "Show only soulbound items";
            case 1:
                return "Show only tradeable items";
            case 2:
                return "Show all items";
            default:
                return "";
        }
    };

    return (
        <Box
            id="soulbound-filter-root"
            sx={{
                display: 'flex',
                flexDirection: 'row',
                alignItems: 'center',
            }}
        >
            <Tooltip title={getTooltipTitle()}>
                <FormControlLabel
                    label="Soulbound"
                    labelPlacement="top"
                    sx={{
                        display: 'flex',
                        justifyContent: 'start',
                        alignItems: 'start',
                        p: 0,
                        m: 0,
                        ml: 1,
                    }}
                    control={
                        <Checkbox
                            sx={{ ml: -1 }}
                            checked={filter.soulbound.value === 0}
                            indeterminate={filter.soulbound.value === 1}
                            onChange={handleChange}
                            indeterminateIcon={<LockOpenOutlinedIcon />}
                            checkedIcon={<LockIcon />}
                        />
                    }
                />
            </Tooltip>
        </Box>
    );
}

export default SoulboundFilter;