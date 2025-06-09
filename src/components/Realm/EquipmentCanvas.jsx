import { useEffect, useRef, useState } from "react";
import itemsSlotTypeMap from "../../assets/slotmap";
import items, { classes } from "../../assets/constants";
import { drawItemAsync } from "../../utils/realmItemDrawUtils";
import { Box } from "@mui/material";
import useVaultPeeker from "../../hooks/useVaultPeeker";
import useVaultPeekerPopper from "../../hooks/useVaultPeekerPopper";

function EquipmentCanvas({ canvasIdentifier, character }) {
    const [itemData, setItemData] = useState([null, null, null, null]);
    const [slotMapData, setSlotMapData] = useState([null, null, null, null]);
    const [filteredItemIds, setFilteredItemIds] = useState([-1, -1, -1, -1]);
    const [hoveredId, setHoveredId] = useState(-1);    
    const { selectedItem, setSelectedItem, totalItems, addItemFilterCallback, removeItemFilterCallback } = useVaultPeeker();
    const { setPopperPosition } = useVaultPeekerPopper();
    
    const itemRef1 = useRef(null);
    const itemRef2 = useRef(null);
    const itemRef3 = useRef(null);
    const itemRef4 = useRef(null);

    useEffect(() => {
        const charSlots = classes[character.class]?.[4];
        const slotMapKeys = charSlots.map((slot) => Object.keys(itemsSlotTypeMap).find((key) => itemsSlotTypeMap[key].slotType === slot));
        const slotMapValues = slotMapKeys.map((key) => itemsSlotTypeMap[key]);
        const eqItemIds = character.equipment.slice(0, 4);

        while (eqItemIds.length < 4) {
            eqItemIds.push(-1);
        }
        
        setFilteredItemIds(eqItemIds);
        addItemFilterCallback(canvasIdentifier, (itemIds) => { setFilteredItemIds(itemIds); }, eqItemIds);        

        const promises = [];
        const promiseItems = [];
        for (let i = 0; i < 4; i++) {
            const slot = slotMapValues[i];
            const itemId = character.equipment[i];           

            const slotItem = [
                itemId,
                null,
                null,
                slot.sheet[0],
                slot.sheet[1],
            ];

            promises.push(drawItemAsync(
                "realm/itemsilhouettes_25p.png",
                slotItem,
                5
            ));
            promiseItems.push(itemId);
        }
        Promise.all(promises).then((images) => {
            const result = images.map((image, index) => {
                return {
                    itemId: promiseItems[index],
                    img: image,
                };
            });
            setSlotMapData(result);
        });

        const itemPromises = [];
        const itemPromisesIds = [];
        for (let i = 0; i < 4; i++) {
            const slot = slotMapValues[i];
            const itemId = character.equipment[i];
            const item = items[itemId];         
            if (itemId && itemId !== -1) {            
                itemPromises.push(drawItemAsync(
                    "renders.png",
                    item,
                    5
                ));
                itemPromisesIds.push(itemId);
                continue;
            }

            const slotItem = [
                itemId,
                null,
                null,
                slot.sheet[0],
                slot.sheet[1],
            ];

            itemPromises.push(drawItemAsync(
                "realm/itemsilhouettes_25p.png",
                slotItem,
                5
            ));
            itemPromisesIds.push(itemId);
        }
        Promise.all(itemPromises).then((images) => {
            const result = images.map((image, index) => {
                return {
                    itemId: itemPromisesIds[index],
                    img: image,
                };
            });
            setItemData(result);
        });

        return () => {
            removeItemFilterCallback(canvasIdentifier);
        };
    }, [character]);   

    useEffect(() => {
        const data = itemData.map((data) => {
            if(!data) return null;
            return {
                itemId: data.itemId,
                img: data.img,
                hidden: !filteredItemIds.includes(data.itemId),
            };
        });
        setItemData(data);
    }, [filteredItemIds]);

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
                                setHoveredId(index);
                            }}
                            onMouseLeave={() => setHoveredId(-1)}
                            onClick={() => {
                                if(!data || data.hidden) return;
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
                                data && !data.hidden ?
                                <img src={data.img} width={50} height={50} alt="Equipment Image" />
                                :
                                slotMapData && slotMapData[index] &&
                                <img src={slotMapData[index].img} width={50} height={50} alt="Equipment Image" />
                            }
                        </Box>
                    );
                })
            }
        </Box>
    )
}

export default EquipmentCanvas;