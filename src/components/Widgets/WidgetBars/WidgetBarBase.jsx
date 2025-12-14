
import { Box } from '@mui/material';
import useWidgets from '../../../hooks/useWidgets';

function WidgetBarBase() {
    const { widgetBarState } = useWidgets();

    return (
        <Box
            sx={{
                display: 'flex',
                flexDirection: 'column',
                height: '100%',
                width: '100%',
                overflowY: 'auto',
                backgroundColor: 'background.paper',
                borderRadius: (theme) => `${theme.shape.borderRadius * 2}px 0 0 ${theme.shape.borderRadius * 2}px`,
                borderLeft: (theme) => `1px solid ${theme.palette.divider}`,
            }}
        >
            {/* Header */}
            {
                widgetBarState?.type?.headerComponents &&
                widgetBarState.type.headerComponents.map((Component, index) => (
                    <Component key={"H_" + index} />
                ))
            }

            {/* Content */}
            <Box
                sx={{
                    display: 'flex',
                    flexDirection: 'column',
                    width: '100%',
                    flexGrow: 1,
                    overflowY: 'auto',
                    px: 1,
                }}
            >
            {
                widgetBarState?.type?.components &&                
                widgetBarState.type.components.map((Component, index) => (
                    <Component key={"C_" + index} />
                ))
            }
            </Box>

            {/* Footer */}
            <Box
                sx={{
                    display: 'flex',
                    flexDirection: 'column',
                    width: '100%',
                    marginTop: 'auto',
                }}
            >
                {
                    widgetBarState?.type?.footerComponents &&
                    widgetBarState.type.footerComponents.map((Component, index) => (
                        <Component key={"F_" + index} />
                    ))
                }
            </Box>
        </Box>
    );
}

export default WidgetBarBase;