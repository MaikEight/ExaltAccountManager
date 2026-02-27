import { useState, useMemo, useCallback } from "react";
import { Box, IconButton, Tooltip, Typography } from "@mui/material";
import { useTheme } from "@emotion/react";
import FileUploadOutlinedIcon from '@mui/icons-material/FileUploadOutlined';
import useVaultPeeker from "../../../hooks/useVaultPeeker";
import useUserSettings from "../../../hooks/useUserSettings";
import ComponentBox from "../../ComponentBox";
import VaultPeekerLogo from "../../VaultPeekerLogo";
import ItemCanvasGridV2 from "./ItemCanvasGridV2";
import ExportModalV2 from "./ExportModalV2";

function TotalsViewV2() {
    const theme = useTheme();
    const { filteredItemIds, totalsMap, isLoading, selectItem } = useVaultPeeker();
    const collapsedFields = useUserSettings().getByKeyAndSubKey('vaultPeeker', 'collapsedFileds');

    const [exportModalOpen, setExportModalOpen] = useState(false);

    // Build item entries array for ItemCanvasGridV2: [itemId, { count, itemData }]
    const itemEntries = useMemo(() => {
        if (!totalsMap || !filteredItemIds.length) return [];

        return filteredItemIds
            .filter(itemId => totalsMap.has(itemId))
            .map(itemId => {
                const data = totalsMap.get(itemId);
                return [itemId, { count: data.count, itemData: data }];
            })
            .sort((a, b) => b[1].count - a[1].count); // Sort by count descending
    }, [totalsMap, filteredItemIds]);

    // Build items array for ExportModalV2
    const exportItems = useMemo(() => {
        return itemEntries.map(([itemId, data]) => ({
            itemId,
            count: data.count,
            maxRarity: data.itemData?.maxRarity ?? 0,
        }));
    }, [itemEntries]);

    // Handle item click - open detail popover
    const handleItemClick = useCallback((itemId, data, event) => {
        if (!event) return;
        const rect = event.currentTarget.getBoundingClientRect();
        const isLeftHalf = rect.left < window.innerWidth / 2;
        const position = {
            top: rect.top,
            left: isLeftHalf ? rect.right + 5 : rect.left - 5,
            isLeftHalf,
        };
        selectItem(itemId, position, data.itemData || data);
    }, [selectItem]);

    // Export canvas as image - open modal
    const handleExport = useCallback((event) => {
        event.stopPropagation();
        setExportModalOpen(true);
    }, []);

    return (
        <ComponentBox
            title={
                <Box
                    sx={{
                        display: 'flex',
                        justifyContent: 'space-between',
                        alignItems: 'center',
                        width: '100%',
                    }}
                >
                    <Typography
                        variant="h6"
                        sx={{
                            fontWeight: 600,
                            textAlign: 'center',
                        }}
                    >
                        Totals
                    </Typography>
                    <Tooltip title="Export totals as image">
                        <IconButton
                            onClick={handleExport}
                        >
                            <FileUploadOutlinedIcon />
                        </IconButton>
                    </Tooltip>
                </Box>
            }
            icon={
                <VaultPeekerLogo
                    sx={{ display: 'flex', ml: '2px', width: '20px', height: '24px', mr: 0.25 }}
                    color={theme.palette.text.primary}
                />
            }
            isCollapseable={true}
            defaultCollapsed={collapsedFields?.totals ?? false}
            isLoading={isLoading}
            innerSx={{
                position: 'relative',
                overflow: 'hidden',
            }}
            sx={{
                my: 0,                
                mx: 0,
            }}
        >
            {itemEntries.length > 0 ? (
                <Box
                    id="vault-peeker-totals-canvas"
                    sx={{
                        position: 'relative',
                        display: 'flex',
                        justifyContent: 'center',
                    }}
                >
                    <ItemCanvasGridV2
                        itemEntries={itemEntries}
                        onItemClick={handleItemClick}
                        showCounts={true}
                        maxHeight={10000}
                        minColumns={8}
                    />
                    <img
                        src={theme.palette.mode === 'dark' ? '/logo/logo_inner_big.png' : '/logo/logo_inner_big_dark.png'}
                        alt="EAM Logo"
                        style={{
                            maxHeight: '50%',
                            maxWidth: '50%',
                            position: 'absolute',
                            top: '50%',
                            left: '50%',
                            transform: 'translate(-50%, -50%)',
                            opacity: theme.palette.mode === 'dark' ? 0.05 : 0.1,
                            pointerEvents: 'none',
                            zIndex: 0,
                        }}
                    />
                </Box>
            ) : (
                <Box
                    sx={{
                        display: 'flex',
                        alignItems: 'center',
                        justifyContent: 'center',
                        py: 4,
                    }}
                >
                    <Typography variant="body2" color="text.secondary">
                        {isLoading ? 'Loading items...' : 'No items found'}
                    </Typography>
                </Box>
            )}

            {/* Export Modal */}
            <ExportModalV2
                open={exportModalOpen}
                onClose={() => setExportModalOpen(false)}
                items={exportItems}
            />
        </ComponentBox>
    );
}

export default TotalsViewV2;
