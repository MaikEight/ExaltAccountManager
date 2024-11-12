CREATE TABLE EamAccount (
    id INTEGER UNIQUE,
    name TEXT,
    email TEXT NOT NULL UNIQUE PRIMARY KEY,
    isSteam BOOLEAN NOT NULL DEFAULT FALSE, 
    steamId TEXT,
    password TEXT NOT NULL,
    serverName TEXT,
    performDailyLogin BOOLEAN NOT NULL,
    state TEXT,
    lastLogin TEXT,
    lastRefresh TEXT,
    "group" TEXT,
    token TEXT,
    extra TEXT
);

CREATE TABLE EamGroup (
    id INTEGER UNIQUE PRIMARY KEY,
    name TEXT NOT NULL UNIQUE,
    color INTEGER NOT NULL,
    iconType TEXT NOT NULL,
    icon TEXT NOT NULL,
    padding TEXT NOT NULL
);