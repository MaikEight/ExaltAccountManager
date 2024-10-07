import { useEffect, useState } from "react";
import { ColorContextProvider } from "eam-commons-js";
import { onStartUp, setApiHwidHash } from "./utils/startUpUtils";
import useHWID from "./hooks/useHWID";
import { heartBeat } from "./backend/eamApi";
import MainProviders from "./MainProviders";

function App() {
  const [hasTriggeredStartup, setHasTriggeredStartup] = useState(false);
  const hwid = useHWID();

  useEffect(() => {
    onStartUp();
    
    const heartBeatInterval = setInterval(async () => {
      heartBeat();
    }, 59_000);

    return () => {
      clearInterval(heartBeatInterval);
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
