import { Box, Typography } from "@mui/material";
import StyledButton from "../StyledButton";
import AddCircleIcon from '@mui/icons-material/AddCircle';
import useAccounts from "../../hooks/useAccounts";
import { MASCOT_NAME } from "../../constants";
import { useRef } from "react";

const notFoundTexts = [
    `No accounts here... maybe ${MASCOT_NAME} took a wrong turn.`,
    `The search party led by ${MASCOT_NAME} came back empty-handed.`,
    `Well, this is awkward. ${MASCOT_NAME} found absolutely nothing.`,
    `Even ${MASCOT_NAME}'s keen eyes couldn’t spot a single account.`,
    `Accounts? What accounts? ${MASCOT_NAME} sees only tumbleweeds.`,
    `After intense investigation, ${MASCOT_NAME} concluded: no accounts.`,
    `${MASCOT_NAME} swears there were accounts here a minute ago.`,
    `The trail has gone cold. ${MASCOT_NAME} has no leads.`,
    `If there were accounts here, ${MASCOT_NAME} would’ve found them. Promise.`,
    `Not a single account in sight. ${MASCOT_NAME} suspects foul play.`,
    `Despite heroic effort, ${MASCOT_NAME} has nothing to show for it.`,
    `${MASCOT_NAME} even checked under the digital couch cushions. Nada.`,
    `It’s like the accounts vanished into thin air.`,
    `Some say the accounts are shy. ${MASCOT_NAME} says they're hiding.`,
    `${MASCOT_NAME} opened every drawer — not even a username left behind.`,
    `${MASCOT_NAME} would’ve found them… if they existed.`,
    `The database is silent. Too silent. ${MASCOT_NAME}’s suspicious.`,
    `Nothing to see here. ${MASCOT_NAME} recommends snacks and a retry.`,
    `${MASCOT_NAME}'s account search is complete. Result: a majestic void.`,
    `${MASCOT_NAME} searched high and low, but found no accounts`,
    `${MASCOT_NAME} has lost its way`,,
];

function NoRowsOverlay({ onAddNew }) {
    const { accounts } = useAccounts();
    const textToDisplay = useRef(
        notFoundTexts[Math.floor(Math.random() * notFoundTexts.length)]
    );
    const imageRef = useRef(Math.random() < 0.5 ? '/mascot/Search/no_accounts_1_small_very_low_res.png' : '/mascot/Search/no_accounts_2_very_low_res.png');

    return (
        <Box
            sx={{
                display: 'flex',
                alignItems: 'center',
                justifyContent: 'center',
                height: '100%',
                width: '100%',
            }}
        >
            <Box
                sx={{
                    position: 'relative',
                    display: 'flex',
                    flexDirection: 'column',
                    alignItems: 'center',
                    justifyContent: 'center',
                    height: '100%',
                    width: '100%',
                }}
            >
                <Box
                    sx={{
                        maxWidth: 'calc(min(30%, 300px))',
                    }}
                >
                    <img
                        src={imageRef?.current ? imageRef.current : "/mascot/Search/no_accounts_1_small_very_low_res.png"}
                        alt="No accounts found"
                        style={{
                            width: '100%',
                            height: 'auto',
                            maxHeight: '100%',
                        }}
                    />
                </Box>
                <Typography variant="h6">
                    {accounts.length === 0 ? 'No accounts found' : textToDisplay?.current ? textToDisplay?.current : `${MASCOT_NAME} could not find anything here`}
                </Typography>
                {
                    accounts.length === 0 &&
                    <StyledButton
                        sx={{ mt: 2 }}
                        startIcon={<AddCircleIcon />}
                        onClick={() => onAddNew?.()}
                        color="primary"
                        variant="contained"
                    >
                        Add your first one here
                    </StyledButton>
                }
            </Box>
        </Box>
    );
}

export default NoRowsOverlay;

