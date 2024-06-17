import { Box, TextField, Typography } from "@mui/material";
import ComponentBox from "../ComponentBox";
import StyledButton from "../StyledButton";
import ArrowBackIosNewOutlinedIcon from '@mui/icons-material/ArrowBackIosNewOutlined';
import PersonAddAltOutlinedIcon from '@mui/icons-material/PersonAddAltOutlined';
import { useEffect, useState } from "react";
import { postRegisterAccount } from "../../backend/decaApi";
import { DatePicker, LocalizationProvider } from "@mui/x-date-pickers";
import { AdapterDateFns } from '@mui/x-date-pickers/AdapterDateFnsV3';
import { de } from 'date-fns/locale/de';
import { useTheme } from "@emotion/react";
import useSnack from "../../hooks/useSnack";
import useAccounts from "../../hooks/useAccounts";
import { useNavigate } from "react-router-dom";

function RegisterAccount({ open, onClose }) {
    const [isLoading, setIsLoading] = useState(false);
    const [newAccount, setNewAccount] = useState({ name: '', email: '', password: '' });
    const [passwordConfirm, setPasswordConfirm] = useState('');
    const [dateOfBirth, setDateOfBirth] = useState(null);
    const { showSnackbar } = useSnack();
    const { updateAccount } = useAccounts();
    const theme = useTheme();
    const navigate = useNavigate();

    const registerAccount = async () => {
        setIsLoading(true);
        const response = await postRegisterAccount(newAccount);

        if (response === 'EAM API error' || response === null) {
            showSnackbar('Error registering new account, please try again alter.', 'error');
            setIsLoading(false);
            return;
        } 

        if(response === '<Success/>') {
            showSnackbar('Account registered successfully', 'success');

            //Add account to the database
            const account = {
                isDeleted: false,
                name: newAccount.name,
                email: newAccount.email,
                password: newAccount.password,
                performDailyLogin: false,
                group: null,
                state: 'Registered',
                isSteam: false,
                steamId: null,
                extra: null,
                token: null,
            };
            const addedAccount = await updateAccount(account, true);

            setIsLoading(false);
            if (!addedAccount) {
                showSnackbar('Error adding account to database, please add it manually.', 'error');
                onClose(false);
                return;
            }

            onClose(true);
            navigate(`/accounts?selectedAccount=${account.email}`);
            return;
        }

        setIsLoading(false);

        if(response === '<Error>Error.invalidEmail</Error>') {
            showSnackbar('Error registering new account: Invalid email', 'error');
            return;        
        }

        if(response === '<Error>Error.emailAlreadyUsed</Error>') {
            showSnackbar('Error registering new account: Email already in use', 'error');
            return;
        }

        if(response === '<Error>Error.nameAlreadyInUse</Error>') {
            showSnackbar('Error registering new account: Name already in use', 'error');
            return;        
        }
        
        if(response === '<Error>LinkWebAccountDialog.shortError</Error>') {
            showSnackbar('Error registering new account: Password to short', 'error');
            return;        
        }

        if(response === '<Error>LinkWebAccountDialog.repeatError</Error>') {
            showSnackbar('Error registering new account: Password contains too many repeating characters', 'error');
            return;        
        }

        if(response === '<Error>LinkWebAccountDialog.alphaNumError</Error>') {
            showSnackbar('Error registering new account: Password must contain at least one letter and one number', 'error');            
            return;        
        }

        showSnackbar('Error registering new account, please try again alter.', 'error');
    };

    useEffect(() => {
        setNewAccount({ name: '', email: '', password: '' });
        setPasswordConfirm('');
        setDateOfBirth(null);
    }, [open]);

    const isAccountNameValid = () => newAccount.name.length === 0 || (newAccount.name.length <= 15 && newAccount.name.match(/^[a-zA-Z]+$/));

    if (!open)
        return null;

    return (
        <Box
            sx={{
                mt: -2,
            }}
        >
            <ComponentBox
                title="Enter new account details"
                icon={<PersonAddAltOutlinedIcon />}
            >
                <Box
                    sx={{
                        display: 'flex',
                        flexDirection: 'column',
                        gap: 1,
                    }}
                >
                    <TextField
                        id="accName"
                        label="Account name"
                        variant="standard"
                        fullWidth
                        error={!isAccountNameValid()}
                        value={newAccount.name}
                        onChange={(event) => setNewAccount({ ...newAccount, name: event.target.value })}
                        onKeyDown={(event) => {
                            if (event.key === 'Enter') {
                                event.preventDefault();
                                document.getElementById('email').focus();
                            }
                        }}
                    />
                    <TextField
                        id="email"
                        label="E-Mail"
                        variant="standard"
                        fullWidth
                        value={newAccount.email}
                        onChange={(event) => setNewAccount({ ...newAccount, email: event.target.value })}
                        onKeyDown={(event) => {
                            if (event.key === 'Enter') {
                                event.preventDefault();
                                document.getElementById('password').focus();
                            }
                        }}
                    />
                    <TextField
                        id="password"
                        label="Password"
                        variant="standard"
                        type="password"
                        fullWidth
                        value={newAccount.password}
                        onChange={(event) => setNewAccount({ ...newAccount, password: event.target.value })}
                        onKeyDown={(event) => {
                            if (event.key === 'Enter') {
                                event.preventDefault();
                                document.getElementById('passwordConfirm').focus();
                            }
                        }}
                    />
                    <TextField
                        id="passwordConfirm"
                        label="Confirm Password"
                        variant="standard"
                        type="password"
                        fullWidth
                        error={passwordConfirm.length > 0 && newAccount.password !== passwordConfirm}
                        helperText={passwordConfirm.length > 0 && newAccount.password !== passwordConfirm ? 'Passwords do not match' : ''}
                        value={passwordConfirm}
                        onChange={(event) => setPasswordConfirm(event.target.value)}
                        onKeyDown={(event) => {
                            if (event.key === 'Enter') {
                                event.preventDefault();
                                document.getElementById('dateOfBirth').focus();
                            }
                        }}
                    />
                    <LocalizationProvider dateAdapter={AdapterDateFns} adapterLocale={de}>
                        <DatePicker
                            sx={{
                                mt: 2,
                            }}
                            id="dateOfBirth"
                            label="Date of Birth"
                            value={dateOfBirth}
                            onChange={(newValue) => setDateOfBirth(newValue)}
                            slotProps={{
                                layout: {
                                    sx: {
                                        borderRadius: `${theme.shape.borderRadius}px`,
                                        backgroundColor: theme.palette.background.paperLight,
                                        '& .MuiDayCalendar-root': {
                                            color: theme.palette.primary.main,
                                            borderRadius: `${theme.shape.borderRadius}px`,
                                            backgroundColor: theme.palette.background.paper,
                                        },
                                    }
                                }
                            }}
                        />
                    </LocalizationProvider>

                    <Typography variant="body2" sx={{ mt: 2 }}>
                        By clicking 'Register', you are indicating that you have read and agree to the <a href="https://decagames.com/tos.html" target="_blank" rel="noreferrer">Terms of Service</a> and <a href="https://decagames.com/privacy.html" target="_blank" rel="noreferrer">Privacy Policy</a>.
                    </Typography>
                </Box>
                <Box
                    sx={{
                        display: 'flex',
                        justifyContent: 'space-between',
                        mt: 2,
                    }}
                >
                    <StyledButton
                        color="secondary"
                        onClick={() => onClose(true)}
                        startIcon={<ArrowBackIosNewOutlinedIcon />}
                    >
                        Back
                    </StyledButton>
                    <StyledButton
                        disabled={isLoading || !isAccountNameValid() || newAccount.name.length === 0 || newAccount.email.length === 0 || newAccount.password.length === 0 || passwordConfirm.length === 0 || newAccount.password !== passwordConfirm || dateOfBirth === null}
                        onClick={registerAccount}
                        startIcon={<PersonAddAltOutlinedIcon />}
                    >
                        Register
                    </StyledButton>
                </Box>
            </ComponentBox>
        </Box>
    );
}

export default RegisterAccount;