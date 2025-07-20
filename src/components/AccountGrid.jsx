import { useTheme } from "@emotion/react";
import { Box, LinearProgress, Paper } from "@mui/material";
import { DataGrid, } from '@mui/x-data-grid';
import { useEffect, useMemo, useState } from "react";
import { CustomPagination } from "./GridComponents/CustomPagination";
import ServerChip from "./GridComponents/ServerChip";
import DailyLoginCheckbox from "./GridComponents/DailyLoginCheckbox";
import { formatTime, useGroups } from "eam-commons-js";
import CustomToolbar from "./GridComponents/CustomToolbar";
import useUserSettings from "../hooks/useUserSettings";
import useAccounts from "../hooks/useAccounts";
import SteamworksMailColumn from "./GridComponents/SteamworksMailColumn";
import useApplySettingsToHeaderName from "../hooks/useApplySettingsToHeaderName";
import NoRowsOverlay from "./GridComponents/NoRowsOverlay";
import { GroupUI } from "./GridComponents/GroupUI";

function AccountGrid({ setShowAddNewAccount }) {
    const { accounts, selectedAccount, setSelectedAccount, updateAccount, isLoading } = useAccounts();
    const theme = useTheme();
    const { groups } = useGroups();
    const settings = useUserSettings();
    const { applySettingsToHeaderName, hideEmojis } = useApplySettingsToHeaderName();

    const [shownAccounts, setShownAccounts] = useState(accounts);
    const [search, setSearch] = useState('');
    const [paginationModel, setPaginationModel] = useState({ page: 0, pageSize: 100 });

    useEffect(() => {
        setShownAccounts(getSearchedAccounts());
    }, [accounts]);

    useEffect(() => {
        setShownAccounts(getSearchedAccounts());
    }, [search]);

    const getGroupUI = (params) => {
        if (!params.value) return null;

        const group = groups.find((g) => g.name === params.value);

        if (!group) return null;

        return (
            <Box
                sx={{
                    mx: 'auto',
                }}
            >
                <GroupUI group={group} />
            </Box>
        );
    };

    const columns = useMemo(() => {
        return [
            { field: 'orderId', headerName: applySettingsToHeaderName('🆔 Order ID'), width: hideEmojis ? 75 : 95 },
            { field: 'group', headerName: applySettingsToHeaderName('👥 Group'), width: hideEmojis ? 65 : 80, renderCell: (params) => getGroupUI(params) },
            { field: 'name', headerName: applySettingsToHeaderName('🗣️ Accountname'), minWidth: 135, width: 230, flex: 0.25 },
            { field: 'email', headerName: applySettingsToHeaderName('📧 Email'), minWidth: 150, flex: 0.35, renderCell: (params) => { return (params.value && params.row.isSteam) ? <SteamworksMailColumn params={params} /> : params.value } },
            { field: 'lastLogin', headerName: applySettingsToHeaderName('⏰ Last Login'), minWidth: 115, flex: 0.125, type: 'dateTime', renderCell: (params) => <div style={{ textAlign: 'center' }}> {formatTime(params.value)} </div> },
            { field: 'serverName', headerName: applySettingsToHeaderName('🌐 Server'), width: 125, renderCell: (params) => <ServerChip params={params} /> },
            { field: 'lastRefresh', headerName: applySettingsToHeaderName('🔄 Refresh'), minWidth: 115, flex: 0.125, renderCell: (params) => <div style={{ textAlign: 'center' }}> {formatTime(params.value)} </div> },
            { field: 'performDailyLogin', headerName: applySettingsToHeaderName('📅 Daily Login'), width: hideEmojis ? 95 : 115, renderCell: (params) => <DailyLoginCheckbox params={params} onChange={(event) => handleDailyLoginCheckboxChange(event, params)} /> },
            { field: 'state', headerName: applySettingsToHeaderName('📊 Last State'), width: 110 },
            { field: 'comment', headerName: applySettingsToHeaderName('💬 Comment'), minWidth: hideEmojis ? 100 : 105, flex: 0.125, renderCell: (params) => (<div style={{ whiteSpace: 'nowrap', overflow: 'hidden', textOverflow: 'ellipsis' }}>{params.value}</div>) },
        ]
    }, [settings, groups]);

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
            <DataGrid
                initialState={{
                    columns: {
                        columnVisibilityModel: { orderId: false, ...settings.getByKeyAndSubKey('accounts', 'columnsHidden') },
                    },
                }}
                rows={shownAccounts}
                getRowId={(row) => row.id}
                columns={columns}
                pageSizeOptions={[10, 25, 50, 100]}
                getRowHeight={() => "auto"}
                rowSelection
                getEstimatedRowHeight={() => 41}
                onCellClick={handleCellClick}
                onRowSelectionModelChange={(model) => {
                    const ids = model?.ids;
                    const selectedId = ids?.keys()?.next()?.value;

                    const selected = accounts.find((account) => account.id === selectedId);
                    if (selected && selected !== selectedAccount) {
                        setSelectedAccount(selected);
                        return;
                    }
                    setSelectedAccount(null);
                }}
                rowSelectionModel={{
                    type: 'include',
                    ids: new Set(selectedAccount ? [selectedAccount.id] : []),
                }}
                paginationModel={paginationModel}
                onPaginationModelChange={setPaginationModel}
                checkboxSelection={false}
                hideFooterSelectedRowCount
                loading={isLoading}
                showToolbar={true}
                slots={{
                    pagination: CustomPagination,
                    toolbar: CustomToolbar,
                    loadingOverlay: LinearProgress,
                    noRowsOverlay: NoRowsOverlay,
                }}
                slotProps={{
                    toolbar: { onSearchChanged: (search) => setSearch(search), onAddNew: () => setShowAddNewAccount(true) },
                    pagination: { labelRowsPerPage: "Accounts per page:" },
                    noRowsOverlay: { onAddNew: () => setShowAddNewAccount(true) },
                    basePopper: {
                        placement: 'bottom-start',
                    },
                }}
            />
        </Paper>
    );
}

export default AccountGrid;