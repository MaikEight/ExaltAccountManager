import styled from "styled-components";

const GradientBorderButtonInner = styled('div').withConfig({
    shouldForwardProp: (prop) => prop !== 'sx'
})(({ theme, selected, sx }) => ({
    height: '36px',
    display: 'flex',
    justifyContent: 'center',
    alignItems: 'center',
    padding: '6px',
    borderRadius: '28px',
    background: selected ? 'linear-gradient(98deg, rgb(198, 167, 254), rgba(145, 85, 253) 94%)' : theme.palette.background.default,
    transition: theme.transitions.create('background-color'),
    '&:hover': {
        background: 'linear-gradient(98deg, rgb(198, 167, 254), rgba(145, 85, 253) 94%)',
    },
    ...sx
}));

export default GradientBorderButtonInner;