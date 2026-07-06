import { useEffect, useMemo, useState } from "react";
import { Box, IconButton, Skeleton, Tooltip, Typography, alpha } from "@mui/material";
import { useTheme } from "@emotion/react";
import ChevronLeftRoundedIcon from '@mui/icons-material/ChevronLeftRounded';
import ChevronRightRoundedIcon from '@mui/icons-material/ChevronRightRounded';
import CheckCircleRoundedIcon from '@mui/icons-material/CheckCircleRounded';
import TaskAltRoundedIcon from '@mui/icons-material/TaskAltRounded';
import MonetizationOnRoundedIcon from '@mui/icons-material/MonetizationOnRounded';
import items from "../../assets/constants";
import { drawItemAsync } from "../../utils/realmItemDrawUtils";
import { TooltipUiForItem } from "../Widgets/Widgets/Components/InventoryRender";

const ITEM_SIZE = 40;
const ITEM_PADDING = 2;
const SPRITE_SIZE = ITEM_SIZE + 2 * ITEM_PADDING; // 44
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
 * A single reward day cell: draws the item sprite (cached) with an optional
 * quantity badge, a highlight state, and a corner marker/overlay.
 */
function DayCell({ label, reward, highlight = 'none', marker = false }) {
    const theme = useTheme();
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
        drawItemAsync("renders.png", itemData, 0, ITEM_PADDING)
            .then((src) => { if (!cancelled) setImg(src); })
            .catch(() => { if (!cancelled) setImg(null); });
        return () => { cancelled = true; };
    }, [itemId]);

    // Highlight -> border colour / emphasis.
    const borderColor = highlight === 'claimed'
        ? theme.palette.success.main
        : highlight === 'unlocked'
            ? theme.palette.warning.main
            : theme.palette.divider;
    const isLocked = highlight === 'locked';

    const spriteBox = (
        <Box
            sx={{
                position: 'relative',
                width: SPRITE_SIZE,
                height: SPRITE_SIZE,
                borderRadius: 1,
                border: `2px solid ${borderColor}`,
                boxShadow: highlight === 'unlocked' ? `0 0 6px ${alpha(theme.palette.warning.main, 0.6)}` : 'none',
                backgroundColor: alpha(theme.palette.action.hover, 0.4),
                opacity: isLocked ? 0.5 : 1,
                display: 'flex',
                alignItems: 'center',
                justifyContent: 'center',
                overflow: 'hidden',
            }}
        >
            {img ? (
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
                <Typography variant="caption" sx={{ color: 'text.disabled', fontSize: '0.6rem' }}>
                    #{itemId}
                </Typography>
            ) : null}

            {/* Quantity badge (bottom-left, so it never collides with the marker) */}
            {quantity > 1 && (
                <Typography
                    variant="caption"
                    sx={{
                        position: 'absolute',
                        bottom: 1,
                        left: 1,
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

            {/* Claimed overlay check (bottom-right) */}
            {highlight === 'claimed' && (
                <CheckCircleRoundedIcon
                    sx={{
                        position: 'absolute',
                        bottom: -2,
                        right: -2,
                        fontSize: 16,
                        color: 'success.main',
                        backgroundColor: 'background.paper',
                        borderRadius: '50%',
                    }}
                />
            )}

            {/* Daily-login-done marker (bottom-right) for the page/month view */}
            {marker && highlight !== 'claimed' && (
                <TaskAltRoundedIcon
                    sx={{
                        position: 'absolute',
                        bottom: -2,
                        right: -2,
                        fontSize: 16,
                        color: 'primary.main',
                        backgroundColor: 'background.paper',
                        borderRadius: '50%',
                    }}
                />
            )}
        </Box>
    );

    const content = (
        <Box sx={{ display: 'flex', flexDirection: 'column', alignItems: 'center', gap: 0.25 }}>
            <Typography variant="caption" sx={{ color: 'text.secondary', lineHeight: 1, fontSize: '0.65rem' }}>
                {label}
            </Typography>
            {spriteBox}
        </Box>
    );

    return itemData
        ? <Tooltip title={<TooltipUiForItem item={itemData} />} arrow>{content}</Tooltip>
        : content;
}

/**
 * Renders the daily-login reward calendar in one of two layouts:
 *  - variant="month": weekday-aligned month grid; the cell for date N shows reward
 *    tier N and gets a corner marker for dates in `loginDates`.
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

    // Month-navigator bounds.
    const sortedMonths = useMemo(() => [...availableMonths].sort(), [availableMonths]);
    const canPrev = !!month && sortedMonths.length > 0 && month > sortedMonths[0];
    const canNext = !!month && sortedMonths.length > 0 && month < sortedMonths[sortedMonths.length - 1];

    const highlightFor = (day) => {
        if (variant !== 'tier') return 'none';
        if (claimedSet.has(day)) return 'claimed';
        if (day <= unlockableDays) return 'unlocked';
        return 'locked';
    };

    // Build the cells for the current layout.
    let gridCells = null;

    if (variant === 'month' && month) {
        const [year, m] = month.split('-').map(Number);
        const daysInMonth = new Date(year, m, 0).getDate();
        const firstWeekday = new Date(year, m - 1, 1).getDay(); // 0=Sun..6=Sat
        const leadingBlanks = (firstWeekday + 6) % 7; // Monday-first

        const cells = [];
        for (let i = 0; i < leadingBlanks; i++) {
            cells.push(<Box key={`blank-${i}`} />);
        }
        for (let day = 1; day <= daysInMonth; day++) {
            const dateStr = `${month}-${String(day).padStart(2, '0')}`;
            cells.push(
                <DayCell
                    key={day}
                    label={day}
                    reward={rewardsByDay.get(day)}
                    marker={loginDateSet.has(dateStr)}
                />
            );
        }
        gridCells = (
            <>
                {WEEKDAYS.map((w) => (
                    <Typography
                        key={w}
                        variant="caption"
                        sx={{ textAlign: 'center', color: 'text.secondary', fontWeight: 'bold', pb: 0.5 }}
                    >
                        {w}
                    </Typography>
                ))}
                {cells}
            </>
        );
    } else if (variant === 'tier') {
        const days = [...rewardsByDay.keys()].sort((a, b) => a - b);
        gridCells = days.map((day) => (
            <DayCell
                key={day}
                label={day}
                reward={rewardsByDay.get(day)}
                highlight={highlightFor(day)}
            />
        ));
    }

    return (
        <Box sx={{ width: '100%' }}>
            {/* Month navigator */}
            <Box sx={{ display: 'flex', alignItems: 'center', justifyContent: 'center', gap: 1, mb: 1 }}>
                <IconButton
                    size="small"
                    aria-label="Previous month"
                    disabled={!canPrev}
                    onClick={() => canPrev && onMonthChange?.(addMonths(month, -1))}
                >
                    <ChevronLeftRoundedIcon fontSize="small" />
                </IconButton>
                <Typography variant="subtitle2" sx={{ minWidth: 130, textAlign: 'center', fontWeight: 'bold' }}>
                    {month ? monthLabel(month) : '—'}
                </Typography>
                <IconButton
                    size="small"
                    aria-label="Next month"
                    disabled={!canNext}
                    onClick={() => canNext && onMonthChange?.(addMonths(month, 1))}
                >
                    <ChevronRightRoundedIcon fontSize="small" />
                </IconButton>
            </Box>

            {/* Legend (tier variant only) */}
            {variant === 'tier' && (
                <Box sx={{ display: 'flex', gap: 2, justifyContent: 'center', mb: 1, flexWrap: 'wrap' }}>
                    <Box sx={{ display: 'flex', alignItems: 'center', gap: 0.5 }}>
                        <Box sx={{ width: 12, height: 12, borderRadius: 0.5, border: `2px solid ${theme.palette.warning.main}` }} />
                        <Typography variant="caption" color="text.secondary">Unclaimed</Typography>
                    </Box>
                    <Box sx={{ display: 'flex', alignItems: 'center', gap: 0.5 }}>
                        <Box sx={{ width: 12, height: 12, borderRadius: 0.5, border: `2px solid ${theme.palette.success.main}` }} />
                        <Typography variant="caption" color="text.secondary">Claimed</Typography>
                    </Box>
                    {typeof unlockableDays === 'number' && (
                        <Typography variant="caption" color="text.secondary">
                            {unlockableDays} login{unlockableDays === 1 ? '' : 's'} this month
                        </Typography>
                    )}
                </Box>
            )}

            {/* Content */}
            {isLoading ? (
                <Box sx={{ display: 'grid', gridTemplateColumns: 'repeat(7, 1fr)', gap: 1, justifyItems: 'center' }}>
                    {Array.from({ length: 14 }).map((_, i) => (
                        <Skeleton key={i} variant="rounded" width={SPRITE_SIZE} height={SPRITE_SIZE + 14} />
                    ))}
                </Box>
            ) : rewards.length === 0 ? (
                <Box sx={{ py: 4, textAlign: 'center' }}>
                    <Typography variant="body2" color="text.secondary">
                        No reward calendar stored for this month yet.
                    </Typography>
                    <Typography variant="caption" color="text.secondary">
                        It is fetched automatically on the next login, refresh or daily login.
                    </Typography>
                </Box>
            ) : (
                <Box
                    sx={{
                        display: 'grid',
                        gridTemplateColumns: 'repeat(7, 1fr)',
                        gap: 1,
                        justifyItems: 'center',
                    }}
                >
                    {gridCells}
                </Box>
            )}
        </Box>
    );
}

export default LoginRewardsCalendar;
