import { TableRow, Typography } from "@mui/material";
import PaddedTableCell from "./PaddedTableCell";
import DailyLoginCheckbox from "../GridComponents/DailyLoginCheckbox";

function DailyLoginCheckBoxTableRow({ keyValue, value, ...rest }) {
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
                <DailyLoginCheckbox params={params} sx={{
                    ml: -1.5
                }} />
            </PaddedTableCell>
        </TableRow>
    );
}

export default DailyLoginCheckBoxTableRow;