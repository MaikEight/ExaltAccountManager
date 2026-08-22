import { useContext } from "react";
import RunningGamesContext from "../contexts/RunningGamesContext";

function useRunningGames() {
    return useContext(RunningGamesContext);
}

export default useRunningGames;
