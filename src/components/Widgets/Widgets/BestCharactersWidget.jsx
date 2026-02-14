import { Box } from "@mui/material";
import WidgetBase from "./WidgetBase";
import useWidgets from "../../../hooks/useWidgets";
import { useEffect, useMemo, useRef, useState } from "react";
import { invoke } from "@tauri-apps/api/core";
import SingleCharacterOverview from "./Components/SingleCharacterOverview";

function BestCharactersWidget({ type, widgetId }) {
    const { widgetBarState, widgetBarConfig, getWidgetConfiguration } = useWidgets();
    const config = getWidgetConfiguration(type);

    const [charListDataset, setCharListDataset] = useState(null);
    const [character, setCharacter] = useState([]);

    const lastEmailRef = useRef(null);
    const sortCharacters = (dataset) => {
        if (!dataset || !dataset.character) {
            setCharacter([]);
            return;
        }

        const chars = dataset.character.sort(
            (a, b) => b.current_fame - a.current_fame
        );
        setCharacter(chars);
    }

    useEffect(() => {
        if (!widgetBarState?.data?.email) {
            lastEmailRef.current = null;
            setCharListDataset(null);
            sortCharacters(null);
            return;
        }

        const email = widgetBarState.data.email;
        lastEmailRef.current = email;
        invoke('get_latest_char_list_dataset_for_account', { email })
            .then((res) => {
                if (lastEmailRef.current === email) {
                    setCharListDataset(res);
                    sortCharacters(res);
                }
            })
            .catch((err) => {
                console.error(err);
            });
    }, [widgetBarState.data]);

    const currentSlots = Math.min(widgetBarConfig?.slots || 1, config?.slots || type?.defaultConfig?.slots || 1);
    const columns = currentSlots > 1 ? 2 : 1;

    // Group parsed items by character id for equipment lookup
    const itemsByCharId = useMemo(() => {
        if (!charListDataset?.items) return {};
        const map = {};
        for (const item of charListDataset.items) {
            if (!item.storage_type_id?.startsWith('char:')) continue;
            const charId = parseInt(item.storage_type_id.split(':')[1], 10);
            if (!map[charId]) map[charId] = [];
            map[charId].push(item);
        }
        return map;
    }, [charListDataset]);

    const charsToRender = useMemo(() => {
        const maxAmount = columns * 3;
        const chars = character.slice(0, maxAmount);

        return chars.map((c, i) => (
            <SingleCharacterOverview key={i} number={i + 1} character={c} parsedItems={itemsByCharId[c.char_id] || []} />
        ));
    }, [character, columns, itemsByCharId]);

    return (
        <WidgetBase type={type} widgetId={widgetId}>
            <Box
                sx={{
                    width: '100%',
                    display: 'grid',
                    gridTemplateColumns: `repeat(${columns}, 1fr)`,
                    gridAutoFlow: 'column',
                    gridTemplateRows: 'repeat(3, auto)',
                }}
            >
                {charsToRender}
            </Box>
        </WidgetBase>
    );
}

export default BestCharactersWidget;