import React from "react";
import ReactDOM from "react-dom/client";
import App from "./App";
import { UserSettingsProvider } from 'eam-commons-js';
import "eam-commons-js/src/themes/styles.css";

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
