import { useContext } from "react";
import BackgroundSyncContext from "../contexts/BackgroundSyncContext";

function useBackgroundSync() {
    return useContext(BackgroundSyncContext);
}

export default useBackgroundSync;