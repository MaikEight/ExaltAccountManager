import { Box, CssBaseline, ThemeProvider as MuiThemeProvider } from "@mui/material";
import { ThemeProvider as StyledThemeProvider } from "styled-components";
import Sidebar from "./components/Sidebar";
import ColorContext from "./contexts/ColorContext";
import { useContext } from "react";
import ComponentBox from "./components/ComponentBox";

function MainRouter() {
    const colorContext = useContext(ColorContext);
    const theme = colorContext.theme;

    return (
        <StyledThemeProvider theme={theme}>
            <MuiThemeProvider theme={theme}>
                <CssBaseline enableColorScheme />
                <>
                    <Sidebar >
                    <ComponentBox>
                            <Box sx={{
                                
                            }}>
                                <p>Content</p>
                                <p>Content</p>
                                <p>Content</p>
                            </Box>
                        </ComponentBox>
                        <ComponentBox>
                            <Box sx={{
                                
                            }}>
                                <p>Content</p>
                                <p>Content</p>
                                <p>Content</p>
                            </Box>
                        </ComponentBox>
                    </Sidebar>
                </>
            </MuiThemeProvider>
        </StyledThemeProvider>
    );
}

export default MainRouter;