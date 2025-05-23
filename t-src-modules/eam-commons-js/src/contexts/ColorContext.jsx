import { createContext, useContext, useMemo } from "react";
import { createTheme } from "@mui/material";
import { darkTheme } from "../themes/dark";
import { lightTheme } from "../themes/light";
import { UserSettingsContext } from "./UserSettingsContext";

const ColorContext = createContext();

function ColorContextProvider({ children }) {
    const userSettings = useContext(UserSettingsContext);

    const theme = useMemo(
        () => {
            const mode = userSettings.getByKeyAndSubKey("general", "theme");
            const t = createTheme((!mode || mode === "dark") ? darkTheme : lightTheme);
            const body = document.body;
            body.classList.toggle('dark-theme', t.palette.mode === 'dark');
            return t;
        },
        [userSettings.getByKeyAndSubKey("general", "theme")]
    );

    const colorMode = {
        toggleColorMode: () => {
            const mode = userSettings.getByKeyAndSubKey("general", "theme");
            const m = (!mode || mode === "dark") ? "light" : "dark";
            userSettings.setByKeyAndSubKey("general", "theme", m);
            document.documentElement.setAttribute('data-color-mode', m);
        },
        colorMode: userSettings.getByKeyAndSubKey("general", "theme"),
        setColorMode: (m) => {
            userSettings.setByKeyAndSubKey("general", "theme", m);
            document.documentElement.setAttribute('data-color-mode', m);
        },
        theme: theme,
    };

    return (
        <ColorContext.Provider value={colorMode}>
            {children}
        </ColorContext.Provider>
    );
}

export { ColorContextProvider, ColorContext };
