import { Box, Table, TableBody, TableCell, TableContainer, TableRow, Typography } from "@mui/material";
import { useEffect, useMemo, useState } from "react";
import { portrait } from "../../utils/portraitUtils";
import { classes } from "../../assets/constants";
import { useTheme } from "@emotion/react";
import ItemCanvas from "./ItemCanvas";
import items from './../../assets/constants';

const CharacterPortrait = (type, skin, tex1, tex2, adjust) => {
    const [url, setUrl] = useState('');

    useEffect(() => {
        const _url = portrait(type, skin, tex1, tex2, adjust);
        console.log('url:', _url);
        setUrl(_url);
    }, [type, skin, tex1, tex2, adjust]);

    return (
        <img src={url} width={34} height={34} id="characterPortrait" alt="Character Portrait" />
    );
};

function Character({ character }) {
    const [charClass, setCharClass] = useState([]);

    if (!character) {
        return null;
    }

    useEffect(() => {
        if (!character.class) {
            setCharClass([]);
        }
        setCharClass(classes[character.class]);
    }, [character]);

    const getCharacterClassName = () => {
        if (!character.class) {
            return '';
        }

        return classes[character.class][0];
    };
    console.log(character);
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
                {CharacterPortrait(29816, 29816, 167772208, 33554431, false)}
                <Typography variant="h6">{getCharacterClassName()}</Typography>
            </Box>
            {/* {CharacterPortrait(872, 872, 167772197, 167772197, false)} */}
            <CharacterStats character={character} charClass={charClass} />
            <ItemCanvas imgSrc="renders.png" itemIds={character.equipment} items={items} />
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
            <Box sx={{display: 'flex', height: 'fit-content', width: 'fit-content'}}>
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
