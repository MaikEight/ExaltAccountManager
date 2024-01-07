import { useEffect } from "react";
import { ColorContextProvider } from "./contexts/ColorContext";
import MainRouter from "./MainRouter";
import { appWindow, PhysicalSize } from '@tauri-apps/api/window';

function App() {
  
  useEffect(() => {
    appWindow.setMinSize(new PhysicalSize(850, 600));
  }, []);

  return (
    <ColorContextProvider>
      <MainRouter />        
    </ColorContextProvider>
  );
}

export default App;
