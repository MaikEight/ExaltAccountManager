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
            <Box sx={{ width: 'fit-content', height: 0, mt: -2 }} />
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
                    width: 'fit-content',
                }}
            >
                <CreditEntry title={'Muledump'} url={'https://github.com/jakcodex/muledump'} image={'muledump.png'} text={"Thank you for the great project, it helped a lot."}/>
                <CreditEntry title={'Muledump (Tadus Fork)'} url={'https://github.com/TadusPro/muledump'} image={'muledump.png'} text={"Thanks for providing this fork."}/>
                <CreditEntry title={'Muledump-Asset-Compiler'} url={'https://github.com/jakcodex/muledump-asset-compiler'} image={'muledump-asset-compiler.png'} text={"Thanks for the nice assets!"}/>
                <CreditEntry title={'unDraw'} url={'https://undraw.co/'} image={'unDraw.svg'} text={'Thanks for providing the nice illustrations free of charge.'} />
                <Typography component={"div"} variant="body1" color="textSecondary" sx={{mt: 1}}>
                    A Special <b>THANKS</b> goes to these legends
                </Typography>

                <SpecialThanks>
                    <ThanksLink title={'Jakcodex'} url={'https://github.com/jakcodex'} />
                    for his muledump fork and the asset compiler.
                </SpecialThanks>

                <SpecialThanks>
                    <ThanksLink title={'TadusPro'} url={'https://github.com/TadusPro'} />
                    for his muledump fork and active support / feedback.
                </SpecialThanks>

                <SpecialThanks>
                    Faynt for his active help with solving a render issue and contributions to muledump.
                </SpecialThanks>

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
                    width: 'fit-content',
                }}
            >
                <SpecialThanks>
                    <Box
                        sx={{
                            display: 'flex',
                            flexDirection: 'row',
                            gap: '0.5rem',
                            whiteSpace: 'nowrap',
                        }}
                    >
                        <ThanksLink title={'DECA Games'} url={'https://decagames.com/'} />
                        for creating the game
                        <ThanksLink title={'Realm of the Mad God Exalt'} url={'https://www.realmofthemadgod.com/'} />
                    </Box>
                </SpecialThanks>
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
            <span
                style={{
                    display: 'flex',
                    gap: 2,
                    alignItems: 'center',
                    justifyContent: 'center',
                    textWrap: 'none',
                    wrap: 'nowrap',
                    whiteSpace: 'nowrap',
                }}
            >
                {title} <OpenInNewOutlinedIcon sx={{ fontSize: '18px' }} />
            </span>
        </a>
    );
}

function SpecialThanks({ children }) {
    return (
        <Typography component={"div"} variant="body1" color="textSecondary">
            <Box component={'span'} sx={{ display: 'flex', alignContent: 'center', gap: 0.75 }}>
                {children}
            </Box>
        </Typography>
    )
}

function CreditEntry({ title, image, url, text }) {

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
        <Box
            sx={{
                display: 'flex',
                flexDirection: 'row',
                gap: 0.75,
                alignItems: 'center',
                width: 'fit-content'
            }}
        >
            {
                wrapInLink(
                    <Box
                        sx={{
                            display: 'flex',
                            flexDirection: 'row',
                            alignItems: 'center',
                            gap: 1,
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
                        <Typography component={"div"} variant="body1">
                            {title}
                        </Typography>
                        {
                            url &&
                            <OpenInNewOutlinedIcon sx={{ fontSize: '18px' }} />
                        }
                    </Box>
                )
            }
            {
                text &&
                <>
                    <Typography
                        component={"div"}
                        variant="body1"
                        color="textSecondary"
                        sx={{
                            textDecoration: 'none',
                            fontSize: '0.9rem',
                        }}
                    >
                        -
                    </Typography>
                    <Typography
                        component={"div"}
                        variant="body1"
                        color="textSecondary"
                        sx={{
                            textDecoration: 'none',
                            fontSize: '0.9rem',
                        }}
                    >
                        {text}
                    </Typography>
                </>
            }
        </Box>
    );
}

export default CreditsPopup;