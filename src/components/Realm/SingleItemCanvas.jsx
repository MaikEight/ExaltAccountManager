import { useEffect, useState } from "react";
import { drawItem } from "../../utils/realmItemDrawUtils";

function SingleItemCanvas({ item, doTransition = false }) {
    const [imageData, setImageData] = useState(null);

    useEffect(() => {
        drawItem("renders.png", item, (imageUrl) => {
            setImageData(imageUrl);
        });
    }, [item]);

    if(!imageData && !doTransition) {
        return null;        
    }

    return (
        <img
            src={imageData}
            width={50}
            height={50}
            id="itemImage"
            alt="item Image"
            style={{
                scale: imageData ? 1 : 0,
                transition: doTransition ? "scale 0.2s ease-in-out" : "none",
            }}
        />
    )
}

export default SingleItemCanvas;