import { useContext } from "react";
import { UserLoginContext } from "../contexts/UserLoginContext";

function useUserLogin() {
    return useContext(UserLoginContext);
}

export { useUserLogin };