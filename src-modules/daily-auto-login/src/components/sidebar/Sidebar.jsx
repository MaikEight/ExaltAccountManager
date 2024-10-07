import { useTheme } from "@emotion/react";
import { Box, IconButton, List, Tooltip } from "@mui/material";
import CustomToolbar from "./CustomToolbar";
import SideBarLogo from "./SideBarLogo";
import SidebarButton from "./SidebarButton";
import DashboardOutlinedIcon from '@mui/icons-material/DashboardOutlined';
import { useState } from "react";
import { useLocation, useNavigate } from "react-router-dom";
import InfoOutlinedIcon from '@mui/icons-material/InfoOutlined';
import SettingsOutlinedIcon from '@mui/icons-material/SettingsOutlined';

function Sidebar({ children }) {
    const theme = useTheme();
    const navigate = useNavigate();
    const location = useLocation();

    const [isHovered, setIsHovered] = useState(false);    

    const handleNavigate = (nav) => {
        console.log(nav);
        navigate(nav);
    };

    const menuItems = [
        {
            name: 'Dashboard',
            icon: <DashboardOutlinedIcon />,
            action: handleNavigate,
            navigate: '/dashboard',
            additionalPaths: ['/'],
            showInFooter: false
        },
        {
            name: 'Settings',
            icon: <SettingsOutlinedIcon />,
            action: handleNavigate,
            navigate: '/settings',
            showInFooter: false
        },
        {
            name: 'About',
            icon: <InfoOutlinedIcon />,
            action: handleNavigate,
            navigate: '/about',
            showInFooter: false
        },
    ];

    return (
        <Box
            sx={{
                display: "flex",
                flexDirection: 'column',
                userSelect: "none"
            }}>
            <CustomToolbar
                sx={{
                    width: "100%",
                    minHeight: 35,
                }}
                theme={theme}
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
                    data-tauri-drag-region
                    id="sidebar"
                    sx={{
                        position: "relative",
                        top: -22,
                        left: 0,
                        display: "flex",
                        flexDirection: "column",
                        alignItems: "left",
                        justifyContent: "space-between",
                        height: "calc(100% + 22px)",
                        width: 210,
                    }}
                >
                    <Box>
                        <SideBarLogo />

                        {/* Menu Items */}
                        <List
                            sx={{
                                mt: 1,
                                display: "flex",
                                width: 210,
                                flexDirection: "column",
                                gap: 0.5,
                            }}
                        >
                            {
                                menuItems.map((menu, index) => (
                                    menu.showInFooter ? null :
                                        <SidebarButton
                                            key={index + menu.name}
                                            menu={menu}
                                            selected={menu.navigate === location.pathname || menu.additionalPaths?.includes(location.pathname)}
                                        />
                                ))
                            }
                        </List>
                    </Box>

                    {/* Footer */}
                    <Box
                        sx={{
                            display: "flex",
                            flexDirection: "row",
                            alignItems: "start",
                            justifyContent: "start",
                            gap: 2,
                            width: 210,
                            ml: 2,
                            mb: 2,
                            mr: 2,
                        }}
                    >                        
                        {
                            menuItems.map((menu, index) => (
                                !menu.showInFooter ? null :
                                    <Tooltip title={menu.name} key={index + menu.name}>
                                        <IconButton
                                            key={index + menu.name}
                                            onClick={menu.action}
                                            sx={{
                                                color: menu.navigate === location.pathname ? 'primary.main' : 'text.primary',
                                                transition: 'color 0.2s ease-in-out',
                                            }}
                                        >
                                            {menu.icon}
                                        </IconButton>
                                    </Tooltip>
                            ))
                        }
                    </Box>
                </Box>
                {/* CONTENT */}
                <Box id="content" sx={{ display: 'flex', flex: '1 1 auto', maxWidth: 'calc(100% - 210px)' }}>
                    {children}
                </Box>
            </Box>
        </Box>
    );
}

export default Sidebar;