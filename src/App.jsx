import { useEffect } from "react";
import { ColorContextProvider } from "./contexts/ColorContext";
import MainRouter from "./MainRouter";
import { onStartUp } from "./utils/startUpUtils";

function App() {
  
  useEffect(() => {
    onStartUp();    
  }, []);  

  return (
    <ColorContextProvider>
      <MainRouter />
    </ColorContextProvider>
  );
}

export default App;
