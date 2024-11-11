import { Box, FormControl, IconButton, Input, MenuItem, Select, Tooltip } from "@mui/material";
import AddCircleOutlineIcon from '@mui/icons-material/AddCircleOutline';
import { useTheme } from "@emotion/react";
import { useEffect, useState } from "react";
import { GroupUI, useGroups } from "eam-commons-js";

function GroupSelector({ selected, onChange, showGroupEditor, setShowGroupEditor, sx }) {
    const [selectedGroup, setSelectedGroup] = useState(null);
    const { groups } = useGroups();
    const theme = useTheme();

    useEffect(() => {
        setSelectedGroup(selected);
    }, [selected]);

    const handleChange = (event) => {
        setSelectedGroup(event.target.value);
        onChange(event.target.value);
    };

    return (
        <Box sx={{ display: 'flex', flexDirection: 'row', justifyContent: 'start', alignItems: 'end' }}>
            <FormControl
                variant="standard"
                sx={{
                    m: 1,
                    minWidth: '175px',
                    borderRadius: `${theme.shape.borderRadius}px`,
                    overflow: 'hidden',
                    ...sx
                }}
            >
                <Select
                    id="select-a-group"
                    value={selectedGroup ? selectedGroup : ''}
                    onChange={handleChange}
                    input={
                        <Input
                            id="server-list-label"
                            disableUnderline
                            sx={{
                                borderRadius: `${theme.shape.borderRadius}px`,
                            }}
                        />
                    }
                    renderValue={(selected) => {
                        if (!selected || selected === '' || selected === null || selected === undefined) {
                            return null;
                        }

                        return (
                            <Box id="value-box" sx={{ p: 0.75, px: 1, display: 'flex', flexDirection: 'row', gap: 0.75, alignItems: 'center', justifyContent: 'start' }}>
                                <GroupUI group={groups.find((g) => g.name === selected)} />
                                {selected}
                            </Box>
                        );
                    }}
                >
                    <MenuItem key="None" value="">
                        <em>None</em>
                    </MenuItem>
                    {
                        groups.map((group, index) => (
                            <MenuItem key={index} value={group.name} sx={{ display: 'flex', flexDirection: 'row', gap: 0.75 }}>
                                <GroupUI group={group} />
                                {group.name}
                            </MenuItem>
                        ))
                    }
                </Select>
            </FormControl>
            {
                !showGroupEditor &&
                <Tooltip title="Add new Group">
                    <IconButton
                        size="small"
                        sx={{
                            color: theme.palette.text.primary,
                            width: '34px',
                            height: '34px',
                            mb: 1,
                        }}
                        onClick={() => setShowGroupEditor(true)}
                    >
                        <AddCircleOutlineIcon />
                    </IconButton>
                </Tooltip>
            }
        </Box>
    );
}

export default GroupSelector;