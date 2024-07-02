/*
//  MD IDS //  Slot types
//  0      //  0 - empty
//  1      //  1 - swords
//  2      //  2 - dagger
//  3      //  3 - bows
//  4      //  4 - tomes
//  5      //  5 - shields
//  6      //  6 - light armor
//  7      //  7 - heavy armor
//  8      //  8 - wands
//  9      //  9 - rings
//  42010  //  10 - potions
//  42011  //  10 - potions soulbound
//  42012  //  10 - candies
//  42013  //  10 - keys
//  42014  //  10 - portal keys (incs, vials)
//  42015  //  10 - textiles
//  42016  //  10 - skins
//  42017  //  10 - fine spirits
//  42018  //  10 - elixers
//  42019  //  10 - other consumables
//  42020  //  10 - helpful consumables
//  42021  //  10 - assistant consumables
//  42022  //  10 - event items
//  42023  //  10 - tarot cards
//  42024  //  10 - treasures
//  42025  //  10 - effusions
//  42026  //  10 - pet food
//  42027  //  10 - pet stones
//  42028  //  10 - misc items
//  42029  //  10 - other items
//  50000  //  10 - uncategorized
//  ^^^^^really rotmg???^^^^^
//  11     //  11 - spells
//  12     //  12 - seals
//  13     //  13 - cloaks
//  14     //  14 - robes
//  15     //  15 - quivers
//  16     //  16 - helms
//  17     //  17 - staves
//  18     //  18 - poisons
//  19     //  19 - skulls
//  20     //  20 - traps
//  21     //  21 - orbs
//  22     //  22 - prisms
//  23     //  23 - scepters
//  24     //  24 - katanas
//  25     //  25 - stars
//  26     //  26 - eggs
*/

//  map of item groups and their identifier keys
const itemsSlotTypeMap = {
    empty: {slotType: 0, displayName: 'Empty Slot'},
    swords: {slotType: 1, sheet: [160, 0]},
    daggers: {slotType: 2, sheet: [200, 0]},
    bows: {slotType: 3, sheet: [240, 0]},
    tomes: {slotType: 4, sheet: [840, 0]},
    shields: {slotType: 5, sheet: [400, 0]},
    lightarmor: {
        sheet: [0, 40],
        slotType: 6,
        displayName: 'Light Armor'
    },
    heavyarmor: {
        sheet: [960, 0],
        slotType: 7,
        displayName: 'Heavy Armor'
    },
    wands: {slotType: 8, sheet: [320, 0]},
    rings: {slotType: 9, sheet: [80, 40]},
    potions: {
        sheet: [0, 0],
        name: new RegExp(/^(?:Greater )?Potion of .*$/i),
        slotType: 10,
        virtualSlotType: 42010,
        tier: [2, 4, 5, -1],
        bagType: 5,
        soulbound: false
    },
    potionssb: {
        sheet: [40, 0],
        displayName: 'Potions (SB)',
        name: new RegExp(/^(Potion of .*|Mystery Stat .*)$/i),
        slotType: 10,
        virtualSlotType: 42011,
        tier: -1,
        bagType: 5,
        soulbound: true
    },
    candies: {
        sheet: [80, 0],
        name: new RegExp(/^Candy of Extreme .*$/i),
        slotType: 10,
        virtualSlotType: 42012,
        tier: -1,
        bagType: 7,
        soulbound: true
    },
    portkeys: {
        sheet: [160, 40],
        displayName: 'Portal Keys',
        name: new RegExp(/^(Wine Cellar Incantation|Vial of Pure Darkness)$/i),
        slotType: 10,
        virtualSlotType: 42014,
        tier: -1,
        bagType: [8,4]
    },
    textiles: {
        sheet: [200, 40],
        name: new RegExp(/^(?:(Mystery|Accessory|Clothing) (Dye|Cloth).*|.* (Dye|Cloth))$/i),
        include: new RegExp(/^(?:(Mystery|Accessory|Clothing) (Dye|Cloth).*|.* (Dye|Cloth))$/i),
        displayName: 'Dyes/Clothes',
        slotType: 10,
        virtualSlotType: 42015,
        tier: -1,
        bagType: 2,
        feedPower: 0,
        soulbound: true,
        utst: 0
    },
    skins: {
        sheet: [240, 40],
        name: new RegExp(/^((?!.* pet skin|Red Nosed Skin|.* Bagston Skin|Christmas Tree Skin).)* skin(?: \(.*\))?$/i),
        slotType: 10,
        virtualSlotType: 42016,
        tier: -1,
        bagType: [7, 8],
        include: new RegExp(/^((?!.* pet skin|Red Nosed Skin|.* Bagston Skin|Christmas Tree Skin).)(?:.* Skin|Mystery Skin .*)$/i)
    },
    petstones: {
        sheet: [720, 40],
        name: new RegExp(/^(?:.* pet skin|(?:Halloween )?Mystery Pet Stone|Red Nosed Skin|.* Bagston Skin|Christmas Tree Skin|.* Skin Unlocker)(?: \(.*\))?$/i),
        slotType: 10,
        virtualSlotType: 42027,
        tier: -1,
        bagType: [7, 8]
    },
    //  skipping greater/minor hpmp
    finespirits: {
        sheet: [280, 40],
        displayName: 'Fine Spirits',
        slotType: 10,
        virtualSlotType: 42017,
        tier: 3,
        bagType: 2
    },
    testing: {
        sheet: [760, 40],
        displayName: 'Testing/Dev',
        name: new RegExp(/^((?!Effusion of .*).*)(Creator's Ring|Level 20 Unlocker|Crashy|XP Booster Test|Kingdom Skins .*|Parasite Chambers permakey|Pet Form Stone|Cursed Crown Teleporter|Realm Closer|Godlands Teleporter|Test Guill Spawner|FR Test permakey|FR God Spawner|Ice Cave permakey|Inner Sanctum Permakey|Apple of Semi Maxening|Gift of Krathan|Apple Of Extreme Maxening|Level Chicken|Fame Chicken|LH Lost Halls Debug Item|Lost Halls PermaKey|Guild Hall .*|Dragon Buddies|Enemy Spawner|Tincture of Fear|Tincture of Courage|Testing Gift|Greater Health Potion|Greater Magic Potion|Ivory Wyvern Key|.* Goodie Bag|Oryx Horde key|Oryx Mystery Gift|Wine Cellar Key|Cult Key|Oryx Chamber Key|Skeleton Key|Void Key|Court of Oryx Key|Oryx Chicken Chamber Key|Oryx's Castle Key)$/i),
        virtualSlotType: 42025,
        slotType: 10
    },
    keys: {
        sheet: [120, 40],
        name: new RegExp(/^.* Key(?: \(.*\))?$/i),
        slotType: 10,
        virtualSlotType: 42013,
        tier: -1,
        soulbound: true,
        utst: 0,
        include: new RegExp(/^(?:Secluded Thicket Key)$/)
    },
    helpfulconsumables: {
        sheet: [400, 40],
        displayName: 'Helpful',
        slotType: 10,
        virtualSlotType: 42020,
        name: new RegExp(/^((?!snowball|.* firecracker|Backpack|Santa's Bag|Mark of .*).*)$/i),
        tier: -1,
        bagType: 2,
        include: new RegExp(/^(?:Santa's Sleigh|Egg Nog|Fruitcake|Candy Cane|Figgy Pudding|Mistletoe|Elixir of .*|Effusion of .*|Potion of Max .*)$/i)
    },
    assistants: {
        sheet: [440, 40],
        displayName: 'Loot/Exp',
        virtualSlotType: 42021,
        name: new RegExp(/^((?!.* Chest).*)(XP Booster.*?|Loot.*Potion|.* Chest|Lucky Clover|Golden Lucky Clover)$/i),
        slotType: 10,
        tier: -1,
        bagType: 7,
    },
    unlockers: {
        sheet: [360, 40],
        slotType: 10,
        virtualSlotType: 42019,
        name: new RegExp(/^((?!Snowball|Paddy's Flying Hat|Rainbow Clover|(?:Small|Large) Firecracker|.* Gummy Worm|(Candy|Caramel) Apple).*)(?:Backpack|Santa's Bag|Char Slot Unlocker|Vault Chest Unlocker|.*?Coupon.*?|Mystery ST.*?|Potion of Max Level|Witch's Skull|Demon Pumpkin|Mystery (.*? )?ST Crate|.*? Chest|Supreme Token|Easter Mystery Gift|.* Weapon Cache|Oryxmas Mystery Armor|Guild Present)$/i),
        tier: -1,
        bagType: [2, 6, 7, 8],
        soulbound: true
    },
    marks: {
        sheet: [520, 40],
        slotType: 10,
        virtualSlotType: 42028,
        name: new RegExp(/^(?:Mark of .*)$/i)
    },
    tarot: {
        sheet: [560, 40],
        displayName: 'Tarot Cards',
        name: new RegExp(/^.* Tarot Card$/i),
        slotType: 10,
        virtualSlotType: 42023,
        tier: -1,
        bagType: 7,
        soulbound: false,
        utst: 0
    },
    treasures: {
        sheet: [600, 40],
        displayName: 'Treasure',
        name: new RegExp(/^((?!Heart|Love Letter).)*$/i),
        slotType: 10,
        virtualSlotType: 42024,
        tier: -1,
        bagType: 7,
        soulbound: false,
        utst: 0
    },
    petfood: {
        sheet: [680, 40],
        displayName: 'Pet Food',
        virtualSlotType: 42026,
        name: new RegExp(/^((?!Apple of Semi Maxening|Level Chicken|Snowball|Valentine Launcher|Treasure Map|Beach Ball|Level 20 Unlocker|Crystal of .*).)*$/i),
        slotType: 10,
        tier: -1,
        bagType: 7,
        feedPower: {
            gte: 150,
            lte: 20000
        },
        soulbound: true,
        include: new RegExp(/^(?:Picante Taco|Napalm Taco|Atomic Taco|Power Pizza|Great Taco|Double Cheeseburger Deluxe|Superburger|Soft Drink|Grapes of Wrath|Fries|Ambrosia|Cranberries|Ear of Corn|Sliced Yam|Pumpkin Pie|Thanksgiving Turkey|Egg Ommlette|Reindeer Food|Glazed Apple|Christmas Turkey Leg|Christmas Tree Cupcake|Gingerbread House|Glowing Green Goo|Oryx Cookie|Chaos Cake|Solar Energy Drink|Caramel Apple|Candy Apple|Sugar Gummy Worm|Rainbow Gummy Worm|Eyecicle|Chocolate Bar|)$/)
    },
    eventitems: {
        sheet: [480, 40],
        displayName: 'Event Items',
        slotType: 10,
        virtualSlotType: 42022,
        name: new RegExp(/^((?!Mark of .*|.* Chest|Feed|Treasure Map).*)$/i),
        tier: -1,
        bagType: 7,
        soulbound: true,
        include: new RegExp(/^(?:Love Letter .*|Stone Bag|Moss Bag|Easter Mystery Gift|Reindeer Mystery Bag)$/)
    },
    other: {
        sheet: [800, 40],
        displayName: 'Uncategorized',
        virtualSlotType: 42029,
        slotType: 10
    },
    spells: {slotType: 11, sheet: [720, 0]},
    seals: {slotType: 12, sheet: [480, 0]},
    cloaks: {slotType: 13, sheet: [520, 0]},
    robes: {slotType: 14, sheet: [40, 40]},
    quivers: {slotType: 15, sheet: [640, 0]},
    helms: {slotType: 16, sheet: [440, 0]},
    staves: {slotType: 17, sheet: [280, 0]},
    poisons: {slotType: 18, sheet: [560, 0]},
    skulls: {slotType: 19, sheet: [760, 0]},
    traps: {slotType: 20, sheet: [680, 0]},
    orbs: {slotType: 21, sheet: [800, 0]},
    prisms: {slotType: 22, sheet: [600, 0]},
    scepters: {slotType: 23, sheet: [880, 0]},
    katanas: {slotType: 24, sheet: [360, 0]},
    stars: {slotType: 25, sheet: [920, 0]},
    eggs: {slotType: 26, sheet: [840, 40]},
    wakis: {slotType: 27, sheet: [920, 40]},
    lutes: {slotType: 28, sheet: [960, 40]},
    maces: {slotType: 29, sheet: [0, 80]},
    sheaths: {slotType: 30, sheet: [40, 80]},
};

export default itemsSlotTypeMap;