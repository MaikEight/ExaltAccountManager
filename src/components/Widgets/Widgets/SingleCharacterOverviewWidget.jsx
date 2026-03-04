import { useCallback, useEffect, useMemo, useRef, useState } from "react";
import useWidgets from "../../../hooks/useWidgets";
import { invoke } from '@tauri-apps/api/core';
import WidgetBase from "./WidgetBase";
import { getXof8OfCharacter, isCrucibleActive, isStatMaxed } from "../../../utils/realmCharacterUtils";
import { Box, Chip, Tooltip, Typography, Select, MenuItem, FormControl, InputLabel, Tabs, Tab } from "@mui/material";
import CharacterPortrait from "../../Realm/CharacterPortrait";
import { classes } from "../../../assets/constants";
import { useColorList } from "../../../hooks/useColorList";
import ItemGridV2 from "../../VaultPeeker/V2/ItemGridV2";
import { SparkLineChart } from '@mui/x-charts/SparkLineChart';
import { useTheme } from "@emotion/react";
import { areaElementClasses, chartsAxisHighlightClasses, lineElementClasses } from "@mui/x-charts";
import { formatTime } from 'eam-commons-js';
import StyledButton from "../../StyledButton";
import playerStats, { getDungeonImage } from "../../../assets/playerStats";
import useDebugLogs from "../../../hooks/useDebugLogs";
import SlowAnimatedImage from "../../SlowAnimatedImage";

function SingleCharacterOverviewWidget({ type, widgetId }) {
    const { widgetBarState, widgetBarConfig, getWidgetConfiguration, updateWidgetConfiguration } = useWidgets();
    const { log } = useDebugLogs();

    const config = getWidgetConfiguration(type);
    const currentSlots = Math.min((widgetBarConfig?.slots || 1), (config?.slots || type?.defaultConfig?.slots || 1));
    const daysToFetch = config?.settings?.daysToFetch || 7;

    const theme = useTheme();
    const seasonalChipColor = useColorList(1);
    const crucibleChipColor = useColorList(3);

    const [characterIdToShow, setCharacterIdToShow] = useState(null);
    const [character, setCharacter] = useState(null);
    const [charPcStats, setCharPcStats] = useState([]);
    const [latestCharListDataset, setLatestCharListDataset] = useState(null);
    const [lastDaysCharListDataset, setLastDaysCharListDataset] = useState(null);
    const lastEmailRef = useRef(null);
    const leftContentRef = useRef(null);
    const [leftContentHeight, setLeftContentHeight] = useState(0);

    useEffect(() => {
        if (!leftContentRef.current) return;
        const observer = new ResizeObserver(entries => {
            setLeftContentHeight(entries[0].contentRect.height);
        });
        observer.observe(leftContentRef.current);
        return () => observer.disconnect();
    }, [leftContentRef.current]);

    // Settings editor state
    const [isSettingsEditMode, setIsSettingsEditMode] = useState(false);
    const [statsTab, setStatsTab] = useState(0);
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
        return data;
    }, [lastDaysCharListDataset, characterIdToShow]);

    // Helper to map ParsedItem[] to the format ItemGridV2 expects
    const mapItems = useCallback((itemArray) => {
        if (!itemArray) return [];
        return itemArray.map((item) => ({
            itemId: item.item_id,
            count: 1,
            maxRarity: item.enchant_ids?.length ? Math.min(4, item.enchant_ids.length) : 0,
            parsedItem: item,
        }));
    }, []);

    // Build grouped item arrays from dataset.items for the current character
    const { equipmentItems, inventoryItems, backpackItems, backpackExtenderItems } = useMemo(() => {
        if (!latestCharListDataset?.items || !characterIdToShow) {
            return { equipmentItems: [], inventoryItems: [], backpackItems: [], backpackExtenderItems: [] };
        }

        const charItems = latestCharListDataset.items
            .filter((item) => item.storage_type_id === `char:${characterIdToShow}`)
            .sort((a, b) => {
                const containerDiff = (a.container_index || 0) - (b.container_index || 0);
                if (containerDiff !== 0) return containerDiff;
                return (a.slot_index || 0) - (b.slot_index || 0);
            });

        const equipment = charItems.filter((i) => i.container_index === 0);
        const inventory = charItems.filter((i) => i.container_index === 1);
        const backpacks = charItems.filter((i) => i.container_index >= 2);

        return {
            equipmentItems: mapItems(equipment),
            inventoryItems: mapItems(inventory),
            backpackItems: mapItems(backpacks.slice(0, 8)),
            backpackExtenderItems: mapItems(backpacks.slice(8, 16)),
        };
    }, [latestCharListDataset?.items, characterIdToShow, mapItems]);

    const xof8 = useMemo(() => character ? getXof8OfCharacter(character) : 0, [character]);

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

        const stats = latestCharListDataset.pc_stats?.filter(stat => stat.char_id === id) || [];
        setCharPcStats(stats);
    }, [latestCharListDataset]);

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
            return (
                <Box
                    sx={{
                        display: 'flex',
                        flexDirection: 'column',
                        alignItems: 'center',
                        justifyContent: 'center',
                        width: '100%',
                        gap: 1,
                    }}
                >
                    <img
                        src="/mascot/Search/no_accounts_1_small_very_low_res.png"
                        alt="No character"
                        width="80"
                        height="56"
                    />
                    No character found
                </Box>
            );
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
                        alignItems: 'center',
                        gap: 2,
                    }}
                >
                    {equipmentItems.length > 0 && (
                        <Box
                            sx={{
                                backgroundColor: theme => theme.palette.background.default,
                                borderRadius: 1,
                                p: 0.25,
                            }}
                        >
                            <ItemGridV2
                                items={equipmentItems}
                                showCounts={false}
                                showEmptySlots={true}
                                showTooltips={true}
                                columns={4}
                            />
                        </Box>
                    )}
                    <Box>
                        <Typography variant="body2" fontWeight="bold" color={xof8 === 8 ? 'warning' : 'inherit'}>
                            {xof8} / 8
                        </Typography>
                        <Typography variant="caption" fontSize={16}>
                            {character.current_fame} <img src="/realm/fame.png" alt="Fame" width={16} height={16} style={{ marginBottom: -2 }} />
                        </Typography>
                    </Box>
                </Box>

                {/* Inventory */}
                {inventoryItems.length > 0 && (
                    <Box sx={{ display: 'flex', flexDirection: 'column' }}>
                        <Typography variant="caption" color="text.secondary" sx={{ mb: 0.25 }}>
                            Inventory
                        </Typography>
                        <ItemGridV2
                            items={inventoryItems}
                            showCounts={false}
                            showEmptySlots={true}
                            showTooltips={true}
                            columns={8}
                        />
                    </Box>
                )}

                {/* Backpack */}
                {backpackItems.length > 0 && (
                    <Box sx={{ display: 'flex', flexDirection: 'column' }}>
                        <Typography variant="caption" color="text.secondary" sx={{ mb: 0.25 }}>
                            Backpack
                        </Typography>
                        <ItemGridV2
                            items={backpackItems}
                            showCounts={false}
                            showEmptySlots={true}
                            showTooltips={true}
                            columns={8}
                        />
                    </Box>
                )}

                {/* Backpack Extender */}
                {backpackExtenderItems.length > 0 && (
                    <Box sx={{ display: 'flex', flexDirection: 'column' }}>
                        <Typography variant="caption" color="text.secondary" sx={{ mb: 0.25 }}>
                            Backpack Extender
                        </Typography>
                        <ItemGridV2
                            items={backpackExtenderItems}
                            showCounts={false}
                            showEmptySlots={true}
                            showTooltips={true}
                            columns={8}
                        />
                    </Box>
                )}
            </Box>
        );
    };

    const getChartContent = () => {
        return (
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
        )
    };

    const getStatsContent = () => {
        // Build full list from playerStats definition, merging in actual values
        const statLookup = Object.fromEntries(charPcStats.map(s => [s.stat_type, s.stat_value]));

        const allStats = Object.entries(playerStats)
            .map(([key, def]) => ({
                stat_type: Number(key),
                stat_value: statLookup[key] ?? null,
                isDungeon: def.isDungeon,
            }))
            .sort((a, b) => a.stat_type - b.stat_type);

        const nonDungeonStats = allStats.filter(s => !s.isDungeon);
        const dungeonStats = allStats.filter(s => s.isDungeon);
        const activeList = statsTab === 0 ? nonDungeonStats : dungeonStats;

        return (
            <Box
                sx={{
                    display: 'flex',
                    flexDirection: 'column',
                    flex: 1,
                    minWidth: 0,
                    maxHeight: leftContentHeight || undefined,
                    overflow: 'hidden',
                    ml: 1,
                }}
            >
                <Typography variant="h6">
                    Character Stats
                </Typography>
                <Tabs
                    value={statsTab}
                    onChange={(_, v) => setStatsTab(v)}
                    sx={{ minHeight: 32, mb: 0.5 }}
                    slotProps={{
                        indicator: {
                            style: { height: 2 },
                        },
                    }}
                >
                    <Tab label="Stats" sx={{ minHeight: 32, py: 0.5, fontSize: '0.75rem' }} />
                    <Tab label="Dungeons" sx={{ minHeight: 32, py: 0.5, fontSize: '0.75rem' }} />
                </Tabs>
                <Box
                    sx={{
                        display: 'flex',
                        flexDirection: 'column',
                        gap: 0.5,
                        flex: 1,
                        minHeight: 0,
                        overflowY: 'auto',
                        pr: 0.5,
                    }}
                >
                    {activeList.map(stat => {
                        const statDef = playerStats[stat.stat_type];
                        const imgSrc = statDef ? getDungeonImage(statDef) : null;
                        const isMissing = stat.stat_value === null;
                        return (
                            <Box
                                key={`stat-${stat.stat_type}`}
                                sx={{
                                    display: 'flex',
                                    justifyContent: 'space-between',
                                    alignItems: 'center',
                                    width: '100%',
                                    gap: 1,
                                    opacity: isMissing ? 0.4 : 1,
                                }}
                            >
                                <Box sx={{ display: 'flex', alignItems: 'center', gap: 0.75, minWidth: 0 }}>
                                    {imgSrc && (
                                        <SlowAnimatedImage
                                            src={imgSrc}
                                            alt={statDef.name}
                                            fps={12}
                                            style={{ height: 16, width: 'auto', flexShrink: 0 }}
                                        />
                                    )}
                                    <Typography variant="body2" fontWeight={400} noWrap>
                                        {statDef?.name || 'Unknown'}
                                    </Typography>
                                </Box>
                                <Typography variant="body2" fontWeight={500} sx={{ flexShrink: 0, pr: 1 }}>
                                    {stat.stat_value ?? 0}
                                </Typography>
                            </Box>
                        );
                    })}
                </Box>
            </Box>
        );
    };

    return (
        <WidgetBase type={type} widgetId={widgetId} onWidgetEditModeChanged={!characterIdToShow || !character ? null : handleWidgetEditModeChanged} isEditMode={isSettingsEditMode}>
            {isSettingsEditMode ? (
                renderSettingsEditor()
            ) : (
                <Box
                    sx={{
                        width: '100%',
                        display: 'flex',
                        flexDirection: 'row',
                        alignItems: 'flex-start',
                        gap: 1,
                    }}
                >
                    <Box ref={leftContentRef} sx={{ display: 'flex', flexShrink: 0 }}>
                    {getContent()}
                    </Box>
                    {
                        currentSlots > 1 &&
                        !(!characterIdToShow || !character) &&
                        getStatsContent()
                    }
                </Box>
            )}
        </WidgetBase>
    );
}

export default SingleCharacterOverviewWidget;