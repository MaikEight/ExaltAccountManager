import { Box, Typography } from "@mui/material";
import CharacterPortrait from "../../../Realm/CharacterPortrait";
import { useEffect, useMemo, useState } from "react";
import { ADVENTURERS_BELT, BACKPACK_EXTENDER_ITEM_ID, BACKPACK_ITEM_ID, getXof8OfCharacter } from "../../../../utils/realmCharacterUtils";
import { getItemById } from "../../../../utils/realmItemUtils";
import { drawItemAsync } from "../../../../utils/realmItemDrawUtils";
import ItemGridV2 from "../../../VaultPeeker/V2/ItemGridV2";

function SingleCharacterOverview({ character, number, parsedItems }) {

    // Map parsed equipment items to ItemGridV2 format
    const equipmentItems = useMemo(() => {
        if (!parsedItems) return [];
        return parsedItems
            .filter((i) => i.container_index === 0)
            .sort((a, b) => (a.slot_index || 0) - (b.slot_index || 0))
            .map((item) => ({
                itemId: item.item_id,
                count: 1,
                maxRarity: item.enchant_ids?.length ? Math.min(4, item.enchant_ids.length) : 0,
                parsedItem: item,
            }));
    }, [parsedItems]);

    const xof8 = useMemo(() => getXof8OfCharacter(character), [character]);

    // Backpack / extender / belt indicator icons
    const [backpackIcons, setBackpackIcons] = useState({ backpack: null, extender: null, belt: null });

    useEffect(() => {
        let cancelled = false;
        const load = async () => {
            const results = {};
            if (character.backpack_slots > 0) {
                const item = getItemById(BACKPACK_ITEM_ID);
                if (item) results.backpack = await drawItemAsync("renders.png", item);
            }
            if (character.backpack_slots > 8) {
                const item = getItemById(BACKPACK_EXTENDER_ITEM_ID);
                if (item) results.extender = await drawItemAsync("renders.png", item);
            }
            if (character.has3_quickslots > 0) {
                const item = getItemById(ADVENTURERS_BELT);
                if (item) results.belt = await drawItemAsync("renders.png", item);
            }
            if (!cancelled) setBackpackIcons(prev => ({ ...prev, ...results }));
        };
        load();
        return () => { cancelled = true; };
    }, [character]);

    return (
        <Box
            sx={{
                display: 'flex',
                flexDirection: 'row',
                alignItems: 'center',
                justifyContent: 'flex-start',
                p: 0.5,
                gap: 1.5,
                width: 'fit-content',
            }}
        >
            <Typography variant="h6">{number}.</Typography>
            <CharacterPortrait type={character.class} skin={character?.texture} tex1={character?.tex1} tex2={character?.tex2} adjust={false} />
            {/* Equipment */}
            {equipmentItems.length > 0 && (
                <Box
                    sx={{
                        backgroundColor: theme => theme.palette.background.default,
                        borderRadius: 1,
                        p: 0.25,
                    }}
                >
                    <ItemGridV2
                        items={equipmentItems}
                        showCounts={false}
                        showEmptySlots={true}
                        showTooltips={true}
                        columns={4}
                    />
                </Box>
            )}
            {/* Stats */}
            <Box>
                <Typography variant="body2" fontWeight="bold" color={xof8 === 8 ? 'warning' : 'inherit'}>
                    {xof8} / 8
                </Typography>
                <Typography variant="caption" fontSize={16}>
                    {character.current_fame} <img src="/realm/fame.png" alt="Fame" width={16} height={16} style={{ marginBottom: -2 }} />
                </Typography>
            </Box>
            {/* Backpack / Extender / Belt indicators */}
            <Box
                sx={{
                    display: 'flex',
                    flexDirection: 'column',
                    flexWrap: 'wrap',
                    justifyContent: 'center',
                    alignItems: 'end',
                    maxHeight: '50px',
                    width: 'fit-content',
                    ml: '-8px',
                }}
            >
                {backpackIcons.backpack && <img src={backpackIcons.backpack} alt="Backpack" width={25} height={25} />}
                {backpackIcons.extender && <img src={backpackIcons.extender} alt="Backpack Extender" width={25} height={25} />}
                {backpackIcons.belt && <img src={backpackIcons.belt} alt="Adventurer's Belt" width={25} height={25} />}
            </Box>
        </Box>
    );
}

export default SingleCharacterOverview;