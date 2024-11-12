import { Box, Grid2, IconButton, LinearProgress, Tooltip, Typography } from "@mui/material";
import useTasks from "../../hooks/useTasks";
import PlayCircleOutlinedIcon from '@mui/icons-material/PlayCircleOutlined';
//import PauseCircleOutlinedIcon from '@mui/icons-material/PauseCircleOutlined';
import SkipNextOutlinedIcon from '@mui/icons-material/SkipNextOutlined';
import { useEffect, useState } from "react";

function TaskControlls() {
    const { currentTask } = useTasks();

    const [timeSinceStart, setTimeSinceStart] = useState('00:00');
    const [progressData, setProgressData] = useState({
        progress: 0,
        timeSinceStart: '00:00',
        timeLeft: '00:00',
    });

    const getTimeSinceStart = () => {
        if (!currentTask) {
            return null;
        }

        const now = new Date();
        const diff = now - currentTask.startTime;
        const seconds = Math.floor((diff / 1000) % 60);
        const minutes = Math.floor(diff / 1000 / 60);

        const formattedSeconds = seconds.toString().padStart(2, '0');
        const formattedMinutes = minutes.toString().padStart(2, '0');

        return `${formattedMinutes}:${formattedSeconds}`;
    };

    const getTimeLeft = () => {
        if (!currentTask || !currentTask.endTime) {
            return null;
        }

        const now = new Date();
        const diff = currentTask.endTime - now;
        const seconds = Math.max(Math.floor((diff / 1000) % 60), 0);
        const minutes = Math.max(Math.floor(diff / 1000 / 60), 0);

        const formattedSeconds = seconds.toString().padStart(2, '0');
        const formattedMinutes = minutes.toString().padStart(2, '0');

        return `${formattedMinutes}:${formattedSeconds}`;
    };

    const getPercentageOfTime = () => {
        if (!currentTask || !currentTask.endTime) {
            return null;
        }

        const now = new Date();
        const diff = now - currentTask.startTime;
        const total = currentTask.endTime - currentTask.startTime;
        const percentage = Math.min(Math.max(Math.floor((diff / total) * 100), 0), 100);

        return percentage;
    };

    useEffect(() => {
        setTimeSinceStart("00:00");
        setProgressData({
            progress: 0,
            timeSinceStart: '00:00',
            timeLeft: '00:00',
        });

        if (!currentTask) {
            return;
        }

        const intervalId = setInterval(() => {
            if (currentTask.endTime) {
                const progressValue = {
                    progress: getPercentageOfTime(),
                    timeSinceStart: getTimeSinceStart(),
                    timeLeft: getTimeLeft(),
                }
                setProgressData(progressValue);
                return;
            }
            setTimeSinceStart(getTimeSinceStart());
        }, 250);
        return () => { clearInterval(intervalId); }
    }, [currentTask]);

    if (!currentTask) {
        return null;
    }

    return (
        <Box id="TaskControlls" sx={styles.root}>
            <Grid2 container spacing={1} direction="column" sx={styles.gridRoot}>
                <Grid2 size={'auto'}>
                    {
                        currentTask.type === 'Login' ?
                            <Box sx={styles.gridControlls}>
                                <Tooltip title="Pause">
                                    <IconButton
                                        size="small"
                                    >
                                        <PlayCircleOutlinedIcon sx={{ fontSize: "32px" }} />
                                    </IconButton>
                                </Tooltip>
                                <Tooltip title="Skip">
                                    <IconButton
                                        size="small"
                                    >
                                        <SkipNextOutlinedIcon sx={{ fontSize: "32px" }} />
                                    </IconButton>
                                </Tooltip>
                            </Box>
                            :
                            <Box sx={{ ...styles.gridRoot, height: '42px' }} />
                    }
                </Grid2>
                <Grid2 size={'auto'}>
                    {
                        currentTask.endTime ?
                            <Box sx={styles.gridProgressComplex}>
                                <Box
                                    sx={{
                                        display: "flex",
                                        flexDirection: "row",
                                        justifyContent: "space-between",
                                        width: "100%",
                                    }}
                                >
                                    <Typography variant="body2">
                                        {progressData.timeSinceStart}
                                    </Typography>
                                    <Typography variant="body2">
                                        {progressData.progress}%
                                    </Typography>
                                    <Typography variant="body2">
                                        {progressData.timeLeft}
                                    </Typography>
                                </Box>
                                <LinearProgress
                                    sx={styles.progress}
                                    variant="determinate"
                                    value={progressData.progress}
                                />
                            </Box>
                            :
                            <Box sx={styles.gridProgressSimple}>
                                <Typography variant="body2">
                                    {timeSinceStart}
                                </Typography>
                                <LinearProgress sx={styles.progress} />
                            </Box>
                    }
                </Grid2>
            </Grid2>
        </Box>
    );
}

export default TaskControlls;

const styles = {
    root: {
        width: "100%",
        height: "100%",
    },
    gridRoot: {
        width: "100%",
        height: "100%",
    },
    gridProgressSimple: {
        display: "flex",
        flexDirection: "column",
        justifyContent: "center",
        alignItems: "center",
        width: "100%",
        maxWidth: "100%",
        height: "100%",
        maxHeight: "100%",
        minHeight: "12px",
    },
    gridProgressComplex: {
        display: "flex",
        flexDirection: "column",
        justifyContent: "center",
        alignItems: "center",
        width: "100%",
        maxWidth: "100%",
        height: "100%",
        maxHeight: "100%",
        minHeight: "12px",
    },
    gridControlls: {
        display: "flex",
        flexDirection: "row",
        justifyContent: "center",
        alignItems: "start",
        width: "100%",
        maxWidth: "100%",
        height: "100%",
        maxHeight: "100%",
    },
    progress: {
        width: "100%",
        height: "6px",
        borderRadius: theme => `${theme.shape.borderRadius}px`,
    }
}