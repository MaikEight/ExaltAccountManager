import { useEffect, useState } from "react";
import reactLogo from "./assets/react.svg";
import { invoke } from "@tauri-apps/api/tauri";
import "./App.css";
import { Toolbar } from "@mui/material";
import Sidebar from "./components/Sidebar";

function App() {
  const [greetMsg, setGreetMsg] = useState("");
  const [name, setName] = useState("");
  const [fileContent, setFileContent] = useState('');

  const filePath = "C:\\Users\\Maik8\\Desktop\\test.txt";

  const readFile = async () => {
    try {
      console.log(filePath);
      console.log(window.__TAURI__.fs);
      const bytes = await window.__TAURI__.fs.readBinaryFile(filePath);
      const content = JSON.parse(new TextDecoder().decode(bytes));
      console.log(content);
      setFileContent(content);
    } catch (error) {
      console.error('Error reading file:', error);
    }
  };

  useEffect(() => {
    readFile();
  }, []);

  async function greet() {
    // Learn more about Tauri commands at https://tauri.app/v1/guides/features/command
    setGreetMsg(await invoke("greet", { name }));
  }

  return (
    <Sidebar>
      <div>
        <p>Content</p>
      </div>
    </Sidebar>
  );
}

export default App;
