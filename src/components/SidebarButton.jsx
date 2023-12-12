import { ListItem, ListItemButton, ListItemIcon, ListItemText } from "@mui/material";

function SidebarButton({ menu, selected }) {
    return (
        <ListItem
            disablePadding
        >
            <ListItemButton
                onClick={menu.action}
                sx={{
                    borderRadius: "0 30px 30px 0",   
                    ...(!selected ?
                        {
                            "&:hover": {
                                backgroundColor: '#2F2C45',
                            }
                        } : {
                            backgroundColor: '#9155FD',
                            "&:hover": {
                                backgroundColor: '#9155FD',
                            }
                        })
                }}
            >
                <ListItemIcon sx={{marginLeft: 1}}>
                    {menu.icon}
                </ListItemIcon>
                <ListItemText primary={menu.name} />
            </ListItemButton>
        </ListItem>
    );
}

export default SidebarButton;