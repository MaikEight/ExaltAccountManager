import { Chip } from "@mui/material";
import useStringToColor from './../../hooks/useStringToColor';

function ServerChip({ params, sx }) {
    
    const serverName = params.value && params.value !== "" ? params.value : "Default";
    return (
        <Chip
            sx={{
                ...useStringToColor(serverName),
                ...sx
            }}
            clickable={false}
            label={serverName}
            size="small"
        />
    );
}

export default ServerChip;