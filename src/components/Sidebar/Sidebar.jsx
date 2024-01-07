import { Badge, Box, List, Typography } from "@mui/material";
import GroupOutlinedIcon from '@mui/icons-material/GroupOutlined';
import NewspaperOutlinedIcon from '@mui/icons-material/NewspaperOutlined';
import SettingsOutlinedIcon from '@mui/icons-material/SettingsOutlined';
import InfoOutlinedIcon from '@mui/icons-material/InfoOutlined';
import SidebarButton from "./SidebarButton";
import { useContext, useEffect, useState } from "react";
import { useTheme } from '@mui/system';
import ColorContext from '../../contexts/ColorContext';
import CustomToolbar from "./CustomToolbar";
import SideBarLogo from "./SideBarLogo";
import { useNavigate } from "react-router-dom";
import SystemUpdateAltOutlinedIcon from '@mui/icons-material/SystemUpdateAltOutlined';


function Sidebar({ children }) {
    const [selectedIndex, setSelectedIndex] = useState(0);
    const [isGameUpdateAvailable, setIsGameUpdateAvailable] = useState(false);
    const theme = useTheme();
    
    const navigate = useNavigate();

    useEffect(() => {
        const updateNeeded = localStorage.getItem("updateNeeded");

        if (updateNeeded) {
            setIsGameUpdateAvailable(true);
            return;
        } setIsGameUpdateAvailable(false);
    }, [localStorage.getItem("updateNeeded")]);

    const menuItems = [
        {
            name: 'Accounts',
            icon: <GroupOutlinedIcon />,
            action: () => setSelectedIndex(0),
            navigate: '/accounts'
        },
        {
            name: 'Realm Updater',
            icon: ( 
                <Badge badgeContent='' overlap="circular" color="error" variant="dot" invisible={!isGameUpdateAvailable}>
                    <SystemUpdateAltOutlinedIcon />
                </Badge>
            ),
            action: () => setSelectedIndex(1),
            navigate: '/gameUpdater'
        },
        {
            name: 'Settings',
            icon: <SettingsOutlinedIcon />,
            action: () => setSelectedIndex(2),
            navigate: '/settings'
        },
        {
            name: 'About',
            icon: <InfoOutlinedIcon />,
            action: () => setSelectedIndex(3),
            navigate: '/about'
        },
    ];

    useEffect(() => {
        console.log(menuItems[selectedIndex].navigate);
        navigate(menuItems[selectedIndex].navigate);
    }, [selectedIndex]);

    return (
        <Box
            sx={{
                display: "flex",
                flexDirection: 'column'
            }}>
            <CustomToolbar
                sx={{
                    width: "100%",
                    minHeight: 35,
                }}
            />
            <Box
                sx={{
                    display: "flex",
                    flexDirection: "row",
                    height: "calc(100vh - 35px)",
                    width: "100%",
                    gap: 0,

                }}
            >
                {/* Sidebar */}
                <Box
                    sx={{
                        position: "relative",
                        top: -22,
                        left: 0,
                        flexDirection: "column",
                        height: "100%",
                        width: 230,
                    }}
                >
                    <SideBarLogo />

                    {/* Menu Items */}
                    <List
                        sx={{
                            mt: 1,
                            display: "flex",
                            width: 210,
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
                {/* CONTENT */}
                <Box id="content" sx={{ display: 'flex', flex: '1 1 auto', maxWidth: 'calc(100% - 230px)' }}>
                    {children}
                </Box>
            </Box>
        </Box>
    );
}

export default Sidebar;