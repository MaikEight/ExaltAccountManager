import { invoke } from '@tauri-apps/api/core';

export const APP_VERSION = '4.2.9';
export const APP_VERSION_RELEASE_DATE = '31.10.2025';
export const IS_PRE_RELEASE = false;

export const CACHE_PREFIX = '!cache-';

export function isUpdateAvailable(latestVersion) {
    if (!latestVersion) return false;

    const current = APP_VERSION.split('.');
    const latest = latestVersion.split('.');

    for (let i = 0; i < current.length; i++) {
        if (parseInt(current[i], 10) < parseInt(latest[i], 10)) return true;
        if (parseInt(current[i], 10) > parseInt(latest[i], 10)) return false;
    }

    return false;
}

export const SAVE_FILE_PATH = async () => await invoke('get_save_file_path');

export const HWID_FILE_PATH = async () => await invoke('combine_paths', { path1: await SAVE_FILE_PATH(), path2: HWID_FILE_NAME });
export const SERVER_LIST_FILE_PATH = async () => await invoke('combine_paths', { path1: await SAVE_FILE_PATH(), path2: SERVER_LIST_FILE_NAME });

export const HWID_FILE_NAME = 'EAM.HWID';
export const SERVER_LIST_FILE_NAME = 'serverList.json';

export const EAM_BASE_URL = 'https://api.exalt-account-manager.eu/';
export const EAM_NEWS_BASE_URL = 'https://news.api.exaltaccountmanager.com/';

export const ROTMG_BASE_URL = 'https://www.realmofthemadgod.com';

const updateBaseUrls = Object.freeze([
    "https://www.realmofthemadgod.com/app/init?platform=standalonewindows64&key=9KnJFxtTvLu2frXv",
    "{0}/rotmg-exalt-win-64/checksum.json", //TODO: add Mac and Linux support
    "build-release/{0}/rotmg-exalt-win-64/{1}.gz" //TODO: add Mac and Linux support
]);

export function UPDATE_URLS(index, values) {
    switch (index) {
        case 0:
            return updateBaseUrls[index];
        case 1: {
            if (!values) return updateBaseUrls[index];
            const v = updateBaseUrls[index].replace("{0}", values);
            return v;
        }
        case 2:
            if (values.length < 2) return updateBaseUrls[index];
            return updateBaseUrls[index].replace("{0}", values[0]).replace("{1}", values[1]);
        default:
            return null;
    }
}

export const AUTH0_CLIENT_ID = 'o1W1coVQMV9qrIg4G2SmZJbz1G5vRCpZ';
export const AUTH0_DOMAIN = 'https://login.exaltaccountmanager.com';
//export const STRIPE_CUSTOMER_PORTAL_URL = 'https://billing.stripe.com/p/login/test_dR63erdeEeUo5nGcMM'; //DEV MODE
export const STRIPE_CUSTOMER_PORTAL_URL = 'https://billing.stripe.com/p/login/3csaEM7fH0M3eVqeUU';

export const DISCORD_APPLICATION_ID = '1069308775854526506';


// OKTA
export const MASCOT_NAME = 'Okta';

export const DAILY_LOGIN_COMPLETED_MESSAGES = Object.freeze([
    `Mission complete! ${MASCOT_NAME} handled your daily login like a pro.`,
    `All done! ${MASCOT_NAME} just finished your daily login adventure.`,
    `${MASCOT_NAME} checked in for you — smooth, fast, and flawless.`,
    `${MASCOT_NAME} has completed the daily login. Time to reap the rewards!`,
    `Daily login? Handled. ${MASCOT_NAME} was faster than a Realm boss wipe.`,
    `${MASCOT_NAME} did the daily grind so you don't have to. All done!`,
    `The login ritual is complete. ${MASCOT_NAME} bows dramatically.`,
    `${MASCOT_NAME} snuck in, logged you in, and snuck back out. Mission accomplished.`,

    `${MASCOT_NAME} has successfully completed the daily login. Smooth as always.`,
    `Daily login done! ${MASCOT_NAME} handled it behind the scenes.`,
    `${MASCOT_NAME} logged in quietly and efficiently. Mission complete.`,
    `All set! ${MASCOT_NAME} performed the daily login like a true professional.`,
    `${MASCOT_NAME} checked in for the day. Everything's ready.`,
    `The daily login is finished. ${MASCOT_NAME} didn't even trip over any wires!`,
    `${MASCOT_NAME} just wrapped up the daily login routine.`,
    `Done and dusted. ${MASCOT_NAME} took care of the login for you.`,
    `${MASCOT_NAME} handled the daily login while you were doing more important things.`,
    `Routine login complete. ${MASCOT_NAME} was very sneaky about it.`,
]);

export const NO_LOGS_FOUND_MESSAGES = (currentLogMode) => [
    `${MASCOT_NAME} flipped through every page of the ${currentLogMode}... still nothing.`,
    `The ${currentLogMode} is emptier than ${MASCOT_NAME}'s snack drawer.`,
    `${MASCOT_NAME} triple-checked the ${currentLogMode} — not even a suspicious comma.`,
    `Even with a magnifying glass, ${MASCOT_NAME} couldn't find anything in the ${currentLogMode}.`,
    `No entries. ${MASCOT_NAME} is starting to question the existence of this ${currentLogMode}.`,
    `${MASCOT_NAME} searched the ${currentLogMode} and only found echoes.`,
    `The ${currentLogMode} is so quiet, ${MASCOT_NAME} took a nap waiting for results.`,
    `All clear! Not a single thing in the ${currentLogMode} to report.`,
    `Either the ${currentLogMode} is spotless, or ${MASCOT_NAME} missed something (unlikely).`,
    `${MASCOT_NAME} stared at the ${currentLogMode} for a long time. Nothing stared back.`,
    `If this ${currentLogMode} were a book, it'd be blank. ${MASCOT_NAME} checked.`,
    `The ${currentLogMode} is suspiciously clean. ${MASCOT_NAME} is impressed… maybe too impressed.`,
    `After an exhaustive scan, ${MASCOT_NAME} reports: zero findings in the ${currentLogMode}.`,
    `${MASCOT_NAME} even asked the logs nicely. Still, ${currentLogMode} gave nothing.`,
    `${MASCOT_NAME} brought popcorn for drama in the ${currentLogMode}. Sadly, no show today.`,
    `No chaos found in the ${currentLogMode}. ${MASCOT_NAME} is both relieved and bored.`,
    `${MASCOT_NAME}'s detective hat is on, but the ${currentLogMode} is playing hard to get.`,
    `The ${currentLogMode} is as empty as ${MASCOT_NAME}'s inbox on weekends.`,
    `Not even a typo in sight. ${currentLogMode} is squeaky clean.`,
    `${MASCOT_NAME} checked. Rechecked. Even did a backflip. Still no ${currentLogMode} entries.`
];

export const NO_ACCOUNTS_FOUND_TEXTS = Object.freeze([
    `No accounts here... maybe ${MASCOT_NAME} took a wrong turn.`,
    `The search party led by ${MASCOT_NAME} came back empty-handed.`,
    `Well, this is awkward. ${MASCOT_NAME} found absolutely nothing.`,
    `Even ${MASCOT_NAME}'s keen eyes couldn't spot a single account.`,
    `Accounts? What accounts? ${MASCOT_NAME} sees only tumbleweeds.`,
    `After intense investigation, ${MASCOT_NAME} concluded: no accounts.`,
    `${MASCOT_NAME} swears there were accounts here a minute ago.`,
    `The trail has gone cold. ${MASCOT_NAME} has no leads.`,
    `If there were accounts here, ${MASCOT_NAME} would've found them. Promise.`,
    `Not a single account in sight. ${MASCOT_NAME} suspects foul play.`,
    `Despite heroic effort, ${MASCOT_NAME} has nothing to show for it.`,
    `${MASCOT_NAME} even checked under the digital couch cushions. Nada.`,
    `It's like the accounts vanished into thin air.`,
    `Some say the accounts are shy. ${MASCOT_NAME} says they're hiding.`,
    `${MASCOT_NAME} opened every drawer — not even a username left behind.`,
    `${MASCOT_NAME} would've found them… if they existed.`,
    `The database is silent. Too silent. ${MASCOT_NAME}'s suspicious.`,
    `Nothing to see here. ${MASCOT_NAME} recommends snacks and a retry.`,
    `${MASCOT_NAME}'s account search is complete. Result: a majestic void.`,
    `${MASCOT_NAME} searched high and low, but found no accounts`,
    `${MASCOT_NAME} has lost its way`,
]);

export const EAM_PLUS_PURCHASE_SUCCESS_MESSAGES = Object.freeze([
    `${MASCOT_NAME} is doing a happy dance — welcome to EAM Plus! 🎉`,
    `You did it! ${MASCOT_NAME} just leveled up thanks to your EAM Plus support.`,
    `${MASCOT_NAME}'s circuits are buzzing with joy — EAM Plus activated!`,
    `${MASCOT_NAME} is now 87% more powerful. (Thanks, EAM Plus!)`,
    `Your support means the world. ${MASCOT_NAME} sends high-fives and virtual cookies! 🍪`,
    `EAM Plus unlocked! ${MASCOT_NAME} just upgraded its imaginary cape.`,
    `You've joined the cool club. ${MASCOT_NAME} approves.`,
    `${MASCOT_NAME} is honored to have you as a Plus member. Expect only mild mischief.`,
    `Plus mode: engaged. ${MASCOT_NAME} feels fancy now.✨`,
    `Thank you for supporting EAM! ${MASCOT_NAME} may cry digital tears of joy.`,
]);