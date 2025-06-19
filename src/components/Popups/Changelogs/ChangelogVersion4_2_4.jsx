import { Box, Typography } from '@mui/material';
import ChangelogEntry from './ChangelogEntry';
import ChangelogPopupBase from './ChangelogPopupBase';
import SortOutlinedIcon from '@mui/icons-material/SortOutlined';

function ChangelogVersion4_2_4() {
    return (
        <ChangelogPopupBase
            version="4.2.4 Pre"
            title="First dungeon bonuses and more improvements!"
            releaseDate={"11.06.2025"}
            icon={<SortOutlinedIcon />}
            width={'850px'}
        >
            <ChangelogEntry
                title={'Accounts'}
                listOfChanges={[
                    "Changed the exp datatype from int to bigint to support larger values. Thanks @m",                    
                ]}
            />
            <ChangelogEntry
                title={'Vault Peeker'}
                listOfChanges={[
                    "The first fame bonus feature got added: Dungeon bonuses. Big Thanks to @Nick for contributing this feature! 🎉",
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
                                <li>Nick - For his first contribution to EAM</li>
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


export default ChangelogVersion4_2_4;