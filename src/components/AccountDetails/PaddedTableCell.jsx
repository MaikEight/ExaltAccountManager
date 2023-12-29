import { IconButton, TableCell } from "@mui/material";
import ContentCopyOutlinedIcon from '@mui/icons-material/ContentCopyOutlined';

function PaddedTableCell({ children, sx, isEditMode, allowCopy, ...props }) {
    return (
        <TableCell {...props} sx={{ pl: 4, pr: 4, pt: 1.5, pb: 1.5, ...sx }}>
            {children}
            {
                allowCopy && !isEditMode ?
                    <IconButton
                        size="small"
                        sx={{
                           ml: 0.5
                        }}
                        onClick={(e) => {
                            e.stopPropagation();
                            console.log(children);
                            navigator.clipboard.writeText(children.props.children);
                        }}
                    >
                        <ContentCopyOutlinedIcon fontSize="small" />
                    </IconButton>
                    : null
            }
        </TableCell>
    );
}

export default PaddedTableCell;