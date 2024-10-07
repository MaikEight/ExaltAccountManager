import { Box, Collapse, LinearProgress, Paper, Typography, Tooltip as MUITooltip, alpha, darken, IconButton } from "@mui/material";
import { invoke } from "@tauri-apps/api/tauri";
import { useEffect, useState } from "react";
import StyledButton from "../components/StyledButton";
import { Chart, BarController, CategoryScale, LinearScale, BarElement, Title, Tooltip, Legend } from 'chart.js';
import { Bar } from 'react-chartjs-2';
import { useTheme } from "@emotion/react";
import ComponentBox from "../components/ComponentBox";
import BarChartOutlinedIcon from '@mui/icons-material/BarChartOutlined';
import WarningAmberOutlinedIcon from '@mui/icons-material/WarningAmberOutlined';
import useSnack from './../hooks/useSnack';
import CheckCircleOutlinedIcon from '@mui/icons-material/CheckCircleOutlined';
import CancelOutlinedIcon from '@mui/icons-material/CancelOutlined';
import styled from "styled-components";
import { DataGrid } from "@mui/x-data-grid";
import { CustomPagination } from "../components/GridComponents/CustomPagination";
import { formatTime } from "eam-commons-js";
import DailyLoginsGridToolbar from "../components/GridComponents/DailyLoginsGridToolbar";
import DailyLoginsSlideout from "../components/DailyLoginsSlideout";
import SettingsOutlinedIcon from '@mui/icons-material/SettingsOutlined';
import InstallDesktopOutlinedIcon from '@mui/icons-material/InstallDesktopOutlined';
import PlayCircleFilledWhiteOutlinedIcon from '@mui/icons-material/PlayCircleFilledWhiteOutlined';
import DeleteOutlineOutlinedIcon from '@mui/icons-material/DeleteOutlineOutlined';
import LockIcon from '@mui/icons-material/Lock';
import LockOpenOutlinedIcon from '@mui/icons-material/LockOpenOutlined';

Chart.register(BarController, CategoryScale, LinearScale, BarElement, Title, Tooltip, Legend);

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

function DailyLoginsPage() {
    const [isTaskInstalled, setIsTaskInstalled] = useState(localStorage.getItem('dailyLoginTaskInstalled') === 'true');
    const [isTaskV1Installed, setIsTaskV1Installed] = useState(localStorage.getItem('dailyLoginTaskV1Installed') === 'true');
    const [dailyLoginReportsOfLastWeek, setDailyLoginReportsOfLastWeek] = useState([]);
    const [dailyLoginReportsOfLastWeekDataSets, setDailyLoginReportsOfLastWeekDataSets] = useState({
        successfulLogins: [],
        failedLogins: []
    });
    const [isInstallingTask, setIsInstallingTask] = useState(false);
    const [allDailyLoginReports, setAllDailyLoginReports] = useState([]);
    const [isLoadingReports, setIsLoadingReports] = useState(false);
    const [paginationModel, setPaginationModel] = useState({ page: 0, pageSize: 100 });
    const [slideoutOpen, setSlideoutOpen] = useState(false);
    const [selectedReport, setSelectedReport] = useState(null);
    const [timeoutFunction, setTimeoutFunction] = useState(null);
    const [isCurrentlyCollapsed, setIsCurrentlyCollapsed] = useState();
    const { showSnackbar } = useSnack();
    const theme = useTheme();

    const convertUtcDatetoLocalDate = (date) => {
        if (!date) return null;

        return new Date(date.getTime() + date.getTimezoneOffset() * 60000);
    }

    const columns = [
        { field: 'hasFinished', headerName: '🏁 Finished', width: 100, renderCell: (params) => <div style={{ display: 'flex', justifyContent: 'center', alignItems: 'center', width: '100%' }}> {params.value ? <CheckCircleOutlinedIcon style={{ color: theme.palette.success.main }} /> : <CancelOutlinedIcon style={{ color: theme.palette.error.main }} />} </div> },
        { field: 'startTime', headerName: '🕛 Start Time', width: 165, flex: 0.1, type: 'dateTime', renderCell: (params) => <div key={params.row.id} style={{ textAlign: 'center' }}> <MUITooltip title={`UTC: ${formatTime(convertUtcDatetoLocalDate(params.value))}`}>{<span>{formatTime(params.value)}</span>}</MUITooltip> </div> },
        { field: 'endTime', headerName: '🕛 End Time', width: 165, flex: 0.1, type: 'dateTime', renderCell: (params) => <div key={params.row.id} style={{ textAlign: 'center' }}> <MUITooltip title={`UTC: ${formatTime(convertUtcDatetoLocalDate(params.value))}`}>{<span>{formatTime(params.value)}</span>}</MUITooltip> </div> },
        { field: 'duration', headerName: '⏱️ Duration', width: 120, flex: 0.1, renderCell: (params) => params.row.endTime && params.row.startTime && <div style={{ textAlign: 'start', paddingLeft: '23px', width: '100%' }}> {params.row.endTime ? `${Math.floor((new Date(params.row.endTime) - new Date(params.row.startTime)) / 1000 / 60)} min` : 'N/A'} </div> },
        { field: 'amountOfAccounts', headerName: '#️⃣ Accounts', width: 100, flex: 0.1, renderCell: (params) => <div style={{ textAlign: 'start', paddingLeft: '23px', width: '100%' }}> {params.value} </div> },
        { field: 'amountOfAccountsFailed', headerName: '🔴 Failed', width: 90, flex: 0.1, renderCell: (params) => <div style={{ textAlign: 'start', paddingLeft: '23px', width: '100%' }}> {params.value} </div> },
        { field: 'amountOfAccountsSucceeded', headerName: '🟢 Successful', width: 120, flex: 0.1, renderCell: (params) => <div style={{ textAlign: 'start', paddingLeft: '23px', width: '100%' }}> {params.value} </div> }
    ];

    const getAllReportData = async () => {
        setIsLoadingReports(true);
        let tasksDone = [false, false]
        invoke('get_daily_login_reports_of_last_days', { amountOfDays: 7 })
            .then((res) => {
                setDailyLoginReportsOfLastWeek(res);
                tasksDone[0] = true;
            })
            .catch((err) => {
                console.error(err);
                tasksDone[0] = true;
            });

        invoke('get_all_daily_login_reports')
            .then((res) => {
                const reports = res.map((report) => {
                    return {
                        ...report,
                        startTime: new Date(report.startTime),
                        endTime: new Date(report.endTime),
                    };
                });
                setAllDailyLoginReports(reports);
                tasksDone[1] = true;
            })
            .catch((err) => {
                console.error(err);
                tasksDone[1] = true;
            });

        while (!tasksDone.every((task) => task)) {
            await new Promise((resolve) => setTimeout(resolve, 25));
        }

        setIsLoadingReports(false);
    };

    useEffect(() => {
        const timeout = setTimeout(async () => {
            await getAllReportData();
            
            invoke('check_for_installed_eam_daily_login_task', { checkForV1: false })
                .then((res) => {
                    setIsTaskInstalled(!!res);
                    localStorage.setItem('dailyLoginTaskInstalled', res ? 'true' : 'false');
                });

            invoke('check_for_installed_eam_daily_login_task', { checkForV1: true })
                .then((res) => {
                    setIsTaskV1Installed(!!res);
                    localStorage.setItem('dailyLoginTaskV1Installed', res ? 'true' : 'false');
                });

        }, 10);

        return () => {
            clearTimeout(timeout);
        };
    }, []);

    useEffect(() => {
        let successfulLogins = [];
        let failedLogins = [];

        for (let i = 0; i < 7; i++) {
            const reportsForDay = dailyLoginReportsOfLastWeek.filter((report) => {
                const reportDate = new Date(report.startTime);
                const today = new Date();
                today.setDate(today.getDate() - (6 - i));
                return reportDate.toDateString() === today.toDateString();
            });

            if (reportsForDay.length === 0) {
                successfulLogins.push(0);
                failedLogins.push(0);
            } else {
                const lastReport = reportsForDay[reportsForDay.length - 1];
                successfulLogins.push(lastReport.amountOfAccountsSucceeded);
                failedLogins.push(lastReport.amountOfAccountsFailed);
            }
        }
        const lastWeek = {
            successfulLogins,
            failedLogins
        };

        setDailyLoginReportsOfLastWeekDataSets(lastWeek);

    }, [dailyLoginReportsOfLastWeek]);

    useEffect(() => {
        setSlideoutOpen(!!selectedReport);
    }, [selectedReport]);

    const data = {
        labels: ['6 days ago', '5 days ago', '4 days ago', '3 days ago', '2 days ago', 'Yesterday', 'Today'],
        datasets: [
            {
                label: 'Successful logins',
                data: dailyLoginReportsOfLastWeekDataSets.successfulLogins,
                backgroundColor: alpha(theme.palette.primary.main, 0.6),
                borderColor: theme.palette.primary.main,
                borderWidth: 2,
                innerborderRadius: theme.shape.borderRadius,
                borderRadius: theme.shape.borderRadius,
            },
            {
                label: 'Failed logins',
                data: dailyLoginReportsOfLastWeekDataSets.failedLogins,
                backgroundColor: alpha(theme.palette.error.main, 0.6),
                borderColor: theme.palette.error.main,
                borderWidth: 2,
                innerborderRadius: theme.shape.borderRadius,
                borderRadius: theme.shape.borderRadius,
            },
        ]
    };

    const options = {
        plugins: {
            tooltip: {
                mode: 'index',
                titleFont: {
                    size: 14,
                    family: theme.typography.fontFamily,
                },
                bodyFont: {
                    size: 14,
                    family: theme.typography.fontFamily,
                },
                footerFont: {
                    size: 14,
                    family: theme.typography.fontFamily,
                }
            },
            legend: {
                labels: {
                    font: {
                        size: 14,
                        family: theme.typography.fontFamily,
                    },
                    padding: 20,
                    useBorderRadius: true,
                    borderRadius: 3,
                },
                align: 'center',
                position: 'bottom',
            }
        },
        scales: {
            y: {
                beginAtZero: true,
                stacked: true,
                ticks: {
                    stepSize: 1,
                    precision: 0,
                    font: {
                        size: 14,
                        family: theme.typography.fontFamily,
                    }
                },
                title: {
                    display: true,
                    text: 'Number of Logins',
                    font: {
                        size: 14,
                        family: theme.typography.fontFamily,
                    }
                }
            },
            x: {
                stacked: true,
                ticks: {
                    font: {
                        size: 14,
                        family: theme.typography.fontFamily,
                    }
                }
            }
        }
    };

    const installTask = async () => {
        setIsInstallingTask(true);

        const _isTaskV1Installed = await invoke('check_for_installed_eam_daily_login_task', { checkForV1: true });
        setIsTaskV1Installed(_isTaskV1Installed);
        localStorage.setItem('dailyLoginTaskV1Installed', _isTaskV1Installed ? 'true' : 'false');

        if (_isTaskV1Installed) {
            const uninstalledSuccessfull = await invoke('uninstall_eam_daily_login_task', { uninstallV1: true })
                .catch((err) => {
                    console.error(err);
                    return false;
                });

            if (!uninstalledSuccessfull) {
                setIsInstallingTask(false);
                showSnackbar('Failed to uninstall the old version of the task, stopping installation. Please try again later.', 'error');
                return;
            }

            localStorage.removeItem('dailyLoginTaskV1Installed');
            setIsTaskV1Installed(false);
        }

        const installedSuccessfull = await invoke('install_eam_daily_login_task')
            .catch((err) => {
                console.error(err);
                return false;
            });

        setIsInstallingTask(false);
        if (!installedSuccessfull) {
            showSnackbar('Failed to install the task, please try again later.', 'error');
            return;
        }

        setIsTaskInstalled(true);
        localStorage.setItem('dailyLoginTaskInstalled', 'true');
        showSnackbar('Task installed successfully', 'success');
    };

    const handleCellClick = (params, event) => {
        if (typeof (event.target.type) !== 'undefined') {
            event.stopPropagation();
        }
    };

    const taskNotInstalledWarningBanner = (
        <Paper
            sx={{
                // width: 'calc(100% - 32px)',
                width: '100%',
                borderRadius: `${theme.shape.borderRadius}px`,
                backgroundColor: theme => theme.palette.mode === 'dark' ? theme.palette.error.dark : theme.palette.error.main,
                display: 'flex',
                justifyContent: 'center',
                alignItems: 'center',
                flexDirection: 'column',
                p: 0.5,
                pb: 1,
            }}
        >
            <Typography variant="h6">
                The daily login task is not installed
            </Typography>
            <Typography variant="body2">
                Without the task, the daily login feature will not work.
            </Typography>
            {
                isTaskV1Installed && (
                    <Box
                        sx={{
                            display: 'flex',
                            justifyContent: 'center',
                            alignItems: 'center',
                            flexDirection: 'row',
                            gap: 2,
                            backgroundColor: theme => theme.palette.mode === 'dark' ? '#0000001f' : '#ffffff0f',
                            p: 1,
                            borderRadius: `${theme.shape.borderRadius}px`,
                            mt: 1,
                        }}
                    >
                        <WarningAmberOutlinedIcon />
                        <Box
                            sx={{
                                display: 'flex',
                                justifyContent: 'center',
                                alignItems: 'center',
                                flexDirection: 'column'
                            }}
                        >
                            <Typography variant="body2">
                                An old version of the task is installed and will be removed during installation.
                            </Typography>
                            <Typography variant="body2">
                                This means that only accounts from this EAM v4 will be used for the daily login.
                            </Typography>
                        </Box>
                        <WarningAmberOutlinedIcon />
                    </Box>
                )
            }
            <StyledButton
                sx={{ mt: 1 }}
                disabled={isInstallingTask}
                color="warning"
                onClick={installTask}
                startIcon={<InstallDesktopOutlinedIcon />}
            >
                Install Task
            </StyledButton>
        </Paper>
    );

    return (
        <Box
            sx={{
                width: '100%',
                position: 'relative',
                overflow: 'auto',
            }}
        >
            <Collapse in={!isTaskInstalled} sx={!isTaskInstalled ? { p: 2 } : { pl: 2, pr: 2 }}>
                {
                    taskNotInstalledWarningBanner
                }
            </Collapse>
            <ComponentBox
                title={<ChatComponentBoxTitle defaultCollapsedChart={isCurrentlyCollapsed} setDefaultCollapsedChart={setIsCurrentlyCollapsed} />}
                icon={<BarChartOutlinedIcon />}
                isCollapseable
                defaultCollapsed={localStorage.getItem('dailyLoginChartCollapsed') === 'true'}
                setIsCurrentlyCollapsed={setIsCurrentlyCollapsed}
            >
                <Box
                    sx={{
                        width: '100%',
                        height: '450px',
                        display: 'flex',
                        justifyContent: 'center',
                        alignItems: 'center',
                    }}
                >
                    <Bar id="paddingBelowLegends" data={data} options={options} />
                </Box>
            </ComponentBox>
            <Box
                sx={{
                    width: '100%',
                    pr: 2,
                    pl: 2,
                }}
            >
                <Paper
                    sx={{
                        minHeight: '200px',
                        height: 'calc(100vh - 271px)',
                        width: '100%',
                        borderRadius: `${theme.shape.borderRadius}px`,
                        background: theme.palette.background.paper,
                    }}
                >
                    <DataGrid                        
                        initialState={{
                            columns: {

                            },
                        }}
                        loading={isLoadingReports}
                        rows={allDailyLoginReports}
                        getRowId={(row) => row.id}
                        columns={columns}
                        pageSizeOptions={[10, 25, 50, 100]}
                        rowSelection
                        getEstimatedRowHeight={() => 41}
                        onCellClick={handleCellClick}
                        onRowSelectionModelChange={(ids) => {
                            const selectedId = ids[0];
                            const selected = allDailyLoginReports.find((report) => report.id === selectedId);
                            if (selected && selected !== selectedReport) {

                                if (timeoutFunction) {
                                    console.log('clearing timeout');
                                    clearTimeout(timeoutFunction);
                                    setTimeoutFunction(null);
                                }

                                setSelectedReport(selected);
                                return;
                            }
                            setSlideoutOpen(false);
                            setTimeoutFunction(setTimeout(() => {
                                setSelectedReport(null);
                                setTimeoutFunction(null);
                            }, 300));
                        }}
                        rowSelectionModel={selectedReport ? [selectedReport.id] : []}
                        paginationModel={paginationModel}
                        onPaginationModelChange={setPaginationModel}
                        checkboxSelection={false}
                        hideFooterSelectedRowCount
                        slots={{
                            pagination: CustomPagination,
                            toolbar: DailyLoginsGridToolbar,
                            loadingOverlay: LinearProgress,
                        }}
                        slotProps={{
                            toolbar: { onRefresh: getAllReportData },
                            pagination: { labelRowsPerPage: "Runs per page:" }
                        }}
                    />
                </Paper>
            </Box>
            <ComponentBox
                title={'Daily Login Task Settings'}
                icon={<SettingsOutlinedIcon />}
                isCollapseable
            >
                <Box
                    sx={{
                        display: 'flex',
                        flexDirection: 'row',
                        gap: 2,
                    }}
                >
                    {
                        !isTaskInstalled &&
                        <StyledButton
                            disabled={isInstallingTask || isTaskInstalled}
                            color="primary"
                            onClick={() => {
                                installTask();
                            }}
                            startIcon={<InstallDesktopOutlinedIcon />}
                        >
                            Install Task Now
                        </StyledButton>
                    }
                    <StyledButton
                        disabled={isInstallingTask || !isTaskInstalled}
                        color="primary"
                        onClick={() => {
                            invoke('run_eam_daily_login_task_now')
                                .then((res) => {
                                    if (res) {
                                        showSnackbar('Task started successfully', 'success');
                                        return;
                                    }
                                    showSnackbar('Failed to start the task, please try again later.', 'error');
                                })
                                .catch((err) => {
                                    console.error(err);
                                    showSnackbar('Failed to start the task, please try again later.', 'error');
                                });
                        }}
                        startIcon={<PlayCircleFilledWhiteOutlinedIcon />}
                    >
                        Run Task now
                    </StyledButton>
                    <StyledButton
                        disabled={isInstallingTask || !isTaskInstalled}
                        color="error"
                        onClick={() => {
                            invoke('uninstall_eam_daily_login_task', { uninstallV1: false })
                                .then(() => {
                                    setIsTaskInstalled(false);
                                    localStorage.removeItem('dailyLoginTaskInstalled');
                                    showSnackbar('Task uninstalled successfully', 'success');
                                })
                                .catch((err) => {
                                    console.error(err);
                                    showSnackbar('Failed to uninstall the task, please try again later.', 'error');
                                });
                        }}
                        startIcon={<DeleteOutlineOutlinedIcon />}
                    >
                        Uninstall Task
                    </StyledButton>
                </Box>
            </ComponentBox>
            <DailyLoginsSlideout isOpen={slideoutOpen} report={selectedReport} onClose={() => {
                setSlideoutOpen(false);
                setTimeout(() => setSelectedReport(null), 300);
            }} />
        </Box>
    );
}

export default DailyLoginsPage;

function ChatComponentBoxTitle({ defaultCollapsedChart }) {
    const [savedCollapsed, setSavedCollapsed] = useState(localStorage.getItem('dailyLoginChartCollapsed') === 'true');
    const [refresh, setRefresh] = useState(0);

    useEffect(() => {
        setSavedCollapsed(localStorage.getItem('dailyLoginChartCollapsed') === 'true');
    }, [defaultCollapsedChart, refresh]);

    return (
        <Box
            sx={{
                display: "flex",
                flexDirection: "row",
                alignItems: "center",
                justifyContent: "space-between",
                width: '100%',
            }}
        >
            <Typography
                variant="h6"
                sx={{
                    fontWeight: 600,
                    textAlign: 'center',
                }}

            >
                Daily Login Reports of the Last Week
            </Typography>
            {
                (defaultCollapsedChart || defaultCollapsedChart !== savedCollapsed) &&
                <IconButton
                    size="small"
                    onClick={(event) => {
                        event.stopPropagation();
                        const storedvalue = localStorage.getItem('dailyLoginChartCollapsed');
                        if (storedvalue === 'true') {
                            localStorage.removeItem('dailyLoginChartCollapsed');
                            setSavedCollapsed(false);
                            setRefresh(refresh + 1);
                            return;
                        }

                        localStorage.setItem('dailyLoginChartCollapsed', defaultCollapsedChart);
                        setSavedCollapsed(true);
                        setRefresh(refresh + 1);
                    }}
                >
                    {
                        (defaultCollapsedChart === savedCollapsed) ?
                            <LockIcon />
                            :
                            <LockOpenOutlinedIcon />
                    }
                </IconButton>
            }
        </Box>
    )
}