import { Box } from "@mui/material";
import WidgetBase from "./WidgetBase";
import useWidgets from "../../../hooks/useWidgets";
import useLoginRewardsCalendar from "../../../hooks/useLoginRewardsCalendar";
import LoginRewardsCalendar from "../../DailyLoginRewards/LoginRewardsCalendar";

function DailyLoginRewardsWidget({ type, widgetId }) {
    const { widgetBarState } = useWidgets();
    const account = widgetBarState?.data;
    const email = account?.email ?? null;

    const { month, setMonth, availableMonths, rewards, accountStatus, isLoading } =
        useLoginRewardsCalendar({ email });

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
                />
            </Box>
        </WidgetBase>
    );
}

export default DailyLoginRewardsWidget;
