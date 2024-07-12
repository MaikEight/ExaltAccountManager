import { alpha, createTheme, darken, lighten } from "@mui/material";
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
        borderRadius: 9,
    },
    components: {
        MuiTooltip: {
            defaultProps: {
                placement: "bottom",
                TransitionComponent: Zoom,
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
        MuiDataGrid: {
            styleOverrides: {
                root: {
                    '& .MuiDataGrid-columnHeader:focus, & .MuiDataGrid-cell': {
                        outline: 'none',
                        height: '42px',
                        display: 'flex',
                        alignItems: 'center',
                    },
                    '& .MuiDataGrid-cell:focus-within, & .MuiDataGrid-cell:focus': {
                        outline: 'none',
                    },
                    '& .css-1oudwrl::after': {
                        height: 0,
                    },
                },
            },
            defaultProps: {
                sx: {
                    minHeight: '200px',
                    width: '100%',
                    border: 0,
                    '& [class^=MuiDataGrid]': {
                        border: 'none'
                    },
                    '& .MuiDataGrid-columnHeaders': {
                        backgroundColor: 'background.paperLight',
                    },
                    '& .MuiDataGrid-columnHeader': {
                        backgroundColor: 'background.paperLight',
                    },
                    '& .MuiDataGrid-scrollbarFiller, .MuiDataGrid-scrollbarFiller--header': {
                        backgroundColor: 'background.paperLight',
                    },
                    '& [class^=MuiDataGrid]::-webkit-scrollbar': {
                        backgroundColor: 'background.paper',
                    },
                    '& [class^=MuiDataGrid]::-webkit-scrollbar-track': {
                        borderRadius: theme => theme.shape.borderRadius,
                    },
                    '& [class^=MuiDataGrid]::-webkit-scrollbar-thumb': {
                        backgroundColor: theme => theme.palette.mode === 'dark'
                            ? theme.palette.background.default
                            : darken(theme.palette.background.default, 0.15),
                        border: theme => `3px solid ${theme.palette.background.paper}`,
                        borderRadius: theme => theme.shape.borderRadius,
                    },
                    '& [class^=MuiDataGrid]::-webkit-scrollbar-thumb:hover': {
                        backgroundColor: theme => theme.palette.mode === 'dark'
                            ? lighten(theme.palette.background.default, 0.12)
                            : darken(theme.palette.background.default, 0.2),
                        border: theme => `3px solid ${theme.palette.background.paper}`,
                        borderRadius: theme => theme.shape.borderRadius,
                    },
                    '& .css-tgsonj': {
                        borderTop: 'none',
                    }
                },
            },
        },
        MuiSelect: {
            defaultProps: {
                sx: {
                    height: 39,
                    backgroundColor: theme => alpha(theme.palette.background.default, 0.5),
                    '&:hover': {
                        backgroundColor: theme => alpha(theme.palette.common.white, 0.08),
                    },

                    //             {
                    // backgroundColor: theme =>alpha(theme.palette.background.default, 0.75),
                    // '&:hover': {
                    //     backgroundColor:theme => alpha(theme.palette.text.primary, 0.05),
                    // },            
                    transition: theme => theme.transitions.create('background-color'),
                    borderRadius: theme => `${theme.shape.borderRadius}px`,
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