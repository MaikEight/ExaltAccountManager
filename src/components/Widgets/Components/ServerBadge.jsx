import { Box, Typography, Divider, Menu, MenuItem } from '@mui/material';
import { useTheme } from '@emotion/react';
import useStringToColor from '../../../hooks/useStringToColor';
import useServerList from '../../../hooks/useServerList';
import KeyboardArrowDownRoundedIcon from '@mui/icons-material/KeyboardArrowDownRounded';
import { useState } from 'react';
import ServerChip from '../../GridComponents/ServerChip';

function ServerBadge({ serverName, editable, onChange }) {
    serverName = serverName ?? "Default";
    editable = editable && typeof onChange === 'function';
    const [anchorEl, setAnchorEl] = useState(null);
    const { serverList } = useServerList();
    const theme = useTheme();

    const clr = useStringToColor(serverName);

    const handleClick = (event) => {
        if (editable) {
            setAnchorEl(event.currentTarget);
        }
    };

    const handleClose = () => {
        setAnchorEl(null);
    };

    const handleServerSelect = (server) => {
        onChange?.(server.Name);
        handleClose();
    };

    return (
        <>
            <Box
                sx={{
                    ...clr,
                    display: 'flex',
                    alignItems: 'center',
                    justifyContent: 'center',
                    borderRadius: '12px',
                    height: '24px',
                    width: 'fit-content',
                    p: 0.25,
                    px: 1,
                    ...(editable ? {
                        pr: 0.25,
                        cursor: 'pointer',
                    } : {})
                }}
                onClick={handleClick}
            >
                <Typography variant='body2'>
                    {serverName}
                </Typography>
                {
                    editable && (
                        <>
                            <Divider orientation="vertical" flexItem sx={{ ml: 0.625, my: 0.5 }} />
                            <KeyboardArrowDownRoundedIcon fontSize="small" />
                        </>
                    )
                }
            </Box>
            {editable && (
                <Menu
                    anchorEl={anchorEl}
                    open={Boolean(anchorEl)}
                    onClose={handleClose}
                    slotProps={{
                        paper: {
                            sx: {
                                maxHeight: 48 * 4.5 + 8,
                                width: 150,
                            }
                        }
                    }}
                >
                    {
                        serverList && serverList.length > 0 && [
                            { Name: 'Default', DNS: 'DEFAULT' },
                            { Name: 'Last server', DNS: 'LAST' },
                            ...serverList].map((server) => (
                                <MenuItem
                                    key={server.DNS}
                                    onClick={() => handleServerSelect(server)}
                                    selected={server.Name === serverName}
                                    sx={{
                                        '&.Mui-selected': {
                                            backgroundColor: theme.palette.action.selected,
                                        },
                                        '&.Mui-selected:hover': {
                                            backgroundColor: theme.palette.action.hover,
                                        },
                                        display: 'flex',
                                        justifyContent: 'center',
                                        alignItems: 'center',
                                    }}
                                >
                                    <ServerChip params={{ value: server.Name }} />
                                </MenuItem>
                            ))
                    }
                </Menu>
            )}
        </>
    );
}

export default ServerBadge;