import { useEffect, useState } from "react";
import { ColorContextProvider } from "eam-commons-js";
import { onStartUp, setApiHwidHash } from "./utils/startUpUtils";
import useHWID from "./hooks/useHWID";
import { heartBeat } from "./backend/eamApi";
import MainProviders from "./MainProviders";
import { invoke } from "@tauri-apps/api/core";
import { refreshRuntimeAssets } from "./backend/assetApi";
import { GameDataLoadingScreen, GameDataStatusToast } from "./components/GameDataStatus";

function App() {
    const [hasTriggeredStartup, setHasTriggeredStartup] = useState(false);
    const [assetStatus, setAssetStatus] = useState({ state: "loading", message: null });
    const [assetRetry, setAssetRetry] = useState(0);
    const { hwid } = useHWID();

    useEffect(() => {
        onStartUp();
        let disposed = false;
        let heartBeatInterval = null;
        const startHeartbeat = () => {
            if (disposed) return;
            heartBeatInterval = setInterval(() => {
                heartBeat();
            }, 59_000);
        };

        invoke('get_user_data_by_key', { key: 'analytics' })
            .then(response => {
                if (response) {
                    try {
                        if (response.dataValue) {
                            const analytics = JSON.parse(response.dataValue);
                            if (analytics && analytics.optOut) {
                                console.log("You have opt-out of analytics. 😭");
                                return null;
                            }
                        }
                    } catch (error) {
                        console.error(error);
                    }
                }

                startHeartbeat();
            })
            .catch(() => {
                startHeartbeat();
            });

        return () => {
            disposed = true;
            if (heartBeatInterval) {
                clearInterval(heartBeatInterval);
            }
        };
    }, []);

    useEffect(() => {
        let cancelled = false;
        setAssetStatus({ state: "loading", message: null });

        refreshRuntimeAssets()
            .then((manifest) => {
                if (!cancelled) {
                    setAssetStatus(manifest.clientCache?.warning
                        ? { state: "cached", message: manifest.clientCache.warning }
                        : { state: "ready", message: null });
                }
            })
            .catch((error) => {
                if (!cancelled) {
                    console.error("Failed to load live game data:", error);
                    setAssetStatus({
                        state: "degraded",
                        message: error?.message || "Unable to load live game assets.",
                    });
                }
            });

        return () => {
            cancelled = true;
        };
    }, [assetRetry]);

    useEffect(() => {
        if (hasTriggeredStartup || !hwid) {
            return;
        }

        setApiHwidHash(hwid);
        setHasTriggeredStartup(true);
    }, [hwid]);

    return (
        <ColorContextProvider>
            {assetStatus.state === "loading"
                ? <GameDataLoadingScreen />
                : <MainProviders />}
            {(assetStatus.state === "degraded" || assetStatus.state === "cached") && (
                <GameDataStatusToast
                    status={assetStatus}
                    onRetry={() => setAssetRetry((value) => value + 1)}
                />
            )}
        </ColorContextProvider>
    );
}

export default App;
