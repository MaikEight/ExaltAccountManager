import { useTheme } from "@emotion/react";
import { ListItem, ListItemButton, ListItemIcon, ListItemText } from "@mui/material";

function SidebarButton({ menu, selected }) {
    const theme = useTheme();

    return (
        <ListItem
            disablePadding
        >
            <ListItemButton
                onClick={() => menu.action?.(menu.navigate)}
                sx={{
                    borderRadius: "0 30px 30px 0",
                    transition: theme.transitions.create(['color', 'background-color', 'background-image', 'box-shadow']),
                    ...(!selected ?
                        {
                            color: theme.palette.text.primary,
                            "&:hover": {
                                backgroundColor: theme.palette.mode === 'light' ? '#ECEDF3' : theme.palette.background.paper,
                            }
                        } : {
                            color: theme.palette.mode === 'light' ? theme.palette.background.default : theme.palette.text.primary,
                            backgroundImage: 'linear-gradient(98deg, rgb(198, 167, 254), rgb(145, 85, 253) 94%)',
                            boxShadow: theme.palette.mode === 'light'
                                ? 'rgba(58, 53, 65, 0.42) 0px 4px 8px -4px'
                                : 'rgba(19, 17, 32, 0.42) 0px 4px 8px -4px',
                            "&:hover": {
                                backgroundImage: 'linear-gradient(98deg, rgb(198, 167, 254), rgb(145, 85, 253) 94%)',
                            }
                        })
                }}
            >
                <ListItemIcon sx={{ marginLeft: 1, color: !selected ? theme.palette.text.primary : theme.palette.mode === 'light' ? theme.palette.background.default : theme.palette.text.primary }}>
                    {menu.icon}
                </ListItemIcon>
                <ListItemText primary={menu.name} sx={{color: !selected ? theme.palette.text.primary : theme.palette.mode === 'light' ? theme.palette.background.default : theme.palette.text.primary}} />
            </ListItemButton>
        </ListItem>
    );
}

export default SidebarButton;