import { Box } from "@mui/material";
import { useEffect, useState } from "react";
import { portrait } from "../../utils/portraitUtils";

const CharacterPortrait = (type, skin, tex1, tex2, adjust) => {
    const [url, setUrl] = useState('');

    useEffect(() => {
        const _url = portrait(type, skin, tex1, tex2, adjust);
        console.log('url:', _url);
        setUrl(_url);
    }, [type, skin, tex1, tex2, adjust]);

    return (
        <img src={url} id="characterPortrait" alt="Character Portrait" />
    );
};

function Character({ character }) {

    return (
        <Box
            sx={{
                display: 'flex',
                flexDirection: 'column',
                m: 1,
            }}
        >
            {/* Character Data */}
            <Box sx ={{display: 'flex', flexDirection: 'row', gap: 1}}>
                {CharacterPortrait(29816, 29816, 167772208, 33554431, false)}
                {CharacterPortrait(872, 872, 167772197, 167772197, false)}
            </Box>
        </Box>
    );
}

export default Character;