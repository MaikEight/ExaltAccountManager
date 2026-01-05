import { useState, useRef, useLayoutEffect, useEffect } from "react";
import {
    Box,
    Table,
    TableBody,
    TableCell,
    TableRow,
    Typography,
    TextField,
    Switch,
    Button,
    TableHead,
} from "@mui/material";
import ContentCopyRoundedIcon from '@mui/icons-material/ContentCopyRounded';
import EditOutlinedIcon from '@mui/icons-material/EditOutlined';
import CheckRoundedIcon from '@mui/icons-material/CheckRounded';
import CloseRoundedIcon from '@mui/icons-material/CloseRounded';
import VisibilityOutlinedIcon from '@mui/icons-material/VisibilityOutlined';
import VisibilityOffOutlinedIcon from '@mui/icons-material/VisibilityOffOutlined';
import { formatTime } from 'eam-commons-js';
import useWidgets from "../../../hooks/useWidgets";
import WidgetBase from "./WidgetBase";
import EamIconButton from "../../EamIconButton";
import useAccounts from "../../../hooks/useAccounts";
import { invoke } from '@tauri-apps/api/core';
import useSnack from "../../../hooks/useSnack";
import StyledButton from "../../StyledButton";

function TableDataWidget({ type, widgetId }) {
    const { getWidgetConfiguration, widgetBarState, updateWidgetConfiguration, widgetBarConfig } = useWidgets();
    const { getAccountByEmail, updateAccount } = useAccounts();
    const { showSnackbar } = useSnack();

    const config = getWidgetConfiguration(type);
    const currentSlots = Math.min(widgetBarConfig.slots, config?.slots || type?.defaultConfig?.slots || 1);
    const fields = config?.settings?.fields || type?.defaultConfig?.settings?.fields || {};

    // State for field visibility edit mode
    const [isFieldVisibilityMode, setIsFieldVisibilityMode] = useState(false);
    // State for inline editing - tracks which field is being edited
    const [editingField, setEditingField] = useState(null);
    const [editValue, setEditValue] = useState("");
    // State for password visibility - stores decrypted passwords
    const [decryptedPasswords, setDecryptedPasswords] = useState({});
    // Refs and state for synchronized row heights in 2-slot mode
    const leftTableRef = useRef(null);
    const rightTableRef = useRef(null);
    const [rowHeights, setRowHeights] = useState([]);
    // Ref for container to observe width changes
    const containerRef = useRef(null);
    const lastWidthRef = useRef(0);

    let data = widgetBarState?.data || {};

    if (type.dataSourceObject) {
        data = data[type.dataSourceObject] || {};
    }

    const decryptPassword = async (encryptedPassword) => {
        return invoke("decrypt_string", { data: encryptedPassword })
            .then((decrypted) => {
                return decrypted;
            })
            .catch((err) => {
                console.error("Error decrypting password:", err);
                showSnackbar("Error decrypting password", "error");
                return "";
            });
    };

    const handleToggleFieldVisibility = (fieldKey) => {
        const updatedFields = {
            ...fields,
            [fieldKey]: {
                ...fields[fieldKey],
                visible: !fields[fieldKey].visible,
            },
        };
        updateWidgetConfiguration(type, {
            settings: {
                ...config?.settings,
                fields: updatedFields,
            },
        });
    };

    const handleCopy = (value) => {
        navigator.clipboard.writeText(value);
        showSnackbar("Copied to clipboard", "default");
    };

    const handleStartEdit = async (fieldKey, currentValue, field) => {
        setEditingField(fieldKey);

        // For password fields, decrypt if not already decrypted
        if (field.dataType === 'password') {
            if (decryptedPasswords[field.dataField]) {
                setEditValue(decryptedPasswords[field.dataField]);
            } else if (currentValue) {
                // Need to decrypt first
                const decrypted = await decryptPassword(currentValue);
                setEditValue(decrypted ?? "");
            } else {
                setEditValue("");
            }
        } else {
            setEditValue(currentValue ?? "");
        }
    };

    const handleCancelEdit = () => {
        setEditingField(null);
        setEditValue("");
    };

    const handleSaveEdit = (fieldKey) => {
        const field = fields[fieldKey];
        let valueToSave = editValue;

        if (field.dataType === 'boolean') {
            valueToSave = editValue === true || editValue === 'true';
        }

        saveDataField(field, valueToSave);
        setEditingField(null);
        setEditValue("");
    };

    const togglePasswordVisibility = async (fieldKey, encryptedValue) => {
        if (decryptedPasswords[fieldKey]) {
            setDecryptedPasswords(prev => {
                const newState = { ...prev };
                delete newState[fieldKey];
                return newState;
            });
            return;
        }

        // Decrypt and show
        const decrypted = await decryptPassword(encryptedValue);
        if (decrypted) {
            setDecryptedPasswords(prev => ({
                ...prev,
                [fieldKey]: decrypted,
            }));
        }
    };

    const saveDataField = (field, newValue) => {
        switch (type.editFunctionType) {
            case 'ACCOUNT':
                const currentAccount = getAccountByEmail(widgetBarState.data.email);
                if (!currentAccount) {
                    showSnackbar("Account not found for updating field", "error");
                    return;
                }

                const updatedAccount = {
                    ...currentAccount,
                    [field.dataField]: newValue,
                };

                updateAccount(updatedAccount, field.dataType === 'password')
                    .then(() => {
                        showSnackbar("Account updated successfully", "success");
                    })
                    .catch((err) => {
                        console.error("Error updating account:", err);
                        showSnackbar("Error updating account", "error");
                    });
                break;
            default:
                console.warn(`No editFunctionType handler for type ${type.editFunctionType}`);
        }
    };

    const formatValue = (field, value) => {
        if (value === null || value === undefined) {
            return "-";
        }

        switch (field.dataType) {
            case 'datetime':
                return formatTime(value) || "-";
            case 'boolean':
                return value ? "Enabled" : "Disabled";
            case 'password':
                return decryptedPasswords[field.dataField] || "••••••••";
            case 'server':
            case 'state':
            case 'string':
            default:
                return String(value);
        }
    };

    const renderEditControl = (field, fieldKey, value) => {
        switch (field.dataType) {
            case 'boolean':
                return (
                    <Switch
                        size="small"
                        checked={editValue === true || editValue === 'true'}
                        onChange={(e) => setEditValue(e.target.checked)}
                    />
                );
            case 'password':
                return (
                    <TextField
                        size="small"
                        type="text"
                        value={editValue}
                        onChange={(e) => setEditValue(e.target.value)}
                        autoFocus
                        sx={{
                            minWidth: 120,
                        }}
                    />
                );
            default:
                return (
                    <TextField
                        size="small"
                        value={editValue}
                        onChange={(e) => setEditValue(e.target.value)}
                        autoFocus
                        sx={{ minWidth: 120 }}
                    />
                );
        }
    };

    const renderFieldValue = (field, fieldKey, value) => {
        const isEditing = editingField === fieldKey;

        if (isEditing) {
            return (
                <Box sx={{ display: 'flex', alignItems: 'center', gap: 0.5 }}>
                    {renderEditControl(field, fieldKey, value)}
                    <EamIconButton
                        icon={<CheckRoundedIcon fontSize="small" color="success" />}
                        onClick={() => handleSaveEdit(fieldKey)}
                    />
                    <EamIconButton
                        icon={<CloseRoundedIcon fontSize="small" color="error" />}
                        onClick={handleCancelEdit}
                    />
                </Box>
            );
        }

        return (
            <Box sx={{ display: 'flex', alignItems: 'center', gap: 0.5 }}>
                <Typography
                    variant="body2"
                    sx={{
                        wordBreak: 'break-word',
                        ...(field.dataType === 'password' && decryptedPasswords[field.dataField] && {
                            fontFamily: 'monospace',
                        }),
                    }}
                >
                    {formatValue(field, value)}
                </Typography>
                {field.dataType === 'password' && value && (
                    <EamIconButton
                        icon={decryptedPasswords[field.dataField]
                            ? <VisibilityOffOutlinedIcon fontSize="small" />
                            : <VisibilityOutlinedIcon fontSize="small" />
                        }
                        onClick={() => togglePasswordVisibility(field.dataField, value)}
                    />
                )}
                {field.showCopyButton && value && (
                    <EamIconButton
                        icon={<ContentCopyRoundedIcon fontSize="small" />}
                        onClick={() => handleCopy(value)}
                    />
                )}
                {field.editable && (
                    <EamIconButton
                        icon={<EditOutlinedIcon fontSize="small" />}
                        onClick={() => handleStartEdit(fieldKey, value, field)}
                    />
                )}
            </Box>
        );
    };

    const renderDataTable = (fieldEntries, tableRef = null, useRowHeights = false) => (
        <Table
            size="small"
            ref={tableRef}
            sx={{
                '& tbody tr:last-of-type th:first-of-type': {
                    borderBottomLeftRadius: theme => `${(theme.shape.borderRadius * 2) - 8}px`,
                },
                '& tbody tr:last-of-type td:first-of-type': {
                    borderBottomRightRadius: theme => `${(theme.shape.borderRadius * 2) - 4}px`,
                },
                overflow: 'hidden',
            }}
        >
            <TableBody>
                {fieldEntries.map(([fieldKey, field], index) => (
                    <TableRow
                        key={fieldKey}
                        sx={{
                            backgroundColor: index % 2 === 1 ? 'action.hover' : 'transparent',
                            ...(useRowHeights && rowHeights[index] ? { height: rowHeights[index] } : {}),
                        }}
                    >
                        <TableCell
                            component="th"
                            scope="row"
                            sx={{
                                borderBottom: 'none',
                                py: 0.5,
                                pl: 1,
                                width: '40%',
                            }}
                        >
                            <Typography variant="body2" color="text.secondary">
                                {field.displayName}
                            </Typography>
                        </TableCell>
                        <TableCell
                            sx={{
                                borderBottom: 'none',
                                py: 0.5,
                                pr: 1,
                            }}
                        >
                            {renderFieldValue(field, fieldKey, data[field.dataField])}
                        </TableCell>
                    </TableRow>
                ))}
            </TableBody>
        </Table>
    );

    const renderFieldVisibilityEditor = () => {
        const allFieldEntries = Object.entries(fields);

        return (
            <Box>
                <Table size="small">
                    <TableHead>
                        <TableRow>
                            <TableCell
                                sx={{
                                    borderBottom: 'none',
                                    py: 0.5,
                                    pl: 1,
                                }}
                            >
                                <Typography variant="body2" fontWeight={600} color="text.secondary">
                                    Field
                                </Typography>
                            </TableCell>
                            <TableCell
                                sx={{
                                    borderBottom: 'none',
                                    py: 0.5,
                                    pr: 1,
                                    textAlign: 'left',
                                }}
                            >
                                <Typography variant="body2" fontWeight={600} color="text.secondary">
                                    Visibility
                                </Typography>
                            </TableCell>
                        </TableRow>
                    </TableHead>
                    <TableBody>
                        {
                            allFieldEntries.map(([fieldKey, field], index) => (
                                <TableRow
                                    key={fieldKey}
                                    sx={{
                                        backgroundColor: index % 2 === 1 ? 'action.hover' : 'transparent',
                                    }}
                                >
                                    <TableCell
                                        component="th"
                                        scope="row"
                                        sx={{
                                            borderBottom: 'none',
                                            py: 0.5,
                                            pl: 1,
                                        }}
                                    >
                                        <Typography variant="body2">
                                            {field.displayName}
                                        </Typography>
                                    </TableCell>
                                    <TableCell
                                        sx={{
                                            borderBottom: 'none',
                                            py: 0.5,
                                            pr: 1,
                                            textAlign: 'left',
                                        }}
                                    >
                                        <Switch
                                            size="small"
                                            checked={field.visible}
                                            onChange={() => handleToggleFieldVisibility(fieldKey)}
                                        />
                                    </TableCell>
                                </TableRow>
                            ))
                        }
                    </TableBody>
                </Table>
                <StyledButton
                    variant="contained"
                    color="secondary"
                    onClick={() => setIsFieldVisibilityMode(false)}
                    sx={{ mt: 1, ml: 'auto', display: 'block' }}
                >
                    Exit Edit Mode
                </StyledButton>
            </Box>
        );
    };

    const visibleFieldEntries = Object.entries(fields).filter(
        ([, field]) => field.visible
    );

    // Ref to track measurement phase: 'idle' | 'measuring' | 'applied'
    const measurePhaseRef = useRef('idle');
    const [widthChangeCounter, setWidthChangeCounter] = useState(0);

    // ResizeObserver
    useEffect(() => {
        if (!containerRef.current || currentSlots !== 2) return;

        const resizeObserver = new ResizeObserver((entries) => {
            for (const entry of entries) {
                const newWidth = entry.contentRect.width;
                if (Math.abs(newWidth - lastWidthRef.current) > 1) {
                    lastWidthRef.current = newWidth;
                    setWidthChangeCounter(c => c + 1);
                }
            }
        });

        resizeObserver.observe(containerRef.current);
        return () => resizeObserver.disconnect();
    }, [currentSlots]);

    useLayoutEffect(() => {
        measurePhaseRef.current = 'pending';
    }, [currentSlots, isFieldVisibilityMode, visibleFieldEntries.length, editingField, widthChangeCounter]);

    useLayoutEffect(() => {
        if (currentSlots !== 2 || isFieldVisibilityMode) {
            return;
        }

        if (measurePhaseRef.current === 'pending') {
            measurePhaseRef.current = 'measuring';

            requestAnimationFrame(() => {
                const leftRows = leftTableRef.current?.querySelectorAll('tbody tr');
                const rightRows = rightTableRef.current?.querySelectorAll('tbody tr');

                if (!leftRows?.length && !rightRows?.length) return;

                const maxRows = Math.max(leftRows?.length || 0, rightRows?.length || 0);
                const newHeights = [];

                for (let i = 0; i < maxRows; i++) {
                    const leftRow = leftRows?.[i];
                    const rightRow = rightRows?.[i];

                    if (leftRow) leftRow.style.height = 'auto';
                    if (rightRow) rightRow.style.height = 'auto';

                    void leftRow?.offsetHeight;
                    void rightRow?.offsetHeight;

                    const leftHeight = leftRow?.offsetHeight || 0;
                    const rightHeight = rightRow?.offsetHeight || 0;
                    const maxHeight = Math.max(leftHeight, rightHeight);
                    newHeights.push(maxHeight);

                    if (leftRow) leftRow.style.height = `${maxHeight}px`;
                    if (rightRow) rightRow.style.height = `${maxHeight}px`;
                }

                // Check if heights actually changed from state
                const heightsChanged = newHeights.length !== rowHeights.length ||
                    newHeights.some((h, i) => h !== rowHeights[i]);

                if (heightsChanged) {
                    measurePhaseRef.current = 'applied';
                    setRowHeights(newHeights);
                } else {
                    measurePhaseRef.current = 'applied';
                }
            });
        }
    });

    useLayoutEffect(() => {
        if (currentSlots !== 2 || isFieldVisibilityMode) {
            if (rowHeights.length > 0) {
                setRowHeights([]);
            }
        }
    }, [currentSlots, isFieldVisibilityMode]);

    const renderContent = () => {
        if (isFieldVisibilityMode) {
            return renderFieldVisibilityEditor();
        }

        if (visibleFieldEntries.length === 0) {
            return (
                <Typography variant="body2" color="text.secondary" sx={{ textAlign: 'center', py: 2 }}>
                    No visible fields. Click edit to configure.
                </Typography>
            );
        }

        // For 2 slots, split into two tables
        if (currentSlots === 2) {
            const midpoint = Math.ceil(visibleFieldEntries.length / 2);
            const leftFields = visibleFieldEntries.slice(0, midpoint);
            const rightFields = visibleFieldEntries.slice(midpoint);

            const applyHeights = rowHeights.length > 0;

            return (
                <Box ref={containerRef} sx={{ display: 'flex', gap: 1 }}>
                    <Box sx={{ flex: 1 }}>
                        {renderDataTable(leftFields, leftTableRef, applyHeights)}
                    </Box>
                    <Box sx={{ flex: 1 }}>
                        {renderDataTable(rightFields, rightTableRef, applyHeights)}
                    </Box>
                </Box>
            );
        }

        // For 1 slot, single table
        return renderDataTable(visibleFieldEntries);
    };

    const handleWidgetEditModeChanged = () => {
        setIsFieldVisibilityMode(prev => !prev);
        setEditingField(null);
        setEditValue("");
    };

    return (
        <WidgetBase type={type} widgetId={widgetId} onWidgetEditModeChanged={handleWidgetEditModeChanged} isEditMode={isFieldVisibilityMode}>
            {renderContent()}
        </WidgetBase>
    );
}

export default TableDataWidget;