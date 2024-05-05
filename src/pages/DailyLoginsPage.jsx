import { Box, Paper, Typography } from "@mui/material";
import { invoke } from "@tauri-apps/api/tauri";
import { useEffect, useState } from "react";
import StyledButton from "../components/StyledButton";

function DailyLoginsPage() {
    const [isTaskInstalled, setIsTaskInstalled] = useState(localStorage.getItem('dailyLoginTaskInstalled') === 'true');
    const [dailyLoginReportsOfLastWeek, setDailyLoginReportsOfLastWeek] = useState([]);

    useEffect(() => {
        invoke('check_for_installed_eam_daily_login_task').then((res) => {
            setIsTaskInstalled(!!res);
            localStorage.setItem('dailyLoginTaskInstalled', res ? 'true' : 'false');
        });

        invoke('get_daily_login_reports_of_last_days',{amountOfDays: 7}).then((res) => {
            setDailyLoginReportsOfLastWeek(res);
            console.log('Daily login reports of last week:', res);
        });
    }, []);


    const taskNotInstalledWarningBanner = (
        <Paper
            sx={{
                position: 'relative',
                top: 0,
                left: 32,
                width: 'calc(100% - 32px)',
                borderRadius: '6px 0 0 6px',
                backgroundColor: theme => theme.palette.error.main,
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
        </Box>
    );
}

export default DailyLoginsPage;