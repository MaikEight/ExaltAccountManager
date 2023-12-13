import { createTheme } from "@mui/material";

export const darkTheme = createTheme({
    palette: {
        mode: "dark",
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
            primary: '#E7E3FCDE',
            secondary: '#E7E3FCDE',
        },
        background: {
            default: '#28243D',
            paper: '#312D4B',
            paperLight: '#383452'
        },
    },
    shadows: [
        "none",
        "rgba(19, 17, 32, 0.1) 0px 2px 10px 0px"
    ]
});