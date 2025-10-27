import { useEffect, useState } from 'react';

/**
 * Custom hook to track when portrait sprites are loaded and ready
 * @returns {boolean} True when portraits are ready to render
 */
function usePortraitReady() {
    const [isReady, setIsReady] = useState(window.portraitReady || false);

    useEffect(() => {
        // Listen for portrait ready event
        const handlePortraitReady = () => {
            setIsReady(true);
            // Clean up listener immediately after it fires
            window.removeEventListener('portraitReady', handlePortraitReady);
        };

        // If already ready, set state and exit early
        if (window.portraitReady) {
            setIsReady(true);
            return;
        }

        window.addEventListener('portraitReady', handlePortraitReady);

        // Cleanup listener on unmount (if event hasn't fired yet)
        return () => {
            window.removeEventListener('portraitReady', handlePortraitReady);
        };
    }, []);

    return isReady;
}

export default usePortraitReady;
