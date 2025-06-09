import { createContext, useState } from "react";


const ItemCanvasContext = createContext();

function ItemCanvasContextProvider({ children }) {
    const [hoveredConvasId, setHoveredConvasId] = useState(null);
    const [hoveredConvasData, setHoveredConvasData] = useState(null);

    const contextValue = {
        hoveredConvasId: hoveredConvasId,
        setHoveredConvasId: (id) => setHoveredConvasId(id),
        setHoveredConvasData: (data) => {
            if (!data && hoveredConvasData) {
                hoveredConvasData.callback?.();
                setHoveredConvasData(null);
                return;
            }

            if (data?.canvasId === hoveredConvasData?.canvasId) {
                return;
            }

            hoveredConvasData?.callback?.();
            setHoveredConvasData(data);
        },
    };

    return (
        <ItemCanvasContext.Provider value={contextValue}>
            {children}
        </ItemCanvasContext.Provider>
    );
}

export default ItemCanvasContext;
export { ItemCanvasContextProvider };