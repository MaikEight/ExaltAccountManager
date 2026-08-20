import { useEffect, useState } from "react";
import { ColorContextProvider } from "eam-commons-js";
import { onStartUp, setApiHwidHash } from "./utils/startUpUtils";
import useHWID from "./hooks/useHWID";
import { heartBeat } from "./backend/eamApi";
import MainProviders from "./MainProviders";
import { invoke } from "@tauri-apps/api/core";
import { refreshRuntimeAssets } from "./backend/assetApi";

function App() {
    const [hasTriggeredStartup, setHasTriggeredStartup] = useState(false);
    const [assetStatus, setAssetStatus] = useState({ state: "loading", message: null });
    const [assetRevision, setAssetRevision] = useState(0);
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
                    if (assetRetry > 0) {
                        setAssetRevision((revision) => revision + 1);
                    }
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
            <MainProviders key={assetRevision} />
            {(assetStatus.state === "degraded" || assetStatus.state === "cached") && (
                <div
                    title={assetStatus.message || undefined}
                    style={{
                        position: "fixed",
                        right: "1rem",
                        bottom: "1rem",
                        zIndex: 10000,
                        display: "flex",
                        alignItems: "center",
                        gap: "0.75rem",
                        maxWidth: "28rem",
                        padding: "0.75rem 1rem",
                        color: "#fff",
                        background: "#332f48",
                        border: "1px solid #8f8aa8",
                        borderRadius: "0.5rem",
                        boxShadow: "0 0.4rem 1.2rem rgba(0, 0, 0, 0.35)",
                    }}
                >
                    <span>
                        {assetStatus.state === "cached"
                            ? "Using cached game data while the asset service is unavailable."
                            : "Game data is unavailable. Unknown items will use question-mark placeholders."}
                    </span>
                    <button
                        type="button"
                        onClick={() => setAssetRetry((value) => value + 1)}
                        style={{ whiteSpace: "nowrap" }}
                    >
                        Retry
                    </button>
                </div>
            )}
        </ColorContextProvider>
    );
}

export default App;
