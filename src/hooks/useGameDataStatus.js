import { useContext } from "react";
import GameDataStatusContext from "../contexts/GameDataStatusContext";

function useGameDataStatus() {
    return useContext(GameDataStatusContext);
}

export default useGameDataStatus;
