import { Box, Chip, FormControl, Input, LinearProgress, MenuItem, Pagination, Paper, Radio, Select, Step, StepLabel, Stepper, Table, TableBody, TableContainer, TableHead, TablePagination, TableRow, Tooltip, Typography, alpha } from "@mui/material";
import { useEffect, useMemo, useState } from "react";
import { listen } from '@tauri-apps/api/event'
import { dialog, invoke } from '@tauri-apps/api';
import Papa from 'papaparse';
import ComponentBox from './../components/ComponentBox';
import PlaylistAddOutlinedIcon from '@mui/icons-material/PlaylistAddOutlined';
import InsertDriveFileOutlinedIcon from '@mui/icons-material/InsertDriveFileOutlined';
import TuneOutlinedIcon from '@mui/icons-material/TuneOutlined';
import CheckCircleOutlineOutlinedIcon from '@mui/icons-material/CheckCircleOutlineOutlined';
import DoneOutlinedIcon from '@mui/icons-material/DoneOutlined';
import useColorList from "../hooks/useColorList";
import StyledButton from "../components/StyledButton";
import InfoOutlinedIcon from '@mui/icons-material/InfoOutlined';
import { useTheme } from "@emotion/react";
import ListOutlinedIcon from '@mui/icons-material/ListOutlined';
import DataArrayOutlinedIcon from '@mui/icons-material/DataArrayOutlined';
import AccountBalanceWalletOutlinedIcon from '@mui/icons-material/AccountBalanceWalletOutlined';
import FolderOpenOutlinedIcon from '@mui/icons-material/FolderOpenOutlined';
import SearchOutlinedIcon from '@mui/icons-material/SearchOutlined';
import DataObjectOutlinedIcon from '@mui/icons-material/DataObjectOutlined';
import useSnack from './../hooks/useSnack';
import { readFileUTF8 } from "../utils/readFileUtil";
import ArrowBackIosNewOutlinedIcon from '@mui/icons-material/ArrowBackIosNewOutlined';
import PaddedTableCell from "../components/AccountDetails/PaddedTableCell";
import GroupAddOutlinedIcon from '@mui/icons-material/GroupAddOutlined';
import styled from "styled-components";
import GroupUI from "../components/GridComponents/GroupUI";
import useGroups from "../hooks/useGroups";
import SteamworksMailColumn from "../components/GridComponents/SteamworksMailColumn";
import { DataGrid } from "@mui/x-data-grid";
import { CustomPagination } from "../components/GridComponents/CustomPagination";
import CheckBoxOutlineBlankIcon from '@mui/icons-material/CheckBoxOutlineBlank';
import CheckBoxIcon from '@mui/icons-material/CheckBox';
import useAccounts from './../hooks/useAccounts';
import RuleOutlinedIcon from '@mui/icons-material/RuleOutlined';
import PlayCircleOutlineIcon from '@mui/icons-material/PlayCircleOutline';
import GroupOutlinedIcon from '@mui/icons-material/GroupOutlined';
import { useNavigate } from "react-router-dom";
import DoneAllIcon from '@mui/icons-material/DoneAll';

const StyledDataGrid = styled(DataGrid)`
  &.MuiDataGrid-root .MuiDataGrid-columnHeader:focus,
  &.MuiDataGrid-root .MuiDataGrid-cell {
    outline: none;
    height: 42px;
  },
  &.MuiDataGrid-root .MuiDataGrid-cell:focus-within {
    outline: none;
  },
  &.MuiDataGrid-root .MuiDataGrid-cell:focus {
    outline: none;
  }
`;

const ITEM_HEIGHT = 48;
const ITEM_PADDING_TOP = 8;
const MenuProps = {
    PaperProps: {
        style: {
            maxHeight: ITEM_HEIGHT * 4.5 + ITEM_PADDING_TOP,
            width: 120,
        },
    },
};

async function getAccountsAsCSV(accounts) {
    accounts = await Promise.all(accounts.map(async (obj) => {
        delete obj.extra;
        delete obj.isDeleted;
        delete obj.lastLogin;
        delete obj.lastRefresh;
        delete obj.serverName;
        delete obj.serverName;
        delete obj.state;
        delete obj.steamId;
        delete obj.token;

        const pw = await invoke("decrypt_string", { data: obj.password });
        obj.password = pw;
        return obj;
    }));

    let csv = Papa.unparse(accounts);
    return csv;
}

function parseCsvToObject(csv) {
    let result = Papa.parse(csv, {
        header: true,
        dynamicTyping: true,
        skipEmptyLines: 'greedy',
        transformHeader: header =>
            header
                .toLowerCase()
                .replace(/\W/g, '_')
    });

    return result.data;
}

const steps = [
    {
        label: 'Select file to import',
        icon: <InsertDriveFileOutlinedIcon />,
        optional: false,
    },
    {
        label: 'Map the data',
        icon: <TuneOutlinedIcon />,
        optional: false,
    },
    {
        label: 'Handle duplicates',
        icon: <GroupAddOutlinedIcon />,
        optional: true,
    },
    {
        label: 'Finish',
        icon: <CheckCircleOutlineOutlinedIcon />,
        optional: false,
    }
];

const dataFields = [
    { name: 'email', description: 'The email address of the account, this field must be unique', required: true, nameVariants: ['email', 'mail', 'e-mail', 'e_mail'] },
    { name: 'password', description: 'The password of the account', required: true, nameVariants: ['password', 'pw', 'pass'] },
    { name: 'name', description: 'The name of the account', required: false, nameVariants: ['name', 'accountname', 'account_name', 'acc'] },
    { name: 'performDailyLogin', description: 'If the daily login should be performed', required: false, nameVariants: ['performdailylogin', 'perform_daily_login', 'dailylogin', 'daily_login', 'dailylogin', 'daily'] },
    { name: 'isSteamAccount', description: 'If the account is a steam account', required: false, nameVariants: ['issteamaccount', 'is_steam_account', 'steamaccount', 'steam_account', 'issteam', 'steam'] },
    { name: 'group', description: 'The EAM-Group of the account', required: false, nameVariants: ['group', 'eamgroup', 'eam_group', 'eam'] },
];

const getDataFieldDescriptionByName = (name) => {
    return dataFields.find(f => f.name === name)?.description;
};

function ImporterPage() {
    const [accounts, setAccounts] = useState([]);
    const [accountsWithMappedFields, setAccountsWithMappedFields] = useState([]);
    const [accountKeys, setAccountKeys] = useState([]);
    const [dataFieldsMapping, setDataFieldsMapping] = useState({});
    const [activeStep, setActiveStep] = useState(0);
    const [paginationModel, setPaginationModel] = useState({ page: 0, pageSize: 100 });
    const [importObject, setImportObject] = useState([]);
    const [duplicateSortingPage, setDuplicateSortingPage] = useState(0);
    const [duplicatesPerPage, setDuplicatesPerPage] = useState(25);
    const [shownDuplicates, setShownDuplicates] = useState([]);
    const rowsPerPageOptions = [25, 50, 100];
    const rowCount = Math.ceil(importObject?.duplicates?.length / duplicatesPerPage) || 0;
    const [accountsToImport, setAccountsToImport] = useState([0]);
    const [accountsImported, setAccountsImported] = useState(0);
    const [accountsFailed, setAccountsFailed] = useState([]);
    const [importFinished, setImportFinished] = useState(false);
    const [isImporting, setIsImporting] = useState(false);

    const color = useColorList(0);
    const theme = useTheme();
    const { showSnackbar } = useSnack();
    const { groups } = useGroups();
    const currentAccounts = useAccounts().accounts;
    const updateAccount = useAccounts().updateAccount;
    const navigate = useNavigate();

    const getGroupUI = (params) => {
        if (!params.value) return null;

        const group = groups.find((g) => g.name === params.value);

        if (!group) return null;

        return <GroupUI group={group} />;
    };

    const columns = [
        { field: 'group', headerName: 'Group', width: 65, renderCell: (params) => getGroupUI(params) },
        { field: 'name', headerName: 'Accountname', minWidth: 75, width: 100, flex: 0.2 },
        { field: 'email', headerName: 'Email', minWidth: 150, flex: 0.3, renderCell: (params) => { return (params.value && params.value.startsWith('steamworks:')) ? <SteamworksMailColumn params={params} /> : params.value } },
        { field: 'password', headerName: 'Password', minWidth: 125, flex: 0.3, renderCell: (params) => { return params.value } },
        { field: 'performDailyLogin', headerName: 'Daily Login', width: 95, renderCell: (params) => params.value ? <CheckBoxIcon color="primary" /> : <CheckBoxOutlineBlankIcon /> },
    ];

    const guessMappingField = (fieldName) => {
        const fieldNameLower = fieldName.toLowerCase();
        for (const field of dataFields) {
            if (field.nameVariants.includes(fieldNameLower)) {
                return field.name;
            }
        }

        return '';
    };

    const hasDoubleValues = (fieldName) => {
        if (!fieldName) return false;
        const values = Object.values(dataFieldsMapping)
            .filter(value => value !== '' && value !== fieldName);
        return values.some((value, index, self) => self.indexOf(value) !== index);
    };

    const checkIfMappingIsValid = () => {
        const requiredFields = dataFields.filter(f => f.required);
        const hasAllValues = requiredFields.every(f => Object.values(dataFieldsMapping).includes(f.name));
        const hasDoubleValuesForField = requiredFields.some(field => hasDoubleValues(field.name));
        return hasAllValues && !hasDoubleValuesForField;
    };

    const readFileContent = async (filePath) => {
        const fileName = filePath.split('\\').pop().split('/').pop();
        let lowerCaseFileName = fileName.toLowerCase();

        if (lowerCaseFileName.endsWith('.csv')) {
            const fileContent = await readFileUTF8(filePath);
            if (!fileContent) {
                showSnackbar('Error reading file', 'error');
                return;
            }

            const _accounts = parseCsvToObject(fileContent);
            if (!_accounts || _accounts.length === 0) {
                showSnackbar('Error parsing CSV file, no accounts found', 'error');
                return;
            }
            setAccounts(_accounts);
            setActiveStep(1);
            return;
        }

        if (lowerCaseFileName.endsWith('.json')) {
            const fileContent = await readFileUTF8(filePath, true);
            if (!fileContent) {
                showSnackbar('Error reading file', 'error');
                return;
            }

            const _accounts = fileContent;
            if (!_accounts || _accounts.length === 0) {
                showSnackbar('Error parsing JSON file, no accounts found', 'error');
                return;
            }
            setAccounts(_accounts);
            setActiveStep(1);
            return;
        }

        if (lowerCaseFileName.endsWith('.txt')) {
            const fileContent = await readFileUTF8(filePath);
            if (!fileContent) {
                showSnackbar('Error reading file', 'error');
                return;
            }

            //Try to parse the file as a JSON file
            try {
                const _accounts = JSON.parse(fileContent);
                if (_accounts && _accounts.length > 0) {
                    setAccounts(_accounts);
                    setActiveStep(1);
                    return;
                }
            } catch (error) {
                //If it fails, try to parse it as a CSV file
            }

            //Try to parse the file as a CSV file
            const _accounts = parseCsvToObject(fileContent);
            if (!_accounts || _accounts.length === 0) {
                showSnackbar('Error parsing TXT file, no accounts found', 'error');
                return;
            }
            setAccounts(_accounts);
            setActiveStep(1);

            return;
        }

        if (lowerCaseFileName.endsWith('eam.accounts')) {
            console.log('Parsing EAM.accounts file:', fileName);
            //TODO: Implement EAM.accounts file parsing
            return;
        }

        showSnackbar('File type not supported', 'error');
    };

    const decryptAccountPassword = async (acc) => {
        if (!acc || !acc.password) return acc;
        const pw = await invoke("decrypt_string", { data: acc.password });
        return { ...acc, password: pw };
    };

    const checkForDuplicates = async () => {
        const _duplicates = [];

        for (const acc of accountsWithMappedFields) {
            const importedAccs = accountsWithMappedFields.filter((a) => a.email === acc.email);
            const existingAccsPromise = currentAccounts?.filter((a) => a.email === acc.email).map(async (a) => {
                return await decryptAccountPassword(a);
            });

            let existingAccs = [];
            if (existingAccsPromise && existingAccsPromise.length > 0) {
                existingAccs = await Promise.all(existingAccsPromise);
            }

            if (importedAccs.length > 1 || existingAccs.length > 0) {
                _duplicates.push({
                    email: acc.email,
                    selected: existingAccs.length > 0 ? 0 : 1,
                    existing: existingAccs.length > 0 ? existingAccs : null,
                    imported: importedAccs.length > 1 ? importedAccs : [...importedAccs],
                });
            }
        }

        for (let i = 0, len = _duplicates.length; i < len; i++) {
            _duplicates[i] = { ..._duplicates[i], renderId: i };
        }

        return getImportObject(_duplicates);
    };

    const getImportObject = (_duplicates) => {
        const duplicateEmails = _duplicates.map(d => d.email);
        const nonDuplicates = accountsWithMappedFields.filter(acc => !duplicateEmails.includes(acc.email));
        const importObject = {
            accounts: nonDuplicates,
            duplicates: _duplicates,
        };

        return importObject;
    };

    const updateShownDuplicates = (_duplicates) => {
        if (!_duplicates) {
            _duplicates = importObject?.duplicates;
        }
        if (!_duplicates) {
            setShownDuplicates([]);
            return;
        }

        const dups = _duplicates
            ?.filter((_, index) => index >= duplicateSortingPage * duplicatesPerPage && index < (duplicateSortingPage + 1) * duplicatesPerPage);
        setShownDuplicates([...dups]);
    }

    const mergeSelectedDuplicatesWithAccountsToImport = (_importObject) => {
        if (!_importObject) _importObject = importObject;

        const _duplicates = (_importObject.duplicates && _importObject.duplicates.length > 0) ? [..._importObject.duplicates] : [];
        const accountsToImport = (_importObject.accounts && _importObject.accounts.length > 0) ? [..._importObject.accounts] : [];
        for (const duplicate of _duplicates) {
            if (duplicate.selected < duplicate.existing?.length) {
                accountsToImport.push(duplicate.existing[duplicate.selected]);
            } else {
                accountsToImport.push(duplicate.imported[duplicate.selected - duplicate.existing?.length]);
            }
        }
        return accountsToImport;
    };

    const onChangeExistingAccount = (email, selectedIndex) => {
        const _duplicates = [...importObject.duplicates];
        _duplicates.find(d => d.email === email).selected = selectedIndex;

        setImportObject({ ...importObject, duplicates: _duplicates });
        updateShownDuplicates(_duplicates);
    };

    const onChangeImportedAccount = (email, selectedIndex) => {
        const _duplicates = [...importObject.duplicates];
        const id = _duplicates.findIndex(d => d.email === email);
        _duplicates[id].selected = selectedIndex + _duplicates[id]?.existing?.length || 0;

        setImportObject({ ...importObject, duplicates: _duplicates });
        updateShownDuplicates(_duplicates);
    };

    useEffect(() => {
        updateShownDuplicates();
    }, [duplicateSortingPage, duplicatesPerPage]);

    const duplicateAccountsBoxes = useMemo(() => {
        if (!shownDuplicates || shownDuplicates.length === 0) {
            return null;
        }

        return (
            <Box
                sx={{
                    display: 'flex',
                    flexDirection: 'column',
                    width: '100%',
                    gap: 2,
                }}
            >
                {
                    shownDuplicates.map((duplicate, index) => {
                        return <DuplicateAccountBox
                            key={duplicate.renderId}
                            duplicate={duplicate}
                            onChangeExisting={onChangeExistingAccount}
                            onChangeImported={onChangeImportedAccount}
                        />;
                    })
                }
            </Box>
        );
    }, [shownDuplicates]);

    useEffect(() => {
        let eventCancel;
        listen('tauri://file-drop', event => {
            if (event.payload.length === 0) return;
            if (event.payload.length > 1) {
                showSnackbar('Only one file can be imported at a time');
                return;
            }
            readFileContent(event.payload[0]);
        }).then(cancel => eventCancel = cancel);

        return () => {
            if (eventCancel) {
                eventCancel();
            }
        };
    }, []);

    useEffect(() => {
        if (activeStep === 2) {
            updateShownDuplicates(importObject.duplicates);
        }
    }, [activeStep]);

    useEffect(() => {
        if (accounts.length === 0) {
            return;
        }

        setDataFieldsMapping({});
        setAccountKeys([]);
        setAccountsWithMappedFields([]);
        setPaginationModel({ page: 0, pageSize: 100 });
        setAccountsImported(0);
        setAccountsFailed([]);
        setImportObject([]);
        setAccountsToImport([]);

        const accKeys = Object.keys(accounts[0]);
        setAccountKeys(accKeys);
        const mapping = {};
        accKeys.forEach(key => {
            mapping[key] = guessMappingField(key);
        });
        setDataFieldsMapping(mapping);
    }, [accounts]);

    useEffect(() => {
        if (!checkIfMappingIsValid()) {
            setAccountsWithMappedFields([]);
            return;
        }

        let mappedAccounts = accounts.map(account => {
            const mappedAccount = {};
            Object.keys(dataFieldsMapping).forEach(key => {
                if (dataFieldsMapping[key] === '') return;
                mappedAccount[dataFieldsMapping[key]] = account[key];
            });
            return mappedAccount;
        });
        mappedAccounts = mappedAccounts.map((acc, index) => { return { ...acc, id: index, isDeleted: false } });
        setAccountsWithMappedFields(mappedAccounts);
    }, [dataFieldsMapping]);

    const getCurrentStep = () => {
        switch (activeStep) {
            case 0: // Select File
                return (
                    <Box
                        sx={{
                            width: '100%',
                        }}
                    >
                        <ComponentBox
                            title="Select a file to import"
                            icon={<PlaylistAddOutlinedIcon />}
                            innerSx={{
                                display: 'flex',
                                flexDirection: 'column',
                                justifyContent: 'center',
                                alignItems: 'start',
                                gap: 1,
                            }}
                        >
                            <Typography variant="body1">
                                To import accounts please Drag & Drop a file here or choose file by clicking the button below.
                            </Typography>
                            <StyledButton
                                startIcon={<SearchOutlinedIcon />}
                                onClick={async () => {
                                    const filePath = await dialog.open({
                                        multiple: false,
                                        filters: [
                                            { name: 'Supported Files', extensions: ['csv', 'json', 'txt', 'accounts'] },
                                            { name: 'CSV Files', extensions: ['csv'] },
                                            { name: 'JSON Files', extensions: ['json'] },
                                            { name: 'Text Files', extensions: ['txt'] },
                                            { name: 'EAM.accounts Files', extensions: ['accounts'] },
                                            { name: 'All Files', extensions: ['*'] }
                                        ]
                                    });
                                    if (!filePath) return;

                                    readFileContent(filePath);
                                }}
                            >
                                Choose a file
                            </StyledButton>
                        </ComponentBox>
                        <ComponentBox
                            title="Supported file types and data fields"
                            icon={<InfoOutlinedIcon />}
                        >
                            <Typography variant="body1">
                                Click to expand the boxes below to see more information.
                            </Typography>
                            <ComponentBox
                                title={<TitleTypography title='Data fields' />}
                                icon={<DataObjectOutlinedIcon />}
                                isCollapseable={true}
                                defaultCollapsed={true}
                                sx={{
                                    background: theme.palette.background.default,
                                    m: 1,
                                    p: 1,
                                }}
                                innerSx={{
                                    display: 'flex',
                                    flexDirection: 'column',
                                    alignItems: 'start',
                                    p: 1
                                }}
                            >
                                <Box
                                    sx={{
                                        display: 'flex',
                                        flexDirection: 'row',
                                        gap: 5,
                                    }}
                                >
                                    <Box
                                        sx={{
                                            display: 'flex',
                                            flexDirection: 'column',
                                            gap: 1,
                                        }}
                                    >
                                        <Typography variant="body1">
                                            The following fields are available
                                        </Typography>
                                        <Typography variant="body2">
                                            Only email and password are required. Steam accounts use the Steam ID inside like this <b>Steamwork:{'id'}</b> the email field and the <b>secret</b> in the password field. Also the isSteamAccount field needs to be set to true.
                                        </Typography>
                                        <Box
                                            sx={{
                                                display: 'flex',
                                                flexDirection: 'row',
                                                gap: 5,
                                            }}
                                        >
                                            <ul style={{ marginTop: 8 }}>
                                                <li>email</li>
                                                <li>password</li>
                                                <li>accountname</li>
                                            </ul>
                                            <ul style={{ marginTop: 8 }}>
                                                <li>performDailyLogin</li>
                                                <li>isSteamAccount</li>
                                                <li>group</li>
                                            </ul>
                                        </Box>

                                    </Box>
                                </Box>
                            </ComponentBox>
                            <ComponentBox
                                title={<TitleTypography title=".CSV File" />}
                                icon={<ListOutlinedIcon />}
                                isCollapseable={true}
                                defaultCollapsed={true}
                                sx={{
                                    background: theme.palette.background.default,
                                    m: 1,
                                    p: 1,
                                }}
                                innerSx={{
                                    display: 'flex',
                                    flexDirection: 'column',
                                    p: 1
                                }}
                            >
                                <Typography variant="body2">
                                    CSV files are the most common file format for importing and exporting data. They are simple and easy to use.
                                </Typography>
                                <Typography variant="body2">
                                    The first line of the file should contain the column names.
                                </Typography>
                                <Typography variant="body1" sx={{ mt: 1 }}>
                                    Here is an example of a CSV file:
                                </Typography>
                                <Paper
                                    sx={{
                                        p: 1,
                                        width: 'fit-content',
                                        userSelect: 'text'
                                    }}
                                >
                                    <Typography variant="body2" fontStyle={'mono'}>
                                        email,password,accountname,performDailyLogin,isSteamAccount,group<br />
                                        mail@maik8.de,MyAwesomePassword,MaikEight,true,false,EAM<br />
                                        another@maik8.de,AnotherPassword,AnotherName,false,true
                                    </Typography>
                                </Paper>
                            </ComponentBox>
                            <ComponentBox
                                title={<TitleTypography title='.JSON File' />}
                                icon={<DataArrayOutlinedIcon />}
                                isCollapseable={true}
                                defaultCollapsed={true}
                                sx={{
                                    background: theme.palette.background.default,
                                    m: 1,
                                    p: 1,
                                }}
                                innerSx={{
                                    display: 'flex',
                                    flexDirection: 'column',
                                    p: 1
                                }}
                            >
                                <Typography variant="body2">
                                    JSON files are a bit more complex than CSV files but they are also more flexible.
                                </Typography>
                                <Typography variant="body2">
                                    The file should contain an array of objects.
                                </Typography>
                                <Typography variant="body1" sx={{ mt: 1 }}>
                                    Here is an example of a JSON file:
                                </Typography>
                                <Paper
                                    sx={{
                                        p: 1,
                                        width: 'fit-content',
                                        userSelect: 'text',
                                    }}
                                >
                                    <Typography variant="body2" fontStyle={'mono'}>
                                        {'['}
                                        <br />
                                        &nbsp;&nbsp;&nbsp;&nbsp;{"{"}<br />
                                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;"email": "
                                        <span style={{ color: theme.palette.primary.main }}>
                                            mail@maik8.de
                                        </span>
                                        ",<br />
                                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;"password": "
                                        <span style={{ color: theme.palette.primary.main }}>
                                            MyAwesomePassword
                                        </span>
                                        ",<br />
                                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;"accountname": "
                                        <span style={{ color: theme.palette.primary.main }}>
                                            MaikEight
                                        </span>
                                        ",<br />
                                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;"performDailyLogin": "
                                        <span style={{ color: theme.palette.primary.main }}>
                                            true
                                        </span>
                                        ",<br />
                                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;"isSteamAccount": "
                                        <span style={{ color: theme.palette.primary.main }}>
                                            false
                                        </span>
                                        ",<br />
                                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;"group": "
                                        <span style={{ color: theme.palette.primary.main }}>
                                            EAM
                                        </span>
                                        "<br />
                                        &nbsp;&nbsp;&nbsp;&nbsp;{"}"},<br />
                                        &nbsp;&nbsp;&nbsp;&nbsp;{"{"}<br />
                                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;"email": "
                                        <span style={{ color: theme.palette.primary.main }}>
                                            another@maik8.de
                                        </span>
                                        ",<br />
                                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;"password": "
                                        <span style={{ color: theme.palette.primary.main }}>
                                            AnotherPassword
                                        </span>
                                        ",<br />
                                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;"accountname": "
                                        <span style={{ color: theme.palette.primary.main }}>
                                            AnotherName
                                        </span>
                                        ",<br />
                                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;"performDailyLogin": "
                                        <span style={{ color: theme.palette.primary.main }}>
                                            false
                                        </span>
                                        ",<br />
                                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;"isSteamAccount": "
                                        <span style={{ color: theme.palette.primary.main }}>
                                            true
                                        </span>
                                        ",<br />
                                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;"group": "
                                        <span style={{ color: theme.palette.primary.main }}>

                                        </span>
                                        "<br />
                                        &nbsp;&nbsp;&nbsp;&nbsp;{"}"}
                                        <br />
                                        {']'}
                                    </Typography>
                                </Paper>
                            </ComponentBox>
                            <ComponentBox
                                title={<TitleTypography title="EAM.accounts File" />}
                                icon={<AccountBalanceWalletOutlinedIcon />}
                                isCollapseable={true}
                                defaultCollapsed={true}
                                sx={{
                                    background: theme.palette.background.default,
                                    m: 1,
                                    p: 1,
                                }}
                                innerSx={{
                                    display: 'flex',
                                    flexDirection: 'column',
                                    alignItems: 'start',
                                    p: 1
                                }}
                            >
                                <Typography variant="body2">
                                    EAM.accounts files are encrypted files that contain all the accounts.
                                </Typography>
                                <Typography variant="body2">
                                    They are the save files of older EAM versions (v2.0.8 and older).
                                </Typography>
                                <Typography variant="body2" sx={{ mt: 0.5 }}>
                                    <b>Note:</b> Only save files of EAM v3.0.0 and newer are supported.
                                    If you have an older save file, please try to update EAM to the version 3.3 and import the file there.
                                </Typography>import useSnack from './../hooks/useSnack';


                                <Typography variant="body1" sx={{ mt: 1 }}>
                                    You can find the file in the EAM save file folder located at:
                                </Typography>
                                <Paper
                                    sx={{
                                        p: 1,
                                        width: 'fit-content',
                                        userSelect: 'text'
                                    }}
                                >
                                    <Typography variant="body2" fontStyle={'mono'}>
                                        C:\Users\%username%\AppData\Local\ExaltAccountManager
                                    </Typography>
                                </Paper>
                                <StyledButton
                                    sx={{
                                        mt: 1,
                                    }}
                                    startIcon={<FolderOpenOutlinedIcon />}
                                    onClick={async () => {
                                        const saveFileFolder = await invoke("get_save_file_path").then(res => res.substring(0, res.length - 3));
                                        invoke("open_folder_in_explorer", { path: saveFileFolder });
                                    }}
                                >
                                    Open folder
                                </StyledButton>
                            </ComponentBox>
                        </ComponentBox>
                    </Box>
                );
            case 1: // Map the data
                return (
                    <Box
                        sx={{
                            width: '100%',
                        }}
                    >
                        <ComponentBox
                            title="Match fields to data"
                            icon={<TuneOutlinedIcon />}
                        >
                            <Typography variant="body1">
                                Please match the fields to the data in the file using the Dropdowns.
                            </Typography>
                            <Typography variant="body1">
                                If you don't want to import a field, leave the dropdown empty.
                            </Typography>

                            <Box>
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
                                                <PaddedTableCell sx={{ width: '175px', pl: 0.75, pb: 0.75 }} align="left">Fields in your file</PaddedTableCell>
                                                <PaddedTableCell sx={{ width: '225px', pl: 0.75, pb: 0.75 }} align="left">Corresponding EAM Field</PaddedTableCell>
                                                <PaddedTableCell sx={{ pl: 0.75, }} align="left">Field Description</PaddedTableCell>
                                            </TableRow>
                                        </TableHead>
                                        <TableBody>
                                            {
                                                accountKeys.map((field, index) => {
                                                    return (<TableRow key={index}>
                                                        <PaddedTableCell sx={{ pl: 4, pr: 0.5, pt: 0.75, pb: 0.75, }} align="left">
                                                            <Typography variant="body2" fontWeight={300} component="span">
                                                                {field}
                                                            </Typography>
                                                        </PaddedTableCell>
                                                        <PaddedTableCell sx={{ p: 0.75 }} align="left">
                                                            <Typography variant="body2" fontWeight={300} component="span" sx={{ textAlign: 'center' }}>
                                                                <FormControl sx={{ width: '175px' }}>
                                                                    <Select
                                                                        sx={{
                                                                            height: 39,
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
                                                                            borderRadius: theme.shape.borderRadius,
                                                                        }}
                                                                        id={"data-field-list-label-" + index}
                                                                        value={dataFieldsMapping[field]}
                                                                        onChange={(event) => {
                                                                            const mapping = { ...dataFieldsMapping };
                                                                            mapping[field] = event.target.value;
                                                                            setDataFieldsMapping(mapping);
                                                                        }}
                                                                        input={
                                                                            <Input
                                                                                id={"data-field-list-label-" + index}
                                                                                disableUnderline
                                                                            />
                                                                        }
                                                                        renderValue={(selected) => {
                                                                            if (!selected || selected === '') {
                                                                                return null;
                                                                            }

                                                                            return (
                                                                                <Box
                                                                                    sx={{
                                                                                        display: 'flex',
                                                                                        justifyContent: 'center',
                                                                                        alignItems: 'center',
                                                                                        width: '100%',
                                                                                        height: '100%',
                                                                                        ml: 0.5,
                                                                                        mr: 0.5
                                                                                    }}
                                                                                >
                                                                                    <Chip
                                                                                        key={"key-" + selected}
                                                                                        clickable={false}
                                                                                        label={selected}
                                                                                        size="small"
                                                                                    />
                                                                                </Box>
                                                                            )
                                                                        }}
                                                                        MenuProps={MenuProps}
                                                                    >
                                                                        {
                                                                            ['', ...dataFields].map((acckey, index) => (
                                                                                <MenuItem
                                                                                    key={index}
                                                                                    value={acckey?.name ?? ''}
                                                                                    sx={{
                                                                                        '&.Mui-selected': {
                                                                                            backgroundColor: theme.palette.action.selected,
                                                                                        },
                                                                                        '&.Mui-selected:hover': {
                                                                                            backgroundColor: theme.palette.action.selected,
                                                                                        },
                                                                                        display: 'flex',
                                                                                        justifyContent: 'center',
                                                                                        alignItems: 'center',
                                                                                    }}
                                                                                >
                                                                                    {
                                                                                        acckey ?
                                                                                            <Chip
                                                                                                key={"key-" + acckey.name}
                                                                                                clickable={false}
                                                                                                label={acckey.name}
                                                                                                size="small"
                                                                                            />
                                                                                            :
                                                                                            <Box sx={{ height: '24px' }} >
                                                                                                None
                                                                                            </Box>
                                                                                    }
                                                                                </MenuItem>
                                                                            ))
                                                                        }
                                                                    </Select>
                                                                </FormControl>
                                                            </Typography>
                                                        </PaddedTableCell>
                                                        <PaddedTableCell sx={{ p: 0.75 }}>
                                                            <Typography variant="body2" fontWeight={300} component="span">
                                                                {dataFieldsMapping[field] ? getDataFieldDescriptionByName(dataFieldsMapping[field]) : ''}
                                                            </Typography>
                                                        </PaddedTableCell>
                                                    </TableRow>
                                                    )
                                                })
                                            }

                                        </TableBody>
                                    </Table>
                                </TableContainer>
                            </Box>
                            <Box
                                sx={{
                                    display: 'flex',
                                    flexDirection: 'row',
                                    justifyContent: 'space-between',
                                    alignItems: 'center',
                                    mt: 2,
                                    mb: 1,
                                    mx: 1,
                                }}
                            >
                                <StyledButton
                                    startIcon={<ArrowBackIosNewOutlinedIcon />}
                                    color="secondary"
                                    onClick={() => {
                                        setActiveStep(0);
                                        setAccounts([]);
                                        setDataFieldsMapping({});
                                        setAccountKeys([]);
                                    }}
                                >
                                    Back
                                </StyledButton>
                                <StyledButton
                                    disabled={!checkIfMappingIsValid()}
                                    startIcon={<DoneOutlinedIcon />}
                                    onClick={async () => {
                                        const importObject = await checkForDuplicates();
                                        setImportObject(importObject);

                                        if (importObject.duplicates?.length > 0) {
                                            setActiveStep(2);
                                            return;
                                        }

                                        const _accountsToImport = mergeSelectedDuplicatesWithAccountsToImport(importObject);
                                        setAccountsToImport(_accountsToImport);
                                        setActiveStep(3);
                                    }}
                                >
                                    Next
                                </StyledButton>
                            </Box>
                        </ComponentBox>
                        <ComponentBox
                            title="Data Preview"
                            icon={<GroupAddOutlinedIcon />}
                            isCollapseable={true}
                            innerSx={{
                                display: 'flex',
                                flexDirection: 'column',
                                justifyContent: 'center',
                                p: 0,
                                m: 0,
                                gap: 1,
                            }}
                        >
                            {
                                accountsWithMappedFields && accountsWithMappedFields.length > 0 ?
                                    <>
                                        <Typography variant="body1">
                                            Here is a preview of the data with the selected fields.
                                        </Typography>
                                        <Box
                                            sx={{
                                                minHeight: '200px',
                                                width: '100%',
                                                p: 0,
                                            }}
                                        >
                                            <StyledDataGrid
                                                sx={{
                                                    minHeight: '200px',
                                                    width: '100%',
                                                    maxHeight: '600px',
                                                    border: 0,
                                                    '&, [class^=MuiDataGrid]': { border: 'none' },
                                                    '& .MuiDataGrid-columnHeaders': {
                                                        backgroundColor: theme.palette.background.paperLight,
                                                    },
                                                    '& .MuiDataGrid-virtualScroller::-webkit-scrollbar': {
                                                        backgroundColor: theme.palette.background.paper,
                                                    },
                                                    '& .MuiDataGrid-virtualScroller::-webkit-scrollbar-thumb': {
                                                        backgroundColor: theme.palette.mode === 'dark' ? theme.palette.background.default : darken(theme.palette.background.default, 0.15),
                                                        border: `3px solid ${theme.palette.background.paper}`,
                                                        borderRadius: 1
                                                    },
                                                }}
                                                rows={accountsWithMappedFields}
                                                getRowId={(row) => row.id}
                                                columns={columns}
                                                pageSizeOptions={[10, 25, 50, 100]}
                                                getRowHeight={() => "auto"}
                                                rowSelection
                                                getEstimatedRowHeight={() => 41}
                                                rowCount={accountsWithMappedFields.length}
                                                paginationModel={paginationModel}
                                                onPaginationModelChange={setPaginationModel}
                                                checkboxSelection={false}
                                                hideFooterSelectedRowCount
                                                slots={{
                                                    pagination: CustomPagination,
                                                    loadingOverlay: LinearProgress,
                                                }}
                                                slotProps={{
                                                    pagination: { labelRowsPerPage: "Accounts per page:" }
                                                }}
                                            />
                                        </Box>
                                    </>
                                    :
                                    <>
                                        <Typography variant="body1">
                                            Once you have matched the fields, you will see a preview of the data here.
                                        </Typography>
                                    </>
                            }
                        </ComponentBox>
                    </Box>
                );
            case 2: // Handle duplicates
                return (
                    <Box
                        sx={{
                            display: 'flex',
                            flexDirection: 'column',
                            width: '100%',
                        }}
                    >
                        <ComponentBox
                            title="Handle duplicates"
                            icon={<GroupAddOutlinedIcon />}
                            isCollapseable={true}
                            innerSx={{
                                display: 'flex',
                                flexDirection: 'column',
                                justifyContent: 'center',
                                p: 0,
                                m: 0,
                                gap: 1,
                            }}
                        >
                            <Typography variant="body1">
                                You have duplicates in your data. Please select which account you want to keep. <br />
                                Existing accounts will be updated with the new data, if you choose to keep the imported data.
                            </Typography>
                        </ComponentBox>
                        <Box
                            sx={{
                                position: 'relative',
                                display: 'flex',
                                flexDirection: 'column',
                                width: '100%',
                                gap: 2,

                            }}
                        >
                            <Box>
                                {
                                    duplicateAccountsBoxes
                                }
                            </Box>
                            <Box
                                sx={{
                                    position: 'sticky',
                                    bottom: 0,
                                    left: 16,
                                    right: 16,
                                    backgroundColor: theme.palette.background.default,
                                    borderRadius: `${theme.shape.borderRadius}px`,
                                    mb: -2,
                                }}
                            >
                                <Paper
                                    sx={{
                                        display: 'flex',
                                        flexDirection: 'row',
                                        justifyContent: 'center',
                                        alignItems: 'center',
                                        pr: 1,
                                        pl: 1,
                                        mx: 2,
                                        mb: 2,
                                    }}
                                >
                                    <TablePagination className="pagination-container"
                                        style={{
                                            display: 'flex',
                                            flex: '1 0 auto',
                                            justifyContent: 'end',
                                            alignItems: 'center',
                                        }}
                                        component="div"
                                        count={rowCount}
                                        page={Math.min(Math.max(duplicateSortingPage, 0), Math.max(0, Math.ceil(rowCount / duplicatesPerPage) - 1))}
                                        rowsPerPage={duplicatesPerPage}
                                        onRowsPerPageChange={(event) => {
                                            const newRowsPerPage = parseInt(event.target.value, 10);
                                            const newPage = Math.floor((duplicatesPerPage * duplicateSortingPage) / newRowsPerPage);
                                            setDuplicatesPerPage(newRowsPerPage);
                                            setDuplicateSortingPage(newPage);
                                        }}
                                        onPageChange={(event, newPage) => setDuplicateSortingPage(newPage)}
                                        rowsPerPageOptions={rowsPerPageOptions}
                                        labelDisplayedRows={() => { return ''; }}
                                        labelRowsPerPage={"Duplicates per page:"}
                                        ActionsComponent={() => (
                                            <Pagination className="pagination-pages"
                                                style={{ flex: '1 0 auto', }}
                                                color="primary"
                                                shape="rounded"
                                                page={duplicateSortingPage + 1}
                                                count={rowCount}
                                                onChange={(event, page) => setDuplicateSortingPage(page - 1)}
                                            />
                                        )}
                                    />
                                    <Tooltip
                                        title={duplicateSortingPage !== Math.floor(importObject.duplicates.length / duplicatesPerPage) && "Available once you reached the last page"}
                                    >
                                        <span>
                                            <StyledButton
                                                disabled={duplicateSortingPage !== Math.floor(importObject.duplicates.length / duplicatesPerPage)}
                                                sx={{ ml: 1 }}
                                                startIcon={<DoneOutlinedIcon />}
                                                onClick={() => {
                                                    const _accountsToImport = mergeSelectedDuplicatesWithAccountsToImport();
                                                    setAccountsToImport(_accountsToImport);
                                                    setActiveStep(3);
                                                }}
                                            >
                                                All Done
                                            </StyledButton>
                                        </span>
                                    </Tooltip>
                                </Paper>
                            </Box>
                        </Box>
                    </Box>
                );
            case 3: // Import
                return (
                    <Box
                        sx={{
                            display: 'flex',
                            flexDirection: 'column',
                            width: '100%',
                        }}
                    >
                        {
                            !importFinished ?
                                <ComponentBox
                                    title="Import accounts"
                                    icon={<PlayCircleOutlineIcon />}
                                    isLoading={isImporting}
                                    innerSx={{
                                        display: 'flex',
                                        flexDirection: 'column',
                                        justifyContent: 'center',
                                        p: 0,
                                        m: 0,
                                    }}
                                >
                                    <Typography variant="body1">
                                        You are ready to import the accounts. Please click the button below to start the import.
                                    </Typography>
                                    <Typography variant="body1">
                                        Keep in mind that importing many accounts at once can take some time.
                                    </Typography>
                                    <Typography variant="body1" fontWeight={'bold'}>
                                        Switching to another page or closing the application will stop the import process.
                                    </Typography>

                                    <Box
                                        sx={{
                                            display: 'flex',
                                            flexDirection: 'row',
                                            justifyContent: 'space-between',
                                            alignItems: 'center',
                                            gap: 2,
                                            mt: 2,
                                        }}
                                    >
                                        <StyledButton
                                            disabled={isImporting || importFinished}
                                            startIcon={<ArrowBackIosNewOutlinedIcon />}
                                            color="secondary"
                                            onClick={() => {
                                                setActiveStep(importObject.duplicates?.length > 0 ? 2 : 1);
                                            }}
                                        >
                                            Back
                                        </StyledButton>
                                        {
                                            isImporting && !importFinished &&
                                            <Typography variant="body1" fontWeight={'bold'}>
                                                Importing, Please wait... {accountsImported} / {accountsToImport?.length}
                                            </Typography>
                                        }
                                        <StyledButton
                                            disabled={isImporting || accountsToImport?.length === 0 || importFinished}
                                            startIcon={<PlayCircleOutlineIcon />}
                                            onClick={async () => {
                                                setIsImporting(true);
                                                const _accs = accountsToImport;
                                                const fails = [];
                                                let _accountsImported = 0;
                                                for (const acc of _accs) {
                                                    delete acc.id;
                                                    const keys = Object.keys(acc);
                                                    if (!keys.includes('isSteam')) {
                                                        acc.isSteam = false;
                                                    }

                                                    if (!keys.includes('performDailyLogin')) {
                                                        acc.performDailyLogin = false;
                                                    }

                                                    if (typeof acc.password === 'boolean') {
                                                        acc.password = acc.password.toString();
                                                    }
                                                    if (typeof acc.email === 'boolean') {
                                                        acc.email = acc.email.toString();
                                                    }
                                                    if (typeof acc.group === 'boolean') {
                                                        acc.group = acc.group.toString();
                                                    }

                                                    try {
                                                        const success = await updateAccount(acc, true)
                                                            .catch(err => {
                                                                console.error('Error while importing account:', err);
                                                                return false;
                                                            });
                                                        if (!success) {
                                                            fails.push(acc);
                                                        }
                                                    } catch (err) {
                                                        console.error('Error while importing account (catch):', err);
                                                        fails.push(acc);
                                                    }

                                                    _accountsImported++;
                                                    setAccountsImported(_accountsImported);
                                                }

                                                setAccountsFailed(fails);
                                                setImportFinished(true);
                                                setIsImporting(false);
                                            }}
                                        >
                                            Start Import
                                        </StyledButton>
                                    </Box>
                                </ComponentBox>
                                :
                                <ComponentBox
                                    title={`Import finished${accountsFailed?.length === 0 ? " 🎉" : ''}`}
                                    icon={<DoneAllIcon />}
                                    innerSx={{
                                        display: 'flex',
                                        flexDirection: 'column',
                                        justifyContent: 'center',
                                        gap: 3,
                                    }}
                                >
                                    <Box>
                                        <Typography variant="body1" fontWeight={'bold'}>
                                         All set and done 😎 
                                        </Typography>
                                        <Typography variant="body1">
                                            {accountsImported - accountsFailed.length} accounts have been imported successfully.
                                        </Typography>
                                        {
                                            accountsFailed.length > 0 &&
                                            <Box
                                                sx={{
                                                    display: 'flex',
                                                    flexDirection: 'column',
                                                    mt: 1
                                                }}
                                            >                                                
                                                <Typography variant="body1" fontWeight={'bold'} color="error">
                                                    Failed to import {accountsFailed.length} accounts.
                                                </Typography>

                                                <Box
                                                    sx={{
                                                        display: 'flex',
                                                        flexDirection: 'column',
                                                        gap: 1,
                                                    }}
                                                >
                                                    <Typography variant="body1">
                                                        Failed Accounts:
                                                    </Typography>
                                                    {
                                                        accountsFailed.map((acc, index) => {
                                                            return (
                                                                <Box
                                                                    key={index}
                                                                    sx={{
                                                                        display: 'flex',
                                                                        flexDirection: 'row',
                                                                        gap: 1,
                                                                        userSelect: 'text',
                                                                    }}
                                                                >
                                                                    <Typography variant="body1">
                                                                        {acc.email}
                                                                    </Typography>
                                                                </Box>
                                                            );
                                                        })
                                                    }
                                                </Box>
                                            </Box>
                                        }
                                    </Box>
                                    <StyledButton
                                        sx={{ display: 'flex', alignSelf: 'start' }}
                                        startIcon={<GroupOutlinedIcon />}
                                        onClick={() => {
                                            navigate('/');
                                        }}
                                    >
                                        View Accounts
                                    </StyledButton>
                                </ComponentBox>
                        }
                    </Box>
                );
            default:
                console.warn("Unknown step: ", activeStep + ". How!?");
                return null;
        };
    };

    return (
        <Box
            sx={{
                display: 'flex',
                flex: 1,
                width: '100%',
                maxWidth: '100%',
                overflowY: 'auto',
                flexDirection: 'column',
            }}
        >
            <Stepper
                activeStep={activeStep}
                alternativeLabel
                sx={{
                    ml: 1,
                    borderRadius: `0 0 0 ${theme.shape.borderRadius}px`,
                    zIndex: 1,
                }}
            >
                {steps.map((step, index) => {
                    const stepProps = {};
                    const labelProps = {};

                    if (step.optional) {
                        labelProps.optional = (
                            <Typography
                                variant="caption"
                                fontWeight={'light'}
                                sx={{
                                    color: theme.palette.text.secondary,
                                }}
                            >
                                (Optional)
                            </Typography>
                        );
                    }
                    stepProps.completed = activeStep > index;

                    return (
                        <Step key={step.label} {...stepProps}>
                            <StepLabel icon={stepProps.completed ? <DoneOutlinedIcon /> : step.icon} {...labelProps} sx={{ ...(activeStep === index ? { color: color.color } : {}) }}>{step.label}</StepLabel>
                        </Step>
                    );
                })}
            </Stepper>
            <Box
                sx={{
                    borderRadius: `${theme.shape.borderRadius}px`,
                    mb: 2,
                }}
            >
                {getCurrentStep()}
            </Box>
        </Box>
    );
}

export default ImporterPage;

function TitleTypography({ title }) {
    return (
        <Typography
            variant="p"
            sx={{
                textAlign: 'center',
                mt: '2px'
            }}
            fontWeight={'bold'}
        >
            {title}
        </Typography>
    );
}

function DuplicateAccountBox({ duplicate, onChangeExisting, onChangeImported }) {
    const theme = useTheme();

    if (!duplicate)
        return null;

    return (
        <ComponentBox
            title={`Duplicate #${duplicate.renderId + 1}: ${duplicate.email}`}
            icon={<RuleOutlinedIcon />}
            isCollapseable={true}
            sx={{
                my: 0
            }}
        >
            <Box
                sx={{
                    borderRadius: `${theme.shape.borderRadius}px`,
                    overflow: 'hidden',
                    border: `1px solid ${theme.palette.background.paperLight}`,
                }}
            >
                <TableContainer component={Box}>
                    <Table
                        sx={{
                            '& tbody tr:last-child td, & tbody tr:last-child th': {
                                borderBottom: 'none',
                            },
                        }}
                    >
                        <TableHead>
                            <TableRow sx={{ backgroundColor: theme.palette.background.default }}>
                                <PaddedTableCell sx={{ width: '65px', pl: 0.75, pb: 0.75 }} align="left">Selected</PaddedTableCell>
                                <PaddedTableCell sx={{ width: '65px', pl: 0.75, pb: 0.75 }} align="left">Group</PaddedTableCell>
                                <PaddedTableCell sx={{ width: '75px', pl: 0.75, pb: 0.75 }} align="left">Accountname</PaddedTableCell>
                                <PaddedTableCell sx={{ width: '150px', pl: 0.75, pb: 0.75 }} align="left">Email</PaddedTableCell>
                                <PaddedTableCell sx={{ width: '140px', pl: 0.75, pb: 0.75 }} align="left">Perform Daily Login</PaddedTableCell>
                                <PaddedTableCell sx={{ width: '125px', pl: 0.75, pb: 0.75 }} align="left">Password</PaddedTableCell>
                            </TableRow>
                        </TableHead>
                        <TableBody>
                            {
                                duplicate.existing && duplicate.existing.length > 0 &&
                                <TableRow key={'existingAccRowSplitter'}>
                                    <PaddedTableCell
                                        colSpan={6}
                                        sx={{
                                            pl: 0.75,
                                            pr: 0.5,
                                            pt: 0.75,
                                            pb: 0.75,
                                            backgroundColor: theme.palette.background.paperLight,
                                        }}
                                        align="left"
                                    >
                                        <Typography variant="body2" fontWeight={'bold'} component="span">
                                            Existing accounts
                                        </Typography>
                                    </PaddedTableCell>
                                </TableRow>
                            }
                            {
                                duplicate.existing && duplicate.existing.length > 0 &&
                                duplicate.existing.map((acc, index) => {
                                    return (
                                        <DuplicateAccountsRow
                                            key={acc.email}
                                            acc={acc}
                                            checked={duplicate.selected === index}
                                            onChange={() => onChangeExisting(duplicate.email, index)}
                                        />
                                    )
                                })
                            }
                            {/* Split between existing and imported */}
                            {
                                duplicate.imported && duplicate.imported.length > 0 &&
                                <TableRow key={'importedAccRowSplitter'}>
                                    <PaddedTableCell
                                        colSpan={6}
                                        sx={{
                                            pl: 0.75,
                                            pr: 0.5,
                                            pt: 0.75,
                                            pb: 0.75,
                                            backgroundColor: theme.palette.background.paperLight,
                                        }}
                                        align="left"
                                    >
                                        <Typography variant="body2" fontWeight={'bold'} component="span">
                                            Imported accounts
                                        </Typography>
                                    </PaddedTableCell>
                                </TableRow>
                            }
                            {
                                duplicate.imported && duplicate.imported.length > 0 &&
                                duplicate.imported.map((acc, index) => {
                                    return (
                                        <DuplicateAccountsRow
                                            key={acc.email}
                                            acc={acc}
                                            checked={duplicate.selected === index + duplicate.existing.length}
                                            onChange={() => onChangeImported(duplicate.email, index)}
                                        />
                                    )
                                })
                            }
                        </TableBody>
                    </Table>
                </TableContainer>
            </Box>
        </ComponentBox>
    )
}

function DuplicateAccountsRow({ acc, checked, onChange }) {
    const { groups } = useGroups();

    const getGroupUI = (groupName) => {
        if (!groupName) return null;

        const group = groups.find((g) => g.name === groupName);

        if (!group) return null;

        return <GroupUI group={group} />;
    };

    return (
        <TableRow>
            <PaddedTableCell sx={{ pl: 0.75, pr: 0.5, pt: 0.75, pb: 0.75, }} align="left">
                <Radio
                    size="small"
                    checked={checked}
                    onChange={onChange}
                />
            </PaddedTableCell>
            <PaddedTableCell sx={{ p: 0.75 }} align="left">
                <Typography variant="body2" fontWeight={300} component="span">
                    {getGroupUI(acc.group)}
                </Typography>
            </PaddedTableCell>
            <PaddedTableCell sx={{ p: 0.75 }} align="left">
                <Typography variant="body2" fontWeight={300} component="span">
                    {acc.name}
                </Typography>
            </PaddedTableCell>
            <PaddedTableCell sx={{ p: 0.75 }} align="left">
                <Typography variant="body2" fontWeight={300} component="span">
                    {acc.email}
                </Typography>
            </PaddedTableCell>
            <PaddedTableCell sx={{ p: 0.75 }} align="left">
                {acc.performDailyLogin ? <CheckBoxIcon color="primary" /> : <CheckBoxOutlineBlankIcon />}
            </PaddedTableCell>
            <PaddedTableCell sx={{ p: 0.75 }} align="left">
                <Typography variant="body2" fontWeight={300} component="span">
                    {acc.password}
                </Typography>
            </PaddedTableCell>
        </TableRow>
    );
}