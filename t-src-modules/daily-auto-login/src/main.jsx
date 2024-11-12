import React from "react";
import ReactDOM from "react-dom/client";
import App from "./App";
import { UserSettingsProvider, UserLoginProvider } from "eam-commons-js";
import "eam-commons-js/src/themes/styles.css";

ReactDOM.createRoot(document.getElementById("root"))
  .render(
    <div>
      <UserSettingsProvider>
        <UserLoginProvider>
          <App />
        </UserLoginProvider>
      </UserSettingsProvider>
    </div>
  );
