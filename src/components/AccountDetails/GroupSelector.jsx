import { Box, FormControl, IconButton, InputLabel, MenuItem, Select, Tooltip } from "@mui/material";
import AddCircleOutlineIcon from '@mui/icons-material/AddCircleOutline';
import { useTheme } from "@emotion/react";
import { useContext, useEffect, useState } from "react";
import GroupUI from "../GridComponents/GroupUI";
import GroupsContext from "../../contexts/GroupsContext";

function GroupSelector({ selected, onChange, showGroupEditor, setShowGroupEditor, sx }) {
    const [selectedGroup, setSelectedGroup] = useState(null);
    const { groups } = useContext(GroupsContext);
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
            <FormControl variant="standard" sx={{ m: 1, minWidth: '175px', ...sx}}>
                <InputLabel id="group selector">Select a group</InputLabel>
                <Select
                    labelId="select-a-group"
                    id="select-a-group"
                    value={selectedGroup ? selectedGroup : ''}
                    onChange={handleChange}
                    label="Select a group"
                    renderValue={(selected) => (
                        <Box sx={{ p: 0.25, display: 'flex', flexDirection: 'row', gap: 0.75, alignItems: 'center' }}>
                            <GroupUI group={groups.find((g) => g.name === selected)} />
                            {selected}
                        </Box>
                    )}
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
                </Tooltip>}
        </Box>
    );
}

export default GroupSelector;