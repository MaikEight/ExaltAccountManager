import { useEffect, useMemo, useState } from "react";
import { Box, IconButton, Skeleton, Tooltip, Typography, alpha } from "@mui/material";
import { useTheme } from "@emotion/react";
import ChevronLeftRoundedIcon from '@mui/icons-material/ChevronLeftRounded';
import ChevronRightRoundedIcon from '@mui/icons-material/ChevronRightRounded';
import CheckCircleRoundedIcon from '@mui/icons-material/CheckCircleRounded';
import MonetizationOnRoundedIcon from '@mui/icons-material/MonetizationOnRounded';
import { items } from "../../assets/runtimeAssets";
import { drawItemAsync } from "../../utils/realmItemDrawUtils";
import { TooltipUiForItem } from "../Widgets/Widgets/Components/InventoryRender";

const ITEM_PADDING = 0;
const SPRITE_SIZE = 40;
const CELL_MIN_HEIGHT = 56;
const WEEKDAYS = ['Mon', 'Tue', 'Wed', 'Thu', 'Fri', 'Sat', 'Sun'];

/** Shifts a "YYYY-MM" month string by `delta` months. */
function addMonths(month, delta) {
    const [y, m] = month.split('-').map(Number);
    const d = new Date(y, (m - 1) + delta, 1);
    return `${d.getFullYear()}-${String(d.getMonth() + 1).padStart(2, '0')}`;
}

/** Human label for a "YYYY-MM" month, e.g. "July 2026". */
function monthLabel(month) {
    const [y, m] = month.split('-').map(Number);
    return new Date(y, m - 1, 1).toLocaleString(undefined, { month: 'long', year: 'numeric' });
}

/**
 * A single calendar rectangle: day/tier number top-left, item sprite centered
 * (no backdrop), an optional quantity badge and a bottom-right corner icon.
 */
function DayCell({ label, reward, cornerIcon = null, dim = false, sx }) {
    const [img, setImg] = useState(null);

    const itemId = reward?.item_id ?? -1;
    const quantity = reward?.quantity ?? 0;
    const gold = reward?.gold ?? 0;
    const itemData = itemId > 0 ? items[itemId] : null;

    useEffect(() => {
        let cancelled = false;
        if (!itemData) {
            setImg(null);
            return;
        }
        drawItemAsync(itemData, 0, ITEM_PADDING)
            .then((src) => { if (!cancelled) setImg(src); })
            .catch(() => { if (!cancelled) setImg(null); });
        return () => { cancelled = true; };
    }, [itemId]);

    const center = img ? (
        <img
            src={img}
            alt={itemData ? itemData[0] : `Item ${itemId}`}
            width={SPRITE_SIZE}
            height={SPRITE_SIZE}
            style={{ display: 'block', imageRendering: 'pixelated' }}
            draggable={false}
        />
    ) : gold > 0 && itemId <= 0 ? (
        <MonetizationOnRoundedIcon sx={{ color: 'warning.light', fontSize: 26 }} />
    ) : itemData ? (
        <Skeleton variant="rectangular" width={SPRITE_SIZE} height={SPRITE_SIZE} />
    ) : itemId > 0 ? (
        <Typography variant="caption" sx={{ color: 'text.disabled', fontSize: '0.6rem' }}>#{itemId}</Typography>
    ) : null;

    const body = (
        <Box
            sx={{
                position: 'relative',
                minHeight: CELL_MIN_HEIGHT,
                display: 'flex',
                alignItems: 'center',
                justifyContent: 'center',
                opacity: dim ? 0.5 : 1,
                ...sx,
            }}
        >
            <Typography
                variant="caption"
                sx={{ position: 'absolute', top: 2, left: 4, fontSize: '0.6rem', lineHeight: 1, color: 'text.secondary' }}
            >
                {label}
            </Typography>

            {center}

            {quantity > 1 && (
                <Typography
                    variant="caption"
                    sx={{
                        position: 'absolute',
                        bottom: 2,
                        left: 3,
                        backgroundColor: 'rgba(0, 0, 0, 0.7)',
                        color: 'white',
                        px: 0.4,
                        borderRadius: 0.5,
                        fontSize: '0.6rem',
                        fontWeight: 'bold',
                        lineHeight: 1.3,
                        pointerEvents: 'none',
                    }}
                >
                    {quantity > 999 ? '999+' : quantity}
                </Typography>
            )}

            {cornerIcon && (
                <Box sx={{ position: 'absolute', bottom: 2, right: 2, display: 'flex', lineHeight: 0 }}>
                    {cornerIcon}
                </Box>
            )}
        </Box>
    );

    return itemData
        ? <Tooltip title={<TooltipUiForItem item={itemData} />} arrow>{body}</Tooltip>
        : body;
}

/**
 * Renders the daily-login reward calendar in one of two layouts:
 *  - variant="month": weekday-aligned month grid with visible cell lines; the
 *    cell for date N shows reward tier N and gets a corner marker for dates in
 *    `loginDates`.
 *  - variant="tier": sequential Day 1..N grid with a 2-stage highlight
 *    (unlocked-but-unclaimed vs. claimed) derived from `accountStatus`.
 */
function LoginRewardsCalendar({
    variant = 'month',
    month,
    availableMonths = [],
    onMonthChange,
    rewards = [],
    loginDates = [],
    accountStatus = null,
    isLoading = false,
    emptyState = null,
    subHeader = null,
}) {
    const theme = useTheme();

    const rewardsByDay = useMemo(() => {
        const map = new Map();
        for (const r of rewards) map.set(r.day, r);
        return map;
    }, [rewards]);

    const loginDateSet = useMemo(() => new Set(loginDates), [loginDates]);

    const claimedSet = useMemo(() => {
        const csv = accountStatus?.claimed_days ?? '';
        return new Set(csv.split(',').filter(Boolean).map(Number));
    }, [accountStatus]);
    const unlockableDays = accountStatus?.unlockable_days ?? 0;

    const sortedMonths = useMemo(() => [...availableMonths].sort(), [availableMonths]);
    const canPrev = !!month && sortedMonths.length > 0 && month > sortedMonths[0];
    const canNext = !!month && sortedMonths.length > 0 && month < sortedMonths[sortedMonths.length - 1];

    const tierHighlight = (day) => {
        if (claimedSet.has(day)) return 'claimed';
        if (day <= unlockableDays) return 'unlocked';
        return 'locked';
    };

    const cornerCheck = (color) => (
        <CheckCircleRoundedIcon
            sx={{ fontSize: 15, color, backgroundColor: 'background.paper', borderRadius: '50%' }}
        />
    );

    // --- Grid content ------------------------------------------------------
    let grid = null;

    if (variant === 'month' && month) {
        const [year, m] = month.split('-').map(Number);
        const daysInMonth = new Date(year, m, 0).getDate();
        const firstWeekday = new Date(year, m - 1, 1).getDay(); // 0=Sun..6=Sat
        const leadingBlanks = (firstWeekday + 6) % 7; // Monday-first

        const cells = [];
        for (let i = 0; i < leadingBlanks; i++) {
            cells.push(<Box key={`blank-${i}`} sx={{ minHeight: CELL_MIN_HEIGHT, backgroundColor: 'background.default' }} />);
        }
        for (let day = 1; day <= daysInMonth; day++) {
            const dateStr = `${month}-${String(day).padStart(2, '0')}`;
            cells.push(
                <DayCell
                    key={day}
                    label={day}
                    reward={rewardsByDay.get(day)}
                    cornerIcon={loginDateSet.has(dateStr) ? cornerCheck(theme.palette.primary.main) : null}
                    sx={{ backgroundColor: 'background.paper' }}
                />
            );
        }
        // Fill the final week so empty grid tracks don't show as solid lines.
        const trailingBlanks = (7 - ((leadingBlanks + daysInMonth) % 7)) % 7;
        for (let i = 0; i < trailingBlanks; i++) {
            cells.push(<Box key={`tblank-${i}`} sx={{ minHeight: CELL_MIN_HEIGHT, backgroundColor: 'background.default' }} />);
        }

        grid = (
            <>
                <Box sx={{ display: 'grid', gridTemplateColumns: 'repeat(7, 1fr)', mb: 0.5 }}>
                    {WEEKDAYS.map((w) => (
                        <Typography key={w} variant="caption" sx={{ textAlign: 'center', color: 'text.secondary', fontWeight: 'bold' }}>
                            {w}
                        </Typography>
                    ))}
                </Box>
                <Box
                    sx={{
                        display: 'grid',
                        gridTemplateColumns: 'repeat(7, 1fr)',
                        gap: '1px',
                        backgroundColor: 'divider',
                        border: '1px solid',
                        borderColor: 'divider',
                        borderRadius: 1,
                        overflow: 'hidden',
                    }}
                >
                    {cells}
                </Box>
            </>
        );
    } else if (variant === 'tier') {
        const days = [...rewardsByDay.keys()].sort((a, b) => a - b);
        grid = (
            <Box sx={{ display: 'grid', gridTemplateColumns: 'repeat(7, 1fr)', gap: 0.75 }}>
                {days.map((day) => {
                    const h = tierHighlight(day);
                    const borderColor = h === 'claimed'
                        ? theme.palette.success.main
                        : h === 'unlocked'
                            ? theme.palette.warning.main
                            : theme.palette.divider;
                    const tint = h === 'claimed'
                        ? alpha(theme.palette.success.main, 0.12)
                        : h === 'unlocked'
                            ? alpha(theme.palette.warning.main, 0.12)
                            : 'transparent';
                    return (
                        <DayCell
                            key={day}
                            label={day}
                            reward={rewardsByDay.get(day)}
                            dim={h === 'locked'}
                            cornerIcon={h === 'claimed' ? cornerCheck(theme.palette.success.main) : null}
                            sx={{
                                border: `2px solid ${borderColor}`,
                                borderRadius: 1,
                                backgroundColor: tint,
                            }}
                        />
                    );
                })}
            </Box>
        );
    }

    return (
        <Box sx={{ width: '100%' }}>
            {/* Month navigator */}
            <Box sx={{ display: 'flex', alignItems: 'center', justifyContent: 'center', gap: 1, mb: 1 }}>
                <IconButton size="small" aria-label="Previous month" disabled={!canPrev} onClick={() => canPrev && onMonthChange?.(addMonths(month, -1))}>
                    <ChevronLeftRoundedIcon fontSize="small" />
                </IconButton>
                <Typography variant="subtitle2" sx={{ minWidth: 130, textAlign: 'center', fontWeight: 'bold' }}>
                    {month ? monthLabel(month) : '—'}
                </Typography>
                <IconButton size="small" aria-label="Next month" disabled={!canNext} onClick={() => canNext && onMonthChange?.(addMonths(month, 1))}>
                    <ChevronRightRoundedIcon fontSize="small" />
                </IconButton>
            </Box>

            {/* Legend (tier variant only) */}
            {variant === 'tier' && rewards.length > 0 && (
                <Box sx={{ display: 'flex', flexWrap: 'wrap', gap: 2, justifyContent: 'center', alignItems: 'center', mb: 1 }}>
                    <Box sx={{ display: 'inline-flex', alignItems: 'center', gap: 0.75 }}>
                        <Box sx={{ width: 12, height: 12, borderRadius: '50%', border: `2px solid ${theme.palette.warning.main}`, flexShrink: 0 }} />
                        <Typography variant="caption" color="text.secondary" sx={{ lineHeight: 1 }}>Unclaimed</Typography>
                    </Box>
                    <Box sx={{ display: 'inline-flex', alignItems: 'center', gap: 0.75 }}>
                        <Box sx={{ width: 12, height: 12, borderRadius: '50%', border: `2px solid ${theme.palette.success.main}`, flexShrink: 0 }} />
                        <Typography variant="caption" color="text.secondary" sx={{ lineHeight: 1 }}>Claimed</Typography>
                    </Box>
                </Box>
            )}

            {/* Widget-supplied note (logins this month / last updated / no account data) */}
            {subHeader && (
                <Box sx={{ display: 'flex', flexDirection: 'column', alignItems: 'center', gap: 0.25, mb: 1 }}>
                    {subHeader}
                </Box>
            )}

            {/* Content */}
            {isLoading ? (
                <Box sx={{ display: 'grid', gridTemplateColumns: 'repeat(7, 1fr)', gap: 0.75 }}>
                    {Array.from({ length: 14 }).map((_, i) => (
                        <Skeleton key={i} variant="rounded" height={CELL_MIN_HEIGHT} />
                    ))}
                </Box>
            ) : rewards.length === 0 ? (
                emptyState ?? (
                    <Box sx={{ py: 4, textAlign: 'center' }}>
                        <Typography variant="body2" color="text.secondary">
                            No reward calendar stored for this month yet.
                        </Typography>
                    </Box>
                )
            ) : (
                grid
            )}
        </Box>
    );
}

export default LoginRewardsCalendar;
