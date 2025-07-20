import { useState, useEffect } from "react";
import { HWID_FILE_PATH } from "../constants";
import { readFileUTF8 } from "eam-commons-js";
import { invoke } from "@tauri-apps/api/core";

function useHWID() {
    const [hwid, setHwid] = useState(null);

    useEffect(() => {
        const readHwidFile = async () => {
            const path = await HWID_FILE_PATH();
            const _hwid = await readFileUTF8(path, false);
            if (_hwid === null || _hwid === undefined || _hwid === '') {
                const id = await invoke('get_device_unique_identifier');
                setHwid(id);
                
                return;
            }
            setHwid(_hwid);
        };

        readHwidFile()
            .catch(error => console.error(error));
    }, []);

    return hwid;
}

export default useHWID;