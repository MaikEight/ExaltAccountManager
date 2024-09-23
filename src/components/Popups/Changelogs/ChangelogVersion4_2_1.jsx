import { Box, Typography } from '@mui/material';
import ChangelogEntry from './ChangelogEntry';
import ChangelogPopupBase from './ChangelogPopupBase';
import BugReportOutlinedIcon from '@mui/icons-material/BugReportOutlined';

function ChangelogVersion4_2_1() {
    return (
        <ChangelogPopupBase
            version="4.2.1 Pre"
            title="Comments, Muledump export support, Bug Fixes and Improvements!"
            releaseDate={"23.09.2024"}
            icon={<BugReportOutlinedIcon />}
            width={'950px'}
        >
            <ChangelogEntry
                title={'Accounts'}
                listOfChanges={[
                    "Added a new Comments section to the Account Details view.",
                    "Comments can also be displayed in the Accounts List.",
                ]}
            />
            <ChangelogEntry
                title={'Importer'}
                listOfChanges={[
                    "Added support for importing Muledump account export file (accounts.js).",
                ]}
            />
            <ChangelogEntry
                title={'Miscellaneous'}
                listOfChanges={[
                    "Added the new comments column to the settings (default columns).",
                    "Added a new 'Fatal Error' page to display when the application crashes.",
                    "Added a new library to use components accross multiple applications. (Preparation for the upcoming Daily Login UI overhaul)",
                    "Fixed an issue where the char/list conversion would fail if the account had no character and no classes unlocked.",
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
                                <li>n1k-o</li>
                            </ul>
                        </Typography>
                        <Typography component={'span'} variant="body2" fontWeight={'bold'} color="textSecondary">
                            <ul>
                                <li>Connie</li>
                                <li>and many more...</li>
                            </ul>
                        </Typography>
                    </Box>
                </Box>
            </Box>
        </ChangelogPopupBase >
    );
}


export default ChangelogVersion4_2_1;