import React from "react";
import ReactDOM from "react-dom/client";
import App from "./App";
import "./styles.css";
import { UserSettingsProvider } from "./contexts/UserSettingsContext";


ReactDOM.createRoot(document.getElementById("root"))
.render(
  <div
    style={{
      borderRadius: '10px',
      overflow: 'hidden',
    }}
  >
    <UserSettingsProvider>
      <App />
    </UserSettingsProvider>
  </div>
);