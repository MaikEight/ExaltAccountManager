import { getLatestEamVersion } from "../backend/eamApi";
import { isUpdateAvailable } from "../constants";

async function onStartUp() {
    getLatestEamVersion()
        .then((version) => {
            if (isUpdateAvailable(version)) {
                console.log("Update available!");
                localStorage.setItem("EAMUpdateAvailable", true);
            }
            else if (localStorage.getItem("EAMUpdateAvailable")) {
                localStorage.removeItem("EAMUpdateAvailable");
            }
        });
}

export { onStartUp };