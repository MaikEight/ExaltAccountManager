using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MK_EAM_Lib
{
    [System.Serializable]
    public class EAMNotificationMessage
    {
        private const string url = "https://raw.githubusercontent.com/MaikEight/ExaltAccountManager/master/NotificationMessages/EAMNotificationMessage.{0}";

        public int id = -1;
        public EAMNotificationMessageType type;

        public string message = string.Empty;
        public string link = string.Empty;
        public string linkM = string.Empty;
        public bool forceShow = false;

        public static EAMNotificationMessage GetEAMNotificationMessage(string version)
        {
            EAMNotificationMessage toRet = new EAMNotificationMessage()
            {
                type = EAMNotificationMessageType.None
            };

            try
            {
                string str = string.Empty;
                System.Net.WebRequest request = System.Net.WebRequest.Create(string.Format(url, version));
                request.Credentials = System.Net.CredentialCache.DefaultCredentials;
                request.CachePolicy = new System.Net.Cache.HttpRequestCachePolicy(System.Net.Cache.HttpRequestCacheLevel.BypassCache);
                System.Net.WebResponse response = request.GetResponse();
                using (System.IO.Stream dataStream = response.GetResponseStream())
                {
                    using (System.IO.StreamReader reader = new System.IO.StreamReader(dataStream))
                    {
                        JsonSerializer serializer = new JsonSerializer();
                        toRet = (EAMNotificationMessage)serializer.Deserialize(reader, typeof(EAMNotificationMessage));
                    }
                }
                response.Close();
            }
            catch
            {
                toRet = new EAMNotificationMessage()
                {
                    type = EAMNotificationMessageType.None
                };
            }
            return toRet;
        }
    }

    public enum EAMNotificationMessageType
    {
        None,
        UpdateAvailable,
        Message,
        Warning,
        Stop
    }

    [System.Serializable]
    public class EAMNotificationMessageSaveFile
    {
        public List<int> knownIDs = new List<int>();
        public DateTime lastCheck = new DateTime(2000, 1, 1);
        public bool forceCheck = false;
        public bool lastCheckWasStop = false;
    }
}
