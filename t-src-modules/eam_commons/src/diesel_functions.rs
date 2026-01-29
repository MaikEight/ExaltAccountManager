use crate::diesel_setup::{with_db_retry, DbPool};
use crate::models::Account;
use crate::models::Character;
use crate::models::ClassStats;
use crate::models::{ApiRequest, CallResult, NewApiRequest};
use crate::models::{AuditLog, NewAuditLog};
use crate::models::{CharListDataset, CharListEntries, NewCharListEntries};
use crate::models::{
    DailyLoginReportEntries, NewDailyLoginReportEntries, UpdateDailyLoginReportEntries,
};
use crate::models::{DailyLoginReports, NewDailyLoginReports, UpdateDailyLoginReports};
use crate::models::{EamAccount, NewEamAccount, UpdateEamAccount};
use crate::models::{EamGroup, NewEamGroup, UpdateEamGroup};
use crate::models::{ErrorLog, NewErrorLog};
use crate::models::{NewAccount, NewCharacter, NewClassStats};
use crate::models::{NewUserData, UpdateUserData, UserData};
use crate::models::{Server, NewServer};
use crate::models::{ParsedItem, ParsedItemRow, NewParsedItem};
use crate::models::{PcStat, PcStatRow, NewPcStat};
use crate::schema::Account as account;
use crate::schema::ApiRequests as api_requests;
use crate::schema::AuditLog as audit_logs;
use crate::schema::AuditLog::dsl::*;
use crate::schema::Char_list_entries as char_list_entries;
use crate::schema::Character as character;
use crate::schema::Class_stats as class_stats;
use crate::schema::DailyLoginReportEntries as daily_login_report_entries;
use crate::schema::DailyLoginReports as daily_login_reports;
use crate::schema::EamAccount as eam_accounts;
use crate::schema::EamGroup as eam_groups;
use crate::schema::EamGroup::dsl::*;
use crate::schema::ErrorLog as error_logs;
use crate::schema::UserData as user_data;
use crate::schema::Servers as servers;
use crate::schema::ParsedItems as parsed_items;
use crate::schema::PcStats as pc_stats_table;
use chrono::{Local, NaiveDateTime, TimeZone, Utc};
use diesel::dsl::sql;
use diesel::insert_into;
use diesel::prelude::QueryDsl;
use diesel::prelude::*;
use diesel::result::Error::DatabaseError;
use diesel::sql_types::{Nullable, Text};
use diesel::ExpressionMethods;
use diesel::RunQueryDsl;
use function_name::named;
use log::{error, info};
use uuid::Uuid;

macro_rules! log_fn {
    () => {
        log::info!("→ {} @ line {}", function_name!(), line!());
    };
}

//########################
//#       UserData       #
//########################
#[named]
pub fn insert_or_update_user_data(
    pool: &DbPool,
    data: UserData,
) -> Result<usize, diesel::result::Error> {
    log_fn!();

    with_db_retry(
        || {
            let mut conn = pool.get().expect("Failed to get connection from pool.");

            let insertable = NewUserData::from(data.clone());
            let updatable = UpdateUserData::from(data.clone());

            diesel::insert_into(user_data::table)
                .values(&insertable)
                .on_conflict(user_data::dataKey)
                .do_update()
                .set(&updatable)
                .execute(&mut conn)
        },
        5,
    )
}

#[named]
pub fn get_user_data_by_key(
    pool: &DbPool,
    data_key: String,
) -> Result<UserData, diesel::result::Error> {
    log_fn!();

    with_db_retry(
        || {
            let mut conn = pool.get().expect("Failed to get connection from pool.");
            user_data::table.find(data_key.clone()).first(&mut conn)
        },
        5,
    )
}

#[named]
pub fn get_all_user_data(pool: &DbPool) -> Result<Vec<UserData>, diesel::result::Error> {
    log_fn!();

    with_db_retry(
        || {
            let mut conn = pool.get().expect("Failed to get connection from pool.");
            user_data::table.load::<UserData>(&mut conn)
        },
        5,
    )
}

#[named]
pub fn delete_user_data_by_key(
    pool: &DbPool,
    data_key: String,
) -> Result<usize, diesel::result::Error> {
    log_fn!();

    with_db_retry(
        || {
            let mut conn = pool.get().expect("Failed to get connection from pool.");
            diesel::delete(user_data::table.find(data_key.clone())).execute(&mut conn)
        },
        5,
    )
}

//#########################
//#   DailyLoginReports   #
//#########################

#[named]
pub fn get_all_daily_login_reports(
    pool: &DbPool,
) -> Result<Vec<DailyLoginReports>, diesel::result::Error> {
    log_fn!();

    with_db_retry(
        || {
            let mut conn = pool.get().expect("Failed to get connection from pool.");
            daily_login_reports::table
                .order(daily_login_reports::startTime.desc())
                .load::<DailyLoginReports>(&mut conn)
        },
        5,
    )
}

#[named]
pub fn get_daily_login_reports_of_last_days(
    pool: &DbPool,
    days: i64,
) -> Result<Vec<DailyLoginReports>, diesel::result::Error> {
    log_fn!();

    with_db_retry(
        || {
            let mut conn = pool.get().expect("Failed to get connection from pool.");
            let days_ago = chrono::Utc::now().naive_utc()
                - chrono::Duration::try_days(days).expect("Invalid number of days");
            daily_login_reports::table
                .filter(daily_login_reports::startTime.ge(diesel::dsl::sql(&format!(
                    "'{}'",
                    days_ago.format("%Y-%m-%d %H:%M:%S")
                ))))
                .order(daily_login_reports::startTime.desc())
                .load::<DailyLoginReports>(&mut conn)
        },
        5,
    )
}

#[named]
pub fn get_daily_login_report_by_id(
    pool: &DbPool,
    report_id: String,
) -> Result<DailyLoginReports, diesel::result::Error> {
    log_fn!();

    with_db_retry(
        || {
            let mut conn = pool.get().expect("Failed to get connection from pool.");
            daily_login_reports::table
                .find(report_id.clone())
                .first(&mut conn)
        },
        5,
    )
}

#[named]
pub fn get_latest_daily_login(pool: &DbPool) -> Result<DailyLoginReports, diesel::result::Error> {
    log_fn!();

    with_db_retry(
        || {
            let mut conn = pool.get().expect("Failed to get connection from pool.");
            daily_login_reports::table
                .order(daily_login_reports::startTime.desc())
                .first(&mut conn)
        },
        5,
    )
}

#[named]
pub fn insert_or_update_daily_login_report(
    pool: &DbPool,
    report: DailyLoginReports,
) -> Result<usize, diesel::result::Error> {
    log_fn!();

    with_db_retry(
        || {
            let mut conn = pool.get().expect("Failed to get connection from pool.");

            let insertable = NewDailyLoginReports::from(report.clone());
            let updatable = UpdateDailyLoginReports::from(report.clone());

            diesel::insert_into(daily_login_reports::table)
                .values(&insertable)
                .on_conflict(daily_login_reports::id)
                .do_update()
                .set(&updatable)
                .execute(&mut conn)
        },
        5,
    )
}

// #############################
// #  DailyLoginReportEntries  #
// #############################

#[named]
pub fn get_all_daily_login_report_entries(
    pool: &DbPool,
) -> Result<Vec<DailyLoginReportEntries>, diesel::result::Error> {
    log_fn!();

    with_db_retry(
        || {
            let mut conn = pool.get().expect("Failed to get connection from pool.");
            daily_login_report_entries::table.load::<DailyLoginReportEntries>(&mut conn)
        },
        5,
    )
}

#[named]
pub fn get_daily_login_report_entry_by_id(
    pool: &DbPool,
    report_entry_id: Option<i32>,
) -> Result<DailyLoginReportEntries, diesel::result::Error> {
    log_fn!();

    with_db_retry(
        || {
            let mut conn = pool.get().expect("Failed to get connection from pool.");
            report_entry_id
                .map(|report_entry_id| {
                    daily_login_report_entries::table
                        .filter(
                            daily_login_report_entries::id
                                .is_not_null()
                                .and(daily_login_report_entries::id.eq(report_entry_id)),
                        )
                        .first(&mut conn)
                })
                .unwrap_or(Err(diesel::result::Error::NotFound))
        },
        5,
    )
}

#[named]
pub fn get_daily_login_report_entries_by_report_id(
    pool: &DbPool,
    report_id: String,
) -> Result<Vec<DailyLoginReportEntries>, diesel::result::Error> {
    log_fn!();

    with_db_retry(
        || {
            let mut conn = pool.get().expect("Failed to get connection from pool.");
            daily_login_report_entries::table
                .filter(daily_login_report_entries::reportId.eq(report_id.clone()))
                .load::<DailyLoginReportEntries>(&mut conn)
        },
        5,
    )
}

#[named]
pub fn insert_or_update_daily_login_report_entry(
    pool: &DbPool,
    entry: DailyLoginReportEntries,
) -> Result<i32, diesel::result::Error> {
    log_fn!();

    with_db_retry(
        || {
            let mut conn = pool.get().expect("Failed to get connection from pool.");

            let insertable = NewDailyLoginReportEntries::from(entry.clone());
            let updatable = UpdateDailyLoginReportEntries::from(entry.clone());

            let _ = diesel::insert_into(daily_login_report_entries::table)
                .values(&insertable)
                .on_conflict(daily_login_report_entries::id)
                .do_update()
                .set(&updatable)
                .execute(&mut conn);

            //Grab & return the id of the report entry
            let res = daily_login_report_entries::table
                .select(daily_login_report_entries::id)
                .filter(daily_login_report_entries::reportId.eq(entry.reportId.clone()))
                .filter(daily_login_report_entries::accountEmail.eq(entry.accountEmail.clone()))
                .first::<Option<i32>>(&mut conn);

            match res {
                Ok(Some(id_value)) => return Ok(id_value),
                Ok(None) => return Err(diesel::result::Error::NotFound),
                Err(e) => return Err(e),
            }
        },
        5,
    )
}

//########################
//#       CharList       #
//########################

#[named]
pub fn get_all_char_list(pool: &DbPool) -> Result<Vec<CharListEntries>, diesel::result::Error> {
    log_fn!();

    with_db_retry(
        || {
            let mut conn = pool.get().expect("Failed to get connection from pool.");
            char_list_entries::table.load::<CharListEntries>(&mut conn)
        },
        5,
    )
}

#[named]
pub fn get_latest_char_list_for_account(
    email: String,
    pool: &DbPool,
) -> Result<CharListEntries, diesel::result::Error> {
    log_fn!();

    let entry = with_db_retry(
        || {
            let mut conn = pool.get().expect("Failed to get connection from pool.");
            char_list_entries::table
                .filter(char_list_entries::email.eq(&email))
                .order(char_list_entries::timestamp.desc())
                .first::<CharListEntries>(&mut conn)
        },
        5,
    )?;

    Ok(entry)
}

#[named]
pub fn get_latest_char_list_for_each_account(
    pool: &DbPool,
) -> Result<Vec<CharListEntries>, diesel::result::Error> {
    log_fn!();

    // Get all emails (distinct) from the char_list_entries table
    let emails_result: Vec<Option<String>> = with_db_retry(
        || {
            let mut conn = pool.get().expect("Failed to get connection from pool.");
            char_list_entries::table
                .select(char_list_entries::email.nullable())
                .distinct()
                .load::<Option<String>>(&mut conn)
        },
        5,
    )?;

    let allowed_emails = get_all_eam_account_emails(pool)?;

    let emails: Vec<String> = emails_result
        .into_iter()
        .filter_map(|x| x)
        .filter(|x| allowed_emails.contains(x))
        .collect();

    // Sort emails in the same order as allowed_emails
    let email_order: std::collections::HashMap<_, _> = allowed_emails
        .iter()
        .enumerate()
        .map(|(index, email)| (email, index))
        .collect();

    let mut sorted_emails = emails.clone();
    sorted_emails.sort_by_key(|email| email_order.get(email).cloned().unwrap_or(usize::MAX));

    // Get the latest entry for each email
    let mut entries = Vec::new();
    for mail in sorted_emails {
        let entry = with_db_retry(
            || {
                let mut conn = pool.get().expect("Failed to get connection from pool.");
                char_list_entries::table
                    .filter(char_list_entries::email.eq(&mail))
                    .order(char_list_entries::timestamp.desc())
                    .first::<CharListEntries>(&mut conn)
            },
            5,
        )?;
        entries.push(entry);
    }

    Ok(entries)
}

//########################
//#    CharListDataset   #
//########################

#[named]
pub fn get_last_days_char_list_dataset_for_account(
    email: String,
    last_days: i32,
    pool: &DbPool,
) -> Result<Vec<(String, Option<CharListDataset>)>, diesel::result::Error> {
    log_fn!();

    // Calculate date range
    let now = chrono::Utc::now().naive_utc();
    let start_date = now - chrono::Duration::try_days(last_days as i64)
        .expect("Invalid number of days");
    
    // Get all char_list_entries for this email within the date range
    let entries: Vec<CharListEntries> = with_db_retry(
        || {
            let mut conn = pool.get().expect("Failed to get connection from pool.");
            char_list_entries::table
                .filter(char_list_entries::email.eq(&email))
                .filter(char_list_entries::timestamp.ge(diesel::dsl::sql(&format!(
                    "'{}'",
                    start_date.format("%Y-%m-%d %H:%M:%S")
                ))))
                .order(char_list_entries::timestamp.desc())
                .load::<CharListEntries>(&mut conn)
        },
        5,
    )?;
    
    // Group entries by date and keep the latest for each day
    let mut entries_by_date: std::collections::HashMap<String, CharListEntries> = 
        std::collections::HashMap::new();
    
    for entry in entries {
        if let Some(ref timestamp_str) = entry.timestamp {
            // Parse timestamp and extract date part
            // Try parsing with fractional seconds first, then without
            let parsed_timestamp = NaiveDateTime::parse_from_str(timestamp_str, "%Y-%m-%d %H:%M:%S%.f")
                .or_else(|_| NaiveDateTime::parse_from_str(timestamp_str, "%Y-%m-%d %H:%M:%S"));
            
            if let Ok(timestamp) = parsed_timestamp {
                let date_key = timestamp.format("%Y-%m-%d").to_string();
                
                // Keep only the latest entry for each date (entries are ordered desc)
                entries_by_date.entry(date_key).or_insert(entry);
            }
        }
    }
    
    // Build result vector for all requested days
    let mut result = Vec::new();
    
    for day_offset in (0..last_days).rev() {
        let target_date = now - chrono::Duration::try_days(day_offset as i64)
            .expect("Invalid day offset");
        let date_key = target_date.format("%Y-%m-%d").to_string();
        
        if let Some(entry) = entries_by_date.get(&date_key) {
            // Build the dataset for this entry
            match build_char_list_dataset_from_entry(entry.clone(), pool) {
                Ok(dataset) => result.push((date_key, Some(dataset))),
                Err(e) => {
                    log::warn!(
                        "[get_last_days_char_list_dataset] Failed to build dataset for {}: {:?}",
                        date_key, e
                    );
                    result.push((date_key, None));
                }
            }
        } else {
            // No entry for this day
            result.push((date_key, None));
        }
    }
    
    Ok(result)
}

// Helper function to build a CharListDataset from a CharListEntries
fn build_char_list_dataset_from_entry(
    entry: CharListEntries,
    pool: &DbPool,
) -> Result<CharListDataset, diesel::result::Error> {
    let account = with_db_retry(
        || {
            let mut conn = pool.get().expect("Failed to get connection from pool.");
            account::table
                .filter(account::entry_id.eq(&entry.id))
                .first::<Account>(&mut conn)
        },
        5,
    )?;

    let class_stats = with_db_retry(
        || {
            let mut conn = pool.get().expect("Failed to get connection from pool.");
            class_stats::table
                .filter(class_stats::entry_id.eq(entry.id.clone()))
                .load::<ClassStats>(&mut conn)
        },
        5,
    )?;

    let character = with_db_retry(
        || {
            let mut conn = pool.get().expect("Failed to get connection from pool.");
            character::table
                .filter(character::entry_id.eq(entry.clone().id))
                .load::<Character>(&mut conn)
        },
        5,
    )?;

    Ok(CharListDataset {
        email: entry.email.unwrap_or_else(|| "<unknown>".into()),
        account,
        class_stats,
        character,
        items: Vec::new(),    // TODO: Implement item parsing
        pc_stats: Vec::new(), // TODO: Implement pc_stats parsing
    })
}

#[named]
pub fn get_latest_char_list_dataset_for_account(
    email: String,
    pool: &DbPool,
) -> Result<CharListDataset, diesel::result::Error> {
    log_fn!();

    let entry = match get_latest_char_list_for_account(email, pool) {
        Ok(e) => e,
        Err(er) => return Err(er),
    };

    let account = match with_db_retry(
        || {
            let mut conn = pool.get().expect("Failed to get connection from pool.");
            account::table
                .filter(account::entry_id.eq(&entry.id))
                .first::<Account>(&mut conn)
        },
        5,
    ) {
        Ok(acc) => acc,
        Err(diesel::result::Error::NotFound) => {
            log::warn!(
                "[DatasetBuilder] Skipping entry {:?}: Account not found",
                entry.id
            );
            return Err(diesel::result::Error::NotFound);
        }
        Err(e) => return Err(e),
    };

    let class_stats = match with_db_retry(
        || {
            let mut conn = pool.get().expect("Failed to get connection from pool.");
            class_stats::table
                .filter(class_stats::entry_id.eq(entry.id.clone()))
                .load::<ClassStats>(&mut conn)
        },
        5,
    ) {
        Ok(stats) => stats,
        Err(e) => {
            log::warn!(
                "[DatasetBuilder] Skipping entry {:?}: Failed to load class_stats: {:?}",
                entry.id,
                e
            );
            return Err(diesel::result::Error::NotFound);
        }
    };

    let character = match with_db_retry(
        || {
            let mut conn = pool.get().expect("Failed to get connection from pool.");
            character::table
                .filter(character::entry_id.eq(entry.clone().id))
                .load::<Character>(&mut conn)
        },
        5,
    ) {
        Ok(chars) => chars,
        Err(e) => {
            log::warn!(
                "[DatasetBuilder] Skipping entry {:?}: Failed to load characters: {:?}",
                entry.id,
                e
            );
            return Err(diesel::result::Error::NotFound);
        }
    };

    let dataset = CharListDataset {
        email: entry.email.unwrap_or_else(|| "<unknown>".into()),
        account,
        class_stats,
        character,
        items: Vec::new(),    //TODO: Implement item parsing
        pc_stats: Vec::new(), //TODO: Implement pc_stats parsing
    };

    Ok(dataset)
}

#[named]
pub fn get_latest_char_list_dataset_for_each_account(
    pool: &DbPool,
) -> Result<Vec<CharListDataset>, diesel::result::Error> {
    log_fn!();

    let char_list_entries = get_latest_char_list_for_each_account(pool)?;

    let mut datasets = Vec::new();

    for entry in char_list_entries {
        let account = match with_db_retry(
            || {
                let mut conn = pool.get().expect("Failed to get connection from pool.");
                account::table
                    .filter(account::entry_id.eq(&entry.id))
                    .first::<Account>(&mut conn)
            },
            5,
        ) {
            Ok(acc) => acc,
            Err(diesel::result::Error::NotFound) => {
                log::warn!(
                    "[DatasetBuilder] Skipping entry {:?}: Account not found",
                    entry.id
                );
                continue;
            }
            Err(e) => return Err(e),
        };

        let class_stats = match with_db_retry(
            || {
                let mut conn = pool.get().expect("Failed to get connection from pool.");
                class_stats::table
                    .filter(class_stats::entry_id.eq(entry.id.clone()))
                    .load::<ClassStats>(&mut conn)
            },
            5,
        ) {
            Ok(stats) => stats,
            Err(e) => {
                log::warn!(
                    "[DatasetBuilder] Skipping entry {:?}: Failed to load class_stats: {:?}",
                    entry.id,
                    e
                );
                continue;
            }
        };

        let character = match with_db_retry(
            || {
                let mut conn = pool.get().expect("Failed to get connection from pool.");
                character::table
                    .filter(character::entry_id.eq(entry.clone().id))
                    .load::<Character>(&mut conn)
            },
            5,
        ) {
            Ok(chars) => chars,
            Err(e) => {
                log::warn!(
                    "[DatasetBuilder] Skipping entry {:?}: Failed to load characters: {:?}",
                    entry.id,
                    e
                );
                continue;
            }
        };

        // Load items for this entry
        let items = match with_db_retry(|| {
            let mut conn = pool.get().expect("Failed to get connection from pool.");
            parsed_items::table
                .filter(parsed_items::entry_id.eq(entry.clone().id))
                .load::<ParsedItemRow>(&mut conn)
        }, 5) {
            Ok(rows) => rows.into_iter().map(ParsedItem::from).collect(),
            Err(e) => {
                log::warn!(
                    "[DatasetBuilder] Entry {:?}: Failed to load items: {:?}, using empty vec",
                    entry.id,
                    e
                );
                Vec::new()
            }
        };

        // Load pc_stats for this entry
        let pc_stats = match with_db_retry(|| {
            let mut conn = pool.get().expect("Failed to get connection from pool.");
            pc_stats_table::table
                .filter(pc_stats_table::entry_id.eq(entry.clone().id))
                .load::<PcStatRow>(&mut conn)
        }, 5) {
            Ok(rows) => rows.into_iter().map(PcStat::from).collect(),
            Err(e) => {
                log::warn!(
                    "[DatasetBuilder] Entry {:?}: Failed to load pc_stats: {:?}, using empty vec",
                    entry.id,
                    e
                );
                Vec::new()
            }
        };

        datasets.push(CharListDataset {
            email: entry.email.unwrap_or_else(|| "<unknown>".into()),
            account,
            class_stats,
            character,
            items,
            pc_stats,
        });
    }

    Ok(datasets)
}

#[named]
pub fn insert_char_list_dataset(
    pool: &DbPool,
    dataset: CharListDataset,
) -> Result<usize, diesel::result::Error> {
    log_fn!();

    with_db_retry(
        || {
            let mut conn = pool.get().expect("Failed to get connection from pool.");
            let mut entry = CharListEntries::from(dataset.clone());
            let entry_uuid = Uuid::new_v4().to_string();
            let entry_uuid_opt = Some(entry_uuid.clone());
            entry.id = Some(entry_uuid.clone());
            let new_entry = NewCharListEntries::from(entry);

            //char_list_entries
            let result = insert_into(char_list_entries::table)
                .values(&new_entry)
                .execute(&mut conn);
            if result.is_err() {
                return Err(result.unwrap_err());
            }

            //account
            let mut acc = NewAccount::from(dataset.account.clone());
            acc.entry_id = Some(entry_uuid.clone());
            let result = insert_into(account::table).values(acc).execute(&mut conn);
            if result.is_err() {
                return Err(result.unwrap_err());
            }

            //class_stats
            for class_stat in &dataset.class_stats {
                let entry_uuid_opt_clone = entry_uuid_opt.as_ref().map(|s| s.clone());
                insert_into(class_stats::table)
                    .values(NewClassStats::from_class_stats(
                        class_stat,
                        entry_uuid_opt_clone,
                    ))
                    .execute(&mut conn)
                    .expect("Failed to insert class_stats");
            }

        //character
        for chara in &dataset.character {
            let entry_uuid_opt_clone = entry_uuid_opt.clone();
            insert_into(character::table)
                .values(NewCharacter::from_character(
                    chara,
                    Some(entry_uuid_opt_clone.expect("Panick: entry_uuid_opt_clone is none")),
                ))
                .execute(&mut conn)
                .expect("Failed to insert character");
        }

        //items (ParsedItems)
        for item in &dataset.items {
            insert_into(parsed_items::table)
                .values(NewParsedItem::from_parsed_item(item, Some(entry_uuid.clone())))
                .execute(&mut conn)
                .expect("Failed to insert parsed_item");
        }

        //pc_stats
        for pc_stat in &dataset.pc_stats {
            insert_into(pc_stats_table::table)
                .values(NewPcStat::from_pc_stat(pc_stat, Some(entry_uuid.clone())))
                .execute(&mut conn)
                .expect("Failed to insert pc_stat");
        }

            Ok(0)
        },
        5,
    )
}

//########################
//#      EamAccount      #
//########################

#[named]
pub fn get_all_eam_accounts_for_daily_login(
    pool: &DbPool,
) -> Result<Vec<EamAccount>, diesel::result::Error> {
    log_fn!();
    info!("Getting all EAM accounts for daily login...");

    info!("Got connection from pool. Loading accounts...");

    let result = with_db_retry(
        || {
            let mut conn = pool.get().expect("Failed to get connection from pool.");
            eam_accounts::table
                .filter(eam_accounts::isDeleted.eq(false))
                .filter(eam_accounts::performDailyLogin.eq(true))
                .load::<EamAccount>(&mut conn)
        },
        5,
    );

    match &result {
        Ok(accounts) => info!("Loaded {} accounts.", accounts.len()),
        Err(e) => error!("Failed to load accounts: {}", e),
    }

    result
}

#[named]
pub fn get_next_eam_account_for_background_sync(
    pool: &DbPool,
) -> Result<Option<EamAccount>, diesel::result::Error> {
    log_fn!();
    info!("Getting next EAM account for background sync...");

    info!("Got connection from pool. Loading accounts...");

    let max_time = chrono::Utc::now().naive_utc() - chrono::Duration::hours(12);
    /*
        -- SQLite
        SELECT id, orderId, name, email, state, lastRefresh, `group`, isDeleted
        FROM EamAccount
        WHERE (lastRefresh IS NULL OR lastRefresh < datetime('now', '-12 hour'))
        AND (state IS NULL OR state NOT IN ('WrongPassword', 'AccountSuspended'))
        AND isDeleted = 0
        ORDER BY CASE WHEN lastRefresh IS NULL THEN 0 ELSE 1 END, lastRefresh ASC, orderId ASC, id ASC
        LIMIT 1;
    */
    let result = with_db_retry(
        || {
            let mut conn = pool.get().expect("Failed to get connection from pool.");
            eam_accounts::table
                .filter(eam_accounts::isDeleted.eq(false))
                .filter(
                    eam_accounts::lastRefresh
                        .is_null()
                        .or(eam_accounts::lastRefresh
                            .lt(max_time.format("%Y-%m-%d %H:%M:%S%.f").to_string())),
                )
                .filter(
                    eam_accounts::state
                        .is_null()
                        .or(eam_accounts::state.ne_all(&[
                            "WrongPassword",
                            "AccountSuspended",
                            "BGSyncError",
                        ])),
                )
                .order((
                    sql::<diesel::sql_types::Integer>(
                        "CASE WHEN lastRefresh IS NULL THEN 0 ELSE 1 END",
                    ),
                    eam_accounts::lastRefresh.asc(),
                    eam_accounts::orderId.asc(),
                    eam_accounts::id.asc(),
                ))
                .first::<EamAccount>(&mut conn)
                .optional()
        },
        5,
    );

    match &result {
        Ok(Some(account)) => info!("Loaded account: {}", account.email),
        Ok(None) => info!("No accounts found for background sync."),
        Err(e) => error!("Failed to load accounts: {}", e),
    }

    result
}

#[named]
pub fn get_all_eam_accounts(pool: &DbPool) -> Result<Vec<EamAccount>, diesel::result::Error> {
    log_fn!();
    info!("Getting all EAM accounts...");

    info!("Got connection from pool. Loading accounts...");

    let result = with_db_retry(
        || {
            let mut conn = pool.get().expect("Failed to get connection from pool.");
            eam_accounts::table
                .filter(eam_accounts::isDeleted.eq(false))
                .order((eam_accounts::orderId.asc(), eam_accounts::id.asc()))
                .load::<EamAccount>(&mut conn)
        },
        5,
    );

    match &result {
        Ok(accounts) => info!("Loaded {} accounts.", accounts.len()),
        Err(e) => error!("Failed to load accounts: {}", e),
    }

    result
}

#[named]
pub fn get_all_eam_account_emails(pool: &DbPool) -> Result<Vec<String>, diesel::result::Error> {
    log_fn!();
    info!("Getting all EAM account emails...");
    info!("Got connection from pool. Loading emails...");

    let result = with_db_retry(
        || {
            let mut conn = pool.get().expect("Failed to get connection from pool.");
            eam_accounts::table
                .select(eam_accounts::email)
                .filter(eam_accounts::isDeleted.eq(false))
                .order((eam_accounts::orderId.asc(), eam_accounts::id.asc()))
                .load::<String>(&mut conn)
        },
        5,
    );

    match &result {
        Ok(emails) => info!("Loaded {} emails.", emails.len()),
        Err(e) => error!("Failed to load emails: {}", e),
    }

    result
}

#[named]
pub fn get_eam_account_by_email(
    pool: &DbPool,
    account_email: String,
) -> Result<EamAccount, diesel::result::Error> {
    log_fn!();
    info!("Getting EAM account by email: {}", account_email);

    with_db_retry(
        || {
            let mut conn = pool.get().expect("Failed to get connection from pool.");
            eam_accounts::table
                .find(account_email.clone())
                .filter(eam_accounts::isDeleted.eq(false))
                .first(&mut conn)
        },
        5,
    )
}

#[named]
pub fn insert_or_update_eam_account(
    pool: &DbPool,
    eam_account: EamAccount,
) -> Result<usize, diesel::result::Error> {
    log_fn!();

    let new_row_inserted = with_db_retry(
        || {
            let mut conn = pool.get().expect("Failed to get connection from pool.");

            // Get the max id and increment it by 1
            let max_id: Option<Option<i32>> = eam_accounts::table
                .select(diesel::dsl::max(eam_accounts::id))
                .first(&mut conn)
                .optional()?;

            let new_id = max_id.unwrap_or(None).unwrap_or(0) + 1; // If no max id, start at 1

            let mut clone_acc = eam_account.clone();
            clone_acc.id = Some(new_id);
            clone_acc.token = None; // Tokens in the account object are deprecated
            let insertable = NewEamAccount::from(clone_acc.clone());
            let updatable = UpdateEamAccount::from(eam_account.clone());

            match insert_into(eam_accounts::table)
                .values(&insertable)
                .execute(&mut conn)
            {
                Ok(_) => Ok(true),
                Err(DatabaseError(_unique_violation, _)) => {
                    diesel::update(eam_accounts::table)
                        .filter(eam_accounts::email.eq(&insertable.email))
                        .set(&updatable)
                        .execute(&mut conn)?;
                    Ok(false)
                }
                Err(e) => Err(e),
            }
        },
        5,
    )?;

    let clone_acc = eam_account.clone();
    if new_row_inserted {
        insert_audit_log(
            pool,
            AuditLog {
                id: None,
                sender: "insert_or_update_eam_account".to_string(),
                accountEmail: Some(clone_acc.email.clone()),
                message: ("Added a new account: ".to_owned() + &clone_acc.email).to_string(),
                time: "".to_string(),
            },
        )?;
    } else {
        insert_audit_log(
            pool,
            AuditLog {
                id: None,
                sender: "insert_or_update_eam_account".to_string(),
                accountEmail: Some(clone_acc.email.clone()),
                message: ("Updated account: ".to_owned() + &clone_acc.email).to_string(),
                time: "".to_string(),
            },
        )?;
    }

    Ok(new_row_inserted as usize)
}

#[named]
pub fn delete_token_for_all_eam_accounts(pool: &DbPool) -> Result<usize, diesel::result::Error> {
    log_fn!();

    with_db_retry(
        || {
            let mut conn = pool.get().expect("Failed to get connection from pool.");
            diesel::update(eam_accounts::table)
                .set(eam_accounts::token.eq(sql::<Nullable<Text>>("NULL")))
                .execute(&mut conn)
        },
        5,
    )
}

#[named]
pub fn delete_eam_account(
    pool: &DbPool,
    account_email: String,
) -> Result<usize, diesel::result::Error> {
    log_fn!();

    insert_audit_log(
        pool,
        AuditLog {
            id: None,
            sender: "delete_eam_account".to_string(),
            accountEmail: Some(account_email.clone()),
            message: ("Deleting account: ".to_owned() + &account_email).to_string(),
            time: "".to_string(),
        },
    )?;

    with_db_retry(
        || {
            let mut conn = pool.get().expect("Failed to get connection from pool.");
            diesel::update(eam_accounts::table.filter(eam_accounts::email.eq(&account_email)))
                .set((
                    eam_accounts::isDeleted.eq(true),
                    eam_accounts::performDailyLogin.eq(false),
                    eam_accounts::password.eq(""),
                    eam_accounts::group.eq(sql::<Nullable<Text>>("NULL")),
                    eam_accounts::extra.eq(sql::<Nullable<Text>>("NULL")),
                    eam_accounts::token.eq(sql::<Nullable<Text>>("NULL")),
                    eam_accounts::serverName.eq(sql::<Nullable<Text>>("NULL")),
                ))
                .execute(&mut conn)
        },
        5,
    )?;

    Ok(0)
}

//########################
//#       EamGroup       #
//########################

#[named]
pub fn get_all_eam_groups(pool: &DbPool) -> Result<Vec<EamGroup>, diesel::result::Error> {
    log_fn!();

    with_db_retry(
        || {
            let mut conn = pool.get().expect("Failed to get connection from pool.");
            EamGroup.load::<EamGroup>(&mut conn)
        },
        5,
    )
}

#[named]
pub fn insert_or_update_eam_group(
    pool: &DbPool,
    eam_group: EamGroup,
) -> Result<usize, diesel::result::Error> {
    log_fn!();

    with_db_retry(
        || {
            let mut conn = pool.get().expect("Failed to get connection from pool.");

            let insertable: NewEamGroup = NewEamGroup::from(eam_group.clone());
            let updatable: UpdateEamGroup = UpdateEamGroup::from(eam_group.clone());

            diesel::insert_into(eam_groups::table)
                .values(&insertable)
                .on_conflict(eam_groups::id)
                .do_update()
                .set(&updatable)
                .execute(&mut conn)
        },
        5,
    )
}

#[named]
pub fn delete_eam_group(pool: &DbPool, group_id: i32) -> Result<usize, diesel::result::Error> {
    log_fn!();

    with_db_retry(
        || {
            let mut conn = pool.get().expect("Failed to get connection from pool.");
            diesel::delete(eam_groups::table.find(group_id)).execute(&mut conn)
        },
        5,
    )
}

// ############################
// #         AuditLog         #
// ############################

#[named]
pub fn get_all_audit_logs(pool: &DbPool) -> Result<Vec<AuditLog>, diesel::result::Error> {
    log_fn!();

    with_db_retry(
        || {
            let mut conn = pool.get().expect("Failed to get connection from pool.");
            audit_logs::table.load::<AuditLog>(&mut conn)
        },
        5,
    )
}

#[named]
pub fn get_audit_log_for_account(
    pool: &DbPool,
    account_email: String,
) -> Result<Vec<AuditLog>, diesel::result::Error> {
    log_fn!();

    with_db_retry(
        || {
            let mut conn = pool.get().expect("Failed to get connection from pool.");
            audit_logs::table
                .filter(accountEmail.eq(account_email.clone()))
                .load::<AuditLog>(&mut conn)
        },
        5,
    )
}

#[named]
pub fn insert_audit_log(pool: &DbPool, log: AuditLog) -> Result<usize, diesel::result::Error> {
    log_fn!();

    with_db_retry(
        || {
            let mut conn = pool.get().expect("Failed to get connection from pool.");

            let mut insertable: NewAuditLog = NewAuditLog::from(log.clone());
            insertable.time = chrono::Utc::now().naive_utc().to_string();

            insert_into(audit_logs::table)
                .values(&insertable)
                .execute(&mut conn)
        },
        5,
    )
}

// ############################
// #         ErrorLog         #
// ############################

#[named]
pub fn get_all_error_logs(pool: &DbPool) -> Result<Vec<ErrorLog>, diesel::result::Error> {
    log_fn!();

    let logs = with_db_retry(
        || {
            let mut conn = pool.get().expect("Failed to get connection from pool.");
            error_logs::table.load::<ErrorLog>(&mut conn)
        },
        5,
    )?;

    let converted_logs = logs
        .into_iter()
        .map(|mut log| {
            match NaiveDateTime::parse_from_str(&log.time, "%Y-%m-%d %H:%M:%S%.f") {
                Ok(naive_datetime) => {
                    let utc_time = Utc.from_utc_datetime(&naive_datetime);
                    let local_time = utc_time.with_timezone(&Local);
                    log.time = local_time.format("%Y-%m-%d %H:%M:%S").to_string();
                }
                Err(e) => println!("Failed to parse time '{}': {}", log.time, e),
            }
            log
        })
        .collect::<Vec<ErrorLog>>();

    Ok(converted_logs)
}

#[named]
pub fn insert_error_log(pool: &DbPool, log: ErrorLog) -> Result<usize, diesel::result::Error> {
    log_fn!();

    with_db_retry(
        || {
            let mut conn = pool.get().expect("Failed to get connection from pool.");

            let mut insertable: NewErrorLog = NewErrorLog::from(log.clone());
            insertable.time = chrono::Utc::now().naive_utc().to_string();

            insert_into(error_logs::table)
                .values(&insertable)
                .execute(&mut conn)
        },
        5,
    )
}

#[named]
pub fn delete_error_logs_older_than_days(
    pool: &DbPool,
    days: i64,
) -> Result<usize, diesel::result::Error> {
    log_fn!();

    with_db_retry(
        || {
            let mut conn = pool.get().expect("Failed to get connection from pool.");

            let target_date = chrono::Utc::now().naive_utc()
                - chrono::Duration::try_days(days as i64).expect("Failed to create duration");
            let target_date_str = target_date.format("%Y-%m-%d %H:%M:%S").to_string();
            let num_deleted =
                diesel::delete(error_logs::table.filter(error_logs::time.lt(target_date_str)))
                    .execute(&mut conn)?;

            Ok(num_deleted)
        },
        5,
    )
}

// ###############################
// #         ApiRequests         #
// ###############################

#[named]
pub fn insert_api_request(
    pool: &DbPool,
    api_name: &str,
    result: CallResult,
) -> Result<usize, diesel::result::Error> {
    log_fn!();

    with_db_retry(
        || {
            let mut conn = pool.get().expect("Failed to get connection from pool.");
            let request = ApiRequest {
                id: None,
                api_name: api_name.to_string(),
                timestamp: Utc::now().timestamp_millis(),
                result: result.to_string(),
            };
            let insetable = NewApiRequest::from(request);

            insert_into(api_requests::table)
                .values(&insetable)
                .execute(&mut conn)
        },
        5,
    )
}

//Note: This function does not log it's useage, as it is called frequently and is not a user action.
pub fn get_recent_requests(pool: &DbPool, api: &str, since_ts: i64) -> Vec<ApiRequest> {
    let api_clone = api.to_string();

    match with_db_retry(
        || {
            let mut conn = pool.get().expect("Failed to get connection from pool.");

            api_requests::table
                .filter(api_requests::api_name.eq(&api_clone))
                .filter(api_requests::timestamp.ge(since_ts))
                .filter(api_requests::result.ne(CallResult::RateLimited.to_string()))
                .load::<ApiRequest>(&mut conn)
        },
        5,
    ) {
        Ok(data) => data,
        Err(e) => {
            log::error!(
                "Failed to get recent requests for API {} after retries: {:?}",
                api,
                e
            );
            vec![]
        }
    }
}

#[named]
pub fn delete_api_requests_older_than_days(
    pool: &DbPool,
    days: i64,
) -> Result<usize, diesel::result::Error> {
    log_fn!();

    with_db_retry(
        || {
            let mut conn = pool.get().expect("Failed to get connection from pool.");

            let target_date = chrono::Utc::now()
                - chrono::Duration::try_days(days as i64).expect("Failed to create duration");
            let target_timestamp_millis = target_date.timestamp_millis();
            let num_deleted = diesel::delete(
                api_requests::table.filter(api_requests::timestamp.lt(target_timestamp_millis)),
            )
            .execute(&mut conn)?;

            Ok(num_deleted)
        },        
        5,
    )
}

//########################
//#       Servers        #
//########################

#[named]
pub fn get_all_servers(pool: &DbPool) -> Result<Vec<Server>, diesel::result::Error> {
    log_fn!();
    
    with_db_retry(|| {
        let mut conn = pool.get().expect("Failed to get connection from pool.");
        servers::table
            .order(servers::name.asc())
            .load::<Server>(&mut conn)
    }, 5)
}

#[named]
pub fn delete_all_servers(pool: &DbPool) -> Result<usize, diesel::result::Error> {
    log_fn!();
    
    with_db_retry(|| {
        let mut conn = pool.get().expect("Failed to get connection from pool.");
        diesel::delete(servers::table).execute(&mut conn)
    }, 5)
}

#[named]
pub fn insert_servers(pool: &DbPool, servers_to_insert: Vec<Server>) -> Result<usize, diesel::result::Error> {
    log_fn!();
    
    let mut total_inserted = 0;
    
    for server in servers_to_insert {
        let insertable = NewServer::from(server);
        
        let result = with_db_retry(|| {
            let mut conn = pool.get().expect("Failed to get connection from pool.");
            diesel::insert_into(servers::table)
                .values(&insertable)
                .execute(&mut conn)
        }, 5);
        
        match result {
            Ok(count) => total_inserted += count,
            Err(e) => {
                log::warn!("Failed to insert server: {:?}", e);
                // Continue with other servers even if one fails (e.g., duplicate)
            }
        }
    }
    
    Ok(total_inserted)
}
