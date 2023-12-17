import { TableCell } from "@mui/material";

function PaddedTableCell({ children, sx, ...props }) {
    return (
        <TableCell {...props} sx={{ pl: 4, pr: 4, pt: 1.5, pb: 1.5, ...sx }}>
            {children}
        </TableCell>
    );
}

export default PaddedTableCell;