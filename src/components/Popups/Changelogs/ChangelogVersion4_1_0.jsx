import ChangelogPopupBase from './ChangelogPopupBase';
import CalendarMonthOutlinedIcon from '@mui/icons-material/CalendarMonthOutlined';
import ChangelogEntry from './ChangelogEntry';
import { Box, Typography } from '@mui/material';

function ChangelogVersion4_1_0() {
    return (
        <ChangelogPopupBase
            version="4.1.0"
            title="Daily Login, Logs and more!"
            releaseDate={"24.05.2024"}
            icon={<CalendarMonthOutlinedIcon />}
        >
            <ChangelogEntry
                title={'Daily Auto Login'}
                listOfChanges={[
                    "The daily auto login is back and working better than ever!",
                    "Automatically sets the correct time of day for the daily login.",
                    "Better logs for the daily login.",
                    "Fixed many bugs where the daily login would not work correctly.",
                ]}
            />
            <ChangelogEntry
                title={'Logs'}
                listOfChanges={[
                    "Added a new logs feature.",
                    "Logs can be found in the sidebar menu.",
                    "There are two types of logs: Audit logs and Error logs.",
                    "Audit logs shows actions taken by the user, mostly regarding accounts.",
                    "Error logs shows errors that occured in the application.",
                ]}
            />
            <ChangelogEntry
                title={'Changelog Popup'}
                listOfChanges={
                    "This changelog popup has been added and will pop up on the first start of a new version."
                }
            />
            <ChangelogEntry
                title={'Miscellaneous'}
                listOfChanges={[
                    "Fixed a bug that made the HWID-Tool not work correctly for some users.",
                    "Improved the appearance of the Settings-Page.",
                    "Improved the appearance of the Feedback-Button.",
                ]}
            />
            <ChangelogEntry
                title={'Bonus: Website'}
                listOfChanges={
                    <span>
                        EAM now has a website: <a href="https://exaltaccountmanager.com" target="_blank" rel="noreferrer">ExaltAccountManager.com</a>
                    </span>
                }
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

export default ChangelogVersion4_1_0;