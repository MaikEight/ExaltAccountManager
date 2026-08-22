import fameBonuses from "./fameBonuses";
import playerStats from "./playerStats";

export const ITEM_SPRITE_HASH_INDEX = 11;
export const MISSING_ITEM_SPRITE_SOURCE = "/realm/missing-item.svg";

const itemRecords = {};
const missingItems = new Map();

function missingItem(itemId) {
    const normalizedId = String(itemId);
    if (!missingItems.has(normalizedId)) {
        const displayId = normalizedId === "0" ? "" : ` #${normalizedId}`;
        missingItems.set(normalizedId, [
            `Unknown item${displayId}`,
            10,
            -1,
            0,
            0,
            0,
            0,
            0,
            false,
            0,
            false,
            null,
        ]);
    }
    return missingItems.get(normalizedId);
}

/**
 * Existing EAM components index this object directly. Unknown numeric IDs are
 * represented by a stable placeholder without polluting item enumeration.
 */
export const items = new Proxy(itemRecords, {
    get(target, property, receiver) {
        if (
            typeof property === "string"
            && property !== "-1"
            && /^\d+$/.test(property)
            && !Object.prototype.hasOwnProperty.call(target, property)
        ) {
            return missingItem(property);
        }
        return Reflect.get(target, property, receiver);
    },
});

const SUPPORTED_ITEM_KINDS = new Set([
    "Equipment",
    "Skin",
    "PetSkin",
    "PetAbility",
    "Dye",
    "Emote",
    "Entrance",
]);

let runtimeManifest = null;

function rarityCode(rarity) {
    if (typeof rarity !== "string") return 0;
    if (rarity.toUpperCase().includes("UT")) return 1;
    if (rarity.toUpperCase().includes("ST")) return 2;
    return 0;
}

function toLegacyItem(record) {
    const equipment = record.equipment || {};
    return [
        record.displayName || record.internalName || `Item ${record.id}`,
        equipment.slotType ?? 10,
        equipment.tier ?? -1,
        0,
        0,
        0,
        equipment.feedPower ?? 0,
        equipment.bagType ?? 0,
        Boolean(equipment.soulbound),
        rarityCode(equipment.rarity),
        Boolean(equipment.shiny),
        record.spriteHash,
    ];
}

function humanizeIdentifier(identifier) {
    return String(identifier || "")
        .replace(/Completed$/, "")
        .replace(/([a-z0-9])([A-Z])/g, "$1 $2")
        .trim();
}

function replaceObjectContents(target, source) {
    Object.keys(target).forEach((key) => delete target[key]);
    Object.assign(target, source);
}

function mergeItems(objects) {
    const nextItems = {};
    for (const [itemId, record] of Object.entries(objects)) {
        if (
            !record
            || !SUPPORTED_ITEM_KINDS.has(record.kind)
            || !/^[a-f0-9]{64}$/i.test(record.spriteHash || "")
        ) {
            continue;
        }
        nextItems[itemId] = toLegacyItem(record);
    }
    replaceObjectContents(items, nextItems);
}

function mergePlayerStats(stats) {
    const nextStats = {};
    for (const [index, stat] of Object.entries(stats)) {
        if (!stat || typeof stat.id !== "string") continue;
        nextStats[index] = {
            short: stat.id,
            name: stat.displayName || humanizeIdentifier(stat.id) || stat.dungeonId || stat.id,
            isDungeon: Boolean(stat.dungeonId),
        };
    }
    replaceObjectContents(playerStats, nextStats);
}

function mergeFameBonuses(bonuses) {
    const nextBonuses = {};
    for (const bonus of bonuses) {
        if (!bonus || typeof bonus.id !== "string") continue;
        const group = bonus.displayGroup || "Other Bonuses";
        const category = bonus.displayCategory || "Other";
        nextBonuses[group] ??= {};
        nextBonuses[group][category] ??= [];
        nextBonuses[group][category].push({
            id: bonus.id,
            displayName: bonus.displayName || humanizeIdentifier(bonus.id) || bonus.id,
            absoluteBonus: bonus.absoluteBonus ?? 0,
            relativeBonus: bonus.relativeBonus ?? 0,
            maxRepeatCount: bonus.maxRepeatCount ?? 0,
            repeatable: Boolean(bonus.repeatable),
            conditions: Array.isArray(bonus.conditions)
                ? bonus.conditions.map((condition) => ({
                    type: condition.value,
                    threshold: condition.threshold ?? 0,
                    ...(condition.stat ? { stat: condition.stat } : {}),
                }))
                : [],
        });
    }
    replaceObjectContents(fameBonuses, nextBonuses);
}

/**
 * Applies the shared game-data service contract to the mutable objects already
 * imported throughout EAM. No bundled item/stat/fame fallback is retained.
 */
export function mergeRuntimeAssets(manifest) {
    if (
        !manifest
        || typeof manifest !== "object"
        || !manifest.objects
        || !manifest.playerStats
        || !Array.isArray(manifest.fameBonuses)
        || !manifest.buildId
    ) {
        return false;
    }

    mergeItems(manifest.objects);
    mergePlayerStats(manifest.playerStats);
    mergeFameBonuses(manifest.fameBonuses);
    if (Object.keys(items).length === 0 || Object.keys(playerStats).length === 0) {
        return false;
    }

    runtimeManifest = manifest;
    return true;
}

export function getRuntimeAssetCacheKey() {
    if (!runtimeManifest?.buildId) return "unavailable";
    return `${runtimeManifest.buildId}-schema${runtimeManifest.schemaVersion || 1}`;
}

export function getRuntimeSpriteHash(item) {
    const spriteHash = item?.[ITEM_SPRITE_HASH_INDEX];
    return /^[a-f0-9]{64}$/i.test(spriteHash || "")
        ? spriteHash.toLowerCase()
        : null;
}
