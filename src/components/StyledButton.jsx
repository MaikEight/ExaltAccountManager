import { useTheme } from "@emotion/react";
import { Button } from "@mui/material";

function StyledButton({ children, fullWidth, ...props }) {
    const theme = useTheme();
    return (
        <Button
            color="primary"
            variant="contained"
            fullWidth={fullWidth}
            {...props}
            sx={{
                borderRadius: `${theme.shape.borderRadius}px`,                
                boxShadow: theme.palette.mode === 'dark' ? 'rgba(19, 17, 32, 0.42) 0px 4px 8px -4px;' : 'rgba(58, 53, 65, 0.42) 0px 4px 8px -4px;',
                ...props.sx,
            }}
        >
            {children}
        </Button>
    );
}

export default StyledButton;