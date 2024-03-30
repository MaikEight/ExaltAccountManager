import { Box, LinearProgress, Paper, darken } from "@mui/material";
import { useEffect, useState } from "react";
import { getAuditLog, getErrorLog } from "../utils/loggingUtils";
import ComponentBox from "../components/ComponentBox";
import HistoryEduOutlinedIcon from '@mui/icons-material/HistoryEduOutlined';
import { DataGrid } from "@mui/x-data-grid";
import CustomToolbar from "../components/GridComponents/CustomToolbar";
import { CustomPagination } from "../components/GridComponents/CustomPagination";
import styled, { useTheme } from "styled-components";
import { formatTime } from "../utils/timeUtils";
import LogsGridToolbar from "../components/GridComponents/LogsGridToolbar";

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

function LogsPage() {
    const [currentLogMode, setCurrentLogMode] = useState('AuditLog'); // 'AuditLog' or 'ErrorLog'
    const [currentLog, setCurrentLog] = useState([]);
    const [currentShownLog, setCurrentShownLog] = useState([]);
    const [currentColumns, setCurrentColumns] = useState([]);
    const [search, setSearch] = useState('');
    const [paginationModel, setPaginationModel] = useState({ page: 0, pageSize: 100 });
    const theme = useTheme();

    const auditLogColumns = [
        { field: 'id', headerName: 'ID', width: 65 },
        { field: 'time', headerName: 'Time', width: 150, type: 'dateTime', renderCell: (params) => <div style={{ textAlign: 'center' }}> {formatTime(params.value)} </div> },
        { field: 'sender', headerName: 'Sender', width: 200 },
        { field: 'message', headerName: 'Message', flex: 0.5 },
        { field: 'accountEmail', headerName: 'Email', flex: 0.325 }
    ];

    const errorLogColumns = [
        { field: 'id', headerName: 'ID', width: 65 },
        { field: 'time', headerName: 'Time', width: 150, type: 'dateTime', renderCell: (params) => <div style={{ textAlign: 'center' }}> {formatTime(params.value)} </div> },
        { field: 'sender', headerName: 'Trigger', width: 200 },
        { field: 'message', headerName: 'Message', minWidth: 115, flex: 0.5 },
    ];

    const getSearchedLogs = () => {
        if (!currentLog) return [];
        if (!search || search === '') return currentLog;
        const searchLower = search.toLowerCase();
        return currentLog.filter((log) => {
            return log.message.toLowerCase().includes(searchLower)
                || log.sender.toLowerCase().includes(searchLower)
                || log.accountEmail?.toLowerCase().includes(searchLower);
        });
    };

    useEffect(() => {
        console.log("search", search);
    }, [search]);

    useEffect(() => {
        setCurrentShownLog(getSearchedLogs());
    }, [currentLog, search]);

    useEffect(() => {
        setCurrentLog([]);
        setCurrentColumns([]);
        switch (currentLogMode) {
            case 'AuditLog':
                // fetch audit log
                setCurrentColumns(auditLogColumns);

                getAuditLog()
                    .then((logs) => {
                        const auditLog = logs.map((log) => {
                            return {
                                ...log,
                                time: new Date(log.time),
                            }
                        }).sort((a, b) => b.id - a.id);
                        console.log('auditlog', auditLog);

                        setCurrentLog(auditLog);
                    });
                break;
            case 'ErrorLog':
                // fetch error log
                setCurrentColumns(errorLogColumns);

                getErrorLog()
                    .then((logs) => {
                        const errorLog = logs.map((log) => {
                            return {
                                ...log,
                                time: new Date(log.time),
                            }
                        }).sort((a, b) => b.id - a.id);
                        setCurrentLog(errorLog);
                    });
                break;
            default:
                break;
        }
    }, [currentLogMode]);

    return (
        <Box id="logspage"
            sx={{
                width: '100%',
                minWidth: '100px',
                p: 2,
            }}
        >
            <Paper sx={{ minHeight: '200px', height: 'calc(100vh - 70px)', width: '100%', borderRadius: 1.5, background: theme.palette.background.paper, }}>
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
                            backgroundColor: theme.palette.background.paper,
                        },
                        '& .MuiDataGrid-virtualScroller::-webkit-scrollbar-thumb': {
                            backgroundColor: theme.palette.mode === 'dark' ? theme.palette.background.default : darken(theme.palette.background.default, 0.15),
                            border: `3px solid ${theme.palette.background.paper}`,
                            borderRadius: 1.5
                        },
                    }}
                    initialState={{
                        columns: {

                        },
                    }}
                    rows={currentShownLog}
                    getRowId={(row) => row.id}
                    columns={currentColumns}
                    pageSizeOptions={[10, 25, 50, 100]}
                    rowSelection                    
                    getEstimatedRowHeight={() => 41}
                    rowCount={currentShownLog.length}
                    // onCellClick={handleCellClick}
                    // onRowSelectionModelChange={(ids) => {
                    //     const selectedId = ids[0];
                    //     const selected = accounts.find((account) => account.id === selectedId);
                    //     if (selected && selected !== selectedAccount) {
                    //         setSelectedAccount(selected);
                    //         return;
                    //     }
                    //     setSelectedAccount(null);
                    // }}
                    // rowSelectionModel={selectedAccount ? [selectedAccount.id] : []}
                    paginationModel={paginationModel}
                    onPaginationModelChange={setPaginationModel}
                    checkboxSelection={false}
                    hideFooterSelectedRowCount
                    slots={{
                        pagination: CustomPagination,
                        toolbar: LogsGridToolbar,
                        loadingOverlay: LinearProgress,
                    }}
                    slotProps={{
                        toolbar: { onSearchChanged: (search) => setSearch(search), selectedLogtype: currentLogMode, setSelectedLogtype: setCurrentLogMode },
                        pagination: { labelRowsPerPage: "Logs per page:" }
                    }}
                />
            </Paper>
        </Box>
    );
}

export default LogsPage;