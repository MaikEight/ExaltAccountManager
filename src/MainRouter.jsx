import { CssBaseline } from "@mui/material";
import { ThemeProvider as MuiThemeProvider } from "@mui/material/styles";
import { BrowserRouter as Router, Route, Routes, Navigate } from 'react-router-dom';
import { ThemeProvider as StyledThemeProvider } from "styled-components";
import Sidebar from "./components/Sidebar/Sidebar";
import ColorContext from "./contexts/ColorContext";
import { useContext } from "react";
import AccountsPage from "./pages/AccountsPage";
import GameUpdaterPage from "./pages/GameUpdaterPage";
import { GroupsContextProvider } from "./contexts/GroupsContext";
import SettingsPage from "./pages/SettingsPage";
import { ServerContextProvider } from "./contexts/ServerContext";
import AboutPage from "./pages/AboutPage";

function MainRouter() {
    const colorContext = useContext(ColorContext);
    const theme = colorContext.theme;

    return (
        <StyledThemeProvider theme={theme}>
            <MuiThemeProvider theme={theme}>
                <CssBaseline enableColorScheme />
                <div style={{ width: '100%' }}>
                    <Router id="router">
                        <Sidebar id="sidebar">
                            <GroupsContextProvider>
                                <ServerContextProvider>
                                    <Routes>
                                        <Route path='/' element={<AccountsPage />}></Route>
                                        <Route path='/accounts' element={<AccountsPage />}></Route>
                                        <Route path='/gameUpdater' element={<GameUpdaterPage />}></Route>
                                        <Route path='/settings' element={<SettingsPage />}></Route>
                                        <Route path='/about' element={<AboutPage />}></Route>
                                        <Route path='*' element={<AccountsPage />}></Route>
                                    </Routes>
                                </ServerContextProvider>
                            </GroupsContextProvider>
                        </Sidebar>
                    </Router>
                </div>
            </MuiThemeProvider>
        </StyledThemeProvider>
    );
}

export default MainRouter;