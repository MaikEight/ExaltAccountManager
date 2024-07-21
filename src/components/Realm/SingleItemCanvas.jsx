import { useEffect, useState } from "react";
import { drawItem } from "../../utils/realmItemDrawUtils";

function SingleItemCanvas({ item }) {
    const [imageData, setImageData] = useState(null);

    useEffect(() => {
        drawItem("renders.png", item, (imageUrl) => {
            setImageData(imageUrl);
        });
    }, [item]);

    if (!imageData) return null;
    return (<img src={imageData} width={50} height={50} id="itemImage" alt="item Image" />)
}

export default SingleItemCanvas;