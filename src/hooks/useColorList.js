import { useMemo } from 'react';
import { useTheme } from "@emotion/react";
import { alpha } from '@mui/material/styles';

/**
 * Custom hook to get a list of colors based on the provided index.
 * @param {number|string} index - The index or name of the color.
 * @returns {object[]} - An array of color objects.
 */
const useColorList = (index) => {
    const theme = useTheme();

    const colorList = useMemo(
        () => [
            { background: alpha(theme.palette.primary.main, 0.12), color: theme.palette.primary.main },
            { background: alpha(theme.palette.info.main, 0.12), color: theme.palette.info.main },
            { background: alpha(theme.palette.success.main, 0.13), color: theme.palette.success.main },
            { background: alpha(theme.palette.error.main, 0.12), color: theme.palette.error.main },
            { background: alpha(theme.palette.warning.main, 0.12), color: theme.palette.warning.main },
        ],
        [theme]
    );

    if (index === 'secondary') {
        return {
            background: alpha(theme.palette.secondary.main, 0.12),
            color: theme.palette.secondary.main,
        };
    }

    return (
        index !== undefined ? (
            typeof index === 'number' ?
                colorList[index >= colorList.length ? index % colorList.length : index]
                : colorList[getIndexByName(index)]
        ) : colorList
    );
};

function getIndexByName(name) {
    const colorList = [
        'primary', 'info', 'success', 'error', 'warning'
    ];

    const index = colorList.indexOf(name);
    return index !== -1 ? index : 0; // Default to primary if not found
}

export { useColorList };
