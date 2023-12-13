import { Box, CssBaseline, ThemeProvider as MuiThemeProvider } from "@mui/material";
import { ThemeProvider as StyledThemeProvider } from "styled-components";
import Sidebar from "./components/Sidebar/Sidebar";
import ColorContext from "./contexts/ColorContext";
import { useContext } from "react";
import AccountsPage from "./pages/AccountsPage";

function MainRouter() {
    const colorContext = useContext(ColorContext);
    const theme = colorContext.theme;

    return (
        <StyledThemeProvider theme={theme}>
            <MuiThemeProvider theme={theme}>
                <CssBaseline enableColorScheme />
                <div id="router" style={{width: '100%'}}>
                    <Sidebar id="sidebar">
                        <AccountsPage />
                    </Sidebar>
                </div>
            </MuiThemeProvider>
        </StyledThemeProvider>
    );
}

export default MainRouter;