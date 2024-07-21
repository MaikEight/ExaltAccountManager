import { useTheme } from "@emotion/react";
import useVaultPeeker from "../../hooks/useVaultPeeker";
import ComponentBox from "../ComponentBox";
import ItemCanvas from "../Realm/ItemCanvas";
import items from "../../assets/constants";
import { useEffect, useState } from "react";
import VaultPeekerLogo from "../VaultPeekerLogo";
import useItemCanvas from "../../hooks/useItemCanvas";
import useUserSettings from "../../hooks/useUserSettings";

function TotalsView() {
    const [filteredTotalItems, setFilteredTotalItems] = useState([]);
    const { totalItems, addItemFilterCallback, removeItemFilterCallback } = useVaultPeeker();
    const { setHoveredConvasId } = useItemCanvas();
    const theme = useTheme();
    const collapsedFileds = useUserSettings().getByKeyAndSubKey('vaultPeeker', 'collapsedFileds');

    useEffect(() => {
        setFilteredTotalItems(totalItems?.itemIds ?? []);

        if (totalItems?.itemIds) {
            addItemFilterCallback('totals', (itemIds) => { setFilteredTotalItems(itemIds); }, totalItems.itemIds);
        }

        return () => {
            removeItemFilterCallback('totals');
        };
    }, [totalItems]);

    return (
        <ComponentBox
            title='Totals'
            icon={
                <VaultPeekerLogo
                    sx={{ ml: '2px', mt: '3px', width: '20px', mr: 0.25 }}
                    color={
                        theme.palette.mode === 'light' ?
                        theme.palette.background.default
                        : theme.palette.text.primary
                    }
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
                itemIds={filteredTotalItems}
                items={items}
                totals={totalItems?.totals ? totalItems.totals : {}}
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