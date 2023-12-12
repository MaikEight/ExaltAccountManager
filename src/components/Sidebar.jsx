import { Box, List, ListItem, ListItemButton, ListItemIcon, ListItemText, Typography } from "@mui/material";
import GroupOutlinedIcon from '@mui/icons-material/GroupOutlined';
import NewspaperOutlinedIcon from '@mui/icons-material/NewspaperOutlined';
import SettingsOutlinedIcon from '@mui/icons-material/SettingsOutlined';
import InfoOutlinedIcon from '@mui/icons-material/InfoOutlined';
import SidebarButton from "./SidebarButton";
import { useState } from "react";

function Sidebar({ children }) {
    const [selectedIndex, setSelectedIndex] = useState(0);

    const menuItems = [
        {
            name: 'Accounts',
            icon: <GroupOutlinedIcon />,
            action: () => setSelectedIndex(0),
        },
        {
            name: 'News',
            icon: <NewspaperOutlinedIcon />,
            action: () => setSelectedIndex(1),
        },
        {
            name: 'Options',
            icon: <SettingsOutlinedIcon />,
            action: () => setSelectedIndex(2),
        },
        {
            name: 'About',
            icon: <InfoOutlinedIcon />,
            action: () => setSelectedIndex(3),
        },
    ];

    return (
        <Box
            sx={{
                display: "flex",
                width: "100%",
                height: "100%",
            }}
        >
            <Box
                sx={{
                    width: 200,
                    height: "100vh",
                    position: "fixed",
                    left: 0,
                }}
            >
                <Box
                    sx={{
                        display: "flex",
                        justifyContent: "start",
                        alignItems: "center",
                        height: 64,
                        paddingLeft: 2,
                        gap: 2,
                    }}
                >
                    <img
                        src='logo/logo.png'
                        alt="EAM-Logo"
                        style={{ width: 48, height: 48 }}
                    />
                    <Typography variant="h6" noWrap>
                        EAM
                    </Typography>
                </Box>

                <List
                    sx={{
                        display: "flex",
                        flexDirection: "column",
                        gap: 1,
                    }}
                >
                    {
                        menuItems.map((menu, index) => (
                            <SidebarButton
                                key={index + menu.name}
                                menu={menu}
                                selected={selectedIndex === index}
                            />
                        ))
                    }
                </List>
            </Box>
            <Box>
                {/* {children} */}
            </Box>
        </Box>
    );
}

export default Sidebar;