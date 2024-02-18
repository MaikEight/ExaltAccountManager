CREATE TABLE EamAccount (
    id INTEGER UNIQUE,
    name TEXT,
    email TEXT NOT NULL UNIQUE PRIMARY KEY,
    password TEXT NOT NULL,
    serverName TEXT,
    performDailyLogin BOOLEAN NOT NULL,
    state TEXT,
    lastLogin TEXT,
    "group" TEXT,
    extra TEXT
);