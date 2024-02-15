CREATE TABLE Pet (
    id INTEGER PRIMARY KEY,
    name TEXT,
    type INTEGER,
    instanceId INTEGER,
    rarity INTEGER,
    maxAbilityPower INTEGER,
    skin INTEGER,
    shader INTEGER,
    createdOn TEXT,
    incInv INTEGER,
    inv TEXT,
    ability1 TEXT,
    ability2 TEXT,
    ability3 TEXT
);

CREATE TABLE ItemData (
    id INTEGER PRIMARY KEY,
    type INTEGER
);

CREATE TABLE Character (
    id INTEGER PRIMARY KEY,
    objectType INTEGER,
    seasonal INTEGER,
    level INTEGER,
    exp INTEGER,
    currentFame INTEGER,
    equipQS TEXT,
    maxHitPoints INTEGER,
    hitPoints INTEGER,
    maxMagicPoints INTEGER,
    magicPoints INTEGER,
    attack INTEGER,
    defense INTEGER,
    speed INTEGER,
    dexterity INTEGER,
    hpRegen INTEGER,
    mpRegen INTEGER,
    pcStats TEXT,
    healthStackCount INTEGER,
    magicStackCount INTEGER,
    dead INTEGER,
    pet_id INTEGER,
    accountName TEXT,
    texture INTEGER,
    backpackSlots INTEGER,
    has3Quickslots INTEGER,
    creationDate TEXT,
    dataset_id TEXT,
    FOREIGN KEY(pet_id) REFERENCES Pet(id)
);

CREATE TABLE ClassStats (
    id INTEGER PRIMARY KEY,
    objectType INTEGER,
    BestLevel INTEGER,
    BestBaseFame INTEGER,
    BestTotalFame INTEGER
);

CREATE TABLE Stats (
    id INTEGER PRIMARY KEY,
    BestCharFame INTEGER,
    TotalFame INTEGER,
    Fame INTEGER
);

CREATE TABLE Account (
    dataset_id TEXT PRIMARY KEY,
    Credits INTEGER,
    FortuneToken TEXT,
    UnityCampaignPoints TEXT,
    EarlyGameEventTracker TEXT,
    AccountId TEXT,
    CreationTimestamp TEXT,
    EnchanterSupportDust TEXT,
    MaxNumChars INTEGER,
    LastServer TEXT,
    Originating TEXT,
    PetYardType INTEGER,
    ForgeFireEnergy INTEGER,
    Name TEXT,
    Guild TEXT,
    Guildrank INTEGER,
    AccessTokenTimestamp TEXT,
    AccessTokenExpiration TEXT,
    OwnedSkins TEXT,
    stats_id INTEGER,
    FOREIGN KEY(stats_id) REFERENCES Stats(id)
);

CREATE TABLE Equipment (
    id INTEGER PRIMARY KEY,
    character_id INTEGER,
    dataset_id TEXT,
    item_type INTEGER,
    slot INTEGER,
    FOREIGN KEY(character_id, dataset_id) REFERENCES Character(id, dataset_id)
);

CREATE TABLE Account_Storage (
    id INTEGER PRIMARY KEY,
    dataset_id TEXT,
    item_id INTEGER,
    storage_type TEXT,
    FOREIGN KEY(dataset_id) REFERENCES Account(dataset_id),
    FOREIGN KEY(item_id) REFERENCES ItemData(id)
);