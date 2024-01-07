import { TableRow, Typography } from "@mui/material";
import PaddedTableCell from "./PaddedTableCell";
import DailyLoginCheckbox from "../GridComponents/DailyLoginCheckbox";

function DailyLoginCheckBoxTableRow({ keyValue, value, onChange, ...rest }) {
    const params = {
        value: value,
    };
    
    return (
        <TableRow {...rest}>
            <PaddedTableCell>
                <Typography variant="body2" fontWeight={300} component="span" sx={{}}>
                    {keyValue}
                </Typography>
            </PaddedTableCell>
            <PaddedTableCell align="left">
                <DailyLoginCheckbox params={params} onChange={onChange} sx={{
                    ml: -1.5
                }} />
            </PaddedTableCell>
        </TableRow>
    );
}

export default DailyLoginCheckBoxTableRow;