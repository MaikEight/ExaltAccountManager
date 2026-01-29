import { classes } from "../assets/constants";

export const BACKPACK_ITEM_ID = 3180;
export const BACKPACK_EXTENDER_ITEM_ID = 65280;
export const ADCENTUREERS_BELT = 31883;

/**
* Extract equipment IDs from a comma-separated string
* Only the first 4 IDs are returned, if more exist
* If less than 4 IDs exist, the remaining slots are filled with -1 (empty)
* @param {string} equipmentString 
* @returns {number[]} Array of equipment IDs - length 4
*/
function extractEquipmentIds(equipmentString) {
    if (!equipmentString) return [];
    const equipmentIds = equipmentString.split(',').map(id => parseInt(id, 10));
    const filledEquipmentIds = equipmentIds.slice(0, 4);
    while (filledEquipmentIds.length < 4) {
        filledEquipmentIds.push(-1);
    }
    return filledEquipmentIds;
};

function getXof8OfCharacter(character) {
    let x = 0;
    const cls = classes[character.char_class];

    try {
        x += cls[3][0] - character.max_hit_points <= 0 ? 1 : 0;
        x += cls[3][1] - character.max_magic_points <= 0 ? 1 : 0;
        x += cls[3][2] - character.attack <= 0 ? 1 : 0;
        x += cls[3][3] - character.defense <= 0 ? 1 : 0;
        x += cls[3][4] - character.speed <= 0 ? 1 : 0;
        x += cls[3][5] - character.dexterity <= 0 ? 1 : 0;
        x += cls[3][6] - character.hp_regen <= 0 ? 1 : 0;
        x += cls[3][7] - character.mp_regen <= 0 ? 1 : 0;
    } catch (e) {
        console.error(e);
    }
    return x;
}

function isStatMaxed(character, statName) {
    const cls = classes[character.char_class];
    const statIndexMap = {
        "max_hit_points": 0,
        "max_magic_points": 1,
        "attack": 2,
        "defense": 3,
        "speed": 4,
        "dexterity": 5,
        "hp_regen": 6,
        "mp_regen": 7,
    };

    const statIndex = statIndexMap[statName];
    if (statIndex === undefined) {
        return false;
    }

    try {
        return character[statName] >= cls[3][statIndex];
    } catch (e) {
        console.error(e);
        return false;
    }
}

function crucibleTimeStampToDate(ticks) {
    if (!ticks) {
        return null;
    }

    if (typeof ticks === 'string' || typeof ticks === 'number') {
        ticks = BigInt(ticks);
    }

    const epochMs = Date.UTC(2008, 6, 31, 0, 0, 0); // 2008-07-31T00:00:00Z (month is 0-based)

    const ms = ticks / 10000n;            // 100ns -> ms
    const subMs100ns = ticks % 10000n;    // remainder in 100ns units (0..9999)

    const d = new Date(epochMs + Number(ms));
    return d;
}

function isCrucibleActive(character) {
    if (!character.crucible_active) {
        return false;
    }

    const d = crucibleTimeStampToDate(BigInt(character.crucible_active));
    return Date.now() < d.getTime();
}

export { extractEquipmentIds, getXof8OfCharacter, isStatMaxed, isCrucibleActive, crucibleTimeStampToDate };