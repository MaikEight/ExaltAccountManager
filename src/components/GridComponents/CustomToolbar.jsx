import { useTheme } from "@emotion/react";
import { Box, Button, Checkbox, FormControlLabel, LinearProgress, Popover, Tooltip, Typography } from "@mui/material";
import { GridToolbarColumnsButton, GridToolbarContainer, GridToolbarFilterButton } from "@mui/x-data-grid";
import Searchbar from "./Searchbar";
import AddIcon from '@mui/icons-material/Add';
import FileUploadOutlinedIcon from '@mui/icons-material/FileUploadOutlined';
import { useState } from "react";
import StyledButton from './../StyledButton';
import DataArrayOutlinedIcon from '@mui/icons-material/DataArrayOutlined';
import ListOutlinedIcon from '@mui/icons-material/ListOutlined';
import useAccounts from './../../hooks/useAccounts';
import Papa from 'papaparse';
import { invoke } from "@tauri-apps/api";

function CustomToolbar({ onSearchChanged, onAddNew }) {
    const [anchorElPopover, setAnchorElPopover] = useState(null);
    const [includeAditionalColumns, setIncludeAditionalColumns] = useState(false);
    const [isExporting, setIsExporting] = useState(false);

    const theme = useTheme();
    const { accounts } = useAccounts();

    const openExportPopover = Boolean(anchorElPopover);
    const popoverId = openExportPopover ? 'export-popover' : undefined;

    const decryptPassword = async (password) => {
        try {
            return await invoke('decrypt_string', { data: password });
        } catch (error) {
            console.error('Error decrypting password: ', error);
            return '';
        }
    };

    const handleExport = async (format) => {
        setIsExporting(true);

        let blob;
        let fileName;

        const accs = accounts;
        for (const acc of accs) {
            acc.password = await decryptPassword(acc.password);
        }

        if (!includeAditionalColumns) {
            accs.forEach(acc => {
                delete acc.id;
                delete acc.isDeleted;
                delete acc.serverName;
                delete acc.state;
                delete acc.lastLogin;
                delete acc.lastRefresh;
                delete acc.token;
                delete acc.extra;
                delete acc.steamId;
            });
        }

        if (format === 'CSV') {
            const csv = Papa.unparse(accs);
            blob = new Blob([csv], { type: 'text/csv' });
            fileName = 'accounts.csv';
        } else {
            blob = new Blob([JSON.stringify(accs, null, 2)], { type: 'application/json' });
            fileName = 'accounts.json';
        }

        const url = URL.createObjectURL(blob);
        const a = document.createElement('a');
        a.href = url;
        a.download = fileName;
        a.click();
        URL.revokeObjectURL(url);

        setIsExporting(false);
        handleCloseExportPopover();
    };

    const handleClickExport = (event) => {
        setAnchorElPopover(event.currentTarget);
    };

    const handleCloseExportPopover = () => {
        setAnchorElPopover(null);
    };

    return (
        <Box style={{ display: 'flex', flexDirection: 'row', justifyContent: 'space-between', alignItems: 'center', borderRadius: theme.shape.borderRadius }}>
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
                <Tooltip title="Show/Hide Columns">
                    <GridToolbarColumnsButton />
                </Tooltip>
                <GridToolbarFilterButton />
                <Tooltip title="Export Accounts">
                    <Button
                        variant="text"
                        color="primary"
                        size="small"
                        sx={{
                            mt: 0.5,
                            mb: 0.5,
                        }}
                        startIcon={<FileUploadOutlinedIcon />}
                        onClick={handleClickExport}
                    >
                        Export
                    </Button>
                </Tooltip>
            </GridToolbarContainer>
            <Box
                sx={{
                    display: 'flex',
                    flexDirection: 'row',
                    justifyContent: 'end',
                    alignItems: 'center',
                }}
            >
                {
                    onAddNew &&
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
                }
                <Box
                    sx={{
                        mr: 0.5,
                        display: 'flex',
                        flexDirection: 'row',
                        maxHeight: 49,
                    }}
                >
                    <Searchbar onSearchChanged={onSearchChanged} />
                </Box>
            </Box>
            <Popover
                id={popoverId}
                open={openExportPopover}
                anchorEl={anchorElPopover}
                onClose={handleCloseExportPopover}
                anchorOrigin={{
                    vertical: 'bottom',
                    horizontal: 'center',
                }}
                transformOrigin={{
                    vertical: 'top',
                    horizontal: 'center',
                }}
            >
                <Box
                    sx={{
                        position: 'relative',
                        pt: 2,
                        px: 2,
                        borderRadius: `${theme.shape.borderRadius}px`,
                    }}
                >
                    {
                        isExporting &&
                        <LinearProgress
                            sx={{
                                position: 'absolute',
                                top: 0,
                                left: 0,
                                right: 0,
                            }}
                        />
                    }
                    <Typography
                        variant="body1"
                    >
                        Please choose the format you want to export to have
                    </Typography>
                    <FormControlLabel
                        control={
                            <Checkbox
                                disabled={isExporting}
                                value={includeAditionalColumns}
                                onChange={(event) => setIncludeAditionalColumns(event.target.checked)}
                            />
                        }
                        label={
                            <Typography
                                variant="body2"
                            >
                                Include additional columns
                            </Typography>
                        }
                    />
                </Box>
                <Box
                    sx={{
                        display: 'flex',
                        flexDirection: 'column',
                        px: 1,
                        pb: 1,
                        gap: 0.5,
                    }}
                >
                    <StyledButton
                        disabled={isExporting}
                        color="primary"
                        size="small"
                        sx={{
                            mt: 0.5,
                            mb: 0.5,
                        }}
                        startIcon={<ListOutlinedIcon />}
                        onClick={() => handleExport('CSV')}
                    >
                        CSV
                    </StyledButton>
                    <StyledButton
                        disabled={isExporting}
                        color="primary"
                        size="small"
                        sx={{
                            mt: 0.5,
                            mb: 0.5,
                        }}
                        startIcon={<DataArrayOutlinedIcon />}
                        onClick={() => handleExport('JSON')}
                    >
                        JSON
                    </StyledButton>
                </Box>
            </Popover>
        </Box>
    );
}

export default CustomToolbar;