import { Box, Typography } from "@mui/material";
import ComponentBox from "../../ComponentBox";

function ChangelogPopupBase({ version, releaseDate, title, icon, children, width }) {

    const getTitle = () => {
        return <Box
            sx={{
                display: 'flex',
                width: '100%',
                flexDirection: 'row',
                justifyContent: 'space-between',
                alignItems: 'center',
                gap: 2
            }}
        >
            <Box sx={{
                display: 'flex',
                flexDirection: 'row',
                alignItems: 'center',
                justifyContent: 'center',
            }}
            >
                <Typography variant="h6"
                                sx={{
                                    fontWeight: 600,
                                    textAlign: 'center',
                                }}
                >
                EAM v{version} - {title}
                </Typography>
            </Box>
            <Typography variant="body2">
                {releaseDate}
            </Typography>
        </Box>
    }

    return (
        <ComponentBox
            title={getTitle()}
            icon={icon}
            sx={{
                userSelect: "none",
                ...(width && { width: width })
            }}
        >
            <Box
                sx={{
                    mt: -1,
                    display: 'flex',
                    flexDirection: 'column',
                    gap: 1.5
                }}
            >
                {children}
            </Box>
        </ComponentBox>
    );
}

export default ChangelogPopupBase;