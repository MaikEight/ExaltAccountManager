import { createContext, useEffect, useState } from "react";
import usePopups from "../hooks/usePopups";
import NewsComponent from "../components/News/NewsComponent";
import { getLatestPopup } from "../backend/newsApi";

const NewsContext = createContext();

function NewsContextProvider({ children }) {
    const [newsItems, setNewsItems] = useState([]);
    const { showPopup, startupPopupResult } = usePopups();

    const fetchLatestPopup = async () => {        

        const debugFlag = sessionStorage.getItem('flag:debug') === 'true';

        let lastSeenPopupData = localStorage.getItem('eam-news-lastSeenPopupData');
        if (lastSeenPopupData) {
            lastSeenPopupData = JSON.parse(lastSeenPopupData);
            if (debugFlag) {
                console.log("Last seen popup data found:", lastSeenPopupData);
            }

            const isValidUUID = (id) => {
                const uuidRegex = /^[0-9a-f]{8}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{12}$/i;
                return uuidRegex.test(id);
            };

            if (!isValidUUID(lastSeenPopupData?.id)) {
                if (debugFlag) {
                    console.warn("Invalid lastSeenPopup UUID detected:", lastSeenPopupData?.id);
                }

                lastSeenPopupData = null;
            }

            if (lastSeenPopupData?.date) {
                lastSeenPopupData.date = new Date(lastSeenPopupData.date);
                //Check if the date is within the last 12h
                const twelveHoursAgo = new Date(Date.now() - 12 * 60 * 60 * 1000);
                if (lastSeenPopupData.date > twelveHoursAgo) {
                    if (debugFlag) {
                        console.log("Last seen popup date is within the last 12 hours:", lastSeenPopupData.date);
                    }

                    return;
                }
            }
        }

        const popup = await getLatestPopup(lastSeenPopupData?.id || null);
        if (debugFlag) {
            console.log("Fetched popup data:", popup);
        }

        if (popup && !popup.error) {
            const popupData = { id: popup.id || lastSeenPopupData?.id, date: new Date() };
            localStorage.setItem('eam-news-lastSeenPopupData', JSON.stringify(popupData));

            showPopup({
                content: <NewsComponent news={popup} />
            });
        }
    };

    useEffect(() => {
        if (startupPopupResult.done
            && !startupPopupResult.hadPopup) {
            fetchLatestPopup();
        }
    }, [startupPopupResult]);

    const contextValue = {
        newsItems,
        fetchLatestNews: fetchLatestPopup,
    };

    return (
        <NewsContext.Provider value={contextValue}>
            {children}
        </NewsContext.Provider>
    );
}

export default NewsContext;
export { NewsContextProvider };
