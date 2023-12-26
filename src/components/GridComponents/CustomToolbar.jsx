import { useTheme } from "@emotion/react";
import { Box } from "@mui/material";
import { GridToolbarColumnsButton, GridToolbarContainer, GridToolbarDensitySelector, GridToolbarExport, GridToolbarFilterButton } from "@mui/x-data-grid";
import Searchbar from "./Searchbar";


function CustomToolbar({onSearchChanged}) {
    const theme = useTheme();

    return (
        <Box style={{display: 'flex', flexDirection: 'row', justifyContent: 'space-between', alignItems: 'center' }}>
            <GridToolbarContainer
                sx={{
                    display: "flex",
                    justifyContent: "start",                    
                    height: 49,
                    backgroundColor: theme.palette.background.paper,
                    borderRadius: '6px 6px 0 0',
                    //  mb: 1,
                    pt: 0.5,
                    pb: 0.5,
                    pl: 1,
                    pr: 1,
                }}
            >
                <GridToolbarColumnsButton />
                <GridToolbarFilterButton />
                {/* <GridToolbarDensitySelector /> */}
                {/* <GridToolbarExport /> */}
            </GridToolbarContainer>
            <Box sx={{ mr: 0.5 }} >
                <Searchbar onSearchChanged={onSearchChanged} />
            </Box>
        </Box>
    );

}

export default CustomToolbar;