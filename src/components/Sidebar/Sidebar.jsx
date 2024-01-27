import { Badge, Box, IconButton, List, Tooltip } from "@mui/material";
import GroupOutlinedIcon from '@mui/icons-material/GroupOutlined';
import NewspaperOutlinedIcon from '@mui/icons-material/NewspaperOutlined';
import SettingsOutlinedIcon from '@mui/icons-material/SettingsOutlined';
import InfoOutlinedIcon from '@mui/icons-material/InfoOutlined';
import SidebarButton from "./SidebarButton";
import { useEffect, useState } from "react";
import CustomToolbar from "./CustomToolbar";
import SideBarLogo from "./SideBarLogo";
import { useNavigate } from "react-router-dom";
import SystemUpdateAltOutlinedIcon from '@mui/icons-material/SystemUpdateAltOutlined';
import useSnack from "../../hooks/useSnack";


function Sidebar({ children }) {
    const [selectedIndex, setSelectedIndex] = useState(0);
    const [isGameUpdateAvailable, setIsGameUpdateAvailable] = useState(false);

    const { showSnackbar } = useSnack();
    const navigate = useNavigate();

    useEffect(() => {
        const intervallId = setInterval(() => {
            const updateNeeded = localStorage.getItem("updateNeeded") === 'true';

            if (updateNeeded && !isGameUpdateAvailable) {
                setIsGameUpdateAvailable(true);
                return;
            }
            setIsGameUpdateAvailable(false);
        }, 1000);

        return () => {
            clearInterval(intervallId);
        };
    }, []);

    useEffect(() => {
        if (isGameUpdateAvailable)
            showSnackbar('A new game update is available!');
    }, [isGameUpdateAvailable]);

    const menuItems = [
        {
            name: 'Accounts',
            icon: <GroupOutlinedIcon />,
            action: () => setSelectedIndex(0),
            navigate: '/accounts',
            showInFooter: false
        },
        {
            name: 'Realm Updater',
            icon: (
                <Badge badgeContent='' overlap="circular" color="error" variant="dot" invisible={!isGameUpdateAvailable}>
                    <SystemUpdateAltOutlinedIcon />
                </Badge>
            ),
            action: () => setSelectedIndex(1),
            navigate: '/gameUpdater',
            showInFooter: false
        },
        {
            name: 'Settings',
            icon: <SettingsOutlinedIcon />,
            action: () => setSelectedIndex(2),
            navigate: '/settings',
            showInFooter: false
        },
        {
            name: 'About',
            icon: <InfoOutlinedIcon />,
            action: () => setSelectedIndex(3),
            navigate: '/about',
            showInFooter: false
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
                flexDirection: 'column',
                userSelect: "none"
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
                        width: 230,
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
                                gap: 1,
                            }}
                        >
                            {
                                menuItems.map((menu, index) => (
                                    menu.showInFooter ? null :
                                        <SidebarButton
                                            key={index + menu.name}
                                            menu={menu}
                                            selected={selectedIndex === index}
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
                            width: 230,
                            ml: 2,
                            mb: 2,
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
                                                color: selectedIndex === index ? 'primary.main' : 'text.primary',
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
                <Box id="content" sx={{ display: 'flex', flex: '1 1 auto', maxWidth: 'calc(100% - 230px)' }}>
                    {children}
                </Box>
            </Box>
        </Box>
    );
}

export default Sidebar;