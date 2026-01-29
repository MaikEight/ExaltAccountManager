import { Avatar, AvatarGroup, Box, Skeleton, Typography } from "@mui/material";
import { useEffect, useState } from "react";
import { portrait } from "../../../../utils/portraitUtils";

function CharacterPortraitAvatarGroup({ characters, renderAmount = 4 }) {
    if (!characters) {
        characters = [];
    } else if (!Array.isArray(characters) && typeof characters === 'object') {
        characters = [characters];
    }

    const [characterPortraits, setCharacterPortraits] = useState([renderAmount].fill(-1));

    useEffect(() => {
        console.log("Generating portraits for characters:", characters, renderAmount);
        const generatePortraits = async () => {
            const chars = [];
            for (let i = 0; i < characters.length && i < renderAmount; i++) {
                const char = characters[i];
                chars.push({
                    type: char.class,
                    skin: char.texture,
                    tex1: char.tex1,
                    tex2: char.tex2,
                    adjust: false,
                });
            }

            const portraits = await Promise.all(chars.map((char) => portrait(char.type, char.skin, char.tex1, char.tex2, char.adjust)));
            if (portraits.length < renderAmount) {
                for (let i = portraits.length; i < renderAmount; i++) {
                    portraits.push(null);
                }
            }
            setCharacterPortraits(portraits);
        }
        generatePortraits();
    }, [characters]);

    return (
        <>
            {
                characters.length === 1 ?
                    characterPortraits?.[0] && characterPortraits[0] !== -1 ?
                        <Avatar
                            src={characterPortraits?.[0] ?? null}
                            sx={{ width: 34, height: 34 }}
                            variant="square"
                        />
                        :
                        <Skeleton variant="rounded" width={34} height={34} />
                    :
                    <AvatarGroup
                        total={characters.length}
                        max={renderAmount + 1}
                        spacing="medium"
                        renderSurplus={(surplus) => (
                            <Avatar
                                key="surplus-avatar"
                                sx={{
                                    backgroundColor: (theme) => theme.palette.background.paper,
                                    pl: 0.35,
                                }}
                                variant="circular"
                            >
                                <Typography variant="subtitle2" sx={{ color: 'text.secondary' }}>
                                    +{surplus}
                                </Typography>
                            </Avatar>
                        )}
                        sx={{
                            height: 34,
                            '& .MuiAvatarGroup-avatar': {
                                width: 34,
                                height: 34,
                                backgroundColor: (theme) => theme.palette.background.paperLight,
                                border: 'none',
                            },
                        }}
                    >
                        {
                            characterPortraits.map((portrait, index) => {
                                if (portrait === -1) {
                                    return (
                                        <Box key={`skeleton-${index}`}>
                                            <Skeleton variant="rounded" width={34} height={34} />
                                        </Box>
                                    );
                                }
                                if (portrait) {
                                    return (
                                        <Avatar
                                            key={`avatar-${index}`}
                                            alt={`Character ${index + 1}`}
                                            src={portrait}
                                            sx={{ width: 34, height: 34 }}
                                            variant="square"
                                        />
                                    );
                                }
                                return null;
                            })
                        }
                    </AvatarGroup>
            }
        </>
    );
}

export default CharacterPortraitAvatarGroup;