import { TableRow, Typography } from "@mui/material";
import PaddedTableCell from "./PaddedTableCell";

function TextTableRow({ keyValue, value, innerSx, editMode, isPassword, onChange, allowCopy, ...rest }) {
  return (
    <TableRow {...rest}>
      <PaddedTableCell sx={innerSx}>
        <Typography variant="body2" fontWeight={300} component="span">
          {keyValue}
        </Typography>
      </PaddedTableCell>
      <PaddedTableCell sx={innerSx} isEditMode={editMode} isPassword={isPassword} onChange={onChange} allowCopy={allowCopy} align="left">
        <Typography variant="body2" fontWeight={300} component="span" sx={{ textAlign: 'center'}}>
          {value}
        </Typography>
      </PaddedTableCell>
    </TableRow>
  );
}

export default TextTableRow;