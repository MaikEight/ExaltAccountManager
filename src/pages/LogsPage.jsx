import { Box, LinearProgress, Paper } from "@mui/material";
import { useEffect, useState } from "react";
import { getAuditLog, getErrorLog } from "../utils/loggingUtils";
import { DataGrid } from "@mui/x-data-grid";
import { CustomPagination } from "../components/GridComponents/CustomPagination";
import { useTheme } from "styled-components";
import { formatTime } from "../utils/timeUtils";
import LogsGridToolbar from "../components/GridComponents/LogsGridToolbar";

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

                        setCurrentLog(auditLog);
                    })
                    .catch((err) => {
                        console.error('getAuditLog', err);
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
                    })
                    .catch((err) => {
                        console.error('getErrorLog', err);
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
            <Paper sx={{ minHeight: '200px', height: 'calc(100vh - 70px)', width: '100%', borderRadius: 1, background: theme.palette.background.paper, }}>
                <DataGrid
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