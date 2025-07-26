import { useMemo } from 'react';
import { useTheme } from "@emotion/react";

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

    return index !== undefined ? colorList[index >= colorList.length ? index % colorList.length : index] : colorList;
};

export { useColorList };
