import { useCallback, useEffect, useRef, useState } from "react";
import {
    Box,
    Button,
    Dialog,
    DialogActions,
    DialogContent,
    DialogTitle,
    FormControlLabel,
    IconButton,
    Switch,
    TextField,
    Typography,
    Tooltip,
    Divider,
} from "@mui/material";
import { useTheme } from "@emotion/react";
import CloseIcon from '@mui/icons-material/Close';
import DownloadIcon from '@mui/icons-material/Download';
import LockIcon from '@mui/icons-material/Lock';
import ZoomInIcon from '@mui/icons-material/ZoomIn';
import ZoomOutIcon from '@mui/icons-material/ZoomOut';
import RestartAltIcon from '@mui/icons-material/RestartAlt';
import { save } from '@tauri-apps/plugin-dialog';
import { writeFile } from '@tauri-apps/plugin-fs';
import { useUserLogin } from 'eam-commons-js';
import { drawItemAsync } from "../../../utils/realmItemDrawUtils";
import items from "../../../assets/constants";
import useVaultPeeker from "../../../hooks/useVaultPeeker";
import { useNavigate } from "react-router-dom";

const DEFAULT_EXPORT_OPTIONS = {
    backgroundColor: '#1e1e2e',
    showBranding: true,
    showCaption: true,
    headerText: 'Total Items exported by Exalt Account Manager',
    footerText: '',
    itemPadding: 2,
};

function ExportModalV2({ open, onClose, items: itemsData = [] }) {
    const theme = useTheme();
    const canvasRef = useRef(null);
    const isGeneratingRef = useRef(false);
    const navigate = useNavigate();
    const { isPlusUser } = useUserLogin();
    const { itemPadding: contextPadding, accountsData } = useVaultPeeker();

    const [exportOptions, setExportOptions] = useState({
        ...DEFAULT_EXPORT_OPTIONS,
        backgroundColor: theme.palette.background.default,
        itemPadding: contextPadding,
    });
    const [isExporting, setIsExporting] = useState(false);
    const [previewUrl, setPreviewUrl] = useState(null);
    const [zoomLevel, setZoomLevel] = useState(1);

    const canCustomize = isPlusUser;

    // Generate account count text
    const accountCount = accountsData?.length || 0;
    const dateString = new Date().toLocaleDateString();

    // Update option handler
    const updateOption = useCallback((key, value) => {
        if (!canCustomize) return;
        // Max length limits for header and footer is 64 characters to prevent overflow in the preview
        if ((key === 'headerText' || key === 'footerText') && value.length > 64) {
            value = value.slice(0, 64);
        }
        setExportOptions((prev) => ({ ...prev, [key]: value }));
    }, [canCustomize]);

    // Generate canvas preview
    const generatePreview = useCallback(async () => {
        if (!itemsData?.length) return;
        if (isGeneratingRef.current) return; // Prevent concurrent executions

        const canvas = canvasRef.current;
        if (!canvas) return;

        isGeneratingRef.current = true;
        try {
            const ctx = canvas.getContext('2d');
            const { backgroundColor, showBranding, showCaption, headerText, footerText, itemPadding } = exportOptions;

        // Calculate dimensions
        const itemSize = 40 + (2 * itemPadding);
        const itemsPerRow = Math.min(20, Math.max(10, Math.ceil(Math.sqrt(itemsData.length * 2))));
        const rows = Math.ceil(itemsData.length / itemsPerRow);

        const padding = 20;
        const headerHeight = headerText ? 40 : 0;
        const footerHeight = footerText ? 30 : 0;
        const captionHeight = showCaption ? 20 : 0;
        const brandingHeight = showBranding ? 60 : 0;

        canvas.width = (itemsPerRow * itemSize) + (2 * padding);
        canvas.height = (rows * itemSize) + (2 * padding) + headerHeight + footerHeight + captionHeight + brandingHeight;

        // Draw background
        ctx.fillStyle = backgroundColor;
        ctx.fillRect(0, 0, canvas.width, canvas.height);

        // Draw header text
        if (headerText) {
            ctx.fillStyle = theme.palette.text.primary;
            ctx.font = 'bold 18px Roboto';
            ctx.textAlign = 'center';
            ctx.fillText(headerText, canvas.width / 2, padding + 25);
        }

        // Draw branding watermark (centered, behind items)
        if (showBranding) {
            const logo = new Image();
            logo.src = '/logo/logo_inner_big.png';
            await new Promise((resolve) => {
                logo.onload = resolve;
                logo.onerror = resolve;
            });

            const logoSize = Math.min(canvas.width, canvas.height) * 0.3;
            const logoX = (canvas.width - logoSize) / 2;
            const logoY = headerHeight + padding + ((rows * itemSize) - logoSize) / 2;

            ctx.globalAlpha = 0.1;
            ctx.drawImage(logo, logoX, logoY, logoSize, logoSize);
            ctx.globalAlpha = 1;
        }

        // Draw items
        const startY = padding + headerHeight;
        for (let i = 0; i < itemsData.length; i++) {
            const item = itemsData[i];
            const itemId = item.itemId ?? item.item_id ?? item;
            const maxRarity = item.maxRarity ?? item.rarity ?? 0;
            const count = item.count ?? 1;

            if (itemId === -1) continue; // Skip empty slots in export

            const row = Math.floor(i / itemsPerRow);
            const col = i % itemsPerRow;
            const x = padding + (col * itemSize);
            const y = startY + (row * itemSize);

            try {
                const itemData = items[itemId];
                if (itemData) {
                    const imageSrc = await drawItemAsync("renders.png", itemData, maxRarity, itemPadding);
                    if (imageSrc) {
                        const img = new Image();
                        img.src = imageSrc;
                        await new Promise((resolve) => {
                            img.onload = resolve;
                            img.onerror = resolve;
                        });
                        ctx.drawImage(img, x, y, itemSize, itemSize);

                        // Draw count badge
                        if (count > 1) {
                            const countText = count > 999 ? '999+' : count.toString();
                            ctx.fillStyle = 'rgba(0, 0, 0, 0.7)';
                            ctx.font = 'bold 10px Roboto';
                            const textWidth = ctx.measureText(countText).width;
                            const badgeX = x + itemSize - textWidth - 6;
                            const badgeY = y + itemSize - 4;
                            ctx.fillRect(badgeX - 2, badgeY - 10, textWidth + 4, 12);
                            ctx.fillStyle = '#ffffff';
                            ctx.fillText(countText, badgeX, badgeY);
                        }
                    }
                }
            } catch (error) {
                console.error(`Failed to draw item ${itemId}:`, error);
            }
        }

        // Draw footer text
        if (footerText) {
            ctx.fillStyle = '#888888';
            ctx.font = '12px Roboto';
            ctx.textAlign = 'center';
            ctx.fillText(footerText, canvas.width / 2, canvas.height - padding - captionHeight - brandingHeight - 5);
        }

        // Draw caption text
        if (showCaption) {
            const captionText = `${itemsData.length} unique items from ${accountCount} accounts`;
            ctx.fillStyle = '#888888';
            ctx.font = '10px Roboto';
            ctx.textAlign = 'center';
            ctx.fillText(captionText, canvas.width / 2, canvas.height - padding - brandingHeight - 8);
        }

        // Draw branding text at bottom
        if (showBranding) {
            ctx.fillStyle = '#666666';
            ctx.font = '11px Roboto';
            ctx.textAlign = 'right';
            ctx.fillText('Generated by Exalt Account Manager', canvas.width - padding, canvas.height - 8);
        }

        // Generate preview URL
        setPreviewUrl(canvas.toDataURL('image/png'));
        } finally {
            isGeneratingRef.current = false;
        }
    }, [itemsData, exportOptions, accountCount]);

    // Regenerate preview when options change
    // Use timeout to ensure Dialog content is mounted before accessing canvas
    // Also debounce to prevent multiple concurrent generations when typing
    useEffect(() => {
        if (open) {
            const timeoutId = setTimeout(() => {
                generatePreview();
            }, 300); // 300ms debounce for typing
            return () => clearTimeout(timeoutId);
        }
    }, [open, generatePreview]);

    // Export to file
    const handleExport = async () => {
        if (!canvasRef.current) return;

        setIsExporting(true);
        try {
            const canvas = canvasRef.current;
            const blob = await new Promise((resolve) => {
                canvas.toBlob(resolve, 'image/png');
            });

            const filePath = await save({
                defaultPath: `vault-totals-${new Date().toISOString().split('T')[0]}.png`,
                filters: [{ name: 'PNG Image', extensions: ['png'] }],
            });

            if (filePath && blob) {
                const arrayBuffer = await blob.arrayBuffer();
                await writeFile(filePath, new Uint8Array(arrayBuffer));
                onClose();
            }
        } catch (error) {
            console.error('Failed to export image:', error);
        } finally {
            setIsExporting(false);
        }
    };

    // Handle zoom controls
    const handleZoomIn = useCallback(() => {
        setZoomLevel(prev => Math.min(prev + 0.25, 5));
    }, []);

    const handleZoomOut = useCallback(() => {
        setZoomLevel(prev => Math.max(prev - 0.25, 0.25));
    }, []);

    const handleZoomReset = useCallback(() => {
        setZoomLevel(1);
    }, []);

    // Handle mouse wheel zoom
    const handleWheel = useCallback((e) => {
        if (e.ctrlKey || e.metaKey) {
            e.preventDefault();
            const delta = e.deltaY > 0 ? -0.1 : 0.1;
            setZoomLevel(prev => Math.max(0.25, Math.min(5, prev + delta)));
        }
    }, []);

    // Reset options and zoom when modal opens
    useEffect(() => {
        if (open) {
            setExportOptions({
                ...DEFAULT_EXPORT_OPTIONS,
                backgroundColor: theme.palette.background.default,
                itemPadding: contextPadding,
            });
            setZoomLevel(1);
        }
    }, [open, contextPadding, theme.palette.background.default]);

    return (
        <Dialog
            open={open}
            onClose={onClose}
            maxWidth="lg"
            fullWidth
            slotProps={{
                paper: {
                    sx: {
                        backgroundColor: 'background.default',
                        border: '1px solid',
                        borderColor: theme => theme.palette.divider,
                        borderRadius: theme => `${theme.shape.borderRadius}px`,
                        height: '80vh',
                        maxHeight: '800px',
                    }
                }
            }}
        >
            <DialogTitle
                sx={{
                    display: 'flex',
                    alignItems: 'center',
                    justifyContent: 'space-between',
                    backgroundColor: 'background.default',
                    borderColor: theme => theme.palette.divider,
                }}
            >
                <Typography variant="h6">Export Totals as Image</Typography>
                <IconButton onClick={onClose} size="small">
                    <CloseIcon />
                </IconButton>
            </DialogTitle>

            <DialogContent dividers sx={{ display: 'flex', gap: 2, backgroundColor: 'background.default', p: 0, overflow: 'hidden' }}>
                {/* Preview Panel */}
                <Box
                    sx={{
                        flex: 1,
                        minWidth: 0,
                        display: 'flex',
                        flexDirection: 'column',
                        backgroundColor: 'background.default',
                        borderRight: 1,
                        borderColor: 'divider',
                        position: 'relative',
                    }}
                >
                    {/* Preview Header with Zoom Controls */}
                    <Box sx={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', p: 1, borderBottom: 1, borderColor: 'divider' }}>
                        <Typography variant="subtitle2" color="text.secondary">
                            Preview
                        </Typography>
                        <Box sx={{ display: 'flex', gap: 0.5, alignItems: 'center' }}>
                            <Tooltip title="Zoom Out">
                                <span>
                                    <IconButton size="small" onClick={handleZoomOut} disabled={zoomLevel <= 0.25}>
                                        <ZoomOutIcon fontSize="small" />
                                    </IconButton>
                                </span>
                            </Tooltip>
                            <Typography variant="caption" color="text.secondary" sx={{ minWidth: 45, textAlign: 'center' }}>
                                {Math.round(zoomLevel * 100)}%
                            </Typography>
                            <Tooltip title="Zoom In">
                                <span>
                                    <IconButton size="small" onClick={handleZoomIn} disabled={zoomLevel >= 5}>
                                        <ZoomInIcon fontSize="small" />
                                    </IconButton>
                                </span>
                            </Tooltip>
                            <Tooltip title="Reset Zoom">
                                <IconButton size="small" onClick={handleZoomReset}>
                                    <RestartAltIcon fontSize="small" />
                                </IconButton>
                            </Tooltip>
                        </Box>
                    </Box>
                    
                    {/* Scrollable Preview Container */}
                    <Box
                        onWheel={handleWheel}
                        sx={{
                            flex: 1,
                            overflow: 'auto',
                            p: 2,
                            cursor: zoomLevel > 1 ? 'grab' : 'default',
                            '&:active': {
                                cursor: zoomLevel > 1 ? 'grabbing' : 'default',
                            },
                        }}
                    >
                        {previewUrl ? (
                            <Box
                                sx={{
                                    display: 'inline-block',
                                    minWidth: '100%',
                                    minHeight: '100%',
                                    display: 'flex',
                                    alignItems: zoomLevel <= 1 ? 'center' : 'flex-start',
                                    justifyContent: zoomLevel <= 1 ? 'center' : 'flex-start',
                                }}
                            >
                                <img
                                    src={previewUrl}
                                    alt="Export preview"
                                    style={{
                                        width: `${zoomLevel * 100}%`,
                                        height: 'auto',
                                        borderRadius: 4,
                                        transition: 'width 0.1s ease-out',
                                    }}
                                />
                            </Box>
                        ) : (
                            <Box sx={{ display: 'flex', alignItems: 'center', justifyContent: 'center', height: '100%' }}>
                                <Typography color="text.secondary">Generating preview...</Typography>
                            </Box>
                        )}
                    </Box>
                    <canvas ref={canvasRef} style={{ display: 'none' }} />
                </Box>

                {/* Options Panel */}
                <Box
                    sx={{
                        width: 320,
                        flexShrink: 0,
                        display: 'flex',
                        flexDirection: 'column',
                        gap: 2,
                        p: 2,
                        position: 'relative',
                        overflowY: 'auto',
                    }}
                >
                    {/* Plus Required Overlay */}
                    {!canCustomize && (
                        <Box
                            sx={{
                                position: 'absolute',
                                top: 0,
                                left: 0,
                                right: 0,
                                bottom: 0,
                                backgroundColor: 'rgba(0, 0, 0, 0.6)',
                                display: 'flex',
                                flexDirection: 'column',
                                alignItems: 'center',
                                justifyContent: 'center',
                                zIndex: 10,
                                borderRadius: 1,
                            }}
                        >
                            <LockIcon sx={{ fontSize: 48, color: 'warning.main', mb: 1 }} />
                            <Typography variant="h6" color="white" textAlign="center">
                                EAM Plus Required
                            </Typography>
                            <Typography variant="body2" color={theme.palette.mode === "dark" ? "grey.400" : "white"} textAlign="center" sx={{ mt: 1, px: 2 }}>
                                Upgrade to EAM Plus to customize your exports
                            </Typography>
                            <Button
                                variant="contained"
                                color="primary"
                                onClick={() => navigate('/profile')}
                                sx={{
                                    mt: 2,
                                }}
                            >
                                Learn More
                            </Button>
                        </Box>
                    )}

                    <Typography variant="subtitle1" fontWeight="bold">
                        Customization Options
                    </Typography>

                    <Divider />

                    {/* Background Color */}
                    <Box>
                        <Typography variant="body2" color="text.secondary" sx={{ mb: 0.5 }}>
                            Background Color
                        </Typography>
                        <Box sx={{ display: 'flex', gap: 1, alignItems: 'center' }}>
                            <input
                                type="color"
                                value={exportOptions.backgroundColor}
                                onChange={(e) => updateOption('backgroundColor', e.target.value)}
                                disabled={!canCustomize}
                                style={{
                                    width: 40,
                                    height: 32,
                                    border: 'none',
                                    borderRadius: 4,
                                    cursor: canCustomize ? 'pointer' : 'not-allowed',
                                }}
                            />
                            <TextField
                                size="small"
                                value={exportOptions.backgroundColor}
                                onChange={(e) => updateOption('backgroundColor', e.target.value)}
                                disabled={!canCustomize}
                                sx={{ flex: 1 }}
                            />
                        </Box>
                    </Box>

                    {/* Show Branding */}
                    <FormControlLabel
                        control={
                            <Switch
                                checked={exportOptions.showBranding}
                                onChange={(e) => updateOption('showBranding', e.target.checked)}
                                disabled={!canCustomize}
                            />
                        }
                        label="Show EAM Branding"
                    />

                    {/* Header Text */}
                    <TextField
                        label="Header Text"
                        placeholder={`${accountCount} Accounts - ${dateString}`}
                        value={exportOptions.headerText}
                        onChange={(e) => updateOption('headerText', e.target.value)}
                        disabled={!canCustomize}
                        size="small"
                        fullWidth
                    />

                    {/* Footer Text */}
                    <TextField
                        label="Footer Text"
                        placeholder="Custom footer text..."
                        value={exportOptions.footerText}
                        onChange={(e) => updateOption('footerText', e.target.value)}
                        disabled={!canCustomize}
                        size="small"
                        fullWidth
                    />

                    {/* Show Caption */}
                    <FormControlLabel
                        control={
                            <Switch
                                checked={exportOptions.showCaption}
                                onChange={(e) => updateOption('showCaption', e.target.checked)}
                                disabled={!canCustomize}
                            />
                        }
                        label={`Show Caption (${itemsData.length} items, ${accountCount} accounts)`}
                    />

                    <Divider />

                    <Typography variant="caption" color="text.secondary">
                        {itemsData.length} unique items from {accountCount} accounts
                    </Typography>
                </Box>
            </DialogContent>

            <DialogActions sx={{ px: 3, py: 2, backgroundColor: 'background.default', }}>
                <Button onClick={onClose} color="inherit">
                    Cancel
                </Button>
                <Button
                    variant="contained"
                    onClick={handleExport}
                    disabled={isExporting || !previewUrl}
                    startIcon={<DownloadIcon />}
                >
                    {isExporting ? 'Exporting...' : 'Export'}
                </Button>
            </DialogActions>
        </Dialog>
    );
}

export default ExportModalV2;
