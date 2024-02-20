import { createContext, useEffect, useState } from "react";
import { invoke } from "@tauri-apps/api";

const GroupsContext = createContext();

const DEFAULT_GROUPS = [
    {
        name: "Favorit",
        color: 4,
        iconType: "mui",
        icon: "StarOutlined",
        padding: "5%"
    },
    {
        name: "Heart",
        color: 3,
        iconType: "mui",
        icon: "FavoriteOutlined",
        padding: "5%"
    },
    {
        name: "EAM",
        color: 0,
        iconType: "mui",
        icon: "AccountBalanceWallet",
        padding: "5%"
    }
];

function GroupsContextProvider({ children }) {
    const [groups, setGroups] = useState([]);

    const loadGroups = async () => {
        const g = await invoke('get_all_eam_groups');
        
        if (!!g && g.length > 0) {
            setGroups(g);
        } else {
            setGroups(DEFAULT_GROUPS);

            for (let i = 0; i < DEFAULT_GROUPS.length; i++) {
                await saveGroup(DEFAULT_GROUPS[i]);
            }
        }
    };

    useEffect(() => {
        loadGroups();
    }, []);

    const saveGroup = async (group) => {
        if (!group.id)
            group.id = -1;
        await invoke('insert_or_update_eam_group', { eamGroup: group });
        loadGroups();
    };

    const value = {
        groups: groups,
        saveGroup: saveGroup
    }

    return (
        <GroupsContext.Provider value={value}>
            {children}
        </GroupsContext.Provider>
    );
}


export { GroupsContextProvider };
export default GroupsContext;