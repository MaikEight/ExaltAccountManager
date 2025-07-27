import { requestStateToColor, requestStateToIcon, requestStateToShortName, requestStateToMessage } from "eam-commons-js";
import { Chip } from '@mui/material';
import { Tooltip } from '@mui/material';
import { useColorList } from "../../hooks/useColorList";

function RequestStateChip({ state, useShortName = true }) {
    const colorString = requestStateToColor(state);
    const color = useColorList(colorString);
    if (!state) return null;

    const shortName = requestStateToShortName(state);
    const message = requestStateToMessage(state);
    const icon = requestStateToIcon(state, { color: color.color } );

    return (
        <Tooltip title={message || shortName}>
            <Chip
                icon={icon}
                size={"small"}
                label={useShortName ? shortName : message}
                sx={{
                    ...color,
                }}
            />
        </Tooltip>
    );
}

export default RequestStateChip;