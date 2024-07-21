import { Box } from "@mui/material";

function InverseBorderRadiusSpacer({ sx, bottomBorder = true, upperBorder = false }) {

    return (
        <Box
            id="filter-box-spacer"
            sx={{
                position: 'relative',
                height: theme => theme.spacing(2),
                backgroundColor: theme => theme.palette.background.default,
                borderRadius: '0 0 0 0',
                
                ...sx,
            }}
        >
            {/* Overlay left */}
            <Box
                id="filter-box-overlay-left"
                sx={{
                    position: 'absolute',
                    backgroundColor: theme => theme.palette.background.default,
                    top: 0,
                    left: theme => -theme.shape.borderRadius,
                    height: theme => theme.spacing(2.75),
                    width: theme => `${theme.shape.borderRadius}px`,
                    zIndex: 1,
                }}
            />
            {/* Overlay right */}
            <Box
                id="filter-box-overlay-right"
                sx={{
                    position: 'absolute',
                    backgroundColor: theme => theme.palette.background.default,
                    top: 0,
                    right: theme => -theme.shape.borderRadius,
                    height: '100%',
                    width: theme => `${theme.shape.borderRadius}px`,
                    zIndex: 1,
                }}
            />
            {/* Bottom-left-corner */}
            {
                bottomBorder &&
                <Box
                    sx={{
                        position: 'absolute',
                        backgroundColor: 'transparent',
                        bottom: theme => `-${theme.shape.borderRadius * 2}px`,
                        left: '0',
                        height: theme => `${theme.shape.borderRadius * 2}px`,
                        width: theme => `${theme.shape.borderRadius}px`,
                        borderTopLeftRadius: theme => `${theme.shape.borderRadius}px`,
                        boxShadow: theme => `0 -${theme.shape.borderRadius}px 0 0 ${theme.palette.background.default}`,
                    }}
                />
            }
            {/* Bottom-right-corner */}
            {
                bottomBorder &&
                <Box
                    sx={{
                        position: 'absolute',
                        backgroundColor: 'transparent',
                        bottom: theme => `-${theme.shape.borderRadius * 2}px`,
                        right: '0',
                        height: theme => `${theme.shape.borderRadius * 2}px`,
                        width: theme => `${theme.shape.borderRadius}px`,
                        borderTopRightRadius: theme => `${theme.shape.borderRadius}px`,
                        boxShadow: theme => `0 -${theme.shape.borderRadius}px 0 0 ${theme.palette.background.default}`,
                    }}
                />
            }
            {/* Upper-left-corner */}
            {
                upperBorder &&
                <Box
                    sx={{
                        position: 'absolute',
                        backgroundColor: 'transparent',
                        top: theme => `-${theme.shape.borderRadius * 2}px`,
                        left: '0',
                        height: theme => `${theme.shape.borderRadius * 2}px`,
                        width: theme => `${theme.shape.borderRadius}px`,
                        borderBottomLeftRadius: theme => `${theme.shape.borderRadius}px`,
                        boxShadow: theme => `0 ${theme.shape.borderRadius}px 0 0 ${theme.palette.background.default}`,
                    }}
                />
            }
            {/* Upper-right-corner */}
            {
                upperBorder &&
                <Box
                    sx={{
                        position: 'absolute',
                        backgroundColor: 'transparent',
                        top: theme => `-${theme.shape.borderRadius * 2}px`,
                        right: '0',
                        height: theme => `${theme.shape.borderRadius * 2}px`,
                        width: theme => `${theme.shape.borderRadius}px`,
                        borderBottomRightRadius: theme => `${theme.shape.borderRadius}px`,
                        boxShadow: theme => `0 ${theme.shape.borderRadius}px 0 0 ${theme.palette.background.default}`,
                    }}
                />
            }
        </Box>
    );
}

export default InverseBorderRadiusSpacer;