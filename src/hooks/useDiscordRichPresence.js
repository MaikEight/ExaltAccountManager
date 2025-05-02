import { useContext } from "react";
import DiscordContext from "../contexts/DiscordContext";

function useDiscordRichPresence() {
    return useContext(DiscordContext);
}

export default useDiscordRichPresence;