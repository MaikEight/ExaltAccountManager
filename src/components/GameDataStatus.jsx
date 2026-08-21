import { Alert, Box, Button, CircularProgress, CssBaseline, Typography } from "@mui/material";
import { ThemeProvider as MuiThemeProvider } from "@mui/material/styles";
import { ColorContext } from "eam-commons-js";
import { useContext } from "react";

/**
 * These screens render as siblings of MainProviders, which is what supplies the
 * MUI theme for the rest of the app, so they provide their own.
 */
function ThemedRoot({ children }) {
    const colorContext = useContext(ColorContext);

    return (
        <MuiThemeProvider theme={colorContext.theme}>
            <CssBaseline enableColorScheme />
            {children}
        </MuiThemeProvider>
    );
}

/**
 * Shown until the game-data manifest has been resolved. The application's item,
 * player-stat and fame-bonus data is populated asynchronously into module-level
 * objects, and components read those on mount, so nothing that consumes them
 * may render before the manifest is in place.
 */
export function GameDataLoadingScreen() {
    return (
        <ThemedRoot>
            <Box
                // The window has no decorations, so keep it draggable while the
                // title bar is not mounted yet.
                data-tauri-drag-region
                sx={{
                    position: 'fixed',
                    inset: 0,
                    display: 'flex',
                    flexDirection: 'column',
                    alignItems: 'center',
                    justifyContent: 'center',
                    gap: 2,
                    backgroundColor: 'background.default',
                }}
            >
                <img
                    src="/mascot/okta_low_res.png"
                    alt=""
                    height={96}
                    style={{ pointerEvents: 'none' }}
                />
                <CircularProgress size={28} />
                <Typography variant="body1" color="textSecondary">
                    Loading game data...
                </Typography>
            </Box>
        </ThemedRoot>
    );
}

/**
 * Reports that the manifest came from the local cache, or that no game data
 * could be loaded at all, and offers a retry.
 */
export function GameDataStatusToast({ status, onRetry }) {
    const isDegraded = status.state === 'degraded';

    return (
        <ThemedRoot>
            <Alert
                severity={isDegraded ? 'warning' : 'info'}
                variant="filled"
                title={status.message || undefined}
                action={
                    <Button color="inherit" size="small" onClick={onRetry}>
                        Retry
                    </Button>
                }
                sx={{
                    position: 'fixed',
                    right: '1rem',
                    bottom: '1rem',
                    zIndex: (theme) => theme.zIndex.snackbar,
                    maxWidth: '28rem',
                    alignItems: 'center',
                }}
            >
                {isDegraded
                    ? 'Game data is unavailable. Items will use question-mark placeholders.'
                    : 'Using cached game data while the asset service is unavailable.'}
            </Alert>
        </ThemedRoot>
    );
}
