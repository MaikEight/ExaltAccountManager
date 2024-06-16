import { Box, Modal } from "@mui/material";
import { createContext, useEffect, useState } from "react";
import useStartupPopups from "../hooks/useStartupPopups";
import useAccounts from "../hooks/useAccounts";

const PopupContext = createContext();

function PopupContextProvider({ children }) {
    const [popupData, setPopupData] = useState(null);
    const { performPopupCheck } = useStartupPopups();
    const { accounts } = useAccounts();    

    const showPopup = (data) => {
        setPopupData(data);
    };

    const closePopup = () => {
        setPopupData(null);
    };

    const handleCloseModal = (event, reason) => {
        if(popupData.preventClose && (reason === 'backdropClick' || reason === 'escapeKeyDown')) {
            return;
        }

        closePopup();
    };

    useEffect(() => {
        const timeoutId = setTimeout(async () => {
            const popupData = await performPopupCheck();
            if (popupData) {
                showPopup(popupData);
            }
        }, 250);

        return () => clearTimeout(timeoutId);
    }, [accounts]);

    const value = {
        showPopup,
        closePopup,
    }

    return (
        <PopupContext.Provider value={value}>
            {children}
            <Modal
                open={!!popupData}
                onClose={handleCloseModal}
                disableAutoFocus
            >
                <Box 
                    sx={{
                        display: 'flex',
                        justifyContent: 'center',
                        alignItems: 'center',
                        position: 'absolute',
                        top: '50%',
                        left: '50%',
                        transform: 'translate(-50%, -50%)',
                    }}
                >
                    {!!popupData && popupData.content}
                </Box>
            </Modal>
        </PopupContext.Provider>
    );
}

export default PopupContext;
export { PopupContextProvider };