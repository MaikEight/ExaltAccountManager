import { Chip } from "@mui/material";
import useStringToColor from './../../hooks/useStringToColor';
import { SERVERS } from "../../constants";
import { useEffect, useState } from "react";
import { useMemo } from 'react';

function ServerChip({ params, sx }) {
    const [serverName, setServerName] = useState('');

    useEffect(() => {
        if (params.value && params.value !== "") {
            setServerName(params.value);
            return;
        }

        setServerName("Default");
    }, [params.value]);

    return (
        <Chip
            sx={{
                ...useStringToColor(serverName),
                ...sx
            }}
            label={serverName}
            size="small"
        />
    );
}

export default ServerChip;