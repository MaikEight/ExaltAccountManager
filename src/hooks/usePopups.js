import { useContext } from "react";
import PopupContext from "../contexts/PopupContext";

function usePopups() {
    return useContext(PopupContext);
}

export default usePopups;