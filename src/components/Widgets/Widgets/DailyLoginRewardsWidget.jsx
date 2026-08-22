import { Box, Typography } from "@mui/material";
import { formatDistanceToNow } from "date-fns";
import WidgetBase from "./WidgetBase";
import useWidgets from "../../../hooks/useWidgets";
import useAccounts from "../../../hooks/useAccounts";
import useLoginRewardsCalendar from "../../../hooks/useLoginRewardsCalendar";
import LoginRewardsCalendar from "../../DailyLoginRewards/LoginRewardsCalendar";

function DailyLoginRewardsWidget({ type, widgetId }) {
    const { widgetBarState } = useWidgets();
    const { getAccountByEmail } = useAccounts();
    const email = widgetBarState?.data?.email ?? null;

    // Track the live account so a refresh (which updates lastRefresh) re-fetches
    // the calendar without needing to reopen the widget bar.
    const liveAccount = email ? getAccountByEmail(email) : null;
    const rawRefresh = liveAccount?.lastRefresh ?? null;
    const reloadKey = rawRefresh instanceof Date ? rawRefresh.getTime() : rawRefresh;

    const { month, setMonth, availableMonths, rewards, accountStatus, isLoading } =
        useLoginRewardsCalendar({ email, reloadKey });

    const hasData = rewards.length > 0;

    // Relative "last updated" for this account/month, if we have a stored row.
    let lastUpdatedText = null;
    if (accountStatus?.updated_at) {
        const d = new Date(accountStatus.updated_at);
        if (!isNaN(d.getTime())) {
            lastUpdatedText = formatDistanceToNow(d, { addSuffix: true });
        }
    }

    const subHeader = hasData ? (
        <>
            {accountStatus ? (
                <Typography variant="caption" color="text.secondary">
                    {accountStatus.unlockable_days} login{accountStatus.unlockable_days === 1 ? '' : 's'} this month
                </Typography>
            ) : (
                <Typography variant="caption" sx={{ color: 'warning.main', textAlign: 'center' }}>
                    No progress data for this account yet — refresh this account.
                </Typography>
            )}
            {lastUpdatedText && (
                <Typography variant="caption" color="text.secondary">
                    Last updated {lastUpdatedText}
                </Typography>
            )}
        </>
    ) : null;

    const emptyState = (
        <Box sx={{ py: 3, textAlign: 'center' }}>
            <Typography variant="body2" color="text.secondary">No reward data yet.</Typography>
            <Typography variant="caption" color="text.secondary">
                Refresh this account to load the current month's rewards.
            </Typography>
        </Box>
    );

    return (
        <WidgetBase type={type} widgetId={widgetId}>
            <Box sx={{ width: '100%', p: 1 }}>
                <LoginRewardsCalendar
                    variant="tier"
                    month={month}
                    availableMonths={availableMonths}
                    onMonthChange={setMonth}
                    rewards={rewards}
                    accountStatus={accountStatus}
                    isLoading={isLoading}
                    subHeader={subHeader}
                    emptyState={emptyState}
                />
            </Box>
        </WidgetBase>
    );
}

export default DailyLoginRewardsWidget;
