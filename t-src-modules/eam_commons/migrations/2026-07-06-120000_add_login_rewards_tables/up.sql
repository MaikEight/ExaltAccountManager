-- Global monthly daily-login reward calendar (shared across all accounts).
-- One row per (month, day). Only NonConsecutive rewards are stored.
CREATE TABLE LoginRewardsCalendar (
    id INTEGER PRIMARY KEY AUTOINCREMENT,
    month TEXT NOT NULL,                 -- "YYYY-MM", derived from the response serverTime (UTC)
    day INTEGER NOT NULL,                -- reward tier 1..31
    item_id INTEGER NOT NULL,
    quantity INTEGER NOT NULL DEFAULT 1,
    gold INTEGER NOT NULL DEFAULT 0,
    updated_at TEXT NOT NULL
);
CREATE UNIQUE INDEX idx_login_rewards_month_day ON LoginRewardsCalendar(month, day);

-- Per-account monthly claim/unlock status. One row per (account_email, month).
CREATE TABLE AccountLoginRewards (
    id INTEGER PRIMARY KEY AUTOINCREMENT,
    account_email TEXT NOT NULL,
    month TEXT NOT NULL,
    unlockable_days INTEGER NOT NULL DEFAULT 0,   -- from <Unlockable days=N>: logins this month = unlocked tier count
    claimed_days TEXT,                            -- CSV of claimed tier numbers, e.g. "1,2,3,4,5"
    server_time TEXT,
    updated_at TEXT NOT NULL
);
CREATE UNIQUE INDEX idx_account_login_rewards_email_month ON AccountLoginRewards(account_email, month);
