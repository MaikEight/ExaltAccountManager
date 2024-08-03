import { useTheme } from "@emotion/react";
import ComponentBox from "../ComponentBox";
import PopupBase from "./PopupBase";
import { Box, Typography } from "@mui/material";
import OpenInNewOutlinedIcon from '@mui/icons-material/OpenInNewOutlined';

function CreditsPopup() {
    const theme = useTheme();

    return (
        <PopupBase
            title={'Credits & Thanks'}

        >
            <Box sx={{ width: '500px', height: 0, mt: -2 }} />
            <ComponentBox
                title={'Frameworks & Libraries'}
                sx={{
                    background: theme.palette.background.default,
                    m: 0,
                }}
                innerSx={{
                    display: 'flex',
                    flexDirection: 'column',
                    gap: 1.25,
                }}
            >
                <CreditEntry title={'Tauri'} url={'https://tauri.app/'} image={'tauri.svg'} />
                <CreditEntry title={'React'} url={'https://react.dev/'} image={'react.svg'} />
                <CreditEntry title={'Material UI'} url={'https://mui.com/'} image={'mui.svg'} />

            </ComponentBox>
            <ComponentBox
                title={'Resources & People'}
                sx={{
                    background: theme.palette.background.default,
                    m: 0,
                }}
                innerSx={{
                    display: 'flex',
                    flexDirection: 'column',
                    gap: 1.25,
                }}
            >
                <CreditEntry title={'Muledump'} url={'https://github.com/jakcodex/muledump'} image={'muledump.png'} />
                <CreditEntry title={'Muledump (Tadus Fork)'} url={'https://github.com/TadusPro/muledump'} image={'muledump.png'} />
                <CreditEntry title={'Muledump-Asset-Compiler'} url={'https://github.com/jakcodex/muledump-asset-compiler'} image={'muledump-asset-compiler.png'} />
                <Typography variant="body1" color="textSecondary">
                    A Special <b>THANKS</b> goes to these legends:
                </Typography>
                <Typography variant="body1" color="textSecondary">
                    <Box sx={{ display: 'flex', alignContent: 'center', gap: 0.75 }}>
                        <ThanksLink title={'Jakcodex'} url={'https://github.com/jakcodex'} />
                        for his muledump fork and the asset compiler.
                    </Box>
                </Typography>

                <Typography variant="body1" color="textSecondary">
                    <Box sx={{ display: 'flex', alignContent: 'center', gap: 0.75 }}>
                        <ThanksLink title={'TadusPro'} url={'https://github.com/TadusPro'} />
                        for his muledump fork and active support / feedback.
                    </Box>
                </Typography>

                <Typography variant="body1" color="textSecondary">
                    <Box sx={{ display: 'flex', alignContent: 'center', gap: 0.75 }}>
                        {/* <ThanksLink title={'Faynt'} url={'https://github.com/Faynt'} /> */}
                        Faynt for his active help with solving a render issue and contributions to muledump.
                    </Box>
                </Typography>

            </ComponentBox>
            <ComponentBox
                title={'Big Thanks to DECA Games'}
                sx={{
                    background: theme.palette.background.default,
                    m: 0,
                }}
                innerSx={{
                    display: 'flex',
                    flexDirection: 'column',
                    gap: 1.25,
                }}
            >
                <Typography variant="body1" color="textSecondary">
                    <Box sx={{ display: 'flex', alignContent: 'center', gap: 0.75 }}>
                        <ThanksLink title={'DECA Games'} url={'https://decagames.com/'} />
                        for creating the game 
                        <ThanksLink title={'Realm of the Mad God Exalt'} url={'https://www.realmofthemadgod.com/'} />
                    </Box>
                </Typography>
            </ComponentBox>
        </PopupBase>
    );
}

function ThanksLink({ title, url }) {
    return (
        <a
            href={url}
            target="_blank"
            rel="noopener noreferrer"
        >
            <div
                style={{
                    display: 'flex',
                    gap: 2,
                    alignItems: 'center',
                    justifyContent: 'center',
                }}
            >
                {title} <OpenInNewOutlinedIcon sx={{ fontSize: '18px' }} />
            </div>
        </a>
    );
}

function CreditEntry({ title, image, url }) {

    const wrapInLink = (children) => {
        if (url) {
            return (
                <a href={url} target="_blank" rel="noopener noreferrer">
                    {children}
                </a>
            );
        }
        return children;
    }

    return (
        <>
            {
                wrapInLink(
                    <Box
                        sx={{
                            display: 'flex',
                            flexDirection: 'row',
                            alignItems: 'center',
                            gap: 1,
                            maxWidth: 'fit-content',
                        }}
                    >
                        {
                            image &&
                            <Box
                                sx={{
                                    display: 'flex',
                                    width: '32px',
                                    justifyContent: 'center',
                                }}
                            >
                                <img
                                    src={image}
                                    alt={title}
                                    style={{
                                        maxHeight: '24px',
                                        width: 'auto',
                                    }}
                                />
                            </Box>
                        }
                        <Typography variant="body1">
                            {title}
                        </Typography>
                        {
                            url &&
                            <OpenInNewOutlinedIcon sx={{ fontSize: '18px' }} />
                        }
                    </Box>
                )
            }
        </>
    );
}

export default CreditsPopup;