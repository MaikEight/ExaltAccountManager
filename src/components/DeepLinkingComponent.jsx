import { onOpenUrl } from '@tauri-apps/plugin-deep-link';
import { useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import { useUserLogin } from 'eam-commons-js';

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

    const performDeepLinking = async () => {
        await onOpenUrl((urls) => {
            const url = new URL(urls[0]);
            //Check if the URL is a callback URL
            if (url.pathname === 'profile/callback') {
                if (!isAuthenticated) {
                    const oldCallbacksData = window.sessionStorage.getItem('auth0Callbacks');
                    if (oldCallbacksData !== null) {
                        const oldCallbacks = JSON.parse(oldCallbacksData);

                        if (oldCallbacks.find(ocb => ocb === urls[0]) !== undefined) {
                            return;
                        }
                        window.sessionStorage.setItem('auth0Callbacks', JSON.stringify([...oldCallbacks, urls[0]]));
                    } else {
                        window.sessionStorage.setItem('auth0Callbacks', JSON.stringify([urls[0]]));
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
                    refreshUserAfterDelay();
                }

                navigate('/profile');
                return;
            }

            const urlData = url.pathname.replace(/^\/+|\/+$/g, '');
            if (deepLinkingPaths.has(urlData)) {
                navigate(deepLinkingPaths.get(urlData) + url.search);
                console.log("Nav:", deepLinkingPaths.get(urlData));
            }
        });
    }

    useEffect(() => {
        performDeepLinking();
    }, []);

    return (
        <></>
    );
}

export default DeepLinkingComponent;