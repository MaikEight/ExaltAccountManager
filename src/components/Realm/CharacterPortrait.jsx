import { useEffect, useState } from "react";
import { portrait } from "../../utils/portraitUtils";

function CharacterPortrait({type, skin, tex1, tex2, adjust}) {
    const [url, setUrl] = useState('');

    useEffect(() => {
        const _url = portrait(type, skin, tex1, tex2, adjust);
        setUrl(_url);
    }, [type, skin, tex1, tex2, adjust]);

    return (
        <img src={url} width={34} height={34} alt="Character Portrait" />
    );
}

export default CharacterPortrait;