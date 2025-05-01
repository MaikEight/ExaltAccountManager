import { TableRow, Tooltip, Typography } from "@mui/material";
import PaddedTableCell from "./PaddedTableCell";
import OpenInNewOutlinedIcon from '@mui/icons-material/OpenInNewOutlined';
import { useTheme } from "@emotion/react";

function SteamworksRow({ guid, innerSx, ...rest }) {
    const steamId = guid.split(':')[1];
    const theme = useTheme();
    return (
        <TableRow {...rest}>
            <PaddedTableCell sx={innerSx ? { ...innerSx} : { }}>

                <Typography variant="body2" fontWeight={300} component="span">
                    Steam ID
                </Typography>
            </PaddedTableCell>
            <PaddedTableCell sx={innerSx ? { ...innerSx} : { }} align="left">
                <Tooltip title="Open Steam profile">
                    <a
                        href={`https://steamcommunity.com/profiles/${steamId}`}
                        target="_blank"
                        rel="noopener noreferrer"
                        style={{ display: 'flex', alignItems: 'center', textDecoration: 'none' }}
                    >
                        <img src={theme.palette.mode === 'dark' ? "/steam.svg" : "/steam_light_mode.svg"} alt="Steam Logo" height='20px' width='20px' style={{ marginRight: '6px' }} />
                        <Typography variant="body2" fontWeight={300} component="span" sx={{ textAlign: 'center', mr: 0.5 }}>
                            {steamId}
                        </Typography>
                        <OpenInNewOutlinedIcon />
                    </a>
                </Tooltip>
            </PaddedTableCell>
        </TableRow>
    );
}

export default SteamworksRow;