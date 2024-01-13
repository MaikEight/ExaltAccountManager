import { createContext, useEffect, useState } from "react";
import { SERVER_LIST_FILE_PATH } from "../constants";
import { readFileUTF8 } from "../utils/readFileUtil";
import { writeFileUTF8 } from "../utils/writeFileUtil";

const ServerContext = createContext();

function ServerContextProvider({ children }) {
    const [serverList, setServerList] = useState([]);

    const getServers = async () => {
        SERVER_LIST_FILE_PATH()
            .then((path) => {
                readFileUTF8(path, true)
                    .then((data) => {
                        setServerList(data);
                        return data;
                    });
            })
            .catch((err) => {
                console.error(err);
                setServerList([]);
            });
    };

    useEffect(() => {
        getServers();
    }, []);

    const saveServerList = (serverList) => {
        setServerList(serverList);
        SERVER_LIST_FILE_PATH().then((path) => {
            writeFileUTF8(path, JSON.stringify(serverList));
        });
    };

    return (
        <ServerContext.Provider value={{ serverList, saveServerList }}>
            {children}
        </ServerContext.Provider>
    );
}

export default ServerContext;
export { ServerContextProvider };