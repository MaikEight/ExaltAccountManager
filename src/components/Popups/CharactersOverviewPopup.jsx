import { useEffect, useMemo, useState } from "react";
import { invoke } from "@tauri-apps/api/core";
import {
    Box,
    Chip,
    Divider,
    Skeleton,
    Tab,
    Tabs,
    Tooltip,
    Typography,
} from "@mui/material";
import { formatTime } from "eam-commons-js";
import CloseRoundedIcon from "@mui/icons-material/CloseRounded";
import { classes } from "../../assets/constants";
import {
    getXof8OfCharacter,
    isCrucibleActive,
    isStatMaxed,
} from "../../utils/realmCharacterUtils";
import { useColorList } from "../../hooks/useColorList";
import playerStats, { getDungeonImage } from "../../assets/playerStats";
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
                    `1px solid ${
                        isSelected
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
                    {activeList.map((stat) => {
                        const statDef = playerStats[stat.stat_type];
                        const imgSrc = statDef
                            ? getDungeonImage(statDef)
                            : null;
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
