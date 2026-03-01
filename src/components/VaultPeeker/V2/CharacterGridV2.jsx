import { useState, useMemo } from "react";
import { Box, Typography, Collapse, IconButton } from "@mui/material";
import KeyboardArrowDownIcon from '@mui/icons-material/KeyboardArrowDown';
import KeyboardArrowUpIcon from '@mui/icons-material/KeyboardArrowUp';
import CharacterCardV2 from "./CharacterCardV2";
import useVaultPeeker from "../../../hooks/useVaultPeeker";

/**
 * CharacterGridV2 - A responsive wrapping grid of character cards
 * 
 * @param {Object} props
 * @param {Array} props.characters - Array of character objects
 * @param {Function} props.onItemClick - Callback when an item is clicked
 */
function CharacterGridV2({ characters = [], email, onItemClick }) {
    const { filter } = useVaultPeeker();
    const [collapsed, setCollapsed] = useState(false);

    // Filter characters based on current filter state
    const visibleCharacters = useMemo(() => {
        if (!characters?.length) return [];
        
        // If filter is "not on character", hide all characters
        if (filter.characterType.enabled && filter.characterType.value === 3) {
            return [];
        }

        return characters.filter((char) => {
            if (!filter.characterType.enabled) return true;
            const value = filter.characterType.value;
            if (value === 0) return true; // All
            if (value === 1) return char.seasonal; // Seasonal only
            if (value === 2) return !char.seasonal; // Normal only
            return true;
        });
    }, [characters, filter.characterType]);

    if (!visibleCharacters.length) {
        return null;
    }

    return (
        <Box
            sx={{
                display: 'flex',
                flexDirection: 'column',
                gap: 1,
            }}
        >
            <Box
                onClick={() => setCollapsed(!collapsed)}
                sx={{
                    display: 'flex',
                    alignItems: 'center',
                    cursor: 'pointer',
                    userSelect: 'none',
                    '&:hover': {
                        opacity: 0.8,
                    },
                    ml: 0.75,
                }}
            >
                <IconButton size="small" sx={{ p: 0, mr: 0.5 }}>
                    {collapsed ? <KeyboardArrowDownIcon fontSize="small" /> : <KeyboardArrowUpIcon fontSize="small" />}
                </IconButton>
                <Typography variant="subtitle2" fontWeight="bold">
                    Characters ({visibleCharacters.length})
                </Typography>
            </Box>
            <Collapse in={!collapsed} timeout="auto">
                <Box
                    sx={{
                        display: 'flex',
                        flexDirection: 'row',
                        flexWrap: 'wrap',
                        gap: 1,
                        justifyContent: 'center',
                    }}
                >
                    {visibleCharacters.map((character) => (
                        <CharacterCardV2
                            key={character.char_id}
                            character={character}
                            email={email}
                            onItemClick={onItemClick}
                        />
                    ))}
                </Box>
            </Collapse>
        </Box>
    );
}

export default CharacterGridV2;
