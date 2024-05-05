import { Box, Paper, Typography } from "@mui/material";
import { invoke } from "@tauri-apps/api/tauri";
import { useEffect, useState } from "react";
import StyledButton from "../components/StyledButton";
import { Chart, BarController, CategoryScale, LinearScale, BarElement, Title, Tooltip, Legend } from 'chart.js';
import { Bar } from 'react-chartjs-2';
import { useTheme } from "@emotion/react";
import ComponentBox from "../components/ComponentBox";
import BarChartOutlinedIcon from '@mui/icons-material/BarChartOutlined';

Chart.register(BarController, CategoryScale, LinearScale, BarElement, Title, Tooltip, Legend);

function DailyLoginsPage() {
    const [isTaskInstalled, setIsTaskInstalled] = useState(localStorage.getItem('dailyLoginTaskInstalled') === 'true');
    const [dailyLoginReportsOfLastWeek, setDailyLoginReportsOfLastWeek] = useState([]);
    const [dailyLoginReportsOfLastWeekDataSets, setDailyLoginReportsOfLastWeekDataSets] = useState({
        successfulLogins: [],
        failedLogins: []
    });
    const theme = useTheme();

    useEffect(() => {
        invoke('check_for_installed_eam_daily_login_task').then((res) => {
            setIsTaskInstalled(!!res);
            localStorage.setItem('dailyLoginTaskInstalled', res ? 'true' : 'false');
        });

        invoke('get_daily_login_reports_of_last_days', { amountOfDays: 7 }).then((res) => {
            setDailyLoginReportsOfLastWeek(res);
            console.log('Daily login reports of last week:', res);
        });
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
        setDailyLoginReportsOfLastWeekDataSets({
            successfulLogins,
            failedLogins        
        });

    }, [dailyLoginReportsOfLastWeek]);

    const data = {
        labels: ['6 days ago', '5 days ago', '4 days ago', '3 days ago', '2 days ago', 'Yesterday', 'Today'],
        datasets: [
            {
                label: 'Successful logins',
                data: dailyLoginReportsOfLastWeekDataSets.successfulLogins,
                backgroundColor: theme.palette.primary.main,
                borderRadius: 5,
            },
            {
                label: 'Failed logins',
                data: dailyLoginReportsOfLastWeekDataSets.failedLogins,
                backgroundColor: theme.palette.error.main,
                borderRadius: 5,
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
                    }
                }
            },
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
                        family: 'Times New Roman'
                    }
                }
            }
        }
    };


    const taskNotInstalledWarningBanner = (
        <Paper
            sx={{
                width: 'calc(100% - 32px)',
                borderRadius: '6px',
                backgroundColor: theme => theme.palette.mode === 'dark' ? theme.palette.error.dark : theme.palette.error.main,
                display: 'flex',
                justifyContent: 'center',
                alignItems: 'center',
                flexDirection: 'column',
                p: 0.5,
                pb: 1,
                m: 2,
            }}
        >
            <Typography variant="h6">
                The daily login task is not installed
            </Typography>
            <Typography variant="body2">
                Without the task, the daily login feature will not work.
            </Typography>
            <StyledButton
                sx={{ mt: 1 }}
                color="warning"
                onClick={() => {
                    invoke('install_eam_daily_login_task').then((res) => {
                        console.log(res ? 'Task installed' : 'Task not installed');
                        setIsTaskInstalled(res);
                        localStorage.setItem('dailyLoginTaskInstalled', res ? 'true' : 'false');
                    });
                }}
            >
                Install Task
            </StyledButton>
        </Paper>
    );

    return (
        <Box sx={{ width: '100%', overflow: 'auto', position: 'relative' }}>
            {!isTaskInstalled && taskNotInstalledWarningBanner}
            <ComponentBox
                title={'Daily Login Reports of the Last Week'}
                icon={<BarChartOutlinedIcon />}
            >
                <Bar data={data} options={options} />
            </ComponentBox>
        </Box>
    );
}

export default DailyLoginsPage;