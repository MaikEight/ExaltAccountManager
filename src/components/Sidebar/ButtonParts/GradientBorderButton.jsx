import styled from "styled-components";

const GradientBorderButton = styled('div').withConfig({
    shouldForwardProp: (prop) => prop !== 'sx'
})(({ theme, selected, sx }) => ({
    color: !selected
        ? theme.palette.text.primary
        : theme.palette.mode === 'light'
            ? theme.palette.background.default
            : theme.palette.text.primary,
    borderRadius: '30px',
    height: 'fit-content',
    cursor: 'pointer',
    padding: '2px',
    background: 'linear-gradient(98deg, rgb(198, 167, 254), rgba(145, 85, 253) 94%)',
    '&:hover': {
        color: theme.palette.mode === 'light' ? theme.palette.background.default : theme.palette.text.primary,
    },
    ...sx
}));

export default GradientBorderButton;