CREATE TABLE AuditLog (
    id INTEGER PRIMARY KEY,
    time DATETIME NOT NULL,
    sender TEXT NOT NULL,
    message TEXT NOT NULL,
    accountEmail TEXT REFERENCES EamAccount(email)
);

CREATE TABLE ErrorLog (
    id INTEGER PRIMARY KEY,
    time DATETIME NOT NULL,
    sender TEXT NOT NULL,
    message TEXT NOT NULL
);