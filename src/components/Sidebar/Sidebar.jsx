import { Badge, Box, IconButton, List, Tooltip } from "@mui/material";
import GroupOutlinedIcon from '@mui/icons-material/GroupOutlined';
import SettingsOutlinedIcon from '@mui/icons-material/SettingsOutlined';
import InfoOutlinedIcon from '@mui/icons-material/InfoOutlined';
import SidebarButton from "./SidebarButton";
import { useEffect, useState } from "react";
import CustomToolbar from "./CustomToolbar";
import SideBarLogo from "./SideBarLogo";
import { useLocation, useNavigate } from "react-router-dom";
import useSnack from "../../hooks/useSnack";
import HandymanOutlinedIcon from '@mui/icons-material/HandymanOutlined';
import FeedbackButton from "./FeedbackButton";
import HistoryEduOutlinedIcon from '@mui/icons-material/HistoryEduOutlined';
import CalendarMonthOutlinedIcon from '@mui/icons-material/CalendarMonthOutlined';
import DiscordButton from "./DiscordButton";
import VaultPeekerLogo from "../VaultPeekerLogo";
import { useTheme } from "@emotion/react";

function Sidebar({ children }) {
    const [isGameUpdateAvailable, setIsGameUpdateAvailable] = useState(false);
    const [isHovered, setIsHovered] = useState(false);
    const { showSnackbar } = useSnack();
    const navigate = useNavigate();
    const location = useLocation();
    const theme = useTheme();

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

    const handleNavigate = (nav) => {
        console.log(nav);
        navigate(nav);
    };

    const menuItems = [
        {
            name: 'Accounts',
            icon: <GroupOutlinedIcon />,
            action: handleNavigate,
            navigate: '/accounts',
            additionalPaths: ['/'],
            showInFooter: false
        },
        {
            name: 'Vault Peeker',
            icon: <VaultPeekerLogo sx={{ml: '2px', mt: '3px', width: '20px'}} color={theme.palette.text.primary} />,
            action: handleNavigate,
            navigate: '/vaultPeeker',
            showInFooter: false
        },
        {
            name: 'Daily Logins',
            icon: <CalendarMonthOutlinedIcon />,
            action: handleNavigate,
            navigate: '/dailyLogins',
            showInFooter: false
        },
        {
            name: 'Utilities',
            icon: (
                <Badge badgeContent='' overlap="circular" color="error" variant="dot" invisible={!isGameUpdateAvailable}>
                    <HandymanOutlinedIcon />
                </Badge>
            ),
            action: handleNavigate,
            navigate: '/utilities',
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
            name: 'Logs',
            icon: <HistoryEduOutlinedIcon />,
            action: handleNavigate,
            navigate: '/logs',
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
                        <FeedbackButton
                            smallSize={isHovered}
                        />
                        <a
                            href="https://discord.exalt-account-manager.eu"
                            target="_blank"
                            rel="noopener noreferrer"
                        >
                            <DiscordButton
                                isHovered={isHovered}
                                setIsHovered={setIsHovered}
                                action={() => {
                                    // setSelectedIndex(-1);
                                }}
                            />
                        </a>
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