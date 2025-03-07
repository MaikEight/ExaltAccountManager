import useUserSettings from "./useUserSettings";

function useApplySettingsToHeaderName() {
    const settings = useUserSettings();

    const applySettingsToHeaderName = (headerName) => {
        const hideEmojis = Boolean(settings.getByKeyAndSubKey('accounts', 'hideEmojis'));

        if (hideEmojis
            && headerName
            && headerName.includes(' ')) {
            return headerName.substring(headerName.indexOf(' ') + 1);
        }

        return headerName;
    };

    return { 
        applySettingsToHeaderName, 
        hideEmojis: Boolean(settings.getByKeyAndSubKey('accounts', 'hideEmojis')) 
    };
}

export default useApplySettingsToHeaderName;