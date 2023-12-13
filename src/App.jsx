import { useContext, useEffect, useState } from "react";
import { invoke } from "@tauri-apps/api/tauri";
import Sidebar from "./components/Sidebar";
import { ColorContextProvider } from "./contexts/ColorContext";
import UserSettingsContext from "./contexts/UserSettingsContext";
import { ThemeProvider } from "styled-components";
import { CssBaseline } from "@mui/material";
import MainRouter from "./MainRouter";

import { appWindow, PhysicalSize } from '@tauri-apps/api/window';
function App() {
  const userSettings = useContext(UserSettingsContext);

  // const [fileContent, setFileContent] = useState('');

  // const filePath = "C:\\Users\\Maik8\\Desktop\\test.txt";

  // const readFile = async () => {
  //   try {
  //     console.log(filePath);
  //     console.log(window.__TAURI__.fs);
  //     const bytes = await window.__TAURI__.fs.readBinaryFile(filePath);
  //     const content = JSON.parse(new TextDecoder().decode(bytes));
  //     console.log(content);
  //     setFileContent(content);
  //   } catch (error) {
  //     console.error('Error reading file:', error);
  //   }
  // };
  
  useEffect(() => {
    // readFile();
    appWindow.setMinSize(new PhysicalSize(600, 500));
  }, []);

  return (
    <ColorContextProvider>
      <MainRouter />        
    </ColorContextProvider>
  );
}

export default App;
