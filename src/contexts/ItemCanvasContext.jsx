import { createContext, useState } from "react";


const ItemCanvasContext = createContext();

function ItemCanvasContextProvider({ children }) {
    const [hoveredConvasId, setHoveredConvasId] = useState(null);

    const contextValue = {
        hoveredConvasId: hoveredConvasId,
        setHoveredConvasId: (id) => setHoveredConvasId(id),
    };

    return (
        <ItemCanvasContext.Provider value={contextValue}>
            {children}
        </ItemCanvasContext.Provider>
    );
}

export default ItemCanvasContext;
export { ItemCanvasContextProvider };