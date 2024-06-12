import { useTheme } from "@emotion/react";
import { Box, IconButton } from "@mui/material";
import { GridToolbarColumnsButton, GridToolbarContainer, GridToolbarFilterButton } from "@mui/x-data-grid";
import RefreshOutlinedIcon from '@mui/icons-material/RefreshOutlined';

function DailyLoginsGridToolbar({ onRefresh }) {
    const theme = useTheme();

    return (
        <Box style={{ display: 'flex', flexDirection: 'row', justifyContent: 'space-between', alignItems: 'center' }}>
            <GridToolbarContainer
                sx={{
                    display: "flex",
                    justifyContent: "start",
                    minHeight: 49,
                    backgroundColor: theme.palette.background.paper,
                    borderRadius: `${theme.shape.borderRadius}px ${theme.shape.borderRadius}px 0 0`,
                    pt: 0.5,
                    pb: 0.5,
                    pl: 1,
                    pr: 1,
                }}
            >
                <GridToolbarColumnsButton />
                <GridToolbarFilterButton />
            </GridToolbarContainer>
            <Box
                id="Test"
                sx={{
                    mr: 1.5,
                    display: 'flex',
                    flexDirection: 'row',
                    maxHeight: 49,
                    overflow: 'hidden',
                }}
            >
                <IconButton
                    size="small"
                    onClick={onRefresh}
                >
                    <RefreshOutlinedIcon />
                </IconButton>
            </Box>
        </Box>
    );
}

export default DailyLoginsGridToolbar;