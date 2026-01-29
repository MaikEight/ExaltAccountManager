import { Box } from "@mui/material";

function EamToggle({ iconLeft, iconRight, isToggled, onToggle }) {

    const getIconToggle = (icon, toggled) => {

        if (!toggled) {
            return (
                <Box
                    sx={{
                        display: 'flex',
                        alignItems: 'center',
                        justifyContent: 'center',
                        border: (theme) => `1px solid transparent`,
                        width: '100%',
                        height: '100%',
                        px: 1.5,
                    }}
                >
                    {icon}
                </Box>
            );
        }

        return (
            <Box
                sx={{
                    display: 'flex',
                    alignItems: 'center',
                    justifyContent: 'center',
                    borderRadius: (theme) => `${theme.shape.borderRadius - 2}px`,
                    border: (theme) => `1px solid ${theme.palette.divider}`,
                    width: '100%',
                    height: '100%',
                    px: 1.5,
                    boxShadow: (theme) => `0px 1px 0.75px 0px ${theme.palette.mode === 'dark' ? 'rgba(0,0,0,0.2)' : 'rgba(0,0,0,0.05)'}, 0px 0.25px 0.25px 0px ${theme.palette.mode === 'dark' ? 'rgba(0,0,0,0.4)' : 'rgba(0,0,0,0.15)'}`,
                    color: (theme) => theme.palette.primary.main,
                }}
            >
                {icon}
            </Box>
        );
    }
    return (
        <Box
            sx={{
                borderRadius: 1,
                display: 'flex',
                alignItems: 'center',
                justifyContent: 'space-between',
                height: '30px',
                p: 0.25,
                cursor: 'pointer',
                boxShadow: (theme) => `0px 0px 2px 0px ${theme.palette.mode === 'dark' ? 'rgba(0,0,0,0.2)' : 'rgba(0,0,0,0.05)'} inset, 0px 0px 4px 0px ${theme.palette.mode === 'dark' ? 'rgba(0,0,0,0.2)' : 'rgba(0,0,0,0.05)'} inset, 0px 0px 2px 0px ${theme.palette.mode === 'dark' ? 'rgba(0,0,0,0.2)' : 'rgba(0,0,0,0.05)'} inset`,
                backgroundColor: (theme) => theme.palette.background.paper,

            }}
            onClick={onToggle}
        >
            {getIconToggle(iconLeft, !isToggled)}
            {getIconToggle(iconRight, isToggled)}
        </Box>
    );
}

export default EamToggle;