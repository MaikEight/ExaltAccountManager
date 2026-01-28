CREATE TABLE Servers (
    id INTEGER PRIMARY KEY,
    name TEXT NOT NULL UNIQUE,
    dns TEXT NOT NULL,
    lat TEXT,
    long TEXT,
    usage TEXT
);
