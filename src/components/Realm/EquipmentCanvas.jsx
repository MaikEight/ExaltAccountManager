import { useEffect, useRef, useState } from "react";
import itemsSlotTypeMap from "../../assets/slotmap";
import items, { classes } from "../../assets/constants";
import { drawItem } from "../../utils/realmItemDrawUtils";
import { Box } from "@mui/material";
import useItemCanvas from "../../hooks/useItemCanvas";
import useVaultPeeker from "../../hooks/useVaultPeeker";

function EquipmentCanvas({ canvasIdentifier, character }) {
    const [itemData, setItemData] = useState([null, null, null, null]);
    const [hoveredId, setHoveredId] = useState(-1);
    const { hoveredConvasId, setHoveredConvasId } = useItemCanvas();
    const { selectedItem, setPopperPosition, setSelectedItem, totalItems } = useVaultPeeker();
    const itemRef1 = useRef(null);
    const itemRef2 = useRef(null);
    const itemRef3 = useRef(null);
    const itemRef4 = useRef(null);

    useEffect(() => {        
        const logMode = canvasIdentifier?.includes('guima_cs@hotmail.com') ?? false;

        const charSlots = classes[character.class]?.[4];
        const slotMapKeys = charSlots.map((slot) => Object.keys(itemsSlotTypeMap).find((key) => itemsSlotTypeMap[key].slotType === slot));
        const slotMapValues = slotMapKeys.map((key) => itemsSlotTypeMap[key]);
        for (let i = 0; i < 4; i++) {
            const slot = slotMapValues[i];
            const itemId = character.equipment[i];
            const item = items[itemId];
            if(logMode) console.log(itemId, item, slot);
            if (itemId && itemId !== -1) {
                drawItem(
                    "renders.png",
                    item,
                    (imageUrl) => {
                        setItemData((prev) => {
                            const newState = [...prev];
                            newState[i] = {
                                itemId: itemId,
                                img: imageUrl
                            };
                            return newState;
                        })
                    }
                );
                continue;
            }

            const slotItem = [
                itemId,
                null,
                null,
                slot.sheet[0],
                slot.sheet[1],
            ];

            drawItem(
                "realm/itemsilhouettes_25p.png",
                slotItem,
                (imageUrl) => {
                    setItemData((prev) => {
                        const newState = [...prev];
                        newState[i] = {
                            itemId: itemId,
                            img: imageUrl
                        };
                        return newState;
                    })
                },
                5
            );
        }
    }, [character]);

    useEffect(() => {
        if (hoveredConvasId !== canvasIdentifier) {
            setHoveredId(-1);
        }
    }, [hoveredConvasId, canvasIdentifier]);


    return (
        <Box
            sx={{
                display: 'flex',
                flexDirection: 'row',
                backgroundColor: theme => theme.palette.background.default,
                borderRadius: theme => `${theme.shape.borderRadius}px`,
            }}
        >
            {
                itemData.map((data, index) => {
                    const uniqueId = `${canvasIdentifier}__${index}`;
                    return (
                        <Box
                            ref={index === 0 ? itemRef1 : index === 1 ? itemRef2 : index === 2 ? itemRef3 : itemRef4}
                            key={index}
                            sx={{
                                p: '2px',
                                display: 'flex',
                                justifyContent: 'center',
                                alignItems: 'center',
                                borderRadius: theme => `${theme.shape.borderRadius}px`,
                                ...((hoveredId === index || (selectedItem?.itemId === data?.itemId && selectedItem?.uniqueId === uniqueId)) && { backgroundColor: theme => theme.palette.mode === 'dark' ? 'rgba(220, 220, 220, 0.2)' : 'rgba(0, 0, 0, 0.2)' }),
                            }}
                            onMouseEnter={() => {
                                setHoveredConvasId(canvasIdentifier);
                                setHoveredId(index);
                            }}
                            onMouseLeave={() => setHoveredId(-1)}
                            onClick={() => {
                                const imgRef = index === 0 ? itemRef1.current : index === 1 ? itemRef2.current : index === 2 ? itemRef3.current : itemRef4.current;
                                if (!imgRef) return;
                                const rect = imgRef.getBoundingClientRect();
                                setPopperPosition({ top: rect.top, left: index > 2 ? rect.left : rect.right, isLeftHalf: index < 2 });
                                setSelectedItem({
                                    itemId: data.itemId ?? -1,
                                    uniqueId: uniqueId,
                                    totals: totalItems?.totals[data.itemId] ?? {}
                                });
                            }}
                        >
                            {
                                data &&
                                <img src={data.img} width={50} height={50} alt="Equipment Image" />
                            }
                        </Box>
                    );
                })
            }
        </Box>
    )
}

export default EquipmentCanvas;