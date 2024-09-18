import styled from "styled-components";

const GradientBorderButtonInner = styled('div')(({ theme, selected }) => ({
    display: 'flex',
    justifyContent: 'center',
    alignItems: 'center',
    padding: '6px',
    gap: '8px',
    borderRadius: '30px',
    background: selected ? 'linear-gradient(98deg, rgb(198, 167, 254), rgba(145, 85, 253) 94%)' : theme.palette.background.default,
    transition: theme.transitions.create('background-color'),
    '&:hover': {
        background: 'linear-gradient(98deg, rgb(198, 167, 254), rgba(145, 85, 253) 94%)',
    }
}));

export default GradientBorderButtonInner;