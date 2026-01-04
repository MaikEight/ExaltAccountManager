
import { Box, Paper, Typography, IconButton, Chip, Tooltip } from '@mui/material';
import useWidgets from '../../../../hooks/useWidgets';
import CloseIcon from '@mui/icons-material/Close';
import ServerBadge from '../../Components/ServerBadge';
import useAccounts from '../../../../hooks/useAccounts';
import RequestStateChip from '../../../GridComponents/RequestStateChip';
import { GroupUI } from '../../../GridComponents/GroupUI';
import { getRelativeTimeString } from 'eam-commons-js';
import { useColorList } from '../../../../hooks/useColorList';
import RefreshRoundedIcon from '@mui/icons-material/RefreshRounded';
import EditOutlinedIcon from '@mui/icons-material/EditOutlined';
import EamIconButton from '../../../EamIconButton';

function AccountHeader() {
    const { widgetBarState, closeWidgetBar, updateWidgetBarEditMode } = useWidgets();
    const { updateAccount, getAccountByEmail } = useAccounts();

    console.log('AccountHeader render', widgetBarState);
    if (!widgetBarState?.data) {
        return null;
    }

    const acc = widgetBarState.data;
    const relativeLastRefresh = acc.lastRefresh ? getRelativeTimeString(acc.lastRefresh) : 'Never';
    /**
     * If the last refresh in null or older than 1 month, colorName is error
     * If the last refresh is older than 3 day, colorName is warning
     * Otherwise, colorName is success
     */
    const lastRefreshColorText = (
        !acc.lastRefresh ? 'error' :
            (Date.now() - new Date(acc.lastRefresh).getTime() > 30 * 24 * 60 * 60 * 1000) ? 'error' :
                (Date.now() - new Date(acc.lastRefresh).getTime() > 3 * 24 * 60 * 60 * 1000) ? 'warning' :
                    'success'
    );
    const lastRefreshColor = useColorList(lastRefreshColorText);

    const handleChangeServer = async (server) => {
        const contextAcc = getAccountByEmail(acc.email);
        if (contextAcc) {
            await updateAccount({ ...contextAcc, serverName: server });
        }
    };

    return (
        <Box
            sx={{
                width: '100%',
                p: 1,
                border: (theme) => `1px solid ${theme.palette.divider}`,
                borderRadius: (theme) => `${theme.shape.borderRadius * 2}px 0 ${theme.shape.borderRadius * 2}px ${theme.shape.borderRadius * 2}px`,
                backgroundColor: 'background.paperLight',
            }}
        >
            {/* Header Actions */}
            <Box
                data-tauri-drag-region
                sx={{
                    position: 'relative',
                    mb: 1.5,
                }}
            >
                <EamIconButton
                    icon={<CloseIcon fontSize='small' />}
                    onClick={closeWidgetBar}
                />

                <Typography variant="h6" component="span" sx={{ position: 'absolute', left: '50%', transform: 'translateX(-50%)' }}>
                    Details
                </Typography>
                <EamIconButton
                    sx={{
                        position: 'absolute',
                        right: 0,
                    }}
                    icon={<EditOutlinedIcon fontSize='small' />}
                    tooltip={'Edit Layout'}
                    tooltipDirection='left'
                    onClick={() => updateWidgetBarEditMode(!widgetBarState.editMode)}
                />
            </Box>
            {/* Account Info */}

            <Box
                sx={{
                    display: 'flex',
                    flexDirection: 'row',
                    alignItems: 'center',
                    gap: 2,
                    px: 0.5,
                }}
            >
                {
                    acc.group && (
                        <Box
                            sx={{
                                scale: '1.25',
                            }}
                        >
                            <GroupUI group={acc.group} size={64} />
                        </Box>
                    )
                }
                <Box>
                    <Box
                        sx={{
                            display: 'flex',
                            flexDirection: 'column',
                            alignItems: 'start',
                            gap: 0
                        }}
                    >
                        <Box
                            sx={{
                                display: 'flex',
                                flexDirection: 'row',
                                alignItems: 'center',

                                gap: 1,
                            }}
                        >
                            <Typography variant="h6" component="h6">
                                {acc.name}
                            </Typography>
                            <Box
                                sx={{
                                    display: 'flex',
                                    flexDirection: 'row',
                                    alignItems: 'start',
                                    justifyContent: 'start',
                                    height: '100%',
                                    mb: 0.5,
                                }}
                            >
                                <Tooltip title="Last Refresh" sx={{}}>
                                    <Chip label={relativeLastRefresh} sx={{ ...lastRefreshColor }} size="small" icon={<RefreshRoundedIcon color={lastRefreshColorText} />} />
                                </Tooltip>
                            </Box>
                        </Box>
                        <Typography variant="body2" color="text.secondary">
                            {acc.email}
                        </Typography>
                    </Box>
                </Box>
                <Box
                    sx={{
                        display: 'flex',
                        flexDirection: 'column',
                        width: 'fit-content',
                        gap: 1,
                        ml: 'auto',
                    }}
                >
                    <ServerBadge serverName={acc?.serverName} editable={true} onChange={handleChangeServer} />
                    <RequestStateChip state={acc?.state} useShortName={false} />
                </Box>
            </Box>
        </Box>
    );
}

export default AccountHeader;