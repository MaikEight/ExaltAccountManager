use crate::diesel_setup::DbPool;
use crate::models::EamAccount;
use crate::schema::EamAccount as eam_accounts;
use diesel::prelude::*;
use diesel::RunQueryDsl;

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

pub fn get_eam_account_by_email(
    pool: &DbPool,
    account_email: String,
) -> Result<EamAccount, diesel::result::Error> {
    let mut conn = pool.get().expect("Failed to get connection from pool.");
    eam_accounts::table.find(account_email).first(&mut conn)
}
