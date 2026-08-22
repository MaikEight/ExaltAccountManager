import { useEffect, useRef, useState } from "react";
import { ColorContextProvider } from "eam-commons-js";
import { onStartUp, setApiHwidHash } from "./utils/startUpUtils";
import useHWID from "./hooks/useHWID";
import { heartBeat } from "./backend/eamApi";
import MainProviders from "./MainProviders";
import { invoke } from "@tauri-apps/api/core";
import { refreshRuntimeAssets } from "./backend/assetApi";
import { GameDataLoadingScreen } from "./components/GameDataStatus";
import { GameDataStatusContextProvider } from "./contexts/GameDataStatusContext";

function App() {
    const [hasTriggeredStartup, setHasTriggeredStartup] = useState(false);
    const [assetStatus, setAssetStatus] = useState({ state: "loading", message: null });
    const [assetRetry, setAssetRetry] = useState(0);
    const [isRefreshingAssets, setIsRefreshingAssets] = useState(false);
    const [assetRevision, setAssetRevision] = useState(0);
    // Which build the mounted tree is showing. Undefined until the first result.
    const appliedBuild = useRef(undefined);
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
        const isRetry = assetRetry > 0;

        // Only the first attempt gates rendering. A retry refreshes in place, so
        // the window is not replaced by a loading screen for what is usually a
        // problem the user has not even noticed.
        if (isRetry) {
            setIsRefreshingAssets(true);
        } else {
            setAssetStatus({ state: "loading", message: null });
        }

        // Item, stat and fame data lands in module-level objects that components
        // read on mount, so the tree only needs remounting when the data behind
        // them actually changed. A retry that returns the same build, or one that
        // fails, leaves the interface alone.
        const applyBuild = (buildId) => {
            const isFirstResult = appliedBuild.current === undefined;
            const hasChanged = appliedBuild.current !== buildId;
            appliedBuild.current = buildId;
            if (!isFirstResult && hasChanged) {
                setAssetRevision((revision) => revision + 1);
            }
        };

        refreshRuntimeAssets()
            .then((manifest) => {
                if (cancelled) return;
                applyBuild(manifest?.buildId ?? null);
                setAssetStatus(manifest.clientCache?.warning
                    ? { state: "cached", message: manifest.clientCache.warning }
                    : { state: "ready", message: null });
            })
            .catch((error) => {
                if (cancelled) return;
                console.error("Failed to load live game data:", error);
                // Nothing was applied, but the tree still mounted, so record that
                // and a later success will remount it.
                applyBuild(null);
                setAssetStatus({
                    state: "degraded",
                    message: error?.message || "Unable to load live game assets.",
                });
            })
            .finally(() => {
                if (!cancelled) {
                    setIsRefreshingAssets(false);
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
            <GameDataStatusContextProvider
                status={assetStatus}
                isRefreshing={isRefreshingAssets}
                onRetry={() => setAssetRetry((value) => value + 1)}
            >
                {assetStatus.state === "loading"
                    ? <GameDataLoadingScreen />
                    : <MainProviders key={assetRevision} />}
            </GameDataStatusContextProvider>
        </ColorContextProvider>
    );
}

export default App;
