import { Box, LinearProgress, Paper } from "@mui/material";
import { useEffect, useMemo, useState } from "react";
import { getAuditLog, getErrorLog, formatTime } from "eam-commons-js";
import { DataGrid } from "@mui/x-data-grid";
import { CustomPagination } from "../components/GridComponents/CustomPagination";
import { useTheme } from "@mui/material/styles";
import LogsGridToolbar from "../components/GridComponents/LogsGridToolbar";
import LogsNoRowsOverlay from "../components/GridComponents/LogsNoRowsOverlay";
import { MASCOT_NAME } from "../constants";

function LogsPage() {
    const [currentLogMode, setCurrentLogMode] = useState('AuditLog'); // 'AuditLog' or 'ErrorLog'
    const [currentLog, setCurrentLog] = useState([]);
    const [currentShownLog, setCurrentShownLog] = useState([]);
    const [currentColumns, setCurrentColumns] = useState([]);
    const [search, setSearch] = useState('');
    const [paginationModel, setPaginationModel] = useState({ page: 0, pageSize: 100 });
    const [isLoading, setIsLoading] = useState(true);
    const [noLogsText, setNoLogsText] = useState('');

    const noLogsFoundTexts = useMemo(() => [
        `${MASCOT_NAME} flipped through every page of the ${currentLogMode}... still nothing.`,
        `The ${currentLogMode} is emptier than ${MASCOT_NAME}'s snack drawer.`,
        `${MASCOT_NAME} triple-checked the ${currentLogMode} — not even a suspicious comma.`,
        `Even with a magnifying glass, ${MASCOT_NAME} couldn't find anything in the ${currentLogMode}.`,
        `No entries. ${MASCOT_NAME} is starting to question the existence of this ${currentLogMode}.`,
        `${MASCOT_NAME} searched the ${currentLogMode} and only found echoes.`,
        `The ${currentLogMode} is so quiet, ${MASCOT_NAME} took a nap waiting for results.`,
        `All clear! Not a single thing in the ${currentLogMode} to report.`,
        `Either the ${currentLogMode} is spotless, or ${MASCOT_NAME} missed something (unlikely).`,
        `${MASCOT_NAME} stared at the ${currentLogMode} for a long time. Nothing stared back.`,
        `If this ${currentLogMode} were a book, it’d be blank. ${MASCOT_NAME} checked.`,
        `The ${currentLogMode} is suspiciously clean. ${MASCOT_NAME} is impressed… maybe too impressed.`,
        `After an exhaustive scan, ${MASCOT_NAME} reports: zero findings in the ${currentLogMode}.`,
        `${MASCOT_NAME} even asked the logs nicely. Still, ${currentLogMode} gave nothing.`,
        `${MASCOT_NAME} brought popcorn for drama in the ${currentLogMode}. Sadly, no show today.`,
        `No chaos found in the ${currentLogMode}. ${MASCOT_NAME} is both relieved and bored.`,
        `${MASCOT_NAME}'s detective hat is on, but the ${currentLogMode} is playing hard to get.`,
        `The ${currentLogMode} is as empty as ${MASCOT_NAME}'s inbox on weekends.`,
        `Not even a typo in sight. ${currentLogMode} is squeaky clean.`,
        `${MASCOT_NAME} checked. Rechecked. Even did a backflip. Still no ${currentLogMode} entries.`
    ], [currentLogMode]);

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
        const searchedLogs = getSearchedLogs();

        if (searchedLogs.length === 0 && currentShownLog.length > 0) {
            setNoLogsText(noLogsFoundTexts[Math.floor(Math.random() * noLogsFoundTexts.length)]);
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
                    slots={{
                        pagination: CustomPagination,
                        toolbar: LogsGridToolbar,
                        loadingOverlay: LinearProgress,
                        noRowsOverlay: LogsNoRowsOverlay,
                    }}
                    slotProps={{
                        toolbar: { onSearchChanged: (search) => setSearch(search), selectedLogtype: currentLogMode, setSelectedLogtype: setCurrentLogMode },
                        pagination: { labelRowsPerPage: "Logs per page:" },
                        noRowsOverlay: { text: noLogsText },
                    }}
                />
            </Paper>
        </Box>
    );
}

export default LogsPage;