import { useTheme } from "@emotion/react";
import useVaultPeeker from "../../hooks/useVaultPeeker";
import ComponentBox from "../ComponentBox";
import ItemCanvas from "../Realm/ItemCanvas";
import items from "../../assets/constants";
import { useEffect, useState } from "react";
import VaultPeekerLogo from "../VaultPeekerLogo";
import useItemCanvas from "../../hooks/useItemCanvas";
import useUserSettings from "../../hooks/useUserSettings";
import { Box, IconButton, Tooltip, Typography } from "@mui/material";
import FileUploadOutlinedIcon from '@mui/icons-material/FileUploadOutlined';

function TotalsView() {
    const [filteredItems, setFilteredItems] = useState([]);
    const [saveCanvas, setSaveCanvas] = useState(0);
    const { totalItems, addItemFilterCallback, removeItemFilterCallback, filteredTotalItems } = useVaultPeeker();
    const { setHoveredConvasId } = useItemCanvas();
    const theme = useTheme();
    const collapsedFileds = useUserSettings().getByKeyAndSubKey('vaultPeeker', 'collapsedFileds');

    useEffect(() => {
        setFilteredItems(totalItems?.itemIds ?? []);

        if (totalItems?.itemIds) {
            addItemFilterCallback('totals', (itemIds) => { setFilteredItems(itemIds); }, totalItems.itemIds);
        }

        return () => {
            removeItemFilterCallback('totals');
        };
    }, [totalItems]);

    return (
        <ComponentBox
            title={
                (
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
                                onClick={(event) => { 
                                    event.stopPropagation();
                                    setSaveCanvas((prev) => prev + 1); 
                                }}                                
                            >
                                <FileUploadOutlinedIcon />
                            </IconButton>
                        </Tooltip>
                    </Box>
                )
            }
            icon={
                <VaultPeekerLogo
                    sx={{ display: 'flex', ml: '2px', width: '20px', height: '24px', mr: 0.25 }}
                    color={theme.palette.text.primary}
                />
            }
            isCollapseable={true}
            defaultCollapsed={collapsedFileds !== undefined ? collapsedFileds.totals : false}
            innerSx={{ position: 'relative', overflow: 'hidden', }}
            sx={{
                mt: 0,
                mx: 0,
            }}
            onMouseMove={() => { setHoveredConvasId(null); }}
        >
            <ItemCanvas
                canvasIdentifier="totals"
                imgSrc="renders.png"
                itemIds={filteredItems}
                items={items}
                totals={totalItems?.totals ? totalItems.totals : {}}
                filteredTotalItems={filteredTotalItems}
                saveCanvas={saveCanvas}
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
                    zIndex: 1,
                }}
            />
        </ComponentBox>
    );
}

export default TotalsView;