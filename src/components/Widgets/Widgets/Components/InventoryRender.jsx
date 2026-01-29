import { useEffect, useState } from "react";
import { getItemById } from "../../../../utils/realmItemUtils";
import { alpha, Box, darken, lighten, Tooltip, Typography } from "@mui/material";
import { drawItem } from "../../../../utils/realmItemDrawUtils";
import { TierText } from "../../../Realm/ItemLocationPopper";
import LockOutlinedIcon from '@mui/icons-material/LockOutlined';


function InventoryRender({ character }) {
    const [itemImages, setItemImages] = useState([null, null, null, null, null, null, null, null]);
    const [items, setItems] = useState([null, null, null, null, null, null, null, null]);

    const renderItemImages = async () => {
        if (!character) {
            setItemImages([8].fill(null));
            setItems([8].fill(null));
            return;
        }

        const equipmentIds = character.equipment.split(',').map(id => parseInt(id, 10)).slice(4);
        const items = equipmentIds.map(id => getItemById(id));
        setItems(items);
        setItemImages([items.length].fill(null));
        const promises = [];
        items.forEach((item, index) => {
            if (!item || equipmentIds[index] === -1) {
                // Empty slot
                promises.push(Promise.resolve("-1"));
                return;
            }

            promises.push(new Promise((resolve) => {
                drawItem("renders.png", item, (imageUrl) => {
                    resolve(imageUrl);
                });
            }));
        });

        Promise.all(promises).then((images) => {
            setItemImages(images);
        });
    };

    const getTooltipUiForItem = (item) => {
        if (!item) return "Loading...";

        return (
            <Box
                sx={{
                    display: 'flex',
                    flexDirection: 'row',
                    alignItems: 'center',
                    gap: 1,
                }}
            >
                <TierText item={item} variant="body2" />
                <Typography variant="body2" fontWeight="bold" color="text.primary">
                    {item[0]}
                </Typography>
                {
                    item[8] &&
                    <LockOutlinedIcon />
                }
            </Box>
        )
    }

    useEffect(() => {
        renderItemImages();
    }, [character]);

    return (
        <Box
            sx={{
                display: 'grid',
                gridTemplateColumns: 'repeat(8, 47px)',
                gridTemplateRows: 'repeat(1, 47px)',
                gap: '4px',
                borderRadius: 1,
                backgroundColor: theme => theme.palette.background.default,
                padding: '4px',
                height: 'fit-content',
            }}
        >
            {
                itemImages.map((img, index) => {
                    if (img === "-1") {
                        return (
                            <Tooltip key={index} title="Empty Slot">
                                <Box
                                    key={index}
                                    sx={{
                                        width: 47,
                                        height: 47,
                                        backgroundColor: theme => alpha(theme.palette.background.paper, 0.4),
                                        border: theme => `1px solid ${theme.palette.mode === 'dark' ? lighten(theme.palette.background.paper, 0.1) : darken(theme.palette.background.paper, 0.2)}`,
                                        borderRadius: theme => `${theme.shape.borderRadius - 2}px`,
                                        display: 'inline-block',
                                        marginRight: '4px',
                                    }}
                                />
                            </Tooltip>
                        );
                    }

                    return (
                        <Tooltip key={index} title={img ? getTooltipUiForItem(items[index] || null) : "Loading..."}>
                            <img
                                key={index}
                                src={img}
                                alt={`Item Slot ${index + 1}`}
                                width={47}
                                height={47}
                            />
                        </Tooltip>
                    )
                })
            }
        </Box>
    )
}

export default InventoryRender;