import { createTheme } from "@mui/material";

export const lightTheme = createTheme({
    palette: {
        mode: "light",
        primary: {
            main: '#9155FD;',
        },
        secondary: {
            main: '#384392',
        },
        info: {
            main: '#16B1FF',
        },
        success: {
            main: '#56CA00',
        },
        warning: {
            main: '#FFB400',
        },
        error: {
            main: '#FF4C51',
        },
        text: {
            primary: '#3A3541DE',
            secondary: '#3A3541DE',
        },
        background: {
            default: '#F4F5FA',
            paper: '#FFFFFF',
            paperLight: '#F7F7F8'
        },
    },
});