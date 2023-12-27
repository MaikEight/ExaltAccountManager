import { useMemo } from "react";
import { readFileUTF8 } from "../utils/readFileUtil";
import { GROUPS_FILE_NAME } from "../constants";

const useGroups = () => {
    const groups = useMemo(
        async () => await readFileUTF8(GROUPS_FILE_NAME, true)
    , []);            
    
    return groups;
};

const useGroupsByName = (name) => {  
    return useGroups().find(g => g.name === name);
};

export { useGroups, useGroupsByName };