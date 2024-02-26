import { useContext } from "react";
import ServerContext from "../contexts/ServerContext";

function useServerList() {
    return useContext(ServerContext);
}

export default useServerList;