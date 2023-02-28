using MK_EAM_Lib;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Management;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace EAM_Daily_Login_Service
{
    class DailyLogin
    {
        private static bool taskRunning = true;
        private static bool refreshRunning = false;

        #region Paths

        public static string saveFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "ExaltAccountManager");

        public static string optionsPath = Path.Combine(saveFilePath, "EAM.options");
        public static string accountsPath = Path.Combine(saveFilePath, "EAM.accounts");
        public static string accountStatsPath = Path.Combine(saveFilePath, "Stats");
        public static string pathLogs = Path.Combine(saveFilePath, "EAM.Logs");
        public static string dailyLoginsPath = Path.Combine(saveFilePath, "EAM.DailyLoginsV2");
        public static string lastUpdateCheckPath = Path.Combine(saveFilePath, "EAM.LastUpdateCheck");
        public static string notificationOptionsPath = Path.Combine(saveFilePath, "EAM.NotificationOptions");
        public static string itemsSaveFilePath = Path.Combine(saveFilePath, "EAM.ItemsSaveFile");
        public static string forceHWIDFilePath = Path.Combine(saveFilePath, "EAM.HWID");
        public static string pathEAMTasktrayToolEXE = Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).FullName, "Notification_Center", "EAM Notification Center.exe");

        #endregion

        private static DailyLogins dailyLogins = new DailyLogins();
        private static List<AccountInfo> accounts = new List<AccountInfo>();
        private static List<AccountInfo> accountsToRefresh = new List<AccountInfo>();

        private static NotificationOptions notOpt = null;

        private static GameUpdater gameUpdater;
        private static bool optionsFound = true;
        private static string exePath = string.Empty;

        public static System.Timers.Timer timer = new System.Timers.Timer();
        public static System.Timers.Timer terminateTimer = new System.Timers.Timer();
        public static System.Timers.Timer refreshAccountsTimer = new System.Timers.Timer();
        public static System.Timers.Timer refershCooldownTimer = new System.Timers.Timer();
        private static Dictionary<System.Timers.Timer, Process> dicTimerToProcess = new Dictionary<System.Timers.Timer, Process>();
        private static Dictionary<System.Timers.Timer, string> dicTimerToEmail = new Dictionary<System.Timers.Timer, string>();

        private static bool isForcedLogin = false;

        #region Show/Hide Window

        [DllImport("kernel32.dll")]
        static extern IntPtr GetConsoleWindow();

        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        const int SW_HIDE = 0;

        #endregion

        static void Main(string[] args)
        {
            var handle = GetConsoleWindow();
            // Hide
            ShowWindow(handle, SW_HIDE);

            LogEvent(new LogData("Daily Login Service", LogEventType.ServiceStart, $"Starting Daily Login Service."));

            if (IsAlreadyRunning())
                ExitApplication(new LogData("Daily Login Service", LogEventType.ServiceInfo, $"Daily Login Service is already running.."));

            #region DailyLogins

            if (File.Exists(dailyLoginsPath))
            {
                try
                {
                    using (StreamReader file = File.OpenText(dailyLoginsPath))
                    {
                        JsonSerializer serializer = new JsonSerializer();
                        dailyLogins = (DailyLogins)serializer.Deserialize(file, typeof(DailyLogins));
                    }

                    if (!(args != null && args[0].Equals("force")))
                    {                        
                        if (dailyLogins.DailyDatas.Count > 0)
                        {
                            DailyData lastDay = dailyLogins.DailyDatas.OrderByDescending(d => d.Date).FirstOrDefault();
                            if (lastDay != null && lastDay.Date.Date == DateTime.Now.Date)
                            {
                                ExitApplication(new LogData("Daily Login Service", LogEventType.ServiceDone, "Daily login already done."));
                                return;
                            }
                        }
                    }
                    else
                    {
                        isForcedLogin = true;
                        LogEvent(new LogData("Daily Login Service", LogEventType.ServiceInfo, "Daily login force started!"));
                    }
                }
                catch
                {
                    LogEvent(new LogData("Daily Login Service", LogEventType.ServiceError, "Failed to load daily logins data."));
                    dailyLogins = new DailyLogins();
                }
            }
            else
            {
                try
                {
                    dailyLogins = new DailyLogins();
                }
                catch (Exception)
                {
                    ExitApplication(new LogData("Daily Login Service", LogEventType.ServiceError, "Failed to create new dailyLogins."));
                }
            }

            #endregion

            #region Accounts

            if (File.Exists(accountsPath))
            {
                if (!LoadAccountInfos(accountsPath))
                    ExitApplication();
            }
            else
                ExitApplication(new LogData("Daily Login Service", LogEventType.ServiceError, "No accounts found!"));

            #endregion

            #region Options

            if (File.Exists(optionsPath))
            {
                try
                {
                    exePath = ((ExaltAccountManager.OptionsData)ByteArrayToObject(File.ReadAllBytes(optionsPath))).exePath;
                }
                catch
                {
                    exePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), @"RealmOfTheMadGod\Production\RotMG Exalt.exe");
                    LogEvent(new LogData("Daily Login Service", LogEventType.ServiceError, "Failed to load options, using Rotmg-default-path."));
                    optionsFound = false;
                }
            }
            else
            {
                exePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), @"RealmOfTheMadGod\Production\RotMG Exalt.exe");
                LogEvent(new LogData("Daily Login Service", LogEventType.ServiceError, "Failed to find options, using Rotmg-default-path."));
                optionsFound = false;
            }

            #endregion

            #region NotificationOptions

            if (File.Exists(notificationOptionsPath))
            {
                try
                {
                    notOpt = (NotificationOptions)ByteArrayToObject(File.ReadAllBytes(notificationOptionsPath));
                }
                catch
                {
                    notOpt = new NotificationOptions();
                }
            }
            else
            {
                try
                {
                    notOpt = new NotificationOptions();
                    File.WriteAllBytes(notificationOptionsPath, ObjectToByteArray(notOpt));
                }
                catch { LogEvent(new LogData("Daily Login Service", LogEventType.ServiceError, $"Failed to save notification options.")); }
            }

            #endregion

            terminateTimer.Interval = ((notOpt.joinTime + notOpt.killTime) * 1000) * 1.5f;
            terminateTimer.Elapsed += TerminateTimer_Elapsed;

            #region Check for Accounts to Refresh

            try
            {
                if (dailyLogins.RefreshAll)
                {
                    accountsToRefresh.AddRange(accounts.Where(a => a.performSave));

                    dailyLogins.RefreshAll = false;
                    SaveDailyLoginsData();
                }
                else
                    accountsToRefresh.AddRange(accounts.Where(a => string.IsNullOrEmpty(a.Name) && a.performSave));

                if (accountsToRefresh.Count > 0)
                {
                    refershCooldownTimer.Interval = 310000; //5minutes 10 seconds
                    refershCooldownTimer.Elapsed += RefershCooldownTimer_Elapsed;

                    refreshRunning = true;
                    refreshAccountsTimer.Interval = 3; //every 3 seconds || Too fast: 3, 3.5, 5, 7, 9 
                    refreshAccountsTimer.Elapsed += RefreshAccountsTimer_Elapsed;
                    refreshAccountsTimer.Start();
                }
            }
            catch { }

            #endregion

            CheckForGameUpdate();

            Console.ReadKey();
        }

        private static void RefershCooldownTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            refershCooldownTimer.Stop();

            if (taskRunning)            
                timer.Start();            
            if (refreshRunning)
                refreshAccountsTimer.Start();
        }

        private static void RefreshAccountsTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (refershCooldownTimer.Enabled)
                return;
            try
            {
                if (accountsToRefresh.Count > 0)
                {
                    lock (accounts)
                    {
                        LoadAccountInfos(accountsPath);
                        AccountInfo info = accounts.Where(a => a.Email.Equals(accountsToRefresh[0].Email)).FirstOrDefault();
                        if (info != null)
                        {
                            LogEvent(new LogData("Daily Login Service", LogEventType.ServiceInfo, $"Refreshing Account: {info.Email}."));
                            info.PerformWebrequest(null, LogEvent, "Daily Login Service", accountStatsPath, itemsSaveFilePath, GetDeviceUniqueIdentifier(), string.IsNullOrEmpty(info.Name), false);

                            if (info.requestState != MK_EAM_Lib.AccountInfo.RequestState.TooManyRequests)
                            {
                                accountsToRefresh.RemoveAt(0);
                                if (info.requestState == MK_EAM_Lib.AccountInfo.RequestState.Success)
                                    SaveAccounts();
                            }
                            else //5 Minutes waiting required
                            {
                                refreshAccountsTimer.Stop();
                                refershCooldownTimer.Start();
                            }

                        }
                        else
                        {
                            accountsToRefresh.RemoveAt(0);
                            RefreshAccountsTimer_Elapsed(sender, e);
                        }
                    }
                }
                else
                {
                    refreshRunning = false;
                    refreshAccountsTimer.Stop();
                }
            }
            catch
            {
                refreshRunning = false;
                refreshAccountsTimer.Stop();
                LogEvent(new LogData("Daily Login Service", LogEventType.ServiceError, $"Error occured while refreshing accounts. Did not refresh {accountsToRefresh.Count} accounts"));
            }

            if (!refreshRunning && !taskRunning && !terminateTimer.Enabled)
                terminateTimer.Start();
        }

        private static void CheckForGameUpdate()
        {
            gameUpdater = new GameUpdater(exePath, lastUpdateCheckPath);
            GameUpdater.Instance.OnUpdateRequired += UpdateCheckResults;
            GameUpdater.Instance.CheckForUpdate();
        }

        private static void UpdateCheckResults(object sender, EventArgs e)
        {
            GameUpdater.Instance.OnUpdateRequired -= UpdateCheckResults;
            if (GameUpdater.Instance.UpdateRequired)
            {
                if (optionsFound)
                {
                    GameUpdater.Instance.OnUpdateFinished += UpdateDone;
                    if (!GameUpdater.Instance.PerformGameUpdate())
                        ExitApplication(new LogData("Daily Login Service", LogEventType.ServiceError, "Update required but failed to update game."));
                }
                else
                {
                    ExitApplication(new LogData("Daily Login Service", LogEventType.ServiceError, "Update required but no options found, please set a game-path in the EAM and save the options."));
                }
                return;
            }

            StartLogins();
        }

        private static void UpdateDone(object sender, EventArgs e)
        {
            GameUpdater.Instance.OnUpdateFinished -= UpdateDone;
            StartLogins();
        }

        private static void StartLogins()
        {
            dailyLogins.DailyDatas.Add(new DailyData() { AccountData = new List<DailyAccountData>(), Date = DateTime.Now, ManualStart = isForcedLogin });
            SaveDailyLoginsData();

            if (notOpt.useTaskTrayTool)
            {
                try
                {
                    Process.Start(pathEAMTasktrayToolEXE);
                }
                catch
                {
                    LogEvent(new LogData("Daily Login Service", LogEventType.ServiceError, "Failed to start the Notification Center."));
                }
            }

            timer.Interval = notOpt.joinTime * 1000;
            timer.Elapsed += Timer_Tick;
            Timer_Tick(timer, null);
            timer.Start();
        }

        private static void Timer_Tick(object sender, ElapsedEventArgs e)
        {
            if (refershCooldownTimer.Enabled)
                return;
            try
            {
                lock (accounts)
                {
                    LoadAccountInfos(accountsPath);

                    AccountInfo accountInfo = null;
                    List<string> emails = dailyLogins.DailyDatas[dailyLogins.DailyDatas.Count - 1].AccountData.Select(x => x.Email).ToList();
                    accountInfo = accounts.Where(a => !a.performSave)
                                          .Where(ac => !emails.Contains(ac.Email))
                                          .FirstOrDefault();
                    if (accountInfo != null)
                    {
                        try
                        {
                            accountInfo.PerformWebrequest(null, LogEvent, "Daily Login Service", accountStatsPath, itemsSaveFilePath, GetDeviceUniqueIdentifier(), string.IsNullOrEmpty(accountInfo.Name), false);
                            accounts = accounts.OrderBy(a => a.orderID).ToList();
                            SaveAccounts();
                            if (accountInfo.requestState == AccountInfo.RequestState.TooManyRequests)
                            {
                                if (refershCooldownTimer.Enabled)
                                    return;
                                refershCooldownTimer.Start();
                                timer.Stop();
                                return;
                            }
                            LogEvent(new LogData("Daily Login Service", LogEventType.Login, $"Start login into account: {accountInfo.email}."));

                            string arguments = string.Format("\"data:{{platform:Deca,guid:{0},token:{1},tokenTimestamp:{2},tokenExpiration:{3},env:4,serverName:{4}}}\"",
                                           StringToBase64String(accountInfo.email), StringToBase64String(accountInfo.accessToken.token), StringToBase64String(accountInfo.accessToken.creationTime), StringToBase64String(accountInfo.accessToken.expirationTime), string.Empty);

                            ProcessStartInfo pinfo = new ProcessStartInfo();
                            pinfo.FileName = exePath;
                            pinfo.Arguments = string.Format("-batchmode {0}", arguments);
                            Process p = new Process();
                            p.StartInfo = pinfo;
                            p.Start();

                            SaveLoginStats(accountInfo);
                            dailyLogins.DailyDatas[dailyLogins.DailyDatas.Count - 1].AccountData.Add(new DailyAccountData() { Email = accountInfo.Email, StartTime = DateTime.Now });
                            SaveDailyLoginsData();

                            System.Timers.Timer timerP = new System.Timers.Timer();
                            timerP.Interval = (notOpt.joinTime + notOpt.killTime) * 1000;
                            timerP.Elapsed += TimerKill_Tick;
                            timerP.Start();

                            dicTimerToProcess.Add(timerP, p);
                            dicTimerToEmail.Add(timerP, accountInfo.Email);
                        }
                        catch
                        {
                            dailyLogins.DailyDatas[dailyLogins.DailyDatas.Count - 1].AccountData.Add(new DailyAccountData() { Email = accountInfo.Email, StartTime = DateTime.Now, EndTime = DateTime.Now, Success = false });
                            SaveDailyLoginsData();
                        }
                    }
                    else
                    { //Done
                        timer.Stop();
                        taskRunning = false;

                        if (!refreshRunning && dicTimerToProcess.Count == 0)
                            ExitApplication(new LogData("Daily Login Service", LogEventType.ServiceDone, $"Finished login into all accounts."));
                        else
                        {
                            if (!refreshRunning && !terminateTimer.Enabled)
                                terminateTimer.Start();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ExitApplication(new LogData("Daily Login Service", LogEventType.ServiceError, $"A critical error occured.{Environment.NewLine}{ex.StackTrace}"));
            }
        }

        private static void TerminateTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            Timer[] timers = dicTimerToProcess.Keys.ToArray();
            bool hadTimers = timers.Length > 0;
            foreach (Timer t in timers)
            {
                try
                {
                    TimerKill_Tick(t, null);
                }
                catch { }
            }

            ExitApplication(new LogData("Daily Login Service", LogEventType.ServiceDone, hadTimers ? $"Force-finished login into all accounts." : "Finished login into all accounts."));
        }

        private static void TimerKill_Tick(object sender, System.Timers.ElapsedEventArgs e)
        {
            bool success = e != null;
            try
            {
                System.Timers.Timer t = sender as System.Timers.Timer;
                if (dicTimerToProcess.ContainsKey(t))
                {
                    t.Stop();
                    DailyAccountData d = null;

                    if (dicTimerToEmail.ContainsKey(t))
                    {
                        d = dailyLogins.DailyDatas[dailyLogins.DailyDatas.Count - 1].AccountData.Where(a => a.Email.Equals(dicTimerToEmail[t])).FirstOrDefault();
                    }

                    if (!dicTimerToProcess[t].HasExited)
                    {
                        dicTimerToProcess[t].Kill();
                        dicTimerToProcess.Remove(t);

                        if (d != null)
                        {
                            d.EndTime = DateTime.Now;
                            d.Success = success;
                            SaveDailyLoginsData();
                        }
                    }
                    else
                    {
                        if (d != null)
                        {
                            d.EndTime = dicTimerToProcess[t].ExitTime;
                            d.Success = false;
                            SaveDailyLoginsData();
                        }
                    }
                    dicTimerToEmail.Remove(t);
                    dicTimerToProcess.Remove(t);
                }
            }
            catch { }
        }

        public static bool SaveAccounts()
        {
            try
            {
                AccountSaveFile saveFile = AccountSaveFile.Encrypt(new AccountSaveFile(), ObjectToByteArray(accounts.ToList() as List<MK_EAM_Lib.AccountInfo>));
                if (saveFile != null && string.IsNullOrEmpty(saveFile.error))
                {
                    File.WriteAllBytes(accountsPath, ObjectToByteArray(saveFile));
                    LogEvent(new LogData("Daily Login Service", LogEventType.SaveAccounts, $"Saving accounts."));
                }
                else
                {
                    LogEvent(new LogData("Daily Login Service", LogEventType.ServiceError, $"Failed to encrypt accounts!"));
                    return false;
                }
            }
            catch
            {
                LogEvent(new LogData("Daily Login Service", LogEventType.ServiceError, $"Failed to save accounts!"));
                return false;
            }
            return true;
        }

        private static void SaveDailyLoginsData()
        {
            try
            {
                dailyLogins.DailyDatas[dailyLogins.DailyDatas.Count - 1].PlannedLogins = accounts.Where(a => !a.performSave).Count();
                using (StreamWriter file = File.CreateText(dailyLoginsPath))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    serializer.Serialize(file, dailyLogins);
                }
            }
            catch
            {
                LogEvent(new LogData("Daily Login Service", LogEventType.ServiceError, $"Failed to save DailyLoginsData."));
            }
        }

        private static void SaveLoginStats(MK_EAM_Lib.AccountInfo info, bool sendCharList = false)
        {
            try
            {
                string fileName = System.IO.Path.Combine(accountStatsPath, GetAccountStatsFilename(info.email));

                if (!System.IO.Directory.Exists(accountStatsPath))
                    System.IO.Directory.CreateDirectory(accountStatsPath);

                if (System.IO.File.Exists(fileName))
                {
                    try
                    {
                        StatsMain stats = (StatsMain)ByteArrayToObject(System.IO.File.ReadAllBytes(fileName));
                        if (stats.logins == null)
                            stats.logins = new List<LoginStats>();
                        stats.logins.Add(new LoginStats() { time = DateTime.Now, isFromTask = true });
                        System.IO.File.WriteAllBytes(fileName, ObjectToByteArray(stats));
                    }
                    catch (Exception)
                    {
                        LogEvent(new LogData("Daily Login Service", LogEventType.ServiceError, $"Failed to load / save login-stats."));
                    }
                }
                else
                {
                    StatsMain stats = new StatsMain() { email = info.email };
                    stats.logins = new List<LoginStats>() { new LoginStats() { time = DateTime.Now, isFromTask = false } };
                    System.IO.File.WriteAllBytes(fileName, ObjectToByteArray(stats));
                }
            }
            catch
            {
                LogEvent(new LogData("Daily Login Service", LogEventType.ServiceError, $"Failed to save login-stats."));
            }
        }

        public static string GetAccountStatsFilename(string email)
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


        private static bool IsAlreadyRunning()
        {
            int ownProcessID = -1;
            try
            {
                ownProcessID = Process.GetCurrentProcess().Id;
            }
            catch { }

            foreach (var process in Process.GetProcesses())
            {
                try
                {
                    if (process.ProcessName == "EAM Daily Login Service" && process.Id != ownProcessID)
                    {
                        return true;
                    }
                }
                catch (System.ComponentModel.Win32Exception ex) when ((uint)ex.ErrorCode == 0x80004005)
                {
                    // Intentionally empty - no security access to the process.
                }
                catch (InvalidOperationException)
                {
                    // Intentionally empty - the process exited before getting details.
                }
                catch { }
            }
            return false;
        }

        private static string StringToBase64String(string toEncode) => Convert.ToBase64String(StringToByteArray(toEncode));

        private static byte[] StringToByteArray(string toConvert)
        {
            List<byte> bytes = new List<byte>();
            foreach (char c in toConvert)
                bytes.Add(Convert.ToByte(c));
            return bytes.ToArray();
        }

        private static void ExitApplication(LogData logEntry = null)
        {
            if (logEntry != null)
                LogEvent(logEntry);
            LogEvent(new LogData("Daily Login Service", LogEventType.ServiceEnd, "Daily Login Service is shutting down."));

            Environment.Exit(0);
        }

        private static bool LoadAccountInfos(string accountsPath)
        {
            try
            {
                AccountSaveFile saveFile = (AccountSaveFile)ByteArrayToObject(File.ReadAllBytes(accountsPath));
                accounts = AccountSaveFile.Decrypt(saveFile).OrderBy(a => a.orderID).ToList();
                return true;
            }
            catch
            {
                LogEvent(new LogData("Daily Login Service", LogEventType.ServiceError, "Failed to decrypt accounts."));
                return false;
            }
        }

        private static void LogEvent(LogData data)
        {

            if (File.Exists(pathLogs))
            {
                try
                {
                    List<LogData> logs = new List<LogData>();
                    logs = (List<LogData>)ByteArrayToObject(File.ReadAllBytes(pathLogs));
                    data.ID = logs.Count - 1;
                    //lastLogData = data;
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
                    //lastLogData = data;
                    logs.Add(data);
                    File.WriteAllBytes(pathLogs, ObjectToByteArray(logs));
                }
                catch { }
            }
        }

        public static byte[] ObjectToByteArray(object obj)
        {
            if (obj == null)
                return null;

            BinaryFormatter bf = new BinaryFormatter();
            using (MemoryStream ms = new MemoryStream())
            {
                bf.Serialize(ms, obj);
                return ms.ToArray();
            }
        }

        public static object ByteArrayToObject(byte[] arrBytes)
        {
            using (MemoryStream memStream = new MemoryStream())
            {
                BinaryFormatter binForm = new BinaryFormatter();
                memStream.Write(arrBytes, 0, arrBytes.Length);
                memStream.Seek(0, SeekOrigin.Begin);
                object obj = (object)binForm.Deserialize(memStream);

                return obj;
            }
        }

        public static string GetDeviceUniqueIdentifier(bool fileFailed = false)
        {
            string ret = string.Empty;

            if (!File.Exists(forceHWIDFilePath) || fileFailed)
            {
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
                    using (var sha1 = System.Security.Cryptography.SHA1.Create())
                    {
                        ret = string.Join("", sha1.ComputeHash(Encoding.UTF8.GetBytes(concatStr)).Select(b => b.ToString("x2")));
                    }
                }
                catch
                {
                    LogEvent(new LogData("Daily Login Service", LogEventType.ServiceError, $"Failed to get the DeviceUniqueIdentifier!"));
                }
            }
            else
            {
                try
                {
                    ret = File.ReadAllText(forceHWIDFilePath);
                }
                catch
                {
                    LogEvent(new LogData("Daily Login Service", LogEventType.ServiceError, $"Failed to load HWID from file, using alternative Methode!"));
                    return GetDeviceUniqueIdentifier(true);
                }
            }

            return ret;
        }
    }
}
