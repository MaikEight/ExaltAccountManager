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
import { save } from '@tauri-apps/plugin-dialog';
import { writeFile } from '@tauri-apps/plugin-fs';
import { useUserLogin } from 'eam-commons-js';
import { drawItemAsync } from "../../../utils/realmItemDrawUtils";
import items from "../../../assets/constants";
import useVaultPeeker from "../../../hooks/useVaultPeeker";

const DEFAULT_EXPORT_OPTIONS = {
    backgroundColor: '#1e1e2e',
    showBranding: true,
    headerText: '',
    footerText: '',
    itemPadding: 2,
};

function ExportModalV2({ open, onClose, items: itemsData = [] }) {
    const theme = useTheme();
    const canvasRef = useRef(null);
    const { isPlusUser } = useUserLogin();
    const { itemPadding: contextPadding, accountsData } = useVaultPeeker();

    const [exportOptions, setExportOptions] = useState({
        ...DEFAULT_EXPORT_OPTIONS,
        itemPadding: contextPadding,
    });
    const [isExporting, setIsExporting] = useState(false);
    const [previewUrl, setPreviewUrl] = useState(null);

    const canCustomize = isPlusUser;

    // Generate account count text
    const accountCount = accountsData?.length || 0;
    const dateString = new Date().toLocaleDateString();

    // Update option handler
    const updateOption = useCallback((key, value) => {
        if (!canCustomize) return;
        setExportOptions((prev) => ({ ...prev, [key]: value }));
    }, [canCustomize]);

    // Generate canvas preview
    const generatePreview = useCallback(async () => {
        if (!itemsData?.length) return;

        const canvas = canvasRef.current;
        if (!canvas) return;

        const ctx = canvas.getContext('2d');
        const { backgroundColor, showBranding, headerText, footerText, itemPadding } = exportOptions;

        // Calculate dimensions
        const itemSize = 40 + (2 * itemPadding);
        const itemsPerRow = Math.min(20, Math.max(10, Math.ceil(Math.sqrt(itemsData.length * 2))));
        const rows = Math.ceil(itemsData.length / itemsPerRow);

        const padding = 20;
        const headerHeight = headerText ? 40 : 0;
        const footerHeight = footerText ? 30 : 0;
        const brandingHeight = showBranding ? 60 : 0;

        canvas.width = (itemsPerRow * itemSize) + (2 * padding);
        canvas.height = (rows * itemSize) + (2 * padding) + headerHeight + footerHeight + brandingHeight;

        // Draw background
        ctx.fillStyle = backgroundColor;
        ctx.fillRect(0, 0, canvas.width, canvas.height);

        // Draw header text
        if (headerText) {
            ctx.fillStyle = '#ffffff';
            ctx.font = 'bold 18px Arial';
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
                            ctx.font = 'bold 10px Arial';
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
            ctx.font = '12px Arial';
            ctx.textAlign = 'center';
            ctx.fillText(footerText, canvas.width / 2, canvas.height - padding - 5);
        }

        // Draw branding text at bottom
        if (showBranding) {
            ctx.fillStyle = '#666666';
            ctx.font = '11px Arial';
            ctx.textAlign = 'right';
            ctx.fillText('Generated by Exalt Account Manager', canvas.width - padding, canvas.height - 8);
        }

        // Generate preview URL
        setPreviewUrl(canvas.toDataURL('image/png'));
    }, [itemsData, exportOptions]);

    // Regenerate preview when options change
    useEffect(() => {
        if (open) {
            generatePreview();
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

    // Reset options when modal opens
    useEffect(() => {
        if (open) {
            setExportOptions({
                ...DEFAULT_EXPORT_OPTIONS,
                itemPadding: contextPadding,
            });
        }
    }, [open, contextPadding]);

    return (
        <Dialog
            open={open}
            onClose={onClose}
            maxWidth="md"
            fullWidth
            slotProps={{
                paper: {
                    sx: {
                        backgroundColor: 'background.default',
                        border: '1px solid',
                        borderColor: theme => theme.palette.divider,
                        borderRadius: theme => `${theme.shape.borderRadius}px`,
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

            <DialogContent dividers sx={{ display: 'flex', gap: 2, backgroundColor: 'background.default', }}>
                {/* Preview Panel */}
                <Box
                    sx={{
                        flex: 2,
                        display: 'flex',
                        flexDirection: 'column',
                        alignItems: 'center',
                        justifyContent: 'center',
                        backgroundColor: 'background.default',
                        borderRadius: 1,
                        p: 2,
                        overflow: 'auto',
                    }}
                >
                    <Typography variant="subtitle2" color="text.secondary" sx={{ mb: 1 }}>
                        Preview
                    </Typography>
                    {previewUrl ? (
                        <img
                            src={previewUrl}
                            alt="Export preview"
                            style={{
                                maxWidth: '100%',
                                maxHeight: '100%',
                                objectFit: 'contain',
                                borderRadius: 4,
                            }}
                        />
                    ) : (
                        <Typography color="text.secondary">Generating preview...</Typography>
                    )}
                    <canvas ref={canvasRef} style={{ display: 'none' }} />
                </Box>

                {/* Options Panel */}
                <Box
                    sx={{
                        flex: 1,
                        display: 'flex',
                        flexDirection: 'column',
                        gap: 2,
                        minWidth: 250,
                        position: 'relative',
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
                            <Typography variant="body2" color="grey.400" textAlign="center" sx={{ mt: 1, px: 2 }}>
                                Upgrade to EAM Plus to customize your exports
                            </Typography>
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
