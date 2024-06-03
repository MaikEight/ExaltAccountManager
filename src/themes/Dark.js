import { createTheme } from "@mui/material";
import Zoom from '@mui/material/Zoom';

export const darkTheme = createTheme({
    palette: {
        mode: "dark",
        primary: {
            main: '#9155FD',
        },
        secondary: {
            main: '#8a8d931f',
            full: '#343148',
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
            paperLight: '#3D3759'
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
        "rgba(19, 17, 32, 0.1) 0px 2px 10px 0px",
        "rgba(19, 17, 32, 0.1) 0px 2px 10px 0px",
        "rgba(19, 17, 32, 0.1) 0px 2px 10px 0px",
        "rgba(19, 17, 32, 0.1) 0px 2px 10px 0px",
        "rgba(19, 17, 32, 0.1) 0px 2px 10px 0px",
        "rgba(19, 17, 32, 0.1) 0px 2px 10px 0px",
        "rgba(19, 17, 32, 0.1) 0px 2px 10px 0px",
        "rgba(19, 17, 32, 0.1) 0px 2px 10px 0px",
        "rgba(19, 17, 32, 0.1) 0px 2px 10px 0px",
        "rgba(19, 17, 32, 0.1) 0px 2px 10px 0px",
    ]
});