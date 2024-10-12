import { Box, Tooltip } from "@mui/material";
import * as Icons from "@mui/icons-material";
import { useColorList } from "../hooks/useColorList";

function GroupUI({ group, onClick, innerSx }) {
    if (!group) return null;

    const Icon = group.icon ? Icons[group.icon] : null;
    const color = useColorList(group.color);
    const padding = group.padding || '10%';

    return (
        <Tooltip
            title={group.name}
        >
            <Box
                sx={{
                    display: 'flex',
                    justifyContent: 'center',
                    alignItems: 'center',
                    height: '30px',
                    width: '30px',
                    borderRadius: '50%',
                    backgroundColor: color.background,
                    cursor: onClick ? 'pointer' : 'default',
                    ...innerSx
                }}
                onClick={onClick ? () => onClick(group) : null}
            >
                {Icon ? <Icon sx={{ padding: padding, color: color.color }} /> : null}
            </Box>
        </Tooltip>
    );
}

export { GroupUI };