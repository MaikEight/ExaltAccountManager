-- ParsedItems table for storing normalized item data
CREATE TABLE ParsedItems (
    id INTEGER PRIMARY KEY AUTOINCREMENT,
    entry_id TEXT,
    account_id TEXT,
    storage_type_id TEXT NOT NULL,
    container_index INTEGER,
    slot_index INTEGER,
    quantity INTEGER NOT NULL DEFAULT 1,
    item_id INTEGER NOT NULL,
    unique_id_raw TEXT,
    enchant_b64 TEXT,
    enchant_ids TEXT  -- stored as CSV: "123,456,789"
);

-- Indexes for common queries
CREATE INDEX idx_parsed_items_entry_id ON ParsedItems(entry_id);
CREATE INDEX idx_parsed_items_item_id ON ParsedItems(item_id);

-- PcStats table for storing character stat data
CREATE TABLE PcStats (
    id INTEGER PRIMARY KEY AUTOINCREMENT,
    entry_id TEXT,
    char_id INTEGER,
    stat_type INTEGER NOT NULL,
    stat_value INTEGER NOT NULL
);

-- Index for querying stats by entry and character
CREATE INDEX idx_pc_stats_entry_char ON PcStats(entry_id, char_id);
