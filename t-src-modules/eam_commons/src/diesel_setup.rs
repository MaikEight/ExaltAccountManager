use diesel::sqlite::SqliteConnection;
use diesel::r2d2::{self, ConnectionManager, CustomizeConnection};
use diesel_migrations::{embed_migrations, EmbeddedMigrations, MigrationHarness};
use diesel::r2d2::PooledConnection;
use diesel::RunQueryDsl;
use std::error::Error;
use std::time::Duration;

pub const MIGRATIONS: EmbeddedMigrations = embed_migrations!("migrations");

pub type DbPool = r2d2::Pool<ConnectionManager<SqliteConnection>>;
pub type DbConn = PooledConnection<ConnectionManager<SqliteConnection>>;

#[derive(Debug)]
struct SqliteConnectionCustomizer;

impl CustomizeConnection<SqliteConnection, diesel::r2d2::Error> for SqliteConnectionCustomizer {
    fn on_acquire(&self, conn: &mut SqliteConnection) -> Result<(), diesel::r2d2::Error> {
        use diesel::sql_query;
        
        // Enable WAL mode for better concurrency
        sql_query("PRAGMA journal_mode = WAL;")
            .execute(conn)
            .map_err(diesel::r2d2::Error::QueryError)?;
            
        // Set busy timeout to 5 seconds (reasonable for local SQLite)
        sql_query("PRAGMA busy_timeout = 5000;")
            .execute(conn)
            .map_err(diesel::r2d2::Error::QueryError)?;
            
        // Enable foreign keys
        sql_query("PRAGMA foreign_keys = ON;")
            .execute(conn)
            .map_err(diesel::r2d2::Error::QueryError)?;
            
        // Optimize synchronization for better performance
        sql_query("PRAGMA synchronous = NORMAL;")
            .execute(conn)
            .map_err(diesel::r2d2::Error::QueryError)?;
            
        Ok(())
    }
}

pub fn setup_database(database_url: &str) -> Result<DbPool, Box<dyn Error + Send + Sync>> {
    std::env::set_var("DATABASE_URL", database_url);
    let manager = ConnectionManager::<SqliteConnection>::new(database_url);
    let pool = r2d2::Pool::builder()
        .max_size(10) // Limit pool size to avoid too many connections
        .connection_timeout(Duration::from_secs(10)) // 10 seconds for connection timeout
        .connection_customizer(Box::new(SqliteConnectionCustomizer))
        .build(manager)?;

    let mut conn = pool.get()?;

    conn.run_pending_migrations(MIGRATIONS)?;

    Ok(pool)
}

/// Execute a database operation with retry logic for handling database locks
/// This function will retry up to `max_retries` times with exponential backoff
pub fn with_db_retry<F, R>(mut operation: F, max_retries: u32) -> Result<R, diesel::result::Error>
where
    F: FnMut() -> Result<R, diesel::result::Error>,
{
    let mut retries = 0;
    let base_delay_ms = 10;
    
    loop {
        match operation() {
            Ok(result) => return Ok(result),
            Err(diesel::result::Error::DatabaseError(diesel::result::DatabaseErrorKind::Unknown, ref info))
                if info.message().contains("locked") || info.message().contains("busy") => {
                
                retries += 1;
                if retries > max_retries {
                    log::error!("Database operation failed after {} retries due to lock", max_retries);
                    return Err(diesel::result::Error::RollbackTransaction);
                }
                
                // Exponential backoff: 10ms, 20ms, 40ms, 80ms, etc.
                let delay_ms = base_delay_ms * (1 << (retries - 1));
                log::warn!("Database locked, retrying in {}ms (attempt {}/{})", delay_ms, retries, max_retries);
                std::thread::sleep(Duration::from_millis(delay_ms));
            }
            Err(e) => return Err(e),
        }
    }
}

