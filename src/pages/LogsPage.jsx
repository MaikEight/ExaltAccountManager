import { Box, LinearProgress, Paper } from "@mui/material";
import { useEffect, useMemo, useState } from "react";
import { getAuditLog, getErrorLog, formatTime } from "eam-commons-js";
import { DataGrid } from "@mui/x-data-grid";
import { CustomPagination } from "../components/GridComponents/CustomPagination";
import { useTheme } from "@mui/material/styles";
import LogsGridToolbar from "../components/GridComponents/LogsGridToolbar";
import LogsNoRowsOverlay from "../components/GridComponents/LogsNoRowsOverlay";
import { NO_LOGS_FOUND_MESSAGES } from "../constants";
import useApplySettingsToHeaderName from "../hooks/useApplySettingsToHeaderName";

function LogsPage() {
    const { applySettingsToHeaderName } = useApplySettingsToHeaderName();

    const [currentLogMode, setCurrentLogMode] = useState('AuditLog'); // 'AuditLog' or 'ErrorLog'
    const [currentLog, setCurrentLog] = useState([]);
    const [currentShownLog, setCurrentShownLog] = useState([]);
    const [currentColumns, setCurrentColumns] = useState([]);
    const [search, setSearch] = useState('');
    const [paginationModel, setPaginationModel] = useState({ page: 0, pageSize: 100 });
    const [isLoading, setIsLoading] = useState(true);
    const [noLogsText, setNoLogsText] = useState(null);

    const noLogsFoundTexts = useMemo(() => 
        NO_LOGS_FOUND_MESSAGES(currentLogMode)
    , [currentLogMode]);

    const theme = useTheme();

    const auditLogColumns = [
        { field: 'id', headerName: applySettingsToHeaderName('🆔 ID'), width: 65 },
        { field: 'time', headerName: applySettingsToHeaderName('🕑 Time'), width: 150, type: 'dateTime', renderCell: (params) => <div style={{ textAlign: 'center' }}> {formatTime(params.value)} </div> },
        { field: 'sender', headerName: applySettingsToHeaderName('👤 Sender'), width: 200 },
        { field: 'message', headerName: applySettingsToHeaderName('💬 Message'), flex: 0.5 },
        { field: 'accountEmail', headerName: applySettingsToHeaderName('📧 Email'), flex: 0.325 }
    ];

    const errorLogColumns = [
        { field: 'id', headerName: applySettingsToHeaderName('🆔 ID'), width: 65 },
        { field: 'time', headerName: applySettingsToHeaderName('🕑 Time'), width: 150, type: 'dateTime', renderCell: (params) => <div style={{ textAlign: 'center' }}> {formatTime(params.value)} </div> },
        { field: 'sender', headerName: applySettingsToHeaderName('👤 Trigger'), width: 200 },
        { field: 'message', headerName: applySettingsToHeaderName('💬 Message'), minWidth: 115, flex: 0.5 },
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
        const searchedLogs = getSearchedLogs();

        if (searchedLogs.length === 0 && currentShownLog.length > 0) {
            setNoLogsText(noLogsFoundTexts[Math.floor(Math.random() * noLogsFoundTexts.length)]);
        } else if (searchedLogs.length > 0 && noLogsText) {
            setNoLogsText(null);
        }

        setCurrentShownLog(searchedLogs);
    }, [currentLog, search]);

    useEffect(() => {
        setIsLoading(true);

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
                    })
                    .finally(() => {
                        setIsLoading(false);
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
                    })
                    .finally(() => {
                        setIsLoading(false);
                    });
                break;
            default:
                setIsLoading(false);
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
                    loading={isLoading}
                    showToolbar={true}
                    slots={{
                        pagination: CustomPagination,
                        toolbar: LogsGridToolbar,
                        loadingOverlay: LinearProgress,
                        noRowsOverlay: LogsNoRowsOverlay,
                    }}
                    slotProps={{
                        toolbar: { onSearchChanged: (search) => setSearch(search), selectedLogtype: currentLogMode, setSelectedLogtype: setCurrentLogMode },
                        pagination: { labelRowsPerPage: "Logs per page:" },
                        noRowsOverlay: { text: noLogsText, isHidden: (!noLogsText || noLogsText?.length === 0) },
                        basePopper: {
                            placement: 'bottom-start',
                        },
                    }}

                />
            </Paper>
        </Box>
    );
}

export default LogsPage;