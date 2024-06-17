import ChangelogPopupBase from './ChangelogPopupBase';
import CalendarMonthOutlinedIcon from '@mui/icons-material/CalendarMonthOutlined';
import ChangelogEntry from './ChangelogEntry';
import { Box, Typography } from '@mui/material';

function ChangelogVersion4_1_5() {
    return (
        <ChangelogPopupBase        
            version="4.1.5"
            title="Importer, Register Accounts, Stability Improvements. and more!"
            releaseDate={"18.06.2024"}
            icon={<CalendarMonthOutlinedIcon />}
            width={'850px'}
        >
            <ChangelogEntry
                title={'Im- & Exporter'}
                listOfChanges={[
                    "Added an importer and exporter for accounts. Supported formats are JSON, CSV and EAM.accounts.",
                    "The importer can be found in the add new accounts slideout.",
                    "The exporter can be found in the toolbar of the accounts page and supports JSON and CSV.",
                ]}
            />
            <ChangelogEntry
                title={'Register Accounts'}
                listOfChanges={[
                    "Added a new feature to register accounts.",
                    "The feature can be found in the add new accounts slideout.",
                ]}
            />
            <ChangelogEntry
                title={'Stability Improvements'}
                listOfChanges={[
                    "Added more error handling to the application to prevent crashes and improve stability.",
                    "A new error boundary has been added to catch errors and display a fallback UI.",
                ]}
            />
            <ChangelogEntry
                title={'Miscellaneous'}
                listOfChanges={[
                    "Added new devtools for debugging purposes.",
                    "Added a new console greeting message.",
                    "Improved the Toolbar of the Accounts page.",
                    "Improved the border radius of the application. It is now more rounded. 6px -> 9px.",
                    "Improved the Daily Logins graph UI.",
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
                                <li>Crixxxu</li>
                            </ul>
                        </Typography>
                        <Typography component={'span'} variant="body2" fontWeight={'bold'} color="textSecondary">
                            <ul>
                                <li>Avyora</li>
                                <li>and more...</li>
                            </ul>
                        </Typography>
                    </Box>
                </Box>
            </Box>
        </ChangelogPopupBase >
    );
}

export default ChangelogVersion4_1_5;