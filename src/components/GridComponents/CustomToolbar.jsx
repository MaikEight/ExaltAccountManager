import { useTheme } from "@emotion/react";
import { Box, Button} from "@mui/material";
import { GridToolbarColumnsButton, GridToolbarContainer, GridToolbarFilterButton } from "@mui/x-data-grid";
import Searchbar from "./Searchbar";
import AddIcon from '@mui/icons-material/Add';

function CustomToolbar({ onSearchChanged, onAddNew }) {
    const theme = useTheme();

    return (
        <Box style={{ display: 'flex', flexDirection: 'row', justifyContent: 'space-between', alignItems: 'center' }}>
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
            <Box
                sx={{
                    display: 'flex',
                    flexDirection: 'row',
                }}
            >
                <Button
                    variant="text"
                    color="primary"
                    onClick={onAddNew}
                    size="small"
                    sx={{
                        mt: 0.5,
                        mb: 0.5,
                    }}
                    startIcon={<AddIcon />}
                >
                    ADD New
                </Button>
                <Box
                    sx={{
                        mr: 0.5,
                        display: 'flex',
                        flexDirection: 'row',
                    }}
                >
                    <Searchbar onSearchChanged={onSearchChanged} />
                </Box>
            </Box>
        </Box>
    );

}

export default CustomToolbar;