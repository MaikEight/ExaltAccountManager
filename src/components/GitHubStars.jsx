import { useEffect, useState } from "react";
import { getGitHubStars } from "../backend/gitHubApi";
import { Box, Typography } from "@mui/material";
import StarBorderOutlinedIcon from '@mui/icons-material/StarBorderOutlined';
import GitHubIcon from '@mui/icons-material/GitHub';

function GitHubStars({ style }) {
    const [stars, setStars] = useState(undefined);

    useEffect(() => {
        if (sessionStorage.getItem('githubStars')) {
            setStars(sessionStorage.getItem('githubStars'));
            return;
        }

        getGitHubStars()
            .then((stars) => {
                setStars(stars);
            });
    }, []);

    return (
        <a href="https://github.com/MaikEight/ExaltAccountManager" target="_blank" rel="noopener noreferrer" style={{ textDecoration: 'none', color: 'inherit', ...style }}>
            <Box
                sx={{
                    display: "flex",
                    flexDirection: "column",
                    gap: 0.5,
                    alignItems: "center",
                    justifyContent: "center",
                    width: 110,
                }}
            >
                <GitHubIcon fontSize='large' />
                <Box
                    sx={{
                        display: "flex",
                        flexDirection: "row",
                        gap: 0.5,
                    }}
                >
                    {
                        stars &&
                        <>
                            <StarBorderOutlinedIcon sx={{ mt: -0.125 }} />
                            <Typography>
                                {stars} Stars
                            </Typography>
                        </>
                    }
                </Box>
            </Box>
        </a>
    );
}

export default GitHubStars;