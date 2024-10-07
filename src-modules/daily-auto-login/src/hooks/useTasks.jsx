import { useContext } from "react";
import TaskContext from "../contexts/TaskContext";

function useTasks() {
  return useContext(TaskContext);
}

export default useTasks;