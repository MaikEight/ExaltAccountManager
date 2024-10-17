import { useAuth0 } from '@auth0/auth0-react';
import { onOpenUrl } from '@tauri-apps/plugin-deep-link';
import { useEffect } from 'react';
import { useNavigate } from 'react-router-dom';

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
    const { handleRedirectCallback } = useAuth0();

    const performDeepLinking = async () =>  {
        await onOpenUrl((urls) => {
            console.log('deep link:', urls[0]);
            
            const url = new URL(urls[0]);
            
            //Check if the URL is a callback URL
            if(url.pathname === 'profile/callback') {  
                console.log('callback url:', urls[0]);          
                handleRedirectCallback(urls[0]);
                navigate('/profile');
                return;
            }

            const urlData = url.pathname.replace(/^\/+|\/+$/g, '');
            if(deepLinkingPaths.has(urlData)) {
                navigate(deepLinkingPaths.get(urlData));
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