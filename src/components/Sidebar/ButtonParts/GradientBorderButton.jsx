import { styled } from '@mui/material/styles';

const GradientBorderButton = styled('div', {
    shouldForwardProp: (prop) => prop !== 'sx' && prop !== 'selected',
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