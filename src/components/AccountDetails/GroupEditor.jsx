import { Box, Chip, Slider, TextField, Typography, Grid } from "@mui/material";
import { useGroups } from "eam-commons-js";
import React, { useEffect, useState } from "react";
import * as Icons from '@mui/icons-material';
import { useTheme } from "@emotion/react";
import { FixedSizeGrid as FixedGrid } from 'react-window';
import StyledButton from "../StyledButton";
import { GroupUI } from "../GridComponents/GroupUI";
import { useColorList } from "../../hooks/useColorList";

const allIcons = Object.keys(Icons).map((key) => key.includes('Outlined') ? key : null).filter((key) => key !== null);
const MAX_ICON_PADDING = 30;

function GroupEditor({ g, onSave, onCancel }) {
    const [newGroup, setNewGroup] = useState(
        {
            name: '',
            color: 0,
            iconType: 'mui',
            icon: 'AccountCircle',
            padding: '10%',
        });

    const theme = useTheme();
    const colors = useColorList();
    const { groups } = useGroups();

    useEffect(() => {
        if (g) {
            setNewGroup(g);
        }
    }, [g]);

    const isGroupNameValid = () => {
        if (newGroup.name.length === 0) return false;
        if (newGroup.name === 'None') return false; //Name used for "No Group"
        if (groups === null || groups === undefined) return false;

        return !groups.some((group) => group.name === newGroup.name);
    }

    const getPaddingAsNumber = () => {
        return MAX_ICON_PADDING - parseInt(newGroup.padding.replace('%', ''));
    }

    const setPadding = (padding) => {
        setNewGroup({ ...newGroup, padding: padding + '%' });
    }

    const getIcons = () => {
        const columnCount = 9;
        const width = 42;
        const height = 42;

        return (
            <FixedGrid
                columnCount={columnCount}
                columnWidth={width}
                height={240}
                rowCount={Math.ceil(allIcons.length / columnCount)}
                rowHeight={height}
                width={columnCount * width + 16}
                style={{
                    outline: 'none',
                    borderRadius: 6,
                    backgroundColor: theme.palette.background.paperLight,
                }}
            >
                {({ columnIndex, rowIndex, style }) => {
                    const index = rowIndex * columnCount + columnIndex;
                    const icon = allIcons[index];
                    if (!icon) return null;
                    return (
                        <div
                            style={{
                                ...style,
                                ...{ backgroundColor: newGroup.icon === icon ? theme.palette.background.default : null },
                                borderRadius: 6,
                                padding: '6px',
                            }}
                            onClick={() => setNewGroup({ ...newGroup, icon: icon })}
                        >
                            {React.createElement(Icons[icon], { style: { width: '100%', height: '100%' } })}
                        </div>
                    );
                }}
            </FixedGrid>
        );
    };

    return (
        <Box
            sx={{
                display: 'flex',
                flexDirection: 'column',
                width: '100%',
                gap: 1,
            }}
        >
            <Box
                sx={{
                    display: 'flex',
                    flexDirection: 'row',
                    width: '100%',
                    justifyContent: 'space-between',
                    gap: 1,
                }}
            >
                <Box
                    sx={{
                        display: 'flex',
                        flexDirection: 'column',
                        gap: 1,
                    }}
                >
                    <Typography variant="h6" fontWeight={300} component="span">
                        Create new group
                    </Typography>
                    <Typography variant="body2" fontWeight={300} component="span">
                        Select a color
                    </Typography>
                    <Box>
                        {
                            colors.map((color, index) => (
                                <Chip
                                    key={index}
                                    sx={{
                                        m: 0.5,
                                        width: '40px',
                                        height: '16px',
                                        borderRadius: theme.shape.borderRadius,
                                        backgroundColor: index === newGroup.color ? color.color : color.background,
                                        '&:hover': {
                                            backgroundColor: color.color,
                                        }
                                    }}
                                    onClick={() => setNewGroup({ ...newGroup, color: index })}
                                />
                            ))
                        }
                    </Box>
                    <TextField
                        label="Group Name"
                        variant="standard"
                        value={newGroup.name}
                        onChange={(event) => setNewGroup({ ...newGroup, name: event.target.value })}
                        error={!isGroupNameValid() && (groups !== undefined || newGroup.name.length === 0)}
                        helperText={!isGroupNameValid() ? newGroup.name.length === 0 ? "Group name can't be empty" : groups !== undefined ? "Group name already exists" : "" : ""}
                    />
                </Box>
                <Box
                    sx={{
                        display: 'flex',
                        flexDirection: 'column',
                        justifyContent: 'center',
                        alignItems: 'center',
                        gap: 1,
                    }}
                >
                    <Typography variant="body2" fontWeight={300} component="span">
                        Preview
                    </Typography>
                    <GroupUI group={newGroup} />
                </Box>
            </Box>
            <Typography variant="body2" fontWeight={300} component="span">
                Select an icon
            </Typography>
            {getIcons()}
            <Typography variant="body2" fontWeight={300} component="span">
                Select icon size
            </Typography>
            <Slider
                aria-label="Size"
                defaultValue={10}
                getAriaValueText={() => newGroup.padding}
                valueLabelDisplay="auto"
                value={getPaddingAsNumber()}
                onChange={(event, newValue) => setPadding(MAX_ICON_PADDING - newValue)}
                step={1}
                min={0}
                max={MAX_ICON_PADDING}
            />
            <Grid container spacing={1}>
                <Grid size={8}>
                    <StyledButton
                        fullWidth
                        disabled={!isGroupNameValid()}
                        onClick={() => onSave(newGroup)}
                        startIcon={<Icons.SaveOutlined />}
                    >
                        Save
                    </StyledButton>
                </Grid>
                <Grid size={4}>
                    <StyledButton
                        color="secondary"
                        fullWidth
                        onClick={() => onCancel()}
                        startIcon={<Icons.Close />}
                    >
                        Cancel
                    </StyledButton>
                </Grid>
            </Grid>
        </Box>
    );
}

export default GroupEditor;