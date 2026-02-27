import { CACHE_PREFIX } from "../constants";

const migrations = [
    {
        id: 1,
        version: "4.4.0",
        migrate: () => {
            console.info("🚀 Performing migration for version 4.4.0: Clearing item caches");
            const clearAllCacheItemsWithPrefix = (prefix) => {                
                const keysToRemove = Object.keys(localStorage).filter(key => key.startsWith(prefix));
                keysToRemove.forEach(key => {
                    localStorage.removeItem(key);
                });
            }

            clearAllCacheItemsWithPrefix(`${CACHE_PREFIX}drawItem`);
            clearAllCacheItemsWithPrefix(`${CACHE_PREFIX}single-item`);
            clearAllCacheItemsWithPrefix(`${CACHE_PREFIX}portrait`);
        }
    }
];

function performMigrations() {
    const lastMigrationId = parseInt(localStorage.getItem("lastMigrationId") || "0", 10);
    const pendingMigrations = migrations.filter(migration => migration.id > lastMigrationId)?.sort((a, b) => a.id - b.id);

    pendingMigrations?.forEach(migration => {
        try {
            migration.migrate();
            localStorage.setItem("lastMigrationId", migration.id.toString());
        } catch (error) {
            console.error(`Migration ${migration.id} failed:`, error);
        }
    });
}

export default performMigrations;