import { useContext } from 'react';
import { UserSettingsContext } from 'eam-commons-js';

function useUserSettings() {
    return useContext(UserSettingsContext);
}

export default useUserSettings;