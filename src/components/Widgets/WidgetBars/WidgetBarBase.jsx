
import { Box, Button } from '@mui/material';
import useWidgets from '../../../hooks/useWidgets';
import { alpha } from '@mui/material/styles';

function WidgetBarBase() {
    const { widgetBarState, updateWidgetBarEditMode } = useWidgets();

    return (
        <Box
            sx={{
                position: 'relative',
                display: 'flex',
                flexDirection: 'column',
                height: '100%',
                width: '100%',
                overflowY: 'auto',
                backgroundColor: 'background.default',
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
                    p: 1,
                    gap: 1,
                }}
            >
                {
                    widgetBarState?.editMode &&
                    <Box
                        sx={{
                            backgroundColor: (theme) => alpha(theme.palette.warning.main, 0.1),
                            border: (theme) => `1px solid ${theme.palette.mode === 'dark' ? theme.palette.warning.dark : theme.palette.warning.main}`,
                            borderRadius: (theme) => `${theme.shape.borderRadius}px`,
                            display: 'flex',
                            alignItems: 'center',
                            justifyContent: 'space-between',
                            p: 1,
                        }}
                    >
                        Edit Mode Enabled

                        <Button
                            variant="contained"
                            size="small"
                            color="secondary"
                            onClick={() => updateWidgetBarEditMode(false)}
                        >
                            End
                        </Button>
                    </Box>
                }
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
                    height: 'fit-content',
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

            {/* Floating Components */}
            {
                widgetBarState?.type?.floatingComponents &&
                widgetBarState.type.floatingComponents.map((Component, index) => (
                    <Component key={"FL_" + index} />
                ))
            }
        </Box>
    );
}

export default WidgetBarBase;