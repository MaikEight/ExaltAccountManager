import { Box, IconButton, TableCell, TableRow, Typography } from "@mui/material"
import PaddedTableCell from "./PaddedTableCell"
import GroupUI from "../GridComponents/GroupUI"
import GroupSelector from "./GroupSelector";
import { useContext, useEffect, useState } from "react";
import GroupEditor from "./GroupEditor";
import GroupsContext from "../../contexts/GroupsContext";

function GroupRow({ group, editMode, onChange, innerSx, ...rest }) {
    const [showGroupEditor, setShowGroupEditor] = useState(false);
    const [selectedGroup, setSelectedGroup] = useState(null);
    const { groups, saveGroups } = useContext(GroupsContext);

    useEffect(() => {
        setSelectedGroup(group);
    }, [group]);

    useEffect(() => {
        if (!editMode) {
            setShowGroupEditor(false);
        }
    }, [editMode]);

    if (showGroupEditor) {
        return (
            <TableRow {...rest}>
                <TableCell colSpan={2} sx={innerSx}>
                    <GroupEditor
                        onSave={(g) => {
                            saveGroups([...groups, g]);
                            setShowGroupEditor(false);
                            setSelectedGroup(g);
                            onChange(g);
                        }}
                        onCancel={() => setShowGroupEditor(false)}
                    />
                </TableCell>
            </TableRow>
        );
    }

    return (
        <TableRow {...rest}>
            <PaddedTableCell sx={innerSx}>
                <Typography variant="body2" fontWeight={300} component="span">
                    Group
                </Typography>
            </PaddedTableCell>
            <PaddedTableCell sx={innerSx} align="left">
                {
                    !editMode ?
                        selectedGroup ? <GroupUI group={selectedGroup} /> : null
                        :
                        <GroupSelector
                            selected={selectedGroup ? selectedGroup.name : ''}
                            onChange={(newValue) => {
                                if (newValue === '') {
                                    onChange(null);
                                } else {
                                    // const newGroup = groups.find((g) => g.name === newValue);
                                    onChange(newValue);
                                }
                            }}
                            showGroupEditor={showGroupEditor}
                            setShowGroupEditor={setShowGroupEditor}
                        />
                }
            </PaddedTableCell>
        </TableRow>
    )
}

export default GroupRow