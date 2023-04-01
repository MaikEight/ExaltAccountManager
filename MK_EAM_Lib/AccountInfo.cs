using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace MK_EAM_Lib
{
    [System.Serializable]
    public sealed class AccountInfo
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

        private async void _PerformWebrequest(Action<LogData> LogEvent, string logSender, string accountStatsPath, string itemsSaveFilePath, string uniqueID, bool getName = true, Action<AccountInfo> callback = null)
        {
            try
            {
                string webPath = string.Format("https://www.realmofthemadgod.com/account/verify?guid={0}&password={1}&clientToken={2}&game_net=Unity&play_platform=Unity&game_net_user_id=", WebUtility.UrlEncode(email), WebUtility.UrlEncode(password), uniqueID);
                string responseData = string.Empty;

                Dictionary<string, string> values = new Dictionary<string, string>
                {
                    { "guid", email },
                    { "password", password },
                    { "clientToken", uniqueID },
                    { "game_net", "Unity" },
                    { "play_platform", "Unity" },
                    { "game_net_user_id", "" }
                };
                FormUrlEncodedContent content = new FormUrlEncodedContent(values);
                content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");

                Task<HttpResponseMessage> response = SendPostRequest("https://www.realmofthemadgod.com/account/verify", content, "form");
                HttpResponseMessage responseMessage = await response;
                responseMessage.EnsureSuccessStatusCode();

                responseData = await responseMessage.Content.ReadAsStringAsync();

                if (responseData.Contains("<Error>"))
                {
                    //Error out

                    if (responseData.Contains("passwordError"))//<Error>WebChangePasswordDialog.passwordError</Error>                    
                        requestState = RequestState.WrongPassword;
                    else if (responseData.ToLower().Contains("wait"))//<Error>Internal error, please wait 5 minutes to try again!</Error>                    
                        requestState = RequestState.TooManyRequests;
                    else if (responseData.Contains("CaptchaLock"))//<Error>CaptchaLock</Error>
                        requestState = RequestState.Captcha;
                    else
                        requestState = RequestState.Error;

                    if (LogEvent != null)
                        LogEvent(new LogData(-1, logSender, LogEventType.WebRequest, $"Failed to get data for {email}."));

                    sender = null;

                    callback?.Invoke(null);

                    return;
                }

                requestState = RequestState.Success;

                System.Tuple<string, string, string> tup = GetClientAccessData(LogEvent, logSender, responseData);
                accessToken = new MK_EAM_Lib.AccessToken(tup.Item1, tup.Item2, tup.Item3, uniqueID);

                if (getName && responseData.Contains("<Account>") && responseData.Contains("<Name>") )
                    name = responseData.Substring(responseData.IndexOf("<Name>") + 6, responseData.IndexOf("</Name>") - 6 - responseData.IndexOf("<Name>"));
  
                try
                {
                    if (LogEvent != null)
                        LogEvent(new LogData(-1, logSender, LogEventType.WebRequest, $"Sending \"char/list\" for {email}."));

                    response = null;
                    values = null;
                    content = null;
                    responseMessage = null;

                    values = new Dictionary<string, string>
                    {
                        { "do_login", "false" },
                        { "accessToken", accessToken.token },
                        { "game_net", "Unity" },
                        { "play_platform", "Unity" },
                        { "game_net_user_id", "" },
                        { "muleDump", "true" },
                        { "__source", "jakcodex-v965" }
                    };
                    content = new FormUrlEncodedContent(values);
                    content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");

                    response = SendPostRequest("https://www.realmofthemadgod.com/char/list", content, "form");
                    responseMessage = await response;
                    responseMessage.EnsureSuccessStatusCode();

                    string charList = await responseMessage.Content.ReadAsStringAsync();

                    SaveAccountStats(LogEvent, logSender, accountStatsPath, itemsSaveFilePath, responseData, charList, callback);
                }
                catch
                {
                    if (LogEvent != null)
                        LogEvent(new LogData(-1, logSender, LogEventType.EAMError, $"Failed to get stats for {email}."));
                }
            }
            catch (System.Exception e)
            {
                requestState = RequestState.Error;

                if (LogEvent != null)
                    LogEvent(new LogData(-1, logSender, LogEventType.EAMError, $"Failed to refresh data. Exception: " + e.Message));
            }

            sender = null;

            callback?.Invoke(null);
        }

        public static async Task<HttpResponseMessage> SendPostRequest(string url, object data, string contentType)
        {
            using (var client = new HttpClient())
            {
                if (contentType.Equals("json"))
                {
                    string json = JsonConvert.SerializeObject(data);
                    var content = new System.Net.Http.StringContent(json, Encoding.UTF8, "application/json");
                    return await client.PostAsync(url, content);
                }

                if (contentType.Equals("form"))
                {
                    return await client.PostAsync(url, (FormUrlEncodedContent)data);
                }

                return null;
            }
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

                    callback?.Invoke(null);

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
                string pathWithExposedMail = Path.Combine(accountStatsPath, fileName);
                string pathObfucated = Path.Combine(accountStatsPath, GetAccountStatsFilenameObfuscated(fileName));

                if (File.Exists(pathObfucated))
                {
                    try
                    {
                        main = (StatsMain)ByteArrayToObject(File.ReadAllBytes(pathObfucated));
                    }
                    catch { main.email = email; }
                }
                else
                {
                    if (File.Exists(pathWithExposedMail))
                    {
                        try
                        {
                            main = (StatsMain)ByteArrayToObject(File.ReadAllBytes(pathWithExposedMail));
                            File.WriteAllBytes(pathObfucated, ObjectToByteArray(main));
                            File.Delete(pathWithExposedMail);
                        }
                        catch { main.email = email; }
                    }
                    else
                    {
                        main.email = email;
                    }
                }

                main.stats.Add(stats);
                main.charList.Add(s);

                File.WriteAllBytes(pathWithExposedMail, ObjectToByteArray(main));
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
            None = 4,
            Captcha = 5,
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

        public static string GetAccountStatsFilenameObfuscated(string filename)
        {
            try
            {
                return Convert.ToBase64String(Encoding.UTF8.GetBytes(filename)).Replace('/', '-');
            }
            catch { }
            return string.Empty;
        }

        public static string DeobfuscateAccountStatsFilename(string filename)
        {
            try
            {
                return Encoding.UTF8.GetString(Convert.FromBase64String(filename.Replace('-', '/')));
            }
            catch { }
            return string.Empty;
        }
    }
}
