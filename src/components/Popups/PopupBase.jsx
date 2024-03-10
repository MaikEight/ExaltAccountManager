import { Box, Typography } from "@mui/material";
import ComponentBox from "../ComponentBox";

function PopupBase({ title, children }) {
    return (
        <ComponentBox>
            <Box
                sx={{
                    display: 'flex',
                    flexDirection: 'column',
                    gap: 2,
                }}
            >
                {title &&
                    <Box
                        sx={{
                            display: 'flex',
                            flexDirection: 'row',
                            alignItems: 'center',
                            justifyContent: 'center',
                            gap: 1,
                        }}
                    >
                        <Typography
                            variant="h6"
                            sx={{
                                fontWeight: 600,
                                textAlign: 'center',
                            }}
                        >
                            {title}
                        </Typography>
                    </Box>
                }
                {children}
            </Box>
        </ComponentBox>
    );
}

export default PopupBase;