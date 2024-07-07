import { useEffect, useState } from "react";
import itemsSlotTypeMap from "../../assets/slotmap";
import items, { classes } from "../../assets/constants";
import { drawItem } from "../../utils/realmItemDrawUtils";
import { Box } from "@mui/material";

function EquipmentCanvas({ character }) {
    const [imageData, setImageData] = useState([null, null, null, null]);
    const [equipment, setEquipment] = useState([]);
    const [hoveredId, setHoveredId] = useState(-1);

    useEffect(() => {
        const charSlots = classes[character.class]?.[4];
        const slotMapKeys = charSlots.map((slot) => Object.keys(itemsSlotTypeMap).find((key) => itemsSlotTypeMap[key].slotType === slot));
        const slotMapValues = slotMapKeys.map((key) => itemsSlotTypeMap[key]);

        for (let i = 0; i < 4; i++) {
            const slot = slotMapValues[i];
            const itemId = character.equipment[i];
            const item = items[itemId];
            if (itemId !== -1) {
                drawItem(
                    "renders.png",
                    item,
                    (imageUrl) => {
                        setImageData((prev) => {
                            const newState = [...prev];
                            newState[i] = imageUrl;
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
                    setImageData((prev) => {
                        const newState = [...prev];
                        newState[i] = imageUrl;
                        return newState;
                    })
                },
                5
            );
        }
    }, [character]);

    return (
        <Box
            sx={{
                display: 'flex',
                flexDirection: 'row',
                // gap: '4px',
            }}
        >
            {
                imageData.map((data, index) => {
                    return (
                        <Box
                            key={index}
                            sx={{
                                p: '2px',
                                display: 'flex',
                                justifyContent: 'center',
                                alignItems: 'center',
                                borderRadius: theme => `${theme.shape.borderRadius}px`,
                                ...(hoveredId === index && { backgroundColor: theme => theme.palette.mode === 'dark' ? 'rgba(220, 220, 220, 0.2)' : 'rgba(0, 0, 0, 0.2)'}),
                            }}
                            onMouseEnter={() => setHoveredId(index)}
                            onMouseLeave={() => setHoveredId(-1)}
                        >
                            {
                                data &&
                                <img src={data} width={50} height={50} alt="Equipment Image" />
                            }
                        </Box>
                    );
                })
            }
        </Box>
    )
}

export default EquipmentCanvas;