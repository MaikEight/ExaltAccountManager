import { Badge, Button, IconButton, Tooltip } from "@mui/material";
import { ColumnsPanelTrigger, FilterPanelTrigger, Toolbar, ToolbarButton } from "@mui/x-data-grid";
import RefreshOutlinedIcon from '@mui/icons-material/RefreshOutlined';
import ViewColumnIcon from '@mui/icons-material/ViewColumn';
import FilterListIcon from '@mui/icons-material/FilterList';

function DailyLoginsGridToolbar({ onRefresh }) {
    return (
        <Toolbar>
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
            <Tooltip 
            sx={{
                ml: 'auto',
            }}
            title="Refresh data"
            >
                <IconButton
                    size="small"
                    onClick={onRefresh}
                >
                    <RefreshOutlinedIcon />
                </IconButton>
            </Tooltip>
        </Toolbar>
    );
}

export default DailyLoginsGridToolbar;