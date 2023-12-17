import { TableRow, Typography } from "@mui/material";
import PaddedTableCell from "./PaddedTableCell";
import ServerChip from "../GridComponents/ServerChip";

function ServerTableRow({ keyValue, value, ...rest }) {
    const params = {
        value: value,
    };
    return (
        <TableRow {...rest}>
            <PaddedTableCell>
                <Typography variant="body2" fontWeight={100} component="span" sx={{}}>
                    {keyValue}
                </Typography>
            </PaddedTableCell>
            <PaddedTableCell>
                <ServerChip params={params} sx={{ ml: -0.15 }} />
            </PaddedTableCell>
        </TableRow>
    );
}

export default ServerTableRow;