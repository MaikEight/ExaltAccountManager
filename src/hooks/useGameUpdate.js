import { useContext } from "react";
import GameUpdateContext from "../contexts/GameUpdateContext";

function useGameUpdate() {
    return useContext(GameUpdateContext);
}

export default useGameUpdate;
