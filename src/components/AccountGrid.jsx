import { useTheme } from "@emotion/react";
import { LinearProgress, Paper } from "@mui/material";
import { DataGrid, } from '@mui/x-data-grid';
import { useContext, useEffect, useState } from "react";
import styled from "styled-components";
import { CustomPagination } from "./GridComponents/CustomPagination";
import ServerChip from "./GridComponents/ServerChip";
import DailyLoginCheckbox from "./GridComponents/DailyLoginCheckbox";
import { formatTime } from "../utils/timeUtils";
import CustomToolbar from "./GridComponents/CustomToolbar";
import GroupUI from "./GridComponents/GroupUI";
import GroupsContext from "../contexts/GroupsContext";
import useUserSettings from "../hooks/useUserSettings";

const StyledDataGrid = styled(DataGrid)`
  &.MuiDataGrid-root .MuiDataGrid-columnHeader:focus,
  &.MuiDataGrid-root .MuiDataGrid-cell:focus-within {
    outline: none;
  },
  &.MuiDataGrid-root .MuiDataGrid-cell:focus {
    outline: none;
  }
`;

function AccountGrid({ acc, selected, setSelected, onAccountChanged, setShowAddNewAccount }) {

    const [accounts, setAccounts] = useState(acc);
    const [shownAccounts, setShownAccounts] = useState(acc);
    const [search, setSearch] = useState('');
    const [selectedAccount, setSelectedAccount] = useState(null);
    const [paginationModel, setPaginationModel] = useState({ page: 0, pageSize: 100 });

    const settings = useUserSettings();

    useEffect(() => {
        setAccounts(acc);
    }, [acc]);

    useEffect(() => {
        setShownAccounts(getSearchedAccounts(search));
    }, [accounts]);

    useEffect(() => {
        if (selected === selectedAccount) return;

        setSelectedAccount(selected);
    }, [selected]);

    useEffect(() => {
        setSelected(selectedAccount);
    }, [selectedAccount]);

    const theme = useTheme();
    const { groups } = useContext(GroupsContext);

    const getGroupUI = (params) => {
        if (!params.value) return null;

        const group = groups.find((g) => g.name === params.value);

        if (!group) return null;

        return <GroupUI group={group} />;
    };   
    

    const columns = [
        { field: 'group', headerName: 'Group', width: 65, renderCell: (params) => getGroupUI(params) },
        { field: 'name', headerName: 'Accountname', minWidth: 65, flex: 0.2 },
        { field: 'email', headerName: 'Email', minWidth: 65, flex: 0.3 },
        { field: 'lastLogin', headerName: 'Last Login', minWidth: 100, flex: 0.15, type: 'dateTime', valueFormatter: (params) => formatTime(params.value) },
        { field: 'serverName', headerName: 'Server', width: 125, renderCell: (params) => <ServerChip params={params} /> },
        { field: 'lastRefresh', headerName: 'Last refresh', minWidth: 140, flex: 0.15, valueFormatter: (params) => formatTime(params.value) },
        { field: 'performDailyLogin', headerName: 'Daily Login', width: 95, renderCell: (params) => <DailyLoginCheckbox params={params} onChange={(event) => handleDailyLoginCheckboxChange(event, params)} /> },
    ];

    const handleDailyLoginCheckboxChange = (event, params) => {
        const updatedAccount = { ...accounts.find((account) => account.id === params.id), performDailyLogin: event.target.checked };
        onAccountChanged(updatedAccount);
    };

    const handleCellClick = (params, event) => {
        if (typeof (event.target.type) !== 'undefined') {
            event.stopPropagation();
        }
    };

    const getSearchedAccounts = (search) => {
        if (search === '') return accounts;

        const filteredAccounts = acc.filter((account) => {
            const { name, email, serverName, group } = account;
            
            return name?.toLowerCase().includes(search.toLowerCase()) 
                || email?.toLowerCase().includes(search.toLowerCase()) 
                || serverName?.toLowerCase().includes(search.toLowerCase()) 
                || group?.toLowerCase().includes(search.toLowerCase());
        });
        return filteredAccounts;
    };

    return (
        <>
            <Paper sx={{ height: 'calc(100vh - 70px)', width: '100%', borderRadius: 1.5, background: theme.palette.background.paper, }}>
                <StyledDataGrid
                    sx={{
                        width: '100%',
                        border: 0,
                        '&, [class^=MuiDataGrid]': { border: 'none' },
                        '& .MuiDataGrid-columnHeaders': {
                            backgroundColor: theme.palette.background.paperLight,
                        },
                        '& .MuiDataGrid-virtualScroller::-webkit-scrollbar': {
                            backgroundColor: theme.palette.background.paper,
                        },
                        '& .MuiDataGrid-virtualScroller::-webkit-scrollbar-thumb': {
                            backgroundColor: theme.palette.background.default,
                            border: `4px solid ${theme.palette.background.paper}`,
                            borderRadius: 1.5
                        },
                    }}
                    initialState={{
                        columns: {
                          columnVisibilityModel: settings.getByKeyAndSubKey('accounts', 'columnsHidden'),
                        },
                      }}
                    rows={shownAccounts}
                    getRowId={(row) => row.id}
                    columns={columns}
                    pageSizeOptions={[10, 25, 50, 100]}
                    getRowHeight={() => "auto"}
                    rowSelection
                    getEstimatedRowHeight={() => 41}
                    rowCount={shownAccounts.length}
                    onCellClick={handleCellClick}
                    onRowSelectionModelChange={(ids) => {
                        const selectedId = ids[0];
                        const selected = accounts.find((account) => account.id === selectedId);
                        if (selected && selected !== selectedAccount) {
                            setSelectedAccount(selected);
                            return;
                        }
                        setSelectedAccount(null);
                    }}
                    rowSelectionModel={selectedAccount ? [selectedAccount.id] : []}
                    paginationModel={paginationModel}
                    onPaginationModelChange={setPaginationModel}
                    checkboxSelection={false}
                    hideFooterSelectedRowCount
                    slots={{
                        pagination: CustomPagination,
                        toolbar: CustomToolbar,
                        loadingOverlay: LinearProgress,
                    }}
                    slotProps={{
                        toolbar: { onSearchChanged: (search) => setShownAccounts(getSearchedAccounts(search)), onAddNew: () => setShowAddNewAccount(true) },
                    }}
                />
            </Paper>
        </>
    );
}

export default AccountGrid;