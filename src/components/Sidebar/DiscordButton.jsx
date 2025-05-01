import Box from "@mui/material/Box";
import DiscordLogo from '../DiscordLogo.jsx';
import DiscordLogoFull from '../DiscordLogoFull.jsx';
import { useTheme } from "@mui/material/styles";
import GradientBorderButton from "./ButtonParts/GradientBorderButton.jsx";
import GradientBorderButtonInner from "./ButtonParts/GradientBorderButtonInner.jsx";

function DiscordButton({isHovered, setIsHovered, action}) {
    const theme = useTheme();
    const selected = false;

    return (
        <Box
            sx={{
                transition: 'width 0.5s ease-in-out',
            }}
        >
            <GradientBorderButton
                selected={selected}
                onClick={() => {
                    action?.();
                }}
                onMouseEnter={() => setIsHovered(true)}
                onMouseLeave={() => setIsHovered(false)}
            >
                <GradientBorderButtonInner
                    selected={selected}
                >
                    <Box
                        id="discord-button-content"
                        sx={{
                            position: 'relative',
                            display: 'flex',
                            gap: '8px',
                            alignItems: 'center',
                            justifyContent: 'center',
                            m: 0.5,
                            p: 1,
                            width: isHovered ? '105px' : '26px',
                            transition: theme.transitions.create('width'),
                        }}
                    >
                        <DiscordLogoFull
                            sx={{
                                display: 'flex',
                                width: '100%',
                                position: 'absolute',
                                height: '20px',
                                opacity: isHovered ? 1 : 0,
                                transition: 'opacity 0.3s ease',
                            }}
                        />
                        <DiscordLogo
                            sx={{
                                display: 'flex',
                                width: '100%',
                                position: 'absolute',
                                height: '20px',
                                opacity: isHovered ? 0 : 1,
                                transition: 'opacity 0.3s ease',
                            }}
                        />
                    </Box>
                </GradientBorderButtonInner>
            </GradientBorderButton>
        </Box>
    );
}

export default DiscordButton;