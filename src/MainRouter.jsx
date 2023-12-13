import { CssBaseline, ThemeProvider as MuiThemeProvider } from "@mui/material";
import { ThemeProvider as StyledThemeProvider } from "styled-components";
import Sidebar from "./components/Sidebar";
import ColorContext from "./contexts/ColorContext";
import { useContext } from "react";
import CustomToolbar from "./components/CustomToolbar";
import SideBarLogo from "./components/SideBarLogo";

function MainRouter() {
    const colorContext = useContext(ColorContext);
    const theme = colorContext.theme;

    return (
        <StyledThemeProvider theme={theme}>
            <MuiThemeProvider theme={theme}>
                <CssBaseline enableColorScheme />
                <>
                    <Sidebar >
                        <p>Content</p>
                    </Sidebar>
                </>
            </MuiThemeProvider>
        </StyledThemeProvider>
    );
}

export default MainRouter;