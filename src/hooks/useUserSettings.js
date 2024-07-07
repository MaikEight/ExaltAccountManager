import { useContext } from 'react';
import UserSettingsContext from './../contexts/UserSettingsContext';

function useUserSettings() {
    return useContext(UserSettingsContext);
}

export default useUserSettings;