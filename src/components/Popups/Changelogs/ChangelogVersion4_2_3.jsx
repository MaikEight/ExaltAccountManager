import { Box, Typography } from '@mui/material';
import ChangelogEntry from './ChangelogEntry';
import ChangelogPopupBase from './ChangelogPopupBase';
import SortOutlinedIcon from '@mui/icons-material/SortOutlined';

function ChangelogVersion4_2_3() {
    return (
        <ChangelogPopupBase
            version="4.2.3 Pre"
            title="Ordering, Emojis, Settings improvements and Bug Fixes!"
            releaseDate={"07.03.2025"}
            icon={<SortOutlinedIcon />}
            width={'850px'}
        >
            <ChangelogEntry
                title={'Accounts'}
                listOfChanges={[
                    "Added the ability to order accounts by Order ID.",
                    "Added emojis to the column headers.",
                ]}
            />
            <ChangelogEntry
                title={'Settings'}
                listOfChanges={[
                    "Added the ability to hide emojis in the account grid.",
                    "Added a Privacy & Legal section.",
                    "Improved the settings page.",
                ]}
            />
            <ChangelogEntry
                title={'Miscellaneous'}
                listOfChanges={[
                    "Added new illustrations at some places. Thanks to undraw.co. ✨",
                    "Fixed a bug where the Last Login value was not updated correctly or at all.",
                    "Fixed the Thanks & Credits popup width.",
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
                                <li>Tadus-Pro</li>
                            </ul>
                        </Typography>
                        <Typography component={'span'} variant="body2" fontWeight={'bold'} color="textSecondary">
                            <ul>                                
                                <li>rengarbage</li>
                                <li>and many more...</li>
                            </ul>
                        </Typography>
                    </Box>
                </Box>
            </Box>
        </ChangelogPopupBase >
    );
}


export default ChangelogVersion4_2_3;