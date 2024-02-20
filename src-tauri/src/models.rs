#![allow(non_snake_case)]

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

impl From<Account> for NewAccount {
    fn from(account: Account) -> Self {
        NewAccount {
            dataset_id: account.dataset_id,
            credits: account.credits,
            fortune_token: account.fortune_token,
            unity_campaign_points: account.unity_campaign_points,
            early_game_event_tracker: account.early_game_event_tracker,
            account_id: account.account_id,
            creation_timestamp: account.creation_timestamp,
            enchanter_support_dust: account.enchanter_support_dust,
            max_num_chars: account.max_num_chars,
            last_server: account.last_server,
            originating: account.originating,
            pet_yard_type: account.pet_yard_type,
            forge_fire_energy: account.forge_fire_energy,
            name: account.name,
            guild: account.guild,
            guildrank: account.guildrank,
            access_token_timestamp: account.access_token_timestamp,
            access_token_expiration: account.access_token_expiration,
            owned_skins: account.owned_skins,
            stats_id: account.stats_id,
        }
    }
}

impl From<Account> for UpdateAccount {
    fn from(account: Account) -> Self {
        UpdateAccount {
            dataset_id: account.dataset_id,
            credits: account.credits,
            fortune_token: account.fortune_token,
            unity_campaign_points: account.unity_campaign_points,
            early_game_event_tracker: account.early_game_event_tracker,
            account_id: account.account_id,
            creation_timestamp: account.creation_timestamp,
            enchanter_support_dust: account.enchanter_support_dust,
            max_num_chars: account.max_num_chars,
            last_server: account.last_server,
            originating: account.originating,
            pet_yard_type: account.pet_yard_type,
            forge_fire_energy: account.forge_fire_energy,
            name: account.name,
            guild: account.guild,
            guildrank: account.guildrank,
            access_token_timestamp: account.access_token_timestamp,
            access_token_expiration: account.access_token_expiration,
            owned_skins: account.owned_skins,
            stats_id: account.stats_id,
        }
    }
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

impl From<AccountStorage> for NewAccountStorage {
    fn from(account_storage: AccountStorage) -> Self {
        NewAccountStorage {
            dataset_id: account_storage.dataset_id,
            item_id: account_storage.item_id,
            storage_type: account_storage.storage_type,
        }
    }
}

impl From<AccountStorage> for UpdateAccountStorage {
    fn from(account_storage: AccountStorage) -> Self {
        UpdateAccountStorage {
            dataset_id: account_storage.dataset_id,
            item_id: account_storage.item_id,
            storage_type: account_storage.storage_type,
        }
    }
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

impl From<Character> for NewCharacter {
    fn from(character: Character) -> Self {
        NewCharacter {
            object_type: character.object_type,
            seasonal: character.seasonal,
            level: character.level,
            exp: character.exp,
            current_fame: character.current_fame,
            equip_qs: character.equip_qs,
            max_hit_points: character.max_hit_points,
            hit_points: character.hit_points,
            max_magic_points: character.max_magic_points,
            magic_points: character.magic_points,
            attack: character.attack,
            defense: character.defense,
            speed: character.speed,
            dexterity: character.dexterity,
            hp_regen: character.hp_regen,
            mp_regen: character.mp_regen,
            pc_stats: character.pc_stats,
            health_stack_count: character.health_stack_count,
            magic_stack_count: character.magic_stack_count,
            dead: character.dead,
            pet_id: character.pet_id,
            account_name: character.account_name,
            texture: character.texture,
            backpack_slots: character.backpack_slots,
            has_3_quickslots: character.has_3_quickslots,
            creation_date: character.creation_date,
            dataset_id: character.dataset_id,
        }
    }
}

impl From<Character> for UpdateCharacter {
    fn from(character: Character) -> Self {
        UpdateCharacter {
            object_type: character.object_type,
            seasonal: character.seasonal,
            level: character.level,
            exp: character.exp,
            current_fame: character.current_fame,
            equip_qs: character.equip_qs,
            max_hit_points: character.max_hit_points,
            hit_points: character.hit_points,
            max_magic_points: character.max_magic_points,
            magic_points: character.magic_points,
            attack: character.attack,
            defense: character.defense,
            speed: character.speed,
            dexterity: character.dexterity,
            hp_regen: character.hp_regen,
            mp_regen: character.mp_regen,
            pc_stats: character.pc_stats,
            health_stack_count: character.health_stack_count,
            magic_stack_count: character.magic_stack_count,
            dead: character.dead,
            pet_id: character.pet_id,
            account_name: character.account_name,
            texture: character.texture,
            backpack_slots: character.backpack_slots,
            has_3_quickslots: character.has_3_quickslots,
            creation_date: character.creation_date,
            dataset_id: character.dataset_id,
        }
    }
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

impl From<ClassStats> for NewClassStats {
    fn from(class_stats: ClassStats) -> Self {
        NewClassStats {
            object_type: class_stats.object_type,
            best_level: class_stats.best_level,
            best_base_fame: class_stats.best_base_fame,
            best_total_fame: class_stats.best_total_fame,
        }
    }
}

impl From<ClassStats> for UpdateClassStats {
    fn from(class_stats: ClassStats) -> Self {
        UpdateClassStats {
            object_type: class_stats.object_type,
            best_level: class_stats.best_level,
            best_base_fame: class_stats.best_base_fame,
            best_total_fame: class_stats.best_total_fame,
        }
    }
}

// ############################
// #        EamAccount        #
// ############################

#[derive(Queryable, Serialize, Deserialize, Clone)]
pub struct EamAccount {
    pub id: Option<i32>,
    pub name: Option<String>,
    pub email: String,
    pub isSteam: bool,
    pub steamId: Option<String>,
    pub password: String,
    #[serde(rename = "serverName")]
    pub server_name: Option<String>,
    #[serde(rename = "performDailyLogin")]
    pub perform_daily_login: bool,
    pub state: Option<String>,
    #[serde(rename = "lastLogin")]
    pub last_login: Option<String>,
    pub group: Option<String>,
    pub token: Option<String>,
    pub extra: Option<String>,
}

#[derive(Insertable, Serialize)]
#[diesel(table_name = schema::EamAccount)]
pub struct NewEamAccount {
    pub id: Option<i32>,
    pub name: Option<String>,
    pub email: String,
    pub isSteam: bool,
    pub steamId: Option<String>,
    pub password: String,
    #[diesel(column_name = "serverName")]
    pub server_name: Option<String>,
    #[diesel(column_name = "performDailyLogin")]
    pub perform_daily_login: bool,
    pub state: Option<String>,
    #[diesel(column_name = "lastLogin")]
    pub last_login: Option<String>,
    pub group: Option<String>,
    pub token: Option<String>,
    pub extra: Option<String>,
}

#[derive(AsChangeset, Serialize)]
#[diesel(table_name = schema::EamAccount)]
pub struct UpdateEamAccount {
    pub id: Option<i32>,
    pub name: Option<String>,
    pub email: String,
    pub isSteam: bool,
    pub steamId: Option<String>,
    pub password: String,
    #[diesel(column_name = "serverName")]
    pub server_name: Option<String>,
    #[diesel(column_name = "performDailyLogin")]
    pub perform_daily_login: bool,
    pub state: Option<String>,
    #[diesel(column_name = "lastLogin")]
    pub last_login: Option<String>,
    pub group: Option<String>,
    pub token: Option<String>,
    pub extra: Option<String>,
}

impl From<EamAccount> for NewEamAccount {
    fn from(account: EamAccount) -> Self {
        NewEamAccount {
            id: account.id,
            name: account.name,
            email: account.email,
            isSteam: account.isSteam,
            steamId: account.steamId,
            password: account.password,
            server_name: account.server_name,
            perform_daily_login: account.perform_daily_login,
            state: account.state,
            last_login: account.last_login,
            group: account.group,
            token: account.token,
            extra: account.extra,
        }
    }
}

impl From<EamAccount> for UpdateEamAccount {
    fn from(account: EamAccount) -> Self {
        UpdateEamAccount {
            id: account.id,
            name: account.name,
            email: account.email,
            isSteam: account.isSteam,
            steamId: account.steamId,
            password: account.password,
            server_name: account.server_name,
            perform_daily_login: account.perform_daily_login,
            state: account.state,
            last_login: account.last_login,
            group: account.group,
            token: account.token,
            extra: account.extra,
        }
    }
}

// ############################
// #         EamGroup         #
// ############################

#[derive(Queryable, Serialize, Deserialize, Clone)]
pub struct EamGroup {
    pub id: i32,
    pub name: Option<String>,
    pub color: Option<i32>,
    #[serde(rename = "iconType")]
    pub icon_type: Option<String>,
    pub icon: Option<String>,
    pub padding: Option<String>,
}

#[derive(Insertable, Serialize)]
#[diesel(table_name = schema::EamGroup)]
pub struct NewEamGroup {
    pub name: Option<String>,
    pub color: Option<i32>,
    #[diesel(column_name = "iconType")]
    pub icon_type: Option<String>,
    pub icon: Option<String>,
    pub padding: Option<String>,
}

#[derive(AsChangeset, Serialize)]
#[diesel(table_name = schema::EamGroup)]
pub struct UpdateEamGroup {    
    pub id: Option<i32>,
    pub name: Option<String>,
    pub color: Option<i32>,
    #[diesel(column_name = "iconType")]
    pub icon_type: Option<String>,
    pub icon: Option<String>,
    pub padding: Option<String>,
}

impl From<EamGroup> for NewEamGroup {
    fn from(group: EamGroup) -> Self {
        NewEamGroup {
            name: group.name,
            color: group.color,
            icon_type: group.icon_type,
            icon: group.icon,
            padding: group.padding,
        }
    }
}

impl From<EamGroup> for UpdateEamGroup {
    fn from(group: EamGroup) -> Self {
        UpdateEamGroup {
            id: Some(group.id),
            name: group.name,
            color: group.color,
            icon_type: group.icon_type,
            icon: group.icon,
            padding: group.padding,
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

impl From<Equipment> for NewEquipment {
    fn from(equipment: Equipment) -> Self {
        NewEquipment {
            character_id: equipment.character_id,
            dataset_id: equipment.dataset_id,
            item_type: equipment.item_type,
            slot: equipment.slot,
        }
    }
}

impl From<Equipment> for UpdateEquipment {
    fn from(equipment: Equipment) -> Self {
        UpdateEquipment {
            character_id: equipment.character_id,
            dataset_id: equipment.dataset_id,
            item_type: equipment.item_type,
            slot: equipment.slot,
        }
    }
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

impl From<ItemData> for NewItemData {
    fn from(item_data: ItemData) -> Self {
        NewItemData {
            type_: item_data.type_,
        }
    }
}

impl From<ItemData> for UpdateItemData {
    fn from(item_data: ItemData) -> Self {
        UpdateItemData {
            type_: item_data.type_,
        }
    }
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

impl From<Pet> for NewPet {
    fn from(pet: Pet) -> Self {
        NewPet {
            name: pet.name,
            type_: pet.type_,
            instance_id: pet.instance_id,
            rarity: pet.rarity,
            max_ability_power: pet.max_ability_power,
            skin: pet.skin,
            shader: pet.shader,
            created_on: pet.created_on,
            inc_inv: pet.inc_inv,
            inv: pet.inv,
            ability1: pet.ability1,
            ability2: pet.ability2,
            ability3: pet.ability3,
        }
    }
}

impl From<Pet> for UpdatePet {
    fn from(pet: Pet) -> Self {
        UpdatePet {
            name: pet.name,
            type_: pet.type_,
            instance_id: pet.instance_id,
            rarity: pet.rarity,
            max_ability_power: pet.max_ability_power,
            skin: pet.skin,
            shader: pet.shader,
            created_on: pet.created_on,
            inc_inv: pet.inc_inv,
            inv: pet.inv,
            ability1: pet.ability1,
            ability2: pet.ability2,
            ability3: pet.ability3,
        }
    }
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

impl From<Stats> for NewStats {
    fn from(stats: Stats) -> Self {
        NewStats {
            best_char_fame: stats.best_char_fame,
            total_fame: stats.total_fame,
            fame: stats.fame,
        }
    }
}

impl From<Stats> for UpdateStats {
    fn from(stats: Stats) -> Self {
        UpdateStats {
            best_char_fame: stats.best_char_fame,
            total_fame: stats.total_fame,
            fame: stats.fame,
        }
    }
}