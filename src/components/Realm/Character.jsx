import { Box, Chip, Table, TableBody, TableCell, TableContainer, TableRow, Tooltip, Typography } from "@mui/material";
import { useEffect, useMemo, useState } from "react";
import { classes } from "../../assets/constants";
import { useTheme } from "@emotion/react";
import ItemCanvas from "./ItemCanvas";
import items from './../../assets/constants';
import EquipmentCanvas from "./EquipmentCanvas";
import CharacterPortrait from "./CharacterPortrait";
import useVaultPeeker from "../../hooks/useVaultPeeker";
import useColorList from './../../hooks/useColorList';

const emptyItemOverride = {
    all: {
        imgSrc: 'realm/itemSlot.png',
        size: 50,
        padding: 0,
    }
};

function Character({ charIdentifier, character }) {
    const [charClass, setCharClass] = useState([]);
    const [charItems, setCharItems] = useState([]);
    const [backpackItems, setBackpackItems] = useState([]);
    const [xof8, setXof8] = useState(0);

    const { filter, totalItems, addItemFilterCallback, removeItemFilterCallback } = useVaultPeeker();
    const seasonalChipColor = useColorList(1);
    const crucibleChipColor = useColorList(3);
    const theme = useTheme();

    useEffect(() => {
        if (!character.class) {
            setCharClass([]);
            setCharItems([]);
            setBackpackItems([]);
            return;
        }

        const cls = classes[character.class];
        setCharClass(cls);

        let x = 0;
        try {
            x += cls[3][0] - character.maxHp <= 0 ? 1 : 0;
            x += cls[3][1] - character.maxMp <= 0 ? 1 : 0;
            x += cls[3][2] - character.atk <= 0 ? 1 : 0;
            x += cls[3][3] - character.def <= 0 ? 1 : 0;
            x += cls[3][4] - character.spd <= 0 ? 1 : 0;
            x += cls[3][5] - character.dex <= 0 ? 1 : 0;
            x += cls[3][6] - character.vit <= 0 ? 1 : 0;
            x += cls[3][7] - character.wis <= 0 ? 1 : 0;
        } catch (e) {
            console.error(e);
        }
        setXof8(x);

        if (character.equipment) {
            //First 4 items are equipment
            if (character.equipment.length < 4) {
                const emptySlots = 4 - character.equipment.length;
                const eq = character.equipment;
                for (let i = 0; i < emptySlots; i++) {
                    eq.push(-1);
                }
                setCharItems(eq);
                setBackpackItems([]);

                addItemFilterCallback(charIdentifier + "_INV", (itemIds) => { setCharItems(itemIds); }, eq);
                return () => {
                    removeItemFilterCallback(charIdentifier + "_INV");
                };
            }

            //Rest of the items are in the inventory
            const cItems = character.equipment.slice(4, 12);
            const bItems = character.equipment.slice(12, character.equipment.length);
            setCharItems(cItems);
            setBackpackItems(bItems);

            addItemFilterCallback(charIdentifier + "_INV", (itemIds) => { setCharItems(itemIds); }, cItems);
            addItemFilterCallback(charIdentifier + "_BACK", (itemIds) => { setBackpackItems(itemIds); }, bItems);
            return () => {
                removeItemFilterCallback(charIdentifier + "_INV");
                removeItemFilterCallback(charIdentifier + "_BACK");
            };
        }
    }, [character]);

    const getCharacterClassName = () => {
        if (!character.class) {
            return '';
        }

        return classes[character.class][0];
    };

    if (!character //No character data
        || ((!backpackItems || backpackItems.length === 0) && (!charItems || charItems.length === 0)) //Items filtered out
        || filter.characterType.value === 1 && !character.seasonal //Seasonal character filter
        || filter.characterType.value === 2 && character.seasonal //Normal character filter
        || filter.characterType.value === 3 //Items not on characters filter
    ) {
        return null;
    }

    return (
        <Box
            sx={{
                display: 'flex',
                flexDirection: 'column',
                m: 1,
            }}
        >
            {/* Character Data */}
            <Box
                sx={{
                    display: 'flex',
                    flexDirection: 'row',
                    justifyContent: 'start',
                    alignItems: 'center',
                    mb: 0.5,
                    gap: 1,
                }}
            >
                <CharacterPortrait type={character.class} skin={character?.texture} tex1={character?.tex1} tex2={character?.tex2} adjust={false} />
                <Box
                    sx={{
                        display: 'flex',
                        flexDirection: 'row',
                        width: '100%',
                        justifyContent: 'space-between',
                        pr: 1,
                    }}
                >

                    <Box
                        sx={{
                            display: 'flex',
                            flexDirection: 'row',
                            width: '100%',
                        }}
                    >
                        <Box
                            sx={{
                                width: '100%',
                                display: 'flex',
                                flexDirection: 'column',
                                alignItems: 'start',
                                justifyContent: 'start',
                                gap: 1,
                            }}
                        > 
                        <Typography variant="h6">{getCharacterClassName()}</Typography>
                        <Typography variant="body2">#{character.char_id}</Typography>
                        </Box>
                        <Box
                            sx={{
                                display: 'flex',
                                flexDirection: 'column',
                                alignItems: 'center',
                                justifyContent: 'start',
                                gap: 0.5,
                            }}
                        >                            
                            {
                                character.seasonal &&
                                <Chip
                                    sx={{
                                        ...seasonalChipColor,
                                    }}
                                    clickable={false}
                                    label={'Seasonal'}
                                    size="small"
                                />
                            }
                            {
                                character.crucibleActive &&
                                <Chip
                                    sx={{
                                        ...crucibleChipColor,
                                    }}
                                    clickable={false}
                                    label={'Crucible'}
                                    size="small"
                                />
                            }
                        </Box>
                    </Box>
                </Box>
            </Box>
            <Box
                sx={{
                    display: 'flex',
                    flexDirection: 'row',
                    justifyContent: 'space-between',
                    pr: 1,
                    gap: 1,
                }}
            >
                <CharacterStats character={character} charClass={charClass} />
                <Box
                    sx={{
                        display: 'flex',
                        flexDirection: 'column',
                        justifyContent: 'start',
                        alignItems: 'end',
                    }}
                >
                    <Tooltip title="Fame">
                        <Box
                            sx={{
                                display: 'flex',
                                flexDirection: 'row',
                                alignItems: 'center',
                                justifyContent: 'center',
                                gap: 0.5,
                            }}
                        >

                            {
                                character.fame >= 0 && character.fame < 100_000 && //100k breaks the layout, so the fame icon is not displayed
                                <img src="/realm/fame.png" alt="Fame" height={20} />
                            }
                            <Typography variant="body1">{character.fame}</Typography>
                        </Box>
                    </Tooltip>
                    <Tooltip title="Level">
                        <Typography variant="h6" color={character.level === 20 ? theme.palette.warning.main : theme.palette.text.primary}>lvl {character.level}</Typography>
                    </Tooltip>
                    <Tooltip title="Maxed out stats">
                        <Typography variant="h6" color={xof8 === 8 ? theme.palette.warning.main : theme.palette.text.primary}> {xof8}/8</Typography>
                    </Tooltip>
                </Box>
            </Box>
            <Box
                sx={{
                    display: 'flex',
                    flexDirection: 'column',
                    width: '216px',
                    gap: 1,
                }}
            >
                <EquipmentCanvas canvasIdentifier={charIdentifier + "_EQ"} character={character} />
                {
                    ((charItems && charItems.length > 0) || (backpackItems && backpackItems.length > 0)) &&
                    <Box
                        sx={{
                            display: 'flex',
                            flexDirection: 'column',
                            backgroundColor: theme => theme.palette.background.default,
                            borderRadius: theme => `${theme.shape.borderRadius + 1}px`,
                            gap: 1,
                        }}
                    >
                        <ItemCanvas canvasIdentifier={charIdentifier + "_INV"} imgSrc="renders.png" itemIds={charItems} items={items} totals={totalItems?.totals} overrideItemImages={emptyItemOverride} override={{ fillNumbers: false }} />
                        {
                            backpackItems.length > 0 &&
                            <ItemCanvas canvasIdentifier={charIdentifier + "_BACK"} imgSrc="renders.png" itemIds={backpackItems} items={items} totals={totalItems?.totals} overrideItemImages={emptyItemOverride} override={{ fillNumbers: false }} />
                        }
                    </Box>
                }
            </Box>
        </Box>
    );
}

export default Character;

function CharacterStats({ character, charClass }) {
    const theme = useTheme();

    if (!character) {
        return null;
    }

    const stats = [
        { leftId: 0, leftName: 'HP', leftValue: character.maxHp, rightId: 1, rightName: 'MP', rightValue: character.maxMp, },
        { leftId: 2, leftName: 'ATK', leftValue: character.atk, rightId: 3, rightName: 'DEF', rightValue: character.def, },
        { leftId: 4, leftName: 'SPD', leftValue: character.spd, rightId: 5, rightName: 'DEX', rightValue: character.dex, },
        { leftId: 6, leftName: 'VIT', leftValue: character.vit, rightId: 7, rightName: 'WIS', rightValue: character.wis, },
    ];

    const getColor = (stat, classMaxStat) => {
        if (stat >= classMaxStat) {
            return theme.palette.warning.main;
        }

        return theme.palette.text.primary;
    }

    const getStatsTable = useMemo(() => {
        return (
            <Box sx={{ display: 'flex', height: 'fit-content', width: 'fit-content' }}>
                <TableContainer>
                    <Table>
                        <TableBody>
                            {
                                stats.map((stat) => (
                                    <TableRow key={stat.leftId}>
                                        <TableCell sx={{ borderBottom: 'none', m: 0, px: 0.5, py: 0.125 }}>{stat.leftName}</TableCell>
                                        <TableCell sx={{ borderBottom: 'none', m: 0, px: 0.5, py: 0.125, color: getColor(stat.leftValue, charClass[3]?.[stat.leftId]) }}>{stat.leftValue}</TableCell>
                                        <TableCell sx={{ borderBottom: 'none', m: 0, pl: 1, pr: 1, py: 0.125 }}>{stat.rightName}</TableCell>
                                        <TableCell sx={{ borderBottom: 'none', m: 0, px: 0.5, py: 0.125, color: getColor(stat.rightValue, charClass[3]?.[stat.rightId]) }}>{stat.rightValue}</TableCell>
                                    </TableRow>
                                ))
                            }
                        </TableBody>
                    </Table>
                </TableContainer>
            </Box>
        );
    }, [character, charClass]);

    return getStatsTable;
}
