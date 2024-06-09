import ChangelogPopupBase from './ChangelogPopupBase';
import CalendarMonthOutlinedIcon from '@mui/icons-material/CalendarMonthOutlined';
import ChangelogEntry from './ChangelogEntry';
import { Box, Typography } from '@mui/material';

function ChangelogVersion4_1_5() {
    return (
        <ChangelogPopupBase        
            version="4.1.5"
            title="Importer, Account creator, Stability impro. and more!"
            releaseDate={"02.06.2024"}
            icon={<CalendarMonthOutlinedIcon />}
            width={'750px'}
        >
            <ChangelogEntry
                title={'Im- & Exporter'}
                listOfChanges={[
                    "Added an importer and exporter for accounts. Supported formats are JSON, CSV and EAM.accounts.",
                    "The importer can be found in the add new accounts slideout.",
                    "The exporter can be found in the accounts page and supports JSON and CSV.",
                ]}
            />
            <ChangelogEntry
                title={'Create accounts'}
                listOfChanges={[
                    "Added a new feature to create accounts.",
                    "The feature can be found in the add new accounts slideout.",
                ]}
            />
            <ChangelogEntry
                title={'Stability improvements'}
                listOfChanges={[
                    "Added more error handling to the application to prevent crashes and improve stability.",
                    "A new error boundary has been added to catch errors and display a fallback UI.",
                ]}
            />
            <ChangelogEntry
                title={'Miscellaneous'}
                listOfChanges={[
                    "",
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

export default ChangelogVersion4_1_5;