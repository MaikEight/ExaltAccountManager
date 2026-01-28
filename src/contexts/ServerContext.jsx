import { createContext, useEffect, useState, useCallback } from "react";
import { SERVER_LIST_FILE_PATH } from "../constants";
import { readFileUTF8 } from "eam-commons-js";
import { invoke } from "@tauri-apps/api/core";
import { remove } from "@tauri-apps/plugin-fs";

const ServerContext = createContext();

function ServerContextProvider({ children }) {
    const [serverList, setServerList] = useState([]);

    const getServers = useCallback(async () => {
        try {
            // Try to load servers from database
            const servers = await invoke('get_all_servers');
            
            if (servers && servers.length > 0) {
                setServerList(servers);
                return servers;
            }

            // If database is empty, try to migrate from file
            console.info('No servers in database, attempting file migration...');
            try {
                const filePath = await SERVER_LIST_FILE_PATH();
                const fileData = await readFileUTF8(filePath, true);
                
                if (fileData && Array.isArray(fileData) && fileData.length > 0) {
                    console.info(`Migrating ${fileData.length} servers from file to database...`);
                    
                    // Convert file format to database format (add id: null if not present)
                    const serversToMigrate = fileData.map(server => ({
                        id: null,
                        name: server.name || server.Name || '',
                        dns: server.dns || server.DNS || '',
                        lat: server.lat || server.Lat || null,
                        long: server.long || server.Long || null,
                        usage: server.usage || server.Usage || null,
                    }));
                    
                    // Insert servers into database
                    await invoke('insert_servers_from_migration', { servers: serversToMigrate });
                    
                    // Delete the old file
                    try {
                        await remove(filePath);
                        console.info('Successfully deleted old server list file after migration');
                    } catch (deleteErr) {
                        console.warn('Could not delete old server list file:', deleteErr);
                    }
                    
                    // Load servers from database again
                    const migratedServers = await invoke('get_all_servers');
                    setServerList(migratedServers);
                    return migratedServers;
                }
            } catch (fileErr) {
                console.info('No server list file to migrate or error reading file:', fileErr);
            }

            // No servers found anywhere
            setServerList([]);
            return [];
        } catch (err) {
            console.error('Error loading servers:', err);
            setServerList([]);
            return [];
        }
    }, []);

    useEffect(() => {
        getServers();
    }, [getServers]);

    /**
     * Reload servers from the database.
     * Call this after a char/list request updates the servers.
     */
    const reloadServers = useCallback(async () => {
        try {
            const servers = await invoke('get_all_servers');
            setServerList(servers || []);
            return servers;
        } catch (err) {
            console.error('Error reloading servers:', err);
            return [];
        }
    }, []);

    return (
        <ServerContext.Provider value={{ serverList, reloadServers }}>
            {children}
        </ServerContext.Provider>
    );
}

export default ServerContext;
export { ServerContextProvider };