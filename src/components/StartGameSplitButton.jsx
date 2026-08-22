import { useEffect, useMemo, useRef, useState } from "react";
import { invoke } from "@tauri-apps/api/core";
import { Box, ClickAwayListener, CircularProgress, Divider, Paper, Popper, Typography } from "@mui/material";
import PlayCircleFilledWhiteOutlinedIcon from '@mui/icons-material/PlayCircleFilledWhiteOutlined';
import ArrowDropDownIcon from '@mui/icons-material/ArrowDropDown';
import StyledButton from "./StyledButton";
import useStartGame from "../hooks/useStartGame";
import SingleCharacterOverview from "./Widgets/Widgets/Components/SingleCharacterOverview";

/**
 * Split button replacing the plain "start game" button. The main action starts
 * the account normally (last/default character); the dropdown lists the
 * account's characters (styled like the Best Characters widget) so the user can
 * start directly into a specific one.
 *
 * The character list is read from the latest local char/list dataset; picking a
 * character passes its `char_id` through to `startGame`, which pre-selects it in
 * the game before launching.
 */
function StartGameSplitButton({ account, disabled = false, loading = false, onLoadingChange, sx }) {
    const { startGame } = useStartGame();

    const [open, setOpen] = useState(false);
    const [dataset, setDataset] = useState(null);
    const [loadingChars, setLoadingChars] = useState(false);
    const [charsError, setCharsError] = useState(false);
    const anchorRef = useRef(null);

    // Reset the cached character list whenever the account changes.
    useEffect(() => {
        setDataset(null);
        setCharsError(false);
        setOpen(false);
    }, [account?.email]);

    const fetchChars = () => {
        if (!account?.email) return;
        setLoadingChars(true);
        setCharsError(false);
        invoke('get_latest_char_list_dataset_for_account', { email: account.email })
            .then((res) => setDataset(res))
            .catch((err) => {
                console.error("Failed to load characters for account", err);
                setCharsError(true);
            })
            .finally(() => setLoadingChars(false));
    };

    const handleToggle = () => {
        setOpen((prev) => {
            const next = !prev;
            if (next) fetchChars();
            return next;
        });
    };

    const handleStart = async (characterId = null) => {
        setOpen(false);
        if (onLoadingChange) onLoadingChange(true);
        try {
            await startGame(account, characterId);
        } finally {
            if (onLoadingChange) onLoadingChange(false);
        }
    };

    const characters = useMemo(() => {
        const chars = dataset?.character ? [...dataset.character] : [];
        return chars.sort((a, b) => (b.current_fame || 0) - (a.current_fame || 0));
    }, [dataset]);

    // Group parsed items by character id for the equipment previews.
    const itemsByCharId = useMemo(() => {
        if (!dataset?.items) return {};
        const map = {};
        for (const item of dataset.items) {
            if (!item.storage_type_id?.startsWith('char:')) continue;
            const charId = parseInt(item.storage_type_id.split(':')[1], 10);
            if (!map[charId]) map[charId] = [];
            map[charId].push(item);
        }
        return map;
    }, [dataset]);

    const dropdownContent = () => {
        if (loadingChars && !dataset) {
            return (
                <Box sx={{ display: 'flex', alignItems: 'center', justifyContent: 'center', p: 2 }}>
                    <CircularProgress size={22} />
                </Box>
            );
        }
        if (charsError) {
            return <Typography variant="body2" sx={{ p: 2, color: 'text.secondary' }}>Failed to load characters</Typography>;
        }
        if (characters.length === 0) {
            return (
                <Box sx={{ display: 'flex', flexDirection: 'column', alignItems: 'center', gap: 1, p: 2 }}>
                    <img src="/mascot/Search/no_accounts_2_very_low_res.png" alt="No characters" width="60" height="60" />
                    No characters found
                </Box>
            );
        }
        return characters.map((c, i) => (
            <Box
                key={c.char_id ?? i}
                onClick={() => handleStart(c.char_id)}
                sx={{
                    cursor: 'pointer',
                    borderRadius: 1,
                    '&:hover': { backgroundColor: theme => theme.palette.action.hover },
                }}
            >
                <SingleCharacterOverview character={c} number={i + 1} parsedItems={itemsByCharId[c.char_id] || []} />
            </Box>
        ));
    };

    return (
        <>
            <Box ref={anchorRef} sx={{ display: 'flex', width: '100%' }}>
                <StyledButton
                    disabled={disabled}
                    loading={loading}
                    onClick={() => handleStart(null)}
                    sx={{
                        flex: 1,
                        borderTopRightRadius: 0,
                        borderBottomRightRadius: 0,
                        ...sx,
                    }}
                >
                    <PlayCircleFilledWhiteOutlinedIcon size='large' sx={{ mr: 1 }} />
                    start game
                </StyledButton>
                <StyledButton
                    disabled={disabled || loading}
                    aria-label="Start a specific character"
                    onClick={handleToggle}
                    sx={{
                        minWidth: 42,
                        px: 0,
                        borderTopLeftRadius: 0,
                        borderBottomLeftRadius: 0,
                        borderLeft: theme => `1px solid ${theme.palette.primary.dark}`,
                        ...sx,
                    }}
                >
                    <ArrowDropDownIcon />
                </StyledButton>
            </Box>
            <Popper
                open={open}
                anchorEl={anchorRef.current}
                placement="bottom-end"
                style={{ zIndex: 1300 }}
            >
                <ClickAwayListener onClickAway={() => setOpen(false)}>
                    <Paper
                        sx={{
                            mt: 0.5,
                            maxHeight: 420,
                            display: 'flex',
                            flexDirection: 'column',
                            overflow: 'hidden',
                            background: theme => theme.palette.background.default,
                            border: theme => `1px solid ${theme.palette.divider}`,
                            borderRadius: 1,
                        }}
                    >
                        <Box sx={{ px: 1, pt: 0.5, flexShrink: 0 }}>
                            <Typography variant="caption" sx={{ color: 'text.secondary' }}>
                                Start a specific character
                            </Typography>
                            <Divider sx={{ mt: 0.5 }} />
                        </Box>
                        <Box sx={{ overflowY: 'auto', p: 0.5 }}>
                            {dropdownContent()}
                        </Box>
                    </Paper>
                </ClickAwayListener>
            </Popper>
        </>
    );
}

export default StartGameSplitButton;
