import { Table, Box, Checkbox, Drawer, FormControlLabel, IconButton, Step, StepLabel, TableBody, TableContainer, TableHead, TableRow, TextField, Tooltip, Typography } from "@mui/material";
import Stepper from '@mui/material/Stepper';
import { useEffect, useState } from "react";
import CloseIcon from '@mui/icons-material/Close';
import { useTheme } from "@emotion/react";
import AccountCircleOutlinedIcon from '@mui/icons-material/AccountCircleOutlined';
import PersonAddAltOutlinedIcon from '@mui/icons-material/PersonAddAltOutlined';
import HowToRegOutlinedIcon from '@mui/icons-material/HowToRegOutlined';
import useColorList from "../hooks/useColorList";
import DoneOutlinedIcon from '@mui/icons-material/DoneOutlined';
import ComponentBox from "./ComponentBox";
import StyledButton from "./StyledButton";
import useHWID from "../hooks/useHWID";
import { postAccountVerify, postCharList } from "../backend/decaApi";
import GroupSelector from "./AccountDetails/GroupSelector";
import GroupRow from "./AccountDetails/GroupRow";
import TextTableRow from "./AccountDetails/TextTableRow";
import DailyLoginCheckBoxTableRow from "./AccountDetails/DailyLoginCheckBoxTableRow";
import PaddedTableCell from "./AccountDetails/PaddedTableCell";
import useAccounts from "../hooks/useAccounts";
import useGroups from "../hooks/useGroups";
import useServerList from './../hooks/useServerList';
import { invoke } from '@tauri-apps/api/tauri';
import { getRequestState, storeCharList } from "../utils/charListUtil";

const steps = ['Login', 'Add details', 'Finish'];
const icons = [
    <AccountCircleOutlinedIcon />,
    <PersonAddAltOutlinedIcon />,
    <HowToRegOutlinedIcon />,
];

function AddNewAccount({ isOpen, onClose }) {
    const { accounts, updateAccount } = useAccounts();

    const [activeStep, setActiveStep] = useState(0);
    const [skipped, setSkipped] = useState(new Set([]));
    const [isLoading, setIsLoading] = useState(false);

    const [newAccount, setNewAccount] = useState({ email: '', password: '', isSteam: false, steamId: null });

    const { saveServerList } = useServerList();

    //STEP 1
    const [passwordEmailWrong, setPasswordEmailWrong] = useState(false);
    //STEP 2
    const { groups } = useGroups();

    const theme = useTheme();
    const hwid = useHWID();
    const color = useColorList(0);

    useEffect(() => {
        setNewAccount({ email: '', password: '', isSteam: false, steamId: null, performDailyLogin: false });
        setActiveStep(0);
    }, [isOpen]);

    useEffect(() => {
        const timeout = setTimeout(() => {
            setPasswordEmailWrong(false);
        }, 2500);
        return () => clearTimeout(timeout);
    }, [passwordEmailWrong]);

    useEffect(() => {
        if (!newAccount) {
            setNewAccount({ email: '', password: '', isSteam: false, steamId: null, performDailyLogin: false });
            return;
        }

        if (newAccount.email.includes('steamworks:') && !newAccount.isSteam) {
            setNewAccount({ ...newAccount, isSteam: true, steamId: newAccount.email.split(':')[1] });
            return;
        } 

        if(!newAccount.email.includes('steamworks:') && newAccount.isSteam) {
            setNewAccount({ ...newAccount, isSteam: false, steamId: null });
            return;
        }

    }, [newAccount]);

        const isStepOptional = (step) => {
            return step === -1;
        };

        const isStepSkipped = (step) => {
            return skipped.has(step);
        };

        const isLoginButtonDisabled = () => newAccount.email.length < 3 || !(newAccount.email.includes('@') || newAccount.email.includes('steamworks:')) || newAccount.password.length < 3 || accountAlreadyExists() || isLoading;

        const accountAlreadyExists = () => accounts.find((account) => account.email === newAccount.email) !== undefined;

        const getFooterButtons = (backButtonText, nextButtontext, onClickBack, onClickNext,) => {
            return (<Box
                sx={{
                    display: 'flex',
                    flexDirection: 'row',
                    justifyContent: 'space-between',
                    gap: 1,
                }}
            >
                <StyledButton
                    color="secondary"
                    onClick={onClickBack}
                >
                    {backButtonText}
                </StyledButton>
                <StyledButton
                    onClick={onClickNext}
                >
                    {nextButtontext}
                </StyledButton>
            </Box>);
        };

        const handleLoginButtonClick = () => {
            setIsLoading(true);
            const email = newAccount.email;            
            let accName = '';
            postAccountVerify(newAccount, hwid, false)
                .then((response) => {
                    if (!response || response.Error) {
                        setPasswordEmailWrong(true);
                        setNewAccount({ ...newAccount, password: '' });
                        return;
                    }

                    accName = response?.Account?.Name;
                    setNewAccount({
                        ...newAccount,                        
                        ...(accName && accName.length > 0 ? { name: accName } : {})
                    });

                    setActiveStep(1);

                    if (response.Account && response.Account.Name) {
                        postCharList(response.Account.AccessToken)
                            .then((charList) => {
                                setNewAccount({ 
                                    ...newAccount, 
                                    state: getRequestState(charList),
                                    ...(accName && accName.length > 0 ? { name: accName } : {})
                                });
                                storeCharList(charList, email);
                                const servers = charList.Chars.Servers.Server;
                                if (servers && servers.length > 0) {
                                    saveServerList(servers);
                                }
                            }).catch((err) => {
                                console.error("error", err);
                            });
                    }
                })
                .catch((error) => {
                    console.error("Error: ", error);
                    setPasswordEmailWrong(true);
                    setNewAccount({ ...newAccount, password: '' });
                })
                .finally(() => {
                    setIsLoading(false);
                });
        };

        const getStepContent = () => {
            switch (activeStep) {
                case 0: //Login
                    return (
                        <ComponentBox
                            title={steps[0]}
                            isLoading={isLoading}
                            icon={icons[0]}
                        >
                            <Typography variant="body2" sx={{ mb: 1 }}>
                                Enter the login credentials for the account you want to add.
                            </Typography>
                            <Box
                                sx={{
                                    display: 'flex',
                                    flexDirection: 'column',
                                    gap: 1,
                                    mb: 1.5,
                                }}
                            >
                                <TextField
                                    id="email"
                                    label="E-Mail"
                                    variant="standard"
                                    error={passwordEmailWrong || accountAlreadyExists()}
                                    helperText={accountAlreadyExists() ? "Account is already in your list" : null}
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
                                    type="password"
                                    error={passwordEmailWrong}
                                    variant="standard"
                                    value={newAccount.password}
                                    onChange={(event) => setNewAccount({ ...newAccount, password: event.target.value })}
                                    onKeyDown={(event) => {
                                        if (event.key === 'Enter') {
                                            event.preventDefault();
                                            handleLoginButtonClick();
                                        }
                                    }}
                                />
                            </Box>

                            <Box sx={{ display: 'flex', justifyContent: 'end' }}>
                                <Tooltip
                                    title={isLoginButtonDisabled() ? 'Please enter a valid login credentials' : ''}
                                >
                                    <span>
                                        <StyledButton
                                            disabled={isLoginButtonDisabled()}
                                            onClick={handleLoginButtonClick}
                                        >
                                            login
                                        </StyledButton>
                                    </span>
                                </Tooltip>
                            </Box>
                        </ComponentBox>);
                case 1:  //Add details
                    return (
                        <ComponentBox
                            title={steps[1]}
                            isLoading={isLoading}
                            icon={icons[1]}
                        >
                            <Box
                                id="add-details"
                                sx={{
                                    display: 'flex',
                                    flexDirection: 'column',
                                    gap: 1,
                                    mb: 1.5,
                                    pl: 1,
                                    pr: 1,
                                }}
                            >
                                <Typography variant="body2" sx={{ mb: 1 }}>
                                    Enter the details for the account you want to add.
                                </Typography>
                                <TextField
                                    id="accountname"
                                    label="Accountname"
                                    variant="standard"
                                    value={newAccount.name}
                                    onChange={(event) => setNewAccount({ ...newAccount, name: event.target.value })}
                                />
                                <GroupSelector
                                    sx={{ margin: 0 }}
                                    selected={newAccount.group ? newAccount.group : ''}
                                    onChange={(newValue) => {
                                        setNewAccount({ ...newAccount, group: newValue ? newValue : null });
                                    }}
                                    showGroupEditor={true}
                                />
                                <FormControlLabel
                                    sx={{ margin: 0, ml: -1.5 }}
                                    control={
                                        <Checkbox
                                            checked={newAccount.performDailyLogin ? newAccount.performDailyLogin : false}
                                            onChange={(event) => setNewAccount({ ...newAccount, performDailyLogin: event.target.checked })}
                                            inputProps={{ 'aria-label': 'Daily login' }}
                                        />
                                    }
                                    label="Daily login"
                                />
                            </Box>
                            {getFooterButtons(
                                'BACK',
                                'NEXT',
                                () => {
                                    setNewAccount({ email: '', password: '' });
                                    setActiveStep(0)
                                },
                                () => setActiveStep(2))
                            }
                        </ComponentBox>
                    );
                case 2: //Finishing
                    return (
                        <ComponentBox
                            title={steps[2]}
                            isLoading={isLoading}
                            icon={icons[2]}
                        >
                            <Box
                                id="add-details"
                                sx={{
                                    display: 'flex',
                                    flexDirection: 'column',
                                    gap: 1,
                                    mb: 1.5,
                                    pl: 1,
                                    pr: 1,
                                }}
                            >
                                <Typography variant="body2" sx={{ mb: 1 }}>
                                    You are about to add the following account:
                                </Typography>
                                <TableContainer component={Box} sx={{ borderRadius: 0 }}>
                                    <Table
                                        sx={{
                                            '& tbody tr:last-child td, & tbody tr:last-child th': {
                                                borderBottom: 'none',
                                            },
                                        }}
                                    >
                                        <TableHead>
                                            <TableRow>
                                                <PaddedTableCell>Attribute</PaddedTableCell>
                                                <PaddedTableCell>Value</PaddedTableCell>
                                            </TableRow>
                                        </TableHead>
                                        <TableBody>
                                            <GroupRow
                                                key='group'
                                                group={newAccount.group ? groups.find((g) => g.name === newAccount.group) : null}
                                            />
                                            <TextTableRow key='name' keyValue={"Accountname"} value={newAccount.name} allowCopy={true} />
                                            <TextTableRow key='email' keyValue={"Email"} value={newAccount.email} allowCopy={true} />
                                            <DailyLoginCheckBoxTableRow key='dailyLogin' keyValue={"Daily login"}
                                                value={newAccount.performDailyLogin}
                                                onChange={() => { }}
                                            />
                                        </TableBody>
                                    </Table>
                                </TableContainer>
                            </Box>
                            {
                                getFooterButtons(
                                    'BACK',
                                    'SAVE ACCOUNT',
                                    () => {
                                        setActiveStep(1)
                                    },
                                    () => {
                                        updateAccount(newAccount, true);
                                        onClose();
                                    })
                            }
                        </ComponentBox>
                    );
                default:
                    return <div>Unknown step</div>;
            }
        };

        return (
            <Drawer
                sx={{
                    width: 500,
                    flexShrink: 0,
                    '& .MuiDrawer-paper': {
                        width: 500,
                        boxSizing: 'border-box',
                        backgroundColor: theme.palette.background.default,
                        border: 'none',
                        borderRadius: '6px 0px 0px 6px',
                        boxShadow: ' 0px 0px 20px 10px rgba(0,0,0,0.2)',
                    },
                }}
                variant="persistent"
                anchor="right"
                open={isOpen}
            >
                <Box
                    sx={{
                        display: 'flex',
                        flexDirection: 'row',
                        justifyContent: 'center',
                        alignContent: 'center',
                        minHeight: 44,
                        maxHeight: 44,
                        pt: 0.5,
                        backgroundColor: theme.palette.background.paperLight,
                        position: 'sticky',
                        top: 0,
                        zIndex: 1,
                    }}
                >
                    <IconButton
                        sx={{ position: 'absolute', left: 5, top: 5, marginLeft: 0, marginRight: 2 }}
                        size="small"
                        onClick={() => onClose()}
                    >
                        <CloseIcon sx={{ fontSize: 21 }} />
                    </IconButton>
                    <Typography variant="h6" component="div" sx={{ textAlign: 'center' }}>
                        Add new account
                    </Typography>
                </Box>

                <Box sx={{ mt: 2, pl: 1, pr: 1, }}>
                    <Stepper activeStep={activeStep} alternativeLabel sx={{ width: '100%' }}>
                        {steps.map((label, index) => {
                            const stepProps = {};
                            const labelProps = {};
                            if (isStepOptional(index)) {
                                labelProps.optional = (
                                    <Typography variant="caption">Optional</Typography>
                                );
                            }
                            if (isStepSkipped(index)) {
                                stepProps.completed = false;
                            } else {
                                stepProps.completed = activeStep > index;
                            }
                            return (
                                <Step key={label} {...stepProps}>
                                    <StepLabel icon={stepProps.completed ? <DoneOutlinedIcon /> : icons[index]} {...labelProps} sx={{ ...(activeStep === index ? { color: color.color } : {}) }}>{label}</StepLabel>
                                </Step>
                            );
                        })}
                    </Stepper>
                </Box>
                <Box sx={{ mt: 2 }}>
                    {
                        getStepContent()
                    }
                </Box>
            </Drawer>
        );
    }

export default AddNewAccount;