import { useState, useMemo, useCallback } from "react";
import {
    Box,
    Button,
    Dialog,
    DialogTitle,
    DialogContent,
    DialogActions,
    IconButton,
    Menu,
    MenuItem,
    ListItemIcon,
    ListItemText,
    TextField,
    Typography,
    Tooltip,
    Divider,
} from "@mui/material";
import { useTheme } from "@emotion/react";
import BookmarkIcon from '@mui/icons-material/Bookmark';
import BookmarkBorderIcon from '@mui/icons-material/BookmarkBorder';
import SaveIcon from '@mui/icons-material/Save';
import DeleteIcon from '@mui/icons-material/Delete';
import AddIcon from '@mui/icons-material/Add';
import useVaultPeeker from "../../../hooks/useVaultPeeker";

/**
 * FilterPresetSelector - Dropdown for managing filter presets
 * 
 * Features:
 * - Load saved presets
 * - Save current filter as new preset
 * - Delete presets
 * - Stored in LocalStorage via VaultPeekerContext
 */
function FilterPresetSelector() {
    const theme = useTheme();
    const { filter, filterPresets, loadFilterPreset, saveFilterPreset, deleteFilterPreset } = useVaultPeeker();
    
    const [anchorEl, setAnchorEl] = useState(null);
    const [saveDialogOpen, setSaveDialogOpen] = useState(false);
    const [newPresetName, setNewPresetName] = useState('');
    const [deleteConfirmName, setDeleteConfirmName] = useState(null);
    
    const menuOpen = Boolean(anchorEl);

    const handleMenuOpen = useCallback((event) => {
        setAnchorEl(event.currentTarget);
    }, []);

    const handleMenuClose = useCallback(() => {
        setAnchorEl(null);
    }, []);

    const handleLoadPreset = useCallback((preset) => {
        loadFilterPreset(preset);
        handleMenuClose();
    }, [loadFilterPreset, handleMenuClose]);

    const handleSaveDialogOpen = useCallback(() => {
        setNewPresetName('');
        setSaveDialogOpen(true);
        handleMenuClose();
    }, [handleMenuClose]);

    const handleSaveDialogClose = useCallback(() => {
        setSaveDialogOpen(false);
        setNewPresetName('');
    }, []);

    const handleSavePreset = useCallback(() => {
        if (newPresetName.trim()) {
            saveFilterPreset(newPresetName.trim());
            handleSaveDialogClose();
        }
    }, [newPresetName, saveFilterPreset, handleSaveDialogClose]);

    const handleDeleteClick = useCallback((presetId, event) => {
        event.stopPropagation();
        setDeleteConfirmName(presetId);
    }, []);

    const handleDeleteConfirm = useCallback(() => {
        if (deleteConfirmName) {
            deleteFilterPreset(deleteConfirmName);
            setDeleteConfirmName(null);
        }
    }, [deleteConfirmName, deleteFilterPreset]);

    const handleDeleteCancel = useCallback(() => {
        setDeleteConfirmName(null);
    }, []);

    // Check if current filter matches any preset
    const activePreset = useMemo(() => {
        if (!filterPresets?.length) return null;
        
        for (const preset of filterPresets) {
            if (JSON.stringify(preset.filter) === JSON.stringify(filter)) {
                return preset;
            }
        }
        return null;
    }, [filterPresets, filter]);

    const hasPresets = filterPresets?.length > 0;

    return (
        <>
            {/* Preset Button */}
            <Tooltip title="Filter presets">
                <IconButton
                    onClick={handleMenuOpen}
                    size="small"
                    sx={{
                        color: activePreset ? theme.palette.primary.main : 'inherit',
                    }}
                >
                    {activePreset ? <BookmarkIcon /> : <BookmarkBorderIcon />}
                </IconButton>
            </Tooltip>

            {/* Preset Menu */}
            <Menu
                anchorEl={anchorEl}
                open={menuOpen}
                onClose={handleMenuClose}
                anchorOrigin={{
                    vertical: 'bottom',
                    horizontal: 'right',
                }}
                transformOrigin={{
                    vertical: 'top',
                    horizontal: 'right',
                }}
                slotProps={{
                    paper: {
                        sx: {
                            minWidth: 200,
                            maxWidth: 300,
                        },
                    },
                }}
            >
                {/* Saved Presets */}
                {hasPresets && (
                    <Box>
                        <Typography
                            variant="caption"
                            color="text.secondary"
                            sx={{ px: 2, py: 0.5, display: 'block' }}
                        >
                            Saved Presets
                        </Typography>
                        {filterPresets.map((preset) => (
                            <MenuItem
                                key={preset.id}
                                onClick={() => handleLoadPreset(preset)}
                                selected={activePreset?.id === preset.id}
                                sx={{
                                    display: 'flex',
                                    justifyContent: 'space-between',
                                }}
                            >
                                <ListItemText primary={preset.name} />
                                <IconButton
                                    size="small"
                                    onClick={(e) => handleDeleteClick(preset.id, e)}
                                    sx={{ ml: 1 }}
                                >
                                    <DeleteIcon fontSize="small" />
                                </IconButton>
                            </MenuItem>
                        ))}
                        <Divider />
                    </Box>
                )}

                {/* Save Current */}
                <MenuItem onClick={handleSaveDialogOpen}>
                    <ListItemIcon>
                        <AddIcon />
                    </ListItemIcon>
                    <ListItemText primary="Save current filter..." />
                </MenuItem>
            </Menu>

            {/* Save Dialog */}
            <Dialog
                open={saveDialogOpen}
                onClose={handleSaveDialogClose}
                maxWidth="xs"
                fullWidth
            >
                <DialogTitle>Save Filter Preset</DialogTitle>
                <DialogContent>
                    <TextField
                        autoFocus
                        margin="dense"
                        label="Preset Name"
                        fullWidth
                        value={newPresetName}
                        onChange={(e) => setNewPresetName(e.target.value)}
                        onKeyPress={(e) => {
                            if (e.key === 'Enter') {
                                handleSavePreset();
                            }
                        }}
                        placeholder="e.g., Legendary Items, ST Rings..."
                    />
                </DialogContent>
                <DialogActions>
                    <Button onClick={handleSaveDialogClose}>Cancel</Button>
                    <Button
                        onClick={handleSavePreset}
                        variant="contained"
                        disabled={!newPresetName.trim()}
                        startIcon={<SaveIcon />}
                    >
                        Save
                    </Button>
                </DialogActions>
            </Dialog>

            {/* Delete Confirmation Dialog */}
            <Dialog
                open={Boolean(deleteConfirmName)}
                onClose={handleDeleteCancel}
                maxWidth="xs"
            >
                <DialogTitle>Delete Preset?</DialogTitle>
                <DialogContent>
                    <Typography>
                        Are you sure you want to delete the preset "{deleteConfirmName}"?
                    </Typography>
                </DialogContent>
                <DialogActions>
                    <Button onClick={handleDeleteCancel}>Cancel</Button>
                    <Button
                        onClick={handleDeleteConfirm}
                        color="error"
                        variant="contained"
                        startIcon={<DeleteIcon />}
                    >
                        Delete
                    </Button>
                </DialogActions>
            </Dialog>
        </>
    );
}

export default FilterPresetSelector;
