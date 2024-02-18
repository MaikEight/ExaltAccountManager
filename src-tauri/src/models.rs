use crate::schema;
use diesel::insertable::Insertable;
use diesel::prelude::*;
use diesel_derives::AsChangeset;
use serde::Serialize;
use serde::Deserialize;

// ############################
// #         Account          #
// ############################

#[derive(Queryable, Serialize, Deserialize, Clone)]
pub struct Account {
    pub dataset_id: Option<String>,
    pub credits: Option<i32>,
    pub fortune_token: Option<String>,
    pub unity_campaign_points: Option<String>,
    pub early_game_event_tracker: Option<String>,
    pub account_id: Option<String>,
    pub creation_timestamp: Option<String>,
    pub enchanter_support_dust: Option<String>,
    pub max_num_chars: Option<i32>,
    pub last_server: Option<String>,
    pub originating: Option<String>,
    pub pet_yard_type: Option<i32>,
    pub forge_fire_energy: Option<i32>,
    pub name: Option<String>,
    pub guild: Option<String>,
    pub guildrank: Option<i32>,
    pub access_token_timestamp: Option<String>,
    pub access_token_expiration: Option<String>,
    pub owned_skins: Option<String>,
    pub stats_id: Option<i32>,
}

#[derive(Insertable, Serialize)]
#[diesel(table_name = schema::Account)]
pub struct NewAccount {
    pub dataset_id: Option<String>,
    #[diesel(column_name = "Credits")]
    pub credits: Option<i32>,
    #[diesel(column_name = "FortuneToken")]
    pub fortune_token: Option<String>,
    #[diesel(column_name = "UnityCampaignPoints")]
    pub unity_campaign_points: Option<String>,
    #[diesel(column_name = "EarlyGameEventTracker")]
    pub early_game_event_tracker: Option<String>,
    #[diesel(column_name = "AccountId")]
    pub account_id: Option<String>,
    #[diesel(column_name = "CreationTimestamp")]
    pub creation_timestamp: Option<String>,
    #[diesel(column_name = "EnchanterSupportDust")]
    pub enchanter_support_dust: Option<String>,
    #[diesel(column_name = "MaxNumChars")]
    pub max_num_chars: Option<i32>,
    #[diesel(column_name = "LastServer")]
    pub last_server: Option<String>,
    #[diesel(column_name = "Originating")]
    pub originating: Option<String>,
    #[diesel(column_name = "PetYardType")]
    pub pet_yard_type: Option<i32>,
    #[diesel(column_name = "ForgeFireEnergy")]
    pub forge_fire_energy: Option<i32>,
    #[diesel(column_name = "Name")]
    pub name: Option<String>,
    #[diesel(column_name = "Guild")]
    pub guild: Option<String>,
    #[diesel(column_name = "Guildrank")]
    pub guildrank: Option<i32>,
    #[diesel(column_name = "AccessTokenTimestamp")]
    pub access_token_timestamp: Option<String>,
    #[diesel(column_name = "AccessTokenExpiration")]
    pub access_token_expiration: Option<String>,
    #[diesel(column_name = "OwnedSkins")]
    pub owned_skins: Option<String>,
    #[diesel(column_name = "stats_id")]
    pub stats_id: Option<i32>,
}

#[derive(AsChangeset, Serialize)]
#[diesel(table_name = schema::Account)]
pub struct UpdateAccount {
    pub dataset_id: Option<String>,
    #[diesel(column_name = "Credits")]
    pub credits: Option<i32>,
    #[diesel(column_name = "FortuneToken")]
    pub fortune_token: Option<String>,
    #[diesel(column_name = "UnityCampaignPoints")]
    pub unity_campaign_points: Option<String>,
    #[diesel(column_name = "EarlyGameEventTracker")]
    pub early_game_event_tracker: Option<String>,
    #[diesel(column_name = "AccountId")]
    pub account_id: Option<String>,
    #[diesel(column_name = "CreationTimestamp")]
    pub creation_timestamp: Option<String>,
    #[diesel(column_name = "EnchanterSupportDust")]
    pub enchanter_support_dust: Option<String>,
    #[diesel(column_name = "MaxNumChars")]
    pub max_num_chars: Option<i32>,
    #[diesel(column_name = "LastServer")]
    pub last_server: Option<String>,
    #[diesel(column_name = "Originating")]
    pub originating: Option<String>,
    #[diesel(column_name = "PetYardType")]
    pub pet_yard_type: Option<i32>,
    #[diesel(column_name = "ForgeFireEnergy")]
    pub forge_fire_energy: Option<i32>,
    #[diesel(column_name = "Name")]
    pub name: Option<String>,
    #[diesel(column_name = "Guild")]
    pub guild: Option<String>,
    #[diesel(column_name = "Guildrank")]
    pub guildrank: Option<i32>,
    #[diesel(column_name = "AccessTokenTimestamp")]
    pub access_token_timestamp: Option<String>,
    #[diesel(column_name = "AccessTokenExpiration")]
    pub access_token_expiration: Option<String>,
    #[diesel(column_name = "OwnedSkins")]
    pub owned_skins: Option<String>,
    #[diesel(column_name = "stats_id")]
    pub stats_id: Option<i32>,
}

// ############################
// #      AccountStorage      #
// ############################

#[derive(Queryable, Serialize, Deserialize, Clone)]
pub struct AccountStorage {
    pub id: Option<i32>,
    pub dataset_id: Option<String>,
    pub item_id: Option<i32>,
    pub storage_type: Option<String>,
}

#[derive(Insertable, Serialize)]
#[diesel(table_name = schema::Account_Storage)]
pub struct NewAccountStorage {
    pub dataset_id: Option<String>,
    pub item_id: Option<i32>,
    pub storage_type: Option<String>,
}

#[derive(AsChangeset, Serialize)]
#[diesel(table_name = schema::Account_Storage)]
pub struct UpdateAccountStorage {
    pub dataset_id: Option<String>,
    pub item_id: Option<i32>,
    pub storage_type: Option<String>,
}

// ############################
// #         Character        #
// ############################

#[derive(Queryable, Serialize, Deserialize, Clone)]
pub struct Character {
    pub id: Option<i32>,
    pub object_type: Option<i32>,
    pub seasonal: Option<i32>,
    pub level: Option<i32>,
    pub exp: Option<i32>,
    pub current_fame: Option<i32>,
    pub equip_qs: Option<String>,
    pub max_hit_points: Option<i32>,
    pub hit_points: Option<i32>,
    pub max_magic_points: Option<i32>,
    pub magic_points: Option<i32>,
    pub attack: Option<i32>,
    pub defense: Option<i32>,
    pub speed: Option<i32>,
    pub dexterity: Option<i32>,
    pub hp_regen: Option<i32>,
    pub mp_regen: Option<i32>,
    pub pc_stats: Option<String>,
    pub health_stack_count: Option<i32>,
    pub magic_stack_count: Option<i32>,
    pub dead: Option<i32>,
    pub pet_id: Option<i32>,
    pub account_name: Option<String>,
    pub texture: Option<i32>,
    pub backpack_slots: Option<i32>,
    pub has_3_quickslots: Option<i32>,
    pub creation_date: Option<String>,
    pub dataset_id: Option<String>,
}

#[derive(Insertable, Serialize)]
#[diesel(table_name = schema::Character)]
pub struct NewCharacter {
    #[diesel(column_name = "objectType")]
    pub object_type: Option<i32>,
    pub seasonal: Option<i32>,
    pub level: Option<i32>,
    pub exp: Option<i32>,
    #[diesel(column_name = "currentFame")]
    pub current_fame: Option<i32>,
    #[diesel(column_name = "equipQS")]
    pub equip_qs: Option<String>,
    #[diesel(column_name = "maxHitPoints")]
    pub max_hit_points: Option<i32>,
    #[diesel(column_name = "hitPoints")]
    pub hit_points: Option<i32>,
    #[diesel(column_name = "maxMagicPoints")]
    pub max_magic_points: Option<i32>,
    #[diesel(column_name = "magicPoints")]
    pub magic_points: Option<i32>,
    pub attack: Option<i32>,
    pub defense: Option<i32>,
    pub speed: Option<i32>,
    pub dexterity: Option<i32>,
    #[diesel(column_name = "hpRegen")]
    pub hp_regen: Option<i32>,
    #[diesel(column_name = "mpRegen")]
    pub mp_regen: Option<i32>,
    #[diesel(column_name = "pcStats")]
    pub pc_stats: Option<String>,
    #[diesel(column_name = "healthStackCount")]
    pub health_stack_count: Option<i32>,
    #[diesel(column_name = "magicStackCount")]
    pub magic_stack_count: Option<i32>,
    pub dead: Option<i32>,
    pub pet_id: Option<i32>,
    #[diesel(column_name = "accountName")]
    pub account_name: Option<String>,
    pub texture: Option<i32>,
    #[diesel(column_name = "backpackSlots")]
    pub backpack_slots: Option<i32>,
    #[diesel(column_name = "has3Quickslots")]
    pub has_3_quickslots: Option<i32>,
    #[diesel(column_name = "creationDate")]
    pub creation_date: Option<String>,
    pub dataset_id: Option<String>,
}

#[derive(AsChangeset, Serialize)]
#[diesel(table_name = schema::Character)]
pub struct UpdateCharacter {
    #[diesel(column_name = "objectType")]
    pub object_type: Option<i32>,
    pub seasonal: Option<i32>,
    pub level: Option<i32>,
    pub exp: Option<i32>,
    #[diesel(column_name = "currentFame")]
    pub current_fame: Option<i32>,
    #[diesel(column_name = "equipQS")]
    pub equip_qs: Option<String>,
    #[diesel(column_name = "maxHitPoints")]
    pub max_hit_points: Option<i32>,
    #[diesel(column_name = "hitPoints")]
    pub hit_points: Option<i32>,
    #[diesel(column_name = "maxMagicPoints")]
    pub max_magic_points: Option<i32>,
    #[diesel(column_name = "magicPoints")]
    pub magic_points: Option<i32>,
    pub attack: Option<i32>,
    pub defense: Option<i32>,
    pub speed: Option<i32>,
    pub dexterity: Option<i32>,
    #[diesel(column_name = "hpRegen")]
    pub hp_regen: Option<i32>,
    #[diesel(column_name = "mpRegen")]
    pub mp_regen: Option<i32>,
    #[diesel(column_name = "pcStats")]
    pub pc_stats: Option<String>,
    #[diesel(column_name = "healthStackCount")]
    pub health_stack_count: Option<i32>,
    #[diesel(column_name = "magicStackCount")]
    pub magic_stack_count: Option<i32>,
    pub dead: Option<i32>,
    pub pet_id: Option<i32>,
    #[diesel(column_name = "accountName")]
    pub account_name: Option<String>,
    pub texture: Option<i32>,
    #[diesel(column_name = "backpackSlots")]
    pub backpack_slots: Option<i32>,
    #[diesel(column_name = "has3Quickslots")]
    pub has_3_quickslots: Option<i32>,
    #[diesel(column_name = "creationDate")]
    pub creation_date: Option<String>,
    pub dataset_id: Option<String>,
}

// ############################
// #        ClassStats        #
// ############################

#[derive(Queryable, Serialize, Deserialize, Clone)]
pub struct ClassStats {
    pub id: Option<i32>,
    pub object_type: Option<i32>,
    pub best_level: Option<i32>,
    pub best_base_fame: Option<i32>,
    pub best_total_fame: Option<i32>,
}

#[derive(Insertable, Serialize)]
#[diesel(table_name = schema::ClassStats)]
pub struct NewClassStats {
    #[diesel(column_name = "objectType")]
    pub object_type: Option<i32>,
    #[diesel(column_name = "BestLevel")]
    pub best_level: Option<i32>,
    #[diesel(column_name = "BestBaseFame")]
    pub best_base_fame: Option<i32>,
    #[diesel(column_name = "BestTotalFame")]
    pub best_total_fame: Option<i32>,
}

#[derive(AsChangeset, Serialize)]
#[diesel(table_name = schema::ClassStats)]
pub struct UpdateClassStats {
    #[diesel(column_name = "objectType")]
    pub object_type: Option<i32>,
    #[diesel(column_name = "BestLevel")]
    pub best_level: Option<i32>,
    #[diesel(column_name = "BestBaseFame")]
    pub best_base_fame: Option<i32>,
    #[diesel(column_name = "BestTotalFame")]
    pub best_total_fame: Option<i32>,
}

// ############################
// #        EamAccount        #
// ############################

#[derive(Queryable, Serialize, Deserialize, Clone)]
pub struct EamAccount {
    pub id: Option<i32>,
    pub name: Option<String>,
    pub email: String,
    pub password: String,
    pub server_name: Option<String>,
    pub perform_daily_login: bool,
    pub state: Option<String>,
    pub last_login: Option<String>,
    pub group: Option<String>,
    pub extra: Option<String>,
}

#[derive(Insertable, Serialize)]
#[diesel(table_name = schema::EamAccount)]
pub struct NewEamAccount {
    pub id: Option<i32>,
    pub name: Option<String>,
    pub email: String,
    pub password: String,
    #[diesel(column_name = "serverName")]
    pub server_name: Option<String>,
    #[diesel(column_name = "performDailyLogin")]
    pub perform_daily_login: bool,
    pub state: Option<String>,
    #[diesel(column_name = "lastLogin")]
    pub last_login: Option<String>,
    pub group: Option<String>,
    pub extra: Option<String>,
}

#[derive(AsChangeset, Serialize)]
#[diesel(table_name = schema::EamAccount)]
pub struct UpdateEamAccount {
    pub name: Option<String>,
    pub email: String,
    pub password: String,
    #[diesel(column_name = "serverName")]
    pub server_name: Option<String>,
    #[diesel(column_name = "performDailyLogin")]
    pub perform_daily_login: bool,
    pub state: Option<String>,
    #[diesel(column_name = "lastLogin")]
    pub last_login: Option<String>,
    pub group: Option<String>,
    pub extra: Option<String>,
}

impl From<EamAccount> for NewEamAccount {
    fn from(account: EamAccount) -> Self {
        NewEamAccount {
            id: account.id,
            name: account.name,
            email: account.email,
            password: account.password,
            server_name: account.server_name,
            perform_daily_login: account.perform_daily_login,
            state: account.state,
            last_login: account.last_login,
            group: account.group,
            extra: account.extra,
        }
    }
}

impl From<EamAccount> for UpdateEamAccount {
    fn from(account: EamAccount) -> Self {
        UpdateEamAccount {
            name: account.name,
            email: account.email,
            password: account.password,
            server_name: account.server_name,
            perform_daily_login: account.perform_daily_login,
            state: account.state,
            last_login: account.last_login,
            group: account.group,
            extra: account.extra,
        }
    }
}

// ############################
// #         Equipment        #
// ############################

#[derive(Queryable, Serialize, Deserialize, Clone)]
pub struct Equipment {
    pub id: Option<i32>,
    pub character_id: Option<i32>,
    pub dataset_id: Option<String>,
    pub item_type: Option<i32>,
    pub slot: Option<i32>,
}

#[derive(Insertable, Serialize)]
#[diesel(table_name = schema::Equipment)]
pub struct NewEquipment {
    pub character_id: Option<i32>,
    pub dataset_id: Option<String>,
    pub item_type: Option<i32>,
    pub slot: Option<i32>,
}

#[derive(AsChangeset, Serialize)]
#[diesel(table_name = schema::Equipment)]
pub struct UpdateEquipment {
    pub character_id: Option<i32>,
    pub dataset_id: Option<String>,
    pub item_type: Option<i32>,
    pub slot: Option<i32>,
}

// ############################
// #         ItemData         #
// ############################

#[derive(Queryable, Serialize, Deserialize, Clone)]
pub struct ItemData {
    pub id: Option<i32>,
    pub type_: Option<i32>,
}

#[derive(Insertable, Serialize)]
#[diesel(table_name = schema::ItemData)]
pub struct NewItemData {
    pub type_: Option<i32>,
}

#[derive(AsChangeset, Serialize)]
#[diesel(table_name = schema::ItemData)]
pub struct UpdateItemData {
    pub type_: Option<i32>,
}

// ############################
// #           Pet            #
// ############################

#[derive(Queryable, Serialize, Deserialize, Clone)]
pub struct Pet {
    pub id: Option<i32>,
    pub name: Option<String>,
    pub type_: Option<i32>,
    pub instance_id: Option<i32>,
    pub rarity: Option<i32>,
    pub max_ability_power: Option<i32>,
    pub skin: Option<i32>,
    pub shader: Option<i32>,
    pub created_on: Option<String>,
    pub inc_inv: Option<i32>,
    pub inv: Option<String>,
    pub ability1: Option<String>,
    pub ability2: Option<String>,
    pub ability3: Option<String>,
}

#[derive(Insertable, Serialize)]
#[diesel(table_name = schema::Pet)]
pub struct NewPet {
    pub name: Option<String>,
    #[diesel(column_name = "type_")]
    pub type_: Option<i32>,
    #[diesel(column_name = "instanceId")]
    pub instance_id: Option<i32>,
    pub rarity: Option<i32>,
    #[diesel(column_name = "maxAbilityPower")]
    pub max_ability_power: Option<i32>,
    pub skin: Option<i32>,
    pub shader: Option<i32>,
    #[diesel(column_name = "createdOn")]
    pub created_on: Option<String>,
    #[diesel(column_name = "incInv")]
    pub inc_inv: Option<i32>,
    pub inv: Option<String>,
    pub ability1: Option<String>,
    pub ability2: Option<String>,
    pub ability3: Option<String>,
}

#[derive(AsChangeset, Serialize)]
#[diesel(table_name = schema::Pet)]
pub struct UpdatePet {
    pub name: Option<String>,
    #[diesel(column_name = "type_")]
    pub type_: Option<i32>,
    #[diesel(column_name = "instanceId")]
    pub instance_id: Option<i32>,
    pub rarity: Option<i32>,
    #[diesel(column_name = "maxAbilityPower")]
    pub max_ability_power: Option<i32>,
    pub skin: Option<i32>,
    pub shader: Option<i32>,
    #[diesel(column_name = "createdOn")]
    pub created_on: Option<String>,
    #[diesel(column_name = "incInv")]
    pub inc_inv: Option<i32>,
    pub inv: Option<String>,
    pub ability1: Option<String>,
    pub ability2: Option<String>,
    pub ability3: Option<String>,
}

// ############################
// #           Stats          #
// ############################

#[derive(Queryable, Serialize, Deserialize, Clone)]
pub struct Stats {
    pub id: Option<i32>,
    pub best_char_fame: Option<i32>,
    pub total_fame: Option<i32>,
    pub fame: Option<i32>,
}

#[derive(Insertable, Serialize)]
#[diesel(table_name = schema::Stats)]
pub struct NewStats {
    #[diesel(column_name = "BestCharFame")]
    pub best_char_fame: Option<i32>,
    #[diesel(column_name = "TotalFame")]
    pub total_fame: Option<i32>,
    #[diesel(column_name = "Fame")]
    pub fame: Option<i32>,
}

#[derive(AsChangeset, Serialize)]
#[diesel(table_name = schema::Stats)]
pub struct UpdateStats {
    #[diesel(column_name = "BestCharFame")]
    pub best_char_fame: Option<i32>,
    #[diesel(column_name = "TotalFame")]
    pub total_fame: Option<i32>,
    #[diesel(column_name = "Fame")]
    pub fame: Option<i32>,
}
