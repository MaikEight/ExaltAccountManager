import { useEffect, useState } from "react";
import useServerList from "../hooks/useServerList";
import { useTheme } from "@emotion/react";
import { Box, FormControl, Input, MenuItem, Select, darken } from "@mui/material";
import ServerChip from "./GridComponents/ServerChip";

const ITEM_HEIGHT = 48;
const ITEM_PADDING_TOP = 8;
const MenuProps = {
    slotProps: {
        paper: {
            sx: {
                maxHeight: ITEM_HEIGHT * 4.5 + ITEM_PADDING_TOP,
                width: 150,
                backgroundColor: 'background.paper',
                border: '1px solid',
                borderColor: 'divider',
            }
        }
    }
};

function ServerListSelect({ serversToAdd, selectedServer, onChange, defaultValue }) {
    const [selected, setSelected] = useState(selectedServer);
    const { serverList } = useServerList();
    const theme = useTheme();
    const _defaultValue = defaultValue ?? "Last server";

    useEffect(() => {
        setSelected(selectedServer);
    }, [selectedServer]);

    return (
        <FormControl>
            <Select
                id="server-list-label"
                value={selected ?? _defaultValue}
                onChange={(event) => {
                    onChange?.(event.target.value);
                    setSelected(event.target.value);
                }}
                input={
                    <Input
                        id="server-list-label"
                        disableUnderline
                        sx={{
                            backgroundColor: theme.palette.background.backdrop,
                            borderRadius: `${theme.shape.borderRadius}px`,
                            height: '39px'
                        }}
                    />
                }
                renderValue={(selected) => (
                    <Box
                        sx={{
                            display: 'flex',
                            justifyContent: 'center',
                            alignItems: 'center',
                            width: '100%',
                            height: '100%',
                            ml: 0.5,
                            mr: 0.5
                        }}
                    >
                        <ServerChip key={"key-" + selected} params={{ value: selected }} />
                    </Box>
                )}
                MenuProps={MenuProps}
            >
                {
                    [
                        ...(serversToAdd ?? []),
                        ...(serverList && serverList.length > 0 ? serverList : [])
                    ].map((server) => (
                        <MenuItem
                            key={server.DNS}
                            value={server.Name}
                            sx={{
                                '&.Mui-selected': {
                                    backgroundColor: darken(theme.palette.action.selected, 0.55),
                                },
                                '&.Mui-selected:hover': {
                                    backgroundColor: theme.palette.action.selected,
                                },
                                display: 'flex',
                                justifyContent: 'center',
                                alignItems: 'center',
                            }}
                        >
                            <ServerChip params={{ value: server.Name }} />
                        </MenuItem>
                    ))
                }
            </Select>
        </FormControl>
    );
}

export default ServerListSelect;