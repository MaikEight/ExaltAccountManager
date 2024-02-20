import { useContext } from "react";
import GroupsContext from "../contexts/GroupsContext";

function useGroups() {
    return useContext(GroupsContext);
}

export default useGroups;