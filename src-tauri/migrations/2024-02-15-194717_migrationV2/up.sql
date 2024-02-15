CREATE TABLE EamAccount (
    id INTEGER PRIMARY KEY,
    name TEXT,
    email TEXT NOT NULL,
    password TEXT NOT NULL,
    serverName TEXT,
    performDailyLogin BOOLEAN NOT NULL,
    state TEXT,
    lastLogin TEXT,
    "group" TEXT,
    extra TEXT
);