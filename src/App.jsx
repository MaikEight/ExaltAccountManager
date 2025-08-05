import { useEffect, useState } from "react";
import { ColorContextProvider } from "eam-commons-js";
import { onStartUp, setApiHwidHash } from "./utils/startUpUtils";
import useHWID from "./hooks/useHWID";
import { heartBeat } from "./backend/eamApi";
import MainProviders from "./MainProviders";
import { invoke } from "@tauri-apps/api/core";

function App() {
    const [hasTriggeredStartup, setHasTriggeredStartup] = useState(false);
    const { hwid } = useHWID();

    useEffect(() => {
        onStartUp();
        const getHearbetInterval = () => {
            return setInterval(async () => {
                heartBeat();
            }, 59_000);
        }

        const heartBeatInterval = invoke('get_user_data_by_key', { key: 'analytics' })
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

                getHearbetInterval();
            })
            .catch(() => {
                return getHearbetInterval();
            });

        return () => {
            if (heartBeatInterval) {
                clearInterval(heartBeatInterval);
            }
        };
    }, []);

    useEffect(() => {
        if (hasTriggeredStartup || !hwid) {
            return;
        }

        setApiHwidHash(hwid);
        setHasTriggeredStartup(true);
    }, [hwid]);

    return (
        <ColorContextProvider>
            <MainProviders />
        </ColorContextProvider>
    );
}

export default App;
