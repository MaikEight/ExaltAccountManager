import { Table, Box, Checkbox, Drawer, FormControlLabel, IconButton, Step, StepIcon, StepLabel, TableBody, TableContainer, TableHead, TableRow, TextField, Tooltip, Typography } from "@mui/material";
import Stepper from '@mui/material/Stepper';
import { useContext, useEffect, useState } from "react";
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
import { postAccountVerify } from "../backend/decaApi";
import GroupSelector from "./AccountDetails/GroupSelector";
import GroupsContext from "../contexts/GroupsContext";
import GroupRow from "./AccountDetails/GroupRow";
import TextTableRow from "./AccountDetails/TextTableRow";
import DailyLoginCheckBoxTableRow from "./AccountDetails/DailyLoginCheckBoxTableRow";
import PaddedTableCell from "./AccountDetails/PaddedTableCell";

const steps = ['Login', 'Add details', 'Finish'];
const icons = [
    <AccountCircleOutlinedIcon />,
    <PersonAddAltOutlinedIcon />,
    <HowToRegOutlinedIcon />,
];

function AddNewAccount({ isOpen, onClose, onSave }) {
    const [activeStep, setActiveStep] = useState(0);
    const [skipped, setSkipped] = useState(new Set([]));
    const [isLoading, setIsLoading] = useState(false);

    const [newAccount, setNewAccount] = useState({ email: '', password: '' });

    //STEP 1
    const [passwordEmailWrong, setPasswordEmailWrong] = useState(true);
    //STEP 2
    const { groups } = useContext(GroupsContext);

    const theme = useTheme();
    const hwid = useHWID();
    const color = useColorList(0);

    useEffect(() => {
        setNewAccount({ email: '', password: '' });
        setActiveStep(0);
    }, [isOpen]);

    useEffect(() => {
        const timeout = setTimeout(() => {
            setPasswordEmailWrong(false);
        }, 2500);
        return () => clearTimeout(timeout);
    }, [passwordEmailWrong]);

    const isStepOptional = (step) => {
        return step === -1;
    };

    const isStepSkipped = (step) => {
        return skipped.has(step);
    };

    const isLoginButtonDisabled = () => newAccount.email.length < 3 || !newAccount.email.includes('@') || newAccount.password.length < 3 || isLoading;

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

    const getStepContent = () => {
        switch (activeStep) {
            case 0: //Login
                return (
                    <ComponentBox
                        headline={steps[0]}
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
                            <TextField id="email" label="E-Mail" variant="standard" error={passwordEmailWrong} value={newAccount.email} onChange={(event) => setNewAccount({ ...newAccount, email: event.target.value })} />
                            <TextField id="password" label="Password" type="password" error={passwordEmailWrong} variant="standard" value={newAccount.password} onChange={(event) => setNewAccount({ ...newAccount, password: event.target.value })} />
                        </Box>

                        <Box sx={{ display: 'flex', justifyContent: 'end' }}>
                            <Tooltip
                                title={isLoginButtonDisabled() ? 'Please enter a valid login credentials' : ''}
                                placement="bottom"
                                slotProps={{
                                    popper: {
                                        modifiers: [
                                            {
                                                name: 'offset',
                                                options: {
                                                    offset: [0, -10],
                                                },
                                            },
                                        ],
                                    },
                                }}
                            >
                                <span>
                                    <StyledButton
                                        disabled={isLoginButtonDisabled()}
                                        onClick={() => {
                                            setIsLoading(true);
                                            postAccountVerify(newAccount, hwid)
                                                .then((response) => {
                                                    if (response.Error) {
                                                        setPasswordEmailWrong(true);
                                                        setNewAccount({ ...newAccount, password: '' });
                                                        return;
                                                    }
                                                    if (newAccount.data === undefined) newAccount.data = { account: null, charList: null };
                                                    setNewAccount({
                                                        ...newAccount,
                                                        ...(response.Account ? { name: response.Account.Name } : {}),
                                                        data: {
                                                            ...newAccount.data,
                                                            account: response.Account
                                                        }
                                                    });

                                                    setActiveStep(1);
                                                })
                                                .catch((error) => {
                                                    console.log(error);
                                                    setPasswordEmailWrong(true);
                                                    setNewAccount({ ...newAccount, password: '' });
                                                })
                                                .finally(() => {
                                                    setIsLoading(false);
                                                });
                                        }}
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
                        headline={steps[1]}
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
                        headline={steps[2]}
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
                                    onSave(newAccount);
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
                    height: 45,
                    pt: 0.5,
                    backgroundColor: theme.palette.background.paperLight,
                    position: 'sticky',
                    top: 0,
                    zIndex: 1,
                }}
            >
                <IconButton
                    sx={{ position: 'absolute', left: 16, marginLeft: 0, marginRight: 2 }}
                    size="small"
                    onClick={() => onClose()}
                >
                    <CloseIcon />
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