import { useTheme } from "@emotion/react";
import { Button, LinearProgress } from "@mui/material";

function StyledButton({ children, loading, ...props }) {
    const theme = useTheme();
    return (
        <Button
            color="primary"
            variant="contained"
            {...props}
            sx={{
                borderRadius: `${theme.shape.borderRadius}px`,
                boxShadow: theme.palette.mode === 'dark' ? 'rgba(19, 17, 32, 0.42) 0px 4px 8px -4px;' : 'rgba(58, 53, 65, 0.42) 0px 4px 8px -4px;',
                transition: theme.transitions.create('background-color'),
                overflow: 'hidden',
                ...props.sx,
            }}
            disabled={loading || props.disabled}
        >
            {loading && <LinearProgress sx={{ position: 'absolute', top: 0, left: 0, right: 0 }} />}
            {children}
        </Button>
    );
}

export default StyledButton;