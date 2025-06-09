import { useState } from "react";


function useItemCanvas() {
    const [hoveredConvasDataState, setHoveredConvasDataState] = useState(null);

    const setHoveredConvasData = (data) => {
        if (!data && hoveredConvasDataState) {
            hoveredConvasDataState.callback?.();
            setHoveredConvasDataState(null);
            return;
        }

        if (data?.canvasId === hoveredConvasDataState?.canvasId) {
            return;
        }

        hoveredConvasDataState?.callback?.();
        setHoveredConvasDataState(data);
    };

    return { setHoveredConvasData };
}

export default useItemCanvas;