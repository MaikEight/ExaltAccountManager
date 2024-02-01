import { useEffect, useState } from "react";
import { ColorContextProvider } from "./contexts/ColorContext";
import { onStartUp, setApiHwidHash } from "./utils/startUpUtils";
import useHWID from "./hooks/useHWID";
import { heartBeat } from "./backend/eamApi";
import useUserSettings from "./hooks/useUserSettings";
import MainProviders from "./MainProviders";

function App() {
  const [hasTriggeredStartup, setHasTriggeredStartup] = useState(false);
  const hwid = useHWID();
  const settings = useUserSettings();

  useEffect(() => {
    const gameExePath = settings.getByKeyAndSubKey("game", "exePath");
    onStartUp(gameExePath);

    const heartBeatInterval = setInterval(async () => {      
      heartBeat();
    }, 59_000);

    return () => {
      clearInterval(heartBeatInterval);
    };
  }, []);

  useEffect(() => {
    if (hasTriggeredStartup || !hwid) return;

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
