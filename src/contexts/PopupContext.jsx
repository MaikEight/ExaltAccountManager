import { Box, Modal } from "@mui/material";
import { createContext, useEffect, useState } from "react";
import useStartupPopups from "../hooks/useStartupPopups";
import useAccounts from "../hooks/useAccounts";

const PopupContext = createContext();

function PopupContextProvider({ children }) {
    const [popupData, setPopupData] = useState(null);
    const { accounts } = useAccounts();
    const { performPopupCheck } = useStartupPopups();
    const [startupPopupResult, setStartupPopupResult] = useState({
        done: false,
        hadPopup: false,
    });

    const showPopup = (data) => {
        setPopupData(data);
    };

    const closePopup = () => {
        if (popupData?.onClose) {
            popupData.onClose();
        }
        setPopupData(null);
    };

    const handleCloseModal = (event, reason) => {
        if (popupData.preventClose && (reason === 'backdropClick' || reason === 'escapeKeyDown')) {
            return;
        }

        closePopup();
    };

    useEffect(() => {
        const timeoutId = setTimeout(async () => {
            const popupData = await performPopupCheck();
            const newStartupPopupResult = {
                done: true,
                hadPopup: false
            };

            if (popupData) {
                newStartupPopupResult.hadPopup = true;
                showPopup(popupData);
            }

            setStartupPopupResult(newStartupPopupResult);
        }, 500);

        return () => clearTimeout(timeoutId);
    }, [accounts]);

    const value = {
        showPopup,
        closePopup,
        startupPopupResult
    }

    return (
        <PopupContext.Provider value={value}>
            {children}
            <Modal
                open={Boolean(popupData)}
                onClose={handleCloseModal}
                disableAutoFocus
                sx={{
                    display: 'flex',
                    alignItems: 'center',
                    justifyContent: 'center',
                    p: 2,
                }}
            >
                <Box
                    sx={{
                        position: 'relative',
                        maxHeight: 'calc(100vh - 32px)',
                        maxWidth: 'calc(100vw - 32px)',
                        outline: 'none',
                        borderRadius: theme => `${theme.shape.borderRadius}px`,
                        overflow: 'auto',
                    }}
                >
                    {Boolean(popupData) && popupData.content}
                </Box>
            </Modal>
        </PopupContext.Provider>
    );
}

export default PopupContext;
export { PopupContextProvider };