import { Box } from "@mui/material";
import { createContext, useEffect, useState } from "react";
import useUserSettings from "../hooks/useUserSettings";
import { useMemo } from "react";
import { WidgetBarEvents, WidgetBars } from "../components/Widgets/Widgetbars";

const WidgetsContext = createContext();

function updateWidgetBarState(current = null, open = null, type = null, data = null) {
    return {
        isOpen: false,
        type: null,
        data: null,
        ...current,
        ...(open !== null ? { isOpen: open } : {}),
        ...(type ? { type } : {}),
        ...(data ? { data } : {}),
    }
}

function WidgetsContextProvider({ children }) {
    const { getByKeyAndSubKey, setByKeyAndSubKey } = useUserSettings();
    const [widgetBarState, setWidgetBarState] = useState(updateWidgetBarState());
    const [updateConfig, setUpdateConfig] = useState(0);
    const [events, setEvents] = useState({
        [WidgetBarEvents.ON_OPEN]: [],
        [WidgetBarEvents.ON_CLOSE]: [],
    });

    /**
     * Subscribe to a widget bar event
     * @param {string} eventName - The event to subscribe to (use WidgetBarEvents constants)
     * @param {function} callback - The callback function to invoke when the event fires
     * @returns {function} unsubscribe - Call this function to unsubscribe
     * @example
     * const unsubscribe = subscribeToEvent(WidgetBarEvents.ON_OPEN, (data) => {
     *   console.log('Widget bar opened', data);
     * });
     * // Later...
     * unsubscribe();
     */
    const subscribeToEvent = (eventName, callback) => {
        if (!WidgetBarEvents.validateName(eventName)) {
            console.warn(`Invalid event name "${eventName}" for WidgetBarEvents`);
            return () => {}; // Return no-op function
        }

        // Add the callback to the subscribers list
        setEvents((currentEvents) => ({
            ...currentEvents,
            [eventName]: [...currentEvents[eventName], callback]
        }));

        // Return an unsubscribe function
        return () => {
            setEvents((currentEvents) => ({
                ...currentEvents,
                [eventName]: currentEvents[eventName].filter(cb => cb !== callback)
            }));
        };
    }

    /**
     * Emit an event to all subscribers
     * @param {string} eventName - The event to emit
     * @param {any} data - Data to pass to subscribers
     * @private
     */
    const emitEvent = (eventName, data) => {
        if (!WidgetBarEvents.validateName(eventName)) {
            return;
        }

        const subscribers = events[eventName] || [];
        subscribers.forEach(callback => {
            try {
                callback(data);
            } catch (error) {
                console.error(`Error in ${eventName} subscriber:`, error);
            }
        });
    }

    const widgetBarConfig = useMemo(() => {
        if (!widgetBarState?.type) return null;

        const savedConfigs = getByKeyAndSubKey('widgets', 'widgetBars');
        if (!savedConfigs) return null;

        return savedConfigs[widgetBarState.type.type] || widgetBarState.type.defaultConfig;
    }, [widgetBarState, getByKeyAndSubKey, updateConfig]);

    const updateWidgetBarConfig = (newConfig) => {
        if (!widgetBarState?.type) return;

        const savedConfigs = getByKeyAndSubKey('widgets', 'widgetBars') || {};
        const updatedConfigs = {
            ...savedConfigs,
            [widgetBarState.type.type]: {
                ...widgetBarState.type.defaultConfig,
                ...savedConfigs[widgetBarState.type.type],
                ...newConfig,
            }
        };
        setByKeyAndSubKey('widgets', 'widgetBars', updatedConfigs);
        setUpdateConfig((current) => current + 1);
    }

    const openWidgetBar = () => {
        setWidgetBarState((current) => {
            const newState = updateWidgetBarState(current, true);
            emitEvent(WidgetBarEvents.ON_OPEN, newState);
            return newState;
        });
    }

    const closeWidgetBar = () => {
        setWidgetBarState((current) => {
            const newState = updateWidgetBarState(current, false);
            emitEvent(WidgetBarEvents.ON_CLOSE, newState);
            return newState;
        });
    }

    const updateWidgetBarType = (type) => {
        setWidgetBarState((current) => updateWidgetBarState(current, null, type));
    }

    const showWidgetBar = (type, data = null) => {
        setWidgetBarState((current) => {
            const newState = updateWidgetBarState(current, true, type, data);
            emitEvent(WidgetBarEvents.ON_OPEN, newState);
            return newState;
        });
    }

    const updateWidgetBarData = (data) => {
        setWidgetBarState((current) => updateWidgetBarState(current, null, null, data));
    }


    const values = {
        widgetBarConfig,
        updateWidgetBarConfig,

        subscribeToEvent,

        widgetBarState,
        openWidgetBar,
        closeWidgetBar,
        updateWidgetBarType,
        showWidgetBar,
        updateWidgetBarData,
    };

    return (
        <WidgetsContext.Provider value={values}>
            {children}
            <Box
                sx={{
                    position: 'fixed',
                    top: 0,
                    right: 0,
                    height: '100vh',
                    zIndex: 1300,
                    pointerEvents: widgetBarState?.isOpen ? 'auto' : 'none',
                    transition: 'width 0.2s ease-in-out',
                    width: widgetBarState?.isOpen ? WidgetBars.getWidgetBarWidthBySlots(widgetBarConfig?.slots || 1) : '0px',
                    overflow: 'hidden',
                }}
            >
                {
                    widgetBarState.type?.BarComponent &&
                    <widgetBarState.type.BarComponent />
                }
            </Box>
        </WidgetsContext.Provider>
    )
}

export default WidgetsContext;
export { WidgetsContextProvider };