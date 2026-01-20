import { useEffect, useMemo, useRef, useState } from "react";
import useWidgets from "../../../hooks/useWidgets";
import { invoke } from '@tauri-apps/api/core';
import WidgetBase from "./WidgetBase";
import usePortraitReady from "../../../hooks/usePortraitReady";
import { ADCENTUREERS_BELT, BACKPACK_EXTENDER_ITEM_ID, BACKPACK_ITEM_ID, extractEquipmentIds, getXof8OfCharacter, isCrucibleActive, isStatMaxed } from "../../../utils/realmCharacterUtils";
import { getItemById } from "../../../utils/realmItemUtils";
import { Box, Chip, Skeleton, Tooltip, Typography, Select, MenuItem, FormControl, InputLabel } from "@mui/material";
import CharacterPortrait from "../../Realm/CharacterPortrait";
import { classes } from "../../../assets/constants";
import itemsSlotTypeMap from "../../../assets/slotmap";
import { drawItem } from "../../../utils/realmItemDrawUtils";
import { useColorList } from "../../../hooks/useColorList";
import InventoryRender from "./Components/InventoryRender";
import { SparkLineChart } from '@mui/x-charts/SparkLineChart';
import { useTheme } from "@emotion/react";
import { areaElementClasses, chartsAxisHighlightClasses, lineElementClasses } from "@mui/x-charts";
import { formatTime } from 'eam-commons-js';
import StyledButton from "../../StyledButton";

function SingleCharacterOverviewWidget({ type, widgetId }) {
    const { widgetBarState, widgetBarConfig, getWidgetConfiguration, updateWidgetConfiguration } = useWidgets();
    const config = getWidgetConfiguration(type);
    const currentSlots = Math.min((widgetBarConfig?.slots || 1), (config?.slots || type?.defaultConfig?.slots || 1));
    const daysToFetch = config?.settings?.daysToFetch || 7;

    const theme = useTheme();
    const isPortraitReady = usePortraitReady();
    const seasonalChipColor = useColorList(1);
    const crucibleChipColor = useColorList(3);

    const [itemImages, setItemImages] = useState([null, null, null, null]);
    const [items, setItems] = useState([null, null, null, null]);
    const [xof8, setXof8] = useState(0);
    const [backpackItemImages, setBackpackItemImages] = useState([null, null, null]);

    const [characterIdToShow, setCharacterIdToShow] = useState(null);
    const [character, setCharacter] = useState(null);
    const [latestCharListDataset, setLatestCharListDataset] = useState(null);
    const [lastDaysCharListDataset, setLastDaysCharListDataset] = useState(null);
    const lastEmailRef = useRef(null);

    // Settings editor state
    const [isSettingsEditMode, setIsSettingsEditMode] = useState(false);
    const [editDaysToFetch, setEditDaysToFetch] = useState(daysToFetch);
    const [editCharacterId, setEditCharacterId] = useState(null);

    const isActiveCrucible = useMemo(() => {
        if (!character) {
            return false;
        }
        return isCrucibleActive(character);
    }, [character]);

    const daysDataArray = useMemo(() => {
        if (!lastDaysCharListDataset || !characterIdToShow) {
            return [];
        }

        const data = [];
        lastDaysCharListDataset.forEach(dayDataset => {
            const dataset = dayDataset[1];
            if (!dataset) {
                data.push({
                    date: new Date(dayDataset[0]),
                    fame: null,
                });
                return;
            }

            const char = dataset.character.find(c => c.char_id === characterIdToShow);
            if (char) {
                data.push({
                    date: new Date(dayDataset[0]),
                    fame: char.current_fame,
                });
                return;
            }

            data.push({
                date: new Date(dayDataset[0]),
                fame: null,
            });
        });

        // fill all null entries with the last known value
        let lastKnownFame = 0;
        data.forEach((entry, index) => {
            if (entry.fame === null) {
                data[index].fame = lastKnownFame;
            } else {
                lastKnownFame = entry.fame;
            }
        });
        console.log("daysDataArray", data);
        return data;
    }, [lastDaysCharListDataset, characterIdToShow]);

    const drawItemImagesOfCharacter = (character) => {
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
    }

    const getCharacterIdToShow = (dataset) => {
        if (!dataset?.email) {
            return null;
        }
        
        const email = dataset.email;
        const charIdMapping = config?.settings?.characterIdPerAccount || {};
        if (charIdMapping[email]) {
            return charIdMapping[email];
        }

        // If no specific character ID is set for this account, return the latest character ID
        if (dataset && dataset.character && dataset.character.length > 0) {
            const sortedChars = dataset.character.sort((a, b) => b.current_fame - a.current_fame);
            return sortedChars[0].char_id;
        }
    }

    const handleSaveSettings = () => {
        const email = widgetBarState?.data?.email;
        if (!email) {
            setIsSettingsEditMode(false);
            return;
        }

        const charIdMapping = config?.settings?.characterIdPerAccount || {};
        const updatedCharIdMapping = {
            ...charIdMapping,
            [email]: editCharacterId,
        };

        const daysChanged = editDaysToFetch !== daysToFetch;
        const characterChanged = editCharacterId !== characterIdToShow;

        updateWidgetConfiguration(type, {
            settings: {
                ...config?.settings,
                daysToFetch: editDaysToFetch,
                characterIdPerAccount: updatedCharIdMapping,
            },
        });

        setIsSettingsEditMode(false);

        // If days changed, refetch the data
        if (daysChanged) {
            fetchDatasets(true, editDaysToFetch);
        }

        // If character changed, update the displayed character
        if (characterChanged && latestCharListDataset) {
            const char = latestCharListDataset.character.find(c => c.char_id === editCharacterId) || latestCharListDataset.character[0] || null;
            setCharacterIdToShow(editCharacterId);
            setCharacter(char);
        }
    };

    const handleWidgetEditModeChanged = () => {
        if (!isSettingsEditMode) {
            // Entering edit mode - initialize edit state
            setEditDaysToFetch(daysToFetch);
            setEditCharacterId(characterIdToShow);
        }
        setIsSettingsEditMode(prev => !prev);
    };

    const renderSettingsEditor = () => {
        const availableCharacters = latestCharListDataset?.character || [];
        const daysOptions = config?.settings?.daysToFetchOptions || type?.defaultConfig?.settings?.daysToFetchOptions || [7, 14, 30];

        if (availableCharacters.length === 0) {
            return (
                <Box sx={{ p: 2 }}>
                    <Typography variant="body2" color="text.secondary" sx={{ textAlign: 'center', mb: 2 }}>
                        No characters available for this account.
                    </Typography>
                    <StyledButton
                        variant="contained"
                        color="secondary"
                        onClick={() => setIsSettingsEditMode(false)}
                        sx={{ ml: 'auto', display: 'block' }}
                    >
                        Exit Edit Mode
                    </StyledButton>
                </Box>
            );
        }

        return (
            <Box sx={{ p: 2, display: 'flex', flexDirection: 'column', gap: 2 }}>
                <FormControl fullWidth size="small">
                    <InputLabel>Character to Display</InputLabel>
                    <Select
                        value={editCharacterId || ''}
                        label="Character to Display"
                        onChange={(e) => setEditCharacterId(e.target.value)}
                    >
                        {availableCharacters.map((char) => (
                            <MenuItem key={char.char_id} value={char.char_id}>
                                {classes[char.char_class]?.[0] || 'Unknown'} - {char.current_fame} Fame (ID: {char.char_id})
                                {char.seasonal && <Chip label="Seasonal" size="small" sx={{ ml: 1, ...seasonalChipColor }} />}
                            </MenuItem>
                        ))}
                    </Select>
                </FormControl>

                <FormControl fullWidth size="small">
                    <InputLabel>Days to Display in Chart</InputLabel>
                    <Select
                        value={editDaysToFetch}
                        label="Days to Display in Chart"
                        onChange={(e) => setEditDaysToFetch(e.target.value)}
                    >
                        {daysOptions.map((days) => (
                            <MenuItem key={days} value={days}>
                                {days} {days === 1 ? 'Day' : 'Days'}
                            </MenuItem>
                        ))}
                    </Select>
                </FormControl>

                <Box sx={{ display: 'flex', gap: 1, ml: 'auto' }}>
                    <StyledButton
                        variant="contained"
                        color="secondary"
                        onClick={() => setIsSettingsEditMode(false)}
                    >
                        Cancel
                    </StyledButton>
                    <StyledButton
                        variant="contained"
                        color="primary"
                        onClick={handleSaveSettings}
                    >
                        Save Settings
                    </StyledButton>
                </Box>
            </Box>
        );
    };

    const fetchDatasets = async (forceRefetch = false, daysOverride = null) => {
        if (!widgetBarState?.data?.email) {
            setLatestCharListDataset(null);
            setLastDaysCharListDataset(null);
            return;
        }

        if (!forceRefetch && widgetBarState.data.email === lastEmailRef.current) {
            return;
        }

        const email = widgetBarState.data.email;
        lastEmailRef.current = email;
        const days = daysOverride !== null ? daysOverride : daysToFetch;
        try {
            const promises = [
                invoke('get_latest_char_list_dataset_for_account', { email }),
                invoke('get_last_days_char_list_dataset_for_account', { email, lastDays: days })
            ];
            const [latestDataset, lastDaysDataset] = await Promise.all(promises);
            if (lastEmailRef.current === email) {
                setLatestCharListDataset(latestDataset);
                setLastDaysCharListDataset(lastDaysDataset);

                console.log("Latest char list dataset:", latestDataset);
                console.log(`Last ${days} days char list dataset:`, lastDaysDataset);
            }
        } catch (err) {
            console.error(err);
            setLatestCharListDataset(null);
            setLastDaysCharListDataset(null);
        }
    }

    useEffect(() => {
        fetchDatasets();
    }, [widgetBarState.data, daysToFetch]);

    useEffect(() => {
        if (!latestCharListDataset) {
            setCharacter(null);
            return;
        }

        const id = getCharacterIdToShow(latestCharListDataset);
        setCharacterIdToShow(id);
        const char = latestCharListDataset.character.find(c => c.char_id === id) || latestCharListDataset.character[0] || null;
        setCharacter(char);
    }, [latestCharListDataset]);

    useEffect(() => {
        if (character) {
            drawItemImagesOfCharacter(character);
        }
    }, [character]);

    const getCreationDate = () => {
        if (!character) {
            return 'N/A';
        }

        const date = new Date(character.creation_date);
        return formatTime(date, true);
    }

    const getStatUI = (statName, statValue, isMax) => {
        return (
            <Box
                sx={{
                    display: 'flex',
                    flexDirection: 'row',
                    alignItems: 'center',
                    justifyContent: 'flex-start',
                    gap: 0.5,
                }}
            >
                <Typography fontWeight={400} variant="body2" color="textSecondary">
                    {statName}:
                </Typography>
                <Typography fontWeight={400} variant="body2" color={isMax ? "warning.main" : "textPrimary"}>
                    {statValue}
                </Typography>
            </Box>
        );
    }

    const getContent = () => {
        if (!characterIdToShow || !character) {
            return null;
        }

        return (
            <Box
                sx={{
                    display: 'flex',
                    flexDirection: 'column',
                    alignItems: 'flex-start',
                    justifyContent: 'flex-start',
                    p: 0.5,
                    gap: 1.5,
                    width: currentSlots === 1 ? '100%' : 'fit-content',
                }}
            >
                <Box
                    sx={{
                        display: 'flex',
                        flexDirection: 'row',
                        alignItems: 'center',
                        justifyContent: 'flex-start',
                        width: '100%',
                        pl: 0.25,
                        gap: 2,
                    }}
                >
                    <CharacterPortrait type={character.class} skin={character?.texture} tex1={character?.tex1} tex2={character?.tex2} adjust={false} />
                    {/* Character stats */}
                    <Box
                        sx={{
                            // Grid for stats
                            display: 'grid',
                            gridTemplateColumns: 'repeat(4, auto)',
                            gridTemplateRows: 'repeat(2, auto)',
                            gap: 1,
                        }}
                    >
                        {getStatUI("HP", character.max_hit_points, isStatMaxed(character, "max_hit_points"))}
                        {getStatUI("MP", character.max_magic_points, isStatMaxed(character, "max_magic_points"))}
                        {getStatUI("ATK", character.attack, isStatMaxed(character, "attack"))}
                        {getStatUI("DEF", character.defense, isStatMaxed(character, "defense"))}
                        {getStatUI("SPD", character.speed, isStatMaxed(character, "speed"))}
                        {getStatUI("DEX", character.dexterity, isStatMaxed(character, "dexterity"))}
                        {getStatUI("VIT", character.hp_regen, isStatMaxed(character, "hp_regen"))}
                        {getStatUI("WIS", character.mp_regen, isStatMaxed(character, "mp_regen"))}
                    </Box>
                    <Box
                        sx={{
                            display: 'flex',
                            flexDirection: 'column',
                            alignItems: 'flex-start',
                            justifyContent: 'space-between',
                            height: '100%',
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
                            character.crucible_active &&
                            isActiveCrucible &&
                            <Tooltip title={`Cricible active until ${new Date(character.crucible_active * 1000).toLocaleDateString()}`}>
                                <Chip
                                    sx={{
                                        ...crucibleChipColor,
                                    }}

                                    clickable={false}
                                    label={'Crucible'}
                                    size="small"
                                />
                            </Tooltip>
                        }
                    </Box>
                </Box>
                {/* Equipment */}
                <Box
                    sx={{
                        display: 'flex',
                        flexDirection: 'row',
                        justifyContent: 'center',
                        alignItems: 'center',
                        gap: 2,
                    }}
                >
                    <Box
                        sx={{
                            display: 'flex',
                            flexDirection: 'row',
                            justifyContent: 'center',
                            alignItems: 'center',
                            backgroundColor: theme => theme.palette.background.default,
                            borderRadius: 1,
                            gap: 0.25,
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
                {/* Inventory & Backpack */}
                <InventoryRender character={character} />
            </Box>
        );
    }

    return (
        <WidgetBase type={type} widgetId={widgetId} onWidgetEditModeChanged={handleWidgetEditModeChanged} isEditMode={isSettingsEditMode}>
            {isSettingsEditMode ? (
                renderSettingsEditor()
            ) : (
                <Box
                    sx={{
                        width: '100%',
                        display: 'flex',
                        flexDirection: 'row',
                        gap: 1,
                    }}
                >
                    {getContent()}
                    {
                        currentSlots > 1 &&
                        <Box
                            sx={{
                                p: 0.5,
                                display: 'flex',
                                flexDirection: 'column',
                                width: '100%',
                            }}
                        >
                            <Box
                                sx={{
                                    display: 'flex',
                                    flexDirection: 'row',
                                    gap: 1,
                                    justifyContent: 'space-between',
                                    width: '100%',
                                    height: 'fit-content',
                                }}
                            >
                                <Box>
                                    <Typography variant="body2" fontWeight={400}>
                                        Creation Date
                                    </Typography>
                                    <Typography variant="body2" fontWeight={500}>
                                        {getCreationDate()}
                                    </Typography>
                                </Box>
                                <Box>
                                <Typography variant="body2" fontWeight={400}>
                                    Character ID
                                </Typography>
                                <Typography variant="body2" fontWeight={500}>
                                    #{character?.char_id}
                                </Typography>
                                </Box>
                            </Box>
                            <Box
                                sx={{
                                    mt: 'auto',
                                    display: 'flex',
                                    flexDirection: 'column',
                                    width: '100%',
                                    height: 'fit-content',
                                }}
                            >
                                <Typography variant="h6">
                                    Base Fame over the last {daysToFetch} days
                                </Typography>
                                <SparkLineChart
                                    sx={{
                                        [`& .${areaElementClasses.root}`]: { opacity: 0.2 },
                                        [`& .${lineElementClasses.root}`]: { strokeWidth: 3 },
                                        [`& .${chartsAxisHighlightClasses.root}`]: {
                                            stroke: theme.palette.primary.main,
                                            strokeDasharray: 'none',
                                            strokeWidth: 0
                                        },
                                    }}
                                    data={daysDataArray.map(d => d.fame)}
                                    valueFormatter={(value) => value !== null ? `${value} Fame` : 'N/A'}
                                    height={100}
                                    color={theme.palette.primary.main}
                                    area
                                    plotType="line"
                                    showHighlight={true}
                                    showTooltip={true}
                                />
                            </Box>
                        </Box>
                    }
                </Box>
            )}
        </WidgetBase>
    );
}

export default SingleCharacterOverviewWidget;