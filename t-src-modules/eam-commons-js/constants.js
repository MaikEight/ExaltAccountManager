export const ROTMG_BASE_URL = 'https://www.realmofthemadgod.com';

const updateBaseUrls = [
    "https://www.realmofthemadgod.com/app/init?platform=standalonewindows64&key=9KnJFxtTvLu2frXv",
    "{0}/rotmg-exalt-win-64/checksum.json", //TODO: add Mac and Linux support
    "build-release/{0}/rotmg-exalt-win-64/{1}.gz" //TODO: add Mac and Linux support
];

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
export const AUTH0_DOMAIN = 'login.exaltaccountmanager.com';
export const AUTH0_REDIRECT_URL = 'eam:profile/callback';

export const EAM_USERS_API = 'https://user.api.exaltaccountmanager.com';