import { useTheme } from "@emotion/react";
import useVaultPeeker from "../../hooks/useVaultPeeker";
import ComponentBox from "../ComponentBox";
import ItemCanvas from "../Realm/ItemCanvas";
import items from "../../assets/constants";

function TotalsView() {
    const { totalItems } = useVaultPeeker();
    const theme = useTheme();

    return (
        <ComponentBox
            title='Totals'
            isCollapseable={true}
            defaultCollapsed
            innerSx={{ position: 'relative', overflow: 'hidden', }}
        >
            <ItemCanvas
                imgSrc="renders.png"
                itemIds={totalItems?.itemIds ? totalItems.itemIds : []}
                items={items} totals={totalItems?.totals ? totalItems.totals : {}}
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