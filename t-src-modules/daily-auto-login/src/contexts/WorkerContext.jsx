import { createContext, useEffect, useState } from "react";
import useTasks from "../hooks/useTasks";
import { checkForUpdates, updateGame, useHWID, useUserLogin, xmlToJson, storeCharList, useGroups, GroupUI } from 'eam-commons-js';
import { invoke } from "@tauri-apps/api/core";

const WorkerContext = createContext();

const STEPS = [
    'IDLE',
    'UPDATE_GAME',
    'LOGINS',
]
function WorkerContextProvider({ children }) {
    const { updateTask, taskTypes } = useTasks();
    const { idToken } = useUserLogin();
    const hwid = useHWID();
    const { groups } = useGroups();

    const [currentStep, setCurrentStep] = useState(0);
    const [accountsToPerformDailyLoginFor, setAccountsToPerformDailyLoginFor] = useState([]);
    const [accountsDone, setAccountsDone] = useState([]);
    const [dailyLoginsReport, setDailyLoginsReport] = useState([]);
    const [isPlusUser, setIsPlusUser] = useState(false);
    const [performingLogins, setPerformingLogins] = useState(false);

    const getGroup = (name) => {
        if(!name) {
            return null;
        }
        const eamGroup = groups.find(g => g.name === name);
        return eamGroup;
    };

    useEffect(() => {
        if (!idToken) {
            setIsPlusUser(false);
            return;
        }
        const result = invoke('is_plus_user', { idToken: idToken }).catch(console.error);
        setIsPlusUser(Boolean(result));
    }, [idToken]);

    useEffect(() => {
        const performStep = async () => {
            switch (STEPS[currentStep]) {
                case 'IDLE':
                    break;
                case 'UPDATE_GAME':
                    await performGameUpdate();
                    break;
                case 'LOGINS':
                    await performLogins();
                    break;
                default:
                    break;
            }
        };

        performStep();
    }, [currentStep]);

    useEffect(() => {
        setCurrentStep(1);

        const fetchAccountsToPerformDailyLoginFor = async (report) => {
            if (!report || !report.emailsToProcess) {
                return;
            }
            const emails = report.emailsToProcess.split(',').map(email => email.trim());

            const accounts = (
                await Promise.all(emails.map(async (email) => {
                    return await invoke('get_eam_account_by_email', { accountEmail: email })
                        .catch(console.error);
                }))
            ).filter(Boolean);

            setAccountsToPerformDailyLoginFor(accounts);
        };

        const getDailyLoginsReport = async () => {
            const report = await invoke('get_daily_login_report')
                .catch(console.error);
            setDailyLoginsReport(report);
            console.log('report', report);
            return report;
        }

        const fetchData = async () => {
            const report = await getDailyLoginsReport();
            const _ = await fetchAccountsToPerformDailyLoginFor(report);
        };

        fetchData();
    }, []);

    const startNextStep = () => {
        if (currentStep === STEPS.length) {
            return; // No more steps, we are done.
        }

        setCurrentStep((prevStep) => prevStep + 1);
    };

    const performGameUpdate = async () => {
        // Perform game update if needed
        updateTask({
            type: 'Update check',
            startTime: new Date(),
        });
        const updateNeeded = await checkForUpdates(true);
        console.log('updateNeeded', updateNeeded);
        if (updateNeeded) {
            updateTask({
                type: 'Update game',
                startTime: new Date(),
            });
            await updateGame();
        }
        console.log('Update done');

        startNextStep();
    };

    const performLogins = async () => {
        if (performingLogins || accountsToPerformDailyLoginFor.length === 0) {
            return;
        }
        setPerformingLogins(true);

        console.log('Performing logins...');
        let accounts = accountsToPerformDailyLoginFor;
        let isPlus = isPlusUser;
        while (accounts.length > 0) {
            accounts = accountsToPerformDailyLoginFor;
            if (accounts.length === 0) {
                break;
            }

            const account = accounts.shift();
            console.log('Processing account:', account.email);
            const result = await performLogin(account, isPlus);
            isPlus = Boolean(result?.usedPlus);

            if (isPlus) {
                updateTask({
                    type: 'Login',
                    heroImage: <GroupUI group={getGroup(account?.group)} />,
                    headline: `Login: ${account.name ?? account.email}`,
                    subheadline: 'Waiting for API cooldown...',
                    startTime: new Date(),
                    endTime: new Date(Date.now() + 60_000),
                });
                await new Promise((resolve) => setTimeout(resolve, 60_000));
            }

            setAccountsToPerformDailyLoginFor((prev) => {
                const updatedAccounts = prev.filter((acc) => acc.email !== account.email);
                console.log('Updated accounts to perform daily login for:', updatedAccounts);
                return updatedAccounts;
            });
            setAccountsDone((prev) => {
                const updatedAccountsDone = [...prev, account];
                console.log('Updated accounts done:', updatedAccountsDone);
                return updatedAccountsDone;
            });
        }

        setPerformingLogins(false);
    };

    const performLogin = async (account, isPlus) => {
        console.log('Performing login for account:', account.email);
        updateTask({
            type: 'Login',
            heroImage: <GroupUI group={getGroup(account?.group)} />,
            headline: `Login: ${account.name ?? account.email}`,
            subheadline: 'Fetching character list...',
            startTime: new Date(),
        });
        await new Promise((resolve) => setTimeout(resolve, 5_000));
        return { usedPlus: true }; //HERE FOR TESTING

        // Webrequest account/verify
        // Webrequest char/list
        // Store char/list data in database
        // Start game
        // Wait 90 seconds
        // Terminate game
        // Return, so the next login can start

        const result = await invoke('perform_daily_login', { idToken: idToken, email: account.email, hwid: hwid })
            .catch(console.error);
        console.log('result', result);
        if (result?.charListxml) {
            // Store charListxml in database
            const charList = xmlToJson(result.charListXml);
            await storeCharList(charList, account.email);
        }
        if (result && !result.usedPlus) {
            updateTask({
                type: 'Login',
                headline: `Login: ${account.email}`,
                subheadline: 'Starting the game...',
                startTime: new Date(),
                endTime: new Date(Date.now() + 90_000),
            });
            await invoke('perform_daily_login_game_run', { email: account.email, accessToken: result.accessToken })
        }
        return result;
    };

    const contextValue = {
        currentStep: currentStep,
        steps: STEPS,
        startNextStep: startNextStep,

        accountsToPerformDailyLoginFor: accountsToPerformDailyLoginFor,
        accountsDone: accountsDone,
        report: dailyLoginsReport,
    };

    return (
        <WorkerContext.Provider value={contextValue}>
            {children}
        </WorkerContext.Provider>
    );
}

export default WorkerContext;
export { WorkerContextProvider };