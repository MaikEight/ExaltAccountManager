use crate::diesel_setup::DbPool;
use crate::models::Account;
use crate::models::Character;
use crate::models::ClassStats;
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
use crate::schema::Account as account;
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
use chrono::{Local, NaiveDateTime, TimeZone, Utc};
use diesel::dsl::sql;
use diesel::insert_into;
use diesel::prelude::QueryDsl;
use diesel::prelude::*;
use diesel::result::Error::DatabaseError;
use diesel::sql_types::{Nullable, Text};
use diesel::ExpressionMethods;
use diesel::RunQueryDsl;
use log::{error, info};
use uuid::Uuid;

//########################
//#       UserData       #
//########################
pub fn insert_or_update_user_data(
    pool: &DbPool,
    data: UserData,
) -> Result<usize, diesel::result::Error> {
    let mut conn = pool.get().expect("Failed to get connection from pool.");

    let insertable = NewUserData::from(data.clone());

    let updatable = UpdateUserData::from(data);

    diesel::insert_into(user_data::table)
        .values(&insertable)
        .on_conflict(user_data::dataKey)
        .do_update()
        .set(&updatable)
        .execute(&mut conn)
}

pub fn get_user_data_by_key(
    pool: &DbPool,
    data_key: String,
) -> Result<UserData, diesel::result::Error> {
    let mut conn = pool.get().expect("Failed to get connection from pool.");
    user_data::table.find(data_key).first(&mut conn)
}

pub fn get_all_user_data(pool: &DbPool) -> Result<Vec<UserData>, diesel::result::Error> {
    let mut conn = pool.get().expect("Failed to get connection from pool.");
    user_data::table.load::<UserData>(&mut conn)
}

pub fn delete_user_data_by_key(
    pool: &DbPool,
    data_key: String,
) -> Result<usize, diesel::result::Error> {
    let mut conn = pool.get().expect("Failed to get connection from pool.");
    diesel::delete(user_data::table.find(data_key)).execute(&mut conn)
}

//#########################
//#   DailyLoginReports   #
//#########################

pub fn get_all_daily_login_reports(
    pool: &DbPool,
) -> Result<Vec<DailyLoginReports>, diesel::result::Error> {
    let mut conn = pool.get().expect("Failed to get connection from pool.");
    daily_login_reports::table
        .order(daily_login_reports::startTime.desc())
        .load::<DailyLoginReports>(&mut conn)
}

pub fn get_daily_login_reports_of_last_days(
    pool: &DbPool,
    days: i64,
) -> Result<Vec<DailyLoginReports>, diesel::result::Error> {
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
}

pub fn get_daily_login_report_by_id(
    pool: &DbPool,
    report_id: String,
) -> Result<DailyLoginReports, diesel::result::Error> {
    let mut conn = pool.get().expect("Failed to get connection from pool.");
    daily_login_reports::table.find(report_id).first(&mut conn)
}

pub fn get_latest_daily_login(pool: &DbPool) -> Result<DailyLoginReports, diesel::result::Error> {
    let mut conn = pool.get().expect("Failed to get connection from pool.");
    daily_login_reports::table
        .order(daily_login_reports::startTime.desc())
        .first(&mut conn)
}

pub fn insert_or_update_daily_login_report(
    pool: &DbPool,
    report: DailyLoginReports,
) -> Result<usize, diesel::result::Error> {
    let mut conn = pool.get().expect("Failed to get connection from pool.");

    let insertable = NewDailyLoginReports::from(report.clone());
    let updatable = UpdateDailyLoginReports::from(report);

    diesel::insert_into(daily_login_reports::table)
        .values(&insertable)
        .on_conflict(daily_login_reports::id)
        .do_update()
        .set(&updatable)
        .execute(&mut conn)
}

// #############################
// #  DailyLoginReportEntries  #
// #############################

pub fn get_all_daily_login_report_entries(
    pool: &DbPool,
) -> Result<Vec<DailyLoginReportEntries>, diesel::result::Error> {
    let mut conn = pool.get().expect("Failed to get connection from pool.");
    daily_login_report_entries::table.load::<DailyLoginReportEntries>(&mut conn)
}

pub fn get_daily_login_report_entry_by_id(
    pool: &DbPool,
    report_entry_id: Option<i32>,
) -> Result<DailyLoginReportEntries, diesel::result::Error> {
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
}

pub fn get_daily_login_report_entries_by_report_id(
    pool: &DbPool,
    report_id: String,
) -> Result<Vec<DailyLoginReportEntries>, diesel::result::Error> {
    let mut conn = pool.get().expect("Failed to get connection from pool.");
    daily_login_report_entries::table
        .filter(daily_login_report_entries::reportId.eq(report_id))
        .load::<DailyLoginReportEntries>(&mut conn)
}

pub fn insert_or_update_daily_login_report_entry(
    pool: &DbPool,
    entry: DailyLoginReportEntries,
) -> Result<i32, diesel::result::Error> {
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
        .filter(daily_login_report_entries::reportId.eq(entry.reportId))
        .filter(daily_login_report_entries::accountEmail.eq(entry.accountEmail))
        .first::<Option<i32>>(&mut conn);

    match res {
        Ok(Some(id_value)) => return Ok(id_value),
        Ok(None) => return Err(diesel::result::Error::NotFound),
        Err(e) => return Err(e),
    }
}

//########################
//#       CharList       #
//########################

pub fn get_all_char_list(pool: &DbPool) -> Result<Vec<CharListEntries>, diesel::result::Error> {
    let mut conn = pool.get().expect("Failed to get connection from pool.");

    char_list_entries::table.load::<CharListEntries>(&mut conn)
}

pub fn get_latest_char_list_for_each_account(
    pool: &DbPool,
) -> Result<Vec<CharListEntries>, diesel::result::Error> {
    let mut conn = pool.get().expect("Failed to get connection from pool.");

    // Get all emails (distinct) from the char_list_entries table
    let emails_result: Vec<Option<String>> = char_list_entries::table
        .select(char_list_entries::email.nullable())
        .distinct()
        .load::<Option<String>>(&mut conn)?;

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
        let entry = char_list_entries::table
            .filter(char_list_entries::email.eq(&mail))
            .order(char_list_entries::timestamp.desc())
            .first::<CharListEntries>(&mut conn)?;
        entries.push(entry);
    }

    Ok(entries)
}

//########################
//#    CharListDataset   #
//########################

pub fn get_latest_char_list_dataset_for_each_account(
    pool: &DbPool,
) -> Result<Vec<CharListDataset>, diesel::result::Error> {
    let mut conn = pool.get().expect("Failed to get connection from pool.");

    let char_list_entries = get_latest_char_list_for_each_account(pool)?;

    let mut datasets = Vec::new();

    for entry in char_list_entries {
        let account = account::table
            .filter(account::entry_id.eq(&entry.id))
            .first::<Account>(&mut conn)
            .expect("Failed to get account");

        let class_stats = class_stats::table
            .filter(class_stats::entry_id.eq(entry.id.clone()))
            .load::<ClassStats>(&mut conn)
            .expect("Failed to get class_stats");

        let character = character::table
            .filter(character::entry_id.eq(entry.id))
            .load::<Character>(&mut conn)
            .expect("Failed to get character");

        datasets.push(CharListDataset {
            email: entry.email.unwrap(),
            account,
            class_stats,
            character,
        });
    }

    Ok(datasets)
}

pub fn insert_char_list_dataset(
    pool: &DbPool,
    dataset: CharListDataset,
) -> Result<usize, diesel::result::Error> {
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
    let mut acc = NewAccount::from(dataset.account);
    acc.entry_id = Some(entry_uuid.clone());
    let result = insert_into(account::table).values(acc).execute(&mut conn);
    if result.is_err() {
        return Err(result.unwrap_err());
    }

    //class_stats
    dataset.class_stats.iter().for_each(|class_stat| {
        let entry_uuid_opt_clone = entry_uuid_opt.as_ref().map(|s| s.clone());
        insert_into(class_stats::table)
            .values(NewClassStats::from_class_stats(
                class_stat,
                entry_uuid_opt_clone,
            ))
            .execute(&mut conn)
            .expect("Failed to insert class_stats");
    });

    //character
    dataset.character.iter().for_each(|chara| {
        let entry_uuid_opt_clone = entry_uuid_opt.clone();
        insert_into(character::table)
            .values(NewCharacter::from_character(
                chara,
                Some(entry_uuid_opt_clone.expect("Panick: entry_uuid_opt_clone is none")),
            ))
            .execute(&mut conn)
            .expect("Failed to insert character");
    });

    Ok(0)
}

//########################
//#      EamAccount      #
//########################

pub fn get_all_eam_accounts_for_daily_login(
    pool: &DbPool,
) -> Result<Vec<EamAccount>, diesel::result::Error> {
    info!("Getting all EAM accounts for daily login...");

    let mut conn = pool.get().expect("Failed to get connection from pool.");

    info!("Got connection from pool. Loading accounts...");

    let result = eam_accounts::table
        .filter(eam_accounts::isDeleted.eq(false))
        .filter(eam_accounts::performDailyLogin.eq(true))
        .load::<EamAccount>(&mut conn);

    match &result {
        Ok(accounts) => info!("Loaded {} accounts.", accounts.len()),
        Err(e) => error!("Failed to load accounts: {}", e),
    }

    result
}

pub fn get_all_eam_accounts(pool: &DbPool) -> Result<Vec<EamAccount>, diesel::result::Error> {
    info!("Getting all EAM accounts...");

    let mut conn = pool.get().expect("Failed to get connection from pool.");

    info!("Got connection from pool. Loading accounts...");

    let result = eam_accounts::table
        .filter(eam_accounts::isDeleted.eq(false))
        .order((eam_accounts::orderId.asc(), eam_accounts::id.asc()))
        .load::<EamAccount>(&mut conn);

    match &result {
        Ok(accounts) => info!("Loaded {} accounts.", accounts.len()),
        Err(e) => error!("Failed to load accounts: {}", e),
    }

    result
}

pub fn get_all_eam_account_emails(pool: &DbPool) -> Result<Vec<String>, diesel::result::Error> {
    info!("Getting all EAM account emails...");
    let mut conn = pool.get().expect("Failed to get connection from pool.");
    info!("Got connection from pool. Loading emails...");
    let result = eam_accounts::table
        .select(eam_accounts::email)
        .filter(eam_accounts::isDeleted.eq(false))
        .order((eam_accounts::orderId.asc(), eam_accounts::id.asc()))
        .load::<String>(&mut conn);

    match &result {
        Ok(emails) => info!("Loaded {} emails.", emails.len()),
        Err(e) => error!("Failed to load emails: {}", e),
    }

    result
}

pub fn get_eam_account_by_email(
    pool: &DbPool,
    account_email: String,
) -> Result<EamAccount, diesel::result::Error> {
    info!("Getting EAM account by email: {}", account_email);

    let mut conn = pool.get().expect("Failed to get connection from pool.");
    eam_accounts::table
        .find(account_email)
        .filter(eam_accounts::isDeleted.eq(false))
        .first(&mut conn)
}

pub fn insert_or_update_eam_account(
    pool: &DbPool,
    eam_account: EamAccount,
) -> Result<usize, diesel::result::Error> {
    let mut conn = pool.get().expect("Failed to get connection from pool.");

    // Get the max id and increment it by 1
    let max_id: Option<Option<i32>> = eam_accounts::table
        .select(diesel::dsl::max(eam_accounts::id))
        .first(&mut conn)
        .optional()?;

    let new_id = max_id.unwrap_or(None).unwrap_or(0) + 1; // If no max id, start at 1

    let mut clone_acc = eam_account.clone();
    clone_acc.id = Some(new_id);
    let insertable = NewEamAccount::from(clone_acc.clone());
    let updatable = UpdateEamAccount::from(eam_account);

    let new_row_inserted;

    match insert_into(eam_accounts::table)
        .values(&insertable)
        .execute(&mut conn)
    {
        Ok(_) => {
            new_row_inserted = true;
        }
        Err(DatabaseError(_unique_violation, _)) => {
            diesel::update(eam_accounts::table)
                .filter(eam_accounts::email.eq(&insertable.email))
                .set(&updatable)
                .execute(&mut conn)?;
            new_row_inserted = false;
        }
        Err(_) => {
            return Err(diesel::result::Error::RollbackTransaction);
        }
    }

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

pub fn delete_eam_account(
    pool: &DbPool,
    account_email: String,
) -> Result<usize, diesel::result::Error> {
    let mut conn = pool.get().expect("Failed to get connection from pool.");

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
        .execute(&mut conn)?;

    Ok(0)
}

//########################
//#       EamGroup       #
//########################

pub fn get_all_eam_groups(pool: &DbPool) -> Result<Vec<EamGroup>, diesel::result::Error> {
    let mut conn = pool.get().expect("Failed to get connection from pool.");
    EamGroup.load::<EamGroup>(&mut conn)
}

pub fn insert_or_update_eam_group(
    pool: &DbPool,
    eam_group: EamGroup,
) -> Result<usize, diesel::result::Error> {
    let mut conn = pool.get().expect("Failed to get connection from pool.");

    let insertable: NewEamGroup = NewEamGroup::from(eam_group.clone());
    let updatable: UpdateEamGroup = UpdateEamGroup::from(eam_group.clone());

    diesel::insert_into(eam_groups::table)
        .values(&insertable)
        .on_conflict(eam_groups::id)
        .do_update()
        .set(&updatable)
        .execute(&mut conn)
}

pub fn delete_eam_group(pool: &DbPool, group_id: i32) -> Result<usize, diesel::result::Error> {
    let mut conn = pool.get().expect("Failed to get connection from pool.");
    diesel::delete(eam_groups::table.find(group_id)).execute(&mut conn)
}

// ############################
// #         AuditLog         #
// ############################

pub fn get_all_audit_logs(pool: &DbPool) -> Result<Vec<AuditLog>, diesel::result::Error> {
    let mut conn = pool.get().expect("Failed to get connection from pool.");
    audit_logs::table.load::<AuditLog>(&mut conn)
}

pub fn get_audit_log_for_account(
    pool: &DbPool,
    account_email: String,
) -> Result<Vec<AuditLog>, diesel::result::Error> {
    let mut conn = pool.get().expect("Failed to get connection from pool.");
    audit_logs::table
        .filter(accountEmail.eq(account_email))
        .load::<AuditLog>(&mut conn)
}

pub fn insert_audit_log(pool: &DbPool, log: AuditLog) -> Result<usize, diesel::result::Error> {
    let mut conn = pool.get().expect("Failed to get connection from pool.");

    let mut insertable: NewAuditLog = NewAuditLog::from(log);
    insertable.time = chrono::Utc::now().naive_utc().to_string();

    insert_into(audit_logs::table)
        .values(&insertable)
        .execute(&mut conn)
}

// ############################
// #         ErrorLog         #
// ############################

pub fn get_all_error_logs(pool: &DbPool) -> Result<Vec<ErrorLog>, diesel::result::Error> {
    let mut conn = pool.get().expect("Failed to get connection from pool.");
    let logs = error_logs::table.load::<ErrorLog>(&mut conn)?;

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

pub fn insert_error_log(pool: &DbPool, log: ErrorLog) -> Result<usize, diesel::result::Error> {
    let mut conn = pool.get().expect("Failed to get connection from pool.");

    let mut insertable: NewErrorLog = NewErrorLog::from(log);
    insertable.time = chrono::Utc::now().naive_utc().to_string();

    insert_into(error_logs::table)
        .values(&insertable)
        .execute(&mut conn)
}

pub fn delete_error_logs_older_than_days(
    pool: &DbPool,
    days: i64,
) -> Result<usize, diesel::result::Error> {
    let mut conn = pool.get().expect("Failed to get connection from pool.");

    let target_date = chrono::Utc::now().naive_utc()
        - chrono::Duration::try_days(days as i64).expect("Failed to create duration");
    let target_date_str = target_date.format("%Y-%m-%d %H:%M:%S").to_string();
    let num_deleted =
        diesel::delete(error_logs::table.filter(error_logs::time.lt(target_date_str)))
            .execute(&mut conn)?;

    Ok(num_deleted)
}
