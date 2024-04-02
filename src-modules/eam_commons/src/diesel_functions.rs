use crate::diesel_setup::DbPool;
use crate::models::{AuditLog, NewAuditLog};
use crate::models::{CharListDataset, CharListEntries, NewCharListEntries};
use crate::models::{EamAccount, NewEamAccount, UpdateEamAccount};
use crate::models::{EamGroup, NewEamGroup, UpdateEamGroup};
use crate::models::{ErrorLog, NewErrorLog};
use crate::models::{NewAccount, NewCharacter, NewClassStats};
use crate::models::{UserData, NewUserData, UpdateUserData};
use crate::models::{DailyLoginReports, NewDailyLoginReports, UpdateDailyLoginReports};
use crate::models::{DailyLoginReportEntries, NewDailyLoginReportEntries, UpdateDailyLoginReportEntries};
use crate::schema::account::dsl::*;
use crate::schema::char_list_entries::dsl::*;
use crate::schema::character::dsl::*;
use crate::schema::class_stats::dsl::*;
use crate::schema::AuditLog as audit_logs;
use crate::schema::AuditLog::dsl::*;
use crate::schema::EamAccount as eam_accounts;
use crate::schema::EamAccount::dsl::*;
use crate::schema::EamGroup as eam_groups;
use crate::schema::EamGroup::dsl::*;
use crate::schema::ErrorLog as error_logs;
use crate::schema::UserData as user_data;
use crate::schema::DailyLoginReports as daily_login_reports;
use crate::schema::DailyLoginReportEntries as daily_login_report_entries;
use diesel::insert_into;
use diesel::prelude::*;
use diesel::result::Error::DatabaseError;
use diesel::RunQueryDsl;
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

pub fn get_all_daily_login_reports(pool: &DbPool) -> Result<Vec<DailyLoginReports>, diesel::result::Error> {
    let mut conn = pool.get().expect("Failed to get connection from pool.");
    daily_login_reports::table.load::<DailyLoginReports>(&mut conn)
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
    report_entry_id: i32,
) -> Result<DailyLoginReportEntries, diesel::result::Error> {
    let mut conn = pool.get().expect("Failed to get connection from pool.");
    daily_login_report_entries::table.find(report_entry_id).first(&mut conn)
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
    daily_login_report_entries::table
        .select(daily_login_report_entries::id)
        .filter(daily_login_report_entries::reportId.eq(entry.reportId))
        .filter(daily_login_report_entries::accountEmail.eq(entry.accountEmail))
        .first::<i32>(&mut conn)
}

//########################
//#    CharListDataset   #
//########################
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
    let result = insert_into(char_list_entries)
        .values(&new_entry)
        .execute(&mut conn);
    if result.is_err() {
        return Err(result.unwrap_err());
    }

    //account
    let mut acc = NewAccount::from(dataset.account);
    acc.entry_id = Some(entry_uuid.clone());
    let result = insert_into(account).values(acc).execute(&mut conn);
    if result.is_err() {
        return Err(result.unwrap_err());
    }

    //class_stats
    dataset.class_stats.iter().for_each(|class_stat| {
        let entry_uuid_opt_clone = entry_uuid_opt.as_ref().map(|s| s.clone());
        insert_into(class_stats)
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
        insert_into(character)
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
    let mut conn = pool.get().expect("Failed to get connection from pool.");

    eam_accounts::table
        .filter(eam_accounts::performDailyLogin.eq(true))
        .load::<EamAccount>(&mut conn)
}

pub fn get_all_eam_accounts(pool: &DbPool) -> Result<Vec<EamAccount>, diesel::result::Error> {
    let mut conn = pool.get().expect("Failed to get connection from pool.");
    EamAccount.load::<EamAccount>(&mut conn)
}

pub fn get_eam_account_by_email(
    pool: &DbPool,
    account_email: String,
) -> Result<EamAccount, diesel::result::Error> {
    let mut conn = pool.get().expect("Failed to get connection from pool.");
    eam_accounts::table.find(account_email).first(&mut conn)
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
    diesel::delete(eam_accounts::table.find(account_email)).execute(&mut conn)
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
    error_logs::table.load::<ErrorLog>(&mut conn)
}

pub fn insert_error_log(pool: &DbPool, log: ErrorLog) -> Result<usize, diesel::result::Error> {
    let mut conn = pool.get().expect("Failed to get connection from pool.");

    let mut insertable: NewErrorLog = NewErrorLog::from(log);
    insertable.time = chrono::Utc::now().naive_utc().to_string();

    insert_into(error_logs::table)
        .values(&insertable)
        .execute(&mut conn)
}
