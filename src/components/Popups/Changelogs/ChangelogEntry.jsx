import { Box, Typography } from "@mui/material";

function ChangelogEntry({ title, listOfChanges }) {

    return (
        <Box
            sx={{
                display: 'flex',
                flexDirection: 'column',
                justifyContent: 'center',
                alignItems: 'start',
                gap: 0.5
            }}
        >
            <Typography variant="h6">
                {title}
            </Typography>
            <Box
                sx={{
                    display: 'flex',
                    flexDirection: 'column',
                    gap: 0.25,
                    pl: 1
                }}
            >
                {
                    !listOfChanges ? null :
                        listOfChanges.length > 1 && typeof listOfChanges !== 'string' ?
                            listOfChanges.map((change, index) => (
                                <Typography component={'div'} key={index} variant="body2">
                                    • {change}
                                </Typography>
                            ))
                            :
                            <Box
                                sx={{
                                    display: 'flex',
                                    flexDirection: 'row',
                                    gap: '0.25rem',
                                    alignItems: 'start'
                                }}
                            >
                                {
                                    typeof listOfChanges === 'string' &&
                                    <Typography variant="body2">
                                        •
                                    </Typography>
                                }
                                <Typography variant="body2">
                                    {listOfChanges}
                                </Typography>
                            </Box>
                }
            </Box>

        </Box>
    )

}

export default ChangelogEntry;