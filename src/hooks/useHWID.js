import { useState, useEffect } from "react";
import { HWID_FILE_PATH } from "../constants";
import { readFileUTF8 } from "../utils/readFileUtil";
import { tauri } from "@tauri-apps/api";

function useHWID() {
    const [hwid, setHwid] = useState(null);

    useEffect(() => {
        const readHwidFile = async () => {
            const path = await HWID_FILE_PATH();
            const hwid = await readFileUTF8(path, false);
            if (hwid === null || hwid === undefined || hwid === '') {
                tauri.invoke('get_device_unique_identifier')
                    .then((id) => setHwid(id));
                return;
            }
            setHwid(hwid);
        }
        readHwidFile().catch(error => console.error(error));
    }, []);

    return hwid;
}

export default useHWID;