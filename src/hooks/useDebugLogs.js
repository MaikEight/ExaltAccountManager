import { useEffect, useState } from "react";

function useDebugLogs() {
    const [debugLogsEnabled, setDebugLogsEnabled] = useState(false);

    useEffect(() => {
        const localStorageValue = localStorage.getItem('flag:debug');
        if (localStorageValue === 'true') {
            setDebugLogsEnabled(true);
            return;
        }

        const sessionStorageValue = sessionStorage.getItem('flag:debug');
        if (sessionStorageValue === 'true') {
            setDebugLogsEnabled(true);
            return;
        }
    }, []);

    const log = (...args) => {
        if (debugLogsEnabled) {
            console.log(...args);
        }
    };

    return { debugLogs: debugLogsEnabled, log };
}

export default useDebugLogs;