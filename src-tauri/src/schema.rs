#![allow(non_snake_case)]
// @generated automatically by Diesel CLI.


diesel::table! {
    Account (dataset_id) {
        dataset_id -> Nullable<Text>,
        Credits -> Nullable<Integer>,
        FortuneToken -> Nullable<Text>,
        UnityCampaignPoints -> Nullable<Text>,
        EarlyGameEventTracker -> Nullable<Text>,
        AccountId -> Nullable<Text>,
        CreationTimestamp -> Nullable<Text>,
        EnchanterSupportDust -> Nullable<Text>,
        MaxNumChars -> Nullable<Integer>,
        LastServer -> Nullable<Text>,
        Originating -> Nullable<Text>,
        PetYardType -> Nullable<Integer>,
        ForgeFireEnergy -> Nullable<Integer>,
        Name -> Nullable<Text>,
        Guild -> Nullable<Text>,
        Guildrank -> Nullable<Integer>,
        AccessTokenTimestamp -> Nullable<Text>,
        AccessTokenExpiration -> Nullable<Text>,
        OwnedSkins -> Nullable<Text>,
        stats_id -> Nullable<Integer>,
    }
}

diesel::table! {
    Account_Storage (id) {
        id -> Nullable<Integer>,
        dataset_id -> Nullable<Text>,
        item_id -> Nullable<Integer>,
        storage_type -> Nullable<Text>,
    }
}

diesel::table! {
    Character (id) {
        id -> Nullable<Integer>,
        objectType -> Nullable<Integer>,
        seasonal -> Nullable<Integer>,
        level -> Nullable<Integer>,
        exp -> Nullable<Integer>,
        currentFame -> Nullable<Integer>,
        equipQS -> Nullable<Text>,
        maxHitPoints -> Nullable<Integer>,
        hitPoints -> Nullable<Integer>,
        maxMagicPoints -> Nullable<Integer>,
        magicPoints -> Nullable<Integer>,
        attack -> Nullable<Integer>,
        defense -> Nullable<Integer>,
        speed -> Nullable<Integer>,
        dexterity -> Nullable<Integer>,
        hpRegen -> Nullable<Integer>,
        mpRegen -> Nullable<Integer>,
        pcStats -> Nullable<Text>,
        healthStackCount -> Nullable<Integer>,
        magicStackCount -> Nullable<Integer>,
        dead -> Nullable<Integer>,
        pet_id -> Nullable<Integer>,
        accountName -> Nullable<Text>,
        texture -> Nullable<Integer>,
        backpackSlots -> Nullable<Integer>,
        has3Quickslots -> Nullable<Integer>,
        creationDate -> Nullable<Text>,
        dataset_id -> Nullable<Text>,
    }
}

diesel::table! {
    ClassStats (id) {
        id -> Nullable<Integer>,
        objectType -> Nullable<Integer>,
        BestLevel -> Nullable<Integer>,
        BestBaseFame -> Nullable<Integer>,
        BestTotalFame -> Nullable<Integer>,
    }
}

diesel::table! {
    EamAccount (email) {
        id -> Nullable<Integer>,
        name -> Nullable<Text>,
        email -> Text,
        isSteam -> Bool,
        steamId -> Nullable<Text>,
        password -> Text,
        serverName -> Nullable<Text>,
        performDailyLogin -> Bool,
        state -> Nullable<Text>,
        lastLogin -> Nullable<Text>,
        group -> Nullable<Text>,
        token -> Nullable<Text>,
        extra -> Nullable<Text>,
    }
}

diesel::table! {
    EamGroup (id) {
        id -> Integer,
        name -> Nullable<Text>,
        color -> Nullable<Integer>,
        iconType -> Nullable<Text>,
        icon -> Nullable<Text>,
        padding -> Nullable<Text>,
    }
}


diesel::table! {
    Equipment (id) {
        id -> Nullable<Integer>,
        character_id -> Nullable<Integer>,
        dataset_id -> Nullable<Text>,
        item_type -> Nullable<Integer>,
        slot -> Nullable<Integer>,
    }
}

diesel::table! {
    ItemData (id) {
        id -> Nullable<Integer>,
        #[sql_name = "type"]
        type_ -> Nullable<Integer>,
    }
}

diesel::table! {
    Pet (id) {
        id -> Nullable<Integer>,
        name -> Nullable<Text>,
        #[sql_name = "type"]
        type_ -> Nullable<Integer>,
        instanceId -> Nullable<Integer>,
        rarity -> Nullable<Integer>,
        maxAbilityPower -> Nullable<Integer>,
        skin -> Nullable<Integer>,
        shader -> Nullable<Integer>,
        createdOn -> Nullable<Text>,
        incInv -> Nullable<Integer>,
        inv -> Nullable<Text>,
        ability1 -> Nullable<Text>,
        ability2 -> Nullable<Text>,
        ability3 -> Nullable<Text>,
    }
}

diesel::table! {
    Stats (id) {
        id -> Nullable<Integer>,
        BestCharFame -> Nullable<Integer>,
        TotalFame -> Nullable<Integer>,
        Fame -> Nullable<Integer>,
    }
}

diesel::joinable!(Account -> Stats (stats_id));
diesel::joinable!(Account_Storage -> Account (dataset_id));
diesel::joinable!(Account_Storage -> ItemData (item_id));
diesel::joinable!(Character -> Pet (pet_id));

diesel::allow_tables_to_appear_in_same_query!(
    Account,
    Account_Storage,
    Character,
    ClassStats,
    EamAccount,
    Equipment,
    ItemData,
    Pet,
    Stats,
);
