import { useEffect, useState } from "react";
import { portrait } from "../../utils/portraitUtils";
import { Skeleton } from "@mui/material";

function CharacterPortrait({type, skin, tex1, tex2, adjust}) {
    const [url, setUrl] = useState('');

    useEffect(() => {
        const _url = portrait(type, skin, tex1, tex2, adjust);
        setUrl(_url);
    }, [type, skin, tex1, tex2, adjust]);

    if(!url) {
        return (
            <Skeleton sx={{mx: '-1px'}} variant="rounded" width={45} height={60} />
        );
    }

    return (
        <img src={url} width={34} height={34} alt="Character Portrait" />
    );
}

export default CharacterPortrait;