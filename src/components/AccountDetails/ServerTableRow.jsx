import { Box, IconButton, TableRow, Tooltip, Typography } from "@mui/material";
import PaddedTableCell from "./PaddedTableCell";
import ServerChip from "../GridComponents/ServerChip";
import { useState } from "react";
import ServerListSelect from "../ServerListSelect";
import SaveOutlinedIcon from '@mui/icons-material/SaveOutlined';


function ServerTableRow({ keyValue, value, showSaveButton, onSave, ...rest }) {
    const [selected, setSelected] = useState(value);

    const { onChange } = rest;
    const params = {
        value: value,
    };

    const handleChangeServer = (server) => {
        setSelected(server);
        onChange?.(server);
    };

    return (
        <TableRow {...rest}>
            <PaddedTableCell>
                <Typography variant="body2" fontWeight={300} component="span" sx={{}}>
                    {keyValue}
                </Typography>
            </PaddedTableCell>
            <PaddedTableCell align="left">
                {
                    !onChange ?
                        <ServerChip params={params} sx={{ ml: -0.15 }} />
                        :
                        <Box
                            sx={{
                                display: 'flex',
                                alignItems: 'center',
                                justifyContent: 'start',
                                gap: 1,
                            }}
                        >
                            <ServerListSelect
                                serversToAdd={[
                                    { Name: 'Default', DNS: 'DEFAULT' },
                                    { Name: 'Last server', DNS: 'LAST' },
                                ]}
                                selectedServer={selected ?? 'Default'}
                                onChange={handleChangeServer}
                                defaultValue={'Default'}
                            />
                            {
                                showSaveButton &&
                                <Tooltip title="Save">
                                    <IconButton
                                        size="small"
                                        onClick={() => onSave?.(selected)}
                                    >
                                        <SaveOutlinedIcon />
                                    </IconButton>
                                </Tooltip>
                            }
                        </Box>
                }
            </PaddedTableCell>
        </TableRow>
    );
}

export default ServerTableRow;