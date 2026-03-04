
import fameBonuses from '../assets/fameBonuses';
import playerStats from '../assets/playerStats';
import { isStatMaxed } from './realmCharacterUtils';

export function calculateFameBonuses(pcStats, character) {
    const results = {};
    for (const groupName of Object.keys(fameBonuses)) {
        results[groupName] = calculateFameBonusOfStatGroup(pcStats, groupName, character);
    }
    return results;
}

export function calculateFameBonusOfStatGroup(pcStats, fameBonusGroupName, character = null) {
    const group = fameBonuses[fameBonusGroupName];
    if (!group) {
        console.warn(`Unknown fame bonus group: ${fameBonusGroupName}`);
        return null;
    }

    const categories = {};
    for (const [categoryName, entries] of Object.entries(group)) {
        categories[categoryName] = calculateFameBonusOfCategory(pcStats, entries, character);
    }
    return categories;
}

// Maps MaxedStat condition stat names to character object field names
const MAXED_STAT_MAP = {
    health:    'max_hit_points',
    magic:     'max_magic_points',
    attack:    'attack',
    defense:   'defense',
    speed:     'speed',
    dexterity: 'dexterity',
    vitality:  'hp_regen',
    wisdom:    'mp_regen',
};

/**
 * Builds a lookup from playerStat short name -> current stat_value
 * using the character's pcStats array and the playerStats definition.
 */
function buildStatLookup(pcStats) {
    const lookup = {};
    for (const s of pcStats) {
        const def = playerStats[s.stat_type];
        if (def) lookup[def.short] = s.stat_value;
    }
    return lookup;
}

/**
 * Check whether a single condition is met.
 * Returns true | false, or null if it cannot be determined (unknown condition type).
 */
function isConditionMet(condition, statLookup, character) {
    const threshold = Number(condition.threshold);
    switch (condition.type) {
        case 'StatValue':
            return (statLookup[condition.stat] ?? 0) >= threshold;
        case 'MaxedStat': {
            if (!character) return null;
            const charStatName = MAXED_STAT_MAP[condition.stat];
            if (!charStatName) return null;
            return isStatMaxed(character, charStatName);
        }
        case 'FirstCharacter':
            return character?.char_id === 0;
        default:
            return null;
    }
}

/**
 * How many times a repeatable bonus has been earned (capped at maxRepeatCount).
 * Repeatable entries always have exactly one StatValue condition.
 */
function getTimesAchieved(entry, statLookup) {
    if (!entry.repeatable || entry.conditions.length !== 1) return 0;
    const cond = entry.conditions[0];
    if (cond.type !== 'StatValue') return 0;
    const val = statLookup[cond.stat] ?? 0;
    const threshold = Number(cond.threshold);
    if (threshold <= 0) return 0;
    return Math.min(Math.floor(val / threshold), entry.maxRepeatCount);
}

/**
 * Calculate the missing amounts for an entry that hasn't been fully achieved yet.
 * For repeatables, nextRepeat is how many times it's already been earned (so we
 * calculate the threshold for the NEXT repeat).
 */
function getMissingAmounts(entry, statLookup, character, nextRepeat = 0) {
    return entry.conditions.map(cond => {
        if (cond.type === 'StatValue') {
            const threshold = Number(cond.threshold) * (nextRepeat > 0 ? nextRepeat + 1 : 1);
            const current = statLookup[cond.stat] ?? 0;
            return {
                type: 'StatValue',
                stat: cond.stat,
                threshold,
                current,
                missing: Math.max(0, threshold - current),
            };
        }
        if (cond.type === 'MaxedStat') {
            const charStatName = MAXED_STAT_MAP[cond.stat] ?? cond.stat;
            const isMet = character ? isStatMaxed(character, charStatName) : false;
            return {
                type: 'MaxedStat',
                stat: cond.stat,
                charStat: charStatName,
                missing: isMet ? 0 : null, // null = non-numeric requirement
            };
        }
        return { type: cond.type, stat: cond.stat ?? null, missing: null };
    });
}

/**
 * Returns true when all entries in the category test the SAME single stat with
 * increasing thresholds — i.e. they are sequential tiers of the same achievement.
 * In that case we stop at the first unmet entry (you can't skip tiers).
 * For independent entries (different stats / conditions) we evaluate every entry.
 */
function isTieredCategory(sortedCategory) {
    if (sortedCategory.length <= 1) return true;
    return (
        sortedCategory.every(e => e.conditions.length === 1 && e.conditions[0].type === 'StatValue') &&
        new Set(sortedCategory.map(e => e.conditions[0].stat)).size === 1
    );
}

/**
 * Calculate the fame bonus totals and progression info for a single DisplayCategory.
 *
 * @param {Array}        pcStats   - Character's pc_stats: [{stat_type, stat_value, ...}]
 * @param {Array}        category  - Array of FameBonus entries for one DisplayCategory
 * @param {object|null}  character - Character object (needed for MaxedStat conditions)
 * @returns {{
 *   absoluteBonus: number,
 *   relativeBonus: number,
 *   isTiered: boolean,
 *   highestCategoryAchieved: {id, displayName} | null,
 *   nextCategories: object[]   // all simultaneously-doable unmet entries (1 item for tiered, N for independent)
 *   entryResults: Array<{id, displayName, achieved, absoluteBonus, relativeBonus, completedCount, totalCount}>
 * }}
 */
export function calculateFameBonusOfCategory(pcStats, category, character = null) {
    const statLookup = buildStatLookup(pcStats);

    // Sort entries by their minimum condition threshold so tiered achievements
    // are always processed in ascending difficulty order, regardless of XML ordering.
    const sortedCategory = [...category].sort((a, b) => {
        const minThreshold = (entry) =>
            entry.conditions.reduce((min, c) => Math.min(min, Number(c.threshold) || 0), Infinity);
        return minThreshold(a) - minThreshold(b);
    });

    const tiered = isTieredCategory(sortedCategory);

    let totalAbsolute = 0;
    let totalRelative = 0;
    let highestAchieved = null;
    const nextCategories = [];
    const entryResults = [];

    for (const entry of sortedCategory) {
        if (entry.repeatable && entry.maxRepeatCount > 0) {
            const times = getTimesAchieved(entry, statLookup);

            if (times > 0) {
                totalAbsolute += entry.absoluteBonus * times;
                totalRelative += entry.relativeBonus * times;
                highestAchieved = { id: entry.id, displayName: entry.displayName };
            }
            entryResults.push({
                id: entry.id,
                displayName: entry.displayName,
                achieved: times > 0,
                absoluteBonus: entry.absoluteBonus * times,
                relativeBonus: entry.relativeBonus * times,
                completedCount: times,
                totalCount: entry.maxRepeatCount,
                conditionDetails: getMissingAmounts(entry, statLookup, character, times),
            });

            // Still has repeats remaining — this is the next goal
            if (times < entry.maxRepeatCount && nextCategories.length === 0) {
                nextCategories.push({
                    ...entry,
                    timesAchieved: times,
                    missingAmounts: getMissingAmounts(entry, statLookup, character, times),
                });
            }
        } else {
            const condResults = entry.conditions.map(c => isConditionMet(c, statLookup, character));
            const allMet = condResults.every(r => r === true);
            const completedCount = condResults.filter(r => r === true).length;

            entryResults.push({
                id: entry.id,
                displayName: entry.displayName,
                achieved: allMet,
                absoluteBonus: allMet ? entry.absoluteBonus : 0,
                relativeBonus: allMet ? entry.relativeBonus : 0,
                completedCount,
                totalCount: entry.conditions.length,
                conditionDetails: getMissingAmounts(entry, statLookup, character, 0),
            });

            if (allMet) {
                totalAbsolute += entry.absoluteBonus;
                totalRelative += entry.relativeBonus;
                highestAchieved = { id: entry.id, displayName: entry.displayName };
            } else {
                nextCategories.push({
                    ...entry,
                    missingAmounts: getMissingAmounts(entry, statLookup, character, 0),
                });
                // For tiered categories stop here — higher tiers cannot be skipped.
                if (tiered) break;
            }
        }
    }

    return {
        absoluteBonus: totalAbsolute,
        relativeBonus: totalRelative,
        isTiered: tiered,
        highestCategoryAchieved: highestAchieved,
        nextCategories,
        entryResults,
    };
}