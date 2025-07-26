import { Box, Chip, Popover, Table, TableBody, TableCell, TableContainer, TableHead, TableRow, Tooltip, Typography } from "@mui/material";
import React, { useEffect, useMemo, useState } from "react";
import { classes } from "../../assets/constants";
import { useTheme } from "@emotion/react";
import ItemCanvas from "./ItemCanvas";
import items from './../../assets/constants';
import EquipmentCanvas from "./EquipmentCanvas";
import CharacterPortrait from "./CharacterPortrait";
import useVaultPeeker from "../../hooks/useVaultPeeker";
import PaddedTableCell from "../AccountDetails/PaddedTableCell";
import { parsePcStats, pcStatsDescriptionEnum } from "../../utils/pcStatsParser";
import CheckIcon from '@mui/icons-material/Check';
import ClearIcon from '@mui/icons-material/Clear';
import QuestionMarkIcon from '@mui/icons-material/QuestionMark';
import { Fragment } from "react";
import { useColorList } from "../../hooks/useColorList";

function emptyItemOverride(darkMode) {
    return {
        all: {
            imgSrc: darkMode ? 'realm/itemSlot.png' : 'realm/itemSlot_light.png',
            size: 50,
            padding: 0,
        }
    };
}

function Character({ charIdentifier, character }) {
    const { filter, totalItems, addItemFilterCallback, removeItemFilterCallback } = useVaultPeeker();
    const seasonalChipColor = useColorList(1);
    const crucibleChipColor = useColorList(3);
    const theme = useTheme();

    const [charClass, setCharClass] = useState([]);
    const [charItems, setCharItems] = useState([]);
    const [backpackItems, setBackpackItems] = useState([]);
    const [xof8, setXof8] = useState(0);
    const [anchorEl, setAnchorEl] = useState(null);

    const handleClick = (event) => {
        setAnchorEl(event.currentTarget);
    }

    const handleClose = () => {
        setAnchorEl(null);
    };


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
                    <Tooltip title="View Dungeon Bonuses | ALPHA (Click to open)">
                        <Box
                            sx={{
                                display: 'flex',
                                flexDirection: 'row',
                                alignItems: 'center',
                                justifyContent: 'center',
                                gap: 0.5,
                                ":hover": {
                                    backgroundColor: theme.palette.mode === 'dark' ? 'rgba(220, 220, 220, 0.2)' : 'rgba(0, 0, 0, 0.2)'
                                }
                            }}
                            onClick={handleClick}
                        >
                            {
                                character.fame >= 0 && character.fame < 100_000 && //100k breaks the layout, so the fame icon is not displayed
                                <img src="/realm/fame.png" alt="Fame" height={20} />
                            }
                            <Typography variant="body1">{character.fame}</Typography>
                        </Box>
                    </Tooltip>
                    <FameAndFameBonusPopover
                        character={character}
                        anchorEl={anchorEl}
                        handleClose={handleClose}
                    />
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
                        <ItemCanvas canvasIdentifier={charIdentifier + "_INV"} imgSrc="renders.png" itemIds={charItems} items={items} totals={totalItems?.totals} overrideItemImages={emptyItemOverride(theme.palette.mode === 'dark')} override={{ fillNumbers: false }} />
                        {
                            backpackItems.length > 0 &&
                            <ItemCanvas canvasIdentifier={charIdentifier + "_BACK"} imgSrc="renders.png" itemIds={backpackItems} items={items} totals={totalItems?.totals} overrideItemImages={emptyItemOverride(theme.palette.mode === 'dark')} override={{ fillNumbers: false }} />
                        }
                    </Box>
                }
            </Box>
        </Box>
    );
}

export default Character;

function FameAndFameBonusPopover({ character, anchorEl, handleClose }) {
    const theme = useTheme();

    const open = Boolean(anchorEl);

    useEffect(() => {
        if (!character.processed_pc_stats && open) {
            try {
                const pcStatsParsed = parsePcStats(character.raw_pc_stats);
                character.processed_pc_stats = pcStatsParsed;
            } catch (error) {
                console.error('Error parsing PC stats:', error);
                character.processed_pc_stats = new Map();
            }
        }
    }, [character, anchorEl]);

    if (!character) {
        return null;
    }
    // console.log(character.processed_pc_stats);

    const tunnelRat = [pcStatsDescriptionEnum.PIRATE_CAVES, pcStatsDescriptionEnum.FORBIDDEN_JUNGLES, pcStatsDescriptionEnum.SPIDER_DENS,
    pcStatsDescriptionEnum.SNAKE_PITS, pcStatsDescriptionEnum.UNDEAD_LAIRS, pcStatsDescriptionEnum.ABYSS_OF_DEMONS, pcStatsDescriptionEnum.MANOR_OF_THE_IMMORTALS,
    pcStatsDescriptionEnum.OCEAN_TRENCHES, pcStatsDescriptionEnum.TOMB_OF_THE_ANCIENTS, pcStatsDescriptionEnum.ORYXS_CASTLE, pcStatsDescriptionEnum.ORYXS_CHAMBER,
    pcStatsDescriptionEnum.WINE_CELLAR];

    const explosiveJourney = [pcStatsDescriptionEnum.DAVY_JONES_LOCKERS, pcStatsDescriptionEnum.MAD_LABS, pcStatsDescriptionEnum.CANDYLAND_HUNTING_GROUNDS, pcStatsDescriptionEnum.HAUNTED_CEMETERIES,
    pcStatsDescriptionEnum.CAVE_OF_A_THOUSAND_TREASURES, pcStatsDescriptionEnum.LAIR_OF_DRACONIS, pcStatsDescriptionEnum.DEADWATER_DOCKS, pcStatsDescriptionEnum.WOODLAND_LABYRINTHS,
    pcStatsDescriptionEnum.THE_CRAWLING_DEPTHS, pcStatsDescriptionEnum.THE_SHATTERS, pcStatsDescriptionEnum.LAIR_OF_SHAITAN, pcStatsDescriptionEnum.PUPPET_MASTERS_THEATRE,
    pcStatsDescriptionEnum.ICE_CAVES]

    const travelOfTheDecade = [pcStatsDescriptionEnum.PUPPET_MASTERS_ENCORE, pcStatsDescriptionEnum.THE_HIVE, pcStatsDescriptionEnum.TOXIC_SEWERS, pcStatsDescriptionEnum.MOUNTAIN_TEMPLE,
    pcStatsDescriptionEnum.THE_THIRD_DIMENSION, pcStatsDescriptionEnum.THE_NEST, pcStatsDescriptionEnum.LOST_HALLS, pcStatsDescriptionEnum.CULTIST_HIDEOUT, pcStatsDescriptionEnum.THE_VOID,
    pcStatsDescriptionEnum.CNIDARIAN_REEF, pcStatsDescriptionEnum.PARASITE_CHAMBERS, pcStatsDescriptionEnum.MAGIC_WOODS, pcStatsDescriptionEnum.SECLUDED_THICKET, pcStatsDescriptionEnum.CURSED_LIBRARY,
    pcStatsDescriptionEnum.ORYXS_SANCTUARY, pcStatsDescriptionEnum.ANCIENT_RUINS, pcStatsDescriptionEnum.HIGH_TECH_TERROR, pcStatsDescriptionEnum.SULFUROUS_WETLANDS, pcStatsDescriptionEnum.SPECTRAL_PENITENTIARY];

    const firstSteps = [pcStatsDescriptionEnum.PIRATE_CAVES, pcStatsDescriptionEnum.FOREST_MAZES, pcStatsDescriptionEnum.FORBIDDEN_JUNGLES, pcStatsDescriptionEnum.SPIDER_DENS,
    pcStatsDescriptionEnum.THE_HIVE];

    const kingOfTheMountains = [pcStatsDescriptionEnum.SNAKE_PITS, pcStatsDescriptionEnum.SPRITE_WORLDS, pcStatsDescriptionEnum.ABYSS_OF_DEMONS, pcStatsDescriptionEnum.TOXIC_SEWERS,
    pcStatsDescriptionEnum.MAD_LABS, pcStatsDescriptionEnum.MAGIC_WOODS, pcStatsDescriptionEnum.PUPPET_MASTERS_THEATRE, pcStatsDescriptionEnum.HAUNTED_CEMETERIES, pcStatsDescriptionEnum.CURSED_LIBRARY,
    pcStatsDescriptionEnum.ANCIENT_RUINS, pcStatsDescriptionEnum.SULFUROUS_WETLANDS, pcStatsDescriptionEnum.SPECTRAL_PENITENTIARY];

    const conquererOfTheRealm = [pcStatsDescriptionEnum.DAVY_JONES_LOCKERS, pcStatsDescriptionEnum.ICE_CAVES, pcStatsDescriptionEnum.LAIR_OF_DRACONIS, pcStatsDescriptionEnum.MOUNTAIN_TEMPLE,
    pcStatsDescriptionEnum.THE_THIRD_DIMENSION, pcStatsDescriptionEnum.OCEAN_TRENCHES, pcStatsDescriptionEnum.TOMB_OF_THE_ANCIENTS, pcStatsDescriptionEnum.THE_SHATTERS,
    pcStatsDescriptionEnum.THE_NEST, pcStatsDescriptionEnum.FUNGAL_CAVERN, pcStatsDescriptionEnum.CRYSTAL_CAVERN, pcStatsDescriptionEnum.LOST_HALLS, pcStatsDescriptionEnum.KOGBOLD_STEAMWORKS];

    const enemyOfTheCourt = [pcStatsDescriptionEnum.LAIR_OF_SHAITAN, pcStatsDescriptionEnum.PUPPET_MASTERS_ENCORE, pcStatsDescriptionEnum.CNIDARIAN_REEF, pcStatsDescriptionEnum.SECLUDED_THICKET,
    pcStatsDescriptionEnum.HIGH_TECH_TERROR];

    const epicBattles = [pcStatsDescriptionEnum.DEADWATER_DOCKS, pcStatsDescriptionEnum.WOODLAND_LABYRINTHS, pcStatsDescriptionEnum.THE_CRAWLING_DEPTHS, pcStatsDescriptionEnum.THE_NEST, pcStatsDescriptionEnum.SECLUDED_THICKET];

    const farOut = [pcStatsDescriptionEnum.MALOGIA, pcStatsDescriptionEnum.UNTARIS, pcStatsDescriptionEnum.FORAX, pcStatsDescriptionEnum.KATALUND];

    const heroOfTheNexus = [pcStatsDescriptionEnum.PIRATE_CAVES, pcStatsDescriptionEnum.FOREST_MAZES, pcStatsDescriptionEnum.SPIDER_DENS, pcStatsDescriptionEnum.SNAKE_PITS,
    pcStatsDescriptionEnum.FORBIDDEN_JUNGLES, pcStatsDescriptionEnum.THE_HIVE, pcStatsDescriptionEnum.ANCIENT_RUINS, pcStatsDescriptionEnum.MAGIC_WOODS,
    pcStatsDescriptionEnum.SPRITE_WORLDS, pcStatsDescriptionEnum.CANDYLAND_HUNTING_GROUNDS, pcStatsDescriptionEnum.CAVE_OF_A_THOUSAND_TREASURES, pcStatsDescriptionEnum.UNDEAD_LAIRS,
    pcStatsDescriptionEnum.ABYSS_OF_DEMONS, pcStatsDescriptionEnum.MANOR_OF_THE_IMMORTALS, pcStatsDescriptionEnum.PUPPET_MASTERS_THEATRE, pcStatsDescriptionEnum.TOXIC_SEWERS,
    pcStatsDescriptionEnum.CURSED_LIBRARY, pcStatsDescriptionEnum.HAUNTED_CEMETERIES, pcStatsDescriptionEnum.MAD_LABS, pcStatsDescriptionEnum.PARASITE_CHAMBERS, pcStatsDescriptionEnum.DAVY_JONES_LOCKERS,
    pcStatsDescriptionEnum.MOUNTAIN_TEMPLE, pcStatsDescriptionEnum.THE_THIRD_DIMENSION, pcStatsDescriptionEnum.LAIR_OF_DRACONIS, pcStatsDescriptionEnum.DEADWATER_DOCKS, pcStatsDescriptionEnum.WOODLAND_LABYRINTHS,
    pcStatsDescriptionEnum.THE_CRAWLING_DEPTHS, pcStatsDescriptionEnum.OCEAN_TRENCHES, pcStatsDescriptionEnum.ICE_CAVES, pcStatsDescriptionEnum.TOMB_OF_THE_ANCIENTS, pcStatsDescriptionEnum.FUNGAL_CAVERN,
    pcStatsDescriptionEnum.CRYSTAL_CAVERN, pcStatsDescriptionEnum.THE_NEST, pcStatsDescriptionEnum.THE_SHATTERS, pcStatsDescriptionEnum.LOST_HALLS, pcStatsDescriptionEnum.CULTIST_HIDEOUT,
    pcStatsDescriptionEnum.THE_VOID, pcStatsDescriptionEnum.SULFUROUS_WETLANDS, pcStatsDescriptionEnum.KOGBOLD_STEAMWORKS, pcStatsDescriptionEnum.ORYXS_CASTLE, pcStatsDescriptionEnum.LAIR_OF_SHAITAN,
    pcStatsDescriptionEnum.PUPPET_MASTERS_ENCORE, pcStatsDescriptionEnum.CNIDARIAN_REEF, pcStatsDescriptionEnum.SECLUDED_THICKET, pcStatsDescriptionEnum.HIGH_TECH_TERROR, pcStatsDescriptionEnum.ORYXS_CHAMBER, pcStatsDescriptionEnum.WINE_CELLAR,
    pcStatsDescriptionEnum.ORYXS_SANCTUARY, pcStatsDescriptionEnum.SPECTRAL_PENITENTIARY];

    const seasonsBeatins = [pcStatsDescriptionEnum.BELLADONNAS_GARDEN, pcStatsDescriptionEnum.ICE_TOMB, pcStatsDescriptionEnum.MAD_GOD_MAYHEMS, pcStatsDescriptionEnum.BATTLE_FOR_THE_NEXUS,
    pcStatsDescriptionEnum.SANTA_WORKSHOP, pcStatsDescriptionEnum.THE_MACHINE, pcStatsDescriptionEnum.MALOGIA, pcStatsDescriptionEnum.UNTARIS, pcStatsDescriptionEnum.FORAX,
    pcStatsDescriptionEnum.KATALUND, pcStatsDescriptionEnum.RAINBOW_ROAD, pcStatsDescriptionEnum.BEACHZONE];

    const realmOfTheMadGod = [pcStatsDescriptionEnum.PIRATE_CAVES, pcStatsDescriptionEnum.FOREST_MAZES, pcStatsDescriptionEnum.SPIDER_DENS, pcStatsDescriptionEnum.SNAKE_PITS,
    pcStatsDescriptionEnum.FORBIDDEN_JUNGLES, pcStatsDescriptionEnum.THE_HIVE, pcStatsDescriptionEnum.ANCIENT_RUINS, pcStatsDescriptionEnum.MAGIC_WOODS,
    pcStatsDescriptionEnum.SPRITE_WORLDS, pcStatsDescriptionEnum.CANDYLAND_HUNTING_GROUNDS, pcStatsDescriptionEnum.CAVE_OF_A_THOUSAND_TREASURES, pcStatsDescriptionEnum.UNDEAD_LAIRS,
    pcStatsDescriptionEnum.ABYSS_OF_DEMONS, pcStatsDescriptionEnum.MANOR_OF_THE_IMMORTALS, pcStatsDescriptionEnum.PUPPET_MASTERS_THEATRE, pcStatsDescriptionEnum.TOXIC_SEWERS,
    pcStatsDescriptionEnum.CURSED_LIBRARY, pcStatsDescriptionEnum.HAUNTED_CEMETERIES, pcStatsDescriptionEnum.MAD_LABS, pcStatsDescriptionEnum.PARASITE_CHAMBERS, pcStatsDescriptionEnum.DAVY_JONES_LOCKERS,
    pcStatsDescriptionEnum.MOUNTAIN_TEMPLE, pcStatsDescriptionEnum.THE_THIRD_DIMENSION, pcStatsDescriptionEnum.LAIR_OF_DRACONIS, pcStatsDescriptionEnum.DEADWATER_DOCKS, pcStatsDescriptionEnum.WOODLAND_LABYRINTHS,
    pcStatsDescriptionEnum.THE_CRAWLING_DEPTHS, pcStatsDescriptionEnum.OCEAN_TRENCHES, pcStatsDescriptionEnum.ICE_CAVES, pcStatsDescriptionEnum.TOMB_OF_THE_ANCIENTS, pcStatsDescriptionEnum.FUNGAL_CAVERN,
    pcStatsDescriptionEnum.CRYSTAL_CAVERN, pcStatsDescriptionEnum.THE_NEST, pcStatsDescriptionEnum.THE_SHATTERS, pcStatsDescriptionEnum.LOST_HALLS, pcStatsDescriptionEnum.CULTIST_HIDEOUT,
    pcStatsDescriptionEnum.THE_VOID, pcStatsDescriptionEnum.SULFUROUS_WETLANDS, pcStatsDescriptionEnum.KOGBOLD_STEAMWORKS, pcStatsDescriptionEnum.ORYXS_CASTLE, pcStatsDescriptionEnum.LAIR_OF_SHAITAN,
    pcStatsDescriptionEnum.PUPPET_MASTERS_ENCORE, pcStatsDescriptionEnum.CNIDARIAN_REEF, pcStatsDescriptionEnum.SECLUDED_THICKET, pcStatsDescriptionEnum.HIGH_TECH_TERROR, pcStatsDescriptionEnum.ORYXS_CHAMBER, pcStatsDescriptionEnum.WINE_CELLAR,
    pcStatsDescriptionEnum.ORYXS_SANCTUARY, pcStatsDescriptionEnum.BELLADONNAS_GARDEN, pcStatsDescriptionEnum.ICE_TOMB, pcStatsDescriptionEnum.MAD_GOD_MAYHEMS, pcStatsDescriptionEnum.BATTLE_FOR_THE_NEXUS,
    pcStatsDescriptionEnum.SANTA_WORKSHOP, pcStatsDescriptionEnum.THE_MACHINE, pcStatsDescriptionEnum.MALOGIA, pcStatsDescriptionEnum.UNTARIS, pcStatsDescriptionEnum.FORAX,
    pcStatsDescriptionEnum.KATALUND, pcStatsDescriptionEnum.RAINBOW_ROAD, pcStatsDescriptionEnum.BEACHZONE, pcStatsDescriptionEnum.SPECTRAL_PENITENTIARY];

    const dungeonBonuses = {
        "Tunnel Rat": tunnelRat, "Explosive Journey": explosiveJourney, "Travel of the Decade": travelOfTheDecade, "First Steps": firstSteps,
        "King of the Mountains": kingOfTheMountains, "Conquerer of the Realm": conquererOfTheRealm, "Enemy of the Court": enemyOfTheCourt,
        "Epic Battles": epicBattles, "Far Out": farOut, "Hero of the Nexus": heroOfTheNexus, "Season's Beatins": seasonsBeatins, "Realm of the Mad God": realmOfTheMadGod
    };

    const MAX_DUNGEONS_PER_ROW = 10;

    const determineNumRows = (arr) => {
        const length = arr.length / MAX_DUNGEONS_PER_ROW;
        const result = [];
        for (let i = 0; i < length; i++) {
            result.push(i);
        }
        return result;
    }

    return (
        <Popover
            id={character.char_id}
            open={open}
            anchorEl={anchorEl}
            onClose={handleClose}
            anchorOrigin={{
                vertical: "bottom",
                horizontal: "left",
            }}
        >
            <Box sx={{ display: 'flex', height: 'fit-content', width: 'fit-content' }}>
                <TableContainer>
                    <Table>
                        <TableHead>
                            <TableRow>
                                <PaddedTableCell sx={{ textAlign: 'start', borderBottom: 'none', py: 0 }} colSpan={3}>
                                    <Typography variant="h5" fontWeight={600}>
                                        Dungeon Bonuses
                                    </Typography>
                                </PaddedTableCell>
                                <PaddedTableCell sx={{ textAlign: 'center', borderBottom: 'none', py: 0 }}>
                                </PaddedTableCell>
                            </TableRow>
                        </TableHead>
                        <TableBody>
                            {
                                Object.keys(dungeonBonuses).map((dungeonBonusName) => {
                                    const rand = crypto.randomUUID();
                                    return (
                                        <Fragment key={`Main-${rand}`}>
                                            <TableRow key={`${dungeonBonusName}-Header-${character.char_id}-${rand}`}>
                                                <TableCell colSpan={dungeonBonuses[dungeonBonusName].length}>
                                                    <Typography color={dungeonBonuses[dungeonBonusName].every((dungeonName) => { return character.processed_pc_stats?.get(dungeonName) >= 1 }) ? theme.palette.warning.main : theme.palette.text.primary} variant="h6" fontWeight={300}>
                                                        {dungeonBonusName}
                                                    </Typography>
                                                </TableCell>
                                            </TableRow>
                                            {
                                                determineNumRows(dungeonBonuses[dungeonBonusName]).map((rowNumber) => {
                                                    const rand2 = crypto.randomUUID();
                                                    return (
                                                        <>
                                                            <TableRow key={`${dungeonBonusName}-Images-${rowNumber}-${character.char_id}-${rand}-${rand2}`}>
                                                                {
                                                                    dungeonBonuses[dungeonBonusName].slice(rowNumber * MAX_DUNGEONS_PER_ROW, MAX_DUNGEONS_PER_ROW * (rowNumber + 1)).map((dungeonName) => {
                                                                        const rand3 = crypto.randomUUID();
                                                                        return (
                                                                            <TableCell key={`${rand}-${rand2}-${rand3}`}>
                                                                                <Tooltip title={`${dungeonName}`}>
                                                                                    <img src={`realm/dungeons/${dungeonName}.png`} alt={{ dungeonName }} style={{ padding: '0', maxWidth: 48, maxHeight: 48 }} />
                                                                                </Tooltip>
                                                                            </TableCell>
                                                                        );
                                                                    })
                                                                }
                                                            </TableRow>
                                                            <TableRow key={`${dungeonBonusName}-${rowNumber}-${character.char_id}-${rand}-${rand2}`}>
                                                                {
                                                                    dungeonBonuses[dungeonBonusName].slice(rowNumber * MAX_DUNGEONS_PER_ROW, MAX_DUNGEONS_PER_ROW * (rowNumber + 1)).map((dungeonName) => {
                                                                        const rand4 = crypto.randomUUID();
                                                                        return (
                                                                            <TableCell key={`${rand}-${rand2}-${rand4}`}>{dungeonName === pcStatsDescriptionEnum.SPECTRAL_PENITENTIARY ? <Tooltip title="Unknown"><QuestionMarkIcon></QuestionMarkIcon></Tooltip> : character.processed_pc_stats?.get(dungeonName) >= 1 ? <Tooltip title="Completed"><CheckIcon></CheckIcon></Tooltip> : <Tooltip title="Not completed"><ClearIcon></ClearIcon></Tooltip>}</TableCell>
                                                                        );
                                                                    })
                                                                }
                                                            </TableRow>
                                                        </>
                                                    )
                                                })
                                            }
                                        </Fragment>
                                    );
                                })
                            }
                        </TableBody>
                    </Table>
                </TableContainer>
            </Box>
        </Popover>
    );
}

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
                                stats.map((stat, index) => (
                                    <TableRow key={index + '-' + stat.leftId}>
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
