import { Chip } from "@mui/material";
import useStringToColor from './../../hooks/useStringToColor';

function ServerChip({ params }) {
    return (
        <Chip
            sx={{
                ...useStringToColor(params.value),
            }}
            label={params.value}
            size="small"
        />
    );
}

export default ServerChip;