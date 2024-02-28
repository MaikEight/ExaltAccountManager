import { BrowserRouter as Router, Route, Routes } from 'react-router-dom';
import Sidebar from "./components/Sidebar/Sidebar";
import AccountsPage from "./pages/AccountsPage";
import UtilitiesPage from "./pages/UtilitiesPage";
import { GroupsContextProvider } from "./contexts/GroupsContext";
import SettingsPage from "./pages/SettingsPage";
import { ServerContextProvider } from "./contexts/ServerContext";
import AboutPage from "./pages/AboutPage";
import { AccountsContextProvider } from "./contexts/AccountsContext";
import { useTheme } from '@emotion/react';
import { Box } from '@mui/material';

function MainRouter() {
    const theme = useTheme();

    return (
        <Box
            sx={{
                width: '100%',
                backgroundColor: theme.palette.background.default,
            }}
        >
            <Router id="router">
                <Sidebar id="sidebar">
                    <AccountsContextProvider>
                        <GroupsContextProvider>
                            <ServerContextProvider>
                                <Routes>
                                    <Route path='/' element={<AccountsPage />}></Route>
                                    <Route path='/accounts' element={<AccountsPage />}></Route>
                                    <Route path='/utilities' element={<UtilitiesPage />}></Route>
                                    <Route path='/settings' element={<SettingsPage />}></Route>
                                    <Route path='/about' element={<AboutPage />}></Route>
                                    <Route path='*' element={<AccountsPage />}></Route>
                                </Routes>
                            </ServerContextProvider>
                        </GroupsContextProvider>
                    </AccountsContextProvider>
                </Sidebar>
            </Router>
        </Box>
    );
}

export default MainRouter;