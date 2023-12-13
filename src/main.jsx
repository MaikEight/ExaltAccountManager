import React from "react";
import ReactDOM from "react-dom/client";
import App from "./App";
import "./styles.css";
import { appWindow } from '@tauri-apps/api/window'
import { UserSettingsProvider } from "./contexts/UserSettingsContext";
import CustomToolbar from "./components/CustomToolbar";


ReactDOM.createRoot(document.getElementById("root"))
  .render(
    <>      
      <UserSettingsProvider>
        <App />
      </UserSettingsProvider>
    </>
  );

// document
//   .getElementById('titlebar-minimize')
//   .addEventListener('click', () => appWindow.minimize())
// document
//   .getElementById('titlebar-maximize')
//   .addEventListener('click', () => appWindow.toggleMaximize())
// document
//   .getElementById('titlebar-close')
//   .addEventListener('click', () => appWindow.close())

