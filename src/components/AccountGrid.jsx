import { useTheme } from "@emotion/react";
import { LinearProgress, Paper, darken } from "@mui/material";
import { DataGrid, } from '@mui/x-data-grid';
import { useEffect, useState } from "react";
import styled from "styled-components";
import { CustomPagination } from "./GridComponents/CustomPagination";
import ServerChip from "./GridComponents/ServerChip";
import DailyLoginCheckbox from "./GridComponents/DailyLoginCheckbox";
import { formatTime } from "../utils/timeUtils";
import CustomToolbar from "./GridComponents/CustomToolbar";
import GroupUI from "./GridComponents/GroupUI";
import useUserSettings from "../hooks/useUserSettings";
import useAccounts from "../hooks/useAccounts";
import SteamworksMailColumn from "./GridComponents/SteamworksMailColumn";
import useGroups from "../hooks/useGroups";

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

function AccountGrid({ setShowAddNewAccount }) {
    const { accounts, selectedAccount, setSelectedAccount, updateAccount } = useAccounts();
    const [shownAccounts, setShownAccounts] = useState(accounts);
    const [search, setSearch] = useState('');
    const [paginationModel, setPaginationModel] = useState({ page: 0, pageSize: 100 });

    const settings = useUserSettings();
    useEffect(() => {
        setShownAccounts(getSearchedAccounts());
    }, [accounts]);

    useEffect(() => {
        setShownAccounts(getSearchedAccounts());
    }, [search]);

    const theme = useTheme();
    const { groups } = useGroups();

    const getGroupUI = (params) => {
        if (!params.value) return null;

        const group = groups.find((g) => g.name === params.value);

        if (!group) return null;

        return <GroupUI group={group} />;
    };


    const columns = [
        { field: 'group', headerName: 'Group', width: 65, renderCell: (params) => getGroupUI(params) },
        { field: 'name', headerName: 'Accountname', minWidth: 150, width: 230, flex: 0.2 },
        { field: 'email', headerName: 'Email', minWidth: 150, flex: 0.3, renderCell: (params) => {  return (params.value && params.row.isSteam ) ? <SteamworksMailColumn params={params} /> : params.value } },
        { field: 'lastLogin', headerName: 'Last Login', minWidth: 115, flex: 0.125, type: 'dateTime', renderCell: (params) => <div style={{ textAlign: 'center' }}> {formatTime(params.value)} </div> },
        { field: 'serverName', headerName: 'Server', width: 125, renderCell: (params) => <ServerChip params={params} /> },
        { field: 'lastRefresh', headerName: 'Last refresh', minWidth: 115, flex: 0.125, renderCell: (params) => <div style={{ textAlign: 'center' }}> {formatTime(params.value)} </div> },
        { field: 'performDailyLogin', headerName: 'Daily Login', width: 95, renderCell: (params) => <DailyLoginCheckbox params={params} onChange={(event) => handleDailyLoginCheckboxChange(event, params)} /> },
        { field: 'state', headerName: 'Last State', width: 110 },
    ];

    const handleDailyLoginCheckboxChange = async (event, params) => {
        const acc = accounts.find((account) => account.id === params.id);
        if (!acc) {
            console.error('Account not found');
            return;
        }
        const updatedAccount = { ...acc, performDailyLogin: event.target.checked };
        await updateAccount(updatedAccount, false);
    };

    const handleCellClick = (params, event) => {
        if (typeof (event.target.type) !== 'undefined') {
            event.stopPropagation();
        }
    };

    const getSearchedAccounts = () => {
        if (search === '') return accounts;

        const filteredAccounts = accounts.filter((account) => {
            const { name, email, serverName, group } = account;

            return name?.toLowerCase().includes(search.toLowerCase())
                || email?.toLowerCase().includes(search.toLowerCase())
                || serverName?.toLowerCase().includes(search.toLowerCase())
                || group?.toLowerCase().includes(search.toLowerCase());
        });
        return filteredAccounts;
    };

    return (
        <Paper sx={{ minHeight: '200px', height: 'calc(100vh - 70px)', width: '100%', background: theme.palette.background.paper, }}>
            <StyledDataGrid
                sx={{
                    minHeight: '200px',
                    width: '100%',
                    border: 0,
                    '&, [class^=MuiDataGrid]': { border: 'none' },
                    '& .MuiDataGrid-columnHeaders': {
                        backgroundColor: theme.palette.background.paperLight,
                    },
                    '& .MuiDataGrid-virtualScroller::-webkit-scrollbar': {
                        borderRadius: theme.shape.borderRadius,
                        backgroundColor: theme.palette.background.paper,
                    },
                    '& .MuiDataGrid-virtualScroller::-webkit-scrollbar-thumb': {
                        backgroundColor: theme.palette.mode === 'dark' ? theme.palette.background.default : darken(theme.palette.background.default, 0.15),
                        border: `3px solid ${theme.palette.background.paper}`,
                        borderRadius: theme.shape.borderRadius,
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
                    toolbar: { onSearchChanged: (search) => setSearch(search), onAddNew: () => setShowAddNewAccount(true) },
                    pagination: { labelRowsPerPage: "Accounts per page:" }
                }}
            />
        </Paper>
    );
}

export default AccountGrid;