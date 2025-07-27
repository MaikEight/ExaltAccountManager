import { useMemo } from 'react';
import { useTheme } from "@emotion/react";

/**
 * Custom hook to get a list of colors based on the provided index.
 * @param {number|string} index - The index or name of the color.
 * @returns {object[]} - An array of color objects.
 */
const useColorList = (index) => {
    const theme = useTheme();

    const colorList = useMemo(
        () => [
            { background: '#9155fd1f', color: theme.palette.primary.main },
            { background: '#16b1ff1f', color: theme.palette.info.main },
            { background: '#56ca0021', color: theme.palette.success.main },
            { background: '#ff4c511f', color: theme.palette.error.main },
            { background: '#ffb4001f', color: theme.palette.warning.main },
        ],
        [theme]
    );

    if (index === 'secondary') {
        return { background: '#8a8d931f', color: '#8a8d931f' };
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
