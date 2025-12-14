import { useContext } from "react";
import WidgetsContext from "../contexts/WidgetsContext";

function useWidgets() {
    return useContext(WidgetsContext);
}

export default useWidgets;