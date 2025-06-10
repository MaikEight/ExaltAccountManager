const STATTAGS = 'MaxHitPoints MaxMagicPoints Attack Defense Speed Dexterity HpRegen MpRegen'.split(' ');
/** The index of the stat as its found in the base64 encoded string is the key, and the value is a human understandable description of the stat. */
const pcStatsIndexToStatDescription = {
    0: 'Shots fired',
    1: 'Hits',
    2: 'Ability uses',
    3: 'Tiles discovered',
    4: 'Teleports',
    5: 'Potions drunk',
    6: 'Kills',
    7: 'Assists',
    8: 'God kills',
    9: 'Assists against Gods',
    10: 'Cube kills',
    11: 'Oryx kills',
    12: 'Quests completed',
    13: 'Pirate Caves',
    14: 'Undead Lairs',
    15: 'Abyss of Demons',
    16: 'Snake Pits',
    17: 'Spider Dens',
    18: 'Sprite Worlds',
    20: 'Minutes active',
    21: 'Tomb of the Ancients',
    22: 'Ocean Trenches',
    23: 'Forbidden Jungles',
    24: 'Manor of the Immortals',
    25: 'Forest Mazes',
    26: 'Lair of Draconis',
    27: 'Candyland Hunting Grounds',
    28: 'Haunted Cemeteries',
    29: 'Cave of A Thousand Treasures',
    30: 'Mad Labs',
    31: 'Davy Jones\' Lockers',
    34: 'Ice Caves',
    35: 'Deadwater Docks',
    36: 'The Crawling Depths',
    37: 'Woodland Labyrinths',
    38: 'Battle for the Nexus',
    39: 'The Shatters',
    40: 'Belladonna\'s Garden',
    41: 'Puppet Master\'s Theatre',
    42: 'Toxic Sewers',
    43: 'The Hive',
    44: 'Mountain Temple',
    45: 'The Nest',
    47: 'Lost Halls',
    48: 'Cultist Hideout',
    49: 'The Void',
    50: 'Puppet Master\'s Encore',
    51: 'Lair of Shaitan',
    52: 'Parasite Chambers',
    53: 'Magic Woods',
    54: 'Cnidarian Reef',
    55: 'Secluded Thicket',
    56: 'Cursed Library',
    57: 'Fungal Cavern',
    58: 'Crystal Cavern',
    59: 'Ancient Ruins',
    60: 'Dungeon types',
    61: 'Forax',
    62: 'Heroic Abyss of Demons',
    63: 'Heroic Undead Lair',
    64: 'High Tech Terror',
    65: 'Ice Tomb',
    66: 'Katalund',
    67: 'Mad God Mayhems',
    68: 'Malogia',
    69: 'Oryx\'s Castle',
    70: 'Oryx\'s Chamber',
    71: 'Oryx\'s Sanctuary',
    72: 'Rainbow Road',
    73: 'Santa Workshop',
    74: 'The Machine',
    75: 'Untaris',
    76: 'Wine Cellar',
    77: 'Party level-ups',
    78: 'Lesser Gods kills',
    79: 'Encounter kills',
    80: 'Hero kills',
    82: 'Critter kills',
    83: 'Beast kills',
    84: 'Humanoid kills',
    85: 'Undead kills',
    86: 'Nature kills',
    87: 'Construct kills',
    88: 'Grotesque kills',
    89: 'Structure kills',
    90: 'The Third Dimension',
    91: 'Beachzone',
    92: 'Hidden Interregnum',
    93: 'Sulfurous Wetlands',
    94: 'Kogbold Steamworks',
    95: 'Moonlight Village',
    96: 'Advanced Kogbold Steamworks',
    97: 'Advanced Nest',
    98: 'The Tavern',
    99: 'Queen Bunny Chamber',
    100: 'Stat Potions consumed',
};

/* NOTE: This Enum MUST match BOTH the above pcStatsIndexToStatDescription variable, and the names of the dungeons in public/realm/dungeons.**/
export const pcStatsDescriptionEnum = {
    SHOTS_FIRED: "Shots fired",
    HITS: "Hits",
    ABILITY_USES: "Ability uses",
    TILES_DISCOVERED: "Tiles discovered",
    TELEPORTS: "Teleports",
    POTIONS_DRUNK: "Potions drunk",
    KILLS: "Kills",
    ASSISTS: "Assists",
    GOD_KILLS: "God kills",
    ASSISTS_AGAINST_GODS: "Assists against Gods",
    CUBE_KILLS: "Cube kills",
    ORYX_KILLS: "Oryx kills",
    QUESTS_COMPLETED: "Quests completed",
    PIRATE_CAVES: "Pirate Caves",
    UNDEAD_LAIRS: "Undead Lairs",
    ABYSS_OF_DEMONS: "Abyss of Demons",
    SNAKE_PITS: "Snake Pits",
    SPIDER_DENS: "Spider Dens",
    SPRITE_WORLDS: "Sprite Worlds",
    MINUTES_ACTIVE: "Minutes active",
    TOMB_OF_THE_ANCIENTS: "Tomb of the Ancients",
    OCEAN_TRENCHES: "Ocean Trenches",
    FORBIDDEN_JUNGLES: "Forbidden Jungles",
    MANOR_OF_THE_IMMORTALS: "Manor of the Immortals",
    FOREST_MAZES: "Forest Mazes",
    LAIR_OF_DRACONIS: "Lair of Draconis",
    CANDYLAND_HUNTING_GROUNDS: "Candyland Hunting Grounds",
    HAUNTED_CEMETERIES: "Haunted Cemeteries",
    CAVE_OF_A_THOUSAND_TREASURES: "Cave of A Thousand Treasures",
    MAD_LABS: "Mad Labs",
    DAVY_JONES_LOCKERS: "Davy Jones' Lockers",
    ICE_CAVES: "Ice Caves",
    DEADWATER_DOCKS: "Deadwater Docks",
    THE_CRAWLING_DEPTHS: "The Crawling Depths",
    WOODLAND_LABYRINTHS: "Woodland Labyrinths",
    BATTLE_FOR_THE_NEXUS: "Battle for the Nexus",
    THE_SHATTERS: "The Shatters",
    BELLADONNAS_GARDEN: "Belladonna's Garden",
    PUPPET_MASTERS_THEATRE: "Puppet Master's Theatre",
    TOXIC_SEWERS: "Toxic Sewers",
    THE_HIVE: "The Hive",
    MOUNTAIN_TEMPLE: "Mountain Temple",
    THE_NEST: "The Nest",
    LOST_HALLS: "Lost Halls",
    CULTIST_HIDEOUT: "Cultist Hideout",
    THE_VOID: "The Void",
    PUPPET_MASTERS_ENCORE: "Puppet Master's Encore",
    LAIR_OF_SHAITAN: "Lair of Shaitan",
    PARASITE_CHAMBERS: "Parasite Chambers",
    MAGIC_WOODS: "Magic Woods",
    CNIDARIAN_REEF: "Cnidarian Reef",
    SECLUDED_THICKET: "Secluded Thicket",
    CURSED_LIBRARY: "Cursed Library",
    FUNGAL_CAVERN: "Fungal Cavern",
    CRYSTAL_CAVERN: "Crystal Cavern",
    ANCIENT_RUINS: "Ancient Ruins",
    DUNGEON_TYPES: "Dungeon types",
    FORAX: "Forax",
    HEROIC_ABYSS_OF_DEMONS: "Heroic Abyss of Demons",
    HEROIC_UNDEAD_LAIR: "Heroic Undead Lair",
    HIGH_TECH_TERROR: "High Tech Terror",
    ICE_TOMB: "Ice Tomb",
    KATALUND: "Katalund",
    MAD_GOD_MAYHEMS: "Mad God Mayhems",
    MALOGIA: "Malogia",
    ORYXS_CASTLE: "Oryx's Castle",
    ORYXS_CHAMBER: "Oryx's Chamber",
    ORYXS_SANCTUARY: "Oryx's Sanctuary",
    RAINBOW_ROAD: "Rainbow Road",
    SANTA_WORKSHOP: "Santa Workshop",
    THE_MACHINE: "The Machine",
    UNTARIS: "Untaris",
    WINE_CELLAR: "Wine Cellar",
    PARTY_LEVEL_UPS: "Party level-ups",
    LESSER_GODS_KILLS: "Lesser Gods kills",
    ENCOUNTER_KILLS: "Encounter kills",
    HERO_KILLS: "Hero kills",
    CRITTER_KILLS: "Critter kills",
    BEAST_KILLS: "Beast kills",
    HUMANOID_KILLS: "Humanoid kills",
    UNDEAD_KILLS: "Undead kills",
    NATURE_KILLS: "Nature kills",
    CONSTRUCT_KILLS: "Construct kills",
    GROTESQUE_KILLS: "Grotesque kills",
    STRUCTURE_KILLS: "Structure kills",
    THE_THIRD_DIMENSION: "The Third Dimension",
    BEACHZONE: "Beachzone",
    HIDDEN_INTERREGNUM: "Hidden Interregnum",
    SULFUROUS_WETLANDS: "Sulfurous Wetlands",
    KOGBOLD_STEAMWORKS: "Kogbold Steamworks",
    MOONLIGHT_VILLAGE: "Moonlight Village",
    ADVANCED_KOGBOLD_STEAMWORKS: "Advanced Kogbold Steamworks",
    ADVANCED_NEST: "Advanced Nest",
    THE_TAVERN: "The Tavern",
    QUEEN_BUNNY_CHAMBER: "Queen Bunny Chamber",
    SPECTRAL_PENITENTIARY: "Spectral Penitentiary", // Note: Currently not parsed out of pcStats. We do not know the position of this stat.
    STAT_POTIONS_CONSUMED: "Stat Potions consumed",
}

const shortpcstats = {
    13: 'pcave',
    14: 'udl',
    15: 'abby',
    16: 'snake',
    17: 'spider',
    18: 'sprite',
    21: 'tomb',
    22: 'ot',
    23: 'jungle',
    24: 'manor',
    25: 'forest',
    26: 'lod',
    27: 'cland',
    28: 'cem',
    29: 'tcave',
    30: 'lab',
    31: 'davy',
    34: 'Ice Caves',
    35: 'ddocks',
    36: 'cdepths',
    37: 'wlab',
    38: 'bnex',
    39: 'shats',
    40: 'bella',
    41: 'pup',
    42: 'sew',
    43: 'hive',
    44: 'temple',
    45: 'nest',
    47: 'lh',
    48: 'cult',
    49: 'void',
    50: 'encore',
    51: 'shaitan',
    52: 'para',
    53: 'mwoods',
    54: 'reef',
    55: 'thicket',
    56: 'lib',
    57: 'fungal',
    58: 'crystal',
    59: 'ruins',
    61: 'Forax',
    62: 'habby',
    63: 'hudl',
    64: 'htt',
    65: 'Ice Tomb',
    66: 'Katalund',
    67: 'mgm',
    68: 'Malogia',
    69: 'castle',
    70: 'o1',
    71: 'o3',
    72: 'rroad',
    73: 'workshop',
    74: 'machine',
    75: 'Untaris',
    76: 'o2',
    90: '3d',
    91: 'Beachzone',
    92: 'interregnum',
    93: 'wetland',
    94: 'kog',
    95: 'mv',
    96: 'akog',
    97: 'anest',
    98: 'tavern',
    99: 'bunny'
};
/**
 * It is recommended to use pcStatsDescriptionEnum to access the desired values of the pcStats.
 * @see pcStatsDescriptionEnum
 * @param {Map} pcStats 
 * @returns a map whose key is a stat name and value is the value of the stat. One such key may be 'Shots fired' and value is self explanatory.
 */
export function parsePcStats(pcStats) {
    // Credits to Tadus for logic to read pcStats and knowing the order that each stat appears in.
    // TODO: Missing spectral penitentiary
    function readInt32BE(str, idx) {
        var r = 0;
        for (var i = 0; i < 4; i++) {
            var t = str.charCodeAt(idx + 3 - i);
            r += t << (8 * i);
        }
        return r;
    }

    pcStats = pcStats || '';
    var b = atob(pcStats.replace(/-/g, '+').replace(/_/g, '/'));
    var i = 0;
    const end_of_unknown = 4;
    i = end_of_unknown;
    const end_of_flags = 20;
    let statsCount = 0;
    const statsFlagged = [];
    let totalFlagsRead = 0;
    while (i < end_of_flags) {
        const t = readInt32BE(b, i); i += 4;
        totalFlagsRead += 8 * 4;
        while (statsCount < totalFlagsRead) {
            if (t & (1 << (statsCount % (8 * 4)))) {
                statsFlagged.push(statsCount);
            }
            statsCount++
        }
    }
    function readNextStat() {
        let r = b.charCodeAt(i); i++;
        if (r < 0x40) {
            return r;
        }
        if (r >= 0x80 && r <= 0xBF) {
            r = r % 0x40;
            let bits_read = 6;
            while (i < b.length) {
                const f = b.charCodeAt(i); i++;
                r += ((f & 0x7F) << bits_read)
                if ((f & 0x80) === 0) {
                    return r;
                }
                bits_read += 7
                if (i >= b.length) {
                    console.error("Failed to properly read PCStats. Reached the end of the string while trying to read a number the last number read in PCString will be corrupted.");
                    return r;
                }
            }
        }
        else {
            console.error("Failed to properly read PCStats. Found hex (0x" + r.toString(16).padStart(2, '0') + ") at the begining of a number. Number was expected to be between 0x00 and 0x3F or between 0x80 and 0xBF. The rest of the PCString read will be corrupted.");
            return r;
        }
    }
    const stats = []
    while (i < b.length) {
        stats.push(readNextStat());
    }
    if (statsFlagged.length !== stats.length) {
        console.error("The flag vector is not consistent with the number of values read from PCStats. There are a different number of flags set (" + statsFlagged.length + ") than values in PCStats (" + stats.length + "). The values read from PCStats may be corrupted.")
    }

    const r = [];
    for (let j = 0; j < statsFlagged.length; j++) {
        r[statsFlagged[j]] = stats[j];
    }

    // zero out any undefined stats.
    // Note that this counts from 0 to the length of the encoded base64 string - 1. Feels strange to me...
    // As long as the length of the encoded base64 string is always an overestimate of how many stats there are, its probably fine. 
    for (var i in pcStats) {
        if (!r[i]) 
            r[i] = 0;
    }

    const statNameToStatValueMap = new Map();
    for (const index in pcStatsIndexToStatDescription) {
        statNameToStatValueMap.set(pcStatsIndexToStatDescription[index], r[index])
    }
    return statNameToStatValueMap;
}