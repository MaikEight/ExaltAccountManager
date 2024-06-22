import { Box, Typography } from '@mui/material';
import ChangelogEntry from './ChangelogEntry';
import ChangelogPopupBase from './ChangelogPopupBase';
import CelebrationOutlinedIcon from '@mui/icons-material/CelebrationOutlined';

function ChangelogVersion4_2_0() {
    return (
        <ChangelogPopupBase
            version="4.2.0"
            title="Vault Peeker, Server list and bug fixes!"
            releaseDate={"26.07.2024"}
            icon={<CelebrationOutlinedIcon />}
        >
            <ChangelogEntry
                title={'Vault Peeker'}
                listOfChanges={[
                    "TBD",
                ]}
            />
            <ChangelogEntry
                title={'Server List'}
                listOfChanges={[
                    "Added a new dropdown to select the next server to join inside the Account Details.",
                    "If you don't save the server, it will only be used for the next login / until the Account Details close.",
                ]}
            />
            <ChangelogEntry
                title={'Miscellaneous'}
                listOfChanges={[
                    "Improved the file-logs of the whole application.",
                    "Improved the Daily Logins Page Layout and Table rendering.",
                    "Improved the border radius of the Close Button.",
                    "Fixed the 'Back'-Button in the Register Accounts view to not close the side panel.",
                    "Fixed timestamps in the Logs to display in the correct timezone instead of UTC.",
                    "Error Logs now get deleted after 7 days automatically to reduce database size.",
                ]}
            />
            <Box
                sx={{
                    display: 'flex',
                    flexDirection: 'column',
                    gap: 2
                }}
            >
                <Box>
                    <Typography variant="body1" color="textSecondary">
                        Special thanks to all beta testers and everyone who provided feedback!
                    </Typography>
                    <Box
                        sx={{
                            display: 'flex',
                            flexDirection: 'row',
                            justifyContent: 'start',
                            gap: 1,
                            mt: -0.5
                        }}
                    >
                        <Typography component={'span'} variant="body2" fontWeight={'bold'} color="textSecondary">
                            <ul>
                                <li>TheDangerScrew</li>
                                <li>Pro90</li>
                                <li>N1k-o</li>
                            </ul>
                        </Typography>
                        <Typography component={'span'} variant="body2" fontWeight={'bold'} color="textSecondary">
                            <ul>
                                <li>Robin</li>
                                <li>BlastaMan</li>
                                <li>and many more...</li>
                            </ul>
                        </Typography>
                    </Box>
                </Box>
            </Box>
        </ChangelogPopupBase >
    );
}

export default ChangelogVersion4_2_0;