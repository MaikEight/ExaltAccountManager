use diesel::sqlite::SqliteConnection;
use diesel::r2d2::{self, ConnectionManager};
use diesel_migrations::{embed_migrations, EmbeddedMigrations, MigrationHarness};
use diesel::r2d2::{Pool, PooledConnection};
use std::error::Error;

pub const MIGRATIONS: EmbeddedMigrations = embed_migrations!("migrations");

pub type DbPool = r2d2::Pool<ConnectionManager<SqliteConnection>>;
pub type DbConn = PooledConnection<ConnectionManager<SqliteConnection>>;

pub fn setup_database(database_url: &str) -> Result<DbPool, Box<dyn Error + Send + Sync>> {
    std::env::set_var("DATABASE_URL", database_url);
    let manager = ConnectionManager::<SqliteConnection>::new(database_url);
    let pool = r2d2::Pool::builder()
        .build(manager)?;

    let mut conn = pool.get()?;

    conn.run_pending_migrations(MIGRATIONS)?;

    Ok(pool)
}

