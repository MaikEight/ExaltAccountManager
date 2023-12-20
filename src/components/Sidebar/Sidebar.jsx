import { Box, List, Typography } from "@mui/material";
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
    const theme = useTheme();
    const colorContext = useContext(ColorContext);
    const navigate = useNavigate();

    const menuItems = [
        {
            name: 'Accounts',
            icon: <GroupOutlinedIcon />,
            action: () => setSelectedIndex(0),
            navigate: '/accounts'
        },
        // {
        //     name: 'News',
        //     icon: <NewspaperOutlinedIcon />,
        //     action: () => setSelectedIndex(1),
        //     navigate: '/news'
        // },
        {
            name: 'Realm Updater',
            icon: <SystemUpdateAltOutlinedIcon />,
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
            action: () => { setSelectedIndex(3); colorContext.toggleColorMode(); },
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
                <Box id="content" sx={{ flex: 1, width: 'calc(100% - 230px)' }}>
                    {children}
                </Box>
            </Box>
        </Box>
    );
}

export default Sidebar;