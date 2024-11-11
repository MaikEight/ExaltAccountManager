import { Box, Grid2, Paper } from "@mui/material";
import { createContext, useRef, useState } from "react";
import SystemUpdateAltOutlinedIcon from '@mui/icons-material/SystemUpdateAltOutlined';
import CurrentTaskPreview from "../components/tasks/CurrentTaskPreview";
import TaskControlls from "../components/tasks/TaskControlls";
import Divider from '@mui/material/Divider';

const TASK_TYPES = {
    "Update check": {
        headline: "Game Update",
        subheadline: "Check if an update is available...",
        heroImage: <SystemUpdateAltOutlinedIcon sx={{ fontSize: "32px" }} />,
    },
    "Update game": {
        headline: "Game Update",
        subheadline: "Updating the game...",
        heroImage: <SystemUpdateAltOutlinedIcon sx={{ fontSize: "32px" }} />,
    },
    "Login": {},
};

// const TASK_DEFAULT = {
//     type: "none", // "Update check", "Update game", "Login"
//     heroImage: null,
//     headline: "",
//     subheadline: "",
//     startTime: null,
//     endTime: null,
// };

const TaskContext = createContext();

function TaskProvider({ children }) {
    const taskRef = useRef(null);  
    
    const [currentTask, setCurrentTask] = useState(null);

    // useEffect(() => {
    //     setCurrentTask({
    //         type: "Update check",
    //         heroImage: <SystemUpdateAltOutlinedIcon sx={{ fontSize: "32px" }} />,
    //         headline: "Game Update",
    //         subheadline: "Check if an update is available...",
    //         startTime: new Date(),
    //         endTime: null,
    //     });

    //     const timeoutId2 = setTimeout(() => {            
    //         setCurrentTask({
    //             type: "Update game",
    //             heroImage: <SystemUpdateAltOutlinedIcon sx={{ fontSize: "32px" }} />,
    //             headline: "Game Update",
    //             subheadline: "Updating the game...",
    //             startTime: new Date(),
    //             endTime: null,
    //         });
    //     }, 3000);

    //     const timeoutId = setTimeout(() => {
    //         const nowPlus90Seconds = new Date();
    //         nowPlus90Seconds.setSeconds(nowPlus90Seconds.getSeconds() + 90);
    //         const eamGroup = groups.find(g => g.name === "EAM");
            
    //         setCurrentTask({
    //             type: "Login",
    //             heroImage: <GroupUI group={eamGroup} />,
    //             headline: "Login: MaikHell",
    //             subheadline: "Starting the game",
    //             startTime: new Date(),
    //             endTime: nowPlus90Seconds,
    //         });
    //     }, 9000);

    //     return () => {
    //         clearTimeout(timeoutId);
    //         clearTimeout(timeoutId2);
    //     };
    // }, [groups]);

    const updateTask = (task) => {
        if(!task) {
            console.error("No task provided.");
            return;
        }

        if(!TASK_TYPES[task.type] === undefined) {
            console.error(`Task type ${task.type} is not valid.`);
            return;
        }

        const ty = TASK_TYPES[task.type];
        setCurrentTask({
            type: ty.type,
            heroImage: task.heroImage ?? ty.heroImage ?? null,
            headline: task.headline ?? ty.headline ?? "",
            subheadline: task.subheadline ?? ty.subheadline ?? "",
            startTime: task.startTime ?? new Date(),
            endTime: task.endTime ?? null,
        });
    };
    
    const value = {
        currentTask,
        taskTypes: TASK_TYPES,
        
        updateTask,
    }

    return (
        <TaskContext.Provider value={value}>
            <Box
                sx={{
                    position: "relative",
                    display: "flex",
                    flexDirection: "column",
                    width: "100%",
                }}
            >
                {children}
                <Box
                    sx={{
                        height: taskRef.current?.clientHeight ?? 0,
                        width: '1px',
                    }}
                />
                <Paper
                    ref={taskRef}
                    sx={{
                        position: "absolute",
                        bottom: 8,
                        left: '8px',
                        right: '8px',
                        height: '96px',
                        borderRadius: theme => `${theme.shape.borderRadius}px`,
                        zIndex: 9999,
                        p: 1,
                        overflow: "hidden",
                    }}
                >
                    <Box
                        sx={{
                            width: "100%",
                            height: "100%",
                        }}
                    >
                        <Grid2 container spacing={2} sx={{ height: '100%' }}>
                            <Grid2 size={4}>
                                <CurrentTaskPreview />
                            </Grid2>
                            <Divider orientation="vertical" flexItem sx={{ opacity: 0.6 }} />
                            <Grid2 size={4}>
                                <TaskControlls />
                            </Grid2>
                            <Divider orientation="vertical" flexItem sx={{ opacity: 0.6 }} />
                            <Grid2 size={4}>
                            </Grid2>
                        </Grid2>
                    </Box>
                </Paper>
            </Box>
        </TaskContext.Provider>
    );
}

export default TaskContext;
export { TaskProvider };

