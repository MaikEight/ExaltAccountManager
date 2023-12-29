import { Box, Tooltip } from "@mui/material";
import * as Icons from "@mui/icons-material";
import useColorList from "../../hooks/useColorList";

function GroupUI({ group, onClick }) {
    if (!group) return null;

    console.log(group);
    const Icon = group.icon ? Icons[group.icon] : null;
    const color = useColorList(group.color);
    const padding = group.padding || '15%';

    return (
        <Tooltip
            title={group.name}
            placement="bottom"
            slotProps={{
                popper: {
                    modifiers: [
                        {
                            name: 'offset',
                            options: {
                                offset: [0, -10],
                            },
                        },
                    ],
                },
            }}
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
                }}
                onClick={onClick ? () => onClick(group) : null}
            >
                {Icon ? <Icon sx={{ padding: padding, color: color.color }} /> : null}
            </Box>
        </Tooltip>
    );
}

export default GroupUI;