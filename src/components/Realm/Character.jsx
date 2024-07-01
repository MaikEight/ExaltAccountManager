import { Box, Table, TableBody, TableCell, TableContainer, TableRow, Typography } from "@mui/material";
import { useEffect, useState } from "react";
import { portrait } from "../../utils/portraitUtils";
import { classes } from "../../assets/constants";

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
    if (!character) {
        return null;
    }

    const getCharacterClassName = () => {
        if (!character.class) {
            return '';
        }

        return classes[character.class][0];
    };
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
            <CharacterStats character={character} />
        </Box>
    );
}

export default Character;

function CharacterStats({ character }) {
    if (!character) {
        return null;
    }

    const stats = [
        { leftName: 'HP', leftValue: character.maxHp, rightName: 'MP', rightValue: character.maxMp, },
        { leftName: 'ATK', leftValue: character.atk, rightName: 'DEF', rightValue: character.def, },
        { leftName: 'SPD', leftValue: character.spd, rightName: 'DEX', rightValue: character.dex, },
        { leftName: 'VIT', leftValue: character.vit, rightName: 'WIS', rightValue: character.wis, },
    ];

    const getStatsTable = () => {
        return (
            <TableContainer>
                <Table>
                    <TableBody>
                        {
                            stats.map((stat) => (
                                <TableRow key={stat}>
                                    <TableCell sx={{ borderBottom: 'none', m: 0, px: 0.5, py: 0.125 }}>{stat.leftName}</TableCell>
                                    <TableCell sx={{ borderBottom: 'none', m: 0, px: 0.5, py: 0.125 }}>{stat.leftValue}</TableCell>
                                    <TableCell sx={{ borderBottom: 'none', m: 0, pl: 1, pr: 1, py: 0.125 }}>{stat.rightName}</TableCell>
                                    <TableCell sx={{ borderBottom: 'none', m: 0, px: 0.5, py: 0.125 }}>{stat.rightValue}</TableCell>
                                </TableRow>
                            ))
                        }
                    </TableBody>
                </Table>
            </TableContainer>
        );
    };

    return getStatsTable();
}
