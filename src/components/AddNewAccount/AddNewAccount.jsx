import { Table, Box, Checkbox, Drawer, FormControlLabel, IconButton, Step, StepLabel, Stepper, TableBody, TableContainer, TableHead, TableRow, TextField, Tooltip, Typography, Collapse } from "@mui/material";
import { useEffect, useRef, useState } from "react";
import CloseIcon from '@mui/icons-material/Close';
import { useTheme } from "@emotion/react";
import AccountCircleOutlinedIcon from '@mui/icons-material/AccountCircleOutlined';
import PersonAddAltOutlinedIcon from '@mui/icons-material/PersonAddAltOutlined';
import HowToRegOutlinedIcon from '@mui/icons-material/HowToRegOutlined';
import DoneOutlinedIcon from '@mui/icons-material/DoneOutlined';
import ComponentBox from "../ComponentBox";
import StyledButton from "../StyledButton";
import useHWID from "../../hooks/useHWID";
import { postAccountVerify, postCharList, getRequestState, storeCharList, useGroups } from 'eam-commons-js';
import GroupSelector from "../AccountDetails/GroupSelector";
import GroupRow from "../AccountDetails/GroupRow";
import TextTableRow from "../AccountDetails/TextTableRow";
import DailyLoginCheckBoxTableRow from "../AccountDetails/DailyLoginCheckBoxTableRow";
import PaddedTableCell from "../AccountDetails/PaddedTableCell";
import useAccounts from "../../hooks/useAccounts";
import useServerList from '../../hooks/useServerList';
import LoginOutlinedIcon from '@mui/icons-material/LoginOutlined';
import ArrowBackIosNewOutlinedIcon from '@mui/icons-material/ArrowBackIosNewOutlined';
import ArrowForwardIosOutlinedIcon from '@mui/icons-material/ArrowForwardIosOutlined';
import { useNavigate } from "react-router-dom";
import GroupAddOutlinedIcon from '@mui/icons-material/GroupAddOutlined';
import RegisterAccount from "./RegisterAccount";
import AddCircleOutlineOutlinedIcon from '@mui/icons-material/AddCircleOutlineOutlined';
import AddAccountSvg from "../Illustrations/AddAccountSvg";
import { useColorList } from "../../hooks/useColorList";

const steps = ['Login', 'Add details', 'Finish'];
const icons = [
    <AccountCircleOutlinedIcon />,
    <PersonAddAltOutlinedIcon />,
    <HowToRegOutlinedIcon />,
];

function AddNewAccount({ isOpen, onClose }) {
    const { accounts, updateAccount } = useAccounts();

    const [activeStep, setActiveStep] = useState(0);
    const [isLoading, setIsLoading] = useState(false);

    const [newAccount, setNewAccount] = useState({ email: '', password: '', isSteam: false, steamId: null });
    const [showRegisterForm, setShowRegisterForm] = useState(false);
    const { saveServerList } = useServerList();

    //STEP 1
    const [passwordEmailWrong, setPasswordEmailWrong] = useState(false);
    //STEP 2
    const { groups } = useGroups();

    const theme = useTheme();
    const hwid = useHWID();
    const color = useColorList(0);
    const containerRef = useRef(null);
    const navigate = useNavigate();

    useEffect(() => {
        setShowRegisterForm(false);
        setNewAccount({ email: '', password: '', isSteam: false, steamId: null, performDailyLogin: false, isDeleted: false });
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
            setNewAccount({ email: '', password: '', isSteam: false, steamId: null, performDailyLogin: false, isDeleted: false });
            return;
        }

        if (newAccount.email.includes('steamworks:')) {
            const steamId = newAccount.email.split(':')[1];
            if (newAccount.steamId !== steamId || !newAccount.isSteam) {
                setNewAccount({ ...newAccount, isSteam: true, steamId: steamId });
            }
            return;
        }

        if (!newAccount.isSteam && newAccount.steamId) {
            const acc = newAccount;
            delete acc.steamId;
            setNewAccount(acc);
        }
    }, [newAccount]);

    const isStepOptional = (step) => {
        return step === -1;
    };

    const isLoginButtonDisabled = () => newAccount.email.length < 3 || !(newAccount.email.includes('@') || (newAccount.isSteam && newAccount.steamId?.length >= 3)) || newAccount.password.length < 3 || accountAlreadyExists() || isLoading;

    const accountAlreadyExists = () => accounts.find((account) => account.email === newAccount.email) !== undefined;

    const getFooterButtons = (backButton, nextButton) => {
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
                onClick={backButton.onClick}
                {...(backButton.icon && { startIcon: backButton.icon })}
            >
                {backButton.text}
            </StyledButton>
            <StyledButton
                onClick={nextButton.onClick}
                {...(nextButton.endIcon && { endIcon: nextButton.endIcon })}
                {...(nextButton.startIcon && { startIcon: nextButton.startIcon })}
            >
                {nextButton.text}
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
                    <Box>
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

                                <Tooltip title="Is the account you want to add a steam account?" placement="bottom-start">
                                    <FormControlLabel
                                        label="Steam account"
                                        control={
                                            <Checkbox
                                                checked={newAccount.isSteam}
                                                onChange={(event) => setNewAccount({ ...newAccount, isSteam: event.target.checked })}
                                                checkedIcon={<img src={theme.palette.mode === 'dark' ? "/steam.svg" : "/steam_light_mode.svg"} alt="Steam Logo" height='24px' width='24px' style={{}} />}
                                                inputProps={{ 'aria-label': 'Steam account' }}
                                            />
                                        }
                                    />
                                </Tooltip>
                                <TextField
                                    id="email"
                                    label={newAccount.isSteam ? "Guid" : "E-Mail"}
                                    variant="standard"
                                    error={passwordEmailWrong || accountAlreadyExists()}
                                    helperText={accountAlreadyExists() ? "Account is already in your list" : null}
                                    value={newAccount.email}
                                    onChange={(event) => setNewAccount({ ...newAccount, email: event.target.value })}
                                    onKeyDown={(event) => {
                                        if (event.key === 'Enter') {
                                            event.preventDefault();
                                            if (newAccount.isSteam) {
                                                d
                                            }
                                            document.getElementById('password').focus();
                                        }
                                    }}
                                />
                                <Collapse in={newAccount.isSteam} sx={{ width: '100%' }}>
                                    <TextField
                                        id="steamId"
                                        label="Steam ID"
                                        variant="standard"
                                        disabled={newAccount.email?.startsWith('steamworks:')}
                                        fullWidth
                                        value={newAccount.steamId || ''}
                                        onChange={(event) => setNewAccount({ ...newAccount, steamId: event.target.value })}
                                        onKeyDown={(event) => {
                                            if (event.key === 'Enter') {
                                                event.preventDefault();
                                                handleLoginButtonClick();
                                            }
                                        }}
                                    />
                                </Collapse>
                                <TextField
                                    id="password"
                                    label={newAccount.isSteam ? "Secret" : "Password"}
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

                            <Box sx={{ display: 'flex', justifyContent: 'space-between' }}>
                                <StyledButton
                                    color="secondary"
                                    onClick={() => setShowRegisterForm(true)}
                                    startIcon={<AddCircleOutlineOutlinedIcon />}
                                >
                                    Register
                                </StyledButton>
                                <Tooltip
                                    title={isLoginButtonDisabled() ? 'Please enter a valid login credentials' : ''}
                                >
                                    <span>
                                        <StyledButton
                                            disabled={isLoginButtonDisabled()}
                                            onClick={handleLoginButtonClick}
                                            startIcon={<LoginOutlinedIcon />}
                                        >
                                            login
                                        </StyledButton>
                                    </span>
                                </Tooltip>
                            </Box>
                        </ComponentBox>

                        {/* Info on how to add steam accounts */}
                        <ComponentBox
                            title="How to add Steam accounts"
                            icon={<img src={theme.palette.mode === 'dark' ? "/steam.svg" : "/steam_light_mode.svg"} alt="Steam Logo" height='20px' width='20px' />}
                            sx={{ mt: 2 }}
                            innerSx={{
                                display: 'flex',
                                flexDirection: 'column',
                                gap: 1,
                            }}
                            isCollapseable={true}
                            defaultCollapsed={true}
                        >
                            <Typography variant="body2">
                                To add a steam account, you need to first get the two required values: <b>Steamworks ID</b> and the <b>secret</b>.
                            </Typography>
                            <Typography variant="body2">
                                How to get this information is well described in the <a href="https://github.com/jakcodex/muledump/wiki/Steam-Users-Setup-Guide" target="_blank" rel="noreferrer">Steam Users Setup Guide from muledump</a>.
                            </Typography>
                            <Typography variant="body2">
                                After you have the required information, you can add the account by entering the <b>steamworks ID</b> in the email field and the <b>secret</b> in the password field.
                            </Typography>
                            <Typography variant="body2">
                                <b>Big thanks</b> to <a href="https://github.com/jakcodex" target="_blank" rel="noreferrer">jakcodex</a> for creating the great muledump-wiki.
                            </Typography>
                        </ComponentBox>
                        <ComponentBox
                            title="Import accounts"
                            icon={<GroupAddOutlinedIcon />}
                            isCollapseable={true}
                            defaultCollapsed={true}
                            innerSx={{
                                display: 'flex',
                                flexDirection: 'column',
                                gap: 1,
                            }}
                        >
                            <Typography variant="body2">
                                You can also import accounts from a file. All accepted formats are descriped in the importer.
                            </Typography>
                            <StyledButton
                                onClick={() => {
                                    navigate('/importer');
                                }}
                                startIcon={<GroupAddOutlinedIcon />}
                            >
                                Show importer
                            </StyledButton>
                        </ComponentBox>
                        <Box
                            sx={{
                                position: 'absolute',
                                bottom: 16,
                                left: 0,
                                right: 0,
                                display: 'flex',
                                justifyContent: 'center',
                                alignItems: 'center',
                                zIndex: -1,
                            }}
                        >
                            <AddAccountSvg h="152px" />
                        </Box>
                    </Box>
                );
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
                        {
                            getFooterButtons(
                                {
                                    text: 'BACK',
                                    onClick: () => {
                                        setNewAccount({ email: '', password: '' });
                                        setActiveStep(0)
                                    },
                                    icon: <ArrowBackIosNewOutlinedIcon />
                                },
                                {
                                    text: 'NEXT',
                                    onClick: () => setActiveStep(2),
                                    endIcon: <ArrowForwardIosOutlinedIcon />
                                }
                            )
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
                                        />
                                    </TableBody>
                                </Table>
                            </TableContainer>
                        </Box>
                        {
                            getFooterButtons(
                                {
                                    text: 'BACK',
                                    onClick: () => setActiveStep(1),
                                    icon: <ArrowBackIosNewOutlinedIcon />
                                },
                                {
                                    text: 'SAVE ACCOUNT',
                                    onClick: () => {
                                        updateAccount({ ...newAccount, isDeleted: false }, true);
                                        onClose();
                                    },
                                    startIcon: <DoneOutlinedIcon />
                                }
                            )
                        }
                    </ComponentBox>
                );
            default:
                return <div>Unknown step</div>;
        }
    };

    return (
        <Box ref={containerRef} sx={{ overflow: 'hidden' }}>
            <Drawer
                sx={{
                    width: 500,
                    flexShrink: 0,
                    '& .MuiDrawer-paper': {
                        width: 500,
                        backgroundColor: theme.palette.background.default,
                        borderRadius: `${theme.shape.borderRadius}px 0 0 ${theme.shape.borderRadius}px`,
                        overflow: 'hidden',
                    },
                }}
                slotProps={{
                    paper: {
                        elevation: 0, 
                        square: false, 
                        sx: { borderRadius: `${theme.shape.borderRadius}px 10px 10px ${theme.shape.borderRadius}px`, 
                        overflow: 'hidden' },
                    },
                    transition: {
                        container: containerRef.current
                    }
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
                        {showRegisterForm ? 'Register new Account' : 'Add new account'}
                    </Typography>
                </Box>
                {
                    !showRegisterForm &&
                    <Box
                        sx={{
                            backgroundColor: theme.palette.background.paperLight,
                        }}
                    >
                        <Box
                            sx={{
                                pt: 2,
                                pl: 1,
                                pr: 1,
                                width: '100%',
                                backgroundColor: theme.palette.background.default,
                                borderRadius: `${theme.shape.borderRadius}px ${theme.shape.borderRadius}px 0 0`,
                            }}
                        >
                            <Stepper
                                id="stepper"
                                activeStep={activeStep}
                                alternativeLabel
                            >
                                {
                                    steps.map((label, index) => {
                                        const stepProps = {};
                                        const labelProps = {};
                                        if (isStepOptional(index)) {
                                            labelProps.optional = (
                                                <Typography
                                                    variant="caption"
                                                >
                                                    Optional
                                                </Typography>
                                            );
                                        }

                                        stepProps.completed = activeStep > index;

                                        return (
                                            <Step
                                                key={label}
                                                {...stepProps}
                                            >
                                                <StepLabel
                                                    icon={stepProps.completed ? <DoneOutlinedIcon /> : icons[index]}
                                                    {...labelProps}
                                                    sx={{
                                                        ...(activeStep === index ? { color: color.color } : {})
                                                    }}
                                                >
                                                    {label}
                                                </StepLabel>
                                            </Step>
                                        );
                                    })
                                }
                            </Stepper>
                        </Box>
                    </Box>
                }
                <Box
                    sx={{
                        overflowX: 'auto',
                        backgroundColor: showRegisterForm ? theme.palette.background.paperLight : theme.palette.background.default,
                    }}
                >
                    <Box
                        sx={{
                            borderRadius: `${theme.shape.borderRadius}px ${theme.shape.borderRadius}px 0 0`,
                            backgroundColor: theme.palette.background.default,
                        }}
                    >
                        {
                            showRegisterForm ?
                                <Box
                                    sx={{
                                        pt: 2,
                                        borderRadius: `${theme.shape.borderRadius}px ${theme.shape.borderRadius}px 0 0`,
                                    }}
                                >
                                    <RegisterAccount
                                        open={showRegisterForm}
                                        onClose={(closeAll) => {
                                            setShowRegisterForm(false);
                                            if (closeAll) {
                                                onClose();
                                            }
                                        }}
                                    />
                                </Box>
                                :
                                getStepContent()
                        }
                    </Box>
                </Box>
            </Drawer>
        </Box>
    );
}

export default AddNewAccount;