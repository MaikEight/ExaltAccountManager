import { TableRow, Typography } from "@mui/material";
import PaddedTableCell from "./PaddedTableCell";

function TextTableRow({ keyValue, value, innerSx, ...rest }) {
  return (
    <TableRow {...rest}>
      <PaddedTableCell sx={innerSx}>
        <Typography variant="body2" fontWeight={100} component="span" sx={{}}>
          {keyValue}
        </Typography>
      </PaddedTableCell>
      <PaddedTableCell sx={innerSx}>
        <Typography variant="body2" fontWeight={100} component="span" sx={{ textAlign: 'center'}}>
          {value}
        </Typography>
      </PaddedTableCell>
    </TableRow>
  );
}

export default TextTableRow;