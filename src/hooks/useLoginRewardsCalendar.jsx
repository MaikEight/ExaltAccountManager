import { useCallback, useEffect, useRef, useState } from "react";
import { invoke } from "@tauri-apps/api/core";

/** Local "YYYY-MM" for the current month (used as the initial selection). */
function currentMonthKey() {
    const d = new Date();
    return `${d.getFullYear()}-${String(d.getMonth() + 1).padStart(2, '0')}`;
}

/**
 * Loads daily-login reward calendar data from the backend and manages the
 * selected month. Starts on the current month and, on first load, jumps to the
 * latest month that actually has stored data (unless the user navigated first).
 *
 * @param {Object}  options
 * @param {string=} options.email          When set, also loads the per-account claim/unlock status.
 * @param {boolean} options.withLoginDates  When true, also loads the dates a daily-login run succeeded.
 * @param {*}       options.reloadKey       Changing this value re-fetches everything (e.g. after an account refresh).
 */
export default function useLoginRewardsCalendar({ email = null, withLoginDates = false, reloadKey = null } = {}) {
    const [availableMonths, setAvailableMonths] = useState([]);
    const [month, setMonth] = useState(currentMonthKey);
    const [rewards, setRewards] = useState([]);
    const [accountStatus, setAccountStatus] = useState(null);
    const [loginDates, setLoginDates] = useState([]);
    const [isLoading, setIsLoading] = useState(true);
    const userNavigatedRef = useRef(false);

    // Manual month selection (via the navigator) pins the month.
    const selectMonth = useCallback((m) => {
        userNavigatedRef.current = true;
        setMonth(m);
    }, []);

    // Load the list of stored months once; jump to the latest that has data.
    useEffect(() => {
        let cancelled = false;
        invoke('get_available_login_reward_months')
            .then((months) => {
                if (cancelled) return;
                const list = Array.isArray(months) ? months : [];
                setAvailableMonths(list);
                if (!userNavigatedRef.current && list.length) {
                    setMonth(list[list.length - 1]);
                }
            })
            .catch(() => { /* keep the current-month default */ });
        return () => { cancelled = true; };
    }, [reloadKey]);

    // Load calendar + optional per-account / login-date data for the selected month.
    // `month` is never null, so this always runs and always clears isLoading.
    useEffect(() => {
        let cancelled = false;
        setIsLoading(true);

        const tasks = [
            invoke('get_login_rewards_calendar_for_month', { month })
                .then((rows) => { if (!cancelled) setRewards(Array.isArray(rows) ? rows : []); })
                .catch(() => { if (!cancelled) setRewards([]); }),
        ];

        if (email) {
            tasks.push(
                invoke('get_account_login_rewards', { email, month })
                    .then((row) => { if (!cancelled) setAccountStatus(row ?? null); })
                    .catch(() => { if (!cancelled) setAccountStatus(null); })
            );
        }

        if (withLoginDates) {
            tasks.push(
                invoke('get_daily_login_success_dates_for_month', { month })
                    .then((dates) => { if (!cancelled) setLoginDates(Array.isArray(dates) ? dates : []); })
                    .catch(() => { if (!cancelled) setLoginDates([]); })
            );
        }

        Promise.all(tasks).finally(() => { if (!cancelled) setIsLoading(false); });
        return () => { cancelled = true; };
    }, [month, email, withLoginDates, reloadKey]);

    return { month, setMonth: selectMonth, availableMonths, rewards, accountStatus, loginDates, isLoading };
}
