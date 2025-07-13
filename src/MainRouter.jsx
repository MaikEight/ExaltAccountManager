import { BrowserRouter as Router, Route, Routes } from 'react-router-dom';
import Sidebar from "./components/Sidebar/Sidebar";
import AccountsPage from "./pages/AccountsPage";
import UtilitiesPage from "./pages/UtilitiesPage";
import { GroupsContextProvider } from "eam-commons-js";
import SettingsPage from "./pages/SettingsPage";
import { ServerContextProvider } from "./contexts/ServerContext";
import AboutPage from "./pages/AboutPage";
import { AccountsContextProvider } from "./contexts/AccountsContext";
import { useTheme } from '@emotion/react';
import { Box } from '@mui/material';
import { PopupContextProvider } from './contexts/PopupContext';
import FeedbackPage from './pages/FeedbackPage';
import LogsPage from './pages/LogsPage';
import DailyLoginsPage from './pages/DailyLoginsPage';
import ImporterPage from './pages/ImporterPage';
import { ErrorBoundary } from 'react-error-boundary';
import ErrorBoundaryFallback from './components/ErrorBoundaryFallback';
import VaultPeekerPage from './pages/VaultPeekerPage';
import FatalErrorPage from './pages/FatalErrorPage';
import DeepLinkingComponent from './components/DeepLinkingComponent';
import ProfilePage from './pages/ProfilePage';
import PaymentSuccessful from './pages/PaymentSuccessful';
import { DiscordContextProvider } from './contexts/DiscordContext';
import DebugFlagsPage from './pages/DebugFlagsPage';
import BackgroundSyncContext, { BackgroundSyncProvider } from './contexts/BackgroundSyncContext';

function MainRouter() {
    const theme = useTheme();

    return (
        <Box
            sx={{
                width: '100%',
                backgroundColor: theme.palette.background.default,
                transition: theme.transitions.create(['background-color', 'color']),
            }}
        >
            <ErrorBoundary fallback={<FatalErrorPage />}>
                <Router
                    id="router"
                    future={{
                        "v7_startTransition": true,
                        "v7_relativeSplatPath": true,
                    }}
                >
                    <ServerContextProvider>
                        <AccountsContextProvider>
                            <BackgroundSyncProvider>
                                <Sidebar id="sidebar">
                                    <GroupsContextProvider>
                                        <PopupContextProvider>
                                            <ErrorBoundary
                                                fallback={<ErrorBoundaryFallback />}
                                                onError={(error, stack) => console.warn('ErrorBoundary', error, stack)}
                                            >
                                                <DiscordContextProvider>
                                                    <DeepLinkingComponent />
                                                    <Routes>
                                                        <Route path='/' element={<AccountsPage />}></Route>
                                                        <Route path='/error' element={<FatalErrorPage />}></Route>
                                                        <Route path='/accounts' element={<AccountsPage />}></Route>
                                                        <Route path='/vaultPeeker' element={<VaultPeekerPage />}></Route>
                                                        <Route path='/dailyLogins' element={<DailyLoginsPage />}></Route>
                                                        <Route path='/utilities' element={<UtilitiesPage />}></Route>
                                                        <Route path='/settings' element={<SettingsPage />}></Route>
                                                        <Route path='/logs' element={<LogsPage />}></Route>
                                                        <Route path='/profile' element={<ProfilePage />}></Route>
                                                        <Route path='/about' element={<AboutPage />}></Route>
                                                        <Route path='/feedback' element={<FeedbackPage />}></Route>
                                                        <Route path='/importer' element={<ImporterPage />}></Route>
                                                        <Route path='/payment/successful' element={<PaymentSuccessful />}></Route>
                                                        <Route path='/flags' element={<DebugFlagsPage />}></Route>
                                                        <Route path='/debug' element={<DebugFlagsPage />}></Route>
                                                        <Route path='*' element={<AccountsPage />}></Route>
                                                    </Routes>
                                                </DiscordContextProvider>
                                            </ErrorBoundary>
                                        </PopupContextProvider>
                                    </GroupsContextProvider>
                                </Sidebar>
                            </BackgroundSyncProvider>
                        </AccountsContextProvider>
                    </ServerContextProvider>
                </Router>
            </ErrorBoundary>
        </Box>
    );
}

export default MainRouter;