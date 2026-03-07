import useWidgets from "../../../hooks/useWidgets";
import StyledButton from "../../StyledButton";
import WidgetBase from "./WidgetBase";
import { Box, Grid, Tooltip } from "@mui/material";
import WarningAmberOutlinedIcon from '@mui/icons-material/WarningAmberOutlined';
import PlayCircleFilledWhiteOutlinedIcon from '@mui/icons-material/PlayCircleFilledWhiteOutlined';
import RefreshOutlinedIcon from '@mui/icons-material/RefreshOutlined';
import DeleteOutlineOutlinedIcon from '@mui/icons-material/DeleteOutlineOutlined';
import { useEffect, useState } from "react";
import useAccounts from "../../../hooks/useAccounts";
import useSnack from "../../../hooks/useSnack";
import usePopups from "../../../hooks/usePopups";
import DeleteAccountWarning from "../Components/DeleteAccountWarning";
import useStartGame from "../../../hooks/useStartGame.jsx";

function BasicActionsWidget({ type, widgetId }) {
    const { getWidgetConfiguration, closeWidgetBar, widgetBarState } = useWidgets();
    const { refreshData } = useAccounts();
    const { showSnackbar } = useSnack();
    const { showPopup, closePopup } = usePopups();
    const { startGame } = useStartGame();

    const [isLoading, setIsLoading] = useState(false);
    const [isLoadingRefresh, setIsLoadingRefresh] = useState(false);
    const [updateInProgress, setUpdateInProgress] = useState(false);

    const account = widgetBarState.data;


    useEffect(() => {
        const checkSessionStorage = () => {
            const updInProgress = sessionStorage.getItem('updateInProgress');
            setUpdateInProgress(updInProgress === 'true');
        };
        checkSessionStorage();

        const intervalId = setInterval(checkSessionStorage, 750);
        return () => { clearInterval(intervalId); }
    }, []);

    return (
        <WidgetBase type={type} widgetId={widgetId}>
            <Box
                sx={{
                    width: '100%',
                }}
            >
                <Grid container spacing={2}>
                    <Grid size={12}>
                        <Tooltip
                            title={account.state === 'Registered' ?
                                (
                                    <Box sx={{
                                        display: 'flex',
                                        flexDirection: 'row',
                                        alignItems: 'center',
                                        justifyContent: 'center',
                                        textAlign: 'left',
                                        gap: 1,
                                    }}
                                    >
                                        <WarningAmberOutlinedIcon color="warning" />
                                        Please confirm the email by following the instructions and click "REFRESH DATA" afterwards
                                    </Box>
                                ) : ""}
                        >
                            <span>
                                <StyledButton
                                    disabled={isLoading || isLoadingRefresh || updateInProgress || account.state === 'Registered'}
                                    fullWidth={true}
                                    sx={{ height: 55 }}
                                    onClick={async () => {
                                        setIsLoading(true);
                                        await startGame(account);
                                        setIsLoading(false);
                                    }}
                                    loading={isLoading}
                                >
                                    <PlayCircleFilledWhiteOutlinedIcon size='large' sx={{ mr: 1 }} />
                                    start game
                                </StyledButton>
                            </span>
                        </Tooltip>
                    </Grid>
                    <Grid size={6}>
                        <StyledButton
                            disabled={isLoading || isLoadingRefresh}
                            fullWidth={true}
                            startIcon={<RefreshOutlinedIcon />}
                            color={account.state === 'Registered' ? "primary" : "secondary"}
                            onClick={async () => {
                                setIsLoadingRefresh(true);
                                const response = await refreshData(account.email);
                                if (response) {
                                    showSnackbar("Refreshing finished");
                                }
                                setIsLoadingRefresh(false);
                            }}
                            loading={isLoadingRefresh}
                        >
                            refresh data
                        </StyledButton>
                    </Grid>
                    <Grid size={6}>
                        {
                            <StyledButton fullWidth={true} startIcon={<DeleteOutlineOutlinedIcon />} color="secondary" sx={{
                                '&:hover': {
                                    backgroundColor: theme => theme.palette.error.main,
                                },
                            }}
                                onClick={() => {
                                    const data = {
                                        content: <DeleteAccountWarning email={account.email} onClose={closePopup} closeWidgetBar={closeWidgetBar}   />
                                    }
                                    showPopup(data);
                                }}
                            >
                                delete account
                            </StyledButton>
                        }
                    </Grid>
                </Grid>
            </Box>
        </WidgetBase>
    );
}

export default BasicActionsWidget;