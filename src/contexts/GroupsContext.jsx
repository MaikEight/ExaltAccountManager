import { createContext, useEffect, useState } from "react";
import { GROUPS_FILE_PATH } from "../constants";
import { readFileUTF8 } from "../utils/readFileUtil";
import { writeFileUTF8 } from "../utils/writeFileUtil";

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

    useEffect(() => {
        const loadGroups = async () => {
            const p = await GROUPS_FILE_PATH();
            const g = await readFileUTF8(p, true);
            if (g) {
                setGroups(g);
            } else {
                setGroups(DEFAULT_GROUPS);
            }
        }

        loadGroups();
    }, []);

    const saveGroups = async (newGroups) => {
        const p = await GROUPS_FILE_PATH();
        await writeFileUTF8(p, newGroups, true);
        setGroups(newGroups);
    };

    const value = {
        groups: groups,
        saveGroups: saveGroups
    }

    return (
        <GroupsContext.Provider value={value}>
            {children}
        </GroupsContext.Provider>
    );
}


export { GroupsContextProvider };
export default GroupsContext;