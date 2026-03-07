import { useTheme } from "@emotion/react";

function CharacterIcon({width = 16, height = 16}) {
    const theme = useTheme();

    return (
        <div
            role="img"
            aria-label="Character"
            style={{
                width,
                height,
                backgroundColor: theme.palette.text.primary,
                WebkitMaskImage: "url(/realm/character_outline.png)",
                WebkitMaskSize: "contain",
                WebkitMaskRepeat: "no-repeat",
                WebkitMaskPosition: "center",
                maskImage: "url(/realm/character_outline.png)",
                maskSize: "contain",
                maskRepeat: "no-repeat",
                maskPosition: "center",
            }}
        />
    )
}

export default CharacterIcon;