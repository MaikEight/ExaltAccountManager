import { useTheme } from "@emotion/react";
import { Box, Chip, FormControl, Input, MenuItem, Select, alpha } from "@mui/material";
import { GridToolbarColumnsButton, GridToolbarContainer, GridToolbarFilterButton } from "@mui/x-data-grid";
import Searchbar from "./Searchbar";

const ITEM_HEIGHT = 48;
const ITEM_PADDING_TOP = 8;
const MenuProps = {
    PaperProps: {
        style: {
            maxHeight: ITEM_HEIGHT * 4.5 + ITEM_PADDING_TOP,
            width: 120,
        },
    },
};

function LogsGridToolbar({ selectedLogtype, setSelectedLogtype, onSearchChanged }) {
    const theme = useTheme();
    const logTypes = ['AuditLog', 'ErrorLog'];

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

            <Box
                sx={{
                    display: 'flex',
                    flexDirection: 'row',
                    justifyContent: 'end',
                    alignItems: 'center',
                }}
            >
                <Box
                    sx={{
                        mr: 0.5,
                        display: 'flex',
                        flexDirection: 'row',
                        maxHeight: 49,
                        overflow: 'hidden',
                    }}
                >
                    <FormControl sx={{ width: 120 }}>
                        {/* <InputLabel id="logtype-label">Logtype</InputLabel> */}
                        <Select
                            sx={{
                                height: 39,
                                ...(theme.palette.mode === 'dark' ? {
                                    backgroundColor: alpha(theme.palette.background.default, 0.5),
                                    '&:hover': {
                                        backgroundColor: alpha(theme.palette.common.white, 0.08),
                                    },
                                } : {
                                    backgroundColor: alpha(theme.palette.background.default, 0.75),
                                    '&:hover': {
                                        backgroundColor: alpha(theme.palette.text.primary, 0.05),
                                    },
                                }),
                                transition: theme.transitions.create('background-color'),
                                borderRadius: '6px',
                            }}
                            id="select-logtype"
                            value={selectedLogtype}
                            onChange={(event) => setSelectedLogtype(event.target.value)}
                            input={
                                <Input
                                    id="select-logtype"
                                    disableUnderline
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
                                    // sx={{
                                    //     p: 0.125,
                                    //     pb: 0,
                                    //     mt: 0.5
                                    // }}
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
        </Box>
    );
}

export default LogsGridToolbar;