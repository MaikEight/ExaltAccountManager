import { useTheme } from "@emotion/react";
import { Box, Chip, FormControl, Input, MenuItem, Select, alpha } from "@mui/material";
import { GridToolbarColumnsButton, GridToolbarContainer, GridToolbarFilterButton } from "@mui/x-data-grid";
import Searchbar from "./Searchbar";

function DailyLoginsGridToolbar({ onSearchChanged }) {
    const theme = useTheme();

    return (
        <Box style={{ display: 'flex', flexDirection: 'row', justifyContent: 'space-between', alignItems: 'center' }}>
            <GridToolbarContainer
                sx={{
                    display: "flex",
                    justifyContent: "start",
                    minHeight: 49,
                    backgroundColor: theme.palette.background.paper,
                    borderRadius: '6px 6px 0 0',
                    pt: 0.5,
                    pb: 0.5,
                    pl: 1,
                    pr: 1,
                }}
            >
                <GridToolbarColumnsButton />
                <GridToolbarFilterButton />
            </GridToolbarContainer>            
        </Box>
    );
}

export default DailyLoginsGridToolbar;