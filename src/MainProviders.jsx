import { CssBaseline } from "@mui/material";
import { ThemeProvider as MuiThemeProvider, styled } from "@mui/material/styles";
import { ColorContext } from "eam-commons-js";
import { useContext, useEffect } from "react";
import { MaterialDesignContent, SnackbarProvider, useSnackbar } from "notistack";
import InfoOutlinedIcon from '@mui/icons-material/InfoOutlined';
import IconButton from '@mui/material/IconButton';
import CloseIcon from '@mui/icons-material/Close';
import MainRouterRoutes from "./MainRouter";
import useUserSettings from "./hooks/useUserSettings";
import {
    DEFAULT_COLOR_SCHEME,
    MULEDUMP_COLOR_SCHEME,
    muledumpTheme,
} from "./themes/muledump";

const getSnackbarStyles = (theme) => ({
    borderRadius: theme.shape.borderRadius,
    '&.notistack-MuiContent-default': {
        backgroundColor: theme.palette.secondary.full,
    },
    '&.notistack-MuiContent-success': {
        backgroundColor: theme.palette.primary.main,
    },
    '&.notistack-MuiContent-error': {
        backgroundColor: (theme.palette.mode === 'dark' ? theme.palette.error.dark : theme.palette.error.main),
    },
    '&.notistack-MuiContent-message': {
        display: 'flex',
        width: 'fit-content',
        height: 'fit-content',
        gap: '8px',
        padding: '8px 16px',
        alignItems: 'center',
        justifyContent: 'center',
        backgroundColor: (theme.palette.mode === 'dark' ? theme.palette.secondary.full : theme.palette.secondary.main),
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

function MainProviders() {
    const colorContext = useContext(ColorContext);
    const userSettings = useUserSettings();
    const colorScheme = userSettings.getByKeyAndSubKey("general", "colorScheme")
        || DEFAULT_COLOR_SCHEME;
    const theme = colorScheme === MULEDUMP_COLOR_SCHEME
        ? muledumpTheme
        : colorContext.theme;

    useEffect(() => {
        document.body.classList.toggle('dark-theme', theme.palette.mode === 'dark');
        document.documentElement.setAttribute('data-color-scheme', colorScheme);
    }, [colorScheme, theme.palette.mode]);

    return (
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
                        message: StyledMaterialDesignContent,
                    }}
                    action={CloseAction}
                    anchorOrigin={{
                        vertical: 'top',
                        horizontal: 'center',
                    }}
                >
                    <MainRouterRoutes />
                </SnackbarProvider>
            </MuiThemeProvider>
    );
}

export default MainProviders;
