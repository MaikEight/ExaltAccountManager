import React from "react";
import ReactDOM from "react-dom/client";
import App from "./App";
import "./styles.css";
import { appWindow } from '@tauri-apps/api/window'


ReactDOM.createRoot(document.getElementById("root"))
  .render(
    <>
      <div className="titlebar-spacer" />
      <App />
    </>
  );

document
  .getElementById('titlebar-minimize')
  .addEventListener('click', () => appWindow.minimize())
document
  .getElementById('titlebar-maximize')
  .addEventListener('click', () => appWindow.toggleMaximize())
document
  .getElementById('titlebar-close')
  .addEventListener('click', () => appWindow.close())

