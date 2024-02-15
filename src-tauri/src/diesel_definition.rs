use diesel::prelude::*;
use diesel::sqlite::SqliteConnection;
use diesel::r2d2::{self, ConnectionManager};
use diesel_migrations::{embed_migrations, EmbeddedMigrations, MigrationHarness};

pub const MIGRATIONS: EmbeddedMigrations = embed_migrations!("migrations");

type DbPool = r2d2::Pool<ConnectionManager<SqliteConnection>>;

pub fn setup_database(database_url: &str) -> DbPool {
    std::env::set_var("DATABASE_URL", database_url);
    let manager = ConnectionManager::<SqliteConnection>::new(database_url);
    let pool = r2d2::Pool::builder()
        .build(manager)
        .expect("Failed to create pool.");

    let mut conn = pool.get().expect("Failed to get connection from pool.");

    // This will run the necessary migrations.
    conn.run_pending_migrations(MIGRATIONS).expect("Failed to run migrations");

    pool
}

// table! {
//     pet (id) {
//         id -> Integer,
//         name -> Text,
//         #[sql_name = "type"]
//         type_col -> Integer,
//         instanceId -> Integer,
//         rarity -> Integer,
//         maxAbilityPower -> Integer,
//         skin -> Integer,
//         shader -> Integer,
//         createdOn -> Text,
//         incInv -> Integer,
//         inv -> Text,
//         ability1 -> Text,
//         ability2 -> Text,
//         ability3 -> Text,
//     }
// }

// table! {
//     item_data (id) {
//         id -> Integer,
//         #[sql_name = "type"]
//         type_col -> Integer,
//     }
// }

// table! {
//     character (id) {
//         id -> Integer,
//         objectType -> Integer,
//         seasonal -> Integer,
//         level -> Integer,
//         exp -> Integer,
//         currentFame -> Integer,
//         equipQS -> Text,
//         maxHitPoints -> Integer,
//         hitPoints -> Integer,
//         maxMagicPoints -> Integer,
//         magicPoints -> Integer,
//         attack -> Integer,
//         defense -> Integer,
//         speed -> Integer,
//         dexterity -> Integer,
//         hpRegen -> Integer,
//         mpRegen -> Integer,
//         pcStats -> Text,
//         healthStackCount -> Integer,
//         magicStackCount -> Integer,
//         dead -> Integer,
//         pet_id -> Integer,
//         accountName -> Text,
//         texture -> Integer,
//         backpackSlots -> Integer,
//         has3Quickslots -> Integer,
//         creationDate -> Text,
//         dataset_id -> Text,
//     }
// }

// table! {
//     class_stats (id) {
//         id -> Integer,
//         objectType -> Integer,
//         BestLevel -> Integer,
//         BestBaseFame -> Integer,
//         BestTotalFame -> Integer,
//     }
// }

// table! {
//     stats (id) {
//         id -> Integer,
//         BestCharFame -> Integer,
//         TotalFame -> Integer,
//         Fame -> Integer,
//     }
// }

// table! {
//     account (dataset_id) {
//         dataset_id -> Text,
//         Credits -> Integer,
//         FortuneToken -> Text,
//         UnityCampaignPoints -> Text,
//         EarlyGameEventTracker -> Text,
//         accountId -> Text,
//         CreationTimestamp -> Text,
//         EnchanterSupportDust -> Text,
//         MaxNumChars -> Integer,
//         LastServer -> Text,
//         Originating -> Text,
//         PetYardType -> Integer,
//         ForgeFireEnergy -> Integer,
//         Name -> Text,
//         Guild -> Text,
//         Guildrank -> Integer,
//         AccessTokenTimestamp -> Text,
//         AccessTokenExpiration -> Text,
//         OwnedSkins -> Text,
//         stats_id -> Integer,
//     }
// }

// table! {
//     equipment (id) {
//         id -> Integer,
//         character_id -> Integer,
//         dataset_id -> Text,
//         item_type -> Integer,
//         slot -> Integer,
//     }
// }

// table! {
//     account_storage (id) {
//         id -> Integer,
//         dataset_id -> Text,
//         item_id -> Integer,
//         storage_type -> Text,
//     }
// }

// joinable!(character -> pet (pet_id));
// joinable!(account -> stats (stats_id));
// joinable!(equipment -> character (character_id));
// joinable!(account_storage -> account (dataset_id));
// joinable!(account_storage -> item_data (item_id));

// allow_tables_to_appear_in_same_query!(
//     pet,
//     item_data,
//     character,
//     class_stats,
//     stats,
//     account,
//     equipment,
//     account_storage,
// );