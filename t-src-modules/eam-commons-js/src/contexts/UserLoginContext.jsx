import { useState, useEffect } from "react";
import { createContext } from "react";
import { AUTH0_REDIRECT_URL, AUTH0_DOMAIN, AUTH0_CLIENT_ID, EAM_USERS_API } from "../../constants";
import { invoke } from '@tauri-apps/api/core';
import { checkForPaidSubscription, postPlusToken } from "../backend/eamSubscriptionsApi";

const UserLoginContext = createContext();
const SUBSCRIPTIONS = ['Default', 'Plus'];
const ENCRYPTION_KEY = "EAM_USER_REFRESH_TOKEN";

function UserLoginProvider({ children }) {
    const [user, setUser] = useState(null);
    const [accessToken, setAccessToken] = useState(null);
    const [idToken, setIdToken] = useState(null);
    const [isLoading, setIsLoading] = useState(false);
    const [refreshUserAfterDelay, setRefreshUserAfterDelay] = useState(0);

    const refreshAuthToken = async (storedRefreshToken) => {
        setIsLoading(true);
        try {
            const requestHeaders = new Map();
            const responseStr = await invoke('send_post_request', {
                url: `https://${AUTH0_DOMAIN}/oauth/token`,
                customHeaders: requestHeaders,
                body: {
                    grant_type: "refresh_token",
                    client_id: AUTH0_CLIENT_ID,
                    refresh_token: storedRefreshToken,
                }
            }).catch(error => console.error(`Error: ${error}`));

            if (!responseStr) {
                return;
            }

            const response = JSON.parse(responseStr);

            const newAccessToken = response.access_token;
            const newIdToken = response.id_token;

            const refresh_token = response.refresh_token;
            if (refresh_token) {
                const encToken = await invoke('encrypt_string', { email: ENCRYPTION_KEY, data: refresh_token }).catch(console.error);
                await invoke('insert_or_update_user_data', { userData: { dataKey: 'Auth0RefreshToken', dataValue: encToken } }).catch(console.error);
            }

            setAccessToken(newAccessToken);
            window.sessionStorage.setItem("access_token", newAccessToken);

            setIdToken(newIdToken);
            window.sessionStorage.setItem("id_token", newIdToken);

            // Fetch and set user profile with new access token
            await refreshUserData(newAccessToken, newIdToken);
        } catch (error) {
            console.error("Failed to refresh token:", error);
            logout();
        } finally {
            setIsLoading(false);
        }
    };

    const login = async () => {
        setIsLoading(true);

        try {
            const authUrl = `https://${AUTH0_DOMAIN}/authorize?` + new URLSearchParams({
                client_id: AUTH0_CLIENT_ID,
                response_type: "code",
                redirect_uri: AUTH0_REDIRECT_URL,
                scope: "openid profile email offline_access",
            }).toString();

            await invoke('open_url', { url: authUrl });
        } catch (error) {
            console.error("Failed to open Auth0 login page:", error);
        } finally {
            setIsLoading(false);
        }
    };

    const logout = () => {
        setUser(null);
        setAccessToken(null);
        setIdToken(null);
        window.sessionStorage.removeItem("access_token");
        window.sessionStorage.removeItem("id_token");

        // Clear the user data from the session storage
        window.sessionStorage.removeItem("greetingText");
        window.sessionStorage.removeItem("profileImage");

        invoke('delete_user_data_by_key', { key: 'Auth0RefreshToken' }).catch(console.error);
    };

    const handleAuthRedirect = async (url) => {
        console.log("Handling Auth0 redirect...", url);
        // Extract the authorization code from the URL if available
        const urlParams = new URLSearchParams(url.search);
        const authCode = urlParams.get("code");

        if (authCode) {
            try {
                setIsLoading(true);
                const requestHeaders = new Map();
                const responseStr = await invoke('send_post_request', {
                    url: `https://${AUTH0_DOMAIN}/oauth/token`,
                    customHeaders: requestHeaders,
                    body: {
                        grant_type: "authorization_code",
                        client_id: AUTH0_CLIENT_ID,
                        code: authCode,
                        redirect_uri: "eam:profile/callback",
                    }
                }).catch(error => console.error(`Error: ${error}`));

                if (!responseStr) {
                    return;
                }

                const response = JSON.parse(responseStr);

                const { access_token, refresh_token, id_token } = response;

                setAccessToken(access_token);
                sessionStorage.setItem("access_token", access_token);

                setIdToken(id_token);
                sessionStorage.setItem("id_token", id_token);

                if (refresh_token) {
                    const encToken = await invoke('encrypt_string', { email: ENCRYPTION_KEY, data: refresh_token });
                    await invoke('insert_or_update_user_data', { userData: { dataKey: 'Auth0RefreshToken', dataValue: encToken } });
                }

                refreshUserData(access_token, id_token);

                // Clear the URL after handling
                window.history.replaceState({}, document.title, window.location.pathname);
            } catch (error) {
                console.error("Error exchanging authorization code:", error);
            } finally {
                setIsLoading(false);
            }
        }
    };

    const refreshUserData = async (access_token = null, id_token = null) => {
        if ((!accessToken && !access_token)
            || (!idToken && !id_token)
        ) {
            return;
        }

        const debugFlag = sessionStorage.getItem('flag:debug') === 'true';

        setIsLoading(true);

        id_token = id_token || idToken;
        access_token = access_token || accessToken;

        try {
            const requestHeaders = new Map();
            requestHeaders.set("Authorization", `Bearer ${id_token}`);

            if (!id_token) {
                console.warn("No id_token found, falling back to access_token for user profile fetch.");
                throw new Error("No id_token found.");
            }

            const userInfoResponseStr = await invoke('send_get_request', {
                url: `${EAM_USERS_API}/user`,
                customHeaders: requestHeaders
            }).catch(error => {
                console.warn(`Failed to fetch user from usermanagement api, falling back to auth0 default. Error: ${error}`);
                const requestHeaders = new Map();
                requestHeaders.set("Authorization", `Bearer ${access_token}`);
                return invoke('send_get_request', {
                    url: `https://${AUTH0_DOMAIN}/userinfo`,
                    customHeaders: requestHeaders
                })
                    .then(response => response)
                    .catch(error => {
                        console.error(`Failed to fetch user from auth0 default api. Error: ${error}`);
                        return null;
                    });
            });

            if (!userInfoResponseStr) {
                return;
            }

            const userInfoResponse = JSON.parse(userInfoResponseStr);
            const enhancedUser = enhanceUserData(userInfoResponse);
            
            if (debugFlag) {
                console.log('userInfoResponse', userInfoResponse, 'enhancedUser', enhancedUser);
            }

            if (enhancedUser && enhancedUser.isPlusUser) {
                const hwid = await invoke('get_device_unique_identifier');
                const plusSignature = await postPlusToken(id_token, hwid);

                let isPlus = true;
                if (debugFlag) {
                    console.log("Plus signature:", plusSignature);
                }

                if (plusSignature) {
                    const jwt = plusSignature.signature;
                    if (jwt) {
                        await invoke('insert_or_update_user_data', {
                            userData: {
                                dataKey: 'jwtSignature',
                                dataValue: jwt
                            }
                        });
                        sessionStorage.setItem('jwtSignature', jwt);
                    }

                    if (!plusSignature.roles || !plusSignature.roles.includes('plus')) {
                        isPlus = false;
                    }
                } else {
                    isPlus = false;
                }

                enhancedUser.isPlusUser = isPlus;
            }

            setUser(enhancedUser);
        } catch (error) {
            console.error("Failed to fetch user profile:", error);
            return;
        } finally {
            setIsLoading(false);
        }
    };

    const checkForPlusUserSubscription = async () => {
        const debugFlag = sessionStorage.getItem('flag:debug') === 'true';
        const _idToken = idToken || window.sessionStorage.getItem("id_token");
        if (!_idToken) {
            console.warn("No id_token found, cannot check for Plus user subscription.");
            return false;
        }

        const updatePlusStatus = (hasValidSubscription) => {
            const newUser = { ...user, isPlusUser: hasValidSubscription };
            setUser(newUser);
        };

        const response = await checkForPaidSubscription(_idToken);
        if (response) {
            updatePlusStatus(response.hasValidSubscription);

            if (!sessionStorage.getItem('jwtSignature')) {
                const hwid = await invoke('get_device_unique_identifier');
                const plusSignature = await postPlusToken(_idToken, hwid);

                if (plusSignature && plusSignature.signature) {
                    const jwt = plusSignature.signature;
                    await invoke('insert_or_update_user_data', {
                        userData: {
                            dataKey: 'jwtSignature',
                            dataValue: jwt
                        }
                    });
                    sessionStorage.setItem('jwtSignature', jwt);
                }
            }

            return response.hasValidSubscription;
        }

        if (debugFlag) {
            console.warn("No valid subscription found for Plus user.");
        }
        updatePlusStatus(false);
        return false;
    }

    const enhanceUserData = (inputUser) => {
        if (!inputUser) {
            return null;
        }

        const subStatus = inputUser.app_metadata?.subscriptionStatus;
        const subEndDate = inputUser.app_metadata?.subscriptionEndDate ? new Date(inputUser.app_metadata?.subscriptionEndDate) : null;
        const _isPlusUser = subStatus === 'active'
            || ((subStatus === 'pending_cancellation' || subStatus === 'cancelled')
                && subEndDate
                && subEndDate > Date.now()
            );
        
        let _user = { 
            ...inputUser,
            isPlusUser: _isPlusUser,
            subName: _isPlusUser ? 'Plus' : 'Default',
            subscriptionEndDate: subEndDate
         };
         
        if (!subEndDate) {
            delete _user.subscriptionEndDate;
        }
        
        return _user;
    };

    useEffect(() => {
        const initializeAuthSession = async () => {
            setIsLoading(true);
            try {
                const encRefreshTokenData = await invoke('get_user_data_by_key', { key: 'Auth0RefreshToken' })
                    .catch((e) => console.warn("No refresh token found.", e));

                if (encRefreshTokenData && encRefreshTokenData.dataValue) {
                    const encRefreshToken = encRefreshTokenData.dataValue;
                    const refreshToken = await invoke('decrypt_string', { data: encRefreshToken }).catch(console.error);
                    await refreshAuthToken(refreshToken);
                }
            } catch (error) {
                console.error("Failed to initialize Auth0 session:", error);
            } finally {
                setIsLoading(false);
            }
        };
        initializeAuthSession();
    }, []);

    useEffect(() => {
        const timeoutId = setTimeout(() => {
            if (refreshUserAfterDelay > 0) {
                setRefreshUserAfterDelay(0);
                refreshUserData();
            }
        }, 10_000);
        return () => clearTimeout(timeoutId);
    }, [refreshUserAfterDelay]);

    const contextValue = {
        user: user,
        accessToken: accessToken,
        idToken: idToken,
        isAuthenticated: Boolean(user),
        isLoading: isLoading,
        isPlusUser: user?.isPlusUser || false,

        login: login,
        logout: logout,
        refreshAuthToken: refreshAuthToken,
        handleAuthRedirect: handleAuthRedirect,
        refreshUserData: refreshUserData,
        refreshUserAfterDelay: () => setRefreshUserAfterDelay((prev) => prev + 1),
        checkForPlusUserSubscription: checkForPlusUserSubscription
    };

    return (
        <UserLoginContext.Provider value={contextValue}>
            {children}
        </UserLoginContext.Provider>
    );
}

export { UserLoginProvider, UserLoginContext };