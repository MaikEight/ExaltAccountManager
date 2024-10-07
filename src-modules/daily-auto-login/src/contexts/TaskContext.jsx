import { Box, Paper } from "@mui/material";
import { createContext, useRef } from "react";

const TaskContext = createContext();

function TaskProvider({ children }) {
    const taskRef = useRef(null);

    return (
        <TaskContext.Provider value={{}}>
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
                        height: '64px',
                        borderRadius: theme => `${theme.shape.borderRadius}px`,
                        zIndex: 9999
                    }}
                >
                    {/* Task */}
                </Paper>
            </Box>
        </TaskContext.Provider>
    );
}

export default TaskContext;
export { TaskProvider };