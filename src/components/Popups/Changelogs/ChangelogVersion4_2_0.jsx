import { Box, Typography } from '@mui/material';
import ChangelogEntry from './ChangelogEntry';
import ChangelogPopupBase from './ChangelogPopupBase';
import CelebrationOutlinedIcon from '@mui/icons-material/CelebrationOutlined';

function ChangelogVersion4_2_0() {
    return (
        <ChangelogPopupBase
            version="4.2.0"
            title="Vault Peeker, Server list and bug fixes!"
            releaseDate={"07.08.2024"}
            icon={<CelebrationOutlinedIcon />}
            width={'700px'}
        >
            <ChangelogEntry
                title={'Vault Peeker'}
                listOfChanges={[
                    "View all your items on all your accounts in one place.",
                    "Useful and extensive filters allow you to find the items or accounts you are looking for.",
                    "Export your items as an image to share with your friends.",
                    "Note: Ordering accounts is not possible yet, but will be added in a future update.",
                ]}
            />
            <ChangelogEntry
                title={'Server List'}
                listOfChanges={[
                    "Added a dropdown menu in Account Details to select the next server to join."
                ]}
            />
            <ChangelogEntry
                title={'Credits & Thanks'}
                listOfChanges={[
                    "Added a new Credits and Thanks section to the About page."
                ]}
            />
            <ChangelogEntry
                title={'Miscellaneous'}
                listOfChanges={[
                    "Added a new Discord invite link to the sidebar.",
                    "Added a new Vault Peeker section to the settings.",
                    "Improved application-wide file logs.",
                    "Improved the Daily Logins Page Layout and Table rendering.",
                    "Improved the border radius of the Close Button.",
                    "Improved the DataGrid styling.",
                    "Fixed the 'Back' button in the Register Accounts view to not close the side panel.",
                    "Fixed log timestamps to display in the correct timezone instead of UTC.",
                    "Error logs now get deleted after 7 days automatically to reduce database size.",
                    "Various code improvements and bug fixes."
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
                                <li>faynt</li>
                                <li>TheDangerScrew</li>
                                <li>Pro90</li>
                            </ul>
                        </Typography>
                        <Typography component={'span'} variant="body2" fontWeight={'bold'} color="textSecondary">
                            <ul>
                                <li>n1k-o</li>                     
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