import { Box, Skeleton, Tooltip, Typography } from "@mui/material";
import usePortraitReady from "../../../../hooks/usePortraitReady";
import CharacterPortrait from "../../../Realm/CharacterPortrait";
import { getItemById } from "../../../../utils/realmItemUtils";
import { classes } from "../../../../assets/constants";
import itemsSlotTypeMap from "../../../../assets/slotmap";
import { drawItem } from "../../../../utils/realmItemDrawUtils";
import { useEffect, useState } from "react";
import { ADCENTUREERS_BELT, BACKPACK_EXTENDER_ITEM_ID, BACKPACK_ITEM_ID, extractEquipmentIds, getXof8OfCharacter } from "../../../../utils/realmCharacterUtils";

function SingleCharacterOverview({ character, number }) {
    const isPortraitReady = usePortraitReady();

    const [itemImages, setItemImages] = useState([null, null, null, null]);
    const [items, setItems] = useState([null, null, null, null]);
    const [xof8, setXof8] = useState(0);
    const [backpackItemImages, setBackpackItemImages] = useState([null, null, null]);

    useEffect(() => {
        console.log("char", character);
        const eq = extractEquipmentIds(character?.equipment);
        const eqItems = eq.map(id => getItemById(id));
        setItems(eqItems);

        const cls = classes[character.char_class];
        const charSlots = cls?.[4];
        const slotMapKeys = charSlots.map((slot) => Object.keys(itemsSlotTypeMap).find((key) => itemsSlotTypeMap[key].slotType === slot));
        const slotMapValues = slotMapKeys.map((key) => itemsSlotTypeMap[key]);

        //Item images
        eqItems.forEach((item, index) => {
            const slot = slotMapValues[index];
            const itemId = eq[index];

            if (!item || itemId === -1) {
                // Empty slot - use silhouette
                const slotItem = [
                    -1,
                    null,
                    null,
                    slot.sheet[0],
                    slot.sheet[1],
                ];
                drawItem(
                    "realm/itemsilhouettes_25p.png",
                    slotItem,
                    (imageUrl) => {
                        setItemImages((prevImages) => {
                            const newImages = [...prevImages];
                            newImages[index] = imageUrl;
                            return newImages;
                        });
                    }
                );
                return;
            }

            drawItem("renders.png", item, (imageUrl) => {
                setItemImages((prevImages) => {
                    const newImages = [...prevImages];
                    newImages[index] = imageUrl;
                    return newImages;
                });
            });
        })

        setXof8(getXof8OfCharacter(character));

        drawItem("renders.png", getItemById(BACKPACK_ITEM_ID), (imageUrl) => {
            setBackpackItemImages((prevImages) => {
                const newImages = [...prevImages];
                newImages[0] = imageUrl;
                return newImages;
            });
        });
        drawItem("renders.png", getItemById(BACKPACK_EXTENDER_ITEM_ID), (imageUrl) => {
            setBackpackItemImages((prevImages) => {
                const newImages = [...prevImages];
                newImages[1] = imageUrl;
                return newImages;
            });
        });
        drawItem("renders.png", getItemById(ADCENTUREERS_BELT), (imageUrl) => {
            setBackpackItemImages((prevImages) => {
                const newImages = [...prevImages];
                newImages[2] = imageUrl;
                return newImages;
            });
        });

    }, [character]);

    if (!isPortraitReady) {
        return;
    }

    return (
        <Box
            sx={{
                display: 'flex',
                flexDirection: 'row',
                alignItems: 'center',
                justifyContent: 'flex-start',
                p: 0.5,
                gap: 1.5,
                width: 'fit-content',
            }}
        >
            <Typography variant="h6">{number}.</Typography>
            <CharacterPortrait type={character.class} skin={character?.texture} tex1={character?.tex1} tex2={character?.tex2} adjust={false} />
            {/* Equipment */}
            <Box
                sx={{
                    display: 'flex',
                    flexDirection: 'row',
                    justifyContent: 'center',
                    alignItems: 'center',
                    backgroundColor: theme => theme.palette.background.default,
                    borderRadius: 1,
                    gap: 0.25,
                    ml: 1
                }}
            >
                {
                    itemImages.map((img, index) => {
                        if (!img) {
                            return (
                                <Skeleton key={index} variant="rounded" width={50} height={50} />
                            )
                        }

                        return (
                            <Tooltip key={index} title={img ? `${items[index][0]}` : "Loading..."}>
                                <img
                                    key={index}
                                    src={img}
                                    alt={`Item Slot ${index + 1}`}
                                    width={50}
                                    height={50}
                                />
                            </Tooltip>
                        )
                    })
                }
            </Box>
            {/* Stats */}
            <Box>
                <Typography variant="body2" fontWeight="bold" color={xof8 === 8 ? 'warning' : 'inherit'}>
                    {xof8} / 8
                </Typography>
                <Typography variant="caption" fontSize={16}>
                    {character.current_fame} <img src="/realm/fame.png" alt="Fame" width={16} height={16} style={{ marginBottom: -2 }} />
                </Typography>
            </Box>
            <Box
                sx={{
                    display: 'flex',
                    flexDirection: 'column',
                    flexWrap: 'wrap',
                    justifyContent: 'center',
                    alignItems: 'end',
                    maxHeight: '50px',
                    width: 'fit-content',
                    ml: "-8px"
                }}
            >
                {
                    character.backpack_slots > 0 && backpackItemImages[0] &&
                    <img src={backpackItemImages[0]} alt="Backpack" width={25} height={25} />
                }
                {
                    character.backpack_slots > 8 && backpackItemImages[1] &&
                    <img src={backpackItemImages[1]} alt="Backpack Extender" width={25} height={25} />
                }
                {
                    character.has3_quickslots > 0 && backpackItemImages[2] &&
                    <img src={backpackItemImages[2]} alt="Adventurer's Belt" width={25} height={25} />
                }
            </Box>
        </Box>
    );
}

export default SingleCharacterOverview;