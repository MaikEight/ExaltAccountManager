import React from "react";
import ReactDOM from "react-dom/client";
import App from "./App";
import { UserSettingsProvider } from "eam-commons-js";
import { Auth0Provider } from "@auth0/auth0-react";
import "eam-commons-js/src/themes/styles.css";

ReactDOM.createRoot(document.getElementById("root"))
    .render(
        <div>
            <UserSettingsProvider>
                <Auth0Provider                    
                    domain="https://login.exaltaccountmanager.com"
                    clientId="o1W1coVQMV9qrIg4G2SmZJbz1G5vRCpZ"
                    useRefreshTokens={true}
                    useRefreshTokensFallback={false}
                    authorizationParams={{
                        redirect_uri: "eam:profile/callback"
                    }}
                >
                    <App />
                </Auth0Provider>
            </UserSettingsProvider>
        </div>
    );