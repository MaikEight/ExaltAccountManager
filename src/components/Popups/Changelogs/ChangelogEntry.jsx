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
                    listOfChanges && listOfChanges.length > 1 ?
                        listOfChanges.map((change, index) => (
                            <Typography key={index} variant="body1">
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
                            <Typography variant="body1">
                                •
                            </Typography>
                            <Typography variant="body1">
                                {listOfChanges}
                            </Typography>
                        </Box>
                }
            </Box>

        </Box>
    )

}

export default ChangelogEntry;