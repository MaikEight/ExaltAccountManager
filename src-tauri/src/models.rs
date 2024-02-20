#![allow(non_snake_case)]

use crate::schema;
use diesel::insertable::Insertable;
use diesel::prelude::*;
use diesel_derives::AsChangeset;
use serde::Serialize;
use serde::Deserialize;

// ############################
// #    char_list_entries     #
// ############################

#[derive(Queryable, Serialize, Deserialize, Clone)]
pub struct CharListEntries {
    pub id: Option<i32>,
    pub email: Option<String>,
    pub timestamp: Option<String>,
}

#[derive(Insertable, Serialize)]
#[diesel(table_name = schema::char_list_entries)]
pub struct NewCharListEntries {
    pub id: Option<i32>,
    pub email: Option<String>,
    pub timestamp: Option<String>,
}

#[derive(AsChangeset, Serialize)]
#[diesel(table_name = schema::char_list_entries)]
pub struct UpdateCharListEntries {
    pub id: Option<i32>,
    pub email: Option<String>,
    pub timestamp: Option<String>,
}

impl From<CharListEntries> for NewCharListEntries {
    fn from(char_list_entries: CharListEntries) -> Self {
        NewCharListEntries {
            id: char_list_entries.id,
            email: char_list_entries.email,
            timestamp: char_list_entries.timestamp,
        }
    }
}

impl From<CharListEntries> for UpdateCharListEntries {
    fn from(char_list_entries: CharListEntries) -> Self {
        UpdateCharListEntries {
            id: char_list_entries.id,
            email: char_list_entries.email,
            timestamp: char_list_entries.timestamp,
        }
    }
}

// ############################
// #          account         #
// ############################

#[derive(Queryable, Serialize, Deserialize, Clone)]
pub struct Account {
    pub entry_id: Option<i32>,
    pub account_id: Option<String>,
    pub credits: Option<i32>,
    pub fortune_token: Option<i32>,
    pub unity_campaign_points: Option<i32>,
    pub early_game_event_tracker: Option<i32>,
    pub creation_timestamp: Option<String>,
    pub enchanter_support_dust: Option<i32>,
    pub vault: Option<String>,
    pub material_storage: Option<String>,
    pub gifts: Option<String>,
    pub temporary_gifts: Option<String>,
    pub potions: Option<String>,
    pub max_num_chars: Option<i32>,
    pub last_server: Option<String>,
    pub originating: Option<String>,
    pub pet_yard_type: Option<i32>,
    pub forge_fire_energy: Option<i32>,
    pub regular_forge_fire_blueprints: Option<String>,
    pub name: Option<String>,
    pub best_char_fame: Option<i32>,
    pub total_fame: Option<i32>,
    pub fame: Option<i32>,
    pub guild_name: Option<String>,
    pub guild_rank: Option<i32>,
    pub access_token_timestamp: Option<String>,
    pub access_token_expiration: Option<String>,
    pub owned_skins: Option<String>,
}

#[derive(Insertable, Serialize)]
#[diesel(table_name = schema::account)]
pub struct NewAccount {
    pub entry_id: Option<i32>,
    pub account_id: Option<String>,
    pub credits: Option<i32>,
    pub fortune_token: Option<i32>,
    pub unity_campaign_points: Option<i32>,
    pub early_game_event_tracker: Option<i32>,
    pub creation_timestamp: Option<String>,
    pub enchanter_support_dust: Option<i32>,
    pub vault: Option<String>,
    pub material_storage: Option<String>,
    pub gifts: Option<String>,
    pub temporary_gifts: Option<String>,
    pub potions: Option<String>,
    pub max_num_chars: Option<i32>,
    pub last_server: Option<String>,
    pub originating: Option<String>,
    pub pet_yard_type: Option<i32>,
    pub forge_fire_energy: Option<i32>,
    pub regular_forge_fire_blueprints: Option<String>,
    pub name: Option<String>,
    pub best_char_fame: Option<i32>,
    pub total_fame: Option<i32>,
    pub fame: Option<i32>,
    pub guild_name: Option<String>,
    pub guild_rank: Option<i32>,
    pub access_token_timestamp: Option<String>,
    pub access_token_expiration: Option<String>,
    pub owned_skins: Option<String>,
}

#[derive(AsChangeset, Serialize)]
#[diesel(table_name = schema::account)]
pub struct UpdateAccount {
    pub entry_id: Option<i32>,
    pub account_id: Option<String>,
    pub credits: Option<i32>,
    pub fortune_token: Option<i32>,
    pub unity_campaign_points: Option<i32>,
    pub early_game_event_tracker: Option<i32>,
    pub creation_timestamp: Option<String>,
    pub enchanter_support_dust: Option<i32>,
    pub vault: Option<String>,
    pub material_storage: Option<String>,
    pub gifts: Option<String>,
    pub temporary_gifts: Option<String>,
    pub potions: Option<String>,
    pub max_num_chars: Option<i32>,
    pub last_server: Option<String>,
    pub originating: Option<String>,
    pub pet_yard_type: Option<i32>,
    pub forge_fire_energy: Option<i32>,
    pub regular_forge_fire_blueprints: Option<String>,
    pub name: Option<String>,
    pub best_char_fame: Option<i32>,
    pub total_fame: Option<i32>,
    pub fame: Option<i32>,
    pub guild_name: Option<String>,
    pub guild_rank: Option<i32>,
    pub access_token_timestamp: Option<String>,
    pub access_token_expiration: Option<String>,
    pub owned_skins: Option<String>,
}

impl From<Account> for NewAccount {
    fn from(account: Account) -> Self {
        NewAccount {
            entry_id: account.entry_id,
            account_id: account.account_id,
            credits: account.credits,
            fortune_token: account.fortune_token,
            unity_campaign_points: account.unity_campaign_points,
            early_game_event_tracker: account.early_game_event_tracker,
            creation_timestamp: account.creation_timestamp,
            enchanter_support_dust: account.enchanter_support_dust,
            vault: account.vault,
            material_storage: account.material_storage,
            gifts: account.gifts,
            temporary_gifts: account.temporary_gifts,
            potions: account.potions,
            max_num_chars: account.max_num_chars,
            last_server: account.last_server,
            originating: account.originating,
            pet_yard_type: account.pet_yard_type,
            forge_fire_energy: account.forge_fire_energy,
            regular_forge_fire_blueprints: account.regular_forge_fire_blueprints,
            name: account.name,
            best_char_fame: account.best_char_fame,
            total_fame: account.total_fame,
            fame: account.fame,
            guild_name: account.guild_name,
            guild_rank: account.guild_rank,
            access_token_timestamp: account.access_token_timestamp,
            access_token_expiration: account.access_token_expiration,
            owned_skins: account.owned_skins,
        }
    }
}

impl From<Account> for UpdateAccount {
    fn from(account: Account) -> Self {
        UpdateAccount {
            entry_id: account.entry_id,
            account_id: account.account_id,
            credits: account.credits,
            fortune_token: account.fortune_token,
            unity_campaign_points: account.unity_campaign_points,
            early_game_event_tracker: account.early_game_event_tracker,
            creation_timestamp: account.creation_timestamp,
            enchanter_support_dust: account.enchanter_support_dust,
            vault: account.vault,
            material_storage: account.material_storage,
            gifts: account.gifts,
            temporary_gifts: account.temporary_gifts,
            potions: account.potions,
            max_num_chars: account.max_num_chars,
            last_server: account.last_server,
            originating: account.originating,
            pet_yard_type: account.pet_yard_type,
            forge_fire_energy: account.forge_fire_energy,
            regular_forge_fire_blueprints: account.regular_forge_fire_blueprints,
            name: account.name,
            best_char_fame: account.best_char_fame,
            total_fame: account.total_fame,
            fame: account.fame,
            guild_name: account.guild_name,
            guild_rank: account.guild_rank,
            access_token_timestamp: account.access_token_timestamp,
            access_token_expiration: account.access_token_expiration,
            owned_skins: account.owned_skins,
        }
    }
}

// ############################
// #       class_stats        #
// ############################

#[derive(Queryable, Serialize, Deserialize, Clone)]
pub struct ClassStats {
    pub id: Option<i32>,
    pub entry_id: Option<i32>,
    pub account_id: Option<String>,
    pub class_type: Option<String>,    
    pub best_level: Option<i32>,
    pub best_base_fame: Option<i32>,
    pub best_total_fame: Option<i32>,
}

#[derive(Insertable, Serialize)]
#[diesel(table_name = schema::class_stats)]
pub struct NewClassStats {
    pub id: Option<i32>,
    pub entry_id: Option<i32>,
    pub account_id: Option<String>,
    pub class_type: Option<String>,    
    pub best_level: Option<i32>,
    pub best_base_fame: Option<i32>,
    pub best_total_fame: Option<i32>,
}

#[derive(AsChangeset, Serialize)]
#[diesel(table_name = schema::class_stats)]
pub struct UpdateClassStats {
    pub id: Option<i32>,
    pub entry_id: Option<i32>,
    pub account_id: Option<String>,
    pub class_type: Option<String>,    
    pub best_level: Option<i32>,
    pub best_base_fame: Option<i32>,
    pub best_total_fame: Option<i32>,
}

impl From<ClassStats> for NewClassStats {
    fn from(class_stats: ClassStats) -> Self {
        NewClassStats {
            id: class_stats.id,
            entry_id: class_stats.entry_id,
            account_id: class_stats.account_id,
            class_type: class_stats.class_type,
            best_level: class_stats.best_level,
            best_base_fame: class_stats.best_base_fame,
            best_total_fame: class_stats.best_total_fame,
        }
    }
}

impl From<ClassStats> for UpdateClassStats {
    fn from(class_stats: ClassStats) -> Self {
        UpdateClassStats {
            id: class_stats.id,
            entry_id: class_stats.entry_id,
            account_id: class_stats.account_id,
            class_type: class_stats.class_type,
            best_level: class_stats.best_level,
            best_base_fame: class_stats.best_base_fame,
            best_total_fame: class_stats.best_total_fame,
        }
    }
}

// ############################
// #         character        #
// ############################

#[derive(Queryable, Serialize, Deserialize, Clone)]
pub struct Character {
    pub id: Option<i32>,
    pub char_id: Option<i32>,
    pub char_class: Option<i32>,
    pub seasonal: Option<bool>,
    pub level: Option<i32>,
    pub exp: Option<i32>,
    pub current_fame: Option<i32>,
    pub equipment: Option<String>,
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
    pub health_stack_count: Option<i32>,
    pub magic_stack_count: Option<i32>,
    pub dead: Option<bool>,
    pub pet_name: Option<String>,
    pub pet_type: Option<i32>,
    pub pet_instance_id: Option<i32>,
    pub pet_rarity: Option<i32>,
    pub pet_max_ability_power: Option<i32>,
    pub pet_skin: Option<i32>,
    pub pet_shader: Option<i32>,
    pub pet_created_on: Option<String>,
    pub pet_inc_inv: Option<i32>,
    pub pet_inv: Option<String>,
    pub pet_ability1_type: Option<i32>,
    pub pet_ability1_power: Option<i32>,
    pub pet_ability1_points: Option<i32>,
    pub pet_ability2_type: Option<i32>,
    pub pet_ability2_power: Option<i32>,
    pub pet_ability2_points: Option<i32>,
    pub pet_ability3_type: Option<i32>,
    pub pet_ability3_power: Option<i32>,
    pub pet_ability3_points: Option<i32>,
    pub account_name: Option<String>,
    pub backpack_slots: Option<i32>,
    pub has3_quickslots: Option<i32>,
    pub creation_date: Option<String>,
}

#[derive(Insertable, Serialize)]
#[diesel(table_name = schema::character)]
pub struct NewCharacter {
    pub id: Option<i32>,
    pub char_id: Option<i32>,
    pub char_class: Option<i32>,
    pub seasonal: Option<bool>,
    pub level: Option<i32>,
    pub exp: Option<i32>,
    pub current_fame: Option<i32>,
    pub equipment: Option<String>,
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
    pub health_stack_count: Option<i32>,
    pub magic_stack_count: Option<i32>,
    pub dead: Option<bool>,
    pub pet_name: Option<String>,
    pub pet_type: Option<i32>,
    pub pet_instance_id: Option<i32>,
    pub pet_rarity: Option<i32>,
    pub pet_max_ability_power: Option<i32>,
    pub pet_skin: Option<i32>,
    pub pet_shader: Option<i32>,
    pub pet_created_on: Option<String>,
    pub pet_inc_inv: Option<i32>,
    pub pet_inv: Option<String>,
    pub pet_ability1_type: Option<i32>,
    pub pet_ability1_power: Option<i32>,
    pub pet_ability1_points: Option<i32>,
    pub pet_ability2_type: Option<i32>,
    pub pet_ability2_power: Option<i32>,
    pub pet_ability2_points: Option<i32>,
    pub pet_ability3_type: Option<i32>,
    pub pet_ability3_power: Option<i32>,
    pub pet_ability3_points: Option<i32>,
    pub account_name: Option<String>,
    pub backpack_slots: Option<i32>,
    pub has3_quickslots: Option<i32>,
    pub creation_date: Option<String>,
}

#[derive(AsChangeset, Serialize)]
#[diesel(table_name = schema::character)]
pub struct UpdateCharacter {
    pub id: Option<i32>,
    pub char_id: Option<i32>,
    pub char_class: Option<i32>,
    pub seasonal: Option<bool>,
    pub level: Option<i32>,
    pub exp: Option<i32>,
    pub current_fame: Option<i32>,
    pub equipment: Option<String>,
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
    pub health_stack_count: Option<i32>,
    pub magic_stack_count: Option<i32>,
    pub dead: Option<bool>,
    pub pet_name: Option<String>,
    pub pet_type: Option<i32>,
    pub pet_instance_id: Option<i32>,
    pub pet_rarity: Option<i32>,
    pub pet_max_ability_power: Option<i32>,
    pub pet_skin: Option<i32>,
    pub pet_shader: Option<i32>,
    pub pet_created_on: Option<String>,
    pub pet_inc_inv: Option<i32>,
    pub pet_inv: Option<String>,
    pub pet_ability1_type: Option<i32>,
    pub pet_ability1_power: Option<i32>,
    pub pet_ability1_points: Option<i32>,
    pub pet_ability2_type: Option<i32>,
    pub pet_ability2_power: Option<i32>,
    pub pet_ability2_points: Option<i32>,
    pub pet_ability3_type: Option<i32>,
    pub pet_ability3_power: Option<i32>,
    pub pet_ability3_points: Option<i32>,
    pub account_name: Option<String>,
    pub backpack_slots: Option<i32>,
    pub has3_quickslots: Option<i32>,
    pub creation_date: Option<String>,
}

impl From<Character> for NewCharacter {
    fn from(character: Character) -> Self {
        NewCharacter {
            id: character.id,
            char_id: character.char_id,
            char_class: character.char_class,
            seasonal: character.seasonal,
            level: character.level,
            exp: character.exp,
            current_fame: character.current_fame,
            equipment: character.equipment,
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
            health_stack_count: character.health_stack_count,
            magic_stack_count: character.magic_stack_count,
            dead: character.dead,
            pet_name: character.pet_name,
            pet_type: character.pet_type,
            pet_instance_id: character.pet_instance_id,
            pet_rarity: character.pet_rarity,
            pet_max_ability_power: character.pet_max_ability_power,
            pet_skin: character.pet_skin,
            pet_shader: character.pet_shader,
            pet_created_on: character.pet_created_on,
            pet_inc_inv: character.pet_inc_inv,
            pet_inv: character.pet_inv,
            pet_ability1_type: character.pet_ability1_type,
            pet_ability1_power: character.pet_ability1_power,
            pet_ability1_points: character.pet_ability1_points,
            pet_ability2_type: character.pet_ability2_type,
            pet_ability2_power: character.pet_ability2_power,
            pet_ability2_points: character.pet_ability2_points,
            pet_ability3_type: character.pet_ability3_type,
            pet_ability3_power: character.pet_ability3_power,
            pet_ability3_points: character.pet_ability3_points,
            account_name: character.account_name,
            backpack_slots: character.backpack_slots,
            has3_quickslots: character.has3_quickslots,
            creation_date: character.creation_date,
        }
    }
}

impl From<Character> for UpdateCharacter {
    fn from(character: Character) -> Self {
        UpdateCharacter {
            id: character.id,
            char_id: character.char_id,
            char_class: character.char_class,
            seasonal: character.seasonal,
            level: character.level,
            exp: character.exp,
            current_fame: character.current_fame,
            equipment: character.equipment,
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
            health_stack_count: character.health_stack_count,
            magic_stack_count: character.magic_stack_count,
            dead: character.dead,
            pet_name: character.pet_name,
            pet_type: character.pet_type,
            pet_instance_id: character.pet_instance_id,
            pet_rarity: character.pet_rarity,
            pet_max_ability_power: character.pet_max_ability_power,
            pet_skin: character.pet_skin,
            pet_shader: character.pet_shader,
            pet_created_on: character.pet_created_on,
            pet_inc_inv: character.pet_inc_inv,
            pet_inv: character.pet_inv,
            pet_ability1_type: character.pet_ability1_type,
            pet_ability1_power: character.pet_ability1_power,
            pet_ability1_points: character.pet_ability1_points,
            pet_ability2_type: character.pet_ability2_type,
            pet_ability2_power: character.pet_ability2_power,
            pet_ability2_points: character.pet_ability2_points,
            pet_ability3_type: character.pet_ability3_type,
            pet_ability3_power: character.pet_ability3_power,
            pet_ability3_points: character.pet_ability3_points,
            account_name: character.account_name,
            backpack_slots: character.backpack_slots,
            has3_quickslots: character.has3_quickslots,
            creation_date: character.creation_date,
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