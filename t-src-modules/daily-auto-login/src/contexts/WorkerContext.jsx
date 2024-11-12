import { createContext, useEffect, useState } from "react";
import useTasks from "../hooks/useTasks";
import { checkForUpdates, updateGame } from 'eam-commons-js';

const WorkerContext = createContext();

const STEPS = [
    'IDLE',
    'UPDATE_GAME',
    'LOGINS',
]
function WorkerContextProvider({ children }) {
    const { updateTask, taskTypes } = useTasks();

    const [currentStep, setCurrentStep] = useState(0);
    const [accountsToPerformDailyLoginFor, setAccountsToPerformDailyLoginFor] = useState([]);
    const [accountsDone, setAccountsDone] = useState([]);
    
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
    }, []);

    const startNextStep = () => {
        if (currentStep === STEPS.length ) {
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
        // Get all accounts that need to be logged in
        // Loop through all accounts
        // Perform login for each account

        

    };

    const performLogin = async (account) => {        
        // Webrequest account/verify
        // Webrequest char/list
        // Store char/list data in database
        // Start game
        // Wait 90 seconds
        // Terminate game
        // Return, so the next login can start
    };

    const contextValue = {
        currentStep: currentStep,
        steps: STEPS,
        startNextStep: startNextStep,

        accountsToPerformDailyLoginFor: accountsToPerformDailyLoginFor,
        accountsDone: accountsDone,
    };

    return (
        <WorkerContext.Provider value={contextValue}>
            {children}
        </WorkerContext.Provider>
    );
}

export default WorkerContext;
export { WorkerContextProvider };