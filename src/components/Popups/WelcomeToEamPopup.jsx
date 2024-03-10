import { Typography } from "@mui/material";
import StyledButton from "../StyledButton";
import PopupBase from "./PopupBase";
import usePopups from "../../hooks/usePopups";


function WelcomeToEamPopup() {
    const { closePopup } = usePopups();

    return (
        <PopupBase
            title="Welcome to the all new Exalt Account Manager!"
        >
            <Typography
                variant="body1"
                component="div"
            >
                This is a new version of the EAM application.
                It has been rewritten from scratch to improve performance, security and (soon™) to add new and exciting features.
            </Typography>
            <StyledButton
                onClick={() => {
                    localStorage.setItem('firstEamStart', 'false');
                    closePopup();
                }}
            >
                Let's go
            </StyledButton>
        </PopupBase>
    );
}

export default WelcomeToEamPopup;