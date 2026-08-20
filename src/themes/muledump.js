import { createTheme } from '@mui/material/styles';
import { darkTheme } from 'eam-commons-js';

export const DEFAULT_COLOR_SCHEME = 'default';
export const MULEDUMP_COLOR_SCHEME = 'muledump';

// Semantic mapping of Muledump's BSD-3-Clause palette into EAM's MUI theme.
// Source: https://github.com/TadusPro/muledump/blob/master/lib/muledump/dump.css
export const muledumpTheme = createTheme(darkTheme, {
    palette: {
        mode: 'dark',
        primary: {
            main: '#00CEFF',
            light: '#90CCFF',
            dark: '#0F6BB1',
            contrastText: '#212121',
            gradient: 'linear-gradient(98deg, #3B86B4, #253C63 94%)',
        },
        secondary: {
            main: '#253C63',
            light: '#3B86B4',
            dark: '#162A41',
            full: '#253C63',
            contrastText: '#FFFFFF',
        },
        info: {
            main: '#90CCFF',
            light: '#DCF7FE',
            dark: '#3B86B4',
            contrastText: '#212121',
        },
        success: {
            main: '#39EF4E',
            light: '#99F7A4',
            dark: '#2E862E',
            contrastText: '#212121',
        },
        warning: {
            main: '#FCDF00',
            light: '#FEFE8E',
            dark: '#DD7700',
            contrastText: '#212121',
        },
        error: {
            main: '#DA281E',
            light: '#FF9090',
            dark: '#A82F27',
            contrastText: '#FFFFFF',
        },
        text: {
            primary: '#CCCCCC',
            secondary: '#B3B3B3',
            disabled: '#707070',
        },
        background: {
            default: '#212121',
            paper: '#2C2C2C',
            paperLight: '#333333',
            backdrop: 'rgba(0, 0, 0, 0.7)',
        },
        divider: '#545454',
        action: {
            active: '#CCCCCC',
            hover: 'rgba(37, 60, 99, 0.65)',
            selected: 'rgba(0, 206, 255, 0.18)',
            disabled: '#707070',
            disabledBackground: 'rgba(112, 112, 112, 0.18)',
            focus: 'rgba(0, 206, 255, 0.22)',
        },
        DataGrid: {
            bg: '#2C2C2C',
            pinnedBg: '#333333',
            headerBg: '#333333',
        },
        vaultPeeker: {
            itemSlotImage: '/realm/itemSlot_muledump.svg',
        },
    },
    shadows: darkTheme.shadows.map((shadow, index) => (
        index === 0 ? 'none' : 'rgba(0, 0, 0, 0.35) 0px 2px 10px 0px'
    )),
});
