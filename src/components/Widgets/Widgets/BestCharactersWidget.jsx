import { Box } from "@mui/material";
import WidgetBase from "./WidgetBase";
import useWidgets from "../../../hooks/useWidgets";
import { Fragment, useEffect, useMemo, useRef, useState } from "react";
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
        console.log(chars);
        setCharacter(chars);
    }

    useEffect(() => {
        if (widgetBarState?.data?.email && widgetBarState.data.email !== lastEmailRef.current) {
            lastEmailRef.current = widgetBarState.data.email;
            const email = widgetBarState.data.email;
            invoke('get_latest_char_list_dataset_for_account', { email: widgetBarState.data.email })
                .then((res) => {
                    if (lastEmailRef.current === email) {
                        setCharListDataset(res);
                        sortCharacters(res);
                    }
                })
                .catch((err) => {
                    console.error(err);
                })
        } else {
            setCharListDataset(null);
            sortCharacters(null);
        }
    }, [widgetBarState.data]);

    const charsToRender = useMemo(() => {
        console.log("Rendering chars", character, widgetBarConfig);
        const currentSlots = config?.slots || type?.defaultConfig?.slots || 1;
        const maxAmount = ((widgetBarConfig?.slots || 1 ) > 1 && currentSlots > 1) ? currentSlots * 3 : 3;
        let amnt = 0;
        const chars = [];

        while (amnt < maxAmount) {
            if (character.length <= amnt) {
                break;
            }
            chars.push(character[amnt])
            amnt++;
        }

        return (
            <Fragment>
                {
                    chars.map((c, i) => {
                        return <SingleCharacterOverview key={i} number={i + 1} character={c} />
                    })
                }
            </Fragment>
        );
    }, [character, widgetBarConfig.slots, config.slots]);


    return (
        <WidgetBase type={type} widgetId={widgetId}>
            <Box
                sx={{
                    width: '100%',
                    display: 'flex',
                    flexDirection: 'column',
                    flexWrap: 'wrap',
                    maxHeight: '200px',
                    alignItems: 'start',
                }}
            >
                {charsToRender}
            </Box>
        </WidgetBase>
    );
}

export default BestCharactersWidget;