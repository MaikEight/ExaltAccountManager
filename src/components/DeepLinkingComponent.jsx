import { onOpenUrl } from '@tauri-apps/plugin-deep-link';
import { useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import { useUserLogin } from 'eam-commons-js';
import navigationService from '../utils/navigationService';
import { getCurrentWindow } from '@tauri-apps/api/window';
import { listen } from '@tauri-apps/api/event';
import { invoke } from '@tauri-apps/api/core';

const deepLinkingPaths = new Map([
    ['accounts', '/accounts'],
    ['vaultpeeker', '/vaultPeeker'],
    ['dailylogins', '/dailyLogins'],
    ['utilities', '/utilities'],
    ['settings', '/settings'],
    ['logs', '/logs'],
    ['profile', '/profile'],
    ['about', '/about'],
    ['feedback', '/feedback'],
    ['importer', '/importer'],
]);

function DeepLinkingComponent() {
    const navigate = useNavigate();
    const { handleAuthRedirect, isAuthenticated, refreshUserAfterDelay } = useUserLogin();

    const handleDeepLinkUrl = (urlString) => {
        const url = new URL(urlString);
        console.log('Handling deep link:', urlString);
        
        //Check if the URL is a callback URL
        if (url.pathname === 'profile/callback') {
            if (!isAuthenticated) {
                const oldCallbacksData = window.sessionStorage.getItem('auth0Callbacks');
                if (oldCallbacksData !== null) {
                    const oldCallbacks = JSON.parse(oldCallbacksData);

                    if (oldCallbacks.find(ocb => ocb === urlString) !== undefined) {
                        return;
                    }
                    window.sessionStorage.setItem('auth0Callbacks', JSON.stringify([...oldCallbacks, urlString]));
                } else {
                    window.sessionStorage.setItem('auth0Callbacks', JSON.stringify([urlString]));
                }

                handleAuthRedirect(url);
            }
            navigate('/profile');
            return;
        }

        if (url.pathname === 'purchases/callback') {
            const success = url.searchParams.get('success');

            if (success === 'true') {
                console.log('🥳 - Purchase successful - 🎉');
                navigate('/payment/successful');
                refreshUserAfterDelay();
                return;
            }

            navigate('/profile');
            return;
        }

        const urlData = url.pathname.replace(/^\/+|\/+$/g, '');
        if (deepLinkingPaths.has(urlData)) {
            navigate(deepLinkingPaths.get(urlData) + url.search);
            console.log(deepLinkingPaths.get(urlData));
            return;
        }

        if (url.pathname === 'start-daily-login-task') {
             getCurrentWindow().hide();
             return;
        }
    };

    const performDeepLinking = async () => {
        await onOpenUrl((urls) => {
            handleDeepLinkUrl(urls[0]);
        });
    };

    const checkForStartupDeepLink = async () => {
        try {
            const deepLinkUrl = await invoke('get_current_deep_link');
            if (deepLinkUrl) {
                console.log('Startup deep link found:', deepLinkUrl);
                handleDeepLinkUrl(deepLinkUrl);
            }
        } catch (error) {
            console.error('Error checking for startup deep link:', error);
        }
    };

    const checkIfStartedWithAutostart = async () => {
        try {
            const isAutostart = await invoke('is_started_with_autostart');
            if (isAutostart) {
                if(sessionStorage.getItem('flag:debug') === 'true') {
                    console.log('Application was started via autostart');
                }
                
                const currentWindow = getCurrentWindow();
                if (currentWindow.isVisible()) {
                    currentWindow.hide();
                }
            }
        } catch (error) {
            console.error('Error checking for autostart:', error);
        }
    };

    useEffect(() => {
        if(navigate) {
            // Check for startup deep link first
            checkForStartupDeepLink();
            
            // Check if started with autostart
            checkIfStartedWithAutostart();
            
            // Perform deep linking when the component mounts
            performDeepLinking();
            
            // Listen for deep-link events from Rust backend (for app startup deep links)
            const unsubscribe = listen('deep-link', (event) => {
                console.log('Deep link event received from backend:', event.payload);
                handleDeepLinkUrl(event.payload);
            });

            // Clean up the listener when component unmounts
            return () => {
                unsubscribe.then(fn => fn());
            };
        }
    }, []);

    useEffect(() => {
        if(!navigate) {
            return;
        }

        // Initialize the navigation service with the navigate function
        navigationService.setNavigate(navigate);
        
        performDeepLinking();
    }, [navigate]);

    return (
        <></>
    );
}

export default DeepLinkingComponent;