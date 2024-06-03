import { createTheme } from "@mui/material";
import Zoom from '@mui/material/Zoom';

export const lightTheme = createTheme({
    palette: {
        mode: "light",
        primary: {
            main: '#9155FD',
        },
        secondary: {
            main: '#6e6978',
            full: '#6e6978',
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
            paperLight: '#F9FAFC'
        },
    },
    shape: {
        borderRadius: 6,
    },
    components: {
        MuiTooltip: {
            defaultProps: {
                placement: "bottom",                
                TransitionComponent:Zoom,
                slotProps: {
                    popper: {
                        modifiers: [
                            {
                                name: 'offset',
                                options: {
                                    offset: [0, -8],
                                },
                            },
                        ],
                    },
                },
            },
        },
    },
    shadows: [
        "none",
        "rgba(58, 53, 65, 0.1) 0px 2px 10px 0px",
        "rgba(58, 53, 65, 0.1) 0px 2px 10px 0px",
        "rgba(58, 53, 65, 0.1) 0px 2px 10px 0px",
        "rgba(58, 53, 65, 0.1) 0px 2px 10px 0px",
        "rgba(58, 53, 65, 0.1) 0px 2px 10px 0px",
        "rgba(58, 53, 65, 0.1) 0px 2px 10px 0px",
        "rgba(58, 53, 65, 0.1) 0px 2px 10px 0px",
        "rgba(58, 53, 65, 0.1) 0px 2px 10px 0px",
        "rgba(58, 53, 65, 0.1) 0px 2px 10px 0px",
        "rgba(58, 53, 65, 0.1) 0px 2px 10px 0px",
    ]
});