using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Net;

namespace MK_EAM_Lib
{
    [System.Serializable]
    public class AccountInfo
    {
        [Browsable(false)]
        public int orderID = -1;

        [Browsable(false)]
        public string name = string.Empty;

        [Browsable(false)]
        public string email = string.Empty;

        [Browsable(false)]
        public string password = string.Empty;

        [Browsable(false)]
        public string serverName = string.Empty;

        [DisplayName("Group")]
        public System.Drawing.Bitmap Group
        {
            get
            {
                System.Drawing.Bitmap bmp = new System.Drawing.Bitmap(15, 15);

                using (System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(bmp))
                using (System.Drawing.SolidBrush sb = new System.Drawing.SolidBrush(color))
                {
                    g.Clear(System.Drawing.Color.Transparent);
                    if (color == System.Drawing.Color.FromArgb(50, 128, 128, 128))
                        return bmp;
                    g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                    g.FillEllipse(sb, new System.Drawing.RectangleF(0, 0, bmp.Width, bmp.Height));
                }

                return bmp;
            }
        }

        [DisplayName("Accountname")]
        public string Name { get => name; }

        [DisplayName("Email")]
        public string Email { get => email; }

        [Browsable(false)]
        public Color Color
        {
            get => color;
            set => color = value;
        }

        [Browsable(false)]
        private System.Drawing.Color color = System.Drawing.Color.FromArgb(50, 128, 128, 128);

        [Browsable(false)]
        public bool performSave;

        [Browsable(false)]
        public AccessToken accessToken;

        [Browsable(false)]
        public bool requestSuccessfull = true;
        
        [Browsable(false)]
        public RequestState requestState { get; set; } = RequestState.None;

        [Browsable(false)]
        [NonSerialized]
        private dynamic sender;

        public AccountInfo() { }
        public AccountInfo(MuledumpAccounts muledump)
        {
            name = muledump.mail;
            email = muledump.mail;
            password = muledump.password;
            performSave = false;
        }

        public AccountInfo(ExportCSVAccount exp)
        {
            name = exp.username;
            password = exp.password;
            email = exp.email;
            color = System.Drawing.ColorTranslator.FromHtml(exp.color);
        }

        public void PerformWebrequest(dynamic _sender, Action<LogData> LogEvent, string logSender, string accountStatsPath, string itemsSaveFilePath, string uniqueID, bool getName = true, bool doAsyncRequest = true, Action<AccountInfo> callback = null)
        {
            sender = _sender;
            if (doAsyncRequest)
            {
                System.Threading.ThreadPool.QueueUserWorkItem(_ => _PerformWebrequest(LogEvent, logSender, accountStatsPath, itemsSaveFilePath, uniqueID, getName, callback));
            }
            else
            {
                _PerformWebrequest(LogEvent, logSender, accountStatsPath, itemsSaveFilePath, uniqueID, getName, callback);
            }
        }

        private void _PerformWebrequest(Action<LogData> LogEvent, string logSender, string accountStatsPath, string itemsSaveFilePath, string uniqueID, bool getName = true, Action<AccountInfo> callback = null)
        {
            try
            {
                string webPath = string.Format("https://www.realmofthemadgod.com/account/verify?guid={0}&password={1}&clientToken={2}&game_net=Unity&play_platform=Unity&game_net_user_id=", WebUtility.UrlEncode(email), WebUtility.UrlEncode(password), uniqueID);
                string responseData = string.Empty;

                WebRequest request = WebRequest.Create(webPath);
                request.Credentials = CredentialCache.DefaultCredentials;
                WebResponse response = request.GetResponse();
                using (System.IO.Stream dataStream = response.GetResponseStream())
                using (System.IO.StreamReader reader = new System.IO.StreamReader(dataStream))
                    responseData = reader.ReadToEnd();
                response.Close();

                if (responseData.Contains("<Error>"))
                {
                    //Error out
                    //requestSuccessfull = false;

                    if (responseData.Contains("passwordError"))//<Error>WebChangePasswordDialog.passwordError</Error>                    
                        requestState = RequestState.WrongPassword;
                    else if (responseData.ToLower().Contains("wait"))//<Error>Internal error, please wait 5 minutes to try again!</Error>                    
                        requestState = RequestState.TooManyRequests;
                    else
                        requestState = RequestState.Error;

                    if (LogEvent != null)
                        LogEvent(new LogData(-1, logSender, LogEventType.WebRequest, $"Failed to get data for {email}."));

                    sender = null;

                    if (callback != null)
                        callback(this);

                    return;
                }
                else
                    requestState = RequestState.Success;

                if (getName && responseData.Contains("<Account>"))
                    name = responseData.Substring(responseData.IndexOf("<Name>") + 6, responseData.IndexOf("</Name>") - 6 - responseData.IndexOf("<Name>"));

                System.Tuple<string, string, string> tup = GetClientAccessData(LogEvent, logSender, responseData);

                accessToken = new MK_EAM_Lib.AccessToken(tup.Item1, tup.Item2, tup.Item3, uniqueID);
                try
                {
                    if (LogEvent != null)
                        LogEvent(new LogData(-1, logSender, LogEventType.WebRequest, $"Sending \"char/list\" for {email}."));
                    string link = $"https://www.realmofthemadgod.com/char/list?do_login=false&accessToken={WebUtility.UrlEncode(accessToken.token)}&game_net=Unity&play_platform=Unity&game_net_user_id=&muleDump=true&__source=jakcodex-v965";
                    request = WebRequest.Create(link);
                    request.Credentials = CredentialCache.DefaultCredentials;
                    response = request.GetResponse();
                    string charList = "";
                    using (System.IO.Stream dataStream = response.GetResponseStream())
                    using (System.IO.StreamReader reader = new System.IO.StreamReader(dataStream))
                        charList = reader.ReadToEnd();
                    response.Close();

                    SaveAccountStats(LogEvent, logSender, accountStatsPath, itemsSaveFilePath, responseData, charList, callback);
                }
                catch
                {
                    if (LogEvent != null)
                        LogEvent(new LogData(-1, logSender, LogEventType.EAMError, $"Failed to get stats for {email}."));
                }
            }
            catch (System.Exception)
            {
                requestState = RequestState.Error;
            }

            sender = null;

            if (callback != null)
                callback(this);
        }

        public Tuple<string, string, string> GetClientAccessData(Action<LogData> LogEvent, string logSender, string resp)
        {
            try
            {
                string token = "";
                string timestamp = "";
                string expiration = "";

                if (resp.Contains("<AccessToken>"))
                {
                    token = resp.Substring(resp.IndexOf("<AccessToken>") + 13, resp.Length - (resp.IndexOf("<AccessToken>") + 13));
                    token = token.Substring(0, token.IndexOf("</AccessToken>"));
                }
                if (resp.Contains("<AccessTokenTimestamp>"))
                {
                    timestamp = resp.Substring(resp.IndexOf("<AccessTokenTimestamp>") + 22, resp.Length - (resp.IndexOf("<AccessTokenTimestamp>") + 22));
                    timestamp = timestamp.Substring(0, timestamp.IndexOf("</AccessTokenTimestamp>"));
                }
                if (resp.Contains("<AccessTokenExpiration>"))
                {
                    expiration = resp.Substring(resp.IndexOf("<AccessTokenExpiration>") + 23, resp.Length - (resp.IndexOf("<AccessTokenExpiration>") + 23));
                    expiration = expiration.Substring(0, expiration.IndexOf("</AccessTokenExpiration>"));
                }

                return new Tuple<string, string, string>(token, timestamp, expiration);
            }
            catch
            {
                if (LogEvent != null)
                    LogEvent(new LogData(-1, logSender, LogEventType.EAMError, $"Failed to parse ClientAccessData!"));
            }

            return new Tuple<string, string, string>(string.Empty, string.Empty, string.Empty);
        }

        private void SaveAccountStats(Action<LogData> LogEvent, string logSender, string accountStatsPath, string itemsSaveFilePath, string request, string charList, Action<AccountInfo> callback = null)
        {
            try
            {
                if (!Directory.Exists(accountStatsPath))
                    Directory.CreateDirectory(accountStatsPath);

                string fileName = GetAccountStatsFilename();

                if (string.IsNullOrEmpty(fileName))
                {
                    if (LogEvent != null)
                        LogEvent(new LogData(-1, logSender, LogEventType.EAMError, $"Failed to save stats for {email}."));

                    sender = null;

                    if (callback != null)
                        callback(this);

                    return;
                }

                try
                {
                    AccountItems accountItems = new AccountItems(name, email, charList);
                    bool found = false;

                    if (File.Exists(itemsSaveFilePath))
                    {
                        ItemsSaveFile isf = ByteArrayToObject(File.ReadAllBytes(itemsSaveFilePath)) as ItemsSaveFile;

                        for (int i = 0; i < isf.accountItems.Count; i++)
                        {
                            if (isf.accountItems[i].email.Equals(accountItems.email))
                            {
                                found = true;
                                isf.accountItems[i] = accountItems;
                                break;
                            }
                        }
                        if (!found)
                            isf.accountItems.Add(accountItems);

                        File.WriteAllBytes(itemsSaveFilePath, ObjectToByteArray(isf));
                    }
                    else
                    {
                        ItemsSaveFile isf = new ItemsSaveFile();

                        isf.accountItems.Add(accountItems);

                        File.WriteAllBytes(itemsSaveFilePath, ObjectToByteArray(isf));
                    }
                }
                catch
                {
                    if (LogEvent != null)
                        LogEvent(new LogData(-1, logSender, LogEventType.EAMError, $"Failed to save AccountItems for {email}."));
                }

                AccountStats stats = new AccountStats(email, request);
                CharListStats s = GetCharacterStatsFromRequest(LogEvent, logSender, charList);

                StatsMain main = new StatsMain();
                string path = Path.Combine(accountStatsPath, fileName);
                if (File.Exists(path))
                {
                    try
                    {
                        main = (StatsMain)ByteArrayToObject(File.ReadAllBytes(path));
                    }
                    catch { main.email = email; }
                }
                else
                {
                    main.email = email;
                }

                main.stats.Add(stats);
                main.charList.Add(s);

                File.WriteAllBytes(path, ObjectToByteArray(main));
            }
            catch
            {
                if (LogEvent != null)
                    LogEvent(new LogData(-1, logSender, LogEventType.EAMError, $"Failed to save stats for {email}."));
            }
        }

        private CharListStats GetCharacterStatsFromRequest(Action<LogData> LogEvent, string logSender, string charList)
        {
            CharListStats s = new CharListStats();
            if (sender != null)
            {
                try
                {
                    sender.serverData = ServerDataCollection.CreateNewCollection(charList);
                }
                catch (Exception)
                {
                    if (LogEvent != null)
                        LogEvent(new LogData(-1, logSender, LogEventType.EAMError, $"Failed to parse / save servers."));
                }
            }

            try
            {
                string[] chars = System.Text.RegularExpressions.Regex.Split(charList, ("</Char>"));
                if (chars.Length > 1)
                    chars[0] = chars[0].Substring(chars[0].IndexOf("<Char id=\""), chars[0].Length - chars[0].IndexOf("<Char id=\""));

                for (int i = 0; i < chars.Length - 1; i++)
                {
                    CharacterStats st = new CharacterStats(chars[i]);
                    s.chars.Add(st);
                }
            }
            catch
            {
                if (LogEvent != null)
                    LogEvent(new LogData(-1, logSender, LogEventType.EAMError, $"Failed to parse CharList."));
            }
            return s;
        }

        private object ByteArrayToObject(byte[] arrBytes)
        {
            using (MemoryStream memStream = new MemoryStream())
            {
                System.Runtime.Serialization.Formatters.Binary.BinaryFormatter binForm = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                memStream.Write(arrBytes, 0, arrBytes.Length);
                memStream.Seek(0, SeekOrigin.Begin);
                object obj = (object)binForm.Deserialize(memStream);

                return obj;
            }
        }

        private byte[] ObjectToByteArray(object obj)
        {
            if (obj == null)
                return null;

            System.Runtime.Serialization.Formatters.Binary.BinaryFormatter bf = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
            using (MemoryStream ms = new MemoryStream())
            {
                bf.Serialize(ms, obj);
                return ms.ToArray();
            }
        }

        private string GetAccountStatsFilename()
        {
            try
            {
                string fileName = email;
                foreach (char c in System.IO.Path.GetInvalidFileNameChars())
                    fileName = fileName.Replace(c.ToString(), "");
                fileName = fileName.Replace(".", "");
                fileName = $"{fileName}.EAMstats";

                return fileName;
            }
            catch { }
            return string.Empty;
        }

        public enum RequestState
        {
            Success = 0,
            WrongPassword = 1,
            TooManyRequests = 2,
            Error = 3,
            None
        }
        public static string RequestStateToString(RequestState state)
        {
            switch (state)
            {
                case RequestState.Success:
                    return "Success";
                case RequestState.WrongPassword:
                    return "Wrong Password";
                case RequestState.TooManyRequests:
                    return "Too Many Requests";
                case RequestState.Error:
                    return "Error";
                case RequestState.None:
                    return "None";
                default:
                    return "Error";
            }
        }
    }
}
