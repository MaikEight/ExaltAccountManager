import { Box, Modal } from "@mui/material";
import { createContext, useState } from "react";

const PopupContext = createContext();

function PopupContextProvider({ children }) {
    const [popupData, setPopupData] = useState(null);

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