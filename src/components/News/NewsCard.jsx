import { Box, Paper, Typography, useTheme } from "@mui/material";
import { useEffect, useRef, useState } from "react";
import Confetti from "react-confetti";
import { NewsRenderer } from "./NewsComponent";


/*
newsItem
{
    title: "News Title",
    subtitle: "News Subtitle",
    heading: {
        hasConfetti: true,
        children: [
            //NewsComponents like images...
        ]
    },
    children: [
        // NewsComponents like images...
    ]
}
*/
function NewsCard({ newsItem }) {
    const theme = useTheme();
    const boxHeaderRef = useRef(null);

    const [windowSize, setWindowSize] = useState({
        width: 0,
        height: 0,
        visible: true,
    });

    useEffect(() => {
        if (!boxHeaderRef.current) {
            return;
        }
        setWindowSize({
            width: boxHeaderRef.current?.offsetWidth || 0,
            height: boxHeaderRef.current?.offsetHeight || 0,
            visible: windowSize.visible,
        });
    }, [boxHeaderRef]);

    return (
        <Paper
            sx={{
                display: 'flex',
                flexDirection: 'column',
                alignItems: 'center',
                justifyContent: 'center',
                borderRadius: `${theme.shape.borderRadius * 2}px`,
                width: '925px',
                maxHeight: '95vh',
                maxWidth: '90wh',
                overflow: 'hidden',
            }}
        >
            <Box
                id="news-card-header"
                ref={boxHeaderRef}
                {...newsItem.heading?.props}
                sx={{
                    position: 'relative',
                    display: 'flex',
                    flexDirection: 'column',
                    alignItems: 'center',
                    justifyContent: 'center',
                    width: '100%',
                    borderRadius: `${theme.shape.borderRadius * 2 - 1}px`,
                    backgroundColor: theme.palette.background.default,
                    p: 2,
                    py: 4,
                    ...newsItem.heading?.props.sx
                }}
            >
                    {
                        newsItem.heading?.hasConfetti &&
                        <Confetti
                            width={windowSize?.width || 0}
                            height={windowSize?.height || 0}
                            style={{
                                borderRadius: `${theme.shape.borderRadius * 2 - 1}px`,
                                opacity: windowSize.visible ? 1 : 0,
                                transition: 'opacity 1s ease-in-out',
                            }}
                        />
                    }
                    {
                        newsItem.heading?.children && (
                            <NewsRenderer content={newsItem.heading.children} />
                        )
                    }
            </Box>
            <Box
                sx={{
                    display: 'flex',
                    width: '100%',
                    borderRadius: `${theme.shape.borderRadius - 1}px`,
                    pt: 2,
                    px: 2,
                    pb: 1,
                }}
            >
                {/* HEADLINE */}
                <Box
                    sx={{
                        display: 'flex',
                        flexDirection: 'column',
                        width: '100%',
                        alignItems: 'start',
                        justifyContent: 'center',
                    }}
                >
                    <Typography variant="h6" component="h2" fontWeight="bold" color={theme.palette.primary.main}>
                        {newsItem.title}
                    </Typography>
                    <Typography variant="subtitle1" color="textSecondary">
                        {newsItem.subtitle}
                    </Typography>
                </Box>
            </Box>
            {/* CONTENT */}
            {
                newsItem.children &&
                <NewsRenderer content={newsItem.children} />
            }
        </Paper>
    );
}

export default NewsCard;