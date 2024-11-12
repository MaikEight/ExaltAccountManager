import { Box, Tooltip, Typography, Divider, ToggleButtonGroup, ToggleButton } from "@mui/material";
import LockOpenOutlinedIcon from '@mui/icons-material/LockOpenOutlined';
import LockIcon from '@mui/icons-material/Lock';
import useVaultPeeker from "../../../hooks/useVaultPeeker";
import FilterListOffOutlinedIcon from '@mui/icons-material/FilterListOffOutlined';

function SoulboundFilter() {
    const { filter, changeFilter } = useVaultPeeker();

    const handleChange = (_, newValue) => {
        if (newValue === null || newValue === filter.soulbound.value) {
            return;
        }

        const checked = newValue;
        changeFilter('soulbound', {
            enabled: checked !== 2,
            value: checked,
        });
    };

    return (
        <Box
            id="soulbound-filter-root"
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
                Soulbound
            </Typography>
            <ToggleButtonGroup
                value={filter.soulbound.value}
                exclusive
                onChange={handleChange}
                aria-label="Soulbound filter"
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
                    <ToggleButton value={2} aria-label="All items" key={'All'}  >
                        <FilterListOffOutlinedIcon color={filter.soulbound.value === 2 ? 'primary' : ''} />
                    </ToggleButton>
                </Tooltip>
                <Divider orientation="vertical" variant="middle" flexItem />
                <Tooltip title={'Show only tradeable items'}>
                    <ToggleButton value={1} aria-label="Tradeable" key={'Tradeable'}>
                        <LockOpenOutlinedIcon color={filter.soulbound.value === 1 ? 'primary' : ''} />
                    </ToggleButton>
                </Tooltip>
                <Divider orientation="vertical" variant="middle" flexItem />
                <Tooltip title={'Show only soulbound items'}>
                    <ToggleButton value={0} aria-label="Soulbound" key={'Soulbound'}>
                        <LockIcon color={filter.soulbound.value === 0 ? 'primary' : ''} />
                    </ToggleButton>
                </Tooltip>
            </ToggleButtonGroup>
        </Box>
    );
}

export default SoulboundFilter;