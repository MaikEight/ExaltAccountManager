use diesel::sqlite::SqliteConnection;
use diesel::r2d2::{self, ConnectionManager};

pub type DbPool = r2d2::Pool<ConnectionManager<SqliteConnection>>;

pub fn setup_database(database_url: &str) -> DbPool {
    std::env::set_var("DATABASE_URL", database_url);
    let manager = ConnectionManager::<SqliteConnection>::new(database_url);
    let pool = r2d2::Pool::builder()
        .build(manager)
        .expect("Failed to create pool.");

    pool
}