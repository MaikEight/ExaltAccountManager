import { Box, Table, TableBody, TableCell, TableContainer, TableRow, Typography } from "@mui/material";
import { useEffect, useMemo, useState } from "react";
import { portrait } from "../../utils/portraitUtils";
import { classes } from "../../assets/constants";
import { useTheme } from "@emotion/react";
import ItemCanvas from "./ItemCanvas";
import items from './../../assets/constants';
import EquipmentCanvas from "./EquipmentCanvas";
import CharacterPortrait from "./CharacterPortrait";

const emptyItemOverride = {
    all: {
        imgSrc: 'realm/itemSlot.png',
        size: 50,
        padding: 0,
    }
};

function Character({ character }) {
    const [charClass, setCharClass] = useState([]);
    const [equipment, setEquipment] = useState([]);
    const [charItems, setCharItems] = useState([]);
    const [backpackItems, setBackpackItems] = useState([]);

    useEffect(() => {
        if (!character.class) {
            setCharClass([]);
            setEquipment([]);
            setCharItems([]);
            setBackpackItems([]);
            return;
        }
        setCharClass(classes[character.class]);
        if (character.equipment) {
            //First 4 items are equipment
            if (character.equipment.length >= 4) {
                setEquipment(character.equipment.slice(0, 4));
            } else {
                const emptySlots = 4 - character.equipment.length;
                const eq = character.equipment;
                for (let i = 0; i < emptySlots; i++) {
                    eq.push(-1);
                }
                setEquipment([]);
                setCharItems([]);
                setBackpackItems([]);
                return;
            }

            //Rest of the items are in the inventory
            setCharItems(character.equipment.slice(4, 12));
            setBackpackItems(character.equipment.slice(12, character.equipment.length));
        }

    }, [character]);

    const getCharacterClassName = () => {
        if (!character.class) {
            return '';
        }

        return classes[character.class][0];
    };

    if (!character) {
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
                }}
            >
                <CharacterPortrait type={character.class} skin={character?.texture} tex1={character?.tex1} tex2={character?.tex2} adjust={false} />
                <Typography variant="h6">{getCharacterClassName()}</Typography>
            </Box>
            <CharacterStats character={character} charClass={charClass} />
            
            <Box
                sx={{
                    display: 'flex',
                    flexDirection: 'column',
                    width: '216px',
                }}
            >
                <EquipmentCanvas character={character}/>
                <ItemCanvas imgSrc="renders.png" itemIds={charItems} items={items} overrideItemImages={emptyItemOverride} />
                {
                    backpackItems.length > 0 &&
                    <ItemCanvas imgSrc="renders.png" itemIds={backpackItems} items={items} overrideItemImages={emptyItemOverride} />
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
