import { IconButton, InputAdornment, TableCell, TextField } from "@mui/material";
import ContentCopyOutlinedIcon from '@mui/icons-material/ContentCopyOutlined';
import Visibility from '@mui/icons-material/Visibility';
import VisibilityOff from '@mui/icons-material/VisibilityOff';
import { useEffect, useState } from "react";

function PaddedTableCell({ children, sx, isEditMode, isPassword, onChange, allowCopy, ...props }) {
    const [showPassword, setShowPassword] = useState(false);

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