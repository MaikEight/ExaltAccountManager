import { Box, Typography } from '@mui/material';
import ChangelogEntry from './ChangelogEntry';
import ChangelogPopupBase from './ChangelogPopupBase';
import BugReportOutlinedIcon from '@mui/icons-material/BugReportOutlined';

function ChangelogVersion4_2_2() {
    return (
        <ChangelogPopupBase
            version="4.2.2 Pre"
            title="Comments, Muledump export support, Bug Fixes and Improvements!"
            releaseDate={"23.09.2024"}
            icon={<BugReportOutlinedIcon />}
            width={'950px'}
        >
            <ChangelogEntry
                title={'Vault Peeker'}
                listOfChanges={[
                    "Added support for hidding vault types.",
                    "Updated the items and pictures.",
                ]}
            />
            <ChangelogEntry
                title={'Daily Logins'}
                listOfChanges={[
                    'Fixed the columns "🕛 End Time" and "⏱️ Duration" to behave correctly while the Daily Login taks is running.',
                ]}
            />
            <ChangelogEntry
                title={'Miscellaneous'}
                listOfChanges={[
                    "Improved error message when refreshing of an account / logging in fails.",
                    "Fixed the account name fetching not working.",
                    "Fixed spelling mistakes.",
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
                                <li>Surrender</li>
                                <li>Connie</li>
                                <li>Rodrigo</li>
                            </ul>
                        </Typography>
                        <Typography component={'span'} variant="body2" fontWeight={'bold'} color="textSecondary">
                            <ul>                                
                                <li>and many more...</li>
                            </ul>
                        </Typography>
                    </Box>
                </Box>
            </Box>
        </ChangelogPopupBase >
    );
}


export default ChangelogVersion4_2_2;