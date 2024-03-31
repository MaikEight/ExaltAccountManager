pub mod encryption_utils;
pub use encryption_utils::{encrypt_data, decrypt_data};

pub mod models;

pub mod schema;

pub mod diesel_setup;
pub use diesel_setup::{DbPool, setup_database};

pub mod diesel_functions;
pub use diesel_functions::*;

#[cfg(target_os = "windows")]
pub mod windows_specifics;