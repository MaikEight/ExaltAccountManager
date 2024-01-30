import { CssBaseline } from "@mui/material";
import { ThemeProvider as MuiThemeProvider } from "@mui/material/styles";
import { BrowserRouter as Router, Route, Routes } from 'react-router-dom';
import styled, { ThemeProvider as StyledThemeProvider } from "styled-components";
import Sidebar from "./components/Sidebar/Sidebar";
import ColorContext from "./contexts/ColorContext";
import { useContext } from "react";
import AccountsPage from "./pages/AccountsPage";
import GameUpdaterPage from "./pages/GameUpdaterPage";
import { GroupsContextProvider } from "./contexts/GroupsContext";
import SettingsPage from "./pages/SettingsPage";
import { ServerContextProvider } from "./contexts/ServerContext";
import AboutPage from "./pages/AboutPage";
import { AccountsContextProvider } from "./contexts/AccountsContext";
import { MaterialDesignContent, SnackbarProvider, useSnackbar } from "notistack";
import InfoOutlinedIcon from '@mui/icons-material/InfoOutlined';
import IconButton from '@mui/material/IconButton';
import CloseIcon from '@mui/icons-material/Close';

const getSnackbarStyles = (theme) => ({
    '&.notistack-MuiContent-default': {
        backgroundColor: theme.palette.secondary.full,
    },
    '&.notistack-MuiContent-success': {
        backgroundColor: theme.palette.primary.main,
    },
    '&.notistack-MuiContent-error': {
        backgroundColor: (theme.palette.mode === 'dark' ? theme.palette.error.dark : theme.palette.error.main),
    },
});

const StyledMaterialDesignContent = styled(MaterialDesignContent)(({ theme }) => getSnackbarStyles(theme));

const CloseAction = (key) => {
    const { closeSnackbar } = useSnackbar();

    return (
        <IconButton
            size="small"
            aria-label="close"
            color="inherit"
            onClick={() => closeSnackbar(key)}
        >
            <CloseIcon fontSize="small" />
        </IconButton>
    );
};

function MainRouter() {
    const colorContext = useContext(ColorContext);
    const theme = colorContext.theme;

    return (
        <StyledThemeProvider theme={theme}>
            <MuiThemeProvider theme={theme}>
                <CssBaseline enableColorScheme />
                <SnackbarProvider
                    iconVariant={{
                        default: <InfoOutlinedIcon size='small' sx={{ width: '20px', height: '20px', mr: 1 }} />,
                    }}
                    Components={{
                        default: StyledMaterialDesignContent,
                        success: StyledMaterialDesignContent,
                        error: StyledMaterialDesignContent,
                    }}
                    action={CloseAction}
                >
                    <div style={{ width: '100%' }}>
                        <Router id="router">
                            <Sidebar id="sidebar">
                                <AccountsContextProvider>
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
                                </AccountsContextProvider>
                            </Sidebar>
                        </Router>
                    </div>
                </SnackbarProvider>
            </MuiThemeProvider>
        </StyledThemeProvider >
    );
}

export default MainRouter;