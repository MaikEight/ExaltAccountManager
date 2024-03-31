CREATE TABLE DailyLoginReports (
    id TEXT PRIMARY KEY,
    startTime DATETIME DEFAULT CURRENT_TIMESTAMP,
    endTime DATETIME,
    hasFinished BOOLEAN NOT NULL DEFAULT FALSE,
    emailsToProcess TEXT,
    amountOfAccounts INTEGER NOT NULL DEFAULT 0,
    amountOfAccountsProcessed INTEGER NOT NULL DEFAULT 0,
    amountOfAccountsFailed INTEGER NOT NULL DEFAULT 0,
    amountOfAccountsSucceeded INTEGER NOT NULL DEFAULT 0
);

CREATE TABLE DailyLoginReportEntries (
    id INTEGER PRIMARY KEY AUTOINCREMENT,
    reportId TEXT,
    startTime DATETIME DEFAULT CURRENT_TIMESTAMP,
    endTime DATETIME,
    accountEmail TEXT REFERENCES EamAccount(email),
    status TEXT NOT NULL,
    errorMessage TEXT,
    FOREIGN KEY (reportId) REFERENCES DailyLoginReports(id)
);

CREATE TABLE UserData (
    dataKey TEXT PRIMARY KEY,
    dataValue TEXT NOT NULL
);