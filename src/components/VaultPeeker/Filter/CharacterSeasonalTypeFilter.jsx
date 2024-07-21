import { Box, Checkbox, FormControlLabel, Tooltip } from "@mui/material";
import PersonOutlineOutlinedIcon from '@mui/icons-material/PersonOutlineOutlined';
import useVaultPeeker from "../../../hooks/useVaultPeeker";
import AccessTimeOutlinedIcon from '@mui/icons-material/AccessTimeOutlined';
import PersonOffOutlinedIcon from '@mui/icons-material/PersonOffOutlined';

function CharacterSeasonalTypeFilter() {
    const { filter, changeFilter } = useVaultPeeker();

    const handleChange = () => {
        const checked = filter.characterType.value === 3 ? 0 : filter.characterType.value + 1;
        changeFilter('characterType', {
            enabled: checked !== 0,
            value: checked,
        });
    };

    const getTooltipTitle = () => {
        switch (filter.characterType.value) {
            case 0:
                return "Show all items";
            case 1:
                return "Show only items on seasonal characters";
            case 2:
                return "Show only items on normal characters";
            case 3:
                return "Show only items not on characters";
            default:
                return "";
        }
    };

    return (
        <Box
            id="character-seasonal-type-filter-root"
            sx={{
                display: 'flex',
                flexDirection: 'row',
                alignItems: 'center',
            }}
        >
            <Tooltip title={getTooltipTitle()}>
                <FormControlLabel
                    label="Character Type"
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
                            checked={filter.characterType.value === 1}
                            indeterminate={filter.characterType.value >= 2}
                            onChange={handleChange}
                            indeterminateIcon={
                                filter.characterType.value === 2 ?
                                    <PersonOutlineOutlinedIcon /> :
                                    <PersonOffOutlinedIcon />
                            }
                            checkedIcon={<AccessTimeOutlinedIcon />}
                        />
                    }
                />
            </Tooltip>
        </Box>
    );
}

export default CharacterSeasonalTypeFilter;