use diesel::r2d2::PooledConnection;
use diesel::r2d2::{self, ConnectionManager, CustomizeConnection};
use diesel::sqlite::SqliteConnection;
use diesel::RunQueryDsl;
use diesel_migrations::{embed_migrations, EmbeddedMigrations, MigrationHarness};
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

        // You can keep these pragmas:
        sql_query("PRAGMA journal_mode = WAL;")
            .execute(conn)
            .map_err(diesel::r2d2::Error::QueryError)?;
        sql_query("PRAGMA busy_timeout = 5000;")
            .execute(conn)
            .map_err(diesel::r2d2::Error::QueryError)?;
        sql_query("PRAGMA foreign_keys = ON;")
            .execute(conn)
            .map_err(diesel::r2d2::Error::QueryError)?;
        sql_query("PRAGMA synchronous = NORMAL;")
            .execute(conn)
            .map_err(diesel::r2d2::Error::QueryError)?;

        sql_query("PRAGMA auto_vacuum = INCREMENTAL;")
            .execute(conn)
            .map_err(diesel::r2d2::Error::QueryError)?;

        Ok(())
    }
}

fn freelist_count(conn: &mut SqliteConnection) -> i64 {
    use diesel::dsl::sql;
    use diesel::sql_types::BigInt;
    diesel::select(sql::<BigInt>(
        "(SELECT COALESCE(pragma_freelist_count, 0) FROM (PRAGMA freelist_count))",
    ))
    .get_result::<i64>(conn)
    .unwrap_or(0)
}

fn run_db_maintenance(conn: &mut SqliteConnection) -> Result<(), diesel::result::Error> {
    use diesel::sql_query;

    sql_query("PRAGMA busy_timeout = 15000;").execute(conn)?;

    sql_query("PRAGMA wal_checkpoint(TRUNCATE);").execute(conn)?;

    sql_query("VACUUM;").execute(conn)?;

    sql_query("PRAGMA optimize;").execute(conn)?;

    Ok(())
}
// ------------------------------------------------------------

pub fn setup_database(database_url: &str) -> Result<DbPool, Box<dyn Error + Send + Sync>> {
    std::env::set_var("DATABASE_URL", database_url);
    let manager = ConnectionManager::<SqliteConnection>::new(database_url);
    let pool = r2d2::Pool::builder()
        .max_size(10)
        .connection_timeout(Duration::from_secs(10))
        .connection_customizer(Box::new(SqliteConnectionCustomizer))
        .build(manager)?;

    let mut conn = pool.get()?;

    let (applied_any, applied_len) = {
        let applied = conn.run_pending_migrations(MIGRATIONS)?;
        (!applied.is_empty(), applied.len())
    };

    let free_pages = freelist_count(&mut conn);
    let should_vacuum = applied_any || free_pages >= 2000;

    if should_vacuum {
        run_db_maintenance(&mut conn)?;
        log::info!(
            "SQLite maintenance complete (applied_migrations={}, freelist_count={})",
            applied_len,
            free_pages
        );
    } else {
        log::info!(
            "SQLite maintenance skipped (applied_migrations={}, freelist_count={})",
            applied_len, // <- use the captured length, not `applied.len()`
            free_pages
        );
    }

    Ok(pool)
}

pub fn with_db_retry<F, R>(mut operation: F, max_retries: u32) -> Result<R, diesel::result::Error>
where
    F: FnMut() -> Result<R, diesel::result::Error>,
{
    let mut retries = 0;
    let base_delay_ms = 10;

    loop {
        match operation() {
            Ok(result) => return Ok(result),
            Err(diesel::result::Error::DatabaseError(
                diesel::result::DatabaseErrorKind::Unknown,
                ref info,
            )) if info.message().contains("locked") || info.message().contains("busy") => {
                retries += 1;
                if retries > max_retries {
                    log::error!(
                        "Database operation failed after {} retries due to lock",
                        max_retries
                    );
                    return Err(diesel::result::Error::RollbackTransaction);
                }
                let delay_ms = base_delay_ms * (1 << (retries - 1));
                log::warn!(
                    "Database locked, retrying in {}ms (attempt {}/{})",
                    delay_ms,
                    retries,
                    max_retries
                );
                std::thread::sleep(Duration::from_millis(delay_ms));
            }
            Err(e) => return Err(e),
        }
    }
}
