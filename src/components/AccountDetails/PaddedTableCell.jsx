import { IconButton, TableCell, TextField, Tooltip } from "@mui/material";
import ContentCopyOutlinedIcon from '@mui/icons-material/ContentCopyOutlined';
import Visibility from '@mui/icons-material/Visibility';
import VisibilityOff from '@mui/icons-material/VisibilityOff';
import { useEffect, useState } from "react";
import useSnack from "../../hooks/useSnack";

function PaddedTableCell({ children, sx, isEditMode, isPassword, onChange, allowCopy, copyHint, ...props }) {
    const [showPassword, setShowPassword] = useState(false);
    const { showSnackbar } = useSnack();

    useEffect(() => {
        setShowPassword(false);
    }, [isEditMode]);

    return (
        <TableCell {...props} sx={{ pl: 4, pr: 4, pt: 1.5, pb: 1.5, ...sx }}>
            {isEditMode ?
                (
                    <>
                        <TextField
                            hiddenLabel
                            variant="standard"
                            size="small"
                            InputProps={{
                                style: {
                                    fontSize: '0.875rem',
                                    fontWeight: 300,
                                    fontFamily: 'Roboto',
                                    padding: 0,
                                },
                            }}
                            value={children.props.children}
                            onChange={(event) => onChange(event.target.value)}
                            type={isPassword && !showPassword ? "password" : "text"}
                        />
                        {
                            isPassword &&
                            <IconButton
                                sx={{ ml: -3.25 }}
                                size="small"
                                onClick={() => setShowPassword(!showPassword)}
                            >
                                {showPassword ? <Visibility fontSize="small" /> : <VisibilityOff fontSize="small" />}
                            </IconButton>
                        }
                    </>
                )
                :
                children
            }
            {
                allowCopy && !isEditMode ?
                    <Tooltip
                        title="Copy to clipboard"
                    >
                        <IconButton
                            size="small"
                            sx={{
                                ml: 0.5
                            }}
                            onClick={(e) => {
                                e.stopPropagation();
                                navigator.clipboard.writeText(children.props.children);
                                showSnackbar(`Copied${copyHint ? ' ' + copyHint : ''} to clipboard`);
                            }}
                        >
                            <ContentCopyOutlinedIcon fontSize="small" />
                        </IconButton>
                    </Tooltip>
                    : null
            }
        </TableCell>
    );
}

export default PaddedTableCell;