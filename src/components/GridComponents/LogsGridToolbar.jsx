import { useTheme } from "@emotion/react";
import { Badge, Box, Button, Chip, FormControl, Input, MenuItem, Select, Tooltip } from "@mui/material";
import { ColumnsPanelTrigger, FilterPanelTrigger, ToolbarButton } from "@mui/x-data-grid";
import Searchbar from "./Searchbar";
import { Toolbar } from "@mui/x-data-grid";
import ViewColumnIcon from '@mui/icons-material/ViewColumn';
import FilterListIcon from '@mui/icons-material/FilterList';

const ITEM_HEIGHT = 48;
const ITEM_PADDING_TOP = 8;
const MenuProps = {
    slotProps: {
        paper: {
            sx: {
                maxHeight: ITEM_HEIGHT * 4.5 + ITEM_PADDING_TOP,
                width: 120,
                backgroundColor: 'background.paper',
                border: '1px solid',
                borderColor: 'divider',
            }
        }
    }
};

function LogsGridToolbar({ selectedLogtype, setSelectedLogtype, onSearchChanged }) {
    const theme = useTheme();

    const logTypes = ['AuditLog', 'ErrorLog'];

    return (
        <Toolbar
            sx={{
                display: 'flex',
                flexDirection: 'row',
                justifyContent: 'space-between',
                alignItems: 'center',
                backgroundColor: theme.palette.background.paper,
                borderRadius: `${theme.shape.borderRadius}px ${theme.shape.borderRadius}px 0 0`,
            }}
        >
            <Box
                sx={{
                    display: "flex",
                    justifyContent: "start",
                    alignItems: "center",
                    minHeight: 49,
                    pt: 0.5,
                    pb: 0.5,
                }}
            >
                <Tooltip title="Columns">
                    <ColumnsPanelTrigger
                        render={
                            <ToolbarButton
                                render={
                                    <Button
                                        aria-label="Columns"
                                        size="small"
                                        variant="text"
                                        startIcon={<ViewColumnIcon />}
                                        sx={{
                                            width: 'fit-content',
                                            height: 'fit-content',
                                            px: 1,
                                        }}
                                    >
                                        Columns
                                    </Button>
                                }
                            />
                        }
                    />
                </Tooltip>
                <Tooltip title="Filters">
                    <FilterPanelTrigger
                        render={(props, state) => (
                            <ToolbarButton
                                {...props}
                                render={
                                    <Badge badgeContent={state.filterCount} color="primary" variant="dot">
                                        <Button
                                            aria-label="Filters"
                                            size="small"
                                            variant="text"
                                            startIcon={<FilterListIcon />}
                                            component={'div'}
                                            sx={{
                                                width: 'fit-content',
                                                height: 'fit-content',
                                                px: 1,
                                            }}
                                        >
                                            Filters
                                        </Button>
                                    </Badge>
                                }
                            />
                        )}
                    />
                </Tooltip>
            </Box>

            <Box
                sx={{
                    display: 'flex',
                    flexDirection: 'row',
                    justifyContent: 'end',
                    alignItems: 'center',
                }}
            >
                <Box
                    id="select-logtype-container"
                    sx={{
                        display: 'flex',
                        flexDirection: 'row',
                        height: '100%',
                        maxHeight: 49,
                        overflow: 'hidden',
                    }}
                >
                    <FormControl sx={{ width: 120, minHeight: '100%' }}>
                        <Select
                            id="select-logtype"
                            value={selectedLogtype}
                            onChange={(event) => setSelectedLogtype(event.target.value)}
                            input={
                                <Input
                                    id="select-logtype"
                                    disableUnderline
                                    sx={{
                                        backgroundColor: theme.palette.background.backdrop,
                                        borderRadius: `${theme.shape.borderRadius}px`,
                                        height: '100%'
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
                                    }}
                                >
                                    <Chip
                                        size="small"
                                        key={selected}
                                        label={selected}
                                    />
                                </Box>
                            )}
                            MenuProps={MenuProps}
                        >
                            {
                                logTypes.map((logType) => (
                                    <MenuItem
                                        key={logType}
                                        value={logType}
                                    >
                                        {logType}
                                    </MenuItem>
                                ))
                            }
                        </Select>
                    </FormControl>
                    <Searchbar onSearchChanged={onSearchChanged} />
                </Box>
            </Box>
        </Toolbar>
    );
}

export default LogsGridToolbar;