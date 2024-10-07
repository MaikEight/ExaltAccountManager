import { BrowserRouter as Router, Route, Routes } from 'react-router-dom';
import { useTheme } from '@emotion/react';
import { Box } from '@mui/material';
import { ErrorBoundary } from 'react-error-boundary';
import Sidebar from './components/sidebar/Sidebar';
import DashboardPage from './pages/DashboardPage';
import { TaskProvider } from './contexts/TaskContext';

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
            <ErrorBoundary fallback={<div>Fatal Fail</div>}>
                <Router id="router">
                    <Sidebar id="sidebar">
                        <ErrorBoundary
                            fallback={<div>Fail</div>}
                            onError={(error, stack) => console.warn('ErrorBoundary', error, stack)}
                        >
                            <TaskProvider>
                                <Routes>
                                    <Route path='/' element={<DashboardPage />}></Route>
                                    <Route path='/dashboard' element={<DashboardPage />}></Route>
                                </Routes>
                            </TaskProvider>
                        </ErrorBoundary>
                    </Sidebar>
                </Router>
            </ErrorBoundary>
        </Box>
    );
}

export default MainRouter;