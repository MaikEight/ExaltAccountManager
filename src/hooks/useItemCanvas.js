import { useContext } from "react";
import ItemCanvasContext from "../contexts/ItemCanvasContext";

function useItemCanvas() {
    return useContext(ItemCanvasContext);
}

export default useItemCanvas;