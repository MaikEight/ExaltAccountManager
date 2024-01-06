import { Box, Drawer, IconButton, Step, StepIcon, StepLabel, TextField, Tooltip, Typography } from "@mui/material";
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
import { postAccountVerify } from "../backend/decaApi";

const steps = ['Login', 'Add details', 'Finish'];
const icons = [
    <AccountCircleOutlinedIcon />,
    <PersonAddAltOutlinedIcon />,
    <HowToRegOutlinedIcon />,
];

function AddNewAccount({ isOpen, onClose }) {
    const [activeStep, setActiveStep] = useState(0);
    const [skipped, setSkipped] = useState(new Set([]));
    const [isLoading, setIsLoading] = useState(false);

    const [newAccount, setNewAccount] = useState({ email: '', password: '' });

    //Step 1
    const [passwordEmailWrong, setPasswordEmailWrong] = useState(true);

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

    const getStepContent = () => {
        switch (activeStep) {
            case 0: //Login
                return (
                    <ComponentBox
                        headline="Login credentials"
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
                                                    newAccount.data.account = response.Account;
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
                        headline="Add details"
                        isLoading={isLoading}
                    >

                    </ComponentBox>
                );
            case 2: //Finishing
                return <div>Step 3</div>;
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