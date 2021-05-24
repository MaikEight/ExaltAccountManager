using ExaltAccountManager;
using MK_EAM_Lib;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Management;
using System.Net;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace EAM_Daily_Login_Service
{
    class DailyLogin
    {
        public static string pathCurl = Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).FullName, "curl", "curl.exe");
        public static string accountsPath = Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).FullName, "EAM.accounts");
        public static string dailyLoginsPath = Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).FullName, "EAM.DailyLogins");
        public static string pathLogs = Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).FullName, "EAM.Logs");
        public static string pathDailyLoginsConfig = Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).FullName, "EAM.DailyLogins");
        public static string pathNotificationConfig = Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).FullName, "EAM.NotificationOptions");
        public static string pathEAMTasktrayToolEXE = Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).FullName, "EAM Tasktray Tool", "EAM Tasktray Tool.exe");
        public static string optionsPath = Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).FullName, "EAM.options");
        public static string userLoginStatsPath = Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).FullName, "EAM.LoginStats");
        public static string accountStatsPath = Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).FullName, "Stats");

        public static string exePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), @"RealmOfTheMadGod\Production\RotMG Exalt.exe");
        public static NotificationOptions notOptions;
        public static List<AccountInfo> accounts = new List<AccountInfo>();
        public static DailyLogins logins;

        public static System.Timers.Timer timer = new System.Timers.Timer();

        private static Dictionary<System.Timers.Timer, Process> dicTimerToProcess = new Dictionary<System.Timers.Timer, Process>();
        private static bool failedToKillProcess = false;
        private static int doneCloseTime = 0;
        private static bool debugMode = true;

        static void Main(string[] args)
        {
            try
            {
                Log("Start...");
                LogEvent(new LogData(0, "EAM Service", LogEventType.ServiceStart, $"Starting daily-login."));
                handler = new ConsoleEventDelegate(ConsoleEventCallback);
                SetConsoleCtrlHandler(handler, true);

                var handle = GetConsoleWindow();

                // Hide
                ShowWindow(handle, SW_HIDE);

                LoadAccounts();
                LoadLogins();
                LoadNotificationConfig();
                LoadEAMOptions();

                if ((DateTime.Now.TimeOfDay < notOptions.execTime.TimeOfDay && logins.lastUpdate != new DateTime()) || (logins.lastUpdate != new DateTime() && logins.lastUpdate.Date == DateTime.Now.Date))
                {
                    Log("Closing...");
                    LogEvent(new LogData(0, "EAM Service", LogEventType.ServiceDone, $"Stopping daily-login, got nothing to do."));
                    ConsoleEventCallback(2);
                    Environment.Exit(0);
                    return;
                }

                logins.isDone = false;
                try
                {
                    File.WriteAllBytes(pathDailyLoginsConfig, ObjectToByteArray(logins));
                }
                catch { }

                if (notOptions.useTaskTrayTool)
                {
                    try
                    {
                        ProcessStartInfo pi = new ProcessStartInfo() { FileName = pathEAMTasktrayToolEXE };
                        Process p = new Process() { StartInfo = pi };
                        p.Start();
                    }
                    catch { }
                }

                if (notOptions == null)
                    notOptions = new NotificationOptions();

                timer.Interval = notOptions.joinTime * 1000;
                timer.Elapsed += Timer_Tick;
                timer.Start();
                Timer_Tick(timer, null);

                Console.ReadKey();
            }
            catch (Exception ex)
            {
                Log(ex.Message);
            }
        }

        public static void LogEvent(LogData data)
        {
            if (File.Exists(pathLogs))
            {
                try
                {
                    List<LogData> logs = new List<LogData>();
                    logs = (List<LogData>)ByteArrayToObject(File.ReadAllBytes(pathLogs));
                    data.ID = logs.Count - 1;
                    logs.Add(data);
                    File.WriteAllBytes(pathLogs, ObjectToByteArray(logs));
                }
                catch { }
            }
            else
            {
                try
                {
                    List<LogData> logs = new List<LogData>();
                    data.ID = 0;
                    logs.Add(data);
                    File.WriteAllBytes(pathLogs, ObjectToByteArray(logs));
                }
                catch { }
            }
        }

        private static void Log(string msg)
        {
            if (!debugMode) return;

            try
            {
                File.AppendAllText(@".\Log.txt", $"{msg}{Environment.NewLine}");
            }
            catch { }
        }



        private static void TimerKill_Tick(object sender, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                System.Timers.Timer t = sender as System.Timers.Timer;
                if (dicTimerToProcess.ContainsKey(t))
                {
                    dicTimerToProcess[t].Kill();
                    t.Stop();
                    dicTimerToProcess.Remove(t);
                }
            }
            catch (Exception ex)
            {
                failedToKillProcess = true;
                Log(ex.Message);
            }
        }

        private static void Timer_Tick(object sender, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                bool isDone = true;
                for (int i = 0; i < logins.logins.Count; i++)
                {
                    if (logins.logins[i].lastLogin == null || logins.logins[i].lastLogin.Date != DateTime.Now.Date)
                    {
                        isDone = false;
                        try
                        {
                            AccountInfo accountInfo = null;
                            for (int x = 0; x < accounts.Count; x++)
                            {
                                if (accounts[x].email.Equals(logins.logins[i].mail))
                                {
                                    accountInfo = accounts[x];

                                    accounts[x] = accountInfo = GetAccountData(accountInfo, false);
                                    SaveAccounts();                                    
                                    break;
                                }
                            }
                            LogEvent(new LogData(0, "EAM Service", LogEventType.Login, $"Start login into account: {accountInfo.email}."));

                            string arguments = string.Format("\"data:{{platform:Deca,guid:{0},token:{1},tokenTimestamp:{2},tokenExpiration:{3},env:4}}\"", StringToBase64String(accountInfo.email), StringToBase64String(accountInfo.accessToken.token), StringToBase64String(accountInfo.accessToken.creationTime), StringToBase64String(accountInfo.accessToken.expirationTime));
                            ProcessStartInfo info = new ProcessStartInfo();
                            info.Arguments = string.Format("-batchmode {0}", arguments);
                            info.FileName = exePath;
                            info.WindowStyle = ProcessWindowStyle.Hidden;
                            info.CreateNoWindow = true;

                            Process p = new Process();
                            p.StartInfo = info;
                            p.Start();

                            SaveLoginStats(accountInfo);

                            System.Timers.Timer timerP = new System.Timers.Timer();
                            timerP.Interval = (notOptions.joinTime + notOptions.killTime) * 1000;
                            timerP.Elapsed += TimerKill_Tick;
                            timerP.Start();

                            dicTimerToProcess.Add(timerP, p);
                        }
                        catch (Exception ex)
                        {
                            logins.logins[i].lastLogin = DateTime.Now.Date;
                            logins.logins[i].lastState = 2;
                            Log(ex.Message);
                            LogEvent(new LogData(0, "EAM Service", LogEventType.ServiceError, $"{ex.Message}"));
                        }

                        logins.logins[i].lastLogin = DateTime.Now.Date;
                        logins.logins[i].lastState = 1;
                        break;
                    }
                }
                if (isDone)
                {
                    if (dicTimerToProcess.Count() > 0 && doneCloseTime <= 3)
                    {
                        doneCloseTime++;
                        return;
                    }
                    ConsoleEventCallback(2);
                    logins.isDone = true;
                    File.WriteAllBytes(pathDailyLoginsConfig, ObjectToByteArray(logins));

                    Environment.Exit(0);
                }
                else
                {
                    logins.lastUpdate = DateTime.Now;
                    File.WriteAllBytes(pathDailyLoginsConfig, ObjectToByteArray(logins));
                }
            }
            catch (Exception ex)
            {
                LogEvent(new LogData(0, "EAM Service", LogEventType.ServiceError, $"{ex.Message}"));
                Log(ex.Message);
            }
        }

        private static void SaveLoginStats(AccountInfo info)
        {
            try
            {
                string fileName = Path.Combine(accountStatsPath, GetAccountStatsFilename(info.email));

                if (!Directory.Exists(accountStatsPath))
                    Directory.CreateDirectory(accountStatsPath);

                if (File.Exists(fileName))
                {
                    try
                    {
                        StatsMain stats = (StatsMain)ByteArrayToObject(File.ReadAllBytes(fileName));
                        if (stats.logins == null)
                            stats.logins = new List<LoginStats>();
                        stats.logins.Add(new LoginStats() { time = DateTime.Now, isFromTask = true });
                        File.WriteAllBytes(fileName, ObjectToByteArray(stats));
                    }
                    catch (Exception)
                    {
                        LogEvent(new LogData(0, "EAM Service", LogEventType.ServiceError, $"Failed to load / save login-stats."));
                    }
                }
                else
                {
                    StatsMain stats = new StatsMain() { email = info.email };
                    stats.logins = new List<LoginStats>() { new LoginStats() { time = DateTime.Now, isFromTask = true } };
                    File.WriteAllBytes(fileName, ObjectToByteArray(stats));
                }
            }
            catch
            {
                LogEvent(new LogData(0, "EAM Service", LogEventType.ServiceError, $"Failed to save login-stats."));
            }
        }

        private static void SaveAccountStats(AccountInfo info, string request, string charList)
        {
            try
            {
                if (!Directory.Exists(accountStatsPath))
                    Directory.CreateDirectory(accountStatsPath);

                string fileName = GetAccountStatsFilename(info.email);

                if (string.IsNullOrEmpty(fileName))
                {
                    LogEvent(new LogData(0, "EAM Service", LogEventType.ServiceError, $"Failed to save stats for {info.email}."));
                    return;
                }

                AccountStats stats = new AccountStats(info.email, request);
                CharListStats s = GetCharacterStatsFromRequest(charList);

                StatsMain main = new StatsMain();
                string path = Path.Combine(accountStatsPath, fileName);
                if (File.Exists(path))
                {
                    try
                    {
                        main = (StatsMain)ByteArrayToObject(File.ReadAllBytes(path));
                    }
                    catch { main.email = info.email; }
                }
                else
                {
                    main.email = info.email;
                }

                main.stats.Add(stats);
                main.charList.Add(s);

                File.WriteAllBytes(path, ObjectToByteArray(main));
            }
            catch
            {
                LogEvent(new LogData(0, "EAM Service", LogEventType.ServiceError, $"Failed to save stats for {info.email}."));
            }
        }

        private static CharListStats GetCharacterStatsFromRequest(string charList)
        {
            CharListStats s = new CharListStats();
            try
            {
                string[] chars = Regex.Split(charList, ("</Char>"));
                if (chars.Length > 0)
                    chars[0] = chars[0].Substring(chars[0].IndexOf("<Char id=\""), chars[0].Length - chars[0].IndexOf("<Char id=\""));

                for (int i = 0; i < chars.Length - 1; i++)
                {
                    CharacterStats st = new CharacterStats(chars[i]);
                    s.chars.Add(st);
                }
            }
            catch
            {
                LogEvent(new LogData(0, "EAM Service", LogEventType.ServiceError, $"Failed to parse CharList."));
            }
            return s;
        }

        private static string GetAccountStatsFilename(string email)
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

        public static AccountInfo GetAccountData(AccountInfo info, bool getName = true)
        {
            try
            {
                LogEvent(new LogData(0, "EAM Service", LogEventType.WebRequest, $"Sending \"account/verify\" for {info.email}."));
                string uniqueID = GetDeviceUniqueIdentifier();
                string webPath = string.Format("https://www.realmofthemadgod.com/account/verify?guid={0}&password={1}&clientToken={2}&game_net=Unity&play_platform=Unity&game_net_user_id=", WebUtility.UrlEncode(info.email), WebUtility.UrlEncode(info.password), uniqueID);
                string responseData = string.Empty;

                WebRequest request = WebRequest.Create(webPath);
                request.Credentials = CredentialCache.DefaultCredentials;
                WebResponse response = request.GetResponse();
                using (Stream dataStream = response.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(dataStream);
                    responseData = reader.ReadToEnd();
                }
                response.Close();
                if (getName && responseData.Contains("<Account>"))
                {
                    info.name = responseData.Substring(responseData.IndexOf("<Name>") + 6, responseData.IndexOf("</Name>") - 6 - responseData.IndexOf("<Name>"));
                }

                Tuple<string, string, string> tup = GetClientAccessData(responseData);

                info.accessToken = new AccessToken(tup.Item1, tup.Item2, tup.Item3, uniqueID);
                try
                {
                    string link = $"https://www.realmofthemadgod.com/char/list?do_login=true&accessToken={WebUtility.UrlEncode(info.accessToken.token)}&game_net=Unity&play_platform=Unity&game_net_user_id=";
                    request = WebRequest.Create(link);
                    request.Credentials = CredentialCache.DefaultCredentials;
                    response = request.GetResponse();
                    string charList = "";
                    using (Stream dataStream = response.GetResponseStream())
                    {
                        StreamReader reader = new StreamReader(dataStream);
                        charList = reader.ReadToEnd();
                    }
                    response.Close();
                    SaveAccountStats(info, responseData, charList);
                }
                catch
                {
                    LogEvent(new LogData(0, "EAM Service", LogEventType.ServiceError, $"Failed to get stats for {info.email}."));
                }
            }
            catch
            {
                LogEvent(new LogData(0, "EAM Service", LogEventType.ServiceError, $"Webrequest for {info.email} failed."));
            }
            return info;
        }

        private static void SaveAccounts()
        {
            try
            {
                AesCryptographyService acs = new AesCryptographyService();
                byte[] obj = ObjectToByteArray(accounts);
                byte[] data = acs.Encrypt(obj);

                File.WriteAllBytes(accountsPath, data);
                LogEvent(new LogData(0, "EAM Service", LogEventType.SaveAccounts, $"Saving accounts."));
            }
            catch
            {
                LogEvent(new LogData(0, "EAM Service", LogEventType.ServiceError, $"Failed to save accounts!"));
            }
        }

        private static void LoadEAMOptions()
        {
            if (File.Exists(optionsPath))
            {
                try
                {
                    OptionsData opt = (OptionsData)ByteArrayToObject(File.ReadAllBytes(optionsPath));
                    if (!string.IsNullOrWhiteSpace(opt.exePath))
                        exePath = opt.exePath;
                }
                catch (Exception ex)
                {
                    Log(ex.Message);
                    LogEvent(new LogData(0, "EAM Service", LogEventType.ServiceError, $"{ex.Message}"));
                }
            }
        }

        private static void LoadNotificationConfig()
        {
            try
            {
                if (File.Exists(pathNotificationConfig))
                    notOptions = (NotificationOptions)ByteArrayToObject(File.ReadAllBytes(pathNotificationConfig));
                else
                {
                    notOptions = new NotificationOptions();
                    File.WriteAllBytes(pathNotificationConfig, ObjectToByteArray(notOptions));
                }
            }
            catch (Exception ex)
            {
                Log(ex.Message);
                LogEvent(new LogData(0, "EAM Service", LogEventType.ServiceError, $"{ex.Message}"));
            }
        }


        private static void LoadAccounts()
        {
            if (File.Exists(accountsPath))
            {
                try
                {
                    byte[] data = File.ReadAllBytes(accountsPath);
                    AesCryptographyService acs = new AesCryptographyService();
                    accounts = (List<AccountInfo>)ByteArrayToObject(acs.Decrypt(data));
                }
                catch (Exception ex)
                {
                    Log(ex.Message);
                    LogEvent(new LogData(0, "EAM Service", LogEventType.ServiceError, $"{ex.Message}"));
                }
            }
        }

        private static void LoadLogins()
        {
            try
            {
                logins = (DailyLogins)ByteArrayToObject(File.ReadAllBytes(pathDailyLoginsConfig));
            }
            catch (Exception ex)
            {
                Log(ex.Message);
                LogEvent(new LogData(0, "EAM Service", LogEventType.ServiceError, $"{ex.Message}"));
            }
        }

        public static byte[] ObjectToByteArray(object obj)
        {
            if (obj == null)
                return null;

            BinaryFormatter bf = new BinaryFormatter();
            MemoryStream ms = new MemoryStream();
            bf.Serialize(ms, obj);

            return ms.ToArray();
        }

        public static object ByteArrayToObject(byte[] arrBytes)
        {
            MemoryStream memStream = new MemoryStream();
            BinaryFormatter binForm = new BinaryFormatter();
            memStream.Write(arrBytes, 0, arrBytes.Length);
            memStream.Seek(0, SeekOrigin.Begin);
            object obj = (object)binForm.Deserialize(memStream);

            return obj;
        }

        /// <summary>
        /// Kill Process of the tasktray tool on closing. (DEPRICATED)
        /// </summary>
        /// <param name="eventType"></param>
        /// <returns></returns>
        static bool ConsoleEventCallback(int eventType)
        {
            if (eventType == 2)
            {
                //foreach (var process in Process.GetProcessesByName("EAM Tasktray Tool"))
                //{
                //    try
                //    {
                //        process.Kill();
                //    }
                //    catch { }
                //}
            }
            return false;
        }
        static ConsoleEventDelegate handler;   // Keeps it from getting garbage collected
                                               // Pinvoke
        private delegate bool ConsoleEventDelegate(int eventType);
        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool SetConsoleCtrlHandler(ConsoleEventDelegate callback, bool add);

        [DllImport("kernel32.dll")]
        static extern IntPtr GetConsoleWindow();

        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        const int SW_HIDE = 0;
        const int SW_SHOW = 5;


        private static string StringToBase64String(string toEncode) => Convert.ToBase64String(StringToByteArray(toEncode));

        private static byte[] StringToByteArray(string toConvert)
        {
            List<byte> bytes = new List<byte>();

            foreach (char c in toConvert)
            {
                bytes.Add(Convert.ToByte(c));
            }
            return bytes.ToArray();
        }

        public static Tuple<string, string, string> GetClientAccessData(string resp)
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

        public static string GetDeviceUniqueIdentifier()
        {
            string ret = string.Empty;

            string concatStr = string.Empty;
            try
            {
                using (ManagementObjectSearcher searcherBb = new ManagementObjectSearcher("SELECT * FROM Win32_BaseBoard"))
                {
                    foreach (var obj in searcherBb.Get())
                    {
                        concatStr += (string)obj.Properties["SerialNumber"].Value ?? string.Empty;
                    }
                }
                using (ManagementObjectSearcher searcherBios = new ManagementObjectSearcher("SELECT * FROM Win32_BIOS"))
                {
                    foreach (var obj in searcherBios.Get())
                    {
                        concatStr += (string)obj.Properties["SerialNumber"].Value ?? string.Empty;
                    }
                }
                using (ManagementObjectSearcher searcherOs = new ManagementObjectSearcher("SELECT * FROM Win32_OperatingSystem"))
                {
                    foreach (var obj in searcherOs.Get())
                    {
                        concatStr += (string)obj.Properties["SerialNumber"].Value ?? string.Empty;
                    }
                }
                using (var sha1 = SHA1.Create())
                {
                    ret = string.Join("", sha1.ComputeHash(Encoding.UTF8.GetBytes(concatStr)).Select(b => b.ToString("x2")));
                }
            }
            catch (Exception e)
            {
                LogEvent(new LogData(0, "EAM Service", LogEventType.ServiceError, $"Failed to get the DeviceUniqueIdentifier!"));
            }

            return ret;
        }
    }
}
