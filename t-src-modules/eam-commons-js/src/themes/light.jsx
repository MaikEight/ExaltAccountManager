import { alpha, darken, lighten } from "@mui/material";
import { createTheme } from '@mui/material/styles';
import Zoom from '@mui/material/Zoom';
import { datagridStyle } from "./datagridStyle";

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
            paperLight: '#F9FAFC',
            backdrop: 'rgba(244, 245, 250, 0.75)',
        },
        ...datagridStyle("light"),
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
                    '& .css-1tdeh38': {
                        height: 0,
                        display: 'none',
                    }
                },
            },
            defaultProps: {
                sx: {
                    minHeight: '200px',
                    width: '100%',
                    border: 0,
                    '& [class^=MuiDataGrid]': {
                        border: 'none !important'
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
                },
            },
        },
        MuiSelect: {
            defaultProps: {
                sx: {
                    height: 39,                                
                    backgroundColor: theme =>alpha(theme.palette.background.default, 0.75),
                    '&:hover': {
                        backgroundColor:theme => alpha(theme.palette.text.primary, 0.05),
                    },
                    transition: theme => theme.transitions.create('background-color'),
                    borderRadius: theme => `${theme.shape.borderRadius}px`,
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