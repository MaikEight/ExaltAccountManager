import { Box, Grid2, Typography } from "@mui/material";
import useTasks from './../../hooks/useTasks';

function CurrentTaskPreview() {
    const { currentTask } = useTasks();

    if (!currentTask) {
        return null;
    }

    return (
        <Box id="CurrentTaskPreview" sx={styles.root}>
            <Grid2 container spacing={1} direction="row" sx={styles.gridRoot}>
                <Grid2 size={"auto"} sx={{minWidth: '40px'}}>
                    <Box sx={styles.gridHero}>
                        {currentTask.heroImage}
                    </Box>
                </Grid2>
                <Grid2 size={"grow"}>
                    <Box sx={styles.grid}>
                        <Typography variant="h6">
                            {currentTask.headline}
                        </Typography>
                        <Typography variant="body1">
                            {currentTask.subheadline}
                        </Typography>
                    </Box>
                </Grid2>
            </Grid2>
        </Box>
    );
}

export default CurrentTaskPreview;

const styles = {
    root: {
        width: "100%",
        height: "100%",
    },
    gridRoot: {
        width: "100%",
        height: "100%",
    },
    gridHero: {
        display: "flex",
        flexDirection: "column",
        justifyContent: "center",
        alignItems: "center",
        width: "100%",
        maxWidth: "100%",
        height: "100%",
        maxHeight: "100%",
    },
    grid: {
        display: "flex",
        flexDirection: "column",
        justifyContent: "center",
        alignItems: "start",
        width: "100%",
        maxWidth: "100%",
        height: "100%",
        maxHeight: "100%",
    }
}