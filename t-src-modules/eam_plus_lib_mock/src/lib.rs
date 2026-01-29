#[macro_use]
extern crate lazy_static;
use std::sync::Mutex;

lazy_static! {
    static ref VERIFICATION_PASSED: Mutex<bool> = Mutex::new(false);
}

#[allow(dead_code)]
fn set_verification_passed(passed: bool) {
    let mut verification = VERIFICATION_PASSED.lock().unwrap();
    *verification = passed;
}

pub fn is_verification_passed() -> bool {
    let verification = VERIFICATION_PASSED.lock().unwrap();
    *verification
}

pub mod constants;
pub mod eam_subscriptions_api_utils;
pub mod eam_user_api_utils;
pub mod user_status_utils;
pub mod daily_login;