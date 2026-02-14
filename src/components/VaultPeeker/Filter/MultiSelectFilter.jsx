import { Box, Checkbox, Chip, FormControl, Input, ListItemText, MenuItem, Select, Typography } from "@mui/material";
import { useTheme } from "@emotion/react";

const ITEM_HEIGHT = 48;
const ITEM_PADDING_TOP = 8;

/**
 * MultiSelectFilter - Reusable multi-select dropdown filter
 * 
 * @param {string} label - Filter label
 * @param {Array} options - Array of { value, name, color? } options
 * @param {Array} selectedValues - Array of selected values
 * @param {Function} onChange - Callback with new selected values array
 * @param {number} width - Component width (default: 150)
 * @param {number} menuWidth - Dropdown menu width (defaults to width if not specified)
 * @param {Function} renderOption - Optional custom render for menu items
 * @param {Function} renderChip - Optional custom render for selected chips
 */
function MultiSelectFilter({
    label,
    options,
    selectedValues,
    onChange,
    width = 150,
    menuWidth,
    renderOption,
    renderChip,
}) {
    const theme = useTheme();

    const handleChange = (event) => {
        const value = event.target.value;
        // Handle "Select All" toggle
        if (value.includes('__all__')) {
            if (selectedValues.length === options.length) {
                onChange([]);
            } else {
                onChange(options.map(opt => opt.value));
            }
            return;
        }
        onChange(typeof value === 'string' ? value.split(',') : value);
    };

    const getOptionByValue = (value) => options.find(opt => opt.value === value);

    const MenuProps = {
        slotProps: {
            paper: {
                sx: {
                    maxHeight: ITEM_HEIGHT * 5 + ITEM_PADDING_TOP,
                    width: menuWidth || width,
                    backgroundColor: 'background.paper',
                    border: '1px solid',
                    borderColor: 'divider',
                }
            }
        }
    };

    return (
        <Box
            sx={{
                display: 'flex',
                flexDirection: 'column',
                gap: 0.25,
            }}
        >
            <Typography sx={{ ml: 0.5 }}>
                {label}
            </Typography>
            <FormControl sx={{ width }}>
                <Select
                    multiple
                    value={selectedValues}
                    onChange={handleChange}
                    input={
                        <Input
                            disableUnderline
                            sx={{
                                backgroundColor: theme.palette.background.backdrop,
                                borderRadius: `${theme.shape.borderRadius}px`,
                                minHeight: '39px',
                            }}
                        />
                    }
                    renderValue={(selected) => {
                        if (selected.length === 0) {
                            return (
                                <Box sx={{ display: 'flex', flexWrap: 'wrap', gap: 0.5, pl: 0.5, py: 0.25 }}>
                                    <Chip
                                        label="All"
                                        size="small"
                                        sx={{
                                            height: 20,
                                            fontSize: '0.7rem',
                                        }}
                                        variant="outlined"
                                    />
                                </Box>
                            );
                        }
                        return (
                            <Box sx={{ display: 'flex', flexWrap: 'wrap', gap: 0.5, pl: 0.5, py: 0.25 }}>
                                {selected.map((value, index) => {
                                    const option = getOptionByValue(value);
                                    if (renderChip) {
                                        return renderChip(option, value, index);
                                    }
                                    return (
                                        <Chip
                                            key={value}
                                            label={option?.name || value}
                                            size="small"
                                            sx={{
                                                height: 20,
                                                fontSize: '0.7rem',
                                                ...(option?.color && {
                                                    borderColor: option.color,
                                                    color: option.color,
                                                }),
                                            }}
                                            variant="outlined"
                                        />
                                    );
                                })}
                            </Box>
                        );
                    }}
                    MenuProps={MenuProps}
                >
                    {/* All Option */}
                    <MenuItem value="__all__" dense>
                        <Checkbox
                            checked={selectedValues.length === 0 || selectedValues.length === options.length}
                            indeterminate={selectedValues.length > 0 && selectedValues.length < options.length}
                            size="small"
                        />
                        <ListItemText
                            primary={
                                <Typography variant="body2" sx={{ fontWeight: 500 }}>
                                    All
                                </Typography>
                            }
                        />
                    </MenuItem>
                    
                    {options.map((option) => {
                        if (renderOption) {
                            return renderOption(option, selectedValues.includes(option.value));
                        }
                        return (
                            <MenuItem key={option.value} value={option.value} dense>
                                <Checkbox
                                    checked={selectedValues.includes(option.value)}
                                    size="small"
                                />
                                <ListItemText
                                    primary={
                                        <Typography
                                            variant="body2"
                                            sx={option.color ? { color: option.color } : {}}
                                        >
                                            {option.name}
                                        </Typography>
                                    }
                                />
                            </MenuItem>
                        );
                    })}
                </Select>
            </FormControl>
        </Box>
    );
}

export default MultiSelectFilter;
