import { useCallback, useEffect, useMemo, useState } from "react";
import { invoke } from "@tauri-apps/api/core";
import {
    Box,
    Chip,
    Collapse,
    Divider,
    IconButton,
    Skeleton,
    Tab,
    Tabs,
    Tooltip,
    Typography,
} from "@mui/material";
import { formatTime } from "eam-commons-js";
import CloseRoundedIcon from "@mui/icons-material/CloseRounded";
import FileDownloadRoundedIcon from "@mui/icons-material/FileDownloadRounded";
import { classes } from "../../assets/constants";
import {
    getXof8OfCharacter,
    isCrucibleActive,
    isStatMaxed,
} from "../../utils/realmCharacterUtils";
import { useColorList } from "../../hooks/useColorList";
import playerStats, { getDungeonImage } from "../../assets/playerStats";
import { calculateFameBonuses } from "../../utils/fameBonusUtils";
import CharacterPortrait from "../Realm/CharacterPortrait";
import ItemGridV2 from "../VaultPeeker/V2/ItemGridV2";
import SlowAnimatedImage from "../SlowAnimatedImage";
import EamIconButton from "../EamIconButton";
import PopupBase from "./PopupBase";
import usePopups from "../../hooks/usePopups";
import useAccounts from "../../hooks/useAccounts";

// ---------------------------------------------------------------------------
// Helpers
// ---------------------------------------------------------------------------

function mapItems(itemArray) {
    if (!itemArray) return [];
    return itemArray.map((item) => ({
        itemId: item.item_id,
        count: 1,
        maxRarity: item.enchant_ids?.length
            ? Math.min(4, item.enchant_ids.length)
            : 0,
        parsedItem: item,
    }));
}

function buildItemGroups(allItems, charId) {
    if (!allItems) {
        return {
            equipmentItems: [],
            inventoryItems: [],
            backpackItems: [],
            backpackExtenderItems: [],
        };
    }
    const charItems = allItems
        .filter((i) => i.storage_type_id === `char:${charId}`)
        .sort((a, b) => {
            const d = (a.container_index || 0) - (b.container_index || 0);
            return d !== 0 ? d : (a.slot_index || 0) - (b.slot_index || 0);
        });
    const backpacks = charItems.filter((i) => i.container_index >= 2);
    return {
        equipmentItems: mapItems(charItems.filter((i) => i.container_index === 0)),
        inventoryItems: mapItems(charItems.filter((i) => i.container_index === 1)),
        backpackItems: mapItems(backpacks.slice(0, 8)),
        backpackExtenderItems: mapItems(backpacks.slice(8, 16)),
    };
}

// ---------------------------------------------------------------------------
// CharacterCard – compact card shown in the scrollable grid
// ---------------------------------------------------------------------------

function CharacterCard({
    character,
    allItems,
    isSelected,
    onClick,
    seasonalChipColor,
    crucibleChipColor,
}) {
    const xof8 = useMemo(() => getXof8OfCharacter(character), [character]);
    const isActiveCrucible = useMemo(
        () => isCrucibleActive(character),
        [character]
    );
    const className = classes[character.char_class]?.[0] || "Unknown";

    const equipmentItems = useMemo(
        () => buildItemGroups(allItems, character.char_id).equipmentItems,
        [allItems, character.char_id]
    );

    return (
        <Box
            onClick={onClick}
            sx={{
                display: "flex",
                flexDirection: "column",
                gap: 0.5,
                p: 1,
                cursor: "pointer",
                border: (theme) =>
                    `1px solid ${isSelected
                        ? theme.palette.primary.main
                        : theme.palette.divider
                    }`,
                borderRadius: 1,
                backgroundColor: (theme) =>
                    isSelected
                        ? `${theme.palette.primary.main}22`
                        : theme.palette.background.paper,
                transition: "border-color 0.15s, background-color 0.15s",
                "&:hover": {
                    borderColor: (theme) => theme.palette.primary.main,
                },
                width: "fit-content",
            }}
        >
            {/* Row 1: class name · x/8 · fame */}
            <Box
                sx={{
                    display: "flex",
                    flexDirection: "row",
                    alignItems: "center",
                    gap: 0.75,
                }}
            >
                <Typography variant="caption" fontWeight={600} noWrap>
                    {className}
                </Typography>
                <Typography
                    variant="caption"
                    color={xof8 === 8 ? "warning.main" : "text.secondary"}
                    noWrap
                >
                    {xof8}/8
                </Typography>
                <Box
                    sx={{ display: "flex", alignItems: "center", gap: 0.25 }}
                >
                    <Typography variant="caption" noWrap>
                        {character.current_fame}
                    </Typography>
                    <img
                        src="/realm/fame.png"
                        alt="Fame"
                        width={12}
                        height={12}
                        style={{ marginBottom: -1 }}
                    />
                </Box>
                {character.seasonal && (
                    <Chip
                        label="S"
                        size="small"
                        sx={{
                            ...seasonalChipColor,
                            height: 16,
                            fontSize: "0.6rem",
                        }}
                    />
                )}
                {character.crucible_active && isActiveCrucible && (
                    <Chip
                        label="C"
                        size="small"
                        sx={{
                            ...crucibleChipColor,
                            height: 16,
                            fontSize: "0.6rem",
                        }}
                    />
                )}
            </Box>

            {/* Row 2: portrait + 4 equipment slots */}
            <Box
                sx={{
                    display: "flex",
                    flexDirection: "row",
                    alignItems: "center",
                    gap: 0.75,
                }}
            >
                <CharacterPortrait
                    type={character.char_class}
                    skin={character?.texture}
                    tex1={character?.tex1}
                    tex2={character?.tex2}
                    adjust={false}
                />
                <Box
                    sx={{
                        backgroundColor: (theme) =>
                            theme.palette.background.default,
                        borderRadius: 1,
                        p: 0.25,
                    }}
                >
                    <ItemGridV2
                        items={equipmentItems}
                        showCounts={false}
                        showEmptySlots={true}
                        showTooltips={false}
                        columns={4}
                        itemPadding={0}
                    />
                </Box>
            </Box>
        </Box>
    );
}

// ---------------------------------------------------------------------------
// CharacterDetail – full detail panel shown below the grid
// ---------------------------------------------------------------------------

function CharacterDetail({
    character,
    latestCharListDataset,
}) {
    const seasonalChipColor = useColorList(1);
    const crucibleChipColor = useColorList(3);
    const [statsTab, setStatsTab] = useState(0);
    const [expandedCollectionEntries, setExpandedCollectionEntries] = useState(new Set());

    const toggleCollectionEntry = useCallback((id) => {
        setExpandedCollectionEntries(prev => {
            const next = new Set(prev);
            if (next.has(id)) next.delete(id);
            else next.add(id);
            return next;
        });
    }, []);

    const isActiveCrucible = useMemo(
        () => isCrucibleActive(character),
        [character]
    );
    const xof8 = useMemo(() => getXof8OfCharacter(character), [character]);
    const className = classes[character.char_class]?.[0] || "Unknown";

    const charPcStats = useMemo(
        () =>
            latestCharListDataset?.pc_stats?.filter(
                (s) => s.char_id === character.char_id
            ) || [],
        [latestCharListDataset, character.char_id]
    );

    const { equipmentItems, inventoryItems, backpackItems, backpackExtenderItems } =
        useMemo(
            () => buildItemGroups(latestCharListDataset?.items, character.char_id),
            [latestCharListDataset?.items, character.char_id]
        );

    const creationDate = useMemo(() => {
        if (!character?.creation_date) return "N/A";
        return formatTime(new Date(character.creation_date), true);
    }, [character]);

    const { nonDungeonStats, dungeonStats } = useMemo(() => {
        const statLookup = Object.fromEntries(
            charPcStats.map((s) => [s.stat_type, s.stat_value])
        );
        const allStats = Object.entries(playerStats)
            .map(([key, def]) => ({
                stat_type: Number(key),
                stat_value: statLookup[key] ?? null,
                isDungeon: def.isDungeon,
            }))
            .sort((a, b) => a.stat_type - b.stat_type);
        return {
            nonDungeonStats: allStats.filter((s) => !s.isDungeon),
            dungeonStats: allStats.filter((s) => s.isDungeon),
        };
    }, [charPcStats]);

    const activeList = statsTab === 0 ? nonDungeonStats : dungeonStats;

    const shortToStatDef = useMemo(() => {
        const map = {};
        for (const def of Object.values(playerStats)) {
            map[def.short] = def;
        }
        return map;
    }, []);

    const fameBonusData = useMemo(
        () => calculateFameBonuses(charPcStats, character),
        [charPcStats, character]
    );

    const totalFameBonus = useMemo(() => {
        let totalAbsolute = 0;
        let totalRelativePercent = 0;
        for (const groupCategories of Object.values(fameBonusData)) {
            for (const result of Object.values(groupCategories)) {
                totalAbsolute += result.absoluteBonus;
                totalRelativePercent += result.relativeBonus;
            }
        }
        const relativeFame = Math.ceil(character.current_fame * totalRelativePercent / 100);
        return { absolute: totalAbsolute, relativePercent: totalRelativePercent, relativeFame, total: totalAbsolute + relativeFame };
    }, [fameBonusData, character]);

    // Combines absolute + relative fame for a single entry's display
    const calcBonus = useCallback(
        (absolute, relativePercent) => absolute + Math.ceil(character.current_fame * relativePercent / 100),
        [character.current_fame]
    );

    const exportDungeonExtended = useCallback(async () => {
        const dungeonGroup = fameBonusData["Dungeon Bonuses"];
        if (!dungeonGroup) return;
        const collectionResult = dungeonGroup["Dungeon Collection"];
        if (!collectionResult) return;

        const lines = ["# Dungeon Collection", ""];
        for (const entry of collectionResult.entryResults) {
            const missingConditions = entry.conditionDetails.filter(c => c.missing > 0);
            if (missingConditions.length === 0) continue;
            lines.push(`### ${entry.displayName}`);
            for (const cond of missingConditions) {
                const statDef = cond.type === "StatValue" ? shortToStatDef[cond.stat] : null;
                const dungeonName = statDef?.name ?? cond.stat;
                lines.push(`- [ ] ${dungeonName}`);
            }
            lines.push("");
        }

        if (lines.length <= 2) return;
        const markdown = lines.join("\n");
        try {
            await invoke("open_text_in_editor", { text: markdown });
        } catch (e) {
            console.error("Failed to export dungeon checklist:", e);
        }
    }, [fameBonusData, shortToStatDef]);

    const exportDungeonSimple = useCallback(async () => {
        const dungeonGroup = fameBonusData["Dungeon Bonuses"];
        if (!dungeonGroup) return;
        const collectionResult = dungeonGroup["Dungeon Collection"];
        if (!collectionResult) return;

        const seen = new Set();
        const dungeons = [];
        for (const entry of collectionResult.entryResults) {
            for (const cond of entry.conditionDetails) {
                if (cond.missing <= 0) continue;
                const statDef = cond.type === "StatValue" ? shortToStatDef[cond.stat] : null;
                const name = statDef?.name ?? cond.stat;
                if (!seen.has(name)) {
                    seen.add(name);
                    dungeons.push(name);
                }
            }
        }

        if (dungeons.length === 0) return;
        const lines = ["# Dungeons needed", "", ...dungeons.map(d => `- [ ] ${d}`)];
        const markdown = lines.join("\n");
        try {
            await invoke("open_text_in_editor", { text: markdown });
        } catch (e) {
            console.error("Failed to export simple dungeon checklist:", e);
        }
    }, [fameBonusData, shortToStatDef]);

    const getStatUI = (statName, statValue, isMax) => (
        <Box
            sx={{
                display: "flex",
                flexDirection: "row",
                alignItems: "center",
                gap: 0.5,
            }}
        >
            <Typography
                fontWeight={400}
                variant="body2"
                color="textSecondary"
            >
                {statName}:
            </Typography>
            <Typography
                fontWeight={400}
                variant="body2"
                color={isMax ? "warning.main" : "textPrimary"}
            >
                {statValue}
            </Typography>
        </Box>
    );

    return (
        <Box
            sx={{
                display: "flex",
                flexDirection: "row",
                alignItems: "flex-start",
                gap: 2,
                width: "100%",
            }}
        >
            {/* ── Left column ─────────────────────────────────────────── */}
            <Box
                sx={{
                    display: "flex",
                    flexDirection: "column",
                    gap: 1.5,
                    flexShrink: 0,
                    p: 0.5,
                }}
            >
                {/* Portrait + character stats grid + chips */}
                <Box
                    sx={{
                        display: "flex",
                        flexDirection: "row",
                        alignItems: "center",
                        gap: 2,
                    }}
                >
                    <CharacterPortrait
                        type={character.char_class}
                        skin={character?.texture}
                        tex1={character?.tex1}
                        tex2={character?.tex2}
                        adjust={false}
                    />
                    <Box
                        sx={{
                            display: "grid",
                            gridTemplateColumns: "repeat(4, auto)",
                            gridTemplateRows: "repeat(2, auto)",
                            gap: 1,
                        }}
                    >
                        {getStatUI(
                            "HP",
                            character.max_hit_points,
                            isStatMaxed(character, "max_hit_points")
                        )}
                        {getStatUI(
                            "MP",
                            character.max_magic_points,
                            isStatMaxed(character, "max_magic_points")
                        )}
                        {getStatUI(
                            "ATK",
                            character.attack,
                            isStatMaxed(character, "attack")
                        )}
                        {getStatUI(
                            "DEF",
                            character.defense,
                            isStatMaxed(character, "defense")
                        )}
                        {getStatUI(
                            "SPD",
                            character.speed,
                            isStatMaxed(character, "speed")
                        )}
                        {getStatUI(
                            "DEX",
                            character.dexterity,
                            isStatMaxed(character, "dexterity")
                        )}
                        {getStatUI(
                            "VIT",
                            character.hp_regen,
                            isStatMaxed(character, "hp_regen")
                        )}
                        {getStatUI(
                            "WIS",
                            character.mp_regen,
                            isStatMaxed(character, "mp_regen")
                        )}
                    </Box>
                    <Box
                        sx={{
                            display: "flex",
                            flexDirection: "column",
                            alignItems: "flex-start",
                            gap: 0.5,
                        }}
                    >
                        {character.seasonal && (
                            <Chip
                                sx={{ ...seasonalChipColor }}
                                clickable={false}
                                label="Seasonal"
                                size="small"
                            />
                        )}
                        {character.crucible_active && isActiveCrucible && (
                            <Tooltip
                                title={`Crucible active until ${new Date(
                                    character.crucible_active * 1000
                                ).toLocaleDateString()}`}
                            >
                                <Chip
                                    sx={{ ...crucibleChipColor }}
                                    clickable={false}
                                    label="Crucible"
                                    size="small"
                                />
                            </Tooltip>
                        )}
                    </Box>
                </Box>

                {/* Class · x/8 · fame */}
                <Box
                    sx={{
                        display: "flex",
                        flexDirection: "row",
                        alignItems: "center",
                        gap: 1,
                    }}
                >
                    <Typography variant="body1" fontWeight={600}>
                        {className}
                    </Typography>
                    <Typography
                        variant="body2"
                        color={xof8 === 8 ? "warning.main" : "text.secondary"}
                    >
                        {xof8}/8
                    </Typography>
                    <Typography variant="body2">
                        {character.current_fame}{" "}
                        <img
                            src="/realm/fame.png"
                            alt="Fame"
                            width={14}
                            height={14}
                            style={{ marginBottom: -2 }}
                        />
                    </Typography>
                </Box>

                {/* Equipment */}
                {equipmentItems.length > 0 && (
                    <Box
                        sx={{
                            backgroundColor: (theme) =>
                                theme.palette.background.default,
                            borderRadius: 1,
                            p: 0.25,
                            width: "fit-content",
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

                {/* Inventory */}
                {inventoryItems.length > 0 && (
                    <Box sx={{ display: "flex", flexDirection: "column" }}>
                        <Typography
                            variant="caption"
                            color="text.secondary"
                            sx={{ mb: 0.25 }}
                        >
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
                    <Box sx={{ display: "flex", flexDirection: "column" }}>
                        <Typography
                            variant="caption"
                            color="text.secondary"
                            sx={{ mb: 0.25 }}
                        >
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
                    <Box sx={{ display: "flex", flexDirection: "column" }}>
                        <Typography
                            variant="caption"
                            color="text.secondary"
                            sx={{ mb: 0.25 }}
                        >
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

                {/* Creation date + ID */}
                <Box
                    sx={{
                        display: "flex",
                        flexDirection: "row",
                        gap: 3,
                        justifyContent: "space-between",
                    }}
                >
                    <Box>
                        <Typography variant="body2" fontWeight={400}>
                            Creation Date
                        </Typography>
                        <Typography variant="body2" fontWeight={500}>
                            {creationDate}
                        </Typography>
                    </Box>
                    <Box>
                        <Typography variant="body2" fontWeight={400}>
                            Character ID
                        </Typography>
                        <Typography variant="body2" fontWeight={500}>
                            #{character.char_id}
                        </Typography>
                    </Box>
                </Box>

            </Box>

            {/* ── Right column: stats / dungeons ───────────────────────── */}
            <Box
                sx={{
                    display: "flex",
                    flexDirection: "column",
                    alignSelf: "flex-start",
                    minWidth: 0,
                    flex: 1,
                    ml: 1,
                }}
            >
                <Typography variant="h6">Character Stats</Typography>
                <Tabs
                    value={statsTab}
                    onChange={(_, v) => setStatsTab(v)}
                    sx={{ minHeight: 32, mb: 0.5 }}
                    slotProps={{ indicator: { style: { height: 2 } } }}
                >
                    <Tab
                        label="Stats"
                        sx={{
                            minHeight: 32,
                            py: 0.5,
                            fontSize: "0.75rem",
                        }}
                    />
                    <Tab
                        label="Dungeons"
                        sx={{
                            minHeight: 32,
                            py: 0.5,
                            fontSize: "0.75rem",
                        }}
                    />
                    <Tab
                        label="Fame Bonuses"
                        sx={{
                            minHeight: 32,
                            py: 0.5,
                            fontSize: "0.75rem",
                        }}
                    />
                </Tabs>
                <Box
                    sx={{
                        display: "flex",
                        flexDirection: "column",
                        gap: 0.5,
                        maxHeight: 360,
                        overflowY: "auto",
                        pr: 0.5,
                    }}
                >
                    {statsTab === 2 ? (
                        <>
                            {Object.entries(fameBonusData).map(([groupName, groupCategories]) => {
                                const hasExportableCollections = groupName === "Dungeon Bonuses" &&
                                    groupCategories["Dungeon Collection"]?.entryResults?.some(
                                        e => !e.achieved && e.conditionDetails.some(c => c.missing > 0)
                                    );
                                return (
                                <Box key={groupName} sx={{ mb: 1 }}>
                                    <Box sx={{ display: "flex", alignItems: "center", justifyContent: "space-between", mb: 0.5 }}>
                                        <Typography
                                            variant="caption"
                                            color="text.secondary"
                                            fontWeight={700}
                                            sx={{ textTransform: "uppercase", letterSpacing: 0.5 }}
                                        >
                                            {groupName}
                                        </Typography>
                                        {hasExportableCollections && (
                                            <Box sx={{ display: "flex", gap: 0.5 }}>
                                                <Tooltip title="Export as simple dungeon list">
                                                    <IconButton size="small" onClick={exportDungeonSimple} sx={{ p: 0.25, borderRadius: 0.75, fontSize: "0.6rem", gap: 0.25 }}>
                                                        <FileDownloadRoundedIcon sx={{ fontSize: 12 }} />
                                                        <Typography variant="caption" sx={{ fontSize: "0.6rem", lineHeight: 1 }}>Simple</Typography>
                                                    </IconButton>
                                                </Tooltip>
                                                <Tooltip title="Export as extended dungeon checklist grouped by category">
                                                    <IconButton size="small" onClick={exportDungeonExtended} sx={{ p: 0.25, borderRadius: 0.75, fontSize: "0.6rem", gap: 0.25 }}>
                                                        <FileDownloadRoundedIcon sx={{ fontSize: 12 }} />
                                                        <Typography variant="caption" sx={{ fontSize: "0.6rem", lineHeight: 1 }}>Extended</Typography>
                                                    </IconButton>
                                                </Tooltip>
                                            </Box>
                                        )}
                                    </Box>
                                    {Object.entries(groupCategories).map(([categoryName, result], catIdx) => {
                                        const isCollection = !result.isTiered && result.entryResults.length > 1;

                                        if (isCollection) {
                                            return (
                                                <Box key={categoryName} sx={{ mb: 0.5 }}>
                                                    {/* Collection header */}
                                                    <Typography
                                                        variant="caption"
                                                        color="text.secondary"
                                                        fontWeight={700}
                                                        sx={{ textTransform: "uppercase", letterSpacing: 0.5, display: "block", mb: 0.25, px: 0.75 }}
                                                    >
                                                        {categoryName}
                                                    </Typography>
                                                    {/* Individual entries, each with their conditions */}
                                                    {result.entryResults.map((entry, entryIdx) => {
                                                        const isExpanded = !entry.achieved || expandedCollectionEntries.has(entry.id);
                                                        return (
                                                            <Box
                                                                key={entry.id}
                                                                sx={{
                                                                    pl: 1.5,
                                                                    pr: 0.75,
                                                                    py: 0.5,
                                                                    mb: 0.25,
                                                                    borderRadius: 0.5,
                                                                    backgroundColor: (theme) =>
                                                                        entryIdx % 2 === 0
                                                                            ? theme.palette.background.paper
                                                                            : theme.palette.background.default,
                                                                }}
                                                            >
                                                                {/* Entry header row */}
                                                                <Box
                                                                    onClick={entry.achieved ? () => toggleCollectionEntry(entry.id) : undefined}
                                                                    sx={{
                                                                        display: "flex",
                                                                        justifyContent: "space-between",
                                                                        alignItems: "center",
                                                                        gap: 1,
                                                                        mb: isExpanded ? 0.25 : 0,
                                                                        cursor: entry.achieved ? "pointer" : "default",
                                                                    }}
                                                                >
                                                                    <Typography variant="body2" fontWeight={500} noWrap sx={{ flex: 1, minWidth: 0 }}>
                                                                        {entry.displayName}
                                                                    </Typography>
                                                                    {entry.achieved ? (
                                                                        <Box sx={{ display: "flex", alignItems: "center", gap: 0.75, flexShrink: 0 }}>
                                                                            <Box sx={{ display: "flex", alignItems: "center", gap: 0.4 }}>
                                                                                <Typography variant="body2" fontWeight={600}>
                                                                                    {calcBonus(entry.absoluteBonus, entry.relativeBonus).toLocaleString()}
                                                                                </Typography>
                                                                                <img src="/realm/fame.png" alt="Fame" width={12} height={12} style={{ marginBottom: -1 }} />
                                                                            </Box>
                                                                            <Typography variant="caption" color="text.secondary" sx={{ lineHeight: 1 }}>
                                                                                {isExpanded ? "▲" : "▼"}
                                                                            </Typography>
                                                                        </Box>
                                                                    ) : (
                                                                        <Typography variant="caption" color="text.disabled" sx={{ flexShrink: 0 }}>
                                                                            {entry.completedCount}/{entry.totalCount}
                                                                        </Typography>
                                                                    )}
                                                                </Box>
                                                                {/* Condition rows */}
                                                                <Collapse in={isExpanded}>
                                                                    <Box sx={{ opacity: entry.achieved ? 1 : 0.65 }}>
                                                                        {entry.conditionDetails.map((ma, i) => {
                                                                            const statDef = ma.type === "StatValue" ? shortToStatDef[ma.stat] : null;
                                                                            const imgSrc = statDef ? getDungeonImage(statDef) : null;
                                                                            const isDone = ma.missing === 0;
                                                                            return (
                                                                                <Box
                                                                                    key={i}
                                                                                    sx={{
                                                                                        display: "flex",
                                                                                        justifyContent: "space-between",
                                                                                        alignItems: "center",
                                                                                        gap: 1,
                                                                                        opacity: isDone ? 0.5 : 1,
                                                                                    }}
                                                                                >
                                                                                    <Box sx={{ display: "flex", alignItems: "center", gap: 0.75, minWidth: 0 }}>
                                                                                        {imgSrc && (
                                                                                            <SlowAnimatedImage
                                                                                                src={imgSrc}
                                                                                                alt={statDef.name}
                                                                                                fps={12}
                                                                                                style={{ height: 14, width: "auto", flexShrink: 0 }}
                                                                                            />
                                                                                        )}
                                                                                        <Typography variant="caption" color="text.secondary" noWrap>
                                                                                            {statDef?.name ?? ma.stat}
                                                                                        </Typography>
                                                                                    </Box>
                                                                                    <Typography
                                                                                        variant="caption"
                                                                                        color={isDone ? "success.main" : "text.secondary"}
                                                                                        sx={{ flexShrink: 0 }}
                                                                                    >
                                                                                        {isDone ? "✓" : "✗"}
                                                                                    </Typography>
                                                                                </Box>
                                                                            );
                                                                        })}
                                                                    </Box>
                                                                </Collapse>
                                                            </Box>
                                                        );
                                                    })}
                                                </Box>
                                            );
                                        }

                                        // Tiered / single-entry rendering
                                        return (
                                            <Box
                                                key={categoryName}
                                                sx={{
                                                    mb: 0,
                                                    px: 0.75,
                                                    py: 0.5,
                                                    borderRadius: 0.5,
                                                    backgroundColor: (theme) =>
                                                        catIdx % 2 === 0
                                                            ? theme.palette.background.paper
                                                            : theme.palette.background.default,
                                                }}
                                            >
                                                {/* Achieved row */}
                                                <Box
                                                    sx={{
                                                        display: "flex",
                                                        justifyContent: "space-between",
                                                        alignItems: "center",
                                                        gap: 1,
                                                    }}
                                                >
                                                    <Typography variant="body2" fontWeight={400} noWrap sx={{ flex: 1, minWidth: 0 }}>
                                                        {result.highestCategoryAchieved
                                                            ? result.highestCategoryAchieved.displayName
                                                            : categoryName}
                                                    </Typography>
                                                    {result.highestCategoryAchieved ? (
                                                        <Box sx={{ display: "flex", alignItems: "center", gap: 0.4, flexShrink: 0 }}>
                                                            <Typography variant="body2" fontWeight={500}>
                                                                {calcBonus(result.absoluteBonus, result.relativeBonus).toLocaleString()}
                                                            </Typography>
                                                            <img src="/realm/fame.png" alt="Fame" width={12} height={12} style={{ marginBottom: -1 }} />
                                                        </Box>
                                                    ) : (
                                                        <Typography
                                                            variant="body2"
                                                            fontWeight={500}
                                                            color="text.disabled"
                                                            sx={{ flexShrink: 0 }}
                                                        >
                                                            —
                                                        </Typography>
                                                    )}
                                                </Box>
                                                {/* Next level conditions */}
                                                {result.nextCategories.length > 0 && (
                                                    <Box sx={{ pl: 1.5, mt: 0.25, opacity: 0.5 }}>
                                                        {result.nextCategories.map((nextCat) => (
                                                            <Box key={nextCat.id} sx={{ mb: 0.25 }}>
                                                                <Typography
                                                                    variant="caption"
                                                                    color="text.secondary"
                                                                    sx={{ display: "block", mb: 0.25 }}
                                                                >
                                                                    {nextCat.displayName}
                                                                </Typography>
                                                                {nextCat.missingAmounts.map((ma, i) => {
                                                                    const statDef = ma.type === "StatValue" ? shortToStatDef[ma.stat] : null;
                                                                    const imgSrc = statDef ? getDungeonImage(statDef) : null;
                                                                    const isDone = ma.missing === 0;
                                                                    return (
                                                                        <Box
                                                                            key={i}
                                                                            sx={{
                                                                                display: "flex",
                                                                                justifyContent: "space-between",
                                                                                alignItems: "center",
                                                                                gap: 1,
                                                                                opacity: isDone ? 0.5 : 1,
                                                                            }}
                                                                        >
                                                                            <Box sx={{ display: "flex", alignItems: "center", gap: 0.75, minWidth: 0 }}>
                                                                                {imgSrc && (
                                                                                    <SlowAnimatedImage
                                                                                        src={imgSrc}
                                                                                        alt={statDef.name}
                                                                                        fps={12}
                                                                                        style={{ height: 14, width: "auto", flexShrink: 0 }}
                                                                                    />
                                                                                )}
                                                                                <Typography variant="caption" color="text.secondary" noWrap>
                                                                                    {ma.type === "StatValue"
                                                                                        ? (statDef?.name ?? ma.stat)
                                                                                        : ma.stat}
                                                                                </Typography>
                                                                            </Box>
                                                                            {ma.type === "StatValue" ? (
                                                                                <Typography
                                                                                    variant="caption"
                                                                                    color={isDone ? "success.main" : "text.secondary"}
                                                                                    sx={{ flexShrink: 0 }}
                                                                                >
                                                                                    {(ma.current ?? 0).toLocaleString()} / {ma.threshold.toLocaleString()}
                                                                                </Typography>
                                                                            ) : (
                                                                                <Typography
                                                                                    variant="caption"
                                                                                    color={isDone ? "success.main" : "text.secondary"}
                                                                                    sx={{ flexShrink: 0 }}
                                                                                >
                                                                                    {isDone ? "✓" : "✗"}
                                                                                </Typography>
                                                                            )}
                                                                        </Box>
                                                                    );
                                                                })}
                                                            </Box>
                                                        ))}
                                                    </Box>
                                                )}
                                            </Box>
                                        );
                                    })}
                                </Box>
                                );
                            })}
                            <Box
                                sx={{
                                    position: "sticky",
                                    bottom: 0,
                                    mt: 0.5,
                                    pt: 1,
                                    borderTop: (theme) => `1px solid ${theme.palette.divider}`,
                                    backgroundColor: "background.paper",
                                    display: "flex",
                                    flexDirection: "column",
                                    gap: 0.25,
                                }}
                            >
                                <Box sx={{ display: "flex", justifyContent: "space-between", alignItems: "center" }}>
                                    <Typography variant="body2" fontWeight={600}>
                                        Total bonus
                                    </Typography>
                                    <Box sx={{ display: "flex", alignItems: "center", gap: 0.4 }}>
                                        <Typography variant="body2" fontWeight={600}>
                                            {totalFameBonus.total.toLocaleString()}
                                        </Typography>
                                        <img src="/realm/fame.png" alt="Fame" width={13} height={13} style={{ marginBottom: -1 }} />
                                    </Box>
                                </Box>
                                <Box sx={{ display: "flex", justifyContent: "space-between", alignItems: "center" }}>
                                    <Typography variant="body2" color="text.secondary">
                                        Predicted fame on death
                                    </Typography>
                                    <Box sx={{ display: "flex", alignItems: "center", gap: 0.4 }}>
                                        <Typography variant="body2" color="text.secondary">
                                            {(character.current_fame + totalFameBonus.total).toLocaleString()}
                                        </Typography>
                                        <img src="/realm/fame.png" alt="Fame" width={13} height={13} style={{ marginBottom: -1 }} />
                                    </Box>
                                </Box>
                            </Box>
                        </>
                    ) : activeList.map((stat) => {
                        const statDef = playerStats[stat.stat_type];
                        const imgSrc = statDef ? getDungeonImage(statDef) : null;
                        const isMissing = stat.stat_value === null;
                        return (
                            <Box
                                key={`stat-${stat.stat_type}`}
                                sx={{
                                    display: "flex",
                                    justifyContent: "space-between",
                                    alignItems: "center",
                                    width: "100%",
                                    gap: 1,
                                    opacity: isMissing ? 0.4 : 1,
                                }}
                            >
                                <Box
                                    sx={{
                                        display: "flex",
                                        alignItems: "center",
                                        gap: 0.75,
                                        minWidth: 0,
                                    }}
                                >
                                    {imgSrc && (
                                        <SlowAnimatedImage
                                            src={imgSrc}
                                            alt={statDef.name}
                                            fps={12}
                                            style={{
                                                height: 16,
                                                width: "auto",
                                                flexShrink: 0,
                                            }}
                                        />
                                    )}
                                    <Typography
                                        variant="body2"
                                        fontWeight={400}
                                        noWrap
                                    >
                                        {statDef?.name || "Unknown"}
                                    </Typography>
                                </Box>
                                <Typography
                                    variant="body2"
                                    fontWeight={500}
                                    sx={{ flexShrink: 0, pr: 1 }}
                                >
                                    {stat.stat_value ?? 0}
                                </Typography>
                            </Box>
                        );
                    })}
                </Box>
            </Box>
        </Box>
    );
}

// ---------------------------------------------------------------------------
// CharactersOverviewPopup – root component
// ---------------------------------------------------------------------------

function CharactersOverviewPopup({ email, initialCharacterId = null }) {
    const { closePopup } = usePopups();
    const { getAccountByEmail } = useAccounts();
    const account = getAccountByEmail(email);
    const seasonalChipColor = useColorList(1);
    const crucibleChipColor = useColorList(3);

    const [latestCharListDataset, setLatestCharListDataset] = useState(null);
    const [isLoading, setIsLoading] = useState(true);
    const [selectedCharId, setSelectedCharId] = useState(null);

    useEffect(() => {
        if (!email) return;
        setIsLoading(true);
        invoke("get_latest_char_list_dataset_for_account", { email })
            .then((latest) => {
                setLatestCharListDataset(latest);
                if (latest?.character?.length > 0) {
                    const hasInitial = initialCharacterId &&
                        latest.character.some((c) => c.char_id === initialCharacterId);
                    if (hasInitial) {
                        setSelectedCharId(initialCharacterId);
                    } else {
                        const sorted = [...latest.character].sort(
                            (a, b) => b.current_fame - a.current_fame
                        );
                        setSelectedCharId(sorted[0].char_id);
                    }
                }
            })
            .catch((err) => {
                console.error("CharactersOverviewPopup fetch error:", err);
            })
            .finally(() => {
                setIsLoading(false);
            });
    }, [email]);

    const characters = latestCharListDataset?.character || [];
    const selectedCharacter =
        characters.find((c) => c.char_id === selectedCharId) || null;

    return (
        <PopupBase sx={{ height: '95vh', overflow: 'auto' }}>
            {/* Header — sticky so close button stays visible while scrolling */}
            <Box
                sx={{
                    position: 'sticky',
                    top: 0,
                    zIndex: 1,
                    backgroundColor: 'background.paper',
                    display: "flex",
                    flexDirection: "row",
                    alignItems: "flex-start",
                    justifyContent: "space-between",
                    gap: 2,
                    pb: 1,
                    mb: -1,
                }}
            >
                <Box>
                    <Typography variant="h6" fontWeight={600}>
                        Characters
                    </Typography>
                    <Typography variant="body2" color="text.secondary">
                        of {account?.name || email}
                    </Typography>
                </Box>
                <EamIconButton
                    icon={<CloseRoundedIcon />}
                    onClick={closePopup}
                    tooltip="Close"
                />
            </Box>

            {/* Character grid */}
            {isLoading ? (
                <Box sx={{ display: "flex", gap: 1, flexWrap: "wrap" }}>
                    {[...Array(4)].map((_, i) => (
                        <Skeleton
                            key={i}
                            variant="rounded"
                            width={185}
                            height={76}
                        />
                    ))}
                </Box>
            ) : characters.length === 0 ? (
                <Typography
                    variant="body2"
                    color="text.secondary"
                    sx={{ textAlign: "center", py: 2 }}
                >
                    No characters found for this account.
                </Typography>
            ) : (
                <Box
                    sx={{
                        display: "grid",
                        // Two rows, then overflow into new columns
                        gridTemplateRows: "repeat(2, auto)",
                        gridAutoFlow: "column",
                        gridAutoColumns: "max-content",
                        gap: 1,
                        overflowX: "auto",
                        pb: 0.5,
                    }}
                >
                    {characters.map((char) => (
                        <CharacterCard
                            key={char.char_id}
                            character={char}
                            allItems={latestCharListDataset?.items}
                            isSelected={char.char_id === selectedCharId}
                            onClick={() => setSelectedCharId(char.char_id)}
                            seasonalChipColor={seasonalChipColor}
                            crucibleChipColor={crucibleChipColor}
                        />
                    ))}
                </Box>
            )}

            {/* Detail section */}
            {selectedCharacter && (
                <>
                    <Divider />
                    <CharacterDetail
                        character={selectedCharacter}
                        latestCharListDataset={latestCharListDataset}
                    />
                </>
            )}
        </PopupBase>
    );
}

export default CharactersOverviewPopup;
