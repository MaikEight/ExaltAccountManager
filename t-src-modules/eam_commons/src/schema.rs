#![allow(non_snake_case)]
// @generated automatically by Diesel CLI.

diesel::table! {
    EamAccount (email) {
        id -> Nullable<Integer>,
        orderId -> Nullable<Integer>,
        name -> Nullable<Text>,
        email -> Text,
        isDeleted -> Bool,
        isSteam -> Bool,
        steamId -> Nullable<Text>,
        password -> Text,
        serverName -> Nullable<Text>,
        performDailyLogin -> Bool,
        state -> Nullable<Text>,
        lastLogin -> Nullable<Text>,
        lastRefresh -> Nullable<Text>,
        group -> Nullable<Text>,
        comment -> Nullable<Text>,
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
    Account (entry_id) {
        entry_id -> Nullable<Text>,
        account_id -> Nullable<Text>,
        credits -> Nullable<Integer>,
        fortune_token -> Nullable<Integer>,
        unity_campaign_points -> Nullable<Integer>,
        early_game_event_tracker -> Nullable<Integer>,
        creation_timestamp -> Nullable<Text>,
        enchanter_support_dust -> Nullable<Integer>,
        vault -> Nullable<Text>,
        material_storage -> Nullable<Text>,
        gifts -> Nullable<Text>,
        temporary_gifts -> Nullable<Text>,
        potions -> Nullable<Text>,
        max_num_chars -> Nullable<Integer>,
        last_server -> Nullable<Text>,
        originating -> Nullable<Text>,
        pet_yard_type -> Nullable<Integer>,
        forge_fire_energy -> Nullable<Integer>,
        regular_forge_fire_blueprints -> Nullable<Text>,
        name -> Nullable<Text>,
        best_char_fame -> Nullable<Integer>,
        total_fame -> Nullable<Integer>,
        fame -> Nullable<Integer>,
        guild_name -> Nullable<Text>,
        guild_rank -> Nullable<Integer>,
        access_token_timestamp -> Nullable<Text>,
        access_token_expiration -> Nullable<Text>,
        owned_skins -> Nullable<Text>,
    }
}

diesel::table! {
    Char_list_entries (id) {
        id -> Nullable<Text>,
        email -> Nullable<Text>,
        timestamp -> Nullable<Timestamp>,
    }
}

diesel::table! {
    Character (id) {
        id -> Nullable<Integer>,
        entry_id -> Nullable<Text>,
        char_id -> Nullable<Integer>,
        char_class -> Nullable<Integer>,
        seasonal -> Nullable<Bool>,
        level -> Nullable<Integer>,
        exp -> Nullable<Integer>,
        current_fame -> Nullable<Integer>,
        equipment -> Nullable<Text>,
        equip_qs -> Nullable<Text>,
        max_hit_points -> Nullable<Integer>,
        hit_points -> Nullable<Integer>,
        max_magic_points -> Nullable<Integer>,
        magic_points -> Nullable<Integer>,
        attack -> Nullable<Integer>,
        defense -> Nullable<Integer>,
        speed -> Nullable<Integer>,
        dexterity -> Nullable<Integer>,
        hp_regen -> Nullable<Integer>,
        mp_regen -> Nullable<Integer>,
        health_stack_count -> Nullable<Integer>,
        magic_stack_count -> Nullable<Integer>,
        dead -> Nullable<Bool>,
        pet_name -> Nullable<Text>,
        pet_type -> Nullable<Integer>,
        pet_instance_id -> Nullable<Integer>,
        pet_rarity -> Nullable<Integer>,
        pet_max_ability_power -> Nullable<Integer>,
        pet_skin -> Nullable<Integer>,
        pet_shader -> Nullable<Integer>,
        pet_created_on -> Nullable<Text>,
        pet_inc_inv -> Nullable<Integer>,
        pet_inv -> Nullable<Text>,
        pet_ability1_type -> Nullable<Integer>,
        pet_ability1_power -> Nullable<Integer>,
        pet_ability1_points -> Nullable<Integer>,
        pet_ability2_type -> Nullable<Integer>,
        pet_ability2_power -> Nullable<Integer>,
        pet_ability2_points -> Nullable<Integer>,
        pet_ability3_type -> Nullable<Integer>,
        pet_ability3_power -> Nullable<Integer>,
        pet_ability3_points -> Nullable<Integer>,
        account_name -> Nullable<Text>,
        backpack_slots -> Nullable<Integer>,
        has3_quickslots -> Nullable<Integer>,
        creation_date -> Nullable<Text>,
        pc_stats -> Nullable<Text>,
        tex1 -> Nullable<Text>,
        tex2 -> Nullable<Text>,
        texture -> Nullable<Text>,
        xp_boosted -> Nullable<Integer>,
        xp_timer -> Nullable<Integer>,
        ld_timer -> Nullable<Integer>,
        lt_timer -> Nullable<Integer>,
        crucible_active -> Nullable<Text>,
    }
}

diesel::table! {
    Class_stats (id) {
        id -> Nullable<Integer>,
        entry_id -> Nullable<Text>,
        account_id -> Nullable<Text>,
        class_type -> Nullable<Integer>,
        best_level -> Nullable<Integer>,
        best_base_fame -> Nullable<Integer>,
        best_total_fame -> Nullable<Integer>,
    }
}

diesel::table! {
    AuditLog (id) {
        id -> Nullable<Integer>,
        time -> Timestamp,
        sender -> Text,
        message -> Text,
        accountEmail -> Nullable<Text>,
    }
}

diesel::table! {
    ErrorLog (id) {
        id -> Nullable<Integer>,
        time -> Timestamp,
        sender -> Text,
        message -> Text,
    }
}

diesel::table! {
    UserData (dataKey) {
        dataKey -> Text,
        dataValue -> Text,
    }
}

diesel::table! {
    DailyLoginReportEntries (id) {
        id -> Nullable<Integer>,
        reportId -> Nullable<Text>,
        startTime -> Nullable<Timestamp>,
        endTime -> Nullable<Timestamp>,
        accountEmail -> Nullable<Text>,
        status -> Text,
        errorMessage -> Nullable<Text>,
    }
}

diesel::table! {
    DailyLoginReports (id) {
        id -> Text,
        startTime -> Nullable<Timestamp>,
        endTime -> Nullable<Timestamp>,
        hasFinished -> Bool,
        emailsToProcess -> Nullable<Text>,
        amountOfAccounts -> Integer,
        amountOfAccountsProcessed -> Integer,
        amountOfAccountsFailed -> Integer,
        amountOfAccountsSucceeded -> Integer,
    }
}

diesel::joinable!(Account -> Char_list_entries (entry_id));
diesel::joinable!(Class_stats -> Account (entry_id));
diesel::joinable!(AuditLog -> EamAccount (accountEmail));
diesel::joinable!(DailyLoginReportEntries -> DailyLoginReports (reportId));
diesel::joinable!(DailyLoginReportEntries -> EamAccount (accountEmail));

diesel::allow_tables_to_appear_in_same_query!(
    EamAccount,
    EamGroup,
    Account,
    Char_list_entries,
    Character,
    Class_stats,
    AuditLog,
    ErrorLog,
    DailyLoginReportEntries,
    DailyLoginReports,
);
