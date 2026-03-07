import { useEffect, useState } from "react";
import WidgetBase from "./WidgetBase";
import useWidgets from "../../../hooks/useWidgets";
import useAccounts from "../../../hooks/useAccounts";
import useSnack from "../../../hooks/useSnack";
import { Box, IconButton, TextField, Tooltip } from "@mui/material";
import SaveOutlinedIcon from '@mui/icons-material/SaveOutlined';

function CommentWidget({ type, widgetId }) {
    const { widgetBarState } = useWidgets();
    const { updateAccount } = useAccounts();
    const { showSnackbar } = useSnack();

    const account = widgetBarState?.data;
    const savedComment = account?.comment ?? '';

    const [comment, setComment] = useState(savedComment);

    useEffect(() => {
        setComment(account?.comment ?? '');
    }, [account?.email]);

    const isDirty = comment !== savedComment;

    const handleSave = async () => {
        if (!account) return;
        await updateAccount({ ...account, comment });
        showSnackbar("Comment saved", 'success');
    };

    return (
        <WidgetBase type={type} widgetId={widgetId}>
            <Box sx={{ width: '100%', p: 1 }}>
                <TextField
                    value={comment}
                    onChange={(e) => setComment(e.target.value)}
                    placeholder="Add a comment..."
                    multiline
                    minRows={3}
                    maxRows={8}
                    fullWidth
                    size="small"
                    slotProps={{
                        input: {
                            endAdornment: (
                                <Tooltip title="Save comment">
                                    <span>
                                        <IconButton
                                            size="small"
                                            disabled={!isDirty}
                                            onClick={handleSave}
                                            sx={{ alignSelf: 'center' }}
                                        >
                                            <SaveOutlinedIcon fontSize="small" />
                                        </IconButton>
                                    </span>
                                </Tooltip>
                            ),
                        },
                    }}
                />
            </Box>
        </WidgetBase>
    );
}

export default CommentWidget;