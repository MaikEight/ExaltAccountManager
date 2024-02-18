use crate::schema::EamAccount::dsl::*;
use crate::schema::EamAccount as eam_accounts;
use crate::models::EamAccount;
use crate::models::NewEamAccount;
use crate::models::UpdateEamAccount;
use diesel::prelude::*;
use diesel::insert_into;
use crate::diesel_setup::DbPool;

pub fn get_all_eam_accounts(pool: &DbPool) -> Result<Vec<EamAccount>, diesel::result::Error> {
    let mut conn = pool.get().expect("Failed to get connection from pool.");
    EamAccount.load::<EamAccount>(&mut conn)
}

pub fn insert_or_update_eam_account(pool: &DbPool, mut eam_account: EamAccount) -> Result<usize, diesel::result::Error> {
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

    insert_into(eam_accounts::table)
        .values(&insertable)
        .on_conflict(eam_accounts::email)
        .do_update()
        .set(&updatable)
        .execute(&mut conn)
}

pub fn delete_eam_account(pool: &DbPool, account_email: String) -> Result<usize, diesel::result::Error> {
    let mut conn = pool.get().expect("Failed to get connection from pool.");
    diesel::delete(eam_accounts::table.find(account_email)).execute(&mut conn)
}