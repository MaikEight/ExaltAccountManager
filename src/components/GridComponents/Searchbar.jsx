import { styled, alpha } from '@mui/material/styles';
import InputBase from '@mui/material/InputBase';
import SearchIcon from '@mui/icons-material/Search';
import { useState } from 'react';
import CloseIcon from '@mui/icons-material/Close';
import { IconButton } from '@mui/material';
import { useTheme } from '@emotion/react';

const Search = styled('div')(({ theme }) => ({
    position: 'relative',
    borderRadius: theme.shape.borderRadius,
    ...(theme.palette.mode === 'dark' ? {
        backgroundColor: alpha(theme.palette.background.default, 0.5),
        '&:hover': {
            backgroundColor: alpha(theme.palette.common.white, 0.08),
        },
    } : {
        backgroundColor: alpha(theme.palette.background.default, 0.75),
        '&:hover': {
            backgroundColor: alpha(theme.palette.text.primary, 0.05),
        },
    }),
    transition: theme.transitions.create('background-color'),
    marginLeft: 0,
    width: '100%',
    [theme.breakpoints.up('sm')]: {
        marginLeft: theme.spacing(1),
        width: 'auto',
    },
}));

const SearchIconWrapper = styled('div')(({ theme }) => ({
    padding: theme.spacing(0, 1.5),
    height: '100%',
    position: 'absolute',
    pointerEvents: 'none',
    display: 'flex',
    alignItems: 'center',
    justifyContent: 'center',
}));

const StyledInputBase = styled(InputBase)(({ theme, value }) => ({
    position: 'relative',
    color: 'inherit',
    width: '100%',
    '& .MuiInputBase-input': {
        padding: theme.spacing(1, 1, 1, 0),
        paddingLeft: `calc(1em + ${theme.spacing(4)})`,
        transition: theme.transitions.create('width'),
        [theme.breakpoints.up('md')]: {
            width: value ? '30ch' : '18ch',
            '&:focus': {
                width: '30ch',
            },
        },
    },
}));

function Searchbar({ onSearchChanged }) {
    const [search, setSearch] = useState('');
    const theme = useTheme();

    return (
        <Search>
            <SearchIconWrapper>
                <SearchIcon />
            </SearchIconWrapper>
            <StyledInputBase
                placeholder="Search…"
                inputProps={{ 'aria-label': 'search' }}
                value={search}
                onChange={(event) => {
                    setSearch(event.target.value);
                    onSearchChanged(event.target.value);
                }}
            />
            {search?.length > 0 && (
                <IconButton
                    sx={{
                        position: 'absolute',
                        right: 4, 
                        top: '50%',
                        transform: 'translateY(-50%)',
                        color: alpha(theme.palette.text.primary, 0.4)
                    }}
                    size='small'
                    aria-label="clear search"                    
                    onClick={() => {
                        setSearch('');
                        onSearchChanged('');
                    }}
                >
                    <CloseIcon sx={{ fontSize: '20px' }} />
                </IconButton>
            )}
        </Search>
    );
}

export default Searchbar;