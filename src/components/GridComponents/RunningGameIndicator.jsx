import { Box, Tooltip } from "@mui/material";
import { alpha, useTheme } from "@mui/material/styles";
import useRunningGames from "../../hooks/useRunningGames";

/**
 * Small pulsing green dot shown next to an account whose game is currently
 * running. Subscribes to the running-games context itself so the (memoized)
 * DataGrid column definitions don't need to depend on the running state.
 */
function RunningGameIndicator({ email }) {
    const theme = useTheme();
    const { isAccountRunning } = useRunningGames();

    if (!isAccountRunning(email)) return null;

    const color = theme.palette.success.main;

    return (
        <Tooltip title="Game is running">
            <Box
                component="span"
                sx={{
                    flexShrink: 0,
                    width: 9,
                    height: 9,
                    borderRadius: '50%',
                    backgroundColor: color,
                    animation: 'runningGamePulse 2.0s ease-in-out infinite',
                    '@keyframes runningGamePulse': {
                        '0%': { boxShadow: `0 0 0 0 ${alpha(color, 0.55)}` },
                        '70%': { boxShadow: `0 0 0 6px ${alpha(color, 0)}` },
                        '100%': { boxShadow: `0 0 0 0 ${alpha(color, 0)}` },
                    },
                }}
            />
        </Tooltip>
    );
}

export default RunningGameIndicator;
