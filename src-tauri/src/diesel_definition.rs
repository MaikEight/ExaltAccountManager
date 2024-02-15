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

    conn.run_pending_migrations(MIGRATIONS).expect("Failed to run migrations");

    pool
}