import { TableRow, Typography } from "@mui/material"
import PaddedTableCell from "./PaddedTableCell"
import GroupUI from "../GridComponents/GroupUI"

function GroupRow({group, innerSx, ...rest}) {

  return (
    <TableRow {...rest}>
      <PaddedTableCell sx={innerSx}>
        <Typography variant="body2" fontWeight={300} component="span">
          Group
        </Typography>
      </PaddedTableCell>
      <PaddedTableCell sx={innerSx}>
        {group ? <GroupUI group={group} /> : null}
      </PaddedTableCell>
    </TableRow>
  )
}

export default GroupRow