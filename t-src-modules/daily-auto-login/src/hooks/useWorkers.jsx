import { useContext } from "react";
import WorkerContext from "../contexts/WorkerContext";

function useWorkers() {
    return useContext(WorkerContext);
}

export default useWorkers;