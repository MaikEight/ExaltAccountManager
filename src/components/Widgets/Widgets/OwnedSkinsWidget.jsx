import { useEffect, useRef, useState } from "react";
import useWidgets from "../../../hooks/useWidgets";
import WidgetBase from "./WidgetBase";
import { invoke } from "@tauri-apps/api/core";
import { portrait } from "../../../utils/portraitUtils";
import { skins } from "../../../assets/constants";
import { Box, Tooltip } from "@mui/material";

function OwnedSkinsWidget({ type, widgetId }) {
    const { widgetBarState, widgetBarConfig, getWidgetConfiguration } = useWidgets();
    const lastEmailRef = useRef(null);

    const [charListDataset, setCharListDataset] = useState(null);
    const [ownedSkins, setOwnedSkins] = useState([]);
    const [noData, setNoData] = useState(false);

    useEffect(() => {
        if (!widgetBarState?.data?.email) {
            lastEmailRef.current = null;
            setNoData(true);
            console.warn("No email found in widget bar state for OwnedSkinsWidget");
            return;
        }

        const email = widgetBarState.data.email;

        if (lastEmailRef.current === email) {
            return;
        }
        lastEmailRef.current = email;

        setCharListDataset(null);

        invoke('get_latest_char_list_dataset_for_account', { email })
            .then((res) => {
                setNoData(false);
                setCharListDataset(res);
                console.log("Res", res);
                const skinsSet = new Set();
                res?.account?.owned_skins?.split(',')?.forEach(skinId => skinsSet.add(parseInt(skinId, 10)));
                setOwnedSkins(Array.from(skinsSet));
            })
            .catch((err) => {
                console.error(err);
                setNoData(true);
            });
    }, [widgetBarState.data]);

    return (
        <WidgetBase type={type} widgetId={widgetId}>
            <Box>
                You own {ownedSkins.length} skins.
            </Box>
            <Box
                sx={{
                    display: 'flex',
                    flexDirection: 'row',
                    flexWrap: 'wrap',
                    gap: 1,
                    mt: 1,
                    maxHeight: 300,
                    overflowY: 'auto',
                    py: 1,
                }}
            >
                {
                    ownedSkins.map(skinId => (
                        <SkinImage key={skinId} skinId={skinId} />
                    ))
                }
            </Box>
        </WidgetBase>
    )
}

export default OwnedSkinsWidget;

function SkinImage({ skinId }) {
    const [skinUrl, setSkinUrl] = useState(null);
    const [skinName, setSkinName] = useState(null);
    useEffect(() => {
        if (!skinId) {
            setSkinUrl(null);
            return;
        }

        const fetchSkinUrl = async () => {
            const skin = skins[skinId];
            if (!skin) {
                console.error(`Skin data not found for skin ID ${skinId}`);
                setSkinUrl(null);
                setSkinName(null);
                return;
            }
            const url = portrait(skin[4], skinId);
            setSkinUrl(url);
            setSkinName(skin[0]);
        }
        fetchSkinUrl();
    }, [skinId]);

    return (
        <Tooltip title={skinName ? skinName : `ID: ${skinId}`}>
            <img src={skinUrl} alt={`Skin ${skinId}`} width={34} height={34} />
        </Tooltip>
    );
}