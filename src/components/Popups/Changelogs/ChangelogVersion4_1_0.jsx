import { Box, Typography } from '@mui/material';
import ChangelogPopupBase from './ChangelogPopupBase';
import CalendarMonthOutlinedIcon from '@mui/icons-material/CalendarMonthOutlined';
import ChangelogEntry from './ChangelogEntry';

function ChangelogVersion4_1_0() {
    return (
        <ChangelogPopupBase
            version="4.1.0"
            title="Daily Login, Logs and more!"
            releaseDate={"30.03.2024"}
            icon={<CalendarMonthOutlinedIcon />}
        >
            <ChangelogEntry
                title={'Daily Auto Login'}
                listOfChanges={[
                    "The daily auto login is back and working better than ever!",
                    "Automatically set's the correct time of day for the daily login.",
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
                    "Audit logs show all actions taken by the user, mostly regarding accounts.",
                    "Error logs show most errors that occured in the application.",
                ]}
            />
            <ChangelogEntry
                title={'Changelog Popup'}
                listOfChanges={[
                    "This changelog popup has been added and will pop up on the first start of the new version.",
                ]}
            />
            <ChangelogEntry
                title={'Miscellaneous'}
                listOfChanges={[
                    "Fixed a bug that made the HWID-Tool not work correctly for some users.",
                    "Improved the appearance of the Feedback-Button.",                    
                ]}
            />
        </ChangelogPopupBase>
    );
}

export default ChangelogVersion4_1_0;