import { Box, Typography, Tooltip, Divider, ToggleButtonGroup, ToggleButton } from "@mui/material";
import PersonOutlineOutlinedIcon from '@mui/icons-material/PersonOutlineOutlined';
import useVaultPeeker from "../../../hooks/useVaultPeeker";
import AccessTimeOutlinedIcon from '@mui/icons-material/AccessTimeOutlined';
import PersonOffOutlinedIcon from '@mui/icons-material/PersonOffOutlined';
import FilterListOffOutlinedIcon from '@mui/icons-material/FilterListOffOutlined';

function CharacterSeasonalTypeFilter() {
    const { filter, changeFilter } = useVaultPeeker();

    const handleChange = (_, newValue) => {
        if (newValue === null || newValue === filter.characterType.value) {
            return;
        }

        const checked = newValue;
        changeFilter('characterType', {
            enabled: checked !== 0,
            value: checked,
        });
    };

    return (
        <Box
            id="character-seasonal-type-filter-root"
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
                Character Type
            </Typography>            
            <ToggleButtonGroup
                value={filter.characterType.value}
                exclusive
                onChange={handleChange}
                aria-label="Character Type filter"
                size="small"
                sx={{
                    backgroundColor: theme => theme.palette.background.backdrop,
                    height: '39px',
                    gap: 0.5,
                    '& .MuiToggleButtonGroup-grouped': {
                        border: 'none',
                        borderRadius: theme => `${theme.shape.borderRadius}px`,
                        width: '39px',
                    },
                    '& .MuiToggleButton-root.Mui-selected': {
                        backgroundColor: 'inherit',
                    },
                }}
            >
                <Tooltip title={'Show all items'}>
                    <ToggleButton value={0} aria-label="All items" key={'All'} >
                        <FilterListOffOutlinedIcon color={filter.characterType.value === 0 ? 'primary' : ''} />
                    </ToggleButton>
                </Tooltip>
                <Divider orientation="vertical" variant="middle" flexItem />
                <Tooltip title={'Show only items on seasonal characters'}>
                    <ToggleButton value={1} aria-label="On seasonal characters" key={'SeasonalChars'}>
                        <AccessTimeOutlinedIcon color={filter.characterType.value === 1 ? 'primary' : ''} />
                    </ToggleButton>
                </Tooltip>
                <Divider orientation="vertical" variant="middle" flexItem />
                <Tooltip title={'Show only items on normal characters'}>
                    <ToggleButton value={2} aria-label="On normal characters" key={'NormalChars'}>
                        <PersonOutlineOutlinedIcon color={filter.characterType.value === 2 ? 'primary' : ''} />
                    </ToggleButton>
                </Tooltip>
                <Divider orientation="vertical" variant="middle" flexItem />
                <Tooltip title={'Show only items not on characters'}>
                    <ToggleButton value={3} aria-label="On character" key={'OnChars'}>
                        <PersonOffOutlinedIcon color={filter.characterType.value === 3 ? 'primary' : ''} />
                    </ToggleButton>
                </Tooltip>
            </ToggleButtonGroup>
        </Box>
    );
}

export default CharacterSeasonalTypeFilter;